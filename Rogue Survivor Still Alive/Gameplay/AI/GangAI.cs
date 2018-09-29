﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;   // Point

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Engine.Actions;
using djack.RogueSurvivor.Engine.AI;
using djack.RogueSurvivor.Gameplay.AI.Sensors;

namespace djack.RogueSurvivor.Gameplay.AI
{
    [Serializable]
    /// <summary>
    /// Gang AI : Bikers, Gangstas...
    /// </summary>
    class GangAI : OrderableAI
    {
        #region Constants
        const int FOLLOW_NPCLEADER_MAXDIST = 1;
        const int FOLLOW_PLAYERLEADER_MAXDIST = 1;
        const int LOS_MEMORY = 10;

        const int EXPLORATION_LOCATIONS = 30;
        const int EXPLORATION_ZONES = 3;

        const int DONT_LEAVE_BEHIND_EMOTE_CHANCE = 50;

        static string[] FIGHT_EMOTES = 
        {
            "Fuck you",
            "Fuck it I'm trapped!",
            "Come on"
        };  
        #endregion

        #region Fields
        LOSSensor m_LOSSensor;
        MemorizedSensor m_MemorizedSensor;

        ExplorationData m_Exploration;
        #endregion

        #region BaseAI
        public override void TakeControl(Actor actor)
        {
            base.TakeControl(actor);

            m_Exploration = new ExplorationData(EXPLORATION_LOCATIONS, EXPLORATION_ZONES);
        }

        protected override void CreateSensors()
        {
            m_LOSSensor = new LOSSensor(LOSSensor.SensingFilter.ACTORS | LOSSensor.SensingFilter.ITEMS);
            m_MemorizedSensor = new MemorizedSensor(m_LOSSensor, LOS_MEMORY);
        }

        protected override List<Percept> UpdateSensors(RogueGame game)
        {
            return m_MemorizedSensor.Sense(game, m_Actor);
        }

