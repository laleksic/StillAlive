using System;
using System.Collections.Generic;
using System.Drawing;   // Point

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Engine.Actions;
using djack.RogueSurvivor.Engine.AI;
using djack.RogueSurvivor.Engine.Items;
using djack.RogueSurvivor.Gameplay.AI.Sensors;
using djack.RogueSurvivor.Gameplay.AI.Tools; //alpha 10

namespace djack.RogueSurvivor.Gameplay.AI
{
    [Serializable]
    /// <summary>
    /// Civilian AI : Civilians, Survivors, Cops.
    /// </summary>
    class CivilianAI : OrderableAI
    {
        #region Constants
        const int FOLLOW_NPCLEADER_MAXDIST = 1;
        const int FOLLOW_PLAYERLEADER_MAXDIST = 1;

        const int EXPLORATION_MAX_LOCATIONS = 30;
        const int EXPLORATION_MAX_ZONES = 3;

        const int USE_EXIT_CHANCE = 20;

        const int BUILD_TRAP_CHANCE = 50;
        const int BUILD_SMALL_FORT_CHANCE = 20;
        const int BUILD_LARGE_FORT_CHANCE = 50;
        const int START_FORT_LINE_CHANCE = 1;

        const int TELL_FRIEND_ABOUT_RAID_CHANCE = 20;
        const int TELL_FRIEND_ABOUT_ENEMY_CHANCE = 10;
        const int TELL_FRIEND_ABOUT_ITEMS_CHANCE = 10;
        const int TELL_FRIEND_ABOUT_SOLDIER_CHANCE = 20;

        const int MIN_TURNS_SAFE_TO_SLEEP = 10;

        const int USE_STENCH_KILLER_CHANCE = 75;

        const int HUNGRY_CHARGE_EMOTE_CHANCE = 50;
        const int HUNGRY_PUSH_OBJECTS_CHANCE = 25;

        const int LAW_ENFORCE_CHANCE = 30;

        const int DONT_LEAVE_BEHIND_EMOTE_CHANCE = 50;

        static string[] FIGHT_EMOTES = 
        {
            "Get away from me",         // flee
            "Damn it I'm trapped!",     // trapped
            "I'm not afraid"            // fight
        };

        // alpha10
        const string CANT_GET_ITEM_EMOTE = "Mmmh. Looks like I can't reach what I want.";

        // Unique emotes.
        static string[] BIG_BEAR_EMOTES =
        {
            "You fool",                 // flee
            "I'm fooled!",              // trapped
            "Be a man"                  // fight
        };
        static string[] FAMU_FATARU_EMOTES =
        {
            "Bakemono",       // flee
            "Nani!?",         // trapped
            "Kawaii"      // fight
        };
        static string[] SANTAMAN_EMOTES =
        {
            "DEM BLOODY KIDS!",                          // flee
            "LEAVE ME ALONE I AIN'T HAVE NO PRESENTS!",  // trapped
            "MERRY FUCKIN' CHRISTMAS"                   // fight
        };
        static string[] ROGUEDJACK_EMOTES =
        {
            "Sorry butt I am le busy,",                 // flee
            "I should have redone ze AI rootines!",     // trapped
            "Let me test le something on you"           // fight
        };
        static string[] DUCKMAN_EMOTES =
        {
            "I'LL QUACK YOU BACK",     // flee
            "THIS IS MY FINAL QUACK",  // trapped
            "I'M GONNA QUACK YOU"      // fight
        };
        static string[] HANS_VON_HANZ_EMOTES =
        {
            "RAUS",             // flee
            "MEIN FUHRER!",     // trapped
            "KOMM HIER BITE"    // fight
        };        

        #endregion

        #region Fields
        LOSSensor m_LOSSensor;

        int m_SafeTurns;
        ExplorationData m_Exploration;

        string[] m_Emotes;
        #endregion

        #region BaseAI
        public override void TakeControl(Actor actor)
        {
            base.TakeControl(actor);
        
            m_SafeTurns = 0;
            m_Exploration = new ExplorationData(EXPLORATION_MAX_LOCATIONS, EXPLORATION_MAX_ZONES);

            m_LastEnemySaw = null;
            m_LastItemsSaw = null;
            m_LastSoldierSaw = null;
            m_LastRaidHeard = null;
            m_Emotes = null;
        }

        protected override void CreateSensors()
        {
            m_LOSSensor = new LOSSensor(LOSSensor.SensingFilter.ACTORS | LOSSensor.SensingFilter.ITEMS | LOSSensor.SensingFilter.CORPSES);
        }

