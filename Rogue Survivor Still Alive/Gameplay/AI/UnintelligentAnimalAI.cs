using System;
using System.Collections.Generic;
using System.Drawing;
using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Engine.Actions;
using djack.RogueSurvivor.Engine.AI;
using djack.RogueSurvivor.Gameplay.AI.Sensors;


namespace djack.RogueSurvivor.Gameplay.AI
{
    [Serializable]
    /// <summary>
    /// Simple Animal AI : used by rabbits and chickens.
    /// Designed for unintelligent creatures that exist only as a food source
    /// Added by (Release 7-6)
    /// </summary>
    class UnintelligentAnimalAI : BaseAI
    {
        #region Constants
        public static string[] FIGHT_EMOTES =
{
            "*screech*",            // flee
            "*screech*",         // trapped
            "*screech*"  // fight
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
            List<Percept> mapPercepts = FilterSameMap(percepts);
            ActorAction determinedAction = null;

            //////////////////////////////////////////////////////////////
            // 0.1 flee from fires
            // 1 flee from closest enemy
            // 2 eat - DISABLED
            // 3 rest or sleep
            // 4 wander
            //////////////////////////////////////////////////////////////

            // 0.1 flee from fires
            #region
            determinedAction = BehaviorFleeFromFires(game, m_Actor.Location);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.FLEEING;
                return determinedAction;
            }
            #endregion

            // 1 flee from closest enemy
            #region
            List<Percept> enemies = FilterEnemies(game, mapPercepts);
            if (enemies != null)
            {
                // try visible enemies first, the closer the best.
                List<Percept> visibleEnemies = Filter(enemies, (p) => p.Turn == m_Actor.Location.Map.LocalTime.TurnCounter); //@@MP - unused parameter (Release 5-7)
                if (visibleEnemies != null)
                {
                    //Percept bestEnemyPercept = null;
                    ActorAction bestBumpAction = null;
                    float closest = int.MaxValue;

                    foreach (Percept enemyP in visibleEnemies)
                    {
                        float distance = game.Rules.GridDistance(m_Actor.Location.Position, enemyP.Location.Position);
                        if (distance < closest)
                        {
                            ActorAction bumpAction = BehaviorWalkAwayFrom(game, enemyP);
                            if (bumpAction != null)
                            {
                                //check that we're staying on the grass. if not, we're "pinned in a corner" or "didn't see that enemy approach"
                                ActionBump bump = bumpAction as ActionBump;
                                Direction dir = bump.Direction;
                                Tile tile = m_Actor.Location.Map.GetTileAt(m_Actor.Location.Position + dir);
                                if (tile.Model.ID == (int)GameTiles.IDs.FLOOR_GRASS || tile.Model.ID == (int)GameTiles.IDs.FLOOR_PLANTED || tile.Model.ID == (int)GameTiles.IDs.FLOOR_DIRT)
                                {
                                    closest = distance;
                                    //bestEnemyPercept = enemyP;
                                    bestBumpAction = bumpAction;
                                }
                            }
                        }
                    }

                    if (bestBumpAction != null)
                    {
                        m_Actor.Activity = Activity.FLEEING;
                        //m_Actor.TargetActor = bestEnemyPercept.Percepted as Actor;
                        FaceSpriteForDirection((bestBumpAction as ActionBump).Direction);
                        return bestBumpAction;
                    }
                }
            }
            #endregion

            // 2 eat.
            #region
            //removed the need for these animals to eat, as it added nothing to the game and just cost CPU cycles
            /*if (m_Actor.FoodPoints < (game.Rules.ActorMaxFood(m_Actor) / 2))
            {
                m_Actor.FoodPoints += 10;
                m_Actor.Activity = Activity.EATING;
                return determinedAction;
            }*/
            #endregion

            // 3. rest or sleep
            #region
            if (game.Rules.IsActorTired(m_Actor))
            {
                m_Actor.Activity = Activity.RESTING;
                return new ActionWait(m_Actor, game);
            }
            //removed the need for these animals to sleep, as it added nothing to the game and just cost CPU cycles
            /*if (game.Rules.IsActorSleepy(m_Actor) && game.Rules.CanActorSleep(m_Actor))
            {
                m_Actor.Activity = Activity.SLEEPING;
                return new ActionSleep(m_Actor, game);
            }*/
            #endregion

            // 4 wander
            determinedAction = BehaviorSimpleAnimalWander(game, null);
            if (determinedAction != null)
            {
                FaceSpriteForDirection((determinedAction as ActionBump).Direction);
                m_Actor.Activity = Activity.WANDERING;
                return determinedAction;
            }
            else
            {
                //disabled the following as it really added nothing to the gameplay, and just cost precious CPU cycles
                //instead, chickens will spawn with an egg
                /*
                if (m_Actor.Model.ID == (int)GameActors.IDs.CHICKEN)
                {
                    int roll = game.Rules.Roll(0, 720); //lay egg?
                    switch (roll)
                    {
                        case 0:
                            Item egg = new Engine.Items.ItemFood(game.GameItems.CHICKEN_EGG, m_Actor.Location.Map.LocalTime.TurnCounter + (WorldTime.TURNS_PER_DAY * game.GameItems.CHICKEN_EGG.BestBeforeDays), true);
                            m_Actor.Location.Map.DropItemAt(egg, m_Actor.Location.Position);
                            break;
                        default: break; //no egg today, the eggs have gone away
                    }
                }
                */
                    
                m_Actor.Activity = Activity.RESTING;
                return new ActionWait(m_Actor, game);
            }
        }
        #endregion

        #region Dumb animals specifics

        protected void FaceSpriteForDirection(Direction direction)
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
                        case "Actors/Decoration/rabbit_skin_east": return; //already the required sprite
                        case "Actors/Decoration/rabbit_skin_west": skinImage = GameImages.RABBIT_SKIN_EAST; break;
                        case "Actors/Decoration/chicken_skin_east": return;
                        case "Actors/Decoration/chicken_skin_west": skinImage = GameImages.CHICKEN_SKIN_EAST; break;
                        default: throw new InvalidOperationException("unhandled dog skin sprite");
                    }
                }
                else if (Direction.COMPASS_WESTERLY.Contains(direction))
                {
                    switch (skin[0])
                    {
                        case "Actors/Decoration/rabbit_skin_east": skinImage = GameImages.RABBIT_SKIN_WEST; break;
                        case "Actors/Decoration/rabbit_skin_west": return;
                        case "Actors/Decoration/chicken_skin_east": skinImage = GameImages.CHICKEN_SKIN_WEST; break;
                        case "Actors/Decoration/chicken_skin_west": return;
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
