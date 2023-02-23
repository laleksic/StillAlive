﻿using System;
using System.Collections.Generic;
using System.Drawing;   // Point

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Engine.Actions;
using djack.RogueSurvivor.Engine.AI;
using djack.RogueSurvivor.Gameplay.AI.Sensors;
using djack.RogueSurvivor.Gameplay.AI.Tools; //alpha 10

namespace djack.RogueSurvivor.Gameplay.AI
{
    [Serializable]
    /// <summary>
    /// CHAR Guard AI.
    /// </summary>
    class CHARGuardAI : OrderableAI
    {
        #region Constants
        const int LOS_MEMORY = 10;

        static string[] FIGHT_EMOTES = 
        {
            "Get away from me",
            "Damn it I'm trapped!",
            "Hey"
        };                              
        #endregion

        #region Fields
        LOSSensor m_LOSSensor;
        MemorizedSensor m_MemorizedSensor;
        #endregion

        #region BaseAI
        protected override void CreateSensors()
        {
            m_LOSSensor = new LOSSensor(LOSSensor.SensingFilter.ACTORS | LOSSensor.SensingFilter.ITEMS);
            m_MemorizedSensor = new MemorizedSensor(m_LOSSensor, LOS_MEMORY);
        }

        public override void TakeControl(Actor actor)
        {
            base.TakeControl(actor);
        }

        protected override List<Percept> UpdateSensors(RogueGame game)
        {
            return m_MemorizedSensor.Sense(game, m_Actor);
        }

