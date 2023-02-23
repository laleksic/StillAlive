using System;
using System.Collections.Generic;

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Engine.Actions;
using djack.RogueSurvivor.Engine.AI;
using djack.RogueSurvivor.Gameplay.AI.Sensors;

namespace djack.RogueSurvivor.Gameplay.AI
{
    [Serializable]
    /// <summary>
    /// Zombie AI : used by Zombie, Zombie Master and Zombified branches.
    /// </summary>
    class ZombieAI : BaseAI
    {
        #region Constants
        const int LOS_MEMORY = 20;

        const int EXPLORATION_LOCATIONS = 30;
        const int EXPLORATION_ZONES = 3;

        const int USE_EXIT_CHANCE = 50;
        const int FOLLOW_SCENT_THROUGH_EXIT_CHANCE = 90;
        const int PUSH_OBJECT_CHANCE = 20;
        #endregion

        #region Fields
        MemorizedSensor m_MemLOSSensor;
        SmellSensor m_LivingSmellSensor;
        SmellSensor m_MasterSmellSensor;

        ExplorationData m_Exploration;
        #endregion

        #region BaseAI
        public override void TakeControl(Actor actor)
        {
            base.TakeControl(actor);

            if (m_Actor.Model.Abilities.ZombieAI_Explore)
                m_Exploration = new ExplorationData(EXPLORATION_LOCATIONS, EXPLORATION_ZONES);
        }

        protected override void CreateSensors()
        {
            m_MemLOSSensor = new MemorizedSensor(new LOSSensor(LOSSensor.SensingFilter.ACTORS | LOSSensor.SensingFilter.CORPSES), LOS_MEMORY);
            m_LivingSmellSensor = new SmellSensor(Odor.LIVING);
            m_MasterSmellSensor = new SmellSensor(Odor.UNDEAD_MASTER);
        }

        protected override List<Percept> UpdateSensors(RogueGame game)
        {
            List<Percept> list = m_MemLOSSensor.Sense(game, m_Actor);
            list.AddRange(m_LivingSmellSensor.Sense(game, m_Actor));
            list.AddRange(m_MasterSmellSensor.Sense(game, m_Actor));
            return list;
        }

