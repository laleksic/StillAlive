using System;
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
            UI_AMBIENTSFXS, //@@MP (Release 6-1)
            UI_AMBIENTSFXS_VOLUME, //@@MP (Release 6-1)
            //UI_SHOW_PLAYER_TAG_ON_MINIMAP, //@@MP (Release 5-7)
            UI_ANIM_DELAY,
            UI_SHOW_MINIMAP,
            UI_ADVISOR,
            UI_COMBAT_ASSISTANT,
            UI_SHOW_TARGETS,
            UI_SHOW_PLAYER_TARGETS,
            UI_AUTOSAVE, //@@MP (Release 6-1)
            UI_AI_VOCALISATION_CHANCE, //@@MP (Release 7-6)

            GAME_DISTRICT_SIZE,
            GAME_MAX_ANIMALS,  //@@MP - was MaxDogs (Release 7-6)
            GAME_SIMULATE_DISTRICTS,
            GAME_TURNS_SIM_CAP, //@@MP (Release 7-3)
            GAME_SIMULATE_SLEEP,
            GAME_SIM_THREAD,
            //GAME_REVEAL_STARTING_DISTRICT, //@@MP (Release 6-1)
            //GAME_REINCARNATE_AS_RAT, //@@MP (Release 5-7)
            //GAME_REINCARNATE_TO_SEWERS, //@@MP (Release 5-7)
            //GAME_REINC_LIVING_RESTRICTED, //@@MP (Release 5-7)
            GAME_CITY_SIZE,
            GAME_PERMADEATH,
            GAME_DEATH_SCREENSHOT,
            GAME_DAYS_BEFORE_ITEM_DESPAWNS, //@@MP (Release 7-6)
            GAME_REDUCED_MAPOBJECT_LIGHTING, //@@MP (Release 7-6)
            GAME_WORLD_DECAY, //@@MP (Release 7-6)
            GAME_DAYS_BEFORE_WORLD_DECAYS, //@@MP (Release 7-6)

            DIFFICULTY_MAX_CIVILIANS,
            DIFFICULTY_MAX_UNDEADS,
            DIFFICULTY_SPAWN_SKELETON_CHANCE,
            DIFFICULTY_SPAWN_ZOMBIE_CHANCE,
            DIFFICULTY_SPAWN_ZOMBIE_MASTER_CHANCE,
            DIFFICULTY_NPC_CAN_STARVE_TO_DEATH,
            DIFFICULTY_ZOMBIFICATION_CHANCE,
            DIFFICULTY_ALLOW_UNDEADS_EVOLUTION,
            DIFFICULTY_DAY_ZERO_UNDEADS_PERCENT,
            DIFFICULTY_ZOMBIE_INVASION_DAILY_INCREASE,
            DIFFICULTY_STARVED_ZOMBIFICATION,  //@@MP - changed from a % chance to bool (Release 7-3)
            DIFFICULTY_SANITY, //@@MP (Release 1)
            DIFFICULTY_AGGRESSIVE_HUNGRY_CIVILIANS,
            DIFFICULTY_NATGUARD_FACTOR,
            DIFFICULTY_BLACKOPS_RAIDS, //@@MP (Release 7-5)
            DIFFICULTY_SUPPLIESDROP_FACTOR,
            DIFFICULTY_UNDEADS_UPGRADE_DAYS,
            //DIFFICULTY_RATS_UPGRADE, //@@MP (Release 5-7)
            DIFFICULTY_SKELETONS_UPGRADE,
            DIFFICULTY_SHAMBLERS_UPGRADE,
            DIFFICULTY_ANTIVIRAL_PILLS, //@@MP (Release 7-6)
            DIFFICULTY_BACKPACKS, //@@MP (Release 8-2)
            DIFFICULTY_RESOURCES_AVAILABILITY, //@@MP (Release 7-4)
            DIFFICULTY_UNDEAD_DAMAGE_PERCENT, //@@MP (Release 7-4)
            DIFFICULTY_LIVING_DAMAGE_PERCENT, //@@MP (Release 7-4)
            DIFFICULTY_RESCUE_DAY, //@@MP (Release 7-4)
            HIDDEN_RESCUE_DAY, //@@MP (Release 7-4)
        };
        #endregion

        #region ENUMs
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

        public enum SimCap //@@MP (Release 7-3)
        {
            //assumes 30 turns per hour
            _FIRST = 0, //4 hours
            LOW = _FIRST,
            LOW_MED, //8 hours
            MED, //12 hours
            MED_HIGH, //18 hours
            HIGH, //24 hours
            MAX, //48 hours
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

        #region Resources availability
        //@@MP (Release 7-4)
        public enum Resources
        {
            LOW,
            MED,
            HIGH
        }
        #endregion

        #region Options category
        //@@MP - Options are now divided into categories so that they can be handled separately (Release 7-4)
        public enum OptionsCategory
        {
            GENERAL,
            DIFFICULTY,
            ALL
        }
        #endregion
        #endregion

        #region Default values
        public const int DEFAULT_DISTRICT_SIZE = 50;
        public const int DEFAULT_MAX_CIVILIANS = 25;
        public const int DEFAULT_MAX_ANIMALS = 4; //@@MP (Release 7-6)
        public const int DEFAULT_MAX_UNDEADS = 100;
        public const int DEFAULT_SPAWN_SKELETON_CHANCE = 60;
        public const int DEFAULT_SPAWN_ZOMBIE_CHANCE = 30;
        public const int DEFAULT_SPAWN_ZOMBIE_MASTER_CHANCE = 10;
        public const int DEFAULT_CITY_SIZE = 7; //@@MP (Release 7-3)
        public const SimRatio DEFAULT_SIM_DISTRICTS = SimRatio.FULL;
        public const SimCap DEFAULT_DISTRCT_SIM_CAP = SimCap.MED; //@@MP (Release 7-3)
        public const int DEFAULT_ZOMBIFICATION_CHANCE = 100;
        public const int DEFAULT_DAY_ZERO_UNDEADS_PERCENT = 30;
        public const int DEFAULT_ZOMBIE_INVASION_DAILY_INCREASE = 3; //@@MP - was 5 (Release 8-2)
        public const bool DEFAULT_STARVED_ZOMBIFICATION = false; //@@MP - changed from % chance to bool (Release 7-3)
        public const int DEFAULT_MAX_REINCARNATIONS = 7; //@@MP - upped after I forgot when removing the max reinc option (Release 6-6)
        public const int DEFAULT_NATGUARD_FACTOR = 100;
        public const bool DEFAULT_BLACKOPS_RAIDS = true; //@@MP (Release 7-5)
        public const int DEFAULT_SUPPLIESDROP_FACTOR = 100;
        public const ZupDays DEFAULT_ZOMBIFIEDS_UPGRADE_DAYS = ZupDays.THREE;
        public const Resources DEFAULT_RESOURCES_AVAILABILITY = Resources.MED; //@@MP (Release 7-4)
        public const int DEFAULT_UNDEAD_DAMAGE_PERCENT = 100; //@@MP (Release 7-4)
        public const int DEFAULT_LIVING_DAMAGE_PERCENT = 100; //@@MP (Release 7-4)
        public const int DEFAULT_RESCUE_DAY = 21; //@@MP (Release 7-4)
        public const int DEFAULT_DAYS_BEFORE_ITEM_DESPAWNS = 4; //@@MP (Release 7-6)
        public const int DEFAULT_UI_AI_VOCALISATION_CHANCE = 20; //@@MP (Release 7-6)
        public const bool DEFAULT_REDUCED_MAPOBJECT_LIGHTING = false; //@@MP (Release 7-6)
        public const int DEFAULT_DAYS_BEFORE_WORLD_DECAYS = 7; //@@MP (Release 7-6)
        #endregion

        #region Fields
        int m_DistrictSize;
        int m_MaxCivilians;
        int m_MaxAnimals;  //@@MP - was MaxDogs (Release 7-6)
        int m_MaxUndeads;
        bool m_PlayMusic;
        int m_MusicVolume;
        bool m_PlaySFXs; //@@MP (Release 2)
        int m_SFXVolume; //@@MP (Release 2)
        bool m_PlayAmbientSFXs; //@@MP (Release 6-1)
        int m_AmbientSFXVolume; //@@MP (Release 6-1)
        bool m_AnimDelay;
        bool m_ShowMinimap;
        bool m_Autosaving; //@@MP (Release 6-1)
        bool m_EnabledAdvisor;
        bool m_CombatAssistant;
        SimRatio m_SimulateDistricts;
        float m_cachedSimRatioFloat;
        SimCap m_SimTurnsCap;  //@@MP (Release 7-3)
        bool m_SimulateWhenSleeping;
        bool m_SimThread;
        int m_SpawnSkeletonChance;
        int m_SpawnZombieChance;
        int m_SpawnZombieMasterChance;
        int m_CitySize;
        bool m_NPCCanStarveToDeath;
        int m_ZombificationChance;
        //bool m_RevealStartingDistrict; //@@MP (Release 6-1)
        bool m_AllowUndeadsEvolution;
        int m_DayZeroUndeadsPercent;
        int m_ZombieInvasionDailyIncrease;
        bool m_StarvedZombification;
        bool m_Sanity; //@@MP (Release 1)
        int m_MaxReincarnations;
        //bool m_CanReincarnateAsRat; //@@MP (Release 5-7)
        //bool m_CanReincarnateToSewers; //@@MP (Release 5-7)
        //bool m_IsLivingReincRestricted; //@@MP (Release 5-7)
        bool m_Permadeath;
        bool m_DeathScreenshot;
        bool m_AggressiveHungryCivilians;
        int m_NatGuardFactor;
        bool m_BlackOpsRaids; //@@MP (Release 7-5)
        int m_SuppliesDropFactor;
        bool m_ShowTargets;
        bool m_ShowPlayerTargets;
        ZupDays m_ZupDays;
        //bool m_RatsUpgrade;//@@MP (Release 5-7)
        bool m_SkeletonsUpgrade;
        bool m_ShamblersUpgrade;
        bool m_AntiviralPills; //@@MP (Release 7-6)
        Resources m_ResourcesAvailability;  //@@MP (Release 7-4)
        int m_UndeadDamagePercent; //@@MP (Release 7-4)
        int m_LivingDamagePercent; //@@MP (Release 7-4)
        int m_VisibleRescueDay; //@@MP (Release 7-4)
        int m_HiddenRescueDay; //@@MP (Release 7-4)
        int m_DaysBeforeDiscardedItemDespawns; //@@MP (Release 7-6)
        int m_AIVocalisationChance; //@@MP (Release 7-6)
        bool m_ReducedMapObjectLighting; //@@MP (Release 7-6)
        bool m_WorldDecay; //@@MP (Release 7-6)
        int m_DaysBeforeWorldDecays; //@@MP (Release 7-6)
        bool m_Backpacks; //@@MP (Release 8-2)
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

        /// <summary>
        /// Disables ambient sound effects
        /// </summary>
        public bool PlayAmbientSFXs //@@MP (Release 6-1)
        {
            get { return m_PlayAmbientSFXs; }
            set { m_PlayAmbientSFXs = value; }
        }

        /// <summary>
        /// Volume for ambient sound effects (eg weather)
        /// </summary>
        public int AmbientSFXVolume //@@MP (Release 6-1)
        {
            get { return m_AmbientSFXVolume; }
            set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;
                m_AmbientSFXVolume = value;
            }
        }

        public bool Autosaving //@@MP (Release 6-1)
        {
            get { return m_Autosaving; }
            set { m_Autosaving = value; }
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
                if (value < 5) value = 5; //@@MP - was 3 (Release 7-3)
                if (value > 7) value = 7;
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

        public int MaxAnimals  //@@MP - was MaxDogs (Release 7-6)
        {
            get { return m_MaxAnimals; }
            set
            {
                if (value < 0) value = 0;
                if (value > 10) value = 10; //@@MP - was 75, lol (Release 7-6)
                m_MaxAnimals = value;
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public float SimRatioFloat
        {
            get { return m_cachedSimRatioFloat; }
        }

        public SimCap SimTurnsCap //@@MP (Release 7-3)
        {
            get { return m_SimTurnsCap; }
            set { m_SimTurnsCap = value; }
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
                if (value < 50) value = 50; //@@MP - was 30 (Release 7-3)
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

        /*public bool RevealStartingDistrict //@@MP (Release 6-1)
        {
            get { return m_RevealStartingDistrict; }
            set { m_RevealStartingDistrict = value; }
        }*/

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
                if (value > 10) value = 10;  //@@MP - was 20 (Release 7-6)
                m_ZombieInvasionDailyIncrease = value;
            }
        }

        public bool StarvedZombification
        {
            get { return m_StarvedZombification; }
            set { m_StarvedZombification = value; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
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
            }
        }

        /*public bool CanReincarnateAsRat //@@MP (Release 5-7)
        {
            get { return m_CanReincarnateAsRat; }
            set { m_CanReincarnateAsRat = value; }
        }*/

        /*public bool CanReincarnateToSewers //@@MP (Release 5-7)
        {
            get { return m_CanReincarnateToSewers; }
            set { m_CanReincarnateToSewers = value; }
        }*/

        /*public bool IsLivingReincRestricted //@@MP (Release 5-7)
        {
            get { return m_IsLivingReincRestricted; }
            set { m_IsLivingReincRestricted = value; }
        }*/

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

        public bool BlackOpsRaidsEnabled  //@@MP (Release 7-5)
        {
            get { return m_BlackOpsRaids; }
            set { m_BlackOpsRaids = value; }
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

        /*public bool RatsUpgrade //@@MP - rats upgrade disabled (Release 5-7)
        {
            get { return m_RatsUpgrade; }
            set { m_RatsUpgrade = value; }
        }*/

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

        public bool AntiviralPills //@@MP (Release 7-6)
        {
            get { return m_AntiviralPills; }
            set { m_AntiviralPills = value; }
        }

        public Resources ResourcesAvailability //@@MP (Release 7-4)
        {
            get { return m_ResourcesAvailability; }
            set { m_ResourcesAvailability = value; }
        }

        /// <summary>
        /// How much damage undeads do
        /// </summary>
        public int UndeadDamagePercent //@@MP (Release 7-4)
        {
            get { return m_UndeadDamagePercent; }
            set
            {
                if (value < 50) value = 50;
                if (value > 200) value = 200;
                m_UndeadDamagePercent = value;
            }
        }

        /// <summary>
        /// How much damage undeads do
        /// </summary>
        public int LivingDamagePercent //@@MP (Release 7-4)
        {
            get { return m_LivingDamagePercent; }
            set
            {
                if (value < 50) value = 50;
                if (value > 200) value = 200;
                m_LivingDamagePercent = value;
            }
        }

        /// <summary>
        /// Holds the player-selected day in case they chose "Random". This is the one the player sees in character creation.
        /// <para>If they did pick random day, it will ensure it stays as "Random" for each new character creation</para>
        /// </summary>
        public int VisibleRescueDay //@@MP (Release 7-4)
        {
            get { return m_VisibleRescueDay; }
            set
            {
                if (value < 6) value = 6; //6 will have it randomised at new character creation
                if (value > 100) value = 100; //max of 100 days
                m_VisibleRescueDay = value;
            }
        }

        /// <summary>
        /// Holds the actual rescue day. If a player chose Random, this will hold the true number but not appear that way to the player
        /// </summary>
        public int HiddenRescueDay //@@MP (Release 7-4)
        {
            get { return m_HiddenRescueDay; }
            set { m_HiddenRescueDay = value; }
        }

        /// <summary>
        /// How many days before items that were dropped by NPCs get despawned
        /// </summary>
        public int DaysBeforeDiscardedItemDespawns //@@MP (Release 7-6)
        {
            get { return m_DaysBeforeDiscardedItemDespawns; }
            set
            {
                if (value < 1) value = 1; //minimum of 1
                if (value > 21) value = 21; //max of 21 days
                m_DaysBeforeDiscardedItemDespawns = value;
            }
        }

        /// <summary>
        /// The higher the number, the more likely undead/dogs will growl when doing actions
        /// </summary>
        public int AIVocalisationChance //@@MP (Release 7-6)
        {
            get { return m_AIVocalisationChance; }
            set
            {
                if (value < 0) value = 0; //minimum of 1
                if (value > 100) value = 100; //max of 100
                m_AIVocalisationChance = value;
            }
        }

        /// <summary>
        /// Cars and barrels only light up a 1 tile radius around them when this is True
        /// </summary>
        public bool ReducedMapObjectLighting //@@MP (Release 7-6)
        {
            get { return m_ReducedMapObjectLighting; }
            set { m_ReducedMapObjectLighting = value; }
        }

        public bool IsWorldDecayOn //@@MP (Release 7-6)
        {
            get { return m_WorldDecay; }
            set { m_WorldDecay = value; }
        }

        /// <summary>
        /// How many days before tiles and certain map objects begin to appear decayed
        /// </summary>
        public int DaysBeforeWorldDecays //@@MP (Release 7-6)
        {
            get { return m_DaysBeforeWorldDecays; }
            set
            {
#if DEBUG
                if (value < 1) value = 1; //can start from the get-go for testing purposes
#else
                if (value < 7) value = 7; //minimum of 7 days
#endif
                if (value > 28) value = 28; //max of 28 days before decay starts
                m_DaysBeforeWorldDecays = value;
            }
        }

        public bool BackpacksEnabled //@@MP (Release 8-2)
        {
            get { return m_Backpacks; }
            set { m_Backpacks = value; }
        }
        #endregion

        #region Dev only options (hidden)
        public bool DEV_ShowActorsStats { get; set; }
        #endregion

        #region Init
        public void ResetToDefaultValues(OptionsCategory category) //@@MP - differentiated between general and difficulty settings (Release 7-4)
        {
            if (category == OptionsCategory.GENERAL || category == OptionsCategory.ALL)
            {
                m_PlayMusic = true;
                //m_MusicVolume = 50;
                m_PlaySFXs = true; //@@MP (Release 2)
                //m_SFXVolume = 50; //@@MP (Release 2)
                m_PlayAmbientSFXs = true; //@@MP (Release 6-1)
                //m_AmbientSFXVolume = 50; //@@MP (Release 6-1)
                m_AnimDelay = false; //@@MP (Release 6-5)
                m_ShowMinimap = true;
                m_EnabledAdvisor = true;
                m_CombatAssistant = false;
                this.SimulateDistricts = DEFAULT_SIM_DISTRICTS;
                m_SimTurnsCap = SimCap.MED; //@@MP (Release 7-3)
                m_SimulateWhenSleeping = false;
                m_SimThread = true;
                m_CitySize = DEFAULT_CITY_SIZE;
                m_Autosaving = true; //@@MP (Release 6-1)
                m_DeathScreenshot = true;
                m_DistrictSize = DEFAULT_DISTRICT_SIZE;
                m_ShowTargets = true;
                m_ShowPlayerTargets = true;
                //m_CanReincarnateAsRat = false; //@@MP (Release 5-7)
                //m_CanReincarnateToSewers = false; //@@MP (Release 5-7)
                //m_IsLivingReincRestricted = false; //@@MP (Release 5-7)
                //m_RevealStartingDistrict = true; //@@MP (Release 6-1)
                m_DaysBeforeDiscardedItemDespawns = DEFAULT_DAYS_BEFORE_ITEM_DESPAWNS; //@@MP (Release 7-6)
                m_DaysBeforeWorldDecays = DEFAULT_DAYS_BEFORE_WORLD_DECAYS; //@@MP (Release 7-6)
                m_Permadeath = true;
                m_MaxAnimals = DEFAULT_MAX_ANIMALS; //@@MP - was MaxDogs (Release 7-6)
                m_AIVocalisationChance = DEFAULT_UI_AI_VOCALISATION_CHANCE; //@@MP (Release 7-6)
                m_WorldDecay = true; //@@MP (Release 7-6)
                m_ReducedMapObjectLighting = DEFAULT_REDUCED_MAPOBJECT_LIGHTING; //@@MP (Release 7-6)
            }
            if (category == OptionsCategory.DIFFICULTY || category == OptionsCategory.ALL)
            {
                m_MaxCivilians = DEFAULT_MAX_CIVILIANS;
                m_MaxUndeads = DEFAULT_MAX_UNDEADS;
                m_SpawnSkeletonChance = DEFAULT_SPAWN_SKELETON_CHANCE;
                m_SpawnZombieChance = DEFAULT_SPAWN_ZOMBIE_CHANCE;
                m_SpawnZombieMasterChance = DEFAULT_SPAWN_ZOMBIE_MASTER_CHANCE;
                m_NPCCanStarveToDeath = true;
                m_ZombificationChance = DEFAULT_ZOMBIFICATION_CHANCE;
                m_AllowUndeadsEvolution = true;
                m_DayZeroUndeadsPercent = DEFAULT_DAY_ZERO_UNDEADS_PERCENT;
                m_ZombieInvasionDailyIncrease = DEFAULT_ZOMBIE_INVASION_DAILY_INCREASE;
                m_StarvedZombification = DEFAULT_STARVED_ZOMBIFICATION;
                m_MaxReincarnations = DEFAULT_MAX_REINCARNATIONS;
                m_Sanity = true;
                m_AggressiveHungryCivilians = true;
                m_NatGuardFactor = DEFAULT_NATGUARD_FACTOR;
                m_BlackOpsRaids = DEFAULT_BLACKOPS_RAIDS;  //@@MP (Release 7-5)
                m_SuppliesDropFactor = DEFAULT_SUPPLIESDROP_FACTOR;
                m_ZupDays = DEFAULT_ZOMBIFIEDS_UPGRADE_DAYS;
                //m_RatsUpgrade = false; //@@MP (Release 5-7)
                m_SkeletonsUpgrade = false;
                m_ShamblersUpgrade = false;
                m_AntiviralPills = false; //@@MP (Release 5-2), made false (Release 7-6)
                m_HiddenRescueDay = m_VisibleRescueDay = DEFAULT_RESCUE_DAY; //@@MP (Release 7-4)
                m_ResourcesAvailability = GameOptions.Resources.MED; //@@MP (Release 7-4)
                m_UndeadDamagePercent = DEFAULT_UNDEAD_DAMAGE_PERCENT; //@@MP (Release 7-4)
                m_LivingDamagePercent = DEFAULT_LIVING_DAMAGE_PERCENT; //@@MP (Release 7-4)
                m_Backpacks = true; //@@MP (Release 8-2)
            }
        }
        #endregion

        #region Helpers
        public static string Name(IDs option)
        {
            switch (option)
            {
                case IDs.DIFFICULTY_AGGRESSIVE_HUNGRY_CIVILIANS:      return "(Living) Aggressive hungry civilians";
                case IDs.DIFFICULTY_ALLOW_UNDEADS_EVOLUTION:          return "(Undead) Allow undeads evolution (non-VTG)";
                case IDs.GAME_CITY_SIZE:                              return "   (Map) City size";
                case IDs.DIFFICULTY_DAY_ZERO_UNDEADS_PERCENT:         return "(Undead) Day 0 undeads";
                case IDs.GAME_DEATH_SCREENSHOT:                       return " (Death) Screenshot on death";
                case IDs.GAME_DISTRICT_SIZE:                          return "   (Map) District map size";
                case IDs.DIFFICULTY_MAX_CIVILIANS:                    return "   (Map) Maximum Civilians cap";
                case IDs.GAME_MAX_ANIMALS:                            return "   (Map) Maximum animals per district cap";
                case IDs.DIFFICULTY_MAX_UNDEADS:                      return "   (Map) Maximum undeads cap";
                case IDs.DIFFICULTY_NATGUARD_FACTOR:                  return " (Event) National Guard unit arrival";
                case IDs.DIFFICULTY_BLACKOPS_RAIDS:                   return " (Event) Black Ops unit arrival";  //@@MP (Release 7-5)
                case IDs.DIFFICULTY_NPC_CAN_STARVE_TO_DEATH:          return "(Living) NPCs can starve to death";
                case IDs.GAME_PERMADEATH:                             return " (Death) Permadeath";
                //case IDs.DIFFICULTY_RATS_UPGRADE:                     return "(Undead) Rats Skill Upgrade (non-VTG)"; //@@MP - added " (non-VTG)" (Release 5-2), removed (Release 5-6)
                //case IDs.GAME_REVEAL_STARTING_DISTRICT:               return "   (Map) Reveal Starting District"; //@@MP (Release 6-1)
                //case IDs.GAME_REINC_LIVING_RESTRICTED:                return " (Reinc) Civilians-only Reincarnation"; //@@MP (Release 5-7)
                //case IDs.GAME_REINCARNATE_AS_RAT:                     return " (Reinc) Can Reincarnate as Rat (non-VTG)"; //@@MP - added " (non-VTG)" (Release 5-2), removed (Release 5-6)
                //case IDs.GAME_REINCARNATE_TO_SEWERS:                  return " (Reinc) Can Reincarnate to Sewers"; //@@MP (Release 5-7)
                case IDs.DIFFICULTY_SANITY:                           return "(Living) Sanity loss"; //@@MP (Release 1)
                case IDs.DIFFICULTY_SHAMBLERS_UPGRADE:                return "(Undead) Shamblers skill upgrade (non-VTG)"; //@@MP - added " (non-VTG)" (Release 5-2)
                case IDs.DIFFICULTY_SKELETONS_UPGRADE:                return "(Undead) Skeletons skill upgrade (non-VTG)"; //@@MP - added " (non-VTG)" (Release 5-2)
                case IDs.GAME_SIMULATE_DISTRICTS:                     return "   (Sim) Districts simulation";
                case IDs.GAME_TURNS_SIM_CAP:                          return "   (Sim) Simulation turns cap"; //@@MP (Release 7-3)
                case IDs.GAME_SIM_THREAD:                             return "   (Sim) > Synchronous Simulation <";
                case IDs.GAME_SIMULATE_SLEEP:                         return "   (Sim) < Simulate when sleeping >";
                case IDs.GAME_DAYS_BEFORE_ITEM_DESPAWNS:              return "   (Map) Days before a junk item despawns"; //@@MP (Release 7-6)
                case IDs.GAME_REDUCED_MAPOBJECT_LIGHTING:             return "   (Map) Fires have a smaller light radius"; //@@MP (Release 7-6)
                case IDs.GAME_WORLD_DECAY:                            return "   (Map) World decays as time passes"; //@@MP (Release 7-6)
                case IDs.GAME_DAYS_BEFORE_WORLD_DECAYS:               return "   (Map) Days before the world looks decayed"; //@@MP (Release 7-6)
                case IDs.DIFFICULTY_SPAWN_SKELETON_CHANCE:            return "(Undead) Spawn Skeleton chance";
                case IDs.DIFFICULTY_SPAWN_ZOMBIE_CHANCE:              return "(Undead) Spawn Zombie chance";
                case IDs.DIFFICULTY_SPAWN_ZOMBIE_MASTER_CHANCE:       return "(Undead) Spawn Zombie Master chance";
                case IDs.DIFFICULTY_STARVED_ZOMBIFICATION:            return "(Living) Starvation will zombify (STD)";
                case IDs.DIFFICULTY_SUPPLIESDROP_FACTOR:              return " (Event) Helicoptered supplies drop";
                case IDs.DIFFICULTY_UNDEADS_UPGRADE_DAYS:             return "(Undead) Undeads skills upgrade days";
                case IDs.DIFFICULTY_ANTIVIRAL_PILLS:                  return "(Living) Allow anti-viral pills (VTG)"; //@@MP (Release 7-6)
                case IDs.DIFFICULTY_RESOURCES_AVAILABILITY:           return "(Living) Resources availability"; //@@MP (Release 7-4)
                case IDs.DIFFICULTY_RESCUE_DAY:                       return "(Living) Helicopter rescue day"; //@@MP (Release 7-4)
                case IDs.DIFFICULTY_ZOMBIFICATION_CHANCE:             return "(Living) Zombification chance (STD)";
                case IDs.DIFFICULTY_ZOMBIE_INVASION_DAILY_INCREASE:   return "(Undead) Invasion daily increase";
                case IDs.DIFFICULTY_UNDEAD_DAMAGE_PERCENT:            return "(Undead) Undeads damage percent"; //@@MP (Release 7-4)
                case IDs.DIFFICULTY_LIVING_DAMAGE_PERCENT:            return "(Living) Livings damage percent"; //@@MP (Release 7-4)
                case IDs.DIFFICULTY_BACKPACKS:                        return "(Living) Backpacks available"; //@@MP (Release 8-2)
                case IDs.UI_ANIM_DELAY:                               return "   (Gfx) Animations delay";
                case IDs.UI_MUSIC:                                    return "   (Sfx) Music enabled";
                case IDs.UI_MUSIC_VOLUME:                             return "   (Sfx) Music volume";
                case IDs.UI_SFXS:                                     return "   (Sfx) Sound effects enabled"; //@@MP (Release 2)
                case IDs.UI_SFXS_VOLUME:                              return "   (Sfx) Sound effects volume"; //@@MP (Release 2)
                case IDs.UI_AMBIENTSFXS:                              return "   (Sfx) Ambient SFXs (eg weather) enabled"; //@@MP (Release 6-1)
                case IDs.UI_AMBIENTSFXS_VOLUME:                       return "   (Sfx) Ambient sound effects volume"; //@@MP (Release 6-1)
                case IDs.UI_AI_VOCALISATION_CHANCE:                   return "   (Sfx) Undead likelihood to growl"; //@@MP (Release 7-6)
                case IDs.UI_SHOW_MINIMAP:                             return "   (Gfx) Show minimap";
                case IDs.UI_ADVISOR:                                  return "  (Help) Enable Hints Advisor";
                case IDs.UI_COMBAT_ASSISTANT:                         return "  (Help) Enable Combat Assistant";
                case IDs.UI_SHOW_TARGETS:                             return "  (Help) Show other actors targets"; // alpha 10
                case IDs.UI_SHOW_PLAYER_TARGETS:                      return "  (Help) Always show player targets";
                case IDs.UI_AUTOSAVE:                                 return "  (Help) Autosave every 18 game-hours"; //@@MP (Release 6-1)

                default:
                    throw new ArgumentOutOfRangeException("option","unhandled option");
            }
        }

        // alpha10
        public static string Describe(IDs option)
        {
            switch (option)
            {
                case IDs.DIFFICULTY_AGGRESSIVE_HUNGRY_CIVILIANS:
                    return "Allows desperately hungry civilians to attack other people for food.";
                case IDs.DIFFICULTY_ALLOW_UNDEADS_EVOLUTION:
                    return "ALWAYS OFF IN VINTAGE MODE.\nAllows undeads to evolve into stronger forms.";
                case IDs.GAME_CITY_SIZE:
                    return "Size of the city grid. The city is a square grid of districts.\nLarger cities are more fun but rapidly increases game saves size and loading time.";
                case IDs.DIFFICULTY_DAY_ZERO_UNDEADS_PERCENT:
                    return "Percentage of the maximum undeads cap spawned when the game starts.";
                case IDs.GAME_DEATH_SCREENSHOT:
                    return "Automatically saves a screenshot when you die.\n(see \\Documents\\Rogue Survivor\\Still Alive\\Config\\Screenshots folder)";
                case IDs.GAME_DISTRICT_SIZE:
                    return "How large are the districts in terms of tiles.\nLarger districts drastically increase saving/loading and turn processing time!";
                case IDs.DIFFICULTY_MAX_CIVILIANS:
                    return "Maximum number of civilians in a district at any given time.\nMore civilians makes the game more fun, but slows turn processing down.";
                case IDs.GAME_MAX_ANIMALS:
                    return "Maximum number of animals in a district at any given time.";
                case IDs.DIFFICULTY_MAX_UNDEADS:
                    return "Maximum number of undeads in a district at any given time.\nMore undeads makes the game harder for humans, but slows turn processing down.\nThe vanilla version of RS has it at 100.";
                case IDs.DIFFICULTY_NATGUARD_FACTOR:
                    return "Affects how likely the National Guard event happens.\n100 is default, 0 to disable.";
                case IDs.DIFFICULTY_BLACKOPS_RAIDS: //@@MP (Release 7-5)
                    return "Enable or disable late-game raids by black ops forces.\nEnabled is default, disabled lowers your score multiplier.";
                case IDs.DIFFICULTY_NPC_CAN_STARVE_TO_DEATH:
                    return "When NPCs are starving they can die. When disabled, AI characters will never die from hunger.";
                case IDs.GAME_PERMADEATH:
                    return "Deletes your saved game file when you die. Extra challenge and tension.\nEnabling this option does not prohibit optional recincarnation.";
                /*case IDs.DIFFICULTY_RATS_UPGRADE:
                    return "ALWAYS OFF IN VTG-VINTAGE MODE.\nCan Rats type of undeads upgrade their skills like other undeads.\nNot recommended unless you want super annoying rats.";*/
                /*case IDs.GAME_REVEAL_STARTING_DISTRICT: //@@MP (Release 6-1)
                    return "You start the game with full knowledge of the district you start in.";*/
                /*case IDs.GAME_REINC_LIVING_RESTRICTED:
                    return "Limit choices of reincarnations as livings to civilians only. If disabled allow you to reincarnte into all kinds of livings.";
                case IDs.GAME_REINCARNATE_AS_RAT:
                    return "Enables the possibility to reincarnate into a zombie rat.";
                case IDs.GAME_REINCARNATE_TO_SEWERS:
                    return "Enables the possibility to reincarnate into the sewers.";*/
                case IDs.DIFFICULTY_SHAMBLERS_UPGRADE:
                    return "ALWAYS OFF IN VINTAGE MODE.\nCan Shamblers type of undeads upgrade their skills like other undeads.";
                case IDs.DIFFICULTY_SKELETONS_UPGRADE:
                    return "ALWAYS OFF IN VINTAGE MODE.\nCan Skeletons type of undeads upgrade their skills like other undeads.";
                case IDs.GAME_SIMULATE_DISTRICTS:
                    return "The game simulates what is happening in districts adjacent to the one you're currently in.\nHigher settings offer a more emergent feeling from the city.\nWhen the simulation happens depends on other sim options.\nWARNING: SETTING IT TO 'OFF' MAY CAUSE CRASHES WHEN CHANGING DISTRICT. Setting to 25% should avoid it.";
                case IDs.GAME_TURNS_SIM_CAP: //@@MP (Release 7-3)
                    return "The maximum number of turns to simulate when changing to a different district.\nFewer turns will speed up loading time, but reduce how much the world feels 'alive'.\nOnly applies to districts you've not been adjacent to much recently.\nUse this if you disable Districts Simulation.";
                case IDs.GAME_SIM_THREAD:
                    return "Simulates activity in adjacent districts whilst you're playing and exploring.\nWhen enabled (recommended), Simulate When Sleeping is not applicable and therefore disabled.\nDisabling Synchronous simulation may help with individual turn performance, but could lead to longer\ndistrict loading times.";
                case IDs.GAME_SIMULATE_SLEEP:
                    return "Simulates activity in adjacent districts only whilst you're sleeping.\nOnly applicable when Synchronous Simulation is disabled.\nRecommended if Synchronous Simulation is off, to help a little with district load times.";
                case IDs.GAME_DAYS_BEFORE_ITEM_DESPAWNS: //@@MP (Release 7-6)
                    return "The number of in-game days at which point an item that a non-follower NPC discarded get despawned.\nOnly applies to common, low-value items. Doesn't affect items dropped by the player or their followers.\nDoes not apply to items in the player or an NPCs's inventory or bank safe.";
                case IDs.GAME_REDUCED_MAPOBJECT_LIGHTING: //@@MP (Release 7-6)
                    return "Fires from objects such as cars and fire barrels give of a smaller light radius in the dark.\nReduces visibility for a more tense and claustrophobic experience at night.\nSetting this to Off may help with performance issues.";
                case IDs.GAME_WORLD_DECAY: //@@MP (Release 7-6)
                    return "After a configurable number of days the world starts to look decayed.\nBuildings and roads begin to crumble, and plants begin to reclaim the city.\nIf this is Off, the 'Days before the world looks decayed' setting is ignored.\nThis is a thematic choice only; it doesn't affect gameplay.";
                case IDs.GAME_DAYS_BEFORE_WORLD_DECAYS: //@@MP (Release 7-6)
                    return "The number of in-game days at which point the world begins to appear apocalyptically decayed.\nThe later it starts, the longer the decay process is drawn out over time from then on.\nThis is a thematic choice only; it doesn't affect gameplay.";
                case IDs.DIFFICULTY_SPAWN_SKELETON_CHANCE:
                    return "YOU SHOULDNT BE READING THIS :)";
                case IDs.DIFFICULTY_SPAWN_ZOMBIE_CHANCE:
                    return "YOU SHOULDNT BE READING THIS :)";
                case IDs.DIFFICULTY_SPAWN_ZOMBIE_MASTER_CHANCE:
                    return "YOU SHOULDNT BE READING THIS :)";
                case IDs.DIFFICULTY_STARVED_ZOMBIFICATION:
                    return "ONLY APPLIES TO STANDARD MODE.\nSets if NPCs who starve to death will turn into a zombie.";
                case IDs.DIFFICULTY_SUPPLIESDROP_FACTOR:
                    return "Affects how likely the Supplies Drop event happens.\n100 is default, 0 to disable.";
                case IDs.DIFFICULTY_UNDEADS_UPGRADE_DAYS:
                    return "How often can undeads upgrade their skills. They usually upgrade at a slower pace than livings.";
                case IDs.DIFFICULTY_ZOMBIFICATION_CHANCE:
                    return "ONLY APPLIES TO STANDARD MODE.\nSome undeads have the ability to turn their living victims into zombies after killing them.\nThis option controls the chances of zombification. Changing this value has a large impact on game difficulty.\nException: the player is always checked for zombification when killed in all game modes.";
                case IDs.DIFFICULTY_ZOMBIE_INVASION_DAILY_INCREASE:
                    return "The zombies invasion increases in size each day, to fill up to Maximum Undeads on a map.\nThe vanilla version of RS has it at 5.";
                case IDs.UI_ANIM_DELAY:
                    return "Enable or disable UI delays when actions or events take place.\nHaving it on can help when learning the game, or if you prefer to be slowed in order to\nsee events take place as they happen.";
                case IDs.UI_MUSIC:
                    return "Enable or disable music.\nMusics are not essential for gameplay, though it is recommended you keep them enabled.\nIf you can't hear music, try using other options in RSConfig.exe from the game folder.";
                case IDs.UI_MUSIC_VOLUME:
                    return "Music volume.";
                case IDs.UI_SHOW_MINIMAP:
                    return "Display or hide the minimap.";
                case IDs.UI_ADVISOR:
                    return "Enable or disable the in-game hints system. The advisor helps you learn the game as a living.\nHints will only be shown once each. It will only give you hints it hasn't already shown you.\nAll hints are also available from the main menu.";
                case IDs.UI_COMBAT_ASSISTANT:
                    return "Draws a colored circle icon on your enemies when enabled, indicating when they will next act.\nGreen = you can act twice before your enemy will\nYellow = your enemy will act after you\nRed = your enemy will act twice after you";
                case IDs.UI_SHOW_TARGETS:
                    return "When mousing over an undead or NPCs, this option will show icons on enemies that are targeting,\nare targeted, or are in group with that individual.";
                case IDs.UI_SHOW_PLAYER_TARGETS:
                    return "Will draw icons on actors that are targeting you.";
                case IDs.DIFFICULTY_ANTIVIRAL_PILLS: //@@MP
                    return "Enable or disable anti-viral pills in the world.\nDisable antiviral pills for a more authenticate (and challenging) Vintage mode zombie experience.\nDeath will come sooner if you aren't very careful.";
                case IDs.UI_SFXS: //@@MP
                    return "Enable or disable sound effects.\nSFXs are not essential for gameplay, though it is recommended you keep them enabled.\nIf you can't hear SFXs, try using other options RSConfig.exe from the game folder.";
                case IDs.UI_SFXS_VOLUME: //@@MP
                    return "Sound effects volume (gunfire, screams, explosions, etc)";
                case IDs.DIFFICULTY_SANITY: //@@MP
                    return "Enabled or disable sanity. Low sanity causes a living to become restless and unpredicatable.";
                case IDs.UI_AMBIENTSFXS: //@@MP
                    return "Enable or disable ambient sounds.\nSFXs are not essential for gameplay, though it is recommended you keep them enabled.\nIf you can't hear SFXs, try using other options RSConfig.exe from the game folder.";
                case IDs.UI_AMBIENTSFXS_VOLUME: //@@MP
                    return "Ambient sounds volume (rain, church bells, distant animals, etc)";
                case IDs.UI_AUTOSAVE: //@@MP
                    return "Automatically saves your game every 18 in-game hours, equivalent to every 540 turns.";
                case IDs.UI_AI_VOCALISATION_CHANCE: //@@MP (Release 7-6)
                    return "Determines how likely undead and dogs are to growl when taking actions such as attacking.\nLower this to make their growls less frequent. 0 disables most growls.";
                case IDs.DIFFICULTY_RESOURCES_AVAILABILITY: //@@MP (Release 7-4)
                    return "Affects the quantity of certain items that the game world starts with, and how frequently plants will fruit.\nHigh : daily. Med : every 2 days. Low : every 3 days.\nAlso affects the chance per turn to catch a fish.";
                case IDs.DIFFICULTY_RESCUE_DAY: //@@MP (Release 7-4)
                    return "Which day that the rescue helicopter will arrive. You just need to find out where that will be...\nChoosing random will select a day between 14-28.";
                case IDs.DIFFICULTY_UNDEAD_DAMAGE_PERCENT: //@@MP (Release 7-4)
                    return "How much damage undead deal when attacking other undead and livings, as a percentage of their base damage.";
                case IDs.DIFFICULTY_LIVING_DAMAGE_PERCENT: //@@MP (Release 7-4)
                    return "How much damage livings deal when attacking other livings and undead, as a percentage of their base damage.";
                case IDs.DIFFICULTY_BACKPACKS: //@@MP (Release 8-2)
                    return "Backpacks can be found throughout the city. They increase the player's carrying capacity.\nNote: They are not usable by NPCs.";
                default:
                    throw new ArgumentOutOfRangeException("option","unhandled option");
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
                    throw new ArgumentOutOfRangeException("mode","unhandled ReincMode");
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
                    throw new ArgumentOutOfRangeException("ratio","unhandled simRatio");
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
                    throw new ArgumentOutOfRangeException("ratio","unhandled simRatio");
            }
        }

        public static string Name(SimCap simCap) //@@MP (Release 7-3)
        {
            switch (simCap)
            {
                case SimCap.LOW: return "60";
                case SimCap.LOW_MED: return "120";
                case SimCap.MED: return "240";
                case SimCap.MED_HIGH: return "360";
                case SimCap.HIGH: return "540";
                case SimCap.MAX: return "720";
                default:
                    throw new ArgumentOutOfRangeException("simCap", "unhandled simCap");
            }
        }

        public static int SimCapToTurns(SimCap simCap) //@@MP (Release 7-3)
        {
            switch (simCap)
            {
                case SimCap.LOW: return 60; //2 hours
                case SimCap.LOW_MED: return 120; //4 hours
                case SimCap.MED: return 240; //8 hours
                case SimCap.MED_HIGH: return 360; //12 hours
                case SimCap.HIGH: return 540; //18 hours
                case SimCap.MAX: return 720; //24 hours
                default:
                    throw new ArgumentOutOfRangeException("simCap", "unhandled simCap");
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
                    throw new ArgumentOutOfRangeException("d","unhandled zupDays");
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

        public static string Name(Resources availability)  //@@MP (Release 7-4)
        {
            switch (availability)
            {
                case Resources.LOW: return "LOW";
                case Resources.MED: return "MED";
                case Resources.HIGH: return "HIGH";
                default:
                    throw new ArgumentOutOfRangeException("availability", "unhandled Resources");
            }
        }

        /// <summary>
        /// Provide a percentage based on the ResourceAvailability difficulty option.
        /// </summary>
        /// <param name="availability">GameOption.ResourcesAvailability</param>
        /// <returns>int as a percentage to be used in dice rolls</returns>
        public static int ResourcesAvailabilityToInt(Resources availability) //@@MP (Release 7-4)
        {
            switch (availability)
            {
                case Resources.LOW: return 33;
                case Resources.MED: return 54;
                case Resources.HIGH: return 75;
                default:
                    throw new ArgumentOutOfRangeException("availability", "unhandled availability");
            }
        }

        public string DescribeValue(IDs option)//, GameMode mode) //@@MP - unused parameter (Release 5-7)
        {
            switch (option)
            {
                case IDs.DIFFICULTY_AGGRESSIVE_HUNGRY_CIVILIANS:
                    return IsAggressiveHungryCiviliansOn ? "ON    (default ON)" : "OFF   (default ON)";
                case IDs.DIFFICULTY_ALLOW_UNDEADS_EVOLUTION:
                    /*if (mode == GameMode.GM_VINTAGE) //@@MP - disabled as this will now be handled in-code rather than forcing options which is messy (Release 5-2)
                        return "---";
                    else*/
                    return AllowUndeadsEvolution ? "YES   (default YES)" : "NO    (default YES)";
                case IDs.GAME_CITY_SIZE:
                    return String.Format("{0:D2}*   (default {1:D2})", CitySize, GameOptions.DEFAULT_CITY_SIZE);
                case IDs.DIFFICULTY_DAY_ZERO_UNDEADS_PERCENT:
                    return String.Format("{0:D3}%  (default {1:D3}%)", DayZeroUndeadsPercent, GameOptions.DEFAULT_DAY_ZERO_UNDEADS_PERCENT);
                case IDs.GAME_DEATH_SCREENSHOT:
                    return IsDeathScreenshotOn ? "YES   (default YES)" : "NO    (default YES)";
                case IDs.GAME_DISTRICT_SIZE:
                    return String.Format("{0:D2}*   (default {1:D2})", DistrictSize, GameOptions.DEFAULT_DISTRICT_SIZE);
                case IDs.DIFFICULTY_MAX_CIVILIANS:
                    return String.Format("{0:D3}*  (default {1:D3})", MaxCivilians, GameOptions.DEFAULT_MAX_CIVILIANS);
                case IDs.GAME_MAX_ANIMALS:
                    return String.Format("{0:D3}*  (default {1:D3})", MaxAnimals, GameOptions.DEFAULT_MAX_ANIMALS);
                case IDs.DIFFICULTY_MAX_UNDEADS:
                    return String.Format("{0:D3}*  (default {1:D3})", MaxUndeads, GameOptions.DEFAULT_MAX_UNDEADS);
                case IDs.DIFFICULTY_NATGUARD_FACTOR:
                    return String.Format("{0:D3}%  (default {1:D3}%)", NatGuardFactor, GameOptions.DEFAULT_NATGUARD_FACTOR);
                case IDs.DIFFICULTY_BLACKOPS_RAIDS:
                    return BlackOpsRaidsEnabled ? "YES   (default YES)" : "NO    (default YES)";
                case IDs.DIFFICULTY_NPC_CAN_STARVE_TO_DEATH:
                    return NPCCanStarveToDeath ? "YES   (default YES)" : "NO    (default YES)";
                case IDs.GAME_PERMADEATH:
                    return IsPermadeathOn ? "YES   (default YES)" : "NO    (default YES)";
                //case IDs.DIFFICULTY_RATS_UPGRADE:
                /*if (mode == GameMode.GM_VINTAGE) //@@MP - disabled as this will now be handled in-code rather than forcing options which is messy (Release 5-2)
                    return "---";
                else*/
                    //return RatsUpgrade ? "YES   (default NO)" : "NO    (default NO)"; //@@MP (Release 5-7)
                /*case IDs.GAME_REINC_LIVING_RESTRICTED:  //@@MP (Release 5-7)
                    return IsLivingReincRestricted ? "YES   (default NO)" : "NO    (default NO)";*/
                /*case IDs.GAME_REINCARNATE_AS_RAT:
                    return CanReincarnateAsRat ? "YES   (default NO)" : "NO    (default NO)";*/  //@@MP (Release 5-7)
                /*case IDs.GAME_REINCARNATE_TO_SEWERS:
                    return CanReincarnateToSewers ? "YES   (default NO)" : "NO    (default NO)";*/  //@@MP (Release 5-7)
                /*case IDs.GAME_REVEAL_STARTING_DISTRICT: //@@MP (Release 6-1)
                    return RevealStartingDistrict ? "YES   (default YES)" : "NO    (default YES)";*/
                case IDs.DIFFICULTY_SANITY://@@MP (Release 1)
                    return IsSanityEnabled ? "ON    (default ON)" : "OFF   (default ON)";
                case IDs.DIFFICULTY_SHAMBLERS_UPGRADE:
                    /*if (mode == GameMode.GM_VINTAGE) //@@MP - disabled as this will now be handled in-code rather than forcing options which is messy (Release 5-2)
                        return "---";
                    else*/
                        return ShamblersUpgrade ? "YES   (default NO)" : "NO    (default NO)";
                case IDs.DIFFICULTY_SKELETONS_UPGRADE:
                    /*if (mode == GameMode.GM_VINTAGE) //@@MP - disabled as this will now be handled in-code rather than forcing options which is messy (Release 5-2)
                        return "---";
                    else*/
                        return SkeletonsUpgrade ? "YES   (default NO)" : "NO    (default NO)";
                case IDs.GAME_SIMULATE_DISTRICTS:
                    return String.Format("{0,-4}* (default {1})", GameOptions.Name(SimulateDistricts), GameOptions.Name(GameOptions.DEFAULT_SIM_DISTRICTS));
                case IDs.GAME_TURNS_SIM_CAP: //@@MP (Release 7-3)
                    return String.Format("{0,-4}* (default {1})", GameOptions.Name(SimTurnsCap), GameOptions.Name(GameOptions.DEFAULT_DISTRCT_SIM_CAP));
                case IDs.GAME_SIM_THREAD:
                    return SimThread ? "YES*  (default YES)" : "NO*   (default YES)";
                case IDs.GAME_SIMULATE_SLEEP:
                    return SimulateWhenSleeping ? "YES*  (default NO)" : "NO*   (default NO)";
                case IDs.GAME_DAYS_BEFORE_ITEM_DESPAWNS: //@@MP (Release 7-6)
                    return String.Format("{0:D3}*  (default {1:D3})", DaysBeforeDiscardedItemDespawns, GameOptions.DEFAULT_DAYS_BEFORE_ITEM_DESPAWNS);
                case IDs.GAME_REDUCED_MAPOBJECT_LIGHTING: //@@MP (Release 7-6)
                    return ReducedMapObjectLighting ? "ON    (default OFF)" : "OFF   (default OFF)";
                case IDs.GAME_WORLD_DECAY: //@@MP (Release 7-6)
                    return IsWorldDecayOn ? "ON    (default ON)" : "OFF   (default ON)";
                case IDs.GAME_DAYS_BEFORE_WORLD_DECAYS: //@@MP (Release 7-6)
                    return String.Format("{0:D3}   (default {1:D3})", DaysBeforeWorldDecays, GameOptions.DEFAULT_DAYS_BEFORE_WORLD_DECAYS);
                case IDs.DIFFICULTY_STARVED_ZOMBIFICATION:
                    return StarvedZombification ? "ON    (default OFF)" : "OFF   (default OFF)";
                case IDs.DIFFICULTY_SUPPLIESDROP_FACTOR:
                    return String.Format("{0:D3}%  (default {1:D3}%)", SuppliesDropFactor, GameOptions.DEFAULT_SUPPLIESDROP_FACTOR);
                case IDs.DIFFICULTY_UNDEADS_UPGRADE_DAYS:
                    return String.Format("{0:D3}   (default {1:D3})", GameOptions.Name(ZombifiedsUpgradeDays), GameOptions.Name(GameOptions.DEFAULT_ZOMBIFIEDS_UPGRADE_DAYS));
                case IDs.DIFFICULTY_ANTIVIRAL_PILLS: //@@MP (Release 7-6)
                    return AntiviralPills ? "YES   (default NO)" : "NO    (default NO)";
                case IDs.DIFFICULTY_ZOMBIE_INVASION_DAILY_INCREASE:
                    return String.Format("{0:D3}%  (default {1:D3}%)", ZombieInvasionDailyIncrease, GameOptions.DEFAULT_ZOMBIE_INVASION_DAILY_INCREASE);
                case IDs.DIFFICULTY_ZOMBIFICATION_CHANCE:
                    return String.Format("{0:D3}%  (default {1:D3}%)", ZombificationChance, GameOptions.DEFAULT_ZOMBIFICATION_CHANCE);
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
                case IDs.UI_AMBIENTSFXS: //@@MP (Release 6-1)
                    return PlayAmbientSFXs ? "ON " : "OFF";
                case IDs.UI_AMBIENTSFXS_VOLUME: //@@MP (Release 6-1)
                    return AmbientSFXVolume.ToString() + "%";
                case IDs.UI_SHOW_MINIMAP:
                    return IsMinimapOn ? "ON " : "OFF";
                case IDs.UI_SHOW_PLAYER_TARGETS:
                    return ShowPlayerTargets ? "ON    (default ON)" : "OFF   (default ON)";
                case IDs.UI_SHOW_TARGETS:
                    return ShowTargets ? "ON    (default ON)" : "OFF   (default ON)";
                case IDs.UI_AUTOSAVE: //@@MP (Release 6-1)
                    return Autosaving ? "ON    (default ON)" : "OFF   (default ON)";
                case IDs.UI_AI_VOCALISATION_CHANCE: //@@MP (Release 7-6)
                    return String.Format("{0:D3}%  (default {1})", AIVocalisationChance, GameOptions.DEFAULT_UI_AI_VOCALISATION_CHANCE);
                case IDs.DIFFICULTY_RESOURCES_AVAILABILITY: //@@MP (Release 7-4)
                    return String.Format("{0}   (default {1})", GameOptions.Name(ResourcesAvailability), GameOptions.Name(GameOptions.DEFAULT_RESOURCES_AVAILABILITY));
                case IDs.DIFFICULTY_RESCUE_DAY: //@@MP (Release 7-4)
                    return String.Format("{0} (default {1})", VisibleRescueDay == 6 ? "random" : VisibleRescueDay.ToString(), GameOptions.DEFAULT_RESCUE_DAY);
                case IDs.DIFFICULTY_UNDEAD_DAMAGE_PERCENT: //@@MP (Release 7-4)
                    return String.Format("{0:D3}%  (default {1})", UndeadDamagePercent, GameOptions.DEFAULT_UNDEAD_DAMAGE_PERCENT);
                case IDs.DIFFICULTY_LIVING_DAMAGE_PERCENT: //@@MP (Release 7-4)
                    return String.Format("{0:D3}%  (default {1})", LivingDamagePercent, GameOptions.DEFAULT_LIVING_DAMAGE_PERCENT);
                case IDs.DIFFICULTY_BACKPACKS: //@@MP (Release 8-2)
                    return BackpacksEnabled ? "ON    (default ON)" : "OFF   (default ON)";
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
            Stream stream = null; //@@MP - try/finally ensures that the stream is always closed (Release 5-7)
            try
            {
                stream = CreateStream(filepath, true);
                formatter.Serialize(stream, options);
                stream.Flush();
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

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
            if (File.Exists(filepath))
            {
                IFormatter formatter = CreateFormatter();
                Stream stream = null; //@@MP - try/finally ensures that the stream is always closed (Release 5-7)
                try
                {
                    stream = CreateStream(filepath, false);
                    options = (GameOptions)formatter.Deserialize(stream);
                    stream.Flush();
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }
            else
            {
                Logger.WriteLine(Logger.Stage.RUN_MAIN, "failed to load options (no custom options, probably first run?).");
                Logger.WriteLine(Logger.Stage.RUN_MAIN, "returning default values.");
                options = new GameOptions();
                options.ResetToDefaultValues(OptionsCategory.ALL);
                //set one-time defaults for audio volumes  //@@MP (Release 7-4)
                options.m_MusicVolume = 50;
                options.AmbientSFXVolume = 75;
                options.SFXVolume = 75;
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
            try
            {
                return new FileStream(saveFileName,
                save ? FileMode.Create : FileMode.Open,
                save ? FileAccess.Write : FileAccess.Read,
                FileShare.None);
            }
            catch (System.IO.FileNotFoundException)
            {
                return null;
            }
        }
#endregion
    }
}
