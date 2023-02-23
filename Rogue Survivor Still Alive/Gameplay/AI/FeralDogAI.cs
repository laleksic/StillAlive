using System;
using System.Collections.Generic;
using System.Drawing;

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Engine.Actions;
using djack.RogueSurvivor.Engine.AI;
using djack.RogueSurvivor.Gameplay.AI.Sensors;
using djack.RogueSurvivor.Gameplay.AI.Tools;

namespace djack.RogueSurvivor.Gameplay.AI
{
    [Serializable]
    class FeralDogAI : BaseAI
    {
        #region Constants
        const int FOLLOW_NPCLEADER_MAXDIST = 1;
        const int FOLLOW_PLAYERLEADER_MAXDIST = 1;
        const int RUN_TO_TARGET_DISTANCE = 3;  // dogs run to their target when close enough

        public static string[] FIGHT_EMOTES = 
        {
            "yeep yeep yeep",            // flee
            "woof!?",         // trapped
            "GRRRRR!!"  // fight
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
            //HashSet<Point> fov = m_LOSSensor.FOV;  //unused so far
            List<Percept> mapPercepts = FilterSameMap(percepts);
            RouteFinder.SpecialActions allowedChargeActions = RouteFinder.SpecialActions.JUMP; // alpha10

            //////////////////////////////////////////////////////////////
            // 1 defend our leader.
            // 2 attack or flee enemies.
            // 3 go eat food on floor if almost hungry
            // 4 go eat corpses if hungry
            // 5 attack survivors if starving?
            // 6 rest or sleep
            // 7 follow leader
            // 8 take lead of other dogs
            // 9 follow survivors if not hungry?
            // 10 wander
            /////////////////////////////////////////////////////////////

            List<Percept> enemies = FilterEnemies(game, mapPercepts);
            bool isLeaderVisible = m_Actor.HasLeader && m_LOSSensor.FOV.Contains(m_Actor.Leader.Location.Position);
            bool isLeaderFighting = m_Actor.HasLeader && IsAdjacentToEnemy(game, m_Actor.Leader);

            // 1 defend our leader
            // 2 attack or flee enemies.
            #region
            bool defendLeader = false;
            if (m_Actor.HasLeader)
            {
                Actor target = m_Actor.Leader.TargetActor;
                if (target != null && target.Location.Map == m_Actor.Location.Map)
                {
                    defendLeader = true;
                    // emote: bark
                    game.DoSay(m_Actor, target, "GRRRRRR WAF WAF", RogueGame.Sayflags.IS_FREE_ACTION | RogueGame.Sayflags.IS_DANGER);
                }
            }

            if (defendLeader || enemies != null)
            {
                // charge or run away?
                ActorAction ff = BehaviorAnimalFightOrFlee(game, enemies, isLeaderVisible, isLeaderFighting, Directives.Courage, FIGHT_EMOTES, allowedChargeActions, out int dirptX, out int dirptY);
                if (ff != null)
                {
                    FaceSpriteForDirection(dirptX, dirptY); //@@MP (Release 7-3)
                    // run to (or away if fleeing) if close.
                    if (m_Actor.TargetActor != null)
                        RunToIfCloseTo(game, m_Actor.TargetActor.Location.Position, RUN_TO_TARGET_DISTANCE);
                    return ff;
                }
            }
            #endregion

            // 3 go eat food on floor if almost hungry
            #region
            if (game.IsAlmostHungry(m_Actor))
            {
                List<Percept> itemsStack = FilterStacks(mapPercepts);
                if (itemsStack != null)
                {
                    ActorAction eatFood = BehaviorGoEatFoodOnGround(game, itemsStack, out int foodPointX, out int foodPointY);
                    if (eatFood != null)
                    {
                        FaceSpriteForDirection(foodPointX, foodPointY); //@@MP (Release 7-3)
                        RunIfPossible(game.Rules);
                        m_Actor.Activity = Activity.EATING;
                        return eatFood;
                    }
                }
            }
            #endregion

            // 4 go eat corpses if hungry
            #region
            if (game.Rules.IsActorHungry(m_Actor))
            {
                List<Percept> corpses = FilterCorpses(mapPercepts);
                if (corpses != null)
                {
                    ActorAction eatCorpses = BehaviorGoEatCorpse(game, corpses, out int foodPointX, out int foodPointY);
                    if (eatCorpses != null)
                    {
                        FaceSpriteForDirection(foodPointX, foodPointY); //@@MP (Release 7-3)
                        RunIfPossible(game.Rules);
                        m_Actor.Activity = Activity.EATING;
                        return eatCorpses;
                    }
                }
            }
            #endregion

            // 5 attack survivors if starving?
            #region
            if (game.Rules.IsActorStarving(m_Actor) && RogueGame.Options.IsAggressiveHungryCiviliansOn)
            {
                List<Percept> survivors = FilterSurvivors(game, mapPercepts);
                if (survivors != null)
                {
                    Actor target = FilterNearest(game, survivors).Percepted as Actor;
                    if (target != null && target.Location.Map == m_Actor.Location.Map)
                    {
                        //make enemies
                        m_Actor.TargetActor = target;
                        m_Actor.MarkAsAgressorOf(target);
                        target.MarkAsSelfDefenceFrom(m_Actor);
                        FaceSpriteForDirection(target.Location.Position.X, target.Location.Position.Y);
                        // emote: bark
                        game.DoSay(m_Actor, target, "GRRRRRR WAF WAF", RogueGame.Sayflags.IS_FREE_ACTION | RogueGame.Sayflags.IS_DANGER);
                        // charge.
                        Percept chargePercept = new Percept(target, m_Actor.Location.Map.LocalTime.TurnCounter, target.Location);
                        ActorAction chargeEnemy = BehaviorChargeEnemy(game, chargePercept, false, false);
                        if (chargeEnemy != null)
                        {
                            RunToIfCloseTo(game, target.Location.Position, RUN_TO_TARGET_DISTANCE);
                            m_Actor.Activity = Activity.FIGHTING;
                            m_Actor.TargetActor = target;
                            return chargeEnemy;
                        }
                    }
                }
            }
            #endregion

            // 6 rest or sleep
            #region
            if (game.Rules.IsActorTired(m_Actor))
            {
                m_Actor.Activity = Activity.RESTING;
                return new ActionWait(m_Actor, game);
            }
            if (game.Rules.IsActorSleepy(m_Actor) && game.Rules.CanActorSleep(m_Actor))
            {
                m_Actor.Activity = Activity.SLEEPING;
                return new ActionSleep(m_Actor, game);
            }
            #endregion

            // 7 follow leader
            #region
            if (m_Actor.HasLeader) //follow leader
            {
                Point lastKnownLeaderPosition = m_Actor.Leader.Location.Position;
                int maxDist = m_Actor.Leader.IsPlayer ? FOLLOW_PLAYERLEADER_MAXDIST : FOLLOW_NPCLEADER_MAXDIST;
                ActorAction followAction = BehaviorFollowActor(game, m_Actor.Leader, lastKnownLeaderPosition, isLeaderVisible, maxDist);
                if (followAction != null)
                {
                    FaceSpriteForDirection(lastKnownLeaderPosition.X, lastKnownLeaderPosition.Y); //@@MP (Release 7-3)
                    m_Actor.IsRunning = false;
                    m_Actor.Activity = Activity.FOLLOWING;
                    m_Actor.TargetActor = m_Actor.Leader;
                    return followAction;
                }
            }
            #endregion
            // 8 take lead of or fight other dogs
            #region
            else //not following
            {
                ActorDirective meDirectives = (m_Actor.Controller as AIController).Directives;
                if (meDirectives != null && meDirectives.Courage == ActorCourage.COURAGEOUS)
                {
                    //take lead of other dogs if courageous (an alpha)
                    Percept nearestFriend = FilterNearest(game, FilterNonEnemyDogs(game, mapPercepts));
                    if (nearestFriend != null)
                    {
                        ActorAction leadAction = BehaviorLeadActor(game, nearestFriend);
                        if (leadAction != null)
                        {
                            FaceSpriteForDirection(nearestFriend.Location.Position.X, nearestFriend.Location.Position.Y);
                            m_Actor.Activity = Activity.CHATTING;
                            m_Actor.TargetActor = nearestFriend.Percepted as Actor;
                            return leadAction;
                        }
                    }

                    //fight courageous (alpha) dogs
                    List<Percept> alphaDogs = FilterEnemyAlphaDogs(game, mapPercepts);
                    if (alphaDogs != null)
                    {
                        ActorAction ff = BehaviorAnimalFightOrFlee(game, alphaDogs, isLeaderVisible, isLeaderFighting, Directives.Courage, FIGHT_EMOTES, allowedChargeActions, out int dirptX, out int dirptY);
                        if (ff != null)
                        {
                            FaceSpriteForDirection(dirptX, dirptY); //@@MP (Release 7-3)
                            // run to (or away if fleeing) if close.
                            if (m_Actor.TargetActor != null)
                                RunToIfCloseTo(game, m_Actor.TargetActor.Location.Position, RUN_TO_TARGET_DISTANCE);
                            return ff;
                        }
                    }
                }
            }
            #endregion

            // 9 if not hungry, follow survivors?
            #region
            if (!m_Actor.HasLeader)
            {
                List<Percept> survivors = FilterSurvivors(game, mapPercepts);
                if (survivors != null)
                {
                    Actor target = FilterNearest(game, survivors).Percepted as Actor;
                    if (target != null && target.Location.Map == m_Actor.Location.Map)
                    {
                        //if (target.IsPlayer || (target.Controller as AIController).Directives.Courage == ActorCourage.COURAGEOUS)  //nah
                        if (target.Sheet.SkillTable.GetSkillLevel((int)Skills.IDs.CHARISMATIC) > 0)
                        {
                            ActorAction followAction = BehaviorFollowActor(game, target, target.Location.Position, m_LOSSensor.FOV.Contains(target.Location.Position), RUN_TO_TARGET_DISTANCE);
                            if (followAction != null)
                            {
                                FaceSpriteForDirection(target.Location.Position.X, target.Location.Position.Y);
                                m_Actor.IsRunning = false;
                                m_Actor.Activity = Activity.FOLLOWING;
                                m_Actor.TargetActor = target;
                                return followAction;
                            }
                        }
                    }
                }
            }
            #endregion

            // 10 wander
            #region
            ActorAction determinedAction = BehaviorWander(game);
            if (determinedAction != null)
            {
                Direction direction;
                //@@MP - handle the possible outcomes of BehaviorWander (Release 7-3)
                ActionBump actionBump = (determinedAction as ActionBump);
                ActionPush actionPush = (determinedAction as ActionPush);
                ActionBreak actionBreak = (determinedAction as ActionBreak);
                if (actionBump != null)
                    direction = actionBump.Direction;
                else if (actionPush != null)
                    throw new InvalidOperationException("dog illegal ActionPush");
                else if (actionBreak != null)
                    throw new InvalidOperationException("dog illegal ActionBreak");
                else
                    throw new InvalidOperationException("unhandled action type");

                FaceSpriteForDirection(direction); //@@MP (Release 7-3)
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

        #region Dogs specifics
        protected void RunToIfCloseTo(RogueGame game, Point pos, int closeDistance)
        {
            if (game.Rules.GridDistance(m_Actor.Location.Position, pos) <= closeDistance)
            {
                m_Actor.IsRunning = game.Rules.CanActorRun(m_Actor);
            }
            else
            {
                m_Actor.IsRunning = false;
            }
        }

        protected void FaceSpriteForDirection(Direction direction) //@@MP (Release 7-3)
        {
            lock (m_Actor)
            {
                //record which sprite model it is now
                List<string> skin = m_Actor.Doll.GetDecorations(DollPart.SKIN);

                //work out which skin sprite it needs
                string skinImage = null;
                if (Direction.COMPASS_EASTERLY.Contains(direction))
                {
                    switch (skin[0])
                    {
                        case "Actors\\Decoration\\dog_skin1_east": return; //already the required sprite
                        case "Actors\\Decoration\\dog_skin1_west": skinImage = GameImages.DOG_SKIN1_EAST; break;
                        case "Actors\\Decoration\\dog_skin2_east": return;
                        case "Actors\\Decoration\\dog_skin2_west": skinImage = GameImages.DOG_SKIN2_EAST; break;
                        case "Actors\\Decoration\\dog_skin3_east": return;
                        case "Actors\\Decoration\\dog_skin3_west": skinImage = GameImages.DOG_SKIN3_EAST; break;
                        default: throw new InvalidOperationException("unhandled dog skin sprite");
                    }
                }
                else if (Direction.COMPASS_WESTERLY.Contains(direction))
                {
                    switch (skin[0])
                    {
                        case "Actors\\Decoration\\dog_skin1_east": skinImage = GameImages.DOG_SKIN1_WEST; break;
                        case "Actors\\Decoration\\dog_skin1_west": return;
                        case "Actors\\Decoration\\dog_skin2_east": skinImage = GameImages.DOG_SKIN2_WEST; break;
                        case "Actors\\Decoration\\dog_skin2_west": return;
                        case "Actors\\Decoration\\dog_skin3_east": skinImage = GameImages.DOG_SKIN3_WEST; break;
                        case "Actors\\Decoration\\dog_skin3_west": return;
                        default: throw new InvalidOperationException("unhandled dog skin sprite");
                    }
                }
                else
                    return; //direction is straight south or north, so just keep the current skin

                if (skinImage == null)
                    throw new InvalidOperationException("issue: attempting to assign null skin image");

                //now set whichever skin sprite is relevant for the direction it's heading
                m_Actor.Doll.RemoveDecoration(skin[0]); //remove the existing skin sprite
                m_Actor.Doll.AddDecoration(DollPart.SKIN, skinImage); //add the other direction
            }
        }

        protected void FaceSpriteForDirection(int x, int y) //@@MP (Release 7-3)
        {
            if (x == 0 || y == 0)
                return;
            Point destination = new Point(x, y);

            Point from = m_Actor.Location.Position;
            Direction direction = LOS.DirectionTo(from.X, from.Y, destination.X, destination.Y); //where its heading
            FaceSpriteForDirection(direction);
        }
        #endregion
    }
}
