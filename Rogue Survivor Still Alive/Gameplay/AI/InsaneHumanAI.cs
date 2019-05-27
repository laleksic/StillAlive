using System;
using System.Collections.Generic;
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
    /// InsaneHumanAI, used by Unique Enraged Patient & Asylum patients.
    /// Extremely simple : attack anyone in sight or shout insanities, wander.
    /// </summary>
    class InsaneHumanAI : BaseAI
    {
        #region Constants
        const int ATTACK_CHANCE = 80;
        const int SHOUT_CHANCE = 80;
        const int USE_EXIT_CHANCE = 50;

        string[] INSANITIES = new string[]
        {
            "WHY WALK THERE?",
            "WHAT MAKES YOU FUN?",
            "YOU WEAR TOO MUCH COLORS!",
            "YOU HAVE BAD HABITS!",
            "MOM DIDN'T HANG ME!",
            "LUCIE! LUCIA!",
            "WHAT EGGS AND PASTA?",
            "TURTLE CATS!",
            "I REMEMBER THE CRABS!",
            "IT WAS AFTER THAT NOW!",
            "CUT IT! CUT IT NOW!",
            "DECEASED TOES!",
            "ICE-CREAM COPS!",
            "I SAW THAT FUCKING TWICE! TWICE! TWICE!",
            "FUCK BASTARD SAUSAGE!",
            "HEY YOU! STOP MOVING THE FLOOR!",
            "DROP THE FUCKING EGGS NOW!",
            "YOU GO FIRST AFTER ME!",
            "IT HURTS BUT ITS OK!",
            "LAST TIME WAS OK...",
            "SHE ISN'T NOT YET!",
            "I WAS CRAWLING HAHA!",
            "ROLLING LIKE AN EGG!",
            "THAT IS NOT DECENT!",
            "JUMP LIKE A FLOWER!",
            "NIGGER TRIGGER!",
            "CHEESE LIKE THESE...",
            "ILL-ADVISED LOBSTER!",
            "SSSHHHH! SILENCE... DO YOU SMELL?",
            "NOTHING BEATS. NOTHING!",
            "GROWN-UP MEN DON'T DO THAT!",
            "BARN BUSTER!",
            "SUPER SUPER?",
            "ONE MORE PASTA CRAP!",
            "LAZY LADY!",
            "I HATE TAP WATER!",
            "STILL WANKING FOR FOOD?",
            "LOOK! IT FITS LIKE A HOLE!",
            "PESKY POLAR PRANKS!",
            "LITTLE BY LITTLE YOU DIE...",
            "PLEASE TIE YOUR NECK PROPERLY!",
            "I SEE WHAT I SHIT ALL THE TIME!",
            "THAT'S FUCKING ANNOYING!",
            "I UNLOCK THE WALLS!",
            "I'M NOT SO SURE NOW!?",
            "RUSTY BUT TRUSTY!",
            "CHEESE LICKER!",
            "LAUNDRY TIME AGAIN AND AGAIN!",
            "DON'T YOU SEE I'M ASSEMBLED?",
            "MEXICAN MIDGETS!",
            "RAZOR RASCALS!",
            "PUNCH MY BALLS!",
            "STUCK IN A VICIOUS SQUARE!",
            "HORSE HOLSTER!",
            "THAT WAS COMPLETLY UNCALLED FOR!",
            "ROBOTS WON'T FOOL ME!"
        };

        static string[] FIGHT_EMOTES =
{
            "Leave me alone",         // flee
            "Damn it I'm trapped!",     // trapped
            "I'm not afraid"            // fight
        };
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
            List<Percept> list = m_LOSSensor.Sense(game, m_Actor);
            return list;
        }

        protected override ActorAction SelectAction(RogueGame game, List<Percept> percepts)
        {
            //HashSet<Point> fov = m_LOSSensor.FOV; //@@MP - unused (Release 5-7)
            List<Percept> mapPercepts = FilterSameMap(percepts); //@@MP - unused parameter (Release 5-7)

            ///////////////////////////////////////////////////////////////////////
            // alpha10 OBSOLETE 1 equip weapon
            // 0.1 if underground in total darkness, find nearest exit //@@MP (Release 6-5)
            // 1 equip best items //alpha10
            // 2 (chance) move closer to an enemy, nearest & visible enemies first
            // 3 (chance) shout insanities.
            // 4 (chance) use exit.
            // 5 wander
            ///////////////////////////////////////////////////////////////////////

            m_Actor.IsRunning = false; // don't run by default. // alpha10
            //@@MP (Release 6-5)
            ActorAction determinedAction = null;
            Map map = m_Actor.Location.Map;

            // 0.1 if in total darkness //@@MP - added (Release 6-5)
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

            // 1 equip best items
            #region
            ActorAction bestEquip = BehaviorEquipBestItems(game, false, false);
            if (bestEquip != null)
            {
                m_Actor.Activity = Activity.MANAGING_INVENTORY;
                return bestEquip;
            }

            /*// 1 equip weapon //alpha 10 replaced
            ActorAction equipWpnAction = BehaviorEquipWeapon(game);
            if (equipWpnAction != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return equipWpnAction;
            }*/
            #endregion

            // 2 (chance) move closer to an enemy, nearest & visible enemies first
            #region
            if (game.Rules.RollChance(ATTACK_CHANCE))
            {
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
            }
            #endregion

            // 3 (chance) shout insanities.
            #region
            if (game.Rules.RollChance(SHOUT_CHANCE))
            {
                string insanity = INSANITIES[game.Rules.Roll(0, INSANITIES.Length)];
                m_Actor.Activity = Activity.SHOUTING;
                game.DoEmote(m_Actor, insanity, true);
            }
            #endregion

            // 4 (chance) use exit.
            if (game.Rules.RollChance(USE_EXIT_CHANCE))
            {
                ActorAction useExit = BehaviorUseExit(game, UseExitFlags.ATTACK_BLOCKING_ENEMIES | UseExitFlags.BREAK_BLOCKING_OBJECTS);
                if (useExit != null)
                {
                    m_Actor.Activity = Activity.FINDING_EXIT;
                    return useExit;
                }
            }

            // 5 wander
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
