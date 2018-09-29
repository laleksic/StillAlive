using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;   // Point

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Engine.Actions;
using djack.RogueSurvivor.Engine.AI;
using djack.RogueSurvivor.Engine.Items;
using djack.RogueSurvivor.Engine.MapObjects;
using djack.RogueSurvivor.Gameplay.AI.Sensors;

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
            "Go away",                  // flee
            "Damn it I'm trapped!",     // trapped
            "I'm not afraid"            // fight
        };      

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

            ///////////////////////
            // 1. Follow order
            // 2. Normal behavior.
            ///////////////////////

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

            // 2. Normal behavior.
            #region
            //////////////////////////////////////////////////////////////////////
            // BEHAVIOR
            // - FLAGS
            // "courageous" : has leader, see leader, he is fighting and actor not tired.
            // - RULES
            // 0 run away from primed explosives (and fires //@@MP (Release 4)).
            // 1 throw grenades at enemies.
            // 2 equip weapon/armor
            // 3 fire at nearest (always if has leader, half of the time if not)  - check directives
            // 4 fight/flee/shout
            // 5 use medicine
            // 6 rest if tired
            // 7 charge enemy if courageous
            // 8 eat when hungry (also eat corpses)
            // 9 sleep when almost sleepy and safe.
            // 10 drop light/tracker with no batteries
            // 11 equip light/tracker/scent spray
            // 12 make room for food items if needed.
            // 13 get nearby item/trade (not if seeing enemy) - check directives.
            // 14 if hungry and no food, charge at people for food (option, not follower or law enforcer)
            // 15 use stench killer.
            // 16 close door behind me.
            // 17 use entertainment
            // 18 follow leader.
            // 19 take lead (if leadership)
            // 20 if hungry, tear down barricades & push objects.
            // 21 go revive corpse.
            // 22 use exit.
            // 23 build trap or fortification.
            // 24 tell friend about latest raid.
            // 25 tell friend about latest friendly soldier.
            // 26 tell friend about latest enemy.
            // 27 tell friend about latest items.
            // 28 (law enforcer) watch for murderers.
            // 29 (leader) don't leave followers behind.
            // 30 explore.
            // 31 wander.
            //////////////////////////////////////////////////////////////////////

            // don't run by default.
            m_Actor.IsRunning = false;

            // get data.
            List<Percept> enemies = FilterEnemies(game, mapPercepts);
            bool hasEnemies = enemies != null && enemies.Count > 0;
            bool checkOurLeader = m_Actor.HasLeader && !DontFollowLeader;
            bool seeLeader = checkOurLeader && m_LOSSensor.FOV.Contains(m_Actor.Leader.Location.Position);
            bool isLeaderFighting = checkOurLeader && IsAdjacentToEnemy(game, m_Actor.Leader);
            bool isCourageous = checkOurLeader && seeLeader && isLeaderFighting && !game.Rules.IsActorTired(m_Actor);

            //setup
            #region setup
            // safety counter.
            if (hasEnemies)
                m_SafeTurns = 0;
            else
                ++m_SafeTurns;

            // exploration.
            m_Exploration.Update(m_Actor.Location);

            // clear taboo tiles : periodically or when changing maps.
            if (m_Actor.Location.Map.LocalTime.TurnCounter % WorldTime.TURNS_PER_HOUR == 0 || 
                (PrevLocation != null && PrevLocation.Map != m_Actor.Location.Map))
            {
                ClearTabooTiles();
            }
            // clear trades.
            if (m_Actor.Location.Map.LocalTime.TurnCounter % WorldTime.TURNS_PER_DAY == 0)
            {
                ClearTabooTrades();
            }

            // last enemy saw.
            if (hasEnemies)
                m_LastEnemySaw = enemies[game.Rules.Roll(0, enemies.Count)];
            #endregion

            // hold the action that we determine to be applicable
            ActorAction determinedAction = null; //@@MP - created to reduce local variables, to keep all within this method enregistered (Release 5-7)

            // 0 run away from primed explosives and fires //@@MP - fires added (Release 4).
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
                        m_Actor.Activity = Activity.IDLE;
                        return unequipGre;
                    }
                }
            }
            else if (hasEnemies) // otherwise, throw? //@@MP - (Release 5-7)
            {
                determinedAction = BehaviorThrowGrenade(game, m_LOSSensor.FOV, enemies);
                if (determinedAction != null)
                {
                    return determinedAction;
                }
            }
            #endregion

            // 2 equip weapon/armor
            #region
            determinedAction = BehaviorEquipWeapon(game);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return determinedAction;
            }
            determinedAction = BehaviorEquipBodyArmor(game);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return determinedAction;
            }
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
                HashSet<Point> FOV = m_LOSSensor.FOV;
                foreach (Point p in FOV)
                {
                    if (Map.IsAnyTileFireThere(m_Actor.Location.Map, p))
                    {
                        // shout
                        determinedAction = BehaviorWarnFriendsOfFire(game, friends);
                        if (determinedAction != null)
                        {
                            m_Actor.Activity = Activity.FLEEING;
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
                            m_Actor.Activity = Activity.IDLE;
                            return determinedAction;
                        }
                    }
                }
                // fight or flee.
                determinedAction = BehaviorFightOrFlee(game, enemies, isLeaderFighting, Directives.Courage, m_Emotes); //@@MP - unused parameter (Release 5-7)
                if (determinedAction != null)
                {
                    return determinedAction;
                }
            }
            #endregion

            // 5 use medicine
            #region
            determinedAction = BehaviorUseMedecine(game, 2, 1, 2, 4, 2);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return determinedAction;
            }
            #endregion

            // 6 rest if tired
            #region
            determinedAction = BehaviorRestIfTired(game);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return new ActionWait(m_Actor, game);
            }
            #endregion

            // 7 charge enemy if courageous
            #region
            if (hasEnemies && isCourageous)
            {
                Percept nearestEnemy = FilterNearest(game, enemies);
                determinedAction = BehaviorChargeEnemy(game, nearestEnemy);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.FIGHTING;
                    m_Actor.TargetActor = nearestEnemy.Percepted as Actor;
                    return determinedAction;
                }
            }
            #endregion

            // 8 eat when hungry (also eat corpses)
            #region
            if (game.Rules.IsActorHungry(m_Actor))
            {
                determinedAction = BehaviorEat(game);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.IDLE;
                    return determinedAction;
                }
                if (game.Rules.IsActorStarving(m_Actor) || game.Rules.IsActorInsane(m_Actor))
                {
                    determinedAction = BehaviorGoEatCorpse(game, FilterCorpses(mapPercepts)); //@@MP - unused parameter (Release 5-7)
                    if (determinedAction != null)
                    {
                        m_Actor.Activity = Activity.IDLE;
                        return determinedAction;
                    }
                }
            }
            #endregion

            // 9 sleep when almost sleepy and safe.
            #region
            if (m_SafeTurns >= MIN_TURNS_SAFE_TO_SLEEP && this.Directives.CanSleep && 
                WouldLikeToSleep(game, m_Actor) && IsInside(m_Actor) && game.Rules.CanActorSleep(m_Actor))
            {
                // secure sleep.
                determinedAction = BehaviorSecurePerimeter(game, m_LOSSensor.FOV);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.IDLE;
                    return determinedAction;
                }

                // sleep.
                determinedAction = BehaviorSleep(game, m_LOSSensor.FOV);
                if (determinedAction != null)
                {
                    if (determinedAction is ActionSleep)
                        m_Actor.Activity = Activity.SLEEPING;
                    return determinedAction;
                }
            }
            #endregion

            // 10 drop useless light/tracker/spray
            #region
            determinedAction = BehaviorDropUselessItem(game);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return determinedAction;
            }
            #endregion

            // 11 equip light/tracker/spray.
            #region
            // tracker : if has leader or is a leader.
            bool needCellPhone = m_Actor.HasLeader || m_Actor.CountFollowers > 0;
            // then light.
            bool needLight = NeedsLight(game);
            // finally spray.
            bool needSpray = IsGoodStenchKillerSpot(m_Actor.Location.Map, m_Actor.Location.Position); //@@MP - unused parameter (Release 5-7)
            // if tracker/light/spray useless, unequip it.
            if (!needCellPhone && !needLight && !needSpray)
            {
                determinedAction = BehaviorUnequipLeftItem(game);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.IDLE;
                    return determinedAction;
                }
            }
            // tracker?
            if(needCellPhone)
            {
                determinedAction = BehaviorEquipCellPhone(game);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.IDLE;
                    return determinedAction;
                }
            }
            // ...or light?
            else if (needLight)
            {
                determinedAction = BehaviorEquipLight(game);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.IDLE;
                    return determinedAction;
                }

            }
            // ... scent spray?
            else if (needSpray)
            {
                determinedAction = BehaviorEquipStenchKiller(game);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.IDLE;
                    return determinedAction;
                }
            }
            #endregion

            // 12 make room for food items if needed.
            // &&
            // 13 get nearby item/trade (not if seeing enemy)
            // ignore not currently visible items & blocked items.
            #region
            if (!hasEnemies && this.Directives.CanTakeItems)
            {
                Map map = m_Actor.Location.Map;

                #region Get nearby item
                // list visible items.
                List<Percept> interestingStacks = FilterOut(FilterStacks(mapPercepts), //@@MP - unused parameter (Release 5-7)
                    (p) => (p.Turn != map.LocalTime.TurnCounter) || 
                            IsOccupiedByOther(map, p.Location.Position) || 
                            IsTileTaboo(p.Location.Position) ||
                            !HasAnyInterestingItem(game, p.Percepted as Inventory));

                // if some items...
                if (interestingStacks != null)
                {
                    // update last percept saw.
                    Percept nearestStack = FilterNearest(game, interestingStacks);
                    m_LastItemsSaw = nearestStack;

                    // 12: make room for food if needed.
                    determinedAction = BehaviorMakeRoomForFood(game, interestingStacks);
                    if (determinedAction != null)
                    {
                        m_Actor.Activity = Activity.IDLE;
                        return determinedAction;
                    }

                    // 13: try to grab.
                    determinedAction = BehaviorGrabFromStack(game, nearestStack.Location.Position, nearestStack.Percepted as Inventory);
                    if (determinedAction != null)
                    {
                        m_Actor.Activity = Activity.IDLE;
                        return determinedAction;
                    }
                    // we can't grab the item. mark the tile as taboo.
                    MarkTileAsTaboo(nearestStack.Location.Position);
                    // emote
                    game.DoEmote(m_Actor, "Mmmh. Looks like I can't reach what I want.");
                }
                #endregion

                #region Trade
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
                            // dont bother if no interesting items.
                            if (!HasAnyInterestingItem(game, other.Inventory)) return true;
                            if (!((other.Controller as BaseAI).HasAnyInterestingItem(game, m_Actor.Inventory))) return true;
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
                                return determinedAction;
                            }
                        }
                        else
                        {
                            determinedAction = BehaviorIntelligentBumpToward(game, tradeTarget.Location.Position);
                            if (determinedAction != null)
                            {
                                m_Actor.Activity = Activity.FOLLOWING;
                                m_Actor.TargetActor = tradeTarget;
                                return determinedAction;
                            }
                        }
                    }
                }
                #endregion
            }
            #endregion

            // 14 if hungry and no food, charge at people for food (option, not follower or law enforcer)
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
                    determinedAction = BehaviorChargeEnemy(game, targetForFood);
                    if (determinedAction != null)
                    {
                        // randomly emote.
                        if (game.Rules.RollChance(HUNGRY_CHARGE_EMOTE_CHANCE))
                            game.DoSay(m_Actor, targetForFood.Percepted as Actor, "HEY! YOU! SHARE SOME FOOD!", RogueGame.Sayflags.IS_FREE_ACTION);

                        // chaaarge!
                        m_Actor.Activity = Activity.FIGHTING;
                        m_Actor.TargetActor = targetForFood.Percepted as Actor;
                        return determinedAction;
                    }
                }
            }
            #endregion

            // 15 use stench killer.
            #region
            if (game.Rules.RollChance(USE_STENCH_KILLER_CHANCE))
            {
                determinedAction = BehaviorUseStenchKiller(game);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.IDLE;
                    return determinedAction;
                }
            }
            #endregion

            // 16 close door behind me.
            #region
            determinedAction = BehaviorCloseDoorBehindMe(game, PrevLocation);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return determinedAction;
            }
            #endregion

            // 17 use entertainment
            #region
            if (m_Actor.Model.Abilities.HasSanity)
            {
                if (m_Actor.Sanity < 0.75f * game.Rules.ActorMaxSanity(m_Actor))
                {
                    determinedAction = BehaviorUseEntertainment(game);
                    if (determinedAction != null)
                    {
                        m_Actor.Activity = Activity.IDLE;
                        return determinedAction;
                    }
                }
                determinedAction = BehaviorDropBoringEntertainment(game);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.IDLE;
                    return determinedAction;
                }                
            }
            #endregion

            // 18 follow leader
            #region
            if (checkOurLeader)
            {
                Point lastKnownLeaderPosition = m_Actor.Leader.Location.Position;
                bool isLeaderVisible = m_LOSSensor.FOV.Contains(m_Actor.Leader.Location.Position);
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

            // 19 take lead (if leadership)
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
                        determinedAction = BehaviorLeadActor(game, nearestFriend);
                        if (determinedAction != null)
                        {
                            m_Actor.Activity = Activity.IDLE;
                            m_Actor.TargetActor = nearestFriend.Percepted as Actor;
                            return determinedAction;
                        }
                    }
                }
            }
            #endregion

            // 20 if hungry, tear down barricades & push objects.
            #region
            if (game.Rules.IsActorHungry(m_Actor))
            {
                determinedAction = BehaviorAttackBarricade(game);
                if (determinedAction != null)
                {
                    // emote.
                    game.DoEmote(m_Actor, "Open damn it! I know there's food in there!");

                    // go!
                    m_Actor.Activity = Activity.IDLE;
                    return determinedAction;
                }
                if (game.Rules.RollChance(HUNGRY_PUSH_OBJECTS_CHANCE))
                {
                    determinedAction = BehaviorPushNonWalkableObject(game);
                    if (determinedAction != null)
                    {
                        // emote.
                        game.DoEmote(m_Actor, "Where's all the damn food?!");

                        // go!
                        m_Actor.Activity = Activity.IDLE;
                        return determinedAction;
                    }
                }
            }
            #endregion

            // 21 go revive corpse.
            #region
            determinedAction = BehaviorGoReviveCorpse(game, FilterCorpses(mapPercepts)); //@@MP - unused parameter (Release 5-7)
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return determinedAction;
            }
            #endregion

            // 22 use exit.
            #region
            if (game.Rules.RollChance(USE_EXIT_CHANCE))
            {
                determinedAction = BehaviorUseExit(game, UseExitFlags.DONT_BACKTRACK);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.IDLE;
                    return determinedAction;
                }
            }
            #endregion

            // 23 build trap or fortification.
            #region
            if (game.Rules.RollChance(BUILD_TRAP_CHANCE))
            {
                determinedAction = BehaviorBuildTrap(game);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.IDLE;
                    return determinedAction;
                }
            }
            // large fortification.
            if (game.Rules.RollChance(BUILD_LARGE_FORT_CHANCE))
            {
                determinedAction = BehaviorBuildLargeFortification(game, START_FORT_LINE_CHANCE);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.IDLE;
                    return determinedAction;
                }
            }
            // small fortification.
            if (game.Rules.RollChance(BUILD_SMALL_FORT_CHANCE))
            {
                determinedAction = BehaviorBuildSmallFortification(game);
                if (determinedAction != null)
                {
                    m_Actor.Activity = Activity.IDLE;
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
                    m_Actor.Activity = Activity.IDLE;
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
                    m_Actor.Activity = Activity.IDLE;
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
                    m_Actor.Activity = Activity.IDLE;
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
                    m_Actor.Activity = Activity.IDLE;
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
                            if (m_LOSSensor.FOV.Contains(target.Location.Position))
                                game.DoEmote(m_Actor, String.Format("Come on {0}! Hurry up!", target.Name));
                            else
                                game.DoEmote(m_Actor, String.Format("Where the hell is {0}?", target.Name));
                        }
                    }

                    // go!
                    m_Actor.Activity = Activity.IDLE;
                    return determinedAction;
                }
            }
            #endregion

            // 30 explore
            #region
            determinedAction = BehaviorExplore(game, m_Exploration);
            if (determinedAction != null)
            {
                m_Actor.Activity = Activity.IDLE;
                return determinedAction;
            }
            #endregion

            // 31 wander.
            #region
            m_Actor.Activity = Activity.IDLE;
            return BehaviorWander(game);
            #endregion

            #endregion
        }
        #endregion
    }
}
