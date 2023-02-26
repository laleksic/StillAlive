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

        const int TRADE_CHANCE = 15; //@@MP (Release 7-6)

        static string[] FIGHT_EMOTES = 
        {
            "Get away from me",         // flee
            "Damn it I'm trapped!",     // trapped
            "I'm not afraid"            // fight
        };

        // alpha10
        const string CANT_GET_ITEM_EMOTE = "Mmmh. Looks like I can't reach what I want.";

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
                m_Emotes = FIGHT_EMOTES;

            // sense.
            return m_LOSSensor.Sense(game, m_Actor);
        }

        protected override ActorAction SelectAction(RogueGame game, List<Percept> percepts)
        {
            List<Percept> mapPercepts = FilterSameMap(percepts); //@@MP - unused parameter (Release 5-7)
            Map map = m_Actor.Location.Map;

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

            // don't run by default.    // alpha10
            m_Actor.IsRunning = false;

            ///////////////////////
            // A  Complete fishing.     //@@MP (Release 7-6)
            // B. Equip best item.  // alpha10
            // C. Follow order
            // D. Normal behavior.
            ///////////////////////


            // A complete fishing.     //@@MP (Release 7-6)
            #region
            //the reason why this is top priority is because we block the AI from equipping anything else to left_hand once they start fishing
            //fishing takes only 2 turns: 1 to unequip a non-fishing rod left_hand item and to then equip the rod, and 1 wait to "catch" the fish
            if (m_Actor.Activity == Activity.FISHING)
            {
                /*string eqName = "nothing";
                Item equipped = m_Actor.GetEquippedItem(DollPart.LEFT_HAND);
                if (equipped != null)
                    eqName = equipped.AName;
                Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("{0} should catch, equipped: {1}", m_Actor.Name, eqName)); //DELETETHIS*/

                determinedAction = BehaviorGoFish(game);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.WAITING;
                    return determinedAction;
                }
            }
            else if (IsEquipmentSlotTaboo(DollPart.LEFT_HAND))
                UnmarkEquipmentSlotAsTaboo(DollPart.LEFT_HAND); //fail-safe in case something distracted the NPC (it happens, though I don't know why given that this is the first checked)
            #endregion

            // B. Equip best item     // alpha10
            #region
            ActorAction bestEquip = BehaviorEquipBestItems(game, true, true);
            if (bestEquip != null)
            {
                m_Actor.Activity = Activity.MANAGING_INVENTORY;
                return bestEquip;
            }
            #endregion

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
            // 0.1 run away from primed explosives. (and fires //@@MP (Release 4)).
            // 0.2 Extinguish self
            // 0.3 if underground in total darkness, find nearest exit.    //@@MP (Release 6-5)
            // 1 throw grenades at enemies.
            // alpha10 OBSOLETE 2 equip weapon/armor
            // 3 fire at nearest (always if has leader, half of the time if not)  - check directives
            // 4 fight/flee/shout
            // 5 use medicine
            // 6 rest if tired
            // 7 head towards the rescue helicopter
            // alpha10 obsolete and redundant with rule 4! 7 charge enemy if courageous
            // 8 eat when hungry
            // 9 cook food
            // 10 sleep when almost sleepy and safe.
            //// 11 recharge lights.   //@@MP - added (Release 6-2), removed (Release 6-4)
            // 12 drop light/tracker with no batteries
            // alpha10 OBSOLETE 13 equip light/tracker/scent spray
            // 14 get nearby item. make room for food items if needed [check directives].
            // 15 if hungry and no food:
            //    15a if have the fishing rod, go fish
            //    15b hunt and butcher animals
            //    15c charge at people for food (option; not follower or law enforcer)
            //    15d tear down barricades & push objects.
            //    15e eat corpses if desperate/crazy
            // 16 use stench killer.
            // 17 close door behind me.
            // 18 use entertainment
            // 19 build trap or fortification.
            // 20 follow leader.
            // 21 take lead (if leadership)
            // 22 go revive corpse.
            // 23 use exit.
            // 24 tell friend about latest raid.
            // 25 tell friend about latest friendly soldier.
            // 26 tell friend about latest enemy.
            // 27 tell friend about latest items.
            // 28 (law enforcer) watch for murderers.
            // 29 (leader) don't leave followers behind.
            // 30 trade with nearby NPCs [check directives].
            // 31 make molotovs
            // 32 explore.
            // 33 wander or wait.
            //////////////////////////////////////////////////////////////////////

            // get data.
            List<Percept> enemies = FilterEnemies(game, mapPercepts);
            bool hasEnemies = enemies != null && enemies.Count > 0;
            bool hasNoFoodItems = HasNoFoodItems(m_Actor); //@@MP (Release 7-6)
            bool isHungry = game.Rules.IsActorHungry(m_Actor);
            bool checkOurLeader = m_Actor.HasLeader && !DontFollowLeader;
            //bool seeLeader = checkOurLeader && m_LOSSensor.FOV.Contains(m_Actor.Leader.Location.Position); //@@MP - unused (Release 6-1)
            bool isLeaderFighting = checkOurLeader && IsAdjacentToEnemy(game, m_Actor.Leader);
            //bool isCourageous = checkOurLeader && seeLeader && isLeaderFighting && !game.Rules.IsActorTired(m_Actor);  //@@MP - unused (Release 6-1)
            //@@MP - these below are used in multiple steps, so make it reusable (Release 6-1)
            HashSet<Point> FOV = m_LOSSensor.FOV;

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

            // 0.1 run away from primed explosives and fires          //@@MP - fires added (Release 4).
            #region
            determinedAction = BehaviorFleeFromFires(game, m_Actor.Location);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.FLEEING;
                return determinedAction;
            }

            determinedAction = BehaviorFleeFromExplosives(game, FilterStacks(mapPercepts)); //@@MP - unused parameter (Release 5-7)
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.FLEEING_FROM_EXPLOSIVE;
                return determinedAction;
            }
            #endregion

            // 0.2 extinguish self       //@@MP (Release 6-1)
            #region
            if (m_Actor.IsOnFire)
            {
                if (map.HasWaterTiles)
                {
                    determinedAction = BehaviorGoToNearestVisibleWater(game, m_LOSSensor.FOV);
                    if (determinedAction != null)
                    {
                        m_Actor.IsRunning = true;
                        m_Actor.Activity = Activity.FLEEING;
                        return determinedAction;
                    }
                }

                //stop-drop-and-roll       //@@MP (Release 7-6)
                m_Actor.Activity = Activity.FLEEING;
                return new ActionWait(m_Actor, game);
            }
            #endregion

            // 0.3 if in total darkness         //@@MP (Release 6-2)
            #region
            int fov = game.Rules.ActorFOV(m_Actor, map.LocalTime, game.Session.World.Weather); //@@MP m_Actor.Location.Map (Release 6-2)
            if (fov <=1) //can't see anything, too dark
            {
                if (!game.Rules.CanActorSeeSky(m_Actor)) //if underground find nearest exit
                {
                    //if already on exit, leave
                    determinedAction = BehaviorUseExit(game, UseExitFlags.ATTACK_BLOCKING_ENEMIES | UseExitFlags.DONT_BACKTRACK);
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

            // 7 head towards the rescue helicopter
            #region
            //check if the heli is here first
            if (map.LocalTime.Day == game.Session.ArmyHelicopterRescue_Day && map == game.Session.ArmyHelicopterRescue_Map && !map.LocalTime.IsNight)
            {
                //do something else if near it already. hopefully they will help defend it from undead
                if (game.Rules.StdDistance(m_Actor.Location.Position, game.Session.ArmyHelicopterRescue_Coordinates) >= 8)
                {
                    //only move to it if they can hear it
                    if (game.Rules.StdDistance(m_Actor.Location.Position, game.Session.ArmyHelicopterRescue_Coordinates) <= m_Actor.AudioRange)
                    {
                        ActorAction getToTheChopper = BehaviorIntelligentBumpToward(game, game.Session.ArmyHelicopterRescue_Coordinates, true, false);
                        if (getToTheChopper != null)
                        {
                            m_Actor.IsRunning = true;
                            m_Actor.Activity = Activity.EXPLORING;
                            return getToTheChopper;
                        }
                    }
                }
            }
            #endregion

            //// OLD 7 charge enemy if courageous  // alpha10 obsolete and redundant with rule 4!
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

            // 8 eat when hungry
            // 9 cook raw food
            #region
            if (!hasNoFoodItems)
            {
                if (isHungry)
                {
                    determinedAction = BehaviorEat(game);
                    if (determinedAction != null)
                    {
                        m_Actor.Activity = Activity.EATING;
                        return determinedAction;
                    }
                }

                if (HasCookableFoodItem(m_Actor))          //@@MP - added (Release 7-6)
                {
                    //next to a MapObject fire
                    determinedAction = BehaviorCookFood(game);
                    if (determinedAction != null)
                    {
                        m_Actor.Activity = Activity.COOKING;
                        return determinedAction;
                    }

                    //need to find a MapObject fire
                    determinedAction = BehaviorGoToNearestVisibleMapObjectFire(game, m_LOSSensor.FOV);
                    if (determinedAction != null)
                    {
                        //Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("{0}: {1} is looking for somewhere to cook", game.Session.WorldTime.TurnCounter.ToString(), m_Actor.Name)); //DELETETHIS
                        m_Actor.Activity = Activity.COOKING;
                        return determinedAction;
                    }
                }
            }
            #endregion

            // 10 sleep when almost sleepy and safe.
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

            //// 11 recharge lights             //@@MP - added (Release 6-2)
            #region
            #if false //@@MP - simplified by making AI's batteries never lose charge, making #10 redundant (Release 7-5)
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

            // 12 drop useless light/tracker/spray
#region
            determinedAction = BehaviorDropUselessItem(game);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.MANAGING_INVENTORY;
                return determinedAction;
            }
            #endregion

            // 13 equip light/tracker/spray.        // alpha10 obsolete
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

            // 14 get nearby item (not if seeing enemy)
#region
            // make room for food items if needed.
            // ignore not currently visible items & blocked items.
            if (!hasEnemies && this.Directives.CanTakeItems)
            {
                // alpha10 new common behaviour code, also used by GangAI
                ActorAction getItemAction = BehaviorGoGetInterestingItems(game, mapPercepts, false, false, CANT_GET_ITEM_EMOTE, true, ref m_LastItemsSaw);

                if (getItemAction != null)
                    return getItemAction;
            }
#endregion

            // 15 if has no food
#region
            if (hasNoFoodItems)//if (isHungry && hasNoFoodItems)
            {
                // 15a have a fishing rod, go fish          //@@MP - added (Release 7-6)
                #region
                if (map.HasFishing && HasItemOfModel(game.GameItems.FISHING_ROD))
                {
                    //be hungry
                    //have no food
                    //have fishing rod
                    //find pond
                    //stand next to pond (if not already)
                    //equip fishing rod (if not equipped)
                    //wait (to land the fish)

                    //next to a pond?
                    determinedAction = BehaviorGoFish(game);
                    if (determinedAction != null)
                    {
                        /*string eqName = "nothing";
                        Item equipped = m_Actor.GetEquippedItem(DollPart.LEFT_HAND);
                        if (equipped != null)
                            eqName = equipped.AName;
                        Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("{0} should wait, equipped: {1}", m_Actor.Name, eqName)); //DELETETHIS */

                        m_Actor.Activity = Activity.FISHING;
                        return determinedAction;
                    }

                    //head towards the pond
                    determinedAction = BehaviorGoToNearestVisibleWater(game, m_LOSSensor.FOV);
                    if (determinedAction != null)
                    {
                        //Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("{0}: {1} is moving towards a fishing spot", game.Session.WorldTime.TurnCounter.ToString(), m_Actor.Name)); //DELETETHIS
                        m_Actor.Activity = Activity.SEARCHING;
                        return determinedAction;
                    }

                    //need to find a pond
                    for (int x = 0; x < map.Width; x++)
                    {
                        for (int y = 0; y < map.Height; y++)
                        {
                            Point pt = new Point(x, y);
                            if (map.GetTileAt(pt).Model.IsWater) //the first tile that's water is good enough. thanks to Map.HasFishing, this will only apply to ponds, and not shopping mall fountains
                            {
                                ActorAction moveTowardsThePond = BehaviorIntelligentBumpToward(game, pt, false, false);
                                if (moveTowardsThePond != null)
                                {
                                    //Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("{0}: {1} is looking for somewhere to fish", game.Session.WorldTime.TurnCounter.ToString(), m_Actor.Name)); //DELETETHIS
                                    m_Actor.Activity = Activity.SEARCHING;
                                    return moveTowardsThePond;
                                }
                            }
                        }
                    }
                }
                #endregion

                if (isHungry)
                {
                    // 15b hunt and butcher animals          //@@MP - added (Release 7-6)
                    #region
                    if (mapPercepts != null)
                    {
                        //placed it behind the IsHungry check so that if the player is nearby they have slightly more chance to hunt
                        //civilians will only hunt 'unintelligent' animals (ie chickens and rabbits), but they will butcher a dog corpse if they come across one

                        // butcher
                        determinedAction = BehaviorGoButcherAnimalCorpse(game, FilterCorpses(mapPercepts)); //@@MP - unused parameter (Release 5-7)
                        if (determinedAction != null)
                        {
                            m_Actor.Activity = Activity.HUNTING;
                            return determinedAction;
                        }

                        // hunt
                        Percept targetForFood = FilterNearest(game, FilterActors(mapPercepts,
                            (a) =>
                            {
                                // we want live simple animals
                                if (a.IsDead) return false;
                                if (a.Faction != game.GameFactions.TheUnintelligentAnimals) return false;

                                return true;
                            }));

                        if (targetForFood != null)
                        {
                            determinedAction = BehaviorChargeEnemy(game, targetForFood, false, false);
                            if (determinedAction != null)
                            {
                                // chaaarge!
                                m_Actor.Activity = Activity.HUNTING;
                                m_Actor.TargetActor = targetForFood.Percepted as Actor;
                                //Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("{0}: {1} is trying to catch {2}.", game.Session.WorldTime.TurnCounter.ToString(), m_Actor.Name, m_Actor.TargetActor.Name)); //DELETETHIS
                                return determinedAction;
                            }
                        }
                    }
                    #endregion

                    // 15c charge at people for food (option, not follower or law enforcer)
                    #region
                    if (RogueGame.Options.IsAggressiveHungryCiviliansOn && mapPercepts != null && !m_Actor.HasLeader && !m_Actor.Model.Abilities.IsLawEnforcer)
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
                                game.DoMakeAggression(m_Actor, m_Actor.TargetActor); //@@MP (Release 7-6)
                                return determinedAction;
                            }
                        }
                    }
                    #endregion

                    // 15d tear down barricades & push objects.
                    #region
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
                        if (map.GetTileAt(m_Actor.Location.Position).IsInside)
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
                    #endregion

                    // 15e eat corpses in desperation/insanity
                    #region
                    if (game.Rules.IsActorStarving(m_Actor) || game.Rules.IsActorInsane(m_Actor))
                    {
                        determinedAction = BehaviorGoEatCorpse(game, FilterCorpses(mapPercepts), out int x, out int y); //@@MP - x y only relevant for FeralDogAI (Release 7-3)
                        if (determinedAction != null)
                        {
                            m_Actor.Activity = Activity.EATING;
                            return determinedAction;
                        }
                    }
                    #endregion
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
#region
            // alpha10.1 moved trap/fortification rule before following leader rule so they will do it much more often
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

            // 22 go revive corpse.
