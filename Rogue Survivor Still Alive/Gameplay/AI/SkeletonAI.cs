﻿using System;
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
    /// SkeletonAI : used by Skeleton branch.
    /// </summary>
    class SkeletonAI : BaseAI
    {
        #region
        const int IDLE_CHANCE = 80;
        #endregion

        #region Fields
        LOSSensor m_LOSSensor;
        #endregion

        #region BaseAI
        protected override void CreateSensors()
        {
            m_LOSSensor = new LOSSensor(LOSSensor.SensingFilter.ACTORS);
        }

        protected override List<Percept> UpdateSensors(RogueGame game)
        {
            return m_LOSSensor.Sense(game, m_Actor);
        }

        protected override ActorAction SelectAction(RogueGame game, List<Percept> percepts)
        {
            List<Percept> mapPercepts = FilterSameMap(percepts); //@@MP - unused parameter (Release 5-7)

            ////////////////////////////////////////////
            // 1 move in straight line to nearest enemy
            // 2 idle? % chance.
            // 3 wander
            ////////////////////////////////////////////

            // 1 move in straight line to nearest enemy
            Percept nearestEnemy = FilterNearest(game, FilterEnemies(game, mapPercepts));
            if (nearestEnemy != null)
            {
                ActorAction bumpAction = BehaviorStupidBumpToward(game, nearestEnemy.Location.Position, true, false);
                if (bumpAction != null)
                {
                    m_Actor.Activity = Activity.CHASING;
                    m_Actor.TargetActor = nearestEnemy.Percepted as Actor;
                    return bumpAction;
                }
            }

            // 2 idle? % chance.
            if (game.Rules.RollChance(IDLE_CHANCE))
            {
                m_Actor.Activity = Activity.IDLE;
                return new ActionWait(m_Actor, game);
            }

            // 3 wander
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