        protected override ActorAction SelectAction(RogueGame game, List<Percept> percepts)
        {
            //HashSet<Point> fov = (m_MemLOSSensor.Sensor as LOSSensor).FOV; //@@MP - unused (Release 5-7)
            List<Percept> mapPercepts = FilterSameMap(percepts); //@@MP - unused parameter (Release 5-7)

            //////////////////////////////////////////////////////////////
            // 0 run away from primed explosives (and fires //@@MP (Release 5-2)).
            // 1 move closer to an enemy, nearest & visible enemies first
            // 2 eat corpses.
            // 3 use exits (if ability)
            // 4 move close to nearest undead master (if not master)
            // 5 move to highest adjacent undead master scent (if not master)
            // 6 move to highest living scent
            // 7 **DISABLED** assault breakables (if ability)
            // 8 randomly push objects around (if ability OR skill STRONG)
            // 9 explore (if ability)
            // 10 wander
            //////////////////////////////////////////////////////////////

            // get data.
            if (m_Actor.Model.Abilities.ZombieAI_Explore)
            {
                // exploration.
                m_Exploration.Update(m_Actor.Location);
            }

            // 0 run away from primed explosives and fires //@@MP - added (Release 5-2)
            #region
            if (m_Actor.Model.Abilities.IsIntelligent)
            {
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
            }
            #endregion

            // 1 move closer to an enemy, nearest & visible enemies first
            #region
            List<Percept> enemies = FilterEnemies(game, mapPercepts);
            if (enemies != null)
            {
                // try visible enemies first, the closer the best.
                List<Percept> visibleEnemies = Filter(enemies, (p) => p.Turn == m_Actor.Location.Map.LocalTime.TurnCounter); //@@MP - unused parameter (Release 5-7)
                if (visibleEnemies != null)
                {
                    Percept bestEnemyPercept = null;
                    ActorAction bestBumpAction = null;
                    float closest = int.MaxValue;

                    foreach (Percept enemyP in visibleEnemies)
                    {
                        float distance = game.Rules.GridDistance(m_Actor.Location.Position, enemyP.Location.Position);
                        if (distance < closest)
                        {
                            ActorAction bumpAction = BehaviorStupidBumpToward(game, enemyP.Location.Position, true, true);
                            if (bumpAction != null)
                            {
                                closest = distance;
                                bestEnemyPercept = enemyP;
                                bestBumpAction = bumpAction;
                            }
                        }
                    }

                    if (bestBumpAction != null)
                    {
                        m_Actor.Activity = Activity.CHASING;
                        m_Actor.TargetActor = bestEnemyPercept.Percepted as Actor;
                        return bestBumpAction;
                    }
                }

                // then try rest, the closer the best.
                List<Percept> oldEnemies = Filter(enemies, (p) => p.Turn != m_Actor.Location.Map.LocalTime.TurnCounter); //@@MP - unused parameter (Release 5-7)
                if (oldEnemies != null)
                {
                    Percept bestEnemyPercept = null;
                    ActorAction bestBumpAction = null;
                    float closest = int.MaxValue;

                    foreach (Percept enemyP in oldEnemies)
                    {
                        float distance = game.Rules.GridDistance(m_Actor.Location.Position, enemyP.Location.Position);
                        if (distance < closest)
                        {
                            ActorAction bumpAction = BehaviorStupidBumpToward(game, enemyP.Location.Position, true, true);
                            if (bumpAction != null)
                            {
                                closest = distance;
                                bestEnemyPercept = enemyP;
                                bestBumpAction = bumpAction;
                            }
                        }
                    }

                    if (bestBumpAction != null)
                    {
                        m_Actor.Activity = Activity.CHASING;
                        m_Actor.TargetActor = bestEnemyPercept.Percepted as Actor;
                        return bestBumpAction;
                    }
                }
            }
            #endregion

            // 2 eat corpses.
            List<Percept> corpses = FilterCorpses(mapPercepts); //@@MP - unused parameter (Release 5-7)
            if (corpses != null)
            {
                ActorAction eatCorpses = BehaviorGoEatCorpse(game, corpses, out int x, out int y);
                if (eatCorpses != null)
                {
                    m_Actor.Activity = Activity.EATING;
                    return eatCorpses;
                }
            }

            // 3 use exit (if ability)
            #region
            // move before following scents so the AI is more likely to move into basements etc...
            if (m_Actor.Model.Abilities.AI_CanUseAIExits && game.Rules.RollChance(USE_EXIT_CHANCE))
            {
                ActorAction useExit = BehaviorUseExit(game, UseExitFlags.ATTACK_BLOCKING_ENEMIES | UseExitFlags.BREAK_BLOCKING_OBJECTS | UseExitFlags.DONT_BACKTRACK);
                if (useExit != null)
                {
                    // memory is obsolete, clear it.
                    m_MemLOSSensor.Clear();
                    m_Actor.Activity = Activity.FINDING_EXIT;
                    return useExit;
                }
            }
            #endregion

            // 4 move close to nearest undead master (if not master)
            #region
            if (!m_Actor.Model.Abilities.IsUndeadMaster)
            {
                Percept nearestMaster = FilterNearest(game, FilterActors(mapPercepts, (a) => a.Model.Abilities.IsUndeadMaster)); //@@MP - unused parameter (Release 5-7)
                if (nearestMaster != null)
                {
                    Map map = m_Actor.Location.Map; //@@MP (Release 6-1)
                    ActorAction bumpAction = BehaviorStupidBumpToward(game, map.RandomPositionNear(game.Rules, m_Actor.Location.Map, nearestMaster.Location.Position, 3), true, true);
                    if (bumpAction != null)
                    {
                        // MAASTEERRR!
                        m_Actor.Activity = Activity.FOLLOWING;
                        m_Actor.TargetActor = nearestMaster.Percepted as Actor;
                        return bumpAction;
                    }
                }
            }
            #endregion

            // 5 move to highest undead master scent (if not master)
            #region
            if (!m_Actor.Model.Abilities.IsUndeadMaster)
            {
                ActorAction trackMasterAction = BehaviorTrackScent(game, m_MasterSmellSensor.Scents);
                if (trackMasterAction != null)
                {
                    m_Actor.Activity = Activity.TRACKING;
                    return trackMasterAction;
                }
            }
            #endregion

            // 6 move to highest living scent
            #region
            ActorAction trackLivingAction = BehaviorTrackScent(game, m_LivingSmellSensor.Scents);
            if (trackLivingAction != null)
            {
                m_Actor.Activity = Activity.TRACKING;
                return trackLivingAction;
            }
            #endregion

            // 7 **DISABLED** assault breakables (if ability)
#if false
            #region
            if (m_Actor.Model.Abilities.ZombieAI_AssaultBreakables)
            {
                ActorAction assaultAction = BehaviorAssaultBreakables(game, fov);
                if (assaultAction != null)
                {
                    m_Actor.Activity = Activity.DESTROYING;
                    return assaultAction;
                }
            }
            #endregion
#endif

            // 8 randomly push objects around (if ability OR skill STRONG)
            #region
            if (game.Rules.HasActorPushAbility(m_Actor) && game.Rules.RollChance(PUSH_OBJECT_CHANCE))
            {
#if false 
                special ZM case disabled
                // zm push any pushable to open up a path for other zombies.
                // non-zm push only blocking objects.
                ActorAction pushAction = m_Actor.Model.Abilities.IsUndeadMaster ? BehaviorPushAnyObject(game) : BehaviorPushBlockingObject(game);
#else
                ActorAction pushAction = BehaviorPushNonWalkableObject(game);
#endif
                if (pushAction != null)
                {
                    m_Actor.Activity = Activity.DESTROYING;
                    return pushAction;
                }
            }
            #endregion

            // 9 explore (if ability)
            #region
            if (m_Actor.Model.Abilities.ZombieAI_Explore)
            {
                ActorAction exploreAction = BehaviorExplore(game, m_Exploration);
                if (exploreAction != null)
                {
                    m_Actor.Activity = Activity.EXPLORING;
                    return exploreAction;
                }
            }
            #endregion

            // 9 wander
            ActorAction determinedAction = BehaviorWander(game);
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
