﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace djack.RogueSurvivor.Engine
{
    [Serializable]
    struct GameOptions
    {
        #region IDs
        public enum IDs
        {
            UI_MUSIC,
            UI_MUSIC_VOLUME,
            UI_SFXS, //@@MP (Release 2)
            UI_SFXS_VOLUME, //@@MP (Release 2)
            UI_SHOW_PLAYER_TAG_ON_MINIMAP,
            UI_ANIM_DELAY,
            UI_SHOW_MINIMAP,
            UI_SHOW_CORPSES,
            UI_ADVISOR,
            UI_COMBAT_ASSISTANT,
            UI_SHOW_TARGETS,
            UI_SHOW_PLAYER_TARGETS,
            
            GAME_DISTRICT_SIZE,
            GAME_MAX_CIVILIANS,
            GAME_MAX_DOGS,
            GAME_MAX_UNDEADS,
            GAME_SIMULATE_DISTRICTS,
            GAME_SIMULATE_SLEEP,
            GAME_SIM_THREAD,
            GAME_SPAWN_SKELETON_CHANCE,
            GAME_SPAWN_ZOMBIE_CHANCE,
            GAME_SPAWN_ZOMBIE_MASTER_CHANCE,
            GAME_CITY_SIZE,
            GAME_NPC_CAN_STARVE_TO_DEATH,
            GAME_ZOMBIFICATION_CHANCE,
            GAME_REVEAL_STARTING_DISTRICT,
            GAME_ALLOW_UNDEADS_EVOLUTION,
            GAME_DAY_ZERO_UNDEADS_PERCENT,
            GAME_ZOMBIE_INVASION_DAILY_INCREASE,
            GAME_STARVED_ZOMBIFICATION_CHANCE,
            GAME_SANITY, //@@MP (Release 1)
            GAME_MAX_REINCARNATIONS,
            GAME_REINCARNATE_AS_RAT,
            GAME_REINCARNATE_TO_SEWERS,
            GAME_REINC_LIVING_RESTRICTED,
            GAME_PERMADEATH,
            GAME_DEATH_SCREENSHOT,
            GAME_AGGRESSIVE_HUNGRY_CIVILIANS,
            GAME_NATGUARD_FACTOR,
            GAME_SUPPLIESDROP_FACTOR,
            GAME_UNDEADS_UPGRADE_DAYS,
            GAME_RATS_UPGRADE,
            GAME_SKELETONS_UPGRADE,
            GAME_SHAMBLERS_UPGRADE
        };
        #endregion

        #region ZUP Days
        public enum ZupDays
        {
            _FIRST = 0,
            ONE = _FIRST,
            TWO,
            THREE,
            FOUR,
            FIVE,
            SIX,
            SEVEN,
            OFF,
            _COUNT
        }
        #endregion

        #region Simulating districts
        public enum SimRatio
        {
            _FIRST = 0,
            OFF = _FIRST,
            ONE_QUARTER,     // 1/4
            ONE_THIRD,       // 1/3
            HALF,            // 1/2
            TWO_THIRDS,      // 2/3
            THREE_QUARTER,   // 3/4
            FULL,
            _COUNT
        }
        #endregion

        #region Reincarnation mode
        public enum ReincMode
        {
            _FIRST = 0,
            RANDOM_FOLLOWER = _FIRST,
            KILLER,
            ZOMBIFIED,
            RANDOM_LIVING,
            RANDOM_UNDEAD,
            RANDOM_ACTOR,
            _LAST = RANDOM_ACTOR,
            _COUNT
        }
        #endregion

        #region Default values
        public const int DEFAULT_DISTRICT_SIZE = 50; 
        public const int DEFAULT_MAX_CIVILIANS = 25;
        public const int DEFAULT_MAX_DOGS = 0;// 5;
        public const int DEFAULT_MAX_UNDEADS = 100; 
        public const int DEFAULT_SPAWN_SKELETON_CHANCE = 60;
        public const int DEFAULT_SPAWN_ZOMBIE_CHANCE = 30;
        public const int DEFAULT_SPAWN_ZOMBIE_MASTER_CHANCE = 10;
        public const int DEFAULT_CITY_SIZE = 5;
        public const SimRatio DEFAULT_SIM_DISTRICTS = SimRatio.FULL;
        public const int DEFAULT_ZOMBIFICATION_CHANCE = 100;
        public const int DEFAULT_DAY_ZERO_UNDEADS_PERCENT = 30;
        public const int DEFAULT_ZOMBIE_INVASION_DAILY_INCREASE = 5;
        public const int DEFAULT_STARVED_ZOMBIFICATION_CHANCE = 0; //@@MP - vanilla = 50 (Release 3)
        public const int DEFAULT_MAX_REINCARNATIONS = 1;
        public const int DEFAULT_NATGUARD_FACTOR = 100;
        public const int DEFAULT_SUPPLIESDROP_FACTOR = 100;
        public const ZupDays DEFAULT_ZOMBIFIEDS_UPGRADE_DAYS = ZupDays.THREE;
        #endregion

        #region Fields
        int m_DistrictSize;
        int m_MaxCivilians;
        int m_MaxDogs;
        int m_MaxUndeads;
        bool m_PlayMusic;
        int m_MusicVolume;
        bool m_PlaySFXs; //@@MP (Release 2)
        int m_SFXVolume; //@@MP (Release 2)
        bool m_AnimDelay;
        bool m_ShowMinimap;
        bool m_ShowCorpses; //@@MP - drop corpses even in standard game mode (Release 1)
        bool m_EnabledAdvisor;
        bool m_CombatAssistant;
        SimRatio m_SimulateDistricts;
        float m_cachedSimRatioFloat;
        bool m_SimulateWhenSleeping;
        bool m_SimThread;
        bool m_ShowPlayerTagsOnMinimap;
        int m_SpawnSkeletonChance;
        int m_SpawnZombieChance;
        int m_SpawnZombieMasterChance;        
        int m_CitySize;
        bool m_NPCCanStarveToDeath;
        int m_ZombificationChance;
        bool m_RevealStartingDistrict;
        bool m_AllowUndeadsEvolution;
        int m_DayZeroUndeadsPercent;
        int m_ZombieInvasionDailyIncrease;
        int m_StarvedZombificationChance;
        bool m_Sanity;  //@@MP (Release 1)
        public static bool m_SanityGlobal; //@@MP - so that BaseTownGenerator.cs can know when Sanity is applicable (Release 1)
        int m_MaxReincarnations;
        bool m_CanReincarnateAsRat;
        bool m_CanReincarnateToSewers;
        bool m_IsLivingReincRestricted;
        bool m_Permadeath;
        bool m_DeathScreenshot;
        bool m_AggressiveHungryCivilians;
        int m_NatGuardFactor;
        int m_SuppliesDropFactor;
        bool m_ShowTargets;
        bool m_ShowPlayerTargets;
        ZupDays m_ZupDays;
        bool m_RatsUpgrade;
        bool m_SkeletonsUpgrade;
        bool m_ShamblersUpgrade;
        #endregion

        #region Properties
        /// <summary>
        /// Disables music
        /// </summary>
        public bool PlayMusic
        {
            get { return m_PlayMusic; }
            set { m_PlayMusic = value; }
        }

        /// <summary>
        /// Volume for music
        /// </summary>
        public int MusicVolume
        {
            get { return m_MusicVolume; }
            set
            {
                if (value < 0) value = 0; 
                if (value > 100) value = 100; 
                m_MusicVolume = value;
            }
        }

        /// <summary>
        /// Disables sound effects
        /// </summary>
        public bool PlaySFXs //@@MP (Release 2)
        {
            get { return m_PlaySFXs; }
            set { m_PlaySFXs = value; }
        }

        /// <summary>
        /// Volume for sound effects
        /// </summary>
        public int SFXVolume //@@MP (Release 2)
        {
            get { return m_SFXVolume; }
            set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;
                m_SFXVolume = value;
            }
        }

        public bool ShowCorpses
        {
            get { return m_ShowCorpses; }
            set { m_ShowCorpses = value; }
        }

        public bool ShowPlayerTagsOnMinimap
        {
            get { return m_ShowPlayerTagsOnMinimap; }
            set { m_ShowPlayerTagsOnMinimap = value; }
        }

        public bool IsAnimDelayOn
        {
            get { return m_AnimDelay; }
            set { m_AnimDelay = value; }
        }

        public bool IsMinimapOn
        {
            get { return m_ShowMinimap; }
            set { m_ShowMinimap = value; }
        }

        public bool IsAdvisorEnabled
        {
            get { return m_EnabledAdvisor; }
            set { m_EnabledAdvisor = value; }
        }

        public bool IsCombatAssistantOn
        {
            get { return m_CombatAssistant; }
            set { m_CombatAssistant = value; }
        }

        public int CitySize
        {
            get { return m_CitySize; }
            set 
            {
                if (value < 3) value = 3;
                if (value > 6) value = 6;
                m_CitySize = value; 
            }
        }

        public int MaxCivilians 
        {
            get { return m_MaxCivilians; }
            set
            {
                if (value < 10) value = 10;
                if (value > 75) value = 75;
                m_MaxCivilians = value;
            }
        }

        public int MaxDogs
        {
            get { return m_MaxDogs; }
            set
            {
                if (value < 0) value = 0;
                if (value > 75) value = 75;
                m_MaxDogs = value;
            }
        }

        public int MaxUndeads 
        {
            get { return m_MaxUndeads; }
            set
            {
                if (value < 10) value = 10;
                if (value > 200) value = 200;
                m_MaxUndeads = value;
            } 
        }

        public int SpawnSkeletonChance
        {
            get { return m_SpawnSkeletonChance; }
            set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;
                m_SpawnSkeletonChance = value;
            }
        }

        public int SpawnZombieChance
        {
            get { return m_SpawnZombieChance; }
            set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;
                m_SpawnZombieChance = value;
            }
        }

        public int SpawnZombieMasterChance
        {
            get { return m_SpawnZombieMasterChance; }
            set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;
                m_SpawnZombieMasterChance = value;
            }
        }

        public SimRatio SimulateDistricts
        {
            get { return m_SimulateDistricts; }
            set
            {
                m_SimulateDistricts = value;
                m_cachedSimRatioFloat = GameOptions.SimRatioToFloat(m_SimulateDistricts);
            }
        }

        public float SimRatioFloat
        {
            get { return m_cachedSimRatioFloat; }
        }

        public bool SimulateWhenSleeping
        {
            get { return m_SimulateWhenSleeping; }
            set { m_SimulateWhenSleeping = value; }
        }

        public bool IsSimON
        {
            get { return m_SimulateDistricts != SimRatio.OFF; }
        }

        public bool SimThread
        {
            get { return m_SimThread; }
            set { m_SimThread = value; }
        }

        public int DistrictSize
        {
            get { return m_DistrictSize; }
            set
            {
                if (value < 30) value = 30;
                if (value > RogueGame.MAP_MAX_HEIGHT || value > RogueGame.MAP_MAX_WIDTH) value = Math.Min(RogueGame.MAP_MAX_WIDTH, RogueGame.MAP_MAX_HEIGHT);
                m_DistrictSize = value;
            }
        }

        public bool NPCCanStarveToDeath
        {
            get { return m_NPCCanStarveToDeath; }
            set { m_NPCCanStarveToDeath = value; }
        }

        public int ZombificationChance
        {
            get { return m_ZombificationChance; }
            set
            {
                if (value < 10) value = 10;
                if (value > 100) value = 100;
                m_ZombificationChance = value;
            }
        }

        public bool RevealStartingDistrict
        {
            get { return m_RevealStartingDistrict; }
            set { m_RevealStartingDistrict = value; }
        }

        public bool AllowUndeadsEvolution
        {
            get { return m_AllowUndeadsEvolution; }
            set { m_AllowUndeadsEvolution = value; }
        }

        public int DayZeroUndeadsPercent
        {
            get { return m_DayZeroUndeadsPercent; }
            set
            {
                if (value < 10) value = 10;
                if (value > 100) value = 100;
                m_DayZeroUndeadsPercent = value;
            }
        }

        public int ZombieInvasionDailyIncrease
        {
            get { return m_ZombieInvasionDailyIncrease; }
            set
            {
                if (value < 1) value = 1;
                if (value > 20) value = 20;
                m_ZombieInvasionDailyIncrease = value;
            }
        }

        public int StarvedZombificationChance
        {
            get { return m_StarvedZombificationChance; }
            set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;
                m_StarvedZombificationChance = value;
            }
        }

        public int MaxReincarnations
        {
            get { return m_MaxReincarnations; }
            set
            {
                if (value < 0) value = 0;
                if (value > 7) value = 7;
                m_MaxReincarnations = value;
            }
        }

        public bool IsSanityEnabled  //@@MP (Release 1)
        {
            get { return m_Sanity; }
            set
            {
                m_Sanity = value;
                m_SanityGlobal = m_Sanity; //@@MP - so that BaseTownGenerator.cs can know when Sanity is applicable
            }
        }

        public bool CanReincarnateAsRat
        {
            get { return m_CanReincarnateAsRat; }
            set { m_CanReincarnateAsRat = value; }
        }

        public bool CanReincarnateToSewers
        {
            get { return m_CanReincarnateToSewers; }
            set { m_CanReincarnateToSewers = value; }
        }

        public bool IsLivingReincRestricted
        {
            get { return m_IsLivingReincRestricted; }
            set { m_IsLivingReincRestricted = value; }
        }

        public bool IsPermadeathOn
        {
            get { return m_Permadeath; }
            set { m_Permadeath = value; }
        }

        public bool IsDeathScreenshotOn
        {
            get { return m_DeathScreenshot; }
            set { m_DeathScreenshot = value; }
        }

        public bool IsAggressiveHungryCiviliansOn
        {
            get { return m_AggressiveHungryCivilians; }
            set { m_AggressiveHungryCivilians = value; }
        }

        public int NatGuardFactor
        {
            get { return m_NatGuardFactor; }
            set
            {
                if (value < 0) value = 0;
                if (value > 200) value = 200;
                m_NatGuardFactor = value;
            }
        }

        public int SuppliesDropFactor
        {
            get { return m_SuppliesDropFactor; }
            set
            {
                if (value < 0) value = 0;
                if (value > 200) value = 200;
                m_SuppliesDropFactor = value;
            }
        }

        public bool ShowTargets
        {
            get { return m_ShowTargets; }
            set { m_ShowTargets = value; }
        }

        public bool ShowPlayerTargets
        {
            get { return m_ShowPlayerTargets; }
            set { m_ShowPlayerTargets = value; }
        }

        public ZupDays ZombifiedsUpgradeDays
        {
            get { return m_ZupDays; }
            set { m_ZupDays = value; }
        }

        public bool RatsUpgrade
        {
            get { return m_RatsUpgrade; }
            set { m_RatsUpgrade = value; }
        }

        public bool SkeletonsUpgrade
        {
            get { return m_SkeletonsUpgrade; }
            set { m_SkeletonsUpgrade = value; }
        }

        public bool ShamblersUpgrade
        {
            get { return m_ShamblersUpgrade; }
            set { m_ShamblersUpgrade = value; }
        }
        #endregion

        #region Dev only options (hidden)
        public bool DEV_ShowActorsStats { get; set; }
        #endregion

        #region Init
        public void ResetToDefaultValues()
        {
            m_DistrictSize = DEFAULT_DISTRICT_SIZE;
            m_MaxCivilians = DEFAULT_MAX_CIVILIANS;
            m_MaxUndeads = DEFAULT_MAX_UNDEADS;
            m_MaxDogs = DEFAULT_MAX_DOGS;
            m_PlayMusic = true;
            m_MusicVolume = 100;
            m_PlaySFXs = true;//@@MP (Release 2)
            m_SFXVolume = 100;//@@MP (Release 2)
            m_AnimDelay = true;
            m_ShowMinimap = true;
            m_ShowPlayerTagsOnMinimap = true;
            m_EnabledAdvisor = true;
            m_CombatAssistant = false;
            this.SimulateDistricts = DEFAULT_SIM_DISTRICTS;
            m_SimulateWhenSleeping = false;
            m_SimThread = true;
            m_SpawnSkeletonChance = DEFAULT_SPAWN_SKELETON_CHANCE;
            m_SpawnZombieChance = DEFAULT_SPAWN_ZOMBIE_CHANCE;
            m_SpawnZombieMasterChance = DEFAULT_SPAWN_ZOMBIE_MASTER_CHANCE;
            m_CitySize = DEFAULT_CITY_SIZE;
            m_NPCCanStarveToDeath = true;
            m_ZombificationChance = DEFAULT_ZOMBIFICATION_CHANCE;
            m_RevealStartingDistrict = true;
            m_AllowUndeadsEvolution = true;
            m_DayZeroUndeadsPercent = DEFAULT_DAY_ZERO_UNDEADS_PERCENT;
            m_ZombieInvasionDailyIncrease = DEFAULT_ZOMBIE_INVASION_DAILY_INCREASE;
            m_StarvedZombificationChance = DEFAULT_STARVED_ZOMBIFICATION_CHANCE;
            m_MaxReincarnations = DEFAULT_MAX_REINCARNATIONS;
            m_Sanity = true;
            m_SanityGlobal = m_Sanity; //@@MP (Release 1)
            m_ShowCorpses = true;
            m_CanReincarnateAsRat = false;
            m_CanReincarnateToSewers = false;
            m_IsLivingReincRestricted = false;
            m_Permadeath = false;
            m_DeathScreenshot = true;
            m_AggressiveHungryCivilians = true;
            m_NatGuardFactor = DEFAULT_NATGUARD_FACTOR;
            m_SuppliesDropFactor = DEFAULT_SUPPLIESDROP_FACTOR;
            m_ShowTargets = true;
            m_ShowPlayerTargets = true;
            m_ZupDays = DEFAULT_ZOMBIFIEDS_UPGRADE_DAYS;
            m_RatsUpgrade = false;
            m_SkeletonsUpgrade = false;
            m_ShamblersUpgrade = false;
        }
        #endregion

        #region Helpers
        public static string Name(IDs option)
        {
            switch (option)
            {
                case IDs.GAME_AGGRESSIVE_HUNGRY_CIVILIANS:      return "(Living) Aggressive Hungry Civilians";
                case IDs.GAME_ALLOW_UNDEADS_EVOLUTION:          return "(Undead) Allow Undeads Evolution";
                case IDs.GAME_CITY_SIZE:                        return "   (Map) City Size";
                case IDs.GAME_DAY_ZERO_UNDEADS_PERCENT:         return "(Undead) Day 0 Undeads";
                case IDs.GAME_DEATH_SCREENSHOT:                 return " (Death) Death Screenshot";
                case IDs.GAME_DISTRICT_SIZE:                    return "   (Map) District Map Size";
                case IDs.GAME_MAX_CIVILIANS:                    return "(Living) Max Civilians";
                case IDs.GAME_MAX_DOGS:                         return "(Living) Max Dogs";
                case IDs.GAME_MAX_REINCARNATIONS:               return " (Reinc) Max Reincarnations";
                case IDs.GAME_MAX_UNDEADS:                      return "(Undead) Max Undeads";
                case IDs.GAME_NATGUARD_FACTOR:                  return " (Event) National Guard";
                case IDs.GAME_NPC_CAN_STARVE_TO_DEATH:          return "(Living) NPCs can Starve to death";
                case IDs.GAME_PERMADEATH:                       return " (Death) Permadeath";
                case IDs.GAME_RATS_UPGRADE:                     return "(Undead) Rats Skill Upgrade";
                case IDs.GAME_REVEAL_STARTING_DISTRICT:         return "   (Map) Reveal Starting District";
                case IDs.GAME_REINC_LIVING_RESTRICTED:          return " (Reinc) Civilians only Reinc.";
                case IDs.GAME_REINCARNATE_AS_RAT:               return " (Reinc) Can Reincarnate as Rat";
                case IDs.GAME_REINCARNATE_TO_SEWERS:            return " (Reinc) Can Reincarnate to Sewers";
                case IDs.GAME_SHAMBLERS_UPGRADE:                return "(Undead) Shamblers Skill Upgrade";
                case IDs.GAME_SKELETONS_UPGRADE:                return "(Undead) Skeletons Skill Upgrade";
                case IDs.GAME_SIMULATE_DISTRICTS:               return "   (Sim) Districts Simulation";
                case IDs.GAME_SIMULATE_SLEEP:                   return "   (Sim) Simulate when Sleeping";
                case IDs.GAME_SIM_THREAD:                       return "   (Sim) Synchronous Simulation";
                case IDs.GAME_SPAWN_SKELETON_CHANCE:            return "(Undead) Spawn Skeleton chance";
                case IDs.GAME_SPAWN_ZOMBIE_CHANCE:              return "(Undead) Spawn Zombie chance";
                case IDs.GAME_SPAWN_ZOMBIE_MASTER_CHANCE:       return "(Undead) Spawn Zombie Master chance";
                case IDs.GAME_STARVED_ZOMBIFICATION_CHANCE:     return "(Living) Zombify if Starved Chance (STD)";
                case IDs.GAME_SUPPLIESDROP_FACTOR:              return " (Event) Supplies Drop";
                case IDs.GAME_UNDEADS_UPGRADE_DAYS:             return "(Undead) Undeads Skills Upgrade Days";
                case IDs.GAME_ZOMBIFICATION_CHANCE:             return "(Living) Zombification Chance (C&I, VTG)";
                case IDs.GAME_SANITY:                           return "(Living) Sanity Loss"; //@@MP (Release 1)
                case IDs.GAME_ZOMBIE_INVASION_DAILY_INCREASE:   return "(Undead) Invasion Daily Increase";
                case IDs.UI_ANIM_DELAY:                         return "   (Gfx) Animations Delay";
                case IDs.UI_MUSIC:                              return "   (Sfx) Music";
                case IDs.UI_MUSIC_VOLUME:                       return "   (Sfx) Music Volume";
                case IDs.UI_SFXS:                               return "   (Sfx) Sound Effects"; //@@MP (Release 2)
                case IDs.UI_SFXS_VOLUME:                        return "   (Sfx) Sound Effects Volume"; //@@MP (Release 2)
                case IDs.UI_SHOW_MINIMAP:                       return "   (Gfx) Show Minimap";
                case IDs.UI_SHOW_PLAYER_TAG_ON_MINIMAP:         return "   (Gfx) Show Tags on Minimap";
                case IDs.UI_SHOW_CORPSES:                       return "   (Gfx) Show Corpses in STD Game Mode";
                case IDs.UI_ADVISOR:                            return "  (Help) Enable Advisor";
                case IDs.UI_COMBAT_ASSISTANT:                   return "  (Help) Combat Assistant";
                case IDs.UI_SHOW_TARGETS:                       return "  (Help) Show Actor Targets";
                case IDs.UI_SHOW_PLAYER_TARGETS:                return "  (Help) Always Show Player Targets";

                default:
                    throw new ArgumentOutOfRangeException("unhandled option");
            }
        }

        public static string Name(ReincMode mode)
        {
            switch (mode)
            {
                case ReincMode.RANDOM_ACTOR: return "Random Actor";
                case ReincMode.RANDOM_LIVING: return "Random Living";
                case ReincMode.RANDOM_UNDEAD: return "Random Undead";
                case ReincMode.RANDOM_FOLLOWER: return "Random Follower";
                case ReincMode.KILLER: return "Your Killer";
                case ReincMode.ZOMBIFIED: return "Your Zombie Self";
                default:
                    throw new ArgumentOutOfRangeException("unhandled ReincMode");
            }
        }

        public static string Name(SimRatio ratio)
        {
            switch (ratio)
            {
                case SimRatio.OFF: return "OFF";
                case SimRatio.ONE_QUARTER: return "25%";
                case SimRatio.ONE_THIRD: return "33%";
                case SimRatio.HALF: return "50%";
                case SimRatio.TWO_THIRDS: return "66%";
                case SimRatio.THREE_QUARTER: return "75%";
                case SimRatio.FULL: return "FULL";
                default:
                    throw new ArgumentOutOfRangeException("unhandled simRatio");
            }

        }

        public static float SimRatioToFloat(SimRatio ratio)
        {
            switch (ratio)
            {
                case SimRatio.OFF: return 0f;
                case SimRatio.ONE_QUARTER: return 1f / 4f;
                case SimRatio.ONE_THIRD: return 1f / 3f;
                case SimRatio.HALF: return 1f / 2f;
                case SimRatio.TWO_THIRDS: return 2f / 3f;
                case SimRatio.THREE_QUARTER: return 3f / 4f;
                case SimRatio.FULL: return 1f;
                default:
                    throw new ArgumentOutOfRangeException("unhandled simRatio");
            }
        }

        public static string Name(ZupDays d)
        {
            switch (d)
            {
                case ZupDays.OFF: return "OFF";
                case ZupDays.ONE: return "1 d";
                case ZupDays.TWO: return "2 d";
                case ZupDays.THREE: return "3 d";
                case ZupDays.FOUR: return "4 d";
                case ZupDays.FIVE: return "5 d";
                case ZupDays.SIX: return "6 d";
                case ZupDays.SEVEN: return "7 d";
                default:
                    throw new ArgumentOutOfRangeException("unhandled zupDays");
            }
        }

        public static bool IsZupDay(ZupDays d, int day)
        {
            switch (d)
            {
                case ZupDays.ONE: return true;
                case ZupDays.TWO: return day % 2 == 0;
                case ZupDays.THREE: return day % 3 == 0;
                case ZupDays.FOUR: return day % 4 == 0;
                case ZupDays.FIVE: return day % 5 == 0;
                case ZupDays.SIX: return day % 6 == 0;
                case ZupDays.SEVEN: return day % 7 == 0;
                case ZupDays.OFF: 
                default:
                    return false;
            }
        }

        public string DescribeValue(GameMode mode, IDs option)
        {
            switch (option)
            {
                case IDs.GAME_AGGRESSIVE_HUNGRY_CIVILIANS:
                    return IsAggressiveHungryCiviliansOn ? "ON    (default ON)" : "OFF   (default ON)";
                case IDs.GAME_ALLOW_UNDEADS_EVOLUTION:
                    if (mode == GameMode.GM_VINTAGE)
                        return "---";
                    else
                        return AllowUndeadsEvolution ? "YES   (default YES)" : "NO    (default YES)";
                case IDs.GAME_CITY_SIZE:
                    return String.Format("{0:D2}*   (default {1:D2})", CitySize, GameOptions.DEFAULT_CITY_SIZE);
                case IDs.GAME_DAY_ZERO_UNDEADS_PERCENT:
                    return String.Format("{0:D3}%  (default {1:D3}%)", DayZeroUndeadsPercent, GameOptions.DEFAULT_DAY_ZERO_UNDEADS_PERCENT);
                case IDs.GAME_DEATH_SCREENSHOT:
                    return IsDeathScreenshotOn ? "YES   (default YES)" : "NO    (default YES)";
                case IDs.GAME_DISTRICT_SIZE:
                    return String.Format("{0:D2}*   (default {1:D2})", DistrictSize, GameOptions.DEFAULT_DISTRICT_SIZE);
                case IDs.GAME_MAX_CIVILIANS:
                    return String.Format("{0:D3}*  (default {1:D3})", MaxCivilians, GameOptions.DEFAULT_MAX_CIVILIANS);
                case IDs.GAME_MAX_DOGS:
                    return String.Format("{0:D3}*  (default {1:D3})", MaxDogs, GameOptions.DEFAULT_MAX_DOGS);
                case IDs.GAME_MAX_REINCARNATIONS:
                    return String.Format("{0:D3}   (default {1:D3})", MaxReincarnations, GameOptions.DEFAULT_MAX_REINCARNATIONS);
                case IDs.GAME_MAX_UNDEADS:
                    return String.Format("{0:D3}*  (default {1:D3})", MaxUndeads, GameOptions.DEFAULT_MAX_UNDEADS);
                case IDs.GAME_NATGUARD_FACTOR:
                    return String.Format("{0:D3}%  (default {1:D3}%)", NatGuardFactor, GameOptions.DEFAULT_NATGUARD_FACTOR);
                case IDs.GAME_NPC_CAN_STARVE_TO_DEATH:
                    return NPCCanStarveToDeath ? "YES   (default YES)" : "NO    (default YES)";
                case IDs.GAME_PERMADEATH:
                    return IsPermadeathOn ? "YES   (default NO)" : "NO    (default NO)";
                case IDs.GAME_RATS_UPGRADE:
                    if (mode == GameMode.GM_VINTAGE)
                        return "---";
                    else
                        return RatsUpgrade ? "YES   (default NO)" : "NO    (default NO)";
                case IDs.GAME_REINC_LIVING_RESTRICTED:
                    return IsLivingReincRestricted ? "YES   (default NO)" : "NO    (default NO)";
                case IDs.GAME_REINCARNATE_AS_RAT:
                    return CanReincarnateAsRat ? "YES   (default NO)" : "NO    (default NO)";
                case IDs.GAME_REINCARNATE_TO_SEWERS:
                    return CanReincarnateToSewers ? "YES   (default NO)" : "NO    (default NO)";
                case IDs.GAME_REVEAL_STARTING_DISTRICT:
                    return RevealStartingDistrict ? "YES   (default YES)" : "NO    (default YES)";
                case IDs.GAME_SANITY://@@MP (Release 1)
                    return IsSanityEnabled ? "ON    (default ON)" : "OFF   (default ON)";
                case IDs.GAME_SHAMBLERS_UPGRADE:
                    if (mode == GameMode.GM_VINTAGE)
                        return "---";
                    else
                        return ShamblersUpgrade ? "YES   (default NO)" : "NO    (default NO)";
                case IDs.GAME_SKELETONS_UPGRADE:
                    if (mode == GameMode.GM_VINTAGE)
                        return "---";
                    else
                        return SkeletonsUpgrade ? "YES   (default NO)" : "NO    (default NO)";
                case IDs.GAME_SIM_THREAD:
                    return SimThread ? "YES*  (default YES)" : "NO*   (default YES)";
                case IDs.GAME_SIMULATE_DISTRICTS:
                    return String.Format("{0,-4}* (default {1})", GameOptions.Name(SimulateDistricts), GameOptions.Name(GameOptions.DEFAULT_SIM_DISTRICTS));
                case IDs.GAME_SIMULATE_SLEEP:
                    return SimulateWhenSleeping ? "YES*  (default NO)" : "NO*   (default NO)";
                case IDs.GAME_STARVED_ZOMBIFICATION_CHANCE:
                    return String.Format("{0:D3}%  (default {1:D3}%)", StarvedZombificationChance, GameOptions.DEFAULT_STARVED_ZOMBIFICATION_CHANCE);
                case IDs.GAME_SUPPLIESDROP_FACTOR:
                    return String.Format("{0:D3}%  (default {1:D3}%)", SuppliesDropFactor, GameOptions.DEFAULT_SUPPLIESDROP_FACTOR);
                case IDs.GAME_ZOMBIE_INVASION_DAILY_INCREASE:
                    return String.Format("{0:D3}%  (default {1:D3}%)", ZombieInvasionDailyIncrease, GameOptions.DEFAULT_ZOMBIE_INVASION_DAILY_INCREASE);
                case IDs.GAME_ZOMBIFICATION_CHANCE:
                    return String.Format("{0:D3}%  (default {1:D3}%)", ZombificationChance, GameOptions.DEFAULT_ZOMBIFICATION_CHANCE);
                case IDs.GAME_UNDEADS_UPGRADE_DAYS:
                    return String.Format("{0:D3}   (default {1:D3})", GameOptions.Name(ZombifiedsUpgradeDays), GameOptions.Name(GameOptions.DEFAULT_ZOMBIFIEDS_UPGRADE_DAYS));
                case IDs.UI_ADVISOR:
                    return IsAdvisorEnabled ? "YES" : "NO ";
                case IDs.UI_ANIM_DELAY:
                    return IsAnimDelayOn ? "ON " : "OFF";
                case IDs.UI_COMBAT_ASSISTANT:
                    return IsCombatAssistantOn ? "ON    (default OFF)" : "OFF   (default OFF)";
                case IDs.UI_MUSIC:
                    return PlayMusic ? "ON " : "OFF";
                case IDs.UI_MUSIC_VOLUME:
                    return MusicVolume.ToString() + "%";
                case IDs.UI_SFXS: //@@MP (Release 2)
                    return PlaySFXs ? "ON " : "OFF";
                case IDs.UI_SFXS_VOLUME: //@@MP (Release 2)
                    return SFXVolume.ToString() + "%";
                case IDs.UI_SHOW_MINIMAP:
                    return IsMinimapOn ? "ON " : "OFF";
                case IDs.UI_SHOW_CORPSES:
                    return ShowCorpses ? "ON*   (default ON)" : "OFF*  (default ON)";
                case IDs.UI_SHOW_PLAYER_TAG_ON_MINIMAP:
                    return ShowPlayerTagsOnMinimap ? "YES" : "NO ";
                case IDs.UI_SHOW_PLAYER_TARGETS:
                    return ShowPlayerTargets ? "ON    (default ON)" : "OFF   (default ON)";
                case IDs.UI_SHOW_TARGETS:
                    return ShowTargets ? "ON    (default ON)" : "OFF   (default ON)";
                default:
                    return "???";
            }
        }
        #endregion

        #region Saving & Loading
        public static void Save(GameOptions options, string filepath)
        {
            if (filepath == null)
                throw new ArgumentNullException("filepath");

            Logger.WriteLine(Logger.Stage.RUN_MAIN, "saving options...");

            IFormatter formatter = CreateFormatter();
            Stream stream = CreateStream(filepath, true);

            formatter.Serialize(stream, options);
            stream.Flush();
            stream.Close();

            Logger.WriteLine(Logger.Stage.RUN_MAIN, "saving options... done!");
        }

        /// <summary>
        /// Try to load, null if failed.
        /// </summary>
        /// <returns></returns>
        public static GameOptions Load(string filepath)
        {
            if (filepath == null)
                throw new ArgumentNullException("filepath");

            Logger.WriteLine(Logger.Stage.RUN_MAIN, "loading options...");

            GameOptions options;
            try
            {
                IFormatter formatter = CreateFormatter();
                Stream stream = CreateStream(filepath, false);

                options = (GameOptions)formatter.Deserialize(stream);
                stream.Close();
                /*//@@MP - switch the global Sanity flag, so we don't need to save it to config.dat (Release 1), commented out and moved to RogueGame.ApplyOptions() (Release 2)
                if (options.IsSanityEnabled)
                    m_SanityGlobal = true;
                else
                    m_SanityGlobal = false;*/
            }
            catch (Exception e)
            {
                Logger.WriteLine(Logger.Stage.RUN_MAIN, "failed to load options (no custom options?).");

                Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("load exception : {0}.", e.ToString()));

                Logger.WriteLine(Logger.Stage.RUN_MAIN, "returning default values.");
                options = new GameOptions();
                options.ResetToDefaultValues();
            }

            Logger.WriteLine(Logger.Stage.RUN_MAIN, "loading options... done!");
            return options;
        }

        static IFormatter CreateFormatter()
        {
            return new BinaryFormatter();
        }

        static Stream CreateStream(string saveFileName, bool save)
        {
            return new FileStream(saveFileName,
                save ? FileMode.Create : FileMode.Open,
                save ? FileAccess.Write : FileAccess.Read,
                FileShare.None);
        }
        #endregion
    }
}