        protected override ActorAction SelectAction(RogueGame game, List<Percept> percepts)
        {
            List<Percept> mapPercepts = FilterSameMap(percepts); //@@MP - unused parameter (Release 5-7)
            m_Actor.IsRunning = false; // don't run by default. // alpha10
            //@@MP (Release 6-5)
            ActorAction determinedAction = null;
            Map map = m_Actor.Location.Map;

            // 0. Equip best item
            ActorAction bestEquip = BehaviorEquipBestItems(game, true, true);
            if (bestEquip != null)
            {
                return bestEquip;
            }
            // end alpha10

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

            ///////////////////////////////////////
            // 0.1 run away from primed explosives (and fires //@@MP (Release 5-2))
            // 0.2 try to extinguish oneself if on fire.     //@@MP (Release 7-6)
            // 0.3 if underground in total darkness, find nearest exit.    //@@MP (Release 6-5)
            // alpha10 OBSOLETE 1 equip weapon
            // alpha10 OBSOLETE 2 equip armor
            // 3 fire at nearest enemy.
            // 4 hit adjacent enemy.
            // 5 warn trepassers.
            // 6 shout
            // 7 rest if tired
            // 8 charge enemy
            // 9 sleep when sleepy.
            // 10 follow leader.
            // 11 wander in CHAR office.
            // 12 wander.
            //////////////////////////////////////

            // don't run by default.
            m_Actor.IsRunning = false;

            // get data.
            List<Percept> allEnemies = FilterEnemies(game, mapPercepts);
            List<Percept> currentEnemies = FilterCurrent(allEnemies); //@@MP - unused parameter (Release 5-7)
            bool checkOurLeader = m_Actor.HasLeader && !DontFollowLeader;
            bool hasAnyEnemies = allEnemies != null;

            // 0.1 run away from primed explosives and fires //@@MP - added (Release 5-2)
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

            // 0.2 try to extinguish oneself if on fire.     //@@MP (Release 7-6)
            #region
            if (m_Actor.IsOnFire) //stop-drop-and-roll
            {
                m_Actor.Activity = Activity.FLEEING;
                return new ActionWait(m_Actor, game);
            }
            #endregion

            // 0.3 if in total darkness    //@@MP - added (Release 6-5)
            #region
            int fov = game.Rules.ActorFOV(m_Actor, map.LocalTime, game.Session.World.Weather);
            if (fov <= 0) //can't see anything, too dark
            {
                if (!game.Rules.CanActorSeeSky(m_Actor)) //if underground find nearest exit
                {
                    //if already on exit, leave
                    determinedAction = BehaviorUseExit(game, UseExitFlags.ATTACK_BLOCKING_ENEMIES | UseExitFlags.DONT_BACKTRACK);
                    if (determinedAction != null)
                    {
                        m_Actor.Activity = Activity.FINDING_EXIT;
                        return determinedAction;
                    }
                    else
                    {
                        //find the nearest exit
                        determinedAction = BehaviorGoToNearestAIExit(game,20,false);
                        if (determinedAction != null)
                        {
                            m_Actor.Activity = Activity.FINDING_EXIT;
                            return determinedAction;
                        }
                        else if (IsAdjacentToEnemy(game, m_Actor)) //can't get to an exit. use self-defense if we're trapped by an enemy
                        {
                            Point pt = m_Actor.Location.Position;
                            int adjacentEnemies = 0;
                            foreach (Direction d in Direction.COMPASS_LIST)
                            {
                                Point p = pt + d;
                                if (map.IsInBounds(p) && map.IsWalkable(p))
                                {
                                    Actor a = map.GetActorAt(p);
                                    if (a == null)
                                        continue;
                                    else if (game.Rules.AreEnemies(m_Actor, a))
                                    {
                                        ++adjacentEnemies;

                                        // emote, trapped
                                        if (m_Actor.Model.Abilities.CanTalk)
                                            game.DoEmote(m_Actor, FIGHT_EMOTES[1], true);

                                        //lash out at the first adjacent enemy each time so that we focus on it, hopefully killing it and making a gap to run through
                                        determinedAction = BehaviorMeleeAttack(game, a);
                                        if (determinedAction != null)
                                        {
                                            m_Actor.Activity = Activity.FIGHTING;
                                            return determinedAction;
                                        }
                                    }
                                }
                            }

                            if (adjacentEnemies == 0) //if it's fallen through to here it's because the actor is trapped by friendly actors / map objects that can't be pushed, jumped or broken / walls
                            {
                                if (m_Actor.Model.Abilities.CanTalk)
                                    game.DoEmote(m_Actor, FIGHT_EMOTES[1], true);

                                Logger.WriteLine(Logger.Stage.RUN_MAIN, m_Actor.Name + " seems to be stuck in the dark... [district: " + m_Actor.Location.Map.District.Name + "] [coords: " + pt.ToString() + "] [turn #" + game.Session.WorldTime.TurnCounter + "]");
                            }
                        }
                    }
                }
            }
            #endregion

            #region alpha 10 OBSOLETE
            //// 1 equip weapon
            //ActorAction equipWpnAction = BehaviorEquipWeapon(game);
            //if (equipWpnAction != null)
            //{
            //    m_Actor.Activity = Activity.IDLE;
            //    return equipWpnAction;
            //}

            //// 2 equip armor
            //ActorAction equipArmAction = BehaviorEquipBestBodyArmor(game);
            //if (equipArmAction != null)
            //{
            //    m_Actor.Activity = Activity.IDLE;
            //    return equipArmAction;
            //}
            #endregion

            // 3 fire at nearest enemy.
            #region
            if (currentEnemies != null)
            {
                List<Percept> fireTargets = FilterFireTargets(game, currentEnemies);
                if (fireTargets != null)
                {
                    Percept nearestTarget = FilterNearest(game, fireTargets);
                    Actor targetActor = nearestTarget.Percepted as Actor;

                    ActorAction fireAction = BehaviorRangedAttack(game, nearestTarget);
                    if (fireAction != null)
                    {
                        m_Actor.Activity = Activity.FIGHTING;
                        m_Actor.TargetActor = targetActor;
                        return fireAction;
                    }
                }
            }
            #endregion

            // 4 hit adjacent enemy
            #region
            if (currentEnemies != null)
            {
                //@@MP - unused (Release 5-7)
                //Percept nearestEnemy = FilterNearest(game, currentEnemies);
                //Actor targetActor = nearestEnemy.Percepted as Actor;

                // fight or flee?
                RouteFinder.SpecialActions allowedChargeActions = RouteFinder.SpecialActions.JUMP | RouteFinder.SpecialActions.DOORS; // alpha10
                ActorAction fightOrFlee = BehaviorFightOrFlee(game, currentEnemies, true, ActorCourage.COURAGEOUS, FIGHT_EMOTES, allowedChargeActions); //@@MP - unused parameter (Release 5-7), alpha 10 added allowedChargeActions
                if (fightOrFlee != null)
                {
                    return fightOrFlee;
                }
            }
            #endregion

            // 5 warn trepassers.
            #region
            List<Percept> nonEnemies = FilterNonEnemies(game, mapPercepts);
            if (nonEnemies != null)
            {
                List<Percept> trespassers = Filter(nonEnemies, (p) => //@@MP - unused parameter (Release 5-7)
                {
                    Actor other = (p.Percepted as Actor);
                    if (other.Faction == game.GameFactions.TheCHARCorporation)
                        return false;

                    // alpha10 bug fix only if visible right now!
                    if (p.Turn != m_Actor.Location.Map.LocalTime.TurnCounter)
                        return false;

                    return game.IsInCHARProperty(other.Location);
                });
                if (trespassers != null)
                {
                    // Hey YOU!
                    Actor trespasser = FilterNearest(game, trespassers).Percepted as Actor;

                    game.DoMakeAggression(m_Actor, trespasser);

                    m_Actor.Activity = Activity.FIGHTING;
                    m_Actor.TargetActor = trespasser;
                    return new ActionSay(m_Actor, game, trespasser, "Hey YOU!", RogueGame.Sayflags.IS_IMPORTANT | RogueGame.Sayflags.IS_DANGER);
                }
            }
            #endregion

            // 6 shout
            #region
            //@@MP - try to wake nearby friends when there's a fire (Release 5-2)
            if (nonEnemies != null)
            {
                HashSet<Point> FOV = m_LOSSensor.FOV;
                foreach (Point p in FOV)
                {
                    if (map.IsAnyTileFireThere(m_Actor.Location.Map, p))
                    {
                        // shout
                        ActorAction shoutAction = BehaviorWarnFriendsOfFire(game, nonEnemies);
                        if (shoutAction != null)
                        {
                            m_Actor.Activity = Activity.SHOUTING;
                            return shoutAction;
                        }
                    }
                }
            }

            if (hasAnyEnemies)
            {
                if (nonEnemies != null)
                {
                    ActorAction shoutAction = BehaviorWarnFriends(game, nonEnemies, FilterNearest(game, allEnemies).Percepted as Actor);
                    if (shoutAction != null)
                    {
                        m_Actor.Activity = Activity.SHOUTING;
                        return shoutAction;
                    }
                }
            }
            #endregion

            // 7 rest if tired
            #region
            ActorAction restAction = BehaviorRestIfTired(game);
            if (restAction != null)
            {
                m_Actor.Activity = Activity.RESTING;
                return new ActionWait(m_Actor, game);
            }
            #endregion

            // 8 charge/chase enemy
            #region
            if (allEnemies != null)
            {
                Percept chasePercept = FilterNearest(game, allEnemies);

                // cheat a bit for good chasing behavior.
                if (m_Actor.Location == chasePercept.Location)
                {
                    // memorized location reached, chase now the actor directly (cheat so they appear more intelligent)
                    Actor chasedActor = chasePercept.Percepted as Actor;
                    chasePercept = new Percept(chasedActor, m_Actor.Location.Map.LocalTime.TurnCounter, chasedActor.Location);
                }

                // alpha10 chase only if reachable
                if (CanReachSimple(game, chasePercept.Location.Position, RouteFinder.SpecialActions.DOORS | RouteFinder.SpecialActions.JUMP))
                {
                    // chase.
                    ActorAction chargeAction = BehaviorChargeEnemy(game, chasePercept, false, false);
                    if (chargeAction != null)
                    {
                        m_Actor.Activity = Activity.CHASING;
                        m_Actor.TargetActor = chasePercept.Percepted as Actor;
                        return chargeAction;
                    }
                }
            }
            #endregion

            // 9 sleep when sleepy
            #region
            if (game.Rules.IsActorSleepy(m_Actor) && !hasAnyEnemies)
            {
                ActorAction sleepAction = BehaviorSleep(game, m_LOSSensor.FOV);
                if (sleepAction != null)
                {
                    if (sleepAction is ActionSleep)
                        m_Actor.Activity = Activity.SLEEPING;
                    return sleepAction;
                }
            }
            #endregion

            // 10 follow leader
            #region
            if (checkOurLeader)
            {
                Point lastKnownLeaderPosition = m_Actor.Leader.Location.Position;
                bool isLeaderVisible = m_LOSSensor.FOV.Contains(m_Actor.Leader.Location.Position);
                ActorAction followAction = BehaviorFollowActor(game, m_Actor.Leader, lastKnownLeaderPosition, isLeaderVisible, 1);
                if (followAction != null)
                {
                    m_Actor.Activity = Activity.FOLLOWING;
                    m_Actor.TargetActor = m_Actor.Leader;
                    return followAction;
                }
            }
            #endregion

            // 11 wander in CHAR office.
            #region
            ActorAction wanderInOfficeAction = BehaviorWander(game, (loc) => RogueGame.IsInCHAROffice(loc));
            if (wanderInOfficeAction != null)
            {
                m_Actor.Activity = Activity.WANDERING;
                return wanderInOfficeAction;
            }
            #endregion

            // 12 wander
            determinedAction = BehaviorWander(game);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.WANDERING;
                return determinedAction;
            }
            else
            {
                m_Actor.Activity = Activity.WAITING;
                return new ActionWait(m_Actor, game); //@@MP (Release 6-5)
            }
        }
        #endregion
    }
}