#region
            determinedAction = BehaviorGoReviveCorpse(game, FilterCorpses(mapPercepts)); //@@MP - unused parameter (Release 5-7)
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.REVIVING;
                return determinedAction;
            }
            #endregion

            // 23 use exit.
#region
            if (game.Rules.RollChance(USE_EXIT_CHANCE))// && !m_Actor.IsPlayer) //useful for testing, stop the bot from exiting
            {
                determinedAction = BehaviorUseExit(game, UseExitFlags.DONT_BACKTRACK);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.FINDING_EXIT;
                    return determinedAction;
                }
            }
#endregion

            // 24 tell friend about latest raid.
#region
            // tell?
            if (m_LastRaidHeard != null && game.Rules.RollChance(TELL_FRIEND_ABOUT_RAID_CHANCE))
            {
                determinedAction = BehaviorTellFriendAboutPercept(game, m_LastRaidHeard);
                if (determinedAction != null)
                {
                    game.RegenActorSanity(m_Actor, WorldTime.TURNS_PER_HOUR);    //@@MP - added (Release 7-6)
                    m_Actor.Activity = Activity.CHATTING;
                    return determinedAction;
                }
            }
#endregion

            // 25 tell friend about latest soldier.
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
                    game.RegenActorSanity(m_Actor, WorldTime.TURNS_PER_HOUR);    //@@MP - added (Release 7-6)
                    m_Actor.Activity = Activity.CHATTING;
                    return determinedAction;
                }
            }