        protected override ActorAction SelectAction(RogueGame game, List<Percept> percepts)
        {
            HashSet<Point> FOV = m_LOSSensor.FOV;
            List<Percept> mapPercepts = FilterSameMap(percepts); //@@MP - unused parameter (Release 5-7)

            // 1. Follow order
            #region
            if (this.Order != null)
            {
                ActorAction orderAction = ExecuteOrder(game, this.Order, mapPercepts);
                if (orderAction == null)
                    SetOrder(null);
                else
                {
                    m_Actor.Activity = Activity.FOLLOWING_ORDER;
                    return orderAction;
                }
            }
            #endregion

            //////////////////////////////////////////////////////////////////////
            // partial copy of Civilian AI 8) but always courageous and gets into fights.
            // BEHAVIOR
            // - FLAGS
            // "courageous" : always if not tired.
            // - RULES
            // 0 run away from primed explosives (and fires //@@MP (Release 5-2)).
            // 1 equip weapon/armor
            // 2 fire at nearest.
            // 3 shout/fight/flee.
            // 4 use medecine
            // 5 rest if tired
            // 6 charge enemy if courageous
            // 7 eat when hungry (also eat corpses)
            // 8 sleep.
            // 9 drop light/tracker with no batteries
            // 10 equip light/tracker
            // 11 get nearby item (not if seeing enemy)
            // 12 steal item from someone.
            // 13 tear down barricade
            // 14 follow leader
            // 15 take lead (if leadership)
            // 16 (leader) don't leave follower behind.
            // 17 explore
            // 18 wander
            //////////////////////////////////////////////////////////////////////

            // don't run by default.
            m_Actor.IsRunning = false;

            // get data.
            List<Percept> allEnemies = FilterEnemies(game, mapPercepts);
            List<Percept> currentEnemies = FilterCurrent(allEnemies); //@@MP - unused parameter (Release 5-7)
            bool hasCurrentEnemies = currentEnemies != null;
            bool hasAnyEnemies = allEnemies != null;
            bool checkOurLeader = m_Actor.HasLeader && !DontFollowLeader;
            //bool seeLeader = checkOurLeader && FOV.Contains(m_Actor.Leader.Location.Position); //@@MP - unused (Release 5-7)
            bool isLeaderFighting = checkOurLeader && IsAdjacentToEnemy(game, m_Actor.Leader);
            bool isCourageous = !game.Rules.IsActorTired(m_Actor);

            // exploration.
            m_Exploration.Update(m_Actor.Location);

            // 0 run away from primed explosives and fires //@@MP - added (Release 5-2)
            #region
            ActorAction runFromFires = BehaviorFleeFromFires(game, m_Actor.Location);
            if (runFromFires != null)
            {
                m_Actor.Activity = Activity.FLEEING;
                return runFromFires;
            }

            ActorAction runFromExplosives = BehaviorFleeFromExplosives(game, FilterStacks(mapPercepts)); //@@MP - unused parameter (Release 5-7)
            if (runFromExplosives != null)
            {
                m_Actor.Activity = Activity.FLEEING_FROM_EXPLOSIVE;
                return runFromExplosives;
            }
            #endregion

            // 1 equip weapon/armor
            #region
            ActorAction equipWpnAction = BehaviorEquipWeapon(game);
            if (equipWpnAction != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return equipWpnAction;
            }
            ActorAction equipArmAction = BehaviorEquipBodyArmor(game);
            if (equipArmAction != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return equipArmAction;
            }
            #endregion

            // 2 fire at nearest enemy (always if has leader, half of the time if not)
            #region
            if (hasCurrentEnemies && (checkOurLeader || game.Rules.RollChance(50)))
            {
                List<Percept> fireTargets = FilterFireTargets(game, currentEnemies);
                if (fireTargets != null)
                {
                    Percept nearestTarget = FilterNearest(game, fireTargets);
                    ActorAction fireAction = BehaviorRangedAttack(game, nearestTarget);
                    if (fireAction != null)
                    {
                        m_Actor.Activity = Activity.FIGHTING;
                        m_Actor.TargetActor = nearestTarget.Percepted as Actor;
                        return fireAction;
                    }
                }
            }
            #endregion

            // 3 shout/fight/flee
            #region
            //@@MP - try to wake nearby friends when there's a fire (Release 5-2)
            List<Percept> friends = FilterNonEnemies(game, mapPercepts);
            if (friends != null)
            {
                foreach (Point p in FOV)
                {
                    if (Map.IsAnyTileFireThere(m_Actor.Location.Map, p))
                    {
                        // shout
                        ActorAction shoutAction = BehaviorWarnFriendsOfFire(game, friends);
                        if (shoutAction != null)
                        {
                            m_Actor.Activity = Activity.FLEEING;
                            return shoutAction;
                        }
                    }
                }
            }

            //@@MP - the Enemies warning below is as was per of vanilla, except with friends moved above for reuse
            if (hasCurrentEnemies)
            {
                // shout?
                if (game.Rules.RollChance(50))
                {
                    if (friends != null)
                    {
                        ActorAction shoutAction = BehaviorWarnFriends(game, friends, FilterNearest(game, currentEnemies).Percepted as Actor);
                        if (shoutAction != null)
                        {
                            m_Actor.Activity = Activity.IDLE;
                            return shoutAction;
                        }
                    }
                }
                // fight or flee.
                ActorAction fightOrFlee = BehaviorFightOrFlee(game, currentEnemies, isLeaderFighting, ActorCourage.COURAGEOUS, FIGHT_EMOTES); //@@MP - unused parameter (Release 5-7)
                if (fightOrFlee != null)
                {
                    return fightOrFlee;
                }
            }
            #endregion

            // 4 use medicine
            #region
            ActorAction useMedAction = BehaviorUseMedecine(game, 2, 1, 2, 4, 2);
            if (useMedAction != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return useMedAction;
            }
            #endregion

            // 5 rest if tired
            #region
            ActorAction restAction = BehaviorRestIfTired(game);
            if (restAction != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return new ActionWait(m_Actor, game);
            }
            #endregion

            // 6 charge enemy if courageous
            #region
            if (hasCurrentEnemies && isCourageous)
            {
                Percept nearestEnemy = FilterNearest(game, currentEnemies);
                ActorAction chargeAction = BehaviorChargeEnemy(game, nearestEnemy);
                if (chargeAction != null)
                {
                    m_Actor.Activity = Activity.FIGHTING;
                    m_Actor.TargetActor = nearestEnemy.Percepted as Actor;
                    return chargeAction;
                }
            }
            #endregion

            // 7 eat when hungry (also eat corpses)
            #region
            if (game.Rules.IsActorHungry(m_Actor))
            {
                ActorAction eatAction = BehaviorEat(game);
                if (eatAction != null)
                {
                    m_Actor.Activity = Activity.IDLE;
                    return eatAction;
                }
                if (game.Rules.IsActorStarving(m_Actor) || game.Rules.IsActorInsane(m_Actor))
                {
                    eatAction = BehaviorGoEatCorpse(game, FilterCorpses(mapPercepts)); //@@MP - unused parameter (Release 5-7)
                    if (eatAction != null)
                    {
                        m_Actor.Activity = Activity.IDLE;
                        return eatAction;
                    }
                }
            }
            #endregion

            // 8 sleep.
            #region
            if (!hasAnyEnemies && WouldLikeToSleep(game, m_Actor) && IsInside(m_Actor) && game.Rules.CanActorSleep(m_Actor))
            {
                // secure sleep?
                ActorAction secureSleepAction = BehaviorSecurePerimeter(game, m_LOSSensor.FOV);
                if (secureSleepAction != null)
                {
                    m_Actor.Activity = Activity.IDLE;
                    return secureSleepAction;
                }

                // sleep.
                ActorAction sleepAction = BehaviorSleep(game, m_LOSSensor.FOV);
                if (sleepAction != null)
                {
                    if (sleepAction is ActionSleep)
                        m_Actor.Activity = Activity.SLEEPING;
                    return sleepAction;
                }
            }
            #endregion

            // 9 drop light/tracker with no batteries
            #region
            ActorAction dropOutOfBatteries = BehaviorDropUselessItem(game);
            if (dropOutOfBatteries != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return dropOutOfBatteries;
            }
            #endregion

            // 10 equip light/tracker
            #region
            // tracker : if has leader or is a leader.
            bool needCellPhone = checkOurLeader || m_Actor.CountFollowers > 0;
            // then light.
            bool needLight = NeedsLight(game);
            // if tracker or light useless, unequip it.
            if (!needCellPhone && !needLight)
            {
                ActorAction unequipUselessLeftItem = BehaviorUnequipLeftItem(game);
                if (unequipUselessLeftItem != null)
                {
                    m_Actor.Activity = Activity.IDLE;
                    return unequipUselessLeftItem;
                }
            }
            // tracker?
            if (needCellPhone)
            {
                ActorAction eqTrackerAction = BehaviorEquipCellPhone(game);
                if (eqTrackerAction != null)
                {
                    m_Actor.Activity = Activity.IDLE;
                    return eqTrackerAction;
                }
            }
            // ...or light?
            else if (needLight)
            {
                ActorAction eqLightAction = BehaviorEquipLight(game);
                if (eqLightAction != null)
                {
                    m_Actor.Activity = Activity.IDLE;
                    return eqLightAction;
                }

            }
            #endregion

            // 11 get nearby item (not if seeing enemy)
            // ignore not currently visible items & blocked items.
            #region
            if (!hasCurrentEnemies)
            {
                Map map = m_Actor.Location.Map;
                List<Percept> stacks = FilterOut(FilterStacks(mapPercepts), //@@MP - unused parameter (Release 5-7)
                    (p) => (p.Turn != map.LocalTime.TurnCounter) || IsOccupiedByOther(map, p.Location.Position));
                if (stacks != null)
                {
                    Percept nearestStack = FilterNearest(game, stacks);
                    ActorAction grabAction = BehaviorGrabFromStack(game, nearestStack.Location.Position, nearestStack.Percepted as Inventory);
                    if (grabAction != null)
                    {
                        m_Actor.Activity = Activity.IDLE;
                        return grabAction;
                    }
                }
            }
            #endregion

            // 12 steal item from someone.
            #region
            if (!hasCurrentEnemies)
            {
                //Map map = m_Actor.Location.Map; //@@MP - unused (Release 5-7)
                List<Percept> mayStealFrom = FilterActors(FilterCurrent(mapPercepts), //@@MP - unused parameter (Release 5-7)
                    (a) =>
                    {
                        if (a.Inventory == null || a.Inventory.CountItems == 0 || IsFriendOf(game, a))
                            return false;
                        if (game.Rules.RollChance(game.Rules.ActorUnsuspicousChance(m_Actor, a)))
                        {
                            // emote.
                            game.DoEmote(a, String.Format("moves unnoticed by {0}.", m_Actor.Name));
                            // unnoticed.
                            return false;
                        }
                        return HasAnyInterestingItem(game, a.Inventory);
                    });
                if (mayStealFrom != null)
                {
                    // get data.
                    Percept nearest = FilterNearest(game, mayStealFrom);
                    Actor victim = nearest.Percepted as Actor;
                    Item wantIt = FirstInterestingItem(game, victim.Inventory);

                    // make an enemy of him.
                    game.DoMakeAggression(m_Actor, victim);

                    // declare my evil intentions.
                    m_Actor.Activity = Activity.CHASING;
                    m_Actor.TargetActor = victim;
                    return new ActionSay(m_Actor, game, victim, 
                        String.Format("Hey! That's some nice {0} you have here!", wantIt.Model.SingleName), RogueGame.Sayflags.IS_IMPORTANT);
                }
            }
            #endregion

            // 13 tear down barricade
            #region
            ActorAction attackBarricadeAction = BehaviorAttackBarricade(game);
            if (attackBarricadeAction != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return attackBarricadeAction;
            }
            #endregion

            // 14 follow leader
            #region
            if (checkOurLeader)
            {
                Point lastKnownLeaderPosition = m_Actor.Leader.Location.Position;
                bool isLeaderVisible = FOV.Contains(m_Actor.Leader.Location.Position);
                int maxDist = m_Actor.Leader.IsPlayer ? FOLLOW_PLAYERLEADER_MAXDIST : FOLLOW_NPCLEADER_MAXDIST;
                ActorAction followAction = BehaviorFollowActor(game, m_Actor.Leader, lastKnownLeaderPosition, isLeaderVisible, maxDist);
                if (followAction != null)
                {
                    m_Actor.Activity = Activity.FOLLOWING;
                    m_Actor.TargetActor = m_Actor.Leader;
                    return followAction;
                }
            }
            #endregion

            // 15 take lead (if leadership)
            #region
            bool isLeader = m_Actor.Sheet.SkillTable.GetSkillLevel((int)Skills.IDs.LEADERSHIP) >= 1;
            bool canLead = !checkOurLeader && isLeader && m_Actor.CountFollowers < game.Rules.ActorMaxFollowers(m_Actor);
            if (canLead)
            {
                Percept nearestFriend = FilterNearest(game, FilterNonEnemies(game, mapPercepts));
                if (nearestFriend != null)
                {
                    ActorAction leadAction = BehaviorLeadActor(game, nearestFriend);
                    if (leadAction != null)
                    {
                        m_Actor.Activity = Activity.IDLE;
                        m_Actor.TargetActor = nearestFriend.Percepted as Actor;
                        return leadAction;
                    }
                }
            }
            #endregion

            // 16 (leader) don't leave followers behind.
            #region
            if (m_Actor.CountFollowers > 0)
            {
                Actor target;
                ActorAction stickTogether = BehaviorDontLeaveFollowersBehind(game, 3, out target);
                if (stickTogether != null)
                {
                    // emote?
                    if (game.Rules.RollChance(DONT_LEAVE_BEHIND_EMOTE_CHANCE))
                    {
                        if (target.IsSleeping)
                            game.DoEmote(m_Actor, String.Format("patiently waits for {0} to wake up.", target.Name));
                        else
                        {
                            if (m_LOSSensor.FOV.Contains(target.Location.Position))
                                game.DoEmote(m_Actor, String.Format("Hey {0}! Fucking move!", target.Name));
                            else
                                game.DoEmote(m_Actor, String.Format("Where is that {0} retard?", target.Name));
                        }
                    }

                    // go!
                    m_Actor.Activity = Activity.IDLE;
                    return stickTogether;
                }
            }
            #endregion

            // 17 explore
            #region
            ActorAction exploreAction = BehaviorExplore(game, m_Exploration);
            if (exploreAction != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return exploreAction;
            }
            #endregion

            // 18 wander
            m_Actor.Activity = Activity.IDLE;
            return BehaviorWander(game);
        }
        #endregion
    }
}