        protected override List<Percept> UpdateSensors(RogueGame game)
        {
            // emotes!
            if (m_Emotes == null)
            {
                // FIXME: ugly code.
                if (m_Actor.IsUnique)
                {
                    if (m_Actor == game.Session.UniqueActors.BigBear.TheActor)
                        m_Emotes = BIG_BEAR_EMOTES;
                    else if (m_Actor == game.Session.UniqueActors.FamuFataru.TheActor)
                        m_Emotes = FAMU_FATARU_EMOTES;
                    else if (m_Actor == game.Session.UniqueActors.Santaman.TheActor)
                        m_Emotes = SANTAMAN_EMOTES;
                    else if (m_Actor == game.Session.UniqueActors.Roguedjack.TheActor)
                        m_Emotes = ROGUEDJACK_EMOTES;
                    else if (m_Actor == game.Session.UniqueActors.Duckman.TheActor)
                        m_Emotes = DUCKMAN_EMOTES;
                    else if (m_Actor == game.Session.UniqueActors.HansVonHanz.TheActor)
                        m_Emotes = HANS_VON_HANZ_EMOTES;
                    else
                        m_Emotes = FIGHT_EMOTES;
                }
                else
                    m_Emotes = FIGHT_EMOTES;
            }

            // sense.
            return m_LOSSensor.Sense(game, m_Actor);
        }

        protected override ActorAction SelectAction(RogueGame game, List<Percept> percepts)
        {
            List<Percept> mapPercepts = FilterSameMap(percepts); //@@MP - unused parameter (Release 5-7)

            // DEBUG BOT
#if DEBUG
            bool botBreakpoint = false;
            bool verboseBotExploreWander = false;
            if (m_Actor.IsBotPlayer)
            {
                botBreakpoint = false; // true;
                verboseBotExploreWander = false; // true;
            }
#endif
            // END DEBUG BOT

            // hold the action that we determine to be applicable
            ActorAction determinedAction = null; //@@MP - created to reduce local variables, to keep all within this method enregistered (Release 5-7), moved (Release 6-1)

            ///////////////////////
            // A. Extinguish self
            // B. Equip best item.  // alpha10
            // C. Follow order
            // D. Normal behavior.
            ///////////////////////

            // A. Extinguish self  //@@MP (Release 6-1)
            if (m_Actor.IsOnFire)
            {
                m_Actor.IsRunning = true;
                determinedAction = BehaviorGoToNearestVisibleWater(game, m_LOSSensor.FOV);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.FLEEING_FROM_EXPLOSIVE;
                    return determinedAction;
                }
            }

            // alpha10
            // don't run by default.
            m_Actor.IsRunning = false;

            // B. Equip best item
            ActorAction bestEquip = BehaviorEquipBestItems(game, true, true);
            if (bestEquip != null)
            {
                m_Actor.Activity = Activity.MANAGING_INVENTORY;
                return bestEquip;
            }
            // end alpha10

            // C. Follow order
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

            // D. Normal behavior.
            #region
            //////////////////////////////////////////////////////////////////////
            // BEHAVIOR
            // - FLAGS
            // "courageous" : has leader, see leader, he is fighting and actor not tired.
            // - RULES
            // 0.1 run away from primed explosives (and fires //@@MP (Release 4)).
            // 0.2 if underground in total darkness, find nearest exit //@@MP (Release 6-5)
            // 1 throw grenades at enemies.
            // alpha10 OBSOLETE 2 equip weapon/armor
            // 3 fire at nearest (always if has leader, half of the time if not)  - check directives
            // 4 fight/flee/shout
            // 5 use medicine
            // 6 rest if tired
            // alpha10 obsolete and redundant with rule 4! 7 charge enemy if courageous
            // 8 eat when hungry (also eat corpses)
            // 9 sleep when almost sleepy and safe.
            //// 10 recharge lights //@@MP - added (Release 6-2), removed (Release 6-4)
            // 11 drop light/tracker with no batteries
            // alpha10 OBSOLETE 12 equip light/tracker/scent spray
            // 13 make room for food items if needed.
            // 14 get nearby item/trade (not if seeing enemy) - check directives.
            // 15 if hungry and no food, charge at people for food (option, not follower or law enforcer)
            // 16 use stench killer.
            // 17 close door behind me.
            // 18 use entertainment
            // 19 build trap or fortification.
            // 20 follow leader.
            // 21 take lead (if leadership)
            // 22 if hungry, tear down barricades & push objects.
            // 23 go revive corpse.
            // 24 use exit.
            // 25 tell friend about latest raid.
            // 26 tell friend about latest friendly soldier.
            // 27 tell friend about latest enemy.
            // 28 tell friend about latest items.
            // 29 (law enforcer) watch for murderers.
            // 30 (leader) don't leave followers behind.
            // 31 explore.
            // 32 wander.
            //////////////////////////////////////////////////////////////////////

