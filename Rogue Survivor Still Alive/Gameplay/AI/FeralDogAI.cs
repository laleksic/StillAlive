using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Engine.Actions;
using djack.RogueSurvivor.Engine.AI;
using djack.RogueSurvivor.Gameplay.AI.Sensors;


namespace djack.RogueSurvivor.Gameplay.AI
{
    [Serializable]
    class FeralDogAI : BaseAI
    {
        #region Constants
        const int FOLLOW_NPCLEADER_MAXDIST = 1;
        const int FOLLOW_PLAYERLEADER_MAXDIST = 1;


        static string[] FIGHT_EMOTES = 
        {
            "waf",            // flee
            "waf!?",         // trapped
            "GRRRRR WAF WAF"  // fight
        };   
        #endregion

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
            // 0 run away from fires or alert sleeping friends //@@MP (Release 5-2)).
            // 1 defend our leader.
            // 2 attack or flee enemies.
            // 3 go eat food on floor if almost hungry
            // 4 go eat corpses if hungry
            // 5 sleep
            // 6 follow leader
            // 7 wander
            /////////////////////////////////////////////////////////////

            // 0 run away from fires or alert sleeping friends //@@MP (Release 5-2)
            #region
            ActorAction runFromFires = BehaviorFleeFromFires(game, m_Actor.Location);
            if (runFromFires != null)
            {
                m_Actor.Activity = Activity.FLEEING;
                return runFromFires;
            }
            
            //@@MP - try to wake nearby friends when there's a fire (Release 5-2)
            List<Percept> friends = FilterNonEnemies(game, mapPercepts);
            if (friends != null)
            {
                HashSet<Point> FOV = m_LOSSensor.FOV;
                foreach (Point p in FOV)
                {
                    if (Map.IsAnyTileFireThere(m_Actor.Location.Map, p))
                    {
                        // shout
                        ActorAction shoutAction = BehaviorWarnFriendsOfFire(game, friends, "WOOF! WOOF! WOOF!");
                        if (shoutAction != null)
                        {
                            m_Actor.Activity = Activity.FLEEING;
                            return shoutAction;
                        }
                    }
                }
            }
            #endregion

            // 1 defend our leader
            #region
            if (m_Actor.HasLeader)
            {
                Actor target = m_Actor.Leader.TargetActor;
                if(target != null && target.Location.Map == m_Actor.Location.Map)
                {
                    // emote: bark
                    game.DoSay(m_Actor, target, "GRRRRRR!", RogueGame.Sayflags.IS_FREE_ACTION);
                    // charge.
                    ActorAction chargeEnemy = BehaviorStupidBumpToward(game, target.Location.Position);
                    if (chargeEnemy != null)
                    {
                        m_Actor.IsRunning = true; // always run!
                        m_Actor.Activity = Activity.FIGHTING;
                        m_Actor.TargetActor = target;
                        return chargeEnemy;
                    }
                }
            }
            #endregion

            List<Percept> enemies = FilterEnemies(game, mapPercepts);
            bool isLeaderVisible = m_Actor.HasLeader && m_LOSSensor.FOV.Contains(m_Actor.Leader.Location.Position);
            bool isLeaderFighting = m_Actor.HasLeader && IsAdjacentToEnemy(game, m_Actor.Leader);

            // 2 attack or flee enemies.
            #region
            if (enemies != null)
            {
                ActorAction ff = BehaviorFightOrFlee(game, enemies, isLeaderFighting, Directives.Courage, FIGHT_EMOTES); //@@MP - unused parameter (Release 5-7)
                if (ff != null)
                {
                    m_Actor.IsRunning = true; // always run!
                    return ff;
                }
            }
            #endregion

            // 3 go eat food on floor if almost hungry
            #region
            if (game.IsAlmostHungry(m_Actor))
            {
                List<Percept> itemsStack = FilterStacks(mapPercepts); //@@MP - unused parameter (Release 5-7)
                if (itemsStack != null)
                {
                    ActorAction eatFood = BehaviorGoEatFoodOnGround(game, itemsStack);
                    if (eatFood != null)
                    {
                        m_Actor.IsRunning = true; // always run!
                        m_Actor.Activity = Activity.IDLE;
                        return eatFood;
                    }
                }
            }
            #endregion

            // 4 go eat corpses if hungry
            #region
            if (game.Rules.IsActorHungry(m_Actor))
            {
                List<Percept> corpses = FilterCorpses(mapPercepts); //@@MP - unused parameter (Release 5-7)
                if (corpses != null)
                {
                    ActorAction eatCorpses = BehaviorGoEatCorpse(game, corpses);
                    if (eatCorpses != null)
                    {
                        m_Actor.IsRunning = true; // always run!
                        m_Actor.Activity = Activity.IDLE;
                        return eatCorpses;
                    }
                }
            }
            #endregion

            // 5 sleep
            #region
            if (game.Rules.IsActorSleepy(m_Actor))
            {
                m_Actor.Activity = Activity.SLEEPING;
                return new ActionSleep(m_Actor, game);
            }
            #endregion

            // 6 follow leader
            #region
            if (m_Actor.HasLeader)
            {
                Point lastKnownLeaderPosition = m_Actor.Leader.Location.Position;
                int maxDist = m_Actor.Leader.IsPlayer ? FOLLOW_PLAYERLEADER_MAXDIST : FOLLOW_NPCLEADER_MAXDIST;
                ActorAction followAction = BehaviorFollowActor(game, m_Actor.Leader, lastKnownLeaderPosition, isLeaderVisible, maxDist);
                if (followAction != null)
                {
                    m_Actor.IsRunning = true; // always run!
                    m_Actor.Activity = Activity.FOLLOWING;
                    m_Actor.TargetActor = m_Actor.Leader;
                    return followAction;
                }
            }
            #endregion

            // 7 wander
            m_Actor.Activity = Activity.IDLE;
            return BehaviorWander(game);
        }
        #endregion
    }
}