#endregion

            // 26 tell friend about latest enemy.
#region
            if (game.Rules.RollChance(TELL_FRIEND_ABOUT_ENEMY_CHANCE) && m_LastEnemySaw != null)
            {
                determinedAction = BehaviorTellFriendAboutPercept(game, m_LastEnemySaw);
                if (determinedAction != null)
                {
                    game.RegenActorSanity(m_Actor, WorldTime.TURNS_PER_HOUR);    //@@MP - added (Release 7-6)
                    m_Actor.Activity = Activity.CHATTING;
                    return determinedAction;
                }
            }
#endregion

            // 27 tell friend about latest items.
#region
            if (game.Rules.RollChance(TELL_FRIEND_ABOUT_ITEMS_CHANCE) && m_LastItemsSaw != null)
            {
                determinedAction = BehaviorTellFriendAboutPercept(game, m_LastItemsSaw);
                if (determinedAction != null)
                {
                    game.RegenActorSanity(m_Actor, WorldTime.TURNS_PER_HOUR);    //@@MP - added (Release 7-6)
                    m_Actor.Activity = Activity.CHATTING;
                    return determinedAction;
                }
            }
#endregion

            // 28 (law enforcer) watch for murderers.
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

            // 29 (leader) don't leave followers behind.
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

            // 30 trade with nearby NPCs (not if seeing enemy)
            #region Trade
            //@@MP - moved this a long way down the ordered compared to vanilla RS, and put it being a dice roll, as the NPCs were getting stuuck in endless loops of trading amongst one another (Release 7-6)
            if (!hasEnemies && this.Directives.CanTrade && game.Rules.RollChance(TRADE_CHANCE))
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
                            //if (m_Actor.Location.Map == game.Player.Location.Map)
                                //Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("[{0}]: {1} tries to trade with {2}.", game.Session.WorldTime.TurnCounter.ToString(), m_Actor.Name, tradeTarget.Name)); //DELETETHIS

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
                            //if (m_Actor.Location.Map == game.Player.Location.Map)
                                //Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("[{0}]: {1} moves to trade with {2}.", game.Session.WorldTime.TurnCounter.ToString(), m_Actor.Name, tradeTarget.Name)); //DELETETHIS

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

            // 31 make molotovs   //@@MP (Release 7-1)
            #region
            determinedAction = BehaviorMakeMolotovs(game);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.MANAGING_INVENTORY;
                return determinedAction;
            }
            #endregion

            // 32 explore
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

            // 33 wander or wait
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