            // get data.
            List<Percept> enemies = FilterEnemies(game, mapPercepts);
            bool hasEnemies = enemies != null && enemies.Count > 0;
            bool checkOurLeader = m_Actor.HasLeader && !DontFollowLeader;
            //bool seeLeader = checkOurLeader && m_LOSSensor.FOV.Contains(m_Actor.Leader.Location.Position); //@@MP - unused (Release 6-1)
            bool isLeaderFighting = checkOurLeader && IsAdjacentToEnemy(game, m_Actor.Leader);
            //bool isCourageous = checkOurLeader && seeLeader && isLeaderFighting && !game.Rules.IsActorTired(m_Actor);  //@@MP - unused (Release 6-1)
            //@@MP - these below are used in multiple steps, so make it reusable (Release 6-1)
            HashSet<Point> FOV = m_LOSSensor.FOV;
            Map map = m_Actor.Location.Map;

            //setup
            #region setup
            // safety counter.
            if (hasEnemies)
                m_SafeTurns = 0;
            else
                ++m_SafeTurns;

            // exploration.
            m_Exploration.Update(m_Actor.Location);

            // clear periodically or when changing maps.
            if (map.LocalTime.TurnCounter % WorldTime.TURNS_PER_HOUR == 0 || (PrevLocation != null && PrevLocation.Map != map))
            {
                ClearTabooTiles();
                ClearTabooTrades();
            }

            // last enemy saw.
            if (hasEnemies)
                m_LastEnemySaw = enemies[game.Rules.Roll(0, enemies.Count)];
            #endregion

            // 0.1 run away from primed explosives and fires //@@MP - fires added (Release 4).
            #region
            determinedAction = BehaviorFleeFromFires(game, m_Actor.Location);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.FLEEING_FROM_EXPLOSIVE;
                return determinedAction;
            }

