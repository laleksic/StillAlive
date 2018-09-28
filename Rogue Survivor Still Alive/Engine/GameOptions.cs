﻿// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.GameOptions
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace djack.RogueSurvivor.Engine
{
  [Serializable]
  internal struct GameOptions
  {
    public const int DEFAULT_DISTRICT_SIZE = 50;
    public const int DEFAULT_MAX_CIVILIANS = 25;
    public const int DEFAULT_MAX_DOGS = 0;
    public const int DEFAULT_MAX_UNDEADS = 100;
    public const int DEFAULT_SPAWN_SKELETON_CHANCE = 60;
    public const int DEFAULT_SPAWN_ZOMBIE_CHANCE = 30;
    public const int DEFAULT_SPAWN_ZOMBIE_MASTER_CHANCE = 10;
    public const int DEFAULT_CITY_SIZE = 5;
    public const GameOptions.SimRatio DEFAULT_SIM_DISTRICTS = GameOptions.SimRatio.FULL;
    public const int DEFAULT_ZOMBIFICATION_CHANCE = 100;
    public const int DEFAULT_DAY_ZERO_UNDEADS_PERCENT = 30;
    public const int DEFAULT_ZOMBIE_INVASION_DAILY_INCREASE = 5;
    public const int DEFAULT_STARVED_ZOMBIFICATION_CHANCE = 50;
    public const int DEFAULT_MAX_REINCARNATIONS = 1;
    public const int DEFAULT_NATGUARD_FACTOR = 100;
    public const int DEFAULT_SUPPLIESDROP_FACTOR = 100;
    public const GameOptions.ZupDays DEFAULT_ZOMBIFIEDS_UPGRADE_DAYS = GameOptions.ZupDays.THREE;
    private int m_DistrictSize;
    private int m_MaxCivilians;
    private int m_MaxDogs;
    private int m_MaxUndeads;
    private bool m_PlayMusic;
    private int m_MusicVolume;
    private bool m_AnimDelay;
    private bool m_ShowMinimap;
    private bool m_ShowCorpses;
    private bool m_EnabledAdvisor;
    private bool m_CombatAssistant;
    private GameOptions.SimRatio m_SimulateDistricts;
    private float m_cachedSimRatioFloat;
    private bool m_SimulateWhenSleeping;
    private bool m_SimThread;
    private bool m_ShowPlayerTagsOnMinimap;
    private int m_SpawnSkeletonChance;
    private int m_SpawnZombieChance;
    private int m_SpawnZombieMasterChance;
    private int m_CitySize;
    private bool m_NPCCanStarveToDeath;
    private int m_ZombificationChance;
    private bool m_RevealStartingDistrict;
    private bool m_AllowUndeadsEvolution;
    private int m_DayZeroUndeadsPercent;
    private int m_ZombieInvasionDailyIncrease;
    private int m_StarvedZombificationChance;
    private bool m_Sanity;
    public static bool m_SanityGlobal;
    private int m_MaxReincarnations;
    private bool m_CanReincarnateAsRat;
    private bool m_CanReincarnateToSewers;
    private bool m_IsLivingReincRestricted;
    private bool m_Permadeath;
    private bool m_DeathScreenshot;
    private bool m_AggressiveHungryCivilians;
    private int m_NatGuardFactor;
    private int m_SuppliesDropFactor;
    private bool m_ShowTargets;
    private bool m_ShowPlayerTargets;
    private GameOptions.ZupDays m_ZupDays;
    private bool m_RatsUpgrade;
    private bool m_SkeletonsUpgrade;
    private bool m_ShamblersUpgrade;

    public bool PlayMusic
    {
      get
      {
        return this.m_PlayMusic;
      }
      set
      {
        this.m_PlayMusic = value;
      }
    }

    public int MusicVolume
    {
      get
      {
        return this.m_MusicVolume;
      }
      set
      {
        if (value < 0)
          value = 0;
        if (value > 100)
          value = 100;
        this.m_MusicVolume = value;
      }
    }

    public bool ShowCorpses
    {
      get
      {
        return this.m_ShowCorpses;
      }
      set
      {
        this.m_ShowCorpses = value;
      }
    }

    public bool ShowPlayerTagsOnMinimap
    {
      get
      {
        return this.m_ShowPlayerTagsOnMinimap;
      }
      set
      {
        this.m_ShowPlayerTagsOnMinimap = value;
      }
    }

    public bool IsAnimDelayOn
    {
      get
      {
        return this.m_AnimDelay;
      }
      set
      {
        this.m_AnimDelay = value;
      }
    }

    public bool IsMinimapOn
    {
      get
      {
        return this.m_ShowMinimap;
      }
      set
      {
        this.m_ShowMinimap = value;
      }
    }

    public bool IsAdvisorEnabled
    {
      get
      {
        return this.m_EnabledAdvisor;
      }
      set
      {
        this.m_EnabledAdvisor = value;
      }
    }

    public bool IsCombatAssistantOn
    {
      get
      {
        return this.m_CombatAssistant;
      }
      set
      {
        this.m_CombatAssistant = value;
      }
    }

    public int CitySize
    {
      get
      {
        return this.m_CitySize;
      }
      set
      {
        if (value < 3)
          value = 3;
        if (value > 6)
          value = 6;
        this.m_CitySize = value;
      }
    }

    public int MaxCivilians
    {
      get
      {
        return this.m_MaxCivilians;
      }
      set
      {
        if (value < 10)
          value = 10;
        if (value > 75)
          value = 75;
        this.m_MaxCivilians = value;
      }
    }

    public int MaxDogs
    {
      get
      {
        return this.m_MaxDogs;
      }
      set
      {
        if (value < 0)
          value = 0;
        if (value > 75)
          value = 75;
        this.m_MaxDogs = value;
      }
    }

    public int MaxUndeads
    {
      get
      {
        return this.m_MaxUndeads;
      }
      set
      {
        if (value < 10)
          value = 10;
        if (value > 200)
          value = 200;
        this.m_MaxUndeads = value;
      }
    }

    public int SpawnSkeletonChance
    {
      get
      {
        return this.m_SpawnSkeletonChance;
      }
      set
      {
        if (value < 0)
          value = 0;
        if (value > 100)
          value = 100;
        this.m_SpawnSkeletonChance = value;
      }
    }

    public int SpawnZombieChance
    {
      get
      {
        return this.m_SpawnZombieChance;
      }
      set
      {
        if (value < 0)
          value = 0;
        if (value > 100)
          value = 100;
        this.m_SpawnZombieChance = value;
      }
    }

    public int SpawnZombieMasterChance
    {
      get
      {
        return this.m_SpawnZombieMasterChance;
      }
      set
      {
        if (value < 0)
          value = 0;
        if (value > 100)
          value = 100;
        this.m_SpawnZombieMasterChance = value;
      }
    }

    public GameOptions.SimRatio SimulateDistricts
    {
      get
      {
        return this.m_SimulateDistricts;
      }
      set
      {
        this.m_SimulateDistricts = value;
        this.m_cachedSimRatioFloat = GameOptions.SimRatioToFloat(this.m_SimulateDistricts);
      }
    }

    public float SimRatioFloat
    {
      get
      {
        return this.m_cachedSimRatioFloat;
      }
    }

    public bool SimulateWhenSleeping
    {
      get
      {
        return this.m_SimulateWhenSleeping;
      }
      set
      {
        this.m_SimulateWhenSleeping = value;
      }
    }

    public bool IsSimON
    {
      get
      {
        return (uint) this.m_SimulateDistricts > 0U;
      }
    }

    public bool SimThread
    {
      get
      {
        return this.m_SimThread;
      }
      set
      {
        this.m_SimThread = value;
      }
    }

    public int DistrictSize
    {
      get
      {
        return this.m_DistrictSize;
      }
      set
      {
        if (value < 30)
          value = 30;
        if (value > 100 || value > 100)
          value = Math.Min(100, 100);
        this.m_DistrictSize = value;
      }
    }

    public bool NPCCanStarveToDeath
    {
      get
      {
        return this.m_NPCCanStarveToDeath;
      }
      set
      {
        this.m_NPCCanStarveToDeath = value;
      }
    }

    public int ZombificationChance
    {
      get
      {
        return this.m_ZombificationChance;
      }
      set
      {
        if (value < 10)
          value = 10;
        if (value > 100)
          value = 100;
        this.m_ZombificationChance = value;
      }
    }

    public bool RevealStartingDistrict
    {
      get
      {
        return this.m_RevealStartingDistrict;
      }
      set
      {
        this.m_RevealStartingDistrict = value;
      }
    }

    public bool AllowUndeadsEvolution
    {
      get
      {
        return this.m_AllowUndeadsEvolution;
      }
      set
      {
        this.m_AllowUndeadsEvolution = value;
      }
    }

    public int DayZeroUndeadsPercent
    {
      get
      {
        return this.m_DayZeroUndeadsPercent;
      }
      set
      {
        if (value < 10)
          value = 10;
        if (value > 100)
          value = 100;
        this.m_DayZeroUndeadsPercent = value;
      }
    }

    public int ZombieInvasionDailyIncrease
    {
      get
      {
        return this.m_ZombieInvasionDailyIncrease;
      }
      set
      {
        if (value < 1)
          value = 1;
        if (value > 20)
          value = 20;
        this.m_ZombieInvasionDailyIncrease = value;
      }
    }

    public int StarvedZombificationChance
    {
      get
      {
        return this.m_StarvedZombificationChance;
      }
      set
      {
        if (value < 0)
          value = 0;
        if (value > 100)
          value = 100;
        this.m_StarvedZombificationChance = value;
      }
    }

    public int MaxReincarnations
    {
      get
      {
        return this.m_MaxReincarnations;
      }
      set
      {
        if (value < 0)
          value = 0;
        if (value > 7)
          value = 7;
        this.m_MaxReincarnations = value;
      }
    }

    public bool IsSanityEnabled
    {
      get
      {
        return this.m_Sanity;
      }
      set
      {
        this.m_Sanity = value;
        GameOptions.m_SanityGlobal = this.m_Sanity;
      }
    }

    public bool CanReincarnateAsRat
    {
      get
      {
        return this.m_CanReincarnateAsRat;
      }
      set
      {
        this.m_CanReincarnateAsRat = value;
      }
    }

    public bool CanReincarnateToSewers
    {
      get
      {
        return this.m_CanReincarnateToSewers;
      }
      set
      {
        this.m_CanReincarnateToSewers = value;
      }
    }

    public bool IsLivingReincRestricted
    {
      get
      {
        return this.m_IsLivingReincRestricted;
      }
      set
      {
        this.m_IsLivingReincRestricted = value;
      }
    }

    public bool IsPermadeathOn
    {
      get
      {
        return this.m_Permadeath;
      }
      set
      {
        this.m_Permadeath = value;
      }
    }

    public bool IsDeathScreenshotOn
    {
      get
      {
        return this.m_DeathScreenshot;
      }
      set
      {
        this.m_DeathScreenshot = value;
      }
    }

    public bool IsAggressiveHungryCiviliansOn
    {
      get
      {
        return this.m_AggressiveHungryCivilians;
      }
      set
      {
        this.m_AggressiveHungryCivilians = value;
      }
    }

    public int NatGuardFactor
    {
      get
      {
        return this.m_NatGuardFactor;
      }
      set
      {
        if (value < 0)
          value = 0;
        if (value > 200)
          value = 200;
        this.m_NatGuardFactor = value;
      }
    }

    public int SuppliesDropFactor
    {
      get
      {
        return this.m_SuppliesDropFactor;
      }
      set
      {
        if (value < 0)
          value = 0;
        if (value > 200)
          value = 200;
        this.m_SuppliesDropFactor = value;
      }
    }

    public bool ShowTargets
    {
      get
      {
        return this.m_ShowTargets;
      }
      set
      {
        this.m_ShowTargets = value;
      }
    }

    public bool ShowPlayerTargets
    {
      get
      {
        return this.m_ShowPlayerTargets;
      }
      set
      {
        this.m_ShowPlayerTargets = value;
      }
    }

    public GameOptions.ZupDays ZombifiedsUpgradeDays
    {
      get
      {
        return this.m_ZupDays;
      }
      set
      {
        this.m_ZupDays = value;
      }
    }

    public bool RatsUpgrade
    {
      get
      {
        return this.m_RatsUpgrade;
      }
      set
      {
        this.m_RatsUpgrade = value;
      }
    }

    public bool SkeletonsUpgrade
    {
      get
      {
        return this.m_SkeletonsUpgrade;
      }
      set
      {
        this.m_SkeletonsUpgrade = value;
      }
    }

    public bool ShamblersUpgrade
    {
      get
      {
        return this.m_ShamblersUpgrade;
      }
      set
      {
        this.m_ShamblersUpgrade = value;
      }
    }

    public bool DEV_ShowActorsStats { get; set; }

    public void ResetToDefaultValues()
    {
      this.m_DistrictSize = 50;
      this.m_MaxCivilians = 25;
      this.m_MaxUndeads = 100;
      this.m_MaxDogs = 0;
      this.m_PlayMusic = true;
      this.m_MusicVolume = 100;
      this.m_AnimDelay = true;
      this.m_ShowMinimap = true;
      this.m_ShowPlayerTagsOnMinimap = true;
      this.m_EnabledAdvisor = true;
      this.m_CombatAssistant = false;
      this.SimulateDistricts = GameOptions.SimRatio.FULL;
      this.m_SimulateWhenSleeping = false;
      this.m_SimThread = true;
      this.m_SpawnSkeletonChance = 60;
      this.m_SpawnZombieChance = 30;
      this.m_SpawnZombieMasterChance = 10;
      this.m_CitySize = 5;
      this.m_NPCCanStarveToDeath = true;
      this.m_ZombificationChance = 100;
      this.m_RevealStartingDistrict = true;
      this.m_AllowUndeadsEvolution = true;
      this.m_DayZeroUndeadsPercent = 30;
      this.m_ZombieInvasionDailyIncrease = 5;
      this.m_StarvedZombificationChance = 50;
      this.m_MaxReincarnations = 1;
      this.m_Sanity = true;
      GameOptions.m_SanityGlobal = this.m_Sanity;
      this.m_ShowCorpses = true;
      this.m_CanReincarnateAsRat = false;
      this.m_CanReincarnateToSewers = false;
      this.m_IsLivingReincRestricted = false;
      this.m_Permadeath = false;
      this.m_DeathScreenshot = true;
      this.m_AggressiveHungryCivilians = true;
      this.m_NatGuardFactor = 100;
      this.m_SuppliesDropFactor = 100;
      this.m_ShowTargets = true;
      this.m_ShowPlayerTargets = true;
      this.m_ZupDays = GameOptions.ZupDays.THREE;
      this.m_RatsUpgrade = false;
      this.m_SkeletonsUpgrade = false;
      this.m_ShamblersUpgrade = false;
    }

    public static string Name(GameOptions.IDs option)
    {
      switch (option)
      {
        case GameOptions.IDs.UI_MUSIC:
          return "   (Sfx) Music";
        case GameOptions.IDs.UI_MUSIC_VOLUME:
          return "   (Sfx) Music Volume";
        case GameOptions.IDs.UI_SHOW_PLAYER_TAG_ON_MINIMAP:
          return "   (Gfx) Show Tags on Minimap";
        case GameOptions.IDs.UI_ANIM_DELAY:
          return "   (Gfx) Animations Delay";
        case GameOptions.IDs.UI_SHOW_MINIMAP:
          return "   (Gfx) Show Minimap";
        case GameOptions.IDs.UI_SHOW_CORPSES:
          return "   (Gfx) Show Corpses in STD Game Mode";
        case GameOptions.IDs.UI_ADVISOR:
          return "  (Help) Enable Advisor";
        case GameOptions.IDs.UI_COMBAT_ASSISTANT:
          return "  (Help) Combat Assistant";
        case GameOptions.IDs.UI_SHOW_TARGETS:
          return "  (Help) Show Actor Targets";
        case GameOptions.IDs.UI_SHOW_PLAYER_TARGETS:
          return "  (Help) Always Show Player Targets";
        case GameOptions.IDs.GAME_DISTRICT_SIZE:
          return "   (Map) District Map Size";
        case GameOptions.IDs.GAME_MAX_CIVILIANS:
          return "(Living) Max Civilians";
        case GameOptions.IDs.GAME_MAX_DOGS:
          return "(Living) Max Dogs";
        case GameOptions.IDs.GAME_MAX_UNDEADS:
          return "(Undead) Max Undeads";
        case GameOptions.IDs.GAME_SIMULATE_DISTRICTS:
          return "   (Sim) Districts Simulation";
        case GameOptions.IDs.GAME_SIMULATE_SLEEP:
          return "   (Sim) Simulate when Sleeping";
        case GameOptions.IDs.GAME_SIM_THREAD:
          return "   (Sim) Synchronous Simulation";
        case GameOptions.IDs.GAME_SPAWN_SKELETON_CHANCE:
          return "(Undead) Spawn Skeleton chance";
        case GameOptions.IDs.GAME_SPAWN_ZOMBIE_CHANCE:
          return "(Undead) Spawn Zombie chance";
        case GameOptions.IDs.GAME_SPAWN_ZOMBIE_MASTER_CHANCE:
          return "(Undead) Spawn Zombie Master chance";
        case GameOptions.IDs.GAME_CITY_SIZE:
          return "   (Map) City Size";
        case GameOptions.IDs.GAME_NPC_CAN_STARVE_TO_DEATH:
          return "(Living) NPCs can starve to death";
        case GameOptions.IDs.GAME_ZOMBIFICATION_CHANCE:
          return "(Living) Zombification Chance";
        case GameOptions.IDs.GAME_REVEAL_STARTING_DISTRICT:
          return "   (Map) Reveal Starting District";
        case GameOptions.IDs.GAME_ALLOW_UNDEADS_EVOLUTION:
          return "(Undead) Allow Undeads Evolution";
        case GameOptions.IDs.GAME_DAY_ZERO_UNDEADS_PERCENT:
          return "(Undead) Day 0 Undeads";
        case GameOptions.IDs.GAME_ZOMBIE_INVASION_DAILY_INCREASE:
          return "(Undead) Invasion Daily Increase";
        case GameOptions.IDs.GAME_STARVED_ZOMBIFICATION_CHANCE:
          return "(Living) Starved Zombification";
        case GameOptions.IDs.GAME_SANITY:
          return "(Living) Sanity Loss";
        case GameOptions.IDs.GAME_MAX_REINCARNATIONS:
          return " (Reinc) Max Reincarnations";
        case GameOptions.IDs.GAME_REINCARNATE_AS_RAT:
          return " (Reinc) Can Reincarnate as Rat";
        case GameOptions.IDs.GAME_REINCARNATE_TO_SEWERS:
          return " (Reinc) Can Reincarnate to Sewers";
        case GameOptions.IDs.GAME_REINC_LIVING_RESTRICTED:
          return " (Reinc) Civilians only Reinc.";
        case GameOptions.IDs.GAME_PERMADEATH:
          return " (Death) Permadeath";
        case GameOptions.IDs.GAME_DEATH_SCREENSHOT:
          return " (Death) Death Screenshot";
        case GameOptions.IDs.GAME_AGGRESSIVE_HUNGRY_CIVILIANS:
          return "(Living) Aggressive Hungry Civsilians";
        case GameOptions.IDs.GAME_NATGUARD_FACTOR:
          return " (Event) National Guard";
        case GameOptions.IDs.GAME_SUPPLIESDROP_FACTOR:
          return " (Event) Supplies Drop";
        case GameOptions.IDs.GAME_UNDEADS_UPGRADE_DAYS:
          return "(Undead) Undeads Skills Upgrade Days";
        case GameOptions.IDs.GAME_RATS_UPGRADE:
          return "(Undead) Rats Skill Upgrade";
        case GameOptions.IDs.GAME_SKELETONS_UPGRADE:
          return "(Undead) Skeletons Skill Upgrade";
        case GameOptions.IDs.GAME_SHAMBLERS_UPGRADE:
          return "(Undead) Shamblers Skill Upgrade";
        default:
          throw new ArgumentOutOfRangeException("unhandled option");
      }
    }

    public static string Name(GameOptions.ReincMode mode)
    {
      switch (mode)
      {
        case GameOptions.ReincMode._FIRST:
          return "Random Follower";
        case GameOptions.ReincMode.KILLER:
          return "Your Killer";
        case GameOptions.ReincMode.ZOMBIFIED:
          return "Your Zombie Self";
        case GameOptions.ReincMode.RANDOM_LIVING:
          return "Random Living";
        case GameOptions.ReincMode.RANDOM_UNDEAD:
          return "Random Undead";
        case GameOptions.ReincMode.RANDOM_ACTOR:
          return "Random Actor";
        default:
          throw new ArgumentOutOfRangeException("unhandled ReincMode");
      }
    }

    public static string Name(GameOptions.SimRatio ratio)
    {
      switch (ratio)
      {
        case GameOptions.SimRatio._FIRST:
          return "OFF";
        case GameOptions.SimRatio.ONE_QUARTER:
          return "25%";
        case GameOptions.SimRatio.ONE_THIRD:
          return "33%";
        case GameOptions.SimRatio.HALF:
          return "50%";
        case GameOptions.SimRatio.TWO_THIRDS:
          return "66%";
        case GameOptions.SimRatio.THREE_QUARTER:
          return "75%";
        case GameOptions.SimRatio.FULL:
          return "FULL";
        default:
          throw new ArgumentOutOfRangeException("unhandled simRatio");
      }
    }

    public static float SimRatioToFloat(GameOptions.SimRatio ratio)
    {
      switch (ratio)
      {
        case GameOptions.SimRatio._FIRST:
          return 0.0f;
        case GameOptions.SimRatio.ONE_QUARTER:
          return 0.25f;
        case GameOptions.SimRatio.ONE_THIRD:
          return 0.3333333f;
        case GameOptions.SimRatio.HALF:
          return 0.5f;
        case GameOptions.SimRatio.TWO_THIRDS:
          return 0.6666667f;
        case GameOptions.SimRatio.THREE_QUARTER:
          return 0.75f;
        case GameOptions.SimRatio.FULL:
          return 1f;
        default:
          throw new ArgumentOutOfRangeException("unhandled simRatio");
      }
    }

    public static string Name(GameOptions.ZupDays d)
    {
      switch (d)
      {
        case GameOptions.ZupDays._FIRST:
          return "1 d";
        case GameOptions.ZupDays.TWO:
          return "2 d";
        case GameOptions.ZupDays.THREE:
          return "3 d";
        case GameOptions.ZupDays.FOUR:
          return "4 d";
        case GameOptions.ZupDays.FIVE:
          return "5 d";
        case GameOptions.ZupDays.SIX:
          return "6 d";
        case GameOptions.ZupDays.SEVEN:
          return "7 d";
        case GameOptions.ZupDays.OFF:
          return "OFF";
        default:
          throw new ArgumentOutOfRangeException("unhandled zupDays");
      }
    }

    public static bool IsZupDay(GameOptions.ZupDays d, int day)
    {
      switch (d)
      {
        case GameOptions.ZupDays._FIRST:
          return true;
        case GameOptions.ZupDays.TWO:
          return day % 2 == 0;
        case GameOptions.ZupDays.THREE:
          return day % 3 == 0;
        case GameOptions.ZupDays.FOUR:
          return day % 4 == 0;
        case GameOptions.ZupDays.FIVE:
          return day % 5 == 0;
        case GameOptions.ZupDays.SIX:
          return day % 6 == 0;
        case GameOptions.ZupDays.SEVEN:
          return day % 7 == 0;
        default:
          return false;
      }
    }

    public string DescribeValue(GameMode mode, GameOptions.IDs option)
    {
      switch (option)
      {
        case GameOptions.IDs.UI_MUSIC:
          return !this.PlayMusic ? "OFF" : "ON ";
        case GameOptions.IDs.UI_MUSIC_VOLUME:
          return this.MusicVolume.ToString() + "%";
        case GameOptions.IDs.UI_SHOW_PLAYER_TAG_ON_MINIMAP:
          return !this.ShowPlayerTagsOnMinimap ? "NO " : "YES";
        case GameOptions.IDs.UI_ANIM_DELAY:
          return !this.IsAnimDelayOn ? "OFF" : "ON ";
        case GameOptions.IDs.UI_SHOW_MINIMAP:
          return !this.IsMinimapOn ? "OFF" : "ON ";
        case GameOptions.IDs.UI_SHOW_CORPSES:
          return !this.ShowCorpses ? "OFF*  (default ON)" : "ON*   (default ON)";
        case GameOptions.IDs.UI_ADVISOR:
          return !this.IsAdvisorEnabled ? "NO " : "YES";
        case GameOptions.IDs.UI_COMBAT_ASSISTANT:
          return !this.IsCombatAssistantOn ? "OFF   (default OFF)" : "ON    (default OFF)";
        case GameOptions.IDs.UI_SHOW_TARGETS:
          return !this.ShowTargets ? "OFF   (default ON)" : "ON    (default ON)";
        case GameOptions.IDs.UI_SHOW_PLAYER_TARGETS:
          return !this.ShowPlayerTargets ? "OFF   (default ON)" : "ON    (default ON)";
        case GameOptions.IDs.GAME_DISTRICT_SIZE:
          return string.Format("{0:D2}*   (default {1:D2})", (object) this.DistrictSize, (object) 50);
        case GameOptions.IDs.GAME_MAX_CIVILIANS:
          return string.Format("{0:D3}*  (default {1:D3})", (object) this.MaxCivilians, (object) 25);
        case GameOptions.IDs.GAME_MAX_DOGS:
          return string.Format("{0:D3}*  (default {1:D3})", (object) this.MaxDogs, (object) 0);
        case GameOptions.IDs.GAME_MAX_UNDEADS:
          return string.Format("{0:D3}*  (default {1:D3})", (object) this.MaxUndeads, (object) 100);
        case GameOptions.IDs.GAME_SIMULATE_DISTRICTS:
          return string.Format("{0,-4}* (default {1})", (object) GameOptions.Name(this.SimulateDistricts), (object) GameOptions.Name(GameOptions.SimRatio.FULL));
        case GameOptions.IDs.GAME_SIMULATE_SLEEP:
          return !this.SimulateWhenSleeping ? "NO*   (default NO)" : "YES*  (default NO)";
        case GameOptions.IDs.GAME_SIM_THREAD:
          return !this.SimThread ? "NO*   (default YES)" : "YES*  (default YES)";
        case GameOptions.IDs.GAME_CITY_SIZE:
          return string.Format("{0:D2}*   (default {1:D2})", (object) this.CitySize, (object) 5);
        case GameOptions.IDs.GAME_NPC_CAN_STARVE_TO_DEATH:
          return !this.NPCCanStarveToDeath ? "NO    (default YES)" : "YES   (default YES)";
        case GameOptions.IDs.GAME_ZOMBIFICATION_CHANCE:
          return string.Format("{0:D3}%  (default {1:D3}%)", (object) this.ZombificationChance, (object) 100);
        case GameOptions.IDs.GAME_REVEAL_STARTING_DISTRICT:
          return !this.RevealStartingDistrict ? "NO    (default YES)" : "YES   (default YES)";
        case GameOptions.IDs.GAME_ALLOW_UNDEADS_EVOLUTION:
          if (mode == GameMode.GM_VINTAGE)
            return "---";
          return !this.AllowUndeadsEvolution ? "NO    (default YES)" : "YES   (default YES)";
        case GameOptions.IDs.GAME_DAY_ZERO_UNDEADS_PERCENT:
          return string.Format("{0:D3}%  (default {1:D3}%)", (object) this.DayZeroUndeadsPercent, (object) 30);
        case GameOptions.IDs.GAME_ZOMBIE_INVASION_DAILY_INCREASE:
          return string.Format("{0:D3}%  (default {1:D3}%)", (object) this.ZombieInvasionDailyIncrease, (object) 5);
        case GameOptions.IDs.GAME_STARVED_ZOMBIFICATION_CHANCE:
          return string.Format("{0:D3}%  (default {1:D3}%)", (object) this.StarvedZombificationChance, (object) 50);
        case GameOptions.IDs.GAME_SANITY:
          return !this.IsSanityEnabled ? "OFF   (default ON)" : "ON    (default ON)";
        case GameOptions.IDs.GAME_MAX_REINCARNATIONS:
          return string.Format("{0:D3}   (default {1:D3})", (object) this.MaxReincarnations, (object) 1);
        case GameOptions.IDs.GAME_REINCARNATE_AS_RAT:
          return !this.CanReincarnateAsRat ? "NO    (default NO)" : "YES   (default NO)";
        case GameOptions.IDs.GAME_REINCARNATE_TO_SEWERS:
          return !this.CanReincarnateToSewers ? "NO    (default NO)" : "YES   (default NO)";
        case GameOptions.IDs.GAME_REINC_LIVING_RESTRICTED:
          return !this.IsLivingReincRestricted ? "NO    (default NO)" : "YES   (default NO)";
        case GameOptions.IDs.GAME_PERMADEATH:
          return !this.IsPermadeathOn ? "NO    (default NO)" : "YES   (default NO)";
        case GameOptions.IDs.GAME_DEATH_SCREENSHOT:
          return !this.IsDeathScreenshotOn ? "NO    (default YES)" : "YES   (default YES)";
        case GameOptions.IDs.GAME_AGGRESSIVE_HUNGRY_CIVILIANS:
          return !this.IsAggressiveHungryCiviliansOn ? "OFF   (default ON)" : "ON    (default ON)";
        case GameOptions.IDs.GAME_NATGUARD_FACTOR:
          return string.Format("{0:D3}%  (default {1:D3}%)", (object) this.NatGuardFactor, (object) 100);
        case GameOptions.IDs.GAME_SUPPLIESDROP_FACTOR:
          return string.Format("{0:D3}%  (default {1:D3}%)", (object) this.SuppliesDropFactor, (object) 100);
        case GameOptions.IDs.GAME_UNDEADS_UPGRADE_DAYS:
          return string.Format("{0:D3}   (default {1:D3})", (object) GameOptions.Name(this.ZombifiedsUpgradeDays), (object) GameOptions.Name(GameOptions.ZupDays.THREE));
        case GameOptions.IDs.GAME_RATS_UPGRADE:
          if (mode == GameMode.GM_VINTAGE)
            return "---";
          return !this.RatsUpgrade ? "NO    (default NO)" : "YES   (default NO)";
        case GameOptions.IDs.GAME_SKELETONS_UPGRADE:
          if (mode == GameMode.GM_VINTAGE)
            return "---";
          return !this.SkeletonsUpgrade ? "NO    (default NO)" : "YES   (default NO)";
        case GameOptions.IDs.GAME_SHAMBLERS_UPGRADE:
          if (mode == GameMode.GM_VINTAGE)
            return "---";
          return !this.ShamblersUpgrade ? "NO    (default NO)" : "YES   (default NO)";
        default:
          return "???";
      }
    }

    public static void Save(GameOptions options, string filepath)
    {
      if (filepath == null)
        throw new ArgumentNullException(nameof (filepath));
      Logger.WriteLine(Logger.Stage.RUN_MAIN, "saving options...");
      IFormatter formatter = GameOptions.CreateFormatter();
      Stream stream = GameOptions.CreateStream(filepath, true);
      Stream serializationStream = stream;
      // ISSUE: variable of a boxed type
      __Boxed<GameOptions> local = (ValueType) options;
      formatter.Serialize(serializationStream, (object) local);
      stream.Flush();
      stream.Close();
      Logger.WriteLine(Logger.Stage.RUN_MAIN, "saving options... done!");
    }

    public static GameOptions Load(string filepath)
    {
      if (filepath == null)
        throw new ArgumentNullException(nameof (filepath));
      Logger.WriteLine(Logger.Stage.RUN_MAIN, "loading options...");
      GameOptions gameOptions;
      try
      {
        IFormatter formatter = GameOptions.CreateFormatter();
        Stream stream = GameOptions.CreateStream(filepath, false);
        Stream serializationStream = stream;
        gameOptions = (GameOptions) formatter.Deserialize(serializationStream);
        stream.Close();
        GameOptions.m_SanityGlobal = gameOptions.IsSanityEnabled;
      }
      catch (Exception ex)
      {
        Logger.WriteLine(Logger.Stage.RUN_MAIN, "failed to load options (no custom options?).");
        Logger.WriteLine(Logger.Stage.RUN_MAIN, string.Format("load exception : {0}.", (object) ex.ToString()));
        Logger.WriteLine(Logger.Stage.RUN_MAIN, "returning default values.");
        gameOptions = new GameOptions();
        gameOptions.ResetToDefaultValues();
      }
      Logger.WriteLine(Logger.Stage.RUN_MAIN, "loading options... done!");
      return gameOptions;
    }

    private static IFormatter CreateFormatter()
    {
      return (IFormatter) new BinaryFormatter();
    }

    private static Stream CreateStream(string saveFileName, bool save)
    {
      return (Stream) new FileStream(saveFileName, save ? FileMode.Create : FileMode.Open, save ? FileAccess.Write : FileAccess.Read, FileShare.None);
    }

    public enum IDs
    {
      UI_MUSIC,
      UI_MUSIC_VOLUME,
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
      GAME_SANITY,
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
      GAME_SHAMBLERS_UPGRADE,
    }

    public enum ZupDays
    {
      ONE = 0,
      _FIRST = 0,
      TWO = 1,
      THREE = 2,
      FOUR = 3,
      FIVE = 4,
      SIX = 5,
      SEVEN = 6,
      OFF = 7,
      _COUNT = 8,
    }

    public enum SimRatio
    {
      OFF = 0,
      _FIRST = 0,
      ONE_QUARTER = 1,
      ONE_THIRD = 2,
      HALF = 3,
      TWO_THIRDS = 4,
      THREE_QUARTER = 5,
      FULL = 6,
      _COUNT = 7,
    }

    public enum ReincMode
    {
      RANDOM_FOLLOWER = 0,
      _FIRST = 0,
      KILLER = 1,
      ZOMBIFIED = 2,
      RANDOM_LIVING = 3,
      RANDOM_UNDEAD = 4,
      RANDOM_ACTOR = 5,
      _LAST = 5,
      _COUNT = 6,
    }
  }
}
