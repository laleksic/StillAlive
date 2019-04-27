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
    /// Rat AI : used by Zombie Rat branch.
    /// </summary>
    class RatAI : BaseAI
    {
        #region Fields
        LOSSensor m_LOSSensor;
        SmellSensor m_LivingSmellSensor;
        #endregion

        #region BaseAI
        protected override void CreateSensors()
        {
            m_LOSSensor = new LOSSensor(LOSSensor.SensingFilter.ACTORS | LOSSensor.SensingFilter.CORPSES);
            m_LivingSmellSensor = new SmellSensor(Odor.LIVING);
        }

        protected override List<Percept> UpdateSensors(RogueGame game)
        {
            List<Percept> list = m_LOSSensor.Sense(game, m_Actor);
            list.AddRange(m_LivingSmellSensor.Sense(game, m_Actor));
            return list;
        }

        protected override ActorAction SelectAction(RogueGame game, List<Percept> percepts)
        {
            //HashSet<Point> fov = m_LOSSensor.FOV; //@@MP - unused (Release 5-7)
            List<Percept> mapPercepts = FilterSameMap(percepts); //@@MP - unused parameter (Release 5-7)

            //////////////////////////////////////////////////////////////
            // 1 move closer to an enemy, nearest & visible enemies first
            // 2 eat corpses
            // 2 move to highest living scent
            // 3 wander
            //////////////////////////////////////////////////////////////

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
                            ActorAction bumpAction = BehaviorStupidBumpToward(game, enemyP.Location.Position, false, false);
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

                // then try the rest, the closer the best.
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
                            ActorAction bumpAction = BehaviorStupidBumpToward(game, enemyP.Location.Position, false, false);
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
                ActorAction eatCorpses = BehaviorGoEatCorpse(game, corpses);
                if (eatCorpses != null)
                {
                    m_Actor.Activity = Activity.IDLE;
                    return eatCorpses;
                }
            }

            // 3 move to highest living scent
            #region
            ActorAction trackLivingAction = BehaviorTrackScent(game, m_LivingSmellSensor.Scents);
            if (trackLivingAction != null)
            {
                m_Actor.Activity = Activity.TRACKING;
                return trackLivingAction;
            }

            #endregion

            // 4 wander
            m_Actor.Activity = Activity.IDLE;
            ActorAction determinedAction = BehaviorWander(game);
            if (determinedAction != null)
                return determinedAction;
            else
                return new ActionWait(m_Actor, game); //@@MP (Release 6-5)
        }
        #endregion
    }
}