            determinedAction = BehaviorFleeFromExplosives(game, FilterStacks(mapPercepts)); //@@MP - unused parameter (Release 5-7)
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.FLEEING_FROM_EXPLOSIVE;
                return determinedAction;
            }
            #endregion

            // 0.2 if in total darkness //@@MP (Release 6-2)
            #region
            int fov = game.Rules.ActorFOV(m_Actor, map.LocalTime, game.Session.World.Weather); //@@MP m_Actor.Location.Map (Release 6-2)
            if (fov <=0) //can't see anything, too dark
            {
                if (!game.Rules.CanActorSeeSky(m_Actor)) //if underground find nearest exit
                {
                    //if already on exit, leave
                    determinedAction = BehaviorUseExit(game, UseExitFlags.ATTACK_BLOCKING_ENEMIES);
                    if (determinedAction != null)
                    {
                        return determinedAction; //@@MP - forgot to actually have them use it (Release 6-5)
                    }
                    else
                    {
                        //find the nearest exit
                        determinedAction = BehaviorGoToNearestAIExit(game, 20, false);
                        if (determinedAction != null)
                        {
                            m_Actor.Activity = Activity.FINDING_EXIT;
                            return determinedAction;
                        }
                        else if (IsAdjacentToEnemy(game, m_Actor)) //@@MP - can't get to an exit. use self-defense if we're trapped by an enemy (Release 6-5)
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
            // if directive off, unequip.
            if (!this.Directives.CanThrowGrenades)
            {
                // unequip grenade?
                ItemGrenade eqGrenade = m_Actor.GetEquippedWeapon() as ItemGrenade;
                if (eqGrenade != null)
                {
                    ActionUnequipItem unequipGre = new ActionUnequipItem(m_Actor, game, eqGrenade);
                    if (unequipGre.IsLegal())
                    {
                        m_Actor.Activity = Activity.FIGHTING;
                        return unequipGre;
                    }
                }
            }
            else if (hasEnemies) // otherwise, throw? //@@MP - (Release 5-7)
            {
                determinedAction = BehaviorThrowGrenade(game, FOV, enemies); //@@MP m_LOSSensor.FOV, enemies); (Release 6-2)
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.FIGHTING;
                    return determinedAction;
                }
            }
            #endregion

            //// 2 equip weapon/armor  // alpha10 obsolete
            #region
            //ActorAction equipWpnAction = BehaviorEquipWeapon(game);
            //if (equipWpnAction != null)
            //{
            //    m_Actor.Activity = Activity.IDLE;
            //    return equipWpnAction;
            //}
            //ActorAction equipArmAction = BehaviorEquipBodyArmor(game);
            //if (equipArmAction != null)
            //{
            //    m_Actor.Activity = Activity.IDLE;
            //    return equipArmAction;
            //}
            #endregion

            // 3 fire at nearest enemy
            #region
            if (hasEnemies && this.Directives.CanFireWeapons && m_Actor.GetEquippedWeapon() is ItemRangedWeapon)
            {
                List<Percept> fireTargets = FilterFireTargets(game, enemies);
                if (fireTargets != null)
                {
                    Percept nearestTarget = FilterNearest(game, fireTargets);
                    Actor target = nearestTarget.Percepted as Actor;

                    // flee contact from someone SLOWER with no ranged weapon.
                    if (game.Rules.GridDistance(nearestTarget.Location.Position, m_Actor.Location.Position) == 1 &&
                        !HasEquipedRangedWeapon(target) &&
                        HasSpeedAdvantage(game, m_Actor,target))
                    {
                        // flee!
                        determinedAction = BehaviorWalkAwayFrom(game, nearestTarget);
                        if (determinedAction != null)
                        {
                            RunIfPossible(game.Rules);
                            m_Actor.Activity = Activity.FLEEING;
                            return determinedAction;
                        }
                    }

                    // fire ze missiles!
                    determinedAction = BehaviorRangedAttack(game, nearestTarget);
                    if (determinedAction != null)
                    {
                        m_Actor.Activity = Activity.FIGHTING;
                        m_Actor.TargetActor = target;
                        return determinedAction;
                    }

                }
            }
            #endregion

            // 4 fight/flee/shout
            #region
            //@@MP - try to wake nearby friends when there's a fire (Release 5-2)
            List<Percept> friends = FilterNonEnemies(game, mapPercepts);
            if (friends != null)
            {
                //HashSet<Point> FOV = m_LOSSensor.FOV; //@@MP (Release 6-2)
                foreach (Point p in FOV)
                {
                    if (map.IsAnyTileFireThere(map, p))
                    {
                        // shout
                        determinedAction = BehaviorWarnFriendsOfFire(game, friends);
                        if (determinedAction != null)
                        {
                            m_Actor.Activity = Activity.SHOUTING;
                            return determinedAction;
                        }
                    }
                }
            }

            //@@MP - the Enemies warning below is as was per of vanilla, except with friends moved above for reuse
            if (hasEnemies)
            {
                // shout?
                if (game.Rules.RollChance(50))
                {
                    if (friends != null)
                    {
                        determinedAction = BehaviorWarnFriends(game, friends, FilterNearest(game, enemies).Percepted as Actor);
                        if (determinedAction != null)
                        {
                            m_Actor.Activity = Activity.SHOUTING;
                            return determinedAction;
                        }
                    }
                }
                // fight or flee.
                RouteFinder.SpecialActions allowedChargeActions = RouteFinder.SpecialActions.JUMP | RouteFinder.SpecialActions.DOORS; // alpha10
                determinedAction = BehaviorFightOrFlee(game, enemies, isLeaderFighting, Directives.Courage, m_Emotes, allowedChargeActions); //@@MP - unused parameter (Release 5-7)
                if (determinedAction != null)
                {
                    return determinedAction;
                }
            }
            #endregion

            // 5 use medicine
            #region
            determinedAction = BehaviorUseMedicine(game, 2, 1, 2, 4, 2);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.HEALING;
                return determinedAction;
            }
            #endregion

            // 6 rest if tired
            #region
            determinedAction = BehaviorRestIfTired(game);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.RESTING;
                return new ActionWait(m_Actor, game);
            }
            #endregion

            //// 7 charge enemy if courageous  // alpha10 obsolete and redundant with rule 4!
            #region
            //if (hasEnemies && isCourageous)
            //{
            //    Percept nearestEnemy = FilterNearest(game, enemies);
            //    ActorAction chargeAction = BehaviorChargeEnemy(game, nearestEnemy, false, false);
            //    if (chargeAction != null)
            //    {
            //        m_Actor.Activity = Activity.FIGHTING;
            //        m_Actor.TargetActor = nearestEnemy.Percepted as Actor;
            //        return chargeAction;
            //    }
            //}
            #endregion

            // 8 eat when hungry (also eat corpses)
            #region
            if (game.Rules.IsActorHungry(m_Actor))
            {
                determinedAction = BehaviorEat(game);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.EATING;
                    return determinedAction;
                }
                if (game.Rules.IsActorStarving(m_Actor) || game.Rules.IsActorInsane(m_Actor))
                {
                    determinedAction = BehaviorGoEatCorpse(game, FilterCorpses(mapPercepts)); //@@MP - unused parameter (Release 5-7)
                    if (determinedAction != null)
                    {
                        m_Actor.Activity = Activity.EATING;
                        return determinedAction;
                    }
                }
                //TODO: flag to explore for food if none in inv //@@MP
            }
            #endregion

            // 9 sleep when almost sleepy and safe.
            #region
            if (m_SafeTurns >= MIN_TURNS_SAFE_TO_SLEEP && WouldLikeToSleep(game, m_Actor) && game.Rules.CanActorSleep(m_Actor) && this.Directives.CanSleep)
            {
                // prefer sleeping indoors
                if (IsInside(m_Actor))
                {
                    // prefer sleeping in basement. //@@MP - caused looping, possibly when exit was blocked (Release 6-6)
                    /*if (game.Rules.CanActorSeeSky(m_Actor)) //if not already in basement
                    {
                        determinedAction = BehaviorGoToNearestAIExit(game, 10, true);
                        if (determinedAction != null)
                        {
                            m_Actor.Activity = Activity.FINDING_EXIT;
                            return determinedAction;
                        }
                    }*/

                    // secure sleep.
                    determinedAction = BehaviorSecurePerimeter(game, FOV); //@@MP m_LOSSensor.FOV); (Release 6-2)
                    if (determinedAction != null)
                    {
                        m_Actor.Activity = Activity.PATROLLING;
                        return determinedAction;
                    }

                    // sleep.
                    determinedAction = BehaviorSleep(game, FOV); //@@MP m_LOSSensor.FOV); (Release 6-2)
                    if (determinedAction != null)
                    {
                        if (determinedAction is ActionSleep)
                            m_Actor.Activity = Activity.SLEEPING;
                        return determinedAction;
                    }
                }
                //TODO: add a flag here so that if we end up exploring/wandering to do so towards indoors (find nearest IsInside) //@@MP
            }
            #endregion

            //// 10 recharge lights //@@MP - added (Release 6-2)
            #region
            #if false //@@MP - simplified by making AI's batteries infinitely regenerating, making #10 redundant (Release 6-4)
            //ItemLight equippedLight = GetEquippedLight();
            //if (equippedLight != null && (equippedLight.Batteries <= ((equippedLight.Model as ItemLightModel).MaxBatteries) / 1.5))
            //ItemLight bestLight = HasItemOfType(typeof(ItemLight));
            /*if (HasItemOfType(typeof(ItemLight)))
            {*/
            ItemLight nominatedLight = null;
            foreach (Item it in m_Actor.Inventory.Items)
            {
                ItemLight light = it as ItemLight;
                if (light != null && light.Batteries <= ((light.Model as ItemLightModel).MaxBatteries) / 1.5)
                {
                    nominatedLight = light;
                    break; //just grab the one
                }
            }

            //choose a random light to recharge. ideally the AI will recharge them all eventually
            if (nominatedLight != null)
            {
                determinedAction = BehaviorGoToVisibleGenerator(game, FOV, nominatedLight);
                if (determinedAction != null)
                {
                    return determinedAction;
                }
            }
            //}
            #endif
            #endregion

            // 11 drop useless light/tracker/spray
