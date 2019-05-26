using System;
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
    /// Soldier AI
    /// </summary>
    class SoldierAI : OrderableAI
    {
        #region Constants
        const int LOS_MEMORY = 10;
        const int FOLLOW_LEADER_MIN_DIST = 1;
        const int FOLLOW_LEADER_MAX_DIST = 2;

        const int EXPLORATION_LOCATIONS = 30;
        const int EXPLORATION_ZONES = 3;

        const int BUILD_SMALL_FORT_CHANCE = 20;
        const int BUILD_LARGE_FORT_CHANCE = 50;
        const int START_FORT_LINE_CHANCE = 1;

        const int DONT_LEAVE_BEHIND_EMOTE_CHANCE = 50;

        static string[] FIGHT_EMOTES = 
        {
            "Falling back",
            "Fuck I'm cornered",
            "Die"
        };  
        #endregion

        #region Fields
        LOSSensor m_LOSSensor;
        MemorizedSensor m_MemLOSSensor;

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
            m_MemLOSSensor = new MemorizedSensor(m_LOSSensor, LOS_MEMORY);
        }

        protected override List<Percept> UpdateSensors(RogueGame game)
        {
            return m_MemLOSSensor.Sense(game, m_Actor);
        }

        protected override ActorAction SelectAction(RogueGame game, List<Percept> percepts)
        {
            List<Percept> mapPercepts = FilterSameMap(percepts); //@@MP - unused parameter (Release 5-7)
            m_Actor.IsRunning = false; // don't run by default. // alpha10
            //@@MP (Release 6-5)
            ActorAction determinedAction = null;
            Map map = m_Actor.Location.Map;

            // 0. Equip best item
            ActorAction bestEquip = BehaviorEquipBestItems(game, false, true);
            if (bestEquip != null)
            {
                m_Actor.Activity = Activity.MANAGING_INVENTORY;
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

            /////////////////////////////////////
            // 0.1 run away from primed explosives (and fires //@@MP (Release 5-2))
            // 0.2 if underground in total darkness, find nearest exit //@@MP (Release 6-5)
            // 1 throw grenades at enemies.
            // alpha10 OBSOLETE 2 equip weapon/armor.
            // 3 shout, fire/hit at nearest enemy.
            // 4 rest if tired
            // alpha10 obsolete and redundant with rule 3! 5 charge enemy.
            // 6 use med.
            // 7 sleep.
            // 8 chase old enemy.
            // 9 build fortification.
            // 10 hang around leader.            
            // 11 (leader) don't leave followers behind.
            // 12 explore.
            // 13 wander.
            ////////////////////////////////////

            // get data.
            List<Percept> allEnemies = FilterEnemies(game, mapPercepts);
            List<Percept> currentEnemies = FilterCurrent(allEnemies); //@@MP - unused parameter (Release 5-7)
            bool checkOurLeader = m_Actor.HasLeader && !DontFollowLeader;
            bool hasCurrentEnemies = (currentEnemies != null);
            bool hasAnyEnemies = (allEnemies != null);

            // exploration.
            m_Exploration.Update(m_Actor.Location);

            // 0.1 run away from primed explosives //@@MP - and fires added (Release 5-2)
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

            // 0.2 if in total darkness //@@MP - added (Release 6-5)
            #region
            int fov = game.Rules.ActorFOV(m_Actor, map.LocalTime, game.Session.World.Weather);
            if (fov <= 0) //can't see anything, too dark
            {
                if (!game.Rules.CanActorSeeSky(m_Actor)) //if underground find nearest exit
                {
                    //if already on exit, leave
                    determinedAction = BehaviorUseExit(game, UseExitFlags.ATTACK_BLOCKING_ENEMIES);
                    if (determinedAction != null)
                    {
                        m_Actor.Activity = Activity.FINDING_EXIT;
                        return determinedAction;
                    }
                    else
                    {
                        //find the nearest exit
                        determinedAction = BehaviorGoToNearestAIExit(game);
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

            // 1 throw grenades at enemies.
            #region
            if (hasCurrentEnemies)
            {
                ActorAction throwAction = BehaviorThrowGrenade(game, m_LOSSensor.FOV, currentEnemies);
                if (throwAction != null)
                {
                    m_Actor.Activity = Activity.FIGHTING;
                    return throwAction;
                }
            }
            #endregion

            // 2 equip weapon/armor //alpha 10 obsolete
            #region
            /*ActorAction equipWpnAction = BehaviorEquipWeapon(game);
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
            }*/
            #endregion

            // 3 shout, fire/hit at nearest enemy.
            #region
            //@@MP - try to wake nearby friends when there's a fire (Release 5-2)
            List<Percept> friends = FilterNonEnemies(game, mapPercepts);
            if (friends != null)
            {
                HashSet<Point> FOV = m_LOSSensor.FOV;
                foreach (Point p in FOV)
                {
                    if (map.IsAnyTileFireThere(m_Actor.Location.Map, p))
                    {
                        // shout
                        ActorAction shoutAction = BehaviorWarnFriendsOfFire(game, friends);
                        if (shoutAction != null)
                        {
                            m_Actor.Activity = Activity.SHOUTING;
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
                            m_Actor.Activity = Activity.SHOUTING;
                            return shoutAction;
                        }
                    }
                }

                // fire?
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

                // fight or flee?
                RouteFinder.SpecialActions allowedChargeActions = RouteFinder.SpecialActions.JUMP | RouteFinder.SpecialActions.DOORS; // alpha10
                ActorAction fightOrFlee = BehaviorFightOrFlee(game, currentEnemies, true, ActorCourage.COURAGEOUS, FIGHT_EMOTES, allowedChargeActions); //@@MP - unused parameter (Release 5-7), alpha01 added allowedChargeActions
                if (fightOrFlee != null)
                {
                    return fightOrFlee;
                }
            }
            #endregion

            // 4 rest if tired
            #region
            ActorAction restAction = BehaviorRestIfTired(game);
            if (restAction != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return restAction;
            }
            #endregion

            // 5 charge enemy //alpha10 obsolete
            #region
            /*if (hasAnyEnemies)
            {
                Percept nearestEnemy = FilterNearest(game, allEnemies);
                ActorAction chargeAction = BehaviorChargeEnemy(game, nearestEnemy);
                if (chargeAction != null)
                {
                    m_Actor.Activity = Activity.FIGHTING;
                    m_Actor.TargetActor = nearestEnemy.Percepted as Actor;
                    return chargeAction;
                }
            }*/
            #endregion

            // 6 use medicine
            #region
            ActorAction useMedAction = BehaviorUseMedicine(game, 2, 1, 2, 4, 2);
            if (useMedAction != null)
            {
                m_Actor.Activity = Activity.HEALING;
                return useMedAction;
            }
            #endregion

            // 7 sleep.
            #region
            if ( !hasAnyEnemies && WouldLikeToSleep(game, m_Actor) && IsInside(m_Actor) && game.Rules.CanActorSleep(m_Actor))
            {               
                // secure sleep?
                ActorAction secureSleepAction = BehaviorSecurePerimeter(game, m_LOSSensor.FOV);
                if (secureSleepAction != null)
                {
                    m_Actor.Activity = Activity.PATROLLING;
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

            // 8 chase old enemy
            #region
            List<Percept> oldEnemies = Filter(allEnemies, (p) => p.Turn != m_Actor.Location.Map.LocalTime.TurnCounter); //@@MP - unused parameter (Release 5-7)
            if (oldEnemies != null)
            {
                Percept chasePercept = FilterNearest(game, oldEnemies);

                // cheat a bit for good chasing behavior.
                if (m_Actor.Location == chasePercept.Location)
                {
                    // memorized location reached, chase now the actor directly (cheat so they appear more intelligent)
                    Actor chasedActor = chasePercept.Percepted as Actor;
                    chasePercept = new Percept(chasedActor, m_Actor.Location.Map.LocalTime.TurnCounter, chasedActor.Location);
                }

                // chase.
                ActorAction chargeAction = BehaviorChargeEnemy(game, chasePercept, false, false);
                if (chargeAction != null)
                {
                    m_Actor.Activity = Activity.FIGHTING;
                    m_Actor.TargetActor = chasePercept.Percepted as Actor;
                    return chargeAction;
                }
            }
            #endregion

            // 9 build fortification
            #region
            // large fortification.
            if (game.Rules.RollChance(BUILD_LARGE_FORT_CHANCE))
            {
                ActorAction buildAction = BehaviorBuildLargeFortification(game, START_FORT_LINE_CHANCE);
                if (buildAction != null)
                {
                    m_Actor.Activity = Activity.BUILDING;
                    return buildAction;
                }
            }
            // small fortification.
            if (game.Rules.RollChance(BUILD_SMALL_FORT_CHANCE))
            {
                ActorAction buildAction = BehaviorBuildSmallFortification(game);
                if (buildAction != null)
                {
                    m_Actor.Activity = Activity.BUILDING;
                    return buildAction;
                }
            }
            #endregion

            // 10 hang around leader.
            #region
            if (checkOurLeader)
            {
                Point lastKnownLeaderPosition = m_Actor.Leader.Location.Position;
                ActorAction followAction = BehaviorHangAroundActor(game, m_Actor.Leader, lastKnownLeaderPosition, FOLLOW_LEADER_MIN_DIST, FOLLOW_LEADER_MAX_DIST);
                if (followAction != null)
                {
                    m_Actor.Activity = Activity.FOLLOWING;
                    m_Actor.TargetActor = m_Actor.Leader;
                    return followAction;
                }
            }
            #endregion

            // 11 (leader) don't leave followers behind.
            #region
            if (m_Actor.CountFollowers > 0)
            {
                Actor target;
                ActorAction stickTogether = BehaviorDontLeaveFollowersBehind(game, 4, out target);
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
                                game.DoEmote(m_Actor, String.Format("{0}! Don't lag behind!", target.Name));
                            else
                                game.DoEmote(m_Actor, String.Format("Where the hell is {0}?", target.Name));
                        }
                    }

                    // go!
                    m_Actor.Activity = Activity.IDLE;
                    return stickTogether;
                }
            }
            #endregion

            // 12 explore
            #region
            ActorAction exploreAction = BehaviorExplore(game, m_Exploration);
            if (exploreAction != null)
            {
                m_Actor.Activity = Activity.EXPLORING;
                return exploreAction;
            }
            #endregion

            // 13 wander
            #region
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
            #endregion
        }
        #endregion
    }
}