#region
            determinedAction = BehaviorDropUselessItem(game);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.MANAGING_INVENTORY;
                return determinedAction;
            }
#endregion

            // 12 equip light/tracker/spray.  // alpha10 obsolete
#region
            //// tracker : if has leader or is a leader.
            //bool needCellPhone = m_Actor.HasLeader || m_Actor.CountFollowers > 0;
            //// then light.
            //bool needLight = NeedsLight(game);
            //// finally spray.
            //bool needSpray = IsGoodStenchKillerSpot(game, m_Actor.Location.Map, m_Actor.Location.Position);
            //// if tracker/light/spray useless, unequip it.
            //if (!needCellPhone && !needLight && !needSpray)
            //{
            //    ActorAction unequipUselessLeftItem = BehaviorUnequipLeftItem(game);
            //    if (unequipUselessLeftItem != null)
            //    {
            //        m_Actor.Activity = Activity.IDLE;
            //        return unequipUselessLeftItem;
            //    }
            //}
            //// tracker?
            //if(needCellPhone)
            //{
            //    ActorAction eqTrackerAction = BehaviorEquipCellPhone(game);
            //    if (eqTrackerAction != null)
            //    {
            //        m_Actor.Activity = Activity.IDLE;
            //        return eqTrackerAction;
            //    }
            //}
            //// ...or light?
            //else if (needLight)
            //{
            //    ActorAction eqLightAction = BehaviorEquipLight(game);
            //    if (eqLightAction != null)
            //    {
            //        m_Actor.Activity = Activity.IDLE;
            //        return eqLightAction;
            //    }

            //}
            //// ... scent spray?
            //else if (needSpray)
            //{
            //    ActorAction eqScentSpray = BehaviorEquipStenchKiller(game);
            //    if (eqScentSpray != null)
            //    {
            //        m_Actor.Activity = Activity.IDLE;
            //        return eqScentSpray;
            //    }
            //}
#endregion

            // 13 make room for food items if needed.
            // &&
            // 14 get nearby item/trade (not if seeing enemy)
            // ignore not currently visible items & blocked items.
#region
            if (!hasEnemies && this.Directives.CanTakeItems)
            {
#region Get items
                // 13. alpha10 new common behaviour code, also used by GangAI
                ActorAction getItemAction = BehaviorGoGetInterestingItems(game, mapPercepts, false, false, CANT_GET_ITEM_EMOTE, true, ref m_LastItemsSaw);

                if (getItemAction != null)
                    return getItemAction;
#endregion

#region Trade
                // 14
                if (Directives.CanTrade)
                {
                    // get actors we want to trade with.
                    List<Percept> tradingActors = FilterOut(FilterNonEnemies(game, mapPercepts), //@@MP - unused parameter (Release 5-7)
                        (p) =>
                        {
                            if (p.Turn != map.LocalTime.TurnCounter) return true;
                            Actor other = p.Percepted as Actor;
                            // dont bother player or someone we can't trade with or already did trade.
                            if (other.IsPlayer) return true;
                            if (!game.Rules.CanActorInitiateTradeWith(m_Actor, other)) return true;
                            if (IsActorTabooTrade(other)) return true;
                            // alpha10 dont bother someone who is fighting or fleeing
                            if (other.IsFightingOrFleeing)
                                return true;
                            // dont bother if no interesting items.
                            if (!HasAnyInterestingItem(game, other.Inventory, ItemSource.ANOTHER_ACTOR)) return true;
                            if (!((other.Controller as BaseAI).HasAnyInterestingItem(game, m_Actor.Inventory, ItemSource.ANOTHER_ACTOR))) return true;
                            // alpha10 reject if unreachable by baseai simple behaviours
                            if (!CanReachSimple(game, other.Location.Position, Tools.RouteFinder.SpecialActions.DOORS | Tools.RouteFinder.SpecialActions.JUMP))
                                return true;
                            // don't reject.
                            return false;
                        });
                    // trade with nearest.
                    if (tradingActors != null)
                    {
                        Actor tradeTarget = FilterNearest(game, tradingActors).Percepted as Actor;
                        if (game.Rules.IsAdjacent(m_Actor.Location, tradeTarget.Location))
                        {
                            determinedAction = new ActionTrade(m_Actor, game, tradeTarget);
                            if (determinedAction.IsLegal())
                            {
                                // remember we tried to trade.
                                MarkActorAsRecentTrade(tradeTarget);
                                // say, so we make sure we spend a turn and won't loop.
                                game.DoSay(m_Actor, tradeTarget, String.Format("Hey {0}, let's make a deal!", tradeTarget.Name), RogueGame.Sayflags.NONE);
                                m_Actor.Activity = Activity.TRADING;
                                return determinedAction;
                            }
                        }
                        else
                        {
                            // TODO -- consider moving this to DropUselessItems() //alpha 10
                            determinedAction = BehaviorIntelligentBumpToward(game, tradeTarget.Location.Position, false, false);
                            if (determinedAction != null)
                            {
                                // alpha10 announce it to make it clear to the player whats happening but dont spend AP (free action)
                                // might spam for a few turns, but its better than not understanding whats going on.
                                game.DoSay(m_Actor, tradeTarget, String.Format("Hey {0}, let's make a deal!", tradeTarget.Name), RogueGame.Sayflags.IS_FREE_ACTION);

                                m_Actor.Activity = Activity.TRADING;
                                m_Actor.TargetActor = tradeTarget;
                                return determinedAction;
                            }
                        }
                    }
                }
#endregion
            }
#endregion

            // 15 if hungry and no food, charge at people for food (option, not follower or law enforcer)
#region
            if (RogueGame.Options.IsAggressiveHungryCiviliansOn && 
                mapPercepts != null && !m_Actor.HasLeader && !m_Actor.Model.Abilities.IsLawEnforcer &&
                game.Rules.IsActorHungry(m_Actor) && HasNoFoodItems(m_Actor))
            {
                Percept targetForFood = FilterNearest(game, FilterActors(mapPercepts, //@@MP - unused parameter (Release 5-7)
                    (a) =>
                    {
                        // reject self, dead and leader/follower.
                        if (a == m_Actor) return false;
                        if (a.IsDead) return false;
                        if (a.Inventory == null || a.Inventory.IsEmpty) return false;
                        if (a.Leader == m_Actor || m_Actor.Leader == a) return false;

                        // actor has food or is standing on food.
                        if (a.Inventory.HasItemOfType(typeof(ItemFood))) return true;
                        Inventory groundInv = a.Location.Map.GetItemsAt(a.Location.Position);
                        if (groundInv == null || groundInv.IsEmpty) return false;
                        return groundInv.HasItemOfType(typeof(ItemFood));
                    }));

                if (targetForFood != null)
                {
                    determinedAction = BehaviorChargeEnemy(game, targetForFood, true, true); // alpha10 added hungry civs can break and push
                    if (determinedAction != null)
                    {
                        // randomly emote.
                        if (game.Rules.RollChance(HUNGRY_CHARGE_EMOTE_CHANCE))
                            game.DoSay(m_Actor, targetForFood.Percepted as Actor, "HEY! YOU! SHARE SOME FOOD!", RogueGame.Sayflags.IS_FREE_ACTION | RogueGame.Sayflags.IS_DANGER);

                        // chaaarge!
                        m_Actor.Activity = Activity.CHASING;
                        m_Actor.TargetActor = targetForFood.Percepted as Actor;
                        return determinedAction;
                    }
                }
            }
#endregion

            // 16 use stench killer.
#region
            if (game.Rules.RollChance(USE_STENCH_KILLER_CHANCE))
            {
                determinedAction = BehaviorUseStenchKiller(game);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.MANAGING_INVENTORY;
                    return determinedAction;
                }
            }
#endregion

            // 17 close door behind me.
#region
            determinedAction = BehaviorCloseDoorBehindMe(game, PrevLocation);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return determinedAction;
            }
#endregion

            // 18 use entertainment
#region
            if (m_Actor.Model.Abilities.HasSanity)
            {
                if (m_Actor.Sanity < 0.75f * game.Rules.ActorMaxSanity(m_Actor))
                {
                    determinedAction = BehaviorUseEntertainment(game);
                    if (determinedAction != null)
                    {
                        m_Actor.Activity = Activity.RESTING;
                        return determinedAction;
                    }
                }
                determinedAction = BehaviorDropBoringEntertainment(game);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.MANAGING_INVENTORY;
                    return determinedAction;
                }                
            }
#endregion

            // 19 build trap or fortification.
            // alpha10.1 moved trap/fortification rule before following leader rule so they will do it much more often
#region
            //build trap
            if (game.Rules.RollChance(BUILD_TRAP_CHANCE))
            {
                ActorAction trapAction = BehaviorBuildTrap(game);
                if (trapAction != null)
                {
                    m_Actor.Activity = Activity.BUILDING;
                    return trapAction;
                }
            }
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

            // 20 follow leader
#region
            if (checkOurLeader)
            {
                Point lastKnownLeaderPosition = m_Actor.Leader.Location.Position;
                bool isLeaderVisible = FOV.Contains(m_Actor.Leader.Location.Position); //@@MP m_LOSSensor.FOV (Release 6-2)
                int maxDist = m_Actor.Leader.IsPlayer ? FOLLOW_PLAYERLEADER_MAXDIST : FOLLOW_NPCLEADER_MAXDIST;
                determinedAction = BehaviorFollowActor(game, m_Actor.Leader, lastKnownLeaderPosition, isLeaderVisible, maxDist);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.FOLLOWING;
                    m_Actor.TargetActor = m_Actor.Leader;
                    return determinedAction;
                }
            }
#endregion

            // 21 take lead (if leadership)
#region
            bool hasLeadership = m_Actor.Sheet.SkillTable.GetSkillLevel((int)Skills.IDs.LEADERSHIP) >= 1;
            if (hasLeadership)
            {
                bool canLead = !checkOurLeader && m_Actor.CountFollowers < game.Rules.ActorMaxFollowers(m_Actor);
                if (canLead)
                {
                    Percept nearestFriend = FilterNearest(game, FilterNonEnemies(game, mapPercepts));
                    if (nearestFriend != null)
                    {
                        // alpha10 only if unreachable by baseai simple behaviours
                        if (CanReachSimple(game, nearestFriend.Location.Position, Tools.RouteFinder.SpecialActions.DOORS | Tools.RouteFinder.SpecialActions.JUMP))
                        {
                            ActorAction leadAction = BehaviorLeadActor(game, nearestFriend);
                            if (leadAction != null)
                            {
                                m_Actor.Activity = Activity.CHATTING;
                                m_Actor.TargetActor = nearestFriend.Percepted as Actor;
                                return leadAction;
                            }
                        }
                    }
                }
            }
#endregion

            // 22 if hungry, tear down barricades & push objects.
#region
            if (game.Rules.IsActorHungry(m_Actor))
            {
                determinedAction = BehaviorAttackBarricade(game);
                if (determinedAction != null)
                {
                    // emote.
                    game.DoEmote(m_Actor, "Open damn it! I know there's food in there!", true);

                    // go!
                    m_Actor.Activity = Activity.SEARCHING;
                    return determinedAction;
                }
                if (game.Rules.RollChance(HUNGRY_PUSH_OBJECTS_CHANCE))
                {
                    // alpha10.1 do that only inside where food is more likely to be hidden, pushing cars outside is stupid -_-
                    if (map.GetTileAt(m_Actor.Location.Position).IsInside) //@@MP m_Actor.Location.Map (Release 6-2)
                    {
                        determinedAction = BehaviorPushNonWalkableObject(game);
                        if (determinedAction != null)
                        {
                            // emote.
                            game.DoEmote(m_Actor, "Where's all the damn food?!", true);

                            // go!
                            m_Actor.Activity = Activity.SEARCHING;
                            return determinedAction;
                        }
                    }
                }
            }
#endregion

            // 23 go revive corpse.
#region
            determinedAction = BehaviorGoReviveCorpse(game, FilterCorpses(mapPercepts)); //@@MP - unused parameter (Release 5-7)
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.REVIVING;
                return determinedAction;
            }
#endregion

            // 24 use exit.
#region
            if (game.Rules.RollChance(USE_EXIT_CHANCE))
            {
                determinedAction = BehaviorUseExit(game, UseExitFlags.DONT_BACKTRACK);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.FINDING_EXIT;
                    return determinedAction;
                }
            }
#endregion

            // 25 tell friend about latest raid.
#region
            // tell?
            if (m_LastRaidHeard != null && game.Rules.RollChance(TELL_FRIEND_ABOUT_RAID_CHANCE))
            {
                determinedAction = BehaviorTellFriendAboutPercept(game, m_LastRaidHeard);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.CHATTING;
                    return determinedAction;
                }
            }
#endregion

            // 26 tell friend about latest soldier.
#region
            // update percept.
            Percept seeingSoldier = FilterFirst(mapPercepts,  //@@MP - unused parameter (Release 5-7)
                (p) =>
                {
                    Actor other = p.Percepted as Actor;
                    if (other == null || other == m_Actor)
                        return false;
                    return IsSoldier(other);
                });
            if (seeingSoldier != null)
                m_LastSoldierSaw = seeingSoldier;
            // tell?
            if (game.Rules.RollChance(TELL_FRIEND_ABOUT_SOLDIER_CHANCE) && m_LastSoldierSaw != null)
            {
                determinedAction = BehaviorTellFriendAboutPercept(game, m_LastSoldierSaw);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.CHATTING;
                    return determinedAction;
                }
            }
#endregion

            // 27 tell friend about latest enemy.
#region
            if (game.Rules.RollChance(TELL_FRIEND_ABOUT_ENEMY_CHANCE) && m_LastEnemySaw != null)
            {
                determinedAction = BehaviorTellFriendAboutPercept(game, m_LastEnemySaw);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.CHATTING;
                    return determinedAction;
                }
            }
#endregion

            // 28 tell friend about latest items.
#region
            if (game.Rules.RollChance(TELL_FRIEND_ABOUT_ITEMS_CHANCE) && m_LastItemsSaw != null)
            {
                determinedAction = BehaviorTellFriendAboutPercept(game, m_LastItemsSaw);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.CHATTING;
                    return determinedAction;
                }
            }
#endregion

            // 29 (law enforcer) watch for murderers.
#region
            if (m_Actor.Model.Abilities.IsLawEnforcer && mapPercepts != null && game.Rules.RollChance(LAW_ENFORCE_CHANCE))
            {
                Actor target;
                determinedAction = BehaviorEnforceLaw(game, mapPercepts, out target);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.PATROLLING;
                    m_Actor.TargetActor = target;
                    return determinedAction;
                }
            }
#endregion

            // 30 (leader) don't leave followers behind.
#region
            if (m_Actor.CountFollowers > 0)
            {
                Actor target;
                determinedAction = BehaviorDontLeaveFollowersBehind(game, 2, out target);
                if (determinedAction != null)
                {
                    // emote?
                    if (game.Rules.RollChance(DONT_LEAVE_BEHIND_EMOTE_CHANCE))
                    {
                        if (target.IsSleeping)
                            game.DoEmote(m_Actor, String.Format("patiently waits for {0} to wake up.", target.Name));
                        else
                        {
                            if (FOV.Contains(target.Location.Position)) //@MP m_LOSSensor.FOV (Release 6-2)
                                game.DoEmote(m_Actor, String.Format("Come on {0}! Hurry up!", target.Name));
                            else
                                game.DoEmote(m_Actor, String.Format("Where the hell is {0}?", target.Name));
                        }
                    }

                    // go!
                    m_Actor.Activity = Activity.WAITING;
                    return determinedAction;
                }
            }
#endregion

            // 31 explore
#region
            // DEBUG BOT
#if DEBUG
            if (botBreakpoint)
                Console.Out.WriteLine("test bot exploration breakpoint");
#endif
            // END DEBUG BOT

            determinedAction = BehaviorExplore(game, m_Exploration);
            if (determinedAction != null)
            {
                // VERBOSE BOT
#if DEBUG
                if (verboseBotExploreWander)
                    game.AddMessage(new Message(">> Bot is Exploring", map.LocalTime.TurnCounter)); //@@MP m_Actor.Location.Map (Release 6-2)
#endif
                // END VERBOSE BOT

                m_Actor.Activity = Activity.EXPLORING;
                return determinedAction;
            }
#endregion

            // 32 wander or wait
#region
            // VERBOSE BOT
#if DEBUG
            if (verboseBotExploreWander)
                game.AddMessage(new Message(">> Bot is Wandering", map.LocalTime.TurnCounter)); //@@MP m_Actor.Location.Map (Release 6-2)
#endif
            // END VERBOSE BOT

            determinedAction = BehaviorWander(game);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.WANDERING;
                return determinedAction;
            }
            else
            {
#if DEBUG && DEBUGAISELECTACTION
                //to assist with AI debugging
                Point pt = new Point(m_Actor.Location.Position.X, m_Actor.Location.Position.Y);
                Logger.WriteLine(Logger.Stage.RUN_MAIN, "CivilianAI::SelectAction() fell through to wait for " + m_Actor.Name + " @" + m_Actor.Location.Map.District.Name.ToString() + " " + pt.ToString() + " on turn #" + game.Session.WorldTime.TurnCounter);
#endif
                m_Actor.Activity = Activity.WAITING;
                return new ActionWait(m_Actor, game); //@@MP (Release 6-5)
            }

#endregion

#endregion
        }
#endregion
    }
}
