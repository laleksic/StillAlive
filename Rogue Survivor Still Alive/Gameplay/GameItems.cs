﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Engine.Items;

namespace djack.RogueSurvivor.Gameplay
{
    class GameItems : ItemModelDB
    {
        #region IDs
        public enum IDs
        {
            //_FIRST = 0, //@@MP - took _FIRST out because for some reason it became totally bugged (Release 4)

            MEDICINE_BANDAGES,
            MEDICINE_MEDIKIT,
            MEDICINE_PILLS_STA,
            MEDICINE_PILLS_SLP,
            MEDICINE_PILLS_SAN,
            MEDICINE_PILLS_ANTIVIRAL,
            //@@MP (Release 4)
            MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN,
            MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN,
            MEDICINE_ALCOHOL_BEER_CAN_BLUE,
            MEDICINE_ALCOHOL_BEER_CAN_RED,
            MEDICINE_ALCOHOL_LIQUOR_AMBER,
            MEDICINE_ALCOHOL_LIQUOR_CLEAR,
            MEDICINE_CIGARETTES,

            FOOD_ARMY_RATION,
            FOOD_GROCERIES,
            FOOD_CANNED_FOOD,
            FOOD_WILD_BERRIES, //@@MP (Release 4)

            MELEE_BASEBALLBAT,
            MELEE_COMBAT_KNIFE,
            MELEE_CROWBAR,
            UNIQUE_JASON_MYERS_AXE,
            MELEE_HUGE_HAMMER,
            MELEE_SMALL_HAMMER,
            MELEE_GOLFCLUB,
            MELEE_IRON_GOLFCLUB,
            MELEE_SHOVEL,
            MELEE_SHORT_SHOVEL,
            MELEE_TRUNCHEON,
            MELEE_IMPROVISED_CLUB,
            MELEE_IMPROVISED_SPEAR,

            RANGED_ARMY_PISTOL,
            RANGED_ARMY_RIFLE,
            RANGED_HUNTING_CROSSBOW,
            RANGED_HUNTING_RIFLE,
            RANGED_PISTOL,
            RANGED_KOLT_REVOLVER,
            RANGED_PRECISION_RIFLE,
            RANGED_SHOTGUN,

            EXPLOSIVE_GRENADE,
            EXPLOSIVE_GRENADE_PRIMED,
            //@@MP (Release 4)
            EXPLOSIVE_MOLOTOV,
            EXPLOSIVE_MOLOTOV_PRIMED,
            EXPLOSIVE_DYNAMITE,
            EXPLOSIVE_DYNAMITE_PRIMED,

            BAR_WOODEN_PLANK,

            ARMOR_ARMY_BODYARMOR,
            ARMOR_CHAR_LIGHT_BODYARMOR,
            ARMOR_HELLS_SOULS_JACKET,
            ARMOR_FREE_ANGELS_JACKET,
            ARMOR_POLICE_JACKET,
            ARMOR_POLICE_RIOT,
            ARMOR_HUNTER_VEST,

            TRACKER_BLACKOPS,
            TRACKER_CELL_PHONE,
            TRACKER_ZTRACKER,
            TRACKER_POLICE_RADIO,

            SPRAY_PAINT1,
            SPRAY_PAINT2,
            SPRAY_PAINT3,
            SPRAY_PAINT4,

            SCENT_SPRAY_STENCH_KILLER,

            LIGHT_FLASHLIGHT,
            LIGHT_BIG_FLASHLIGHT,

            AMMO_LIGHT_PISTOL,
            AMMO_HEAVY_PISTOL,
            AMMO_LIGHT_RIFLE,
            AMMO_HEAVY_RIFLE,
            AMMO_SHOTGUN,
            AMMO_BOLTS,

            TRAP_EMPTY_CAN,
            TRAP_BEAR_TRAP,
            TRAP_SPIKES,
            TRAP_BARBED_WIRE,

            ENT_BOOK,
            ENT_MAGAZINE,

            UNIQUE_SUBWAY_BADGE,
            UNIQUE_FAMU_FATARU_KATANA,
            UNIQUE_BIGBEAR_BAT,
            UNIQUE_ROGUEDJACK_KEYBOARD,
            UNIQUE_SANTAMAN_SHOTGUN,
            UNIQUE_HANS_VON_HANZ_PISTOL,

            //@@MP (Release 3)
            UNIQUE_CHAR_DOCUMENT1,
            UNIQUE_CHAR_DOCUMENT2,
            UNIQUE_CHAR_DOCUMENT3,
            UNIQUE_CHAR_DOCUMENT4,
            UNIQUE_CHAR_DOCUMENT5,
            UNIQUE_CHAR_DOCUMENT6,

            //@@MP (Release 3)
            MELEE_TENNIS_RACKET,
            MELEE_HOCKEY_STICK,
            MELEE_MACHETE,
            MELEE_STANDARD_AXE,
            MELEE_PICKAXE,
            MELEE_PIPE_WRENCH,

            _COUNT
        }
        #endregion

        #region Fields
        ItemModel[] m_Models = new ItemModel[(int)IDs._COUNT];
        #endregion

        #region Properties
        public override ItemModel this[int id]
        {
            get { return m_Models[id]; }
        }

        public ItemModel this[IDs id]
        {
            get { return this[(int)id]; }
            private set
            {
                m_Models[(int)id] = value;
                m_Models[(int)id].ID = (int)id;
            }
        }

        #region Medicine
        struct MedecineData
        {
            public const int COUNT_FIELDS = 10;

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int STACKINGLIMIT { get; set; }
            public int HEALING { get; set; }
            public int STAMINABOOST { get; set; }
            public int SLEEPBOOST { get; set; }
            public int INFECTIONCURE { get; set; }
            public int SANITYCURE { get; set; }
            public string FLAVOR { get; set; }

            public static MedecineData FromCSVLine(CSVLine line)
            {
                return new MedecineData()
                {
                    NAME = line[1].ParseText(),
                    PLURAL = line[2].ParseText(),
                    HEALING = line[3].ParseInt(),
                    STAMINABOOST = line[4].ParseInt(),
                    SLEEPBOOST = line[5].ParseInt(),
                    INFECTIONCURE = line[6].ParseInt(),
                    SANITYCURE = line[7].ParseInt(),
                    STACKINGLIMIT = line[8].ParseInt(),
                    FLAVOR = line[9].ParseText()
                };
            }
        }

        MedecineData DATA_MEDICINE_BANDAGE;
        public ItemMedicineModel BANDAGE { get { return this[IDs.MEDICINE_BANDAGES] as ItemMedicineModel; } }
        MedecineData DATA_MEDICINE_MEDIKIT;
        public ItemMedicineModel MEDIKIT { get { return this[IDs.MEDICINE_MEDIKIT] as ItemMedicineModel; } }
        MedecineData DATA_MEDICINE_PILLS_STA;
        public ItemMedicineModel PILLS_STA { get { return this[IDs.MEDICINE_PILLS_STA] as ItemMedicineModel; } }
        MedecineData DATA_MEDICINE_PILLS_SLP;
        public ItemMedicineModel PILLS_SLP { get { return this[IDs.MEDICINE_PILLS_SLP] as ItemMedicineModel; } }
        MedecineData DATA_MEDICINE_PILLS_SAN;
        public ItemMedicineModel PILLS_SAN { get { return this[IDs.MEDICINE_PILLS_SAN] as ItemMedicineModel; } }
        MedecineData DATA_MEDICINE_PILLS_ANTIVIRAL;
        public ItemMedicineModel PILLS_ANTIVIRAL { get { return this[IDs.MEDICINE_PILLS_ANTIVIRAL] as ItemMedicineModel; } }
        //@@MP (Release 4)
        MedecineData DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN;
        public ItemMedicineModel ALCOHOL_BEER_BOTTLE_BROWN { get { return this[IDs.MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN] as ItemMedicineModel; } }
        MedecineData DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN;
        public ItemMedicineModel ALCOHOL_BEER_BOTTLE_GREEN { get { return this[IDs.MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN] as ItemMedicineModel; } }
        MedecineData DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE;
        public ItemMedicineModel ALCOHOL_BEER_CAN_BLUE { get { return this[IDs.MEDICINE_ALCOHOL_BEER_CAN_BLUE] as ItemMedicineModel; } }
        MedecineData DATA_MEDICINE_ALCOHOL_BEER_CAN_RED;
        public ItemMedicineModel ALCOHOL_BEER_CAN_RED { get { return this[IDs.MEDICINE_ALCOHOL_BEER_CAN_RED] as ItemMedicineModel; } }
        MedecineData DATA_MEDICINE_ALCOHOL_LIQUOR_AMBER;
        public ItemMedicineModel ALCOHOL_LIQUOR_AMBER { get { return this[IDs.MEDICINE_ALCOHOL_LIQUOR_AMBER] as ItemMedicineModel; } }
        MedecineData DATA_MEDICINE_ALCOHOL_LIQUOR_CLEAR;
        public ItemMedicineModel ALCOHOL_LIQUOR_CLEAR { get { return this[IDs.MEDICINE_ALCOHOL_LIQUOR_CLEAR] as ItemMedicineModel; } }
        MedecineData DATA_MEDICINE_CIGARETTES;
        public ItemMedicineModel CIGARETTES { get { return this[IDs.MEDICINE_CIGARETTES] as ItemMedicineModel; } }
        #endregion

        #region Food
        struct FoodData
        {
            public const int COUNT_FIELDS = 7;

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int NUTRITION { get; set; }
            public int BESTBEFORE { get; set; }
            public int STACKINGLIMIT { get; set; }
            public string FLAVOR { get; set; }

            public static FoodData FromCSVLine(CSVLine line)
            {
                return new FoodData()
                {
                    NAME = line[1].ParseText(),
                    PLURAL = line[2].ParseText(),
                    NUTRITION = (int)(Rules.FOOD_BASE_POINTS * line[3].ParseFloat()),
                    BESTBEFORE = line[4].ParseInt(),
                    STACKINGLIMIT = line[5].ParseInt(),
                    FLAVOR = line[6].ParseText()
                };
            }
        }

        FoodData DATA_FOOD_ARMY_RATION;
        public ItemFoodModel ARMY_RATION { get { return this[IDs.FOOD_ARMY_RATION] as ItemFoodModel; } }
        FoodData DATA_FOOD_GROCERIES;
        public ItemFoodModel GROCERIES { get { return this[IDs.FOOD_GROCERIES] as ItemFoodModel; } }
        FoodData DATA_FOOD_CANNED_FOOD;
        public ItemFoodModel CANNED_FOOD { get { return this[IDs.FOOD_CANNED_FOOD] as ItemFoodModel; } }
        FoodData DATA_FOOD_WILD_BERRIES; //@MP (Release 4)
        public ItemFoodModel WILD_BERRIES { get { return this[IDs.FOOD_WILD_BERRIES] as ItemFoodModel; } }
        #endregion

        #region Melee weapons
        struct MeleeWeaponData
        {
            public const int COUNT_FIELDS = 9;

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int ATK { get; set; }
            public int DMG { get; set; }
            public int STA { get; set; }
            public int STACKINGLIMIT { get; set; }
            public bool ISFRAGILE { get; set; }
            public string FLAVOR { get; set; }

            public static MeleeWeaponData FromCSVLine(CSVLine line)
            {
                return new MeleeWeaponData()
                {
                    NAME = line[1].ParseText(),
                    PLURAL = line[2].ParseText(),
                    ATK = line[3].ParseInt(),
                    DMG = line[4].ParseInt(),
                    STA = line[5].ParseInt(),
                    STACKINGLIMIT = line[6].ParseInt(),
                    ISFRAGILE = line[7].ParseBool(),
                    FLAVOR = line[8].ParseText()
                };
            }
        }

        MeleeWeaponData DATA_MELEE_CROWBAR;
        public ItemMeleeWeaponModel CROWBAR { get { return this[IDs.MELEE_CROWBAR] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_BASEBALLBAT;
        public ItemMeleeWeaponModel BASEBALLBAT { get { return this[IDs.MELEE_BASEBALLBAT] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_COMBAT_KNIFE;
        public ItemMeleeWeaponModel COMBAT_KNIFE { get { return this[IDs.MELEE_COMBAT_KNIFE] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_UNIQUE_JASON_MYERS_AXE;
        public ItemMeleeWeaponModel UNIQUE_JASON_MYERS_AXE { get { return this[IDs.UNIQUE_JASON_MYERS_AXE] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_GOLFCLUB;
        public ItemMeleeWeaponModel GOLFCLUB { get { return this[IDs.MELEE_GOLFCLUB] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_HUGE_HAMMER;
        public ItemMeleeWeaponModel HUGE_HAMMER { get { return this[IDs.MELEE_HUGE_HAMMER] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_SMALL_HAMMER;
        public ItemMeleeWeaponModel SMALL_HAMMER { get { return this[IDs.MELEE_SMALL_HAMMER] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_IRON_GOLFCLUB;
        public ItemMeleeWeaponModel IRON_GOLFCLUB { get { return this[IDs.MELEE_IRON_GOLFCLUB] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_SHOVEL;
        public ItemMeleeWeaponModel SHOVEL { get { return this[IDs.MELEE_SHOVEL] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_SHORT_SHOVEL;
        public ItemMeleeWeaponModel SHORT_SHOVEL { get { return this[IDs.MELEE_SHORT_SHOVEL] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_TRUNCHEON;
        public ItemMeleeWeaponModel TRUNCHEON { get { return this[IDs.MELEE_TRUNCHEON] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_IMPROVISED_CLUB;
        public ItemMeleeWeaponModel IMPROVISED_CLUB { get { return this[IDs.MELEE_IMPROVISED_CLUB] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_IMPROVISED_SPEAR;
        public ItemMeleeWeaponModel IMPROVISED_SPEAR { get { return this[IDs.MELEE_IMPROVISED_SPEAR] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_UNIQUE_FAMU_FATARU_KATANA;
        public ItemMeleeWeaponModel UNIQUE_FAMU_FATARU_KATANA { get { return this[IDs.UNIQUE_FAMU_FATARU_KATANA] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_UNIQUE_BIGBEAR_BAT;
        public ItemMeleeWeaponModel UNIQUE_BIGBEAR_BAT { get { return this[IDs.UNIQUE_BIGBEAR_BAT] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_UNIQUE_ROGUEDJACK_KEYBOARD;
        public ItemMeleeWeaponModel UNIQUE_ROGUEDJACK_KEYBOARD { get { return this[IDs.UNIQUE_ROGUEDJACK_KEYBOARD] as ItemMeleeWeaponModel; } }
        //@@MP (Release 3)
        MeleeWeaponData DATA_MELEE_TENNIS_RACKET;
        public ItemMeleeWeaponModel TENNIS_RACKET { get { return this[IDs.MELEE_TENNIS_RACKET] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_HOCKEY_STICK;
        public ItemMeleeWeaponModel HOCKEY_STICK { get { return this[IDs.MELEE_HOCKEY_STICK] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_MACHETE;
        public ItemMeleeWeaponModel MACHETE { get { return this[IDs.MELEE_MACHETE] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_STANDARD_AXE;
        public ItemMeleeWeaponModel STANDARD_AXE { get { return this[IDs.MELEE_STANDARD_AXE] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_PICKAXE;
        public ItemMeleeWeaponModel PICKAXE { get { return this[IDs.MELEE_PICKAXE] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_PIPE_WRENCH;
        public ItemMeleeWeaponModel PIPE_WRENCH { get { return this[IDs.MELEE_PIPE_WRENCH] as ItemMeleeWeaponModel; } }
        #endregion

        #region Ranged weapons
        struct RangedWeaponData
        {
            public const int COUNT_FIELDS = 8;

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int ATK { get; set; }
            public int DMG { get; set; }
            public int RANGE { get; set; }
            public int MAXAMMO { get; set; }
            public string FLAVOR { get; set; }

            public static RangedWeaponData FromCSVLine(CSVLine line)
            {
                return new RangedWeaponData()
                {
                    NAME = line[1].ParseText(),
                    PLURAL = line[2].ParseText(),
                    ATK = line[3].ParseInt(),
                    DMG = line[4].ParseInt(),
                    RANGE = line[5].ParseInt(),
                    MAXAMMO = line[6].ParseInt(),
                    FLAVOR = line[7].ParseText()
                };
            }
        }

        RangedWeaponData DATA_RANGED_ARMY_PISTOL;
        public ItemRangedWeaponModel ARMY_PISTOL { get { return this[IDs.RANGED_ARMY_PISTOL] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_ARMY_RIFLE;
        public ItemRangedWeaponModel ARMY_RIFLE { get { return this[IDs.RANGED_ARMY_RIFLE] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_HUNTING_CROSSBOW;
        public ItemRangedWeaponModel HUNTING_CROSSBOW { get { return this[IDs.RANGED_HUNTING_CROSSBOW] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_HUNTING_RIFLE;
        public ItemRangedWeaponModel HUNTING_RIFLE { get { return this[IDs.RANGED_HUNTING_RIFLE] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_KOLT_REVOLVER;
        public ItemRangedWeaponModel KOLT_REVOLVER { get { return this[IDs.RANGED_KOLT_REVOLVER] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_PISTOL;
        public ItemRangedWeaponModel PISTOL { get { return this[IDs.RANGED_PISTOL] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_PRECISION_RIFLE;
        public ItemRangedWeaponModel PRECISION_RIFLE { get { return this[IDs.RANGED_PRECISION_RIFLE] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_SHOTGUN;
        public ItemRangedWeaponModel SHOTGUN { get { return this[IDs.RANGED_SHOTGUN] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_UNIQUE_SANTAMAN_SHOTGUN;
        public ItemRangedWeaponModel UNIQUE_SANTAMAN_SHOTGUN { get { return this[IDs.UNIQUE_SANTAMAN_SHOTGUN] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_UNIQUE_HANS_VON_HANZ_PISTOL;
        public ItemRangedWeaponModel UNIQUE_HANS_VON_HANZ_PISTOL { get { return this[IDs.UNIQUE_HANS_VON_HANZ_PISTOL] as ItemRangedWeaponModel; } }
        #endregion

        #region Ammos
        public ItemAmmoModel AMMO_LIGHT_PISTOL { get { return this[IDs.AMMO_LIGHT_PISTOL] as ItemAmmoModel; } }
        public ItemAmmoModel AMMO_HEAVY_PISTOL { get { return this[IDs.AMMO_HEAVY_PISTOL] as ItemAmmoModel; } }
        public ItemAmmoModel AMMO_LIGHT_RIFLE { get { return this[IDs.AMMO_LIGHT_RIFLE] as ItemAmmoModel; } }
        public ItemAmmoModel AMMO_HEAVY_RIFLE { get { return this[IDs.AMMO_HEAVY_RIFLE] as ItemAmmoModel; } }
        public ItemAmmoModel AMMO_SHOTGUN { get { return this[IDs.AMMO_SHOTGUN] as ItemAmmoModel; } }
        public ItemAmmoModel AMMO_BOLTS { get { return this[IDs.AMMO_BOLTS] as ItemAmmoModel; } }
        #endregion

        #region Explosives
        struct ExplosiveData
        {
            public const int COUNT_FIELDS = 14;

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int FUSE { get; set; }
            public int MAXTHROW { get; set; }
            public int STACKLINGLIMIT { get; set; }
            public int RADIUS { get; set; }
            public int[] DMG { get; set; }
            public string FLAVOR { get; set; }

            public static ExplosiveData FromCSVLine(CSVLine line)
            {
                return new ExplosiveData()
                {
                    NAME = line[1].ParseText(),
                    PLURAL = line[2].ParseText(),
                    FUSE = line[3].ParseInt(),
                    MAXTHROW = line[4].ParseInt(),
                    STACKLINGLIMIT = line[5].ParseInt(),
                    RADIUS = line[6].ParseInt(),
                    DMG = new int[6] {
                        line[7].ParseInt(),
                        line[8].ParseInt(),
                        line[9].ParseInt(),
                        line[10].ParseInt(),
                        line[11].ParseInt(),
                        line[12].ParseInt() },
                    FLAVOR = line[13].ParseText()
                };
            }
        }

        ExplosiveData DATA_EXPLOSIVE_GRENADE;
        public ItemGrenadeModel GRENADE { get { return this[IDs.EXPLOSIVE_GRENADE] as ItemGrenadeModel; } }
        public ItemGrenadePrimedModel GRENADE_PRIMED { get { return this[IDs.EXPLOSIVE_GRENADE_PRIMED] as ItemGrenadePrimedModel; } }
        //@@MP (Release 4)
        ExplosiveData DATA_EXPLOSIVE_MOLOTOV;
        public ItemGrenadeModel MOLOTOV { get { return this[IDs.EXPLOSIVE_MOLOTOV] as ItemGrenadeModel; } }
        public ItemGrenadePrimedModel MOLOTOV_PRIMED { get { return this[IDs.EXPLOSIVE_MOLOTOV_PRIMED] as ItemGrenadePrimedModel; } }
        ExplosiveData DATA_EXPLOSIVE_DYNAMITE;
        public ItemGrenadeModel DYNAMITE { get { return this[IDs.EXPLOSIVE_DYNAMITE] as ItemGrenadeModel; } }
        public ItemGrenadePrimedModel DYNAMITE_PRIMED { get { return this[IDs.EXPLOSIVE_DYNAMITE_PRIMED] as ItemGrenadePrimedModel; } }
        #endregion

        #region Barricades
        struct BarricadingMaterialData
        {
            public const int COUNT_FIELDS = 6;

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int VALUE { get; set; }
            public int STACKINGLIMIT { get; set; }
            public string FLAVOR { get; set; }

            public static BarricadingMaterialData FromCSVLine(CSVLine line)
            {
                return new BarricadingMaterialData()
                {
                    NAME = line[1].ParseText(),
                    PLURAL = line[2].ParseText(),
                    VALUE = line[3].ParseInt(),
                    STACKINGLIMIT = line[4].ParseInt(),
                    FLAVOR = line[5].ParseText()
                };
            }
        }

        BarricadingMaterialData DATA_BAR_WOODEN_PLANK;
        public ItemBarricadeMaterialModel WOODENPLANK { get { return this[IDs.BAR_WOODEN_PLANK] as ItemBarricadeMaterialModel; } }
        #endregion

        #region Body Armors
        struct ArmorData
        {
            public const int COUNT_FIELDS = 8;

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int PRO_HIT { get; set; }
            public int PRO_SHOT { get; set; }
            public int ENC { get; set; }
            public int WEIGHT { get; set; }
            public string FLAVOR { get; set; }

            public static ArmorData FromCSVLine(CSVLine line)
            {
                return new ArmorData()
                {
                    NAME = line[1].ParseText(),
                    PLURAL = line[2].ParseText(),
                    PRO_HIT = line[3].ParseInt(),
                    PRO_SHOT = line[4].ParseInt(),
                    ENC = line[5].ParseInt(),
                    WEIGHT = line[6].ParseInt(),
                    FLAVOR = line[7].ParseText()
                };
            }
        }

        ArmorData DATA_ARMOR_ARMY;
        public ItemBodyArmorModel ARMY_BODYARMOR { get { return this[IDs.ARMOR_ARMY_BODYARMOR] as ItemBodyArmorModel; } }
        ArmorData DATA_ARMOR_CHAR;
        public ItemBodyArmorModel CHAR_LT_BODYARMOR { get { return this[IDs.ARMOR_CHAR_LIGHT_BODYARMOR] as ItemBodyArmorModel; } }
        ArmorData DATA_ARMOR_HELLS_SOULS_JACKET;
        public ItemBodyArmorModel HELLS_SOULS_JACKET { get { return this[IDs.ARMOR_HELLS_SOULS_JACKET] as ItemBodyArmorModel; } }
        ArmorData DATA_ARMOR_FREE_ANGELS_JACKET;
        public ItemBodyArmorModel FREE_ANGELS_JACKET { get { return this[IDs.ARMOR_FREE_ANGELS_JACKET] as ItemBodyArmorModel; } }
        ArmorData DATA_ARMOR_POLICE_JACKET;
        public ItemBodyArmorModel POLICE_JACKET { get { return this[IDs.ARMOR_POLICE_JACKET] as ItemBodyArmorModel; } }
        ArmorData DATA_ARMOR_POLICE_RIOT;
        public ItemBodyArmorModel POLICE_RIOT { get { return this[IDs.ARMOR_POLICE_RIOT] as ItemBodyArmorModel; } }
        ArmorData DATA_ARMOR_HUNTER_VEST;
        public ItemBodyArmorModel HUNTER_VEST { get { return this[IDs.ARMOR_HUNTER_VEST] as ItemBodyArmorModel; } }
        #endregion

        #region Trackers
        struct TrackerData
        {
            public const int COUNT_FIELDS = 5;

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int BATTERIES { get; set; }
            public string FLAVOR { get; set; }

            public static TrackerData FromCSVLine(CSVLine line)
            {
                return new TrackerData()
                {
                    NAME = line[1].ParseText(),
                    PLURAL = line[2].ParseText(),
                    BATTERIES = line[3].ParseInt(),
                    FLAVOR = line[4].ParseText()
                };
            }
        }

        TrackerData DATA_TRACKER_BLACKOPS_GPS;
        public ItemTrackerModel BLACKOPS_GPS { get { return this[IDs.TRACKER_BLACKOPS] as ItemTrackerModel; } }
        TrackerData DATA_TRACKER_CELL_PHONE;
        public ItemTrackerModel CELL_PHONE { get { return this[IDs.TRACKER_CELL_PHONE] as ItemTrackerModel; } }
        TrackerData DATA_TRACKER_ZTRACKER;
        public ItemTrackerModel ZTRACKER { get { return this[IDs.TRACKER_ZTRACKER] as ItemTrackerModel; } }
        TrackerData DATA_TRACKER_POLICE_RADIO;
        public ItemTrackerModel POLICE_RADIO { get { return this[IDs.TRACKER_POLICE_RADIO] as ItemTrackerModel; } }
        #endregion

        #region Spray Paint
        struct SprayPaintData
        {
            public const int COUNT_FIELDS = 5;

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int QUANTITY { get; set; }
            public string FLAVOR { get; set; }

            public static SprayPaintData FromCSVLine(CSVLine line)
            {
                return new SprayPaintData()
                {
                    NAME = line[1].ParseText(),
                    PLURAL = line[2].ParseText(),
                    QUANTITY = line[3].ParseInt(),
                    FLAVOR = line[4].ParseText()
                };
            }
        }

        SprayPaintData DATA_SPRAY_PAINT1;
        public ItemSprayPaintModel SPRAY_PAINT1 { get { return this[IDs.SPRAY_PAINT1] as ItemSprayPaintModel; } }
        SprayPaintData DATA_SPRAY_PAINT2;
        public ItemSprayPaintModel SPRAY_PAINT2 { get { return this[IDs.SPRAY_PAINT2] as ItemSprayPaintModel; } }
        SprayPaintData DATA_SPRAY_PAINT3;
        public ItemSprayPaintModel SPRAY_PAINT3 { get { return this[IDs.SPRAY_PAINT3] as ItemSprayPaintModel; } }
        SprayPaintData DATA_SPRAY_PAINT4;
        public ItemSprayPaintModel SPRAY_PAINT4 { get { return this[IDs.SPRAY_PAINT4] as ItemSprayPaintModel; } }
        #endregion

        #region Lights
        struct LightData
        {
            public const int COUNT_FIELDS = 6;

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int FOV { get; set; }
            public int BATTERIES { get; set; }
            public string FLAVOR { get; set; }

            public static LightData FromCSVLine(CSVLine line)
            {
                return new LightData()
                {
                    NAME = line[1].ParseText(),
                    PLURAL = line[2].ParseText(),
                    FOV = line[3].ParseInt(),
                    BATTERIES = line[4].ParseInt(),
                    FLAVOR = line[5].ParseText()
                };
            }
        }

        LightData DATA_LIGHT_FLASHLIGHT;
        public ItemLightModel FLASHLIGHT { get { return this[IDs.LIGHT_FLASHLIGHT] as ItemLightModel; } }
        LightData DATA_LIGHT_BIG_FLASHLIGHT;
        public ItemLightModel BIG_FLASHLIGHT { get { return this[IDs.LIGHT_BIG_FLASHLIGHT] as ItemLightModel; } }
        #endregion

        #region Scent Sprays
        struct ScentSprayData
        {
            public const int COUNT_FIELDS = 6;

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int QUANTITY { get; set; }
            public int STRENGTH { get; set; }
            public string FLAVOR { get; set; }

            public static ScentSprayData FromCSVLine(CSVLine line)
            {
                return new ScentSprayData()
                {
                    NAME = line[1].ParseText(),
                    PLURAL = line[2].ParseText(),
                    QUANTITY = line[3].ParseInt(),
                    STRENGTH = line[4].ParseInt(),
                    FLAVOR = line[5].ParseText()
                };
            }
        }

        ScentSprayData DATA_SCENT_SPRAY_STENCH_KILLER;
        public ItemModel STENCH_KILLER { get { return this[IDs.SCENT_SPRAY_STENCH_KILLER]; } }
        #endregion

        #region Traps
        struct TrapData
        {
            public const int COUNT_FIELDS = 16;

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int STACKING { get; set; }
            public bool USE_ACTIVATE { get; set; }
            public int CHANCE { get; set; }
            public int DAMAGE { get; set; }
            public bool DROP_ACTIVATE { get; set; }
            public bool IS_ONE_TIME { get; set; }
            public int BREAK_CHANCE { get; set; }
            public int BLOCK_CHANCE { get; set; }
            public int BREAK_CHANCE_ESCAPE { get; set; }
            public bool IS_NOISY { get; set; }
            public string NOISE_NAME { get; set; }
            public bool IS_FLAMMABLE { get; set; }
            public string FLAVOR { get; set; }

            public static TrapData FromCSVLine(CSVLine line)
            {
                return new TrapData()
                {
                    NAME = line[1].ParseText(),
                    PLURAL = line[2].ParseText(),
                    STACKING = line[3].ParseInt(),
                    DROP_ACTIVATE = line[4].ParseBool(),
                    USE_ACTIVATE = line[5].ParseBool(),
                    CHANCE = line[6].ParseInt(),
                    DAMAGE = line[7].ParseInt(),
                    IS_ONE_TIME = line[8].ParseBool(),
                    BREAK_CHANCE = line[9].ParseInt(),
                    BLOCK_CHANCE = line[10].ParseInt(),
                    BREAK_CHANCE_ESCAPE = line[11].ParseInt(),
                    IS_NOISY = line[12].ParseBool(),
                    NOISE_NAME = line[13].ParseText(),
                    IS_FLAMMABLE = line[14].ParseBool(),
                    FLAVOR = line[15].ParseText()
                };
            }
        }

        TrapData DATA_TRAP_EMPTY_CAN;
        public ItemModel EMPTY_CAN { get { return this[IDs.TRAP_EMPTY_CAN]; } }
        TrapData DATA_TRAP_BEAR_TRAP;
        public ItemModel BEAR_TRAP { get { return this[IDs.TRAP_BEAR_TRAP]; } }
        TrapData DATA_TRAP_SPIKES;
        public ItemModel SPIKES { get { return this[IDs.TRAP_SPIKES]; } }
        TrapData DATA_TRAP_BARBED_WIRE;
        public ItemModel BARBED_WIRE { get { return this[IDs.TRAP_BARBED_WIRE]; } }

        #endregion

        #region Entertainment
        struct EntData
        {
            public const int COUNT_FIELDS = 7;

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int STACKING { get; set; }
            public int VALUE { get; set; }
            public int BORECHANCE { get; set; }
            public string FLAVOR { get; set; }

            public static EntData FromCSVLine(CSVLine line)
            {
                return new EntData()
                {
                    NAME = line[1].ParseText(),
                    PLURAL = line[2].ParseText(),
                    STACKING = line[3].ParseInt(),
                    VALUE = line[4].ParseInt(),
                    BORECHANCE = line[5].ParseInt(),
                    FLAVOR = line[6].ParseText()
                };
            }
        }

        EntData DATA_ENT_BOOK;
        public ItemModel BOOK { get { return this[IDs.ENT_BOOK]; } }
        EntData DATA_ENT_MAGAZINE;
        public ItemModel MAGAZINE { get { return this[IDs.ENT_MAGAZINE]; } }
        #endregion

        #region Special Uniques
        public ItemModel UNIQUE_SUBWAY_BADGE { get { return this[IDs.UNIQUE_SUBWAY_BADGE]; } }
        //@MP (Release 3)
        public ItemModel UNIQUE_CHAR_DOCUMENT1 { get { return this[IDs.UNIQUE_CHAR_DOCUMENT1]; } }
        public ItemModel UNIQUE_CHAR_DOCUMENT2 { get { return this[IDs.UNIQUE_CHAR_DOCUMENT2]; } }
        public ItemModel UNIQUE_CHAR_DOCUMENT3 { get { return this[IDs.UNIQUE_CHAR_DOCUMENT3]; } }
        public ItemModel UNIQUE_CHAR_DOCUMENT4 { get { return this[IDs.UNIQUE_CHAR_DOCUMENT4]; } }
        public ItemModel UNIQUE_CHAR_DOCUMENT5 { get { return this[IDs.UNIQUE_CHAR_DOCUMENT5]; } }
        public ItemModel UNIQUE_CHAR_DOCUMENT6 { get { return this[IDs.UNIQUE_CHAR_DOCUMENT6]; } }
        #endregion

        #endregion

        #region Init
        public GameItems()
        {
            // bind
            Models.Items = this;
        }

        #region Grammar Helpers
        bool StartsWithVowel(string name)
        {
            return name[0] == 'a' || name[0] == 'A' || 
                name[0] == 'e' || name[0] == 'E' ||
                name[0] == 'i' || name[0] == 'I' ||
                name[0] == 'y' || name[0] == 'Y';
        }

        bool CheckPlural(string name, string plural)
        {
            return name == plural;
        }
        #endregion

        /// <summary>
        /// FIXME: clean up the code (sometimes uses temp local var, sometimes use explicit model)
        /// </summary>
        public void CreateModels()
        {
            #region Medicine
            this[IDs.MEDICINE_BANDAGES] = new ItemMedicineModel(DATA_MEDICINE_BANDAGE.NAME, DATA_MEDICINE_BANDAGE.PLURAL, GameImages.ITEM_BANDAGES,
                DATA_MEDICINE_BANDAGE.HEALING, DATA_MEDICINE_BANDAGE.STAMINABOOST, DATA_MEDICINE_BANDAGE.SLEEPBOOST, DATA_MEDICINE_BANDAGE.INFECTIONCURE, DATA_MEDICINE_BANDAGE.SANITYCURE)
            {
                IsPlural = true,
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_BANDAGE.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_BANDAGE.FLAVOR
            };

            this[IDs.MEDICINE_MEDIKIT] = new ItemMedicineModel(DATA_MEDICINE_MEDIKIT.NAME, DATA_MEDICINE_MEDIKIT.PLURAL, GameImages.ITEM_MEDIKIT,
                DATA_MEDICINE_MEDIKIT.HEALING, DATA_MEDICINE_MEDIKIT.STAMINABOOST, DATA_MEDICINE_MEDIKIT.SLEEPBOOST, DATA_MEDICINE_MEDIKIT.INFECTIONCURE, DATA_MEDICINE_MEDIKIT.SANITYCURE)
            {
                FlavorDescription = DATA_MEDICINE_MEDIKIT.FLAVOR
            };

            this[IDs.MEDICINE_PILLS_STA] = new ItemMedicineModel(DATA_MEDICINE_PILLS_STA.NAME, DATA_MEDICINE_PILLS_STA.PLURAL, GameImages.ITEM_PILLS_GREEN,
                DATA_MEDICINE_PILLS_STA.HEALING, DATA_MEDICINE_PILLS_STA.STAMINABOOST, DATA_MEDICINE_PILLS_STA.SLEEPBOOST, DATA_MEDICINE_PILLS_STA.INFECTIONCURE, DATA_MEDICINE_PILLS_STA.SANITYCURE)
            {
                IsPlural = true,
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_PILLS_STA.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_PILLS_STA.FLAVOR
            };

            this[IDs.MEDICINE_PILLS_SLP] = new ItemMedicineModel(DATA_MEDICINE_PILLS_SLP.NAME, DATA_MEDICINE_PILLS_SLP.PLURAL, GameImages.ITEM_PILLS_BLUE,
                DATA_MEDICINE_PILLS_SLP.HEALING, DATA_MEDICINE_PILLS_SLP.STAMINABOOST, DATA_MEDICINE_PILLS_SLP.SLEEPBOOST, DATA_MEDICINE_PILLS_SLP.INFECTIONCURE, DATA_MEDICINE_PILLS_SLP.SANITYCURE)
            {
                IsPlural = true,
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_PILLS_SLP.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_PILLS_SLP.FLAVOR
            };
            this[IDs.MEDICINE_PILLS_SAN] = new ItemMedicineModel(DATA_MEDICINE_PILLS_SAN.NAME, DATA_MEDICINE_PILLS_SAN.PLURAL, GameImages.ITEM_PILLS_SAN,
                DATA_MEDICINE_PILLS_SAN.HEALING, DATA_MEDICINE_PILLS_SAN.STAMINABOOST, DATA_MEDICINE_PILLS_SAN.SLEEPBOOST, DATA_MEDICINE_PILLS_SAN.INFECTIONCURE, DATA_MEDICINE_PILLS_SAN.SANITYCURE)
            {
                IsPlural = true,
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_PILLS_SAN.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_PILLS_SAN.FLAVOR
            };
            this[IDs.MEDICINE_PILLS_ANTIVIRAL] = new ItemMedicineModel(DATA_MEDICINE_PILLS_ANTIVIRAL.NAME, DATA_MEDICINE_PILLS_ANTIVIRAL.PLURAL, GameImages.ITEM_PILLS_ANTIVIRAL,
                DATA_MEDICINE_PILLS_ANTIVIRAL.HEALING, DATA_MEDICINE_PILLS_ANTIVIRAL.STAMINABOOST, DATA_MEDICINE_PILLS_ANTIVIRAL.SLEEPBOOST, DATA_MEDICINE_PILLS_ANTIVIRAL.INFECTIONCURE, DATA_MEDICINE_PILLS_ANTIVIRAL.SANITYCURE)
            {
                IsPlural = true,
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_PILLS_ANTIVIRAL.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_PILLS_ANTIVIRAL.FLAVOR
            };
            //@@MP (Release 4)
            this[IDs.MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN] = new ItemMedicineModel(DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN.NAME, DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN.PLURAL, GameImages.ITEM_BEER_BOTTLE_BROWN,
                DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN.HEALING, DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN.STAMINABOOST, DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN.SLEEPBOOST, DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN.INFECTIONCURE, DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN.SANITYCURE)
            {
                IsPlural = true,
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN.FLAVOR
            };
            this[IDs.MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN] = new ItemMedicineModel(DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.NAME, DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.PLURAL, GameImages.ITEM_BEER_BOTTLE_GREEN,
                DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.HEALING, DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.STAMINABOOST, DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.SLEEPBOOST, DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.INFECTIONCURE, DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.SANITYCURE)
            {
                IsPlural = true,
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.FLAVOR
            };
            this[IDs.MEDICINE_ALCOHOL_BEER_CAN_BLUE] = new ItemMedicineModel(DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.NAME, DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.PLURAL, GameImages.ITEM_BEER_CAN_BLUE,
                DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.HEALING, DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.STAMINABOOST, DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.SLEEPBOOST, DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.INFECTIONCURE, DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.SANITYCURE)
            {
                IsPlural = true,
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.FLAVOR
            };
            this[IDs.MEDICINE_ALCOHOL_BEER_CAN_RED] = new ItemMedicineModel(DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.NAME, DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.PLURAL, GameImages.ITEM_BEER_CAN_RED,
                DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.HEALING, DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.STAMINABOOST, DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.SLEEPBOOST, DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.INFECTIONCURE, DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.SANITYCURE)
            {
                IsPlural = true,
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.FLAVOR
            };
            this[IDs.MEDICINE_ALCOHOL_LIQUOR_AMBER] = new ItemMedicineModel(DATA_MEDICINE_ALCOHOL_LIQUOR_AMBER.NAME, DATA_MEDICINE_ALCOHOL_LIQUOR_AMBER.PLURAL, GameImages.ITEM_LIQUOR_BOTTLE_AMBER,
                DATA_MEDICINE_ALCOHOL_LIQUOR_AMBER.HEALING, DATA_MEDICINE_ALCOHOL_LIQUOR_AMBER.STAMINABOOST, DATA_MEDICINE_ALCOHOL_LIQUOR_AMBER.SLEEPBOOST, DATA_MEDICINE_ALCOHOL_LIQUOR_AMBER.INFECTIONCURE, DATA_MEDICINE_ALCOHOL_LIQUOR_AMBER.SANITYCURE)
            {
                IsPlural = true,
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_ALCOHOL_LIQUOR_AMBER.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_ALCOHOL_LIQUOR_AMBER.FLAVOR
            };
            this[IDs.MEDICINE_ALCOHOL_LIQUOR_CLEAR] = new ItemMedicineModel(DATA_MEDICINE_ALCOHOL_LIQUOR_CLEAR.NAME, DATA_MEDICINE_ALCOHOL_LIQUOR_CLEAR.PLURAL, GameImages.ITEM_LIQUOR_BOTTLE_CLEAR,
                DATA_MEDICINE_ALCOHOL_LIQUOR_CLEAR.HEALING, DATA_MEDICINE_ALCOHOL_LIQUOR_CLEAR.STAMINABOOST, DATA_MEDICINE_ALCOHOL_LIQUOR_CLEAR.SLEEPBOOST, DATA_MEDICINE_ALCOHOL_LIQUOR_CLEAR.INFECTIONCURE, DATA_MEDICINE_ALCOHOL_LIQUOR_CLEAR.SANITYCURE)
            {
                IsPlural = true,
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_ALCOHOL_LIQUOR_CLEAR.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_ALCOHOL_LIQUOR_CLEAR.FLAVOR
            };
            this[IDs.MEDICINE_CIGARETTES] = new ItemMedicineModel(DATA_MEDICINE_CIGARETTES.NAME, DATA_MEDICINE_CIGARETTES.PLURAL, GameImages.ITEM_CIGARETTES,
                DATA_MEDICINE_CIGARETTES.HEALING, DATA_MEDICINE_CIGARETTES.STAMINABOOST, DATA_MEDICINE_CIGARETTES.SLEEPBOOST, DATA_MEDICINE_CIGARETTES.INFECTIONCURE, DATA_MEDICINE_CIGARETTES.SANITYCURE)
            {
                IsPlural = true,
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_CIGARETTES.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_CIGARETTES.FLAVOR
            };
            #endregion

            #region Food
            this[IDs.FOOD_ARMY_RATION] = new ItemFoodModel(DATA_FOOD_ARMY_RATION.NAME, DATA_FOOD_ARMY_RATION.PLURAL, GameImages.ITEM_ARMY_RATION, DATA_FOOD_ARMY_RATION.NUTRITION, DATA_FOOD_ARMY_RATION.BESTBEFORE)
            {
                IsAn = StartsWithVowel(DATA_FOOD_ARMY_RATION.NAME),
                IsPlural = CheckPlural(DATA_FOOD_ARMY_RATION.NAME, DATA_FOOD_ARMY_RATION.PLURAL),
                StackingLimit = DATA_FOOD_ARMY_RATION.STACKINGLIMIT,
                FlavorDescription = DATA_FOOD_ARMY_RATION.FLAVOR
            };

            this[IDs.FOOD_GROCERIES] = new ItemFoodModel(DATA_FOOD_GROCERIES.NAME, DATA_FOOD_GROCERIES.PLURAL, GameImages.ITEM_GROCERIES, DATA_FOOD_GROCERIES.NUTRITION, DATA_FOOD_GROCERIES.BESTBEFORE)
            {
                IsAn = StartsWithVowel(DATA_FOOD_GROCERIES.NAME),
                IsPlural = CheckPlural(DATA_FOOD_GROCERIES.NAME, DATA_FOOD_GROCERIES.PLURAL),
                StackingLimit = DATA_FOOD_GROCERIES.STACKINGLIMIT,
                FlavorDescription = DATA_FOOD_GROCERIES.FLAVOR
            };

            this[IDs.FOOD_CANNED_FOOD] = new ItemFoodModel(DATA_FOOD_CANNED_FOOD.NAME, DATA_FOOD_CANNED_FOOD.PLURAL, GameImages.ITEM_CANNED_FOOD, DATA_FOOD_CANNED_FOOD.NUTRITION, DATA_FOOD_CANNED_FOOD.BESTBEFORE)
            {
                IsAn = StartsWithVowel(DATA_FOOD_CANNED_FOOD.NAME),
                IsPlural = CheckPlural(DATA_FOOD_CANNED_FOOD.NAME, DATA_FOOD_CANNED_FOOD.PLURAL),
                StackingLimit = DATA_FOOD_CANNED_FOOD.STACKINGLIMIT,
                IsStackable = true,
                FlavorDescription = DATA_FOOD_CANNED_FOOD.FLAVOR
            };

            this[IDs.FOOD_WILD_BERRIES] = new ItemFoodModel(DATA_FOOD_WILD_BERRIES.NAME, DATA_FOOD_WILD_BERRIES.PLURAL, GameImages.ITEM_WILD_BERRIES, DATA_FOOD_WILD_BERRIES.NUTRITION, DATA_FOOD_WILD_BERRIES.BESTBEFORE)
            {  //@MP (Release 4)
                IsAn = StartsWithVowel(DATA_FOOD_WILD_BERRIES.NAME),
                IsPlural = CheckPlural(DATA_FOOD_WILD_BERRIES.NAME, DATA_FOOD_WILD_BERRIES.PLURAL),
                StackingLimit = DATA_FOOD_WILD_BERRIES.STACKINGLIMIT,
                IsStackable = true,
                FlavorDescription = DATA_FOOD_WILD_BERRIES.FLAVOR
            };
            #endregion

            #region Melee weapons
            this[IDs.MELEE_BASEBALLBAT] = new ItemMeleeWeaponModel(DATA_MELEE_BASEBALLBAT.NAME, DATA_MELEE_BASEBALLBAT.PLURAL, GameImages.ITEM_BASEBALL_BAT,
                new Attack(AttackKind.PHYSICAL, new Verb("smash", "smashes"), DATA_MELEE_BASEBALLBAT.ATK, DATA_MELEE_BASEBALLBAT.DMG, DATA_MELEE_BASEBALLBAT.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (DATA_MELEE_BASEBALLBAT.STACKINGLIMIT > 1),
                StackingLimit = DATA_MELEE_BASEBALLBAT.STACKINGLIMIT,
                FlavorDescription = DATA_MELEE_BASEBALLBAT.FLAVOR,
                IsFragile = DATA_MELEE_BASEBALLBAT.ISFRAGILE
            };

            this[IDs.MELEE_COMBAT_KNIFE] = new ItemMeleeWeaponModel(DATA_MELEE_COMBAT_KNIFE.NAME, DATA_MELEE_COMBAT_KNIFE.PLURAL, GameImages.ITEM_COMBAT_KNIFE,
                new Attack(AttackKind.PHYSICAL, new Verb("stab", "stabs"), DATA_MELEE_COMBAT_KNIFE.ATK, DATA_MELEE_COMBAT_KNIFE.DMG, DATA_MELEE_COMBAT_KNIFE.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = true,
                StackingLimit = DATA_MELEE_COMBAT_KNIFE.STACKINGLIMIT,
                FlavorDescription = DATA_MELEE_COMBAT_KNIFE.FLAVOR,
                IsFragile = DATA_MELEE_COMBAT_KNIFE.ISFRAGILE
            };

            this[IDs.MELEE_CROWBAR] = new ItemMeleeWeaponModel(DATA_MELEE_CROWBAR.NAME, DATA_MELEE_CROWBAR.PLURAL, GameImages.ITEM_CROWBAR,
                new Attack(AttackKind.PHYSICAL, new Verb("strike"), DATA_MELEE_CROWBAR.ATK, DATA_MELEE_CROWBAR.DMG, DATA_MELEE_CROWBAR.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = true,
                StackingLimit = DATA_MELEE_CROWBAR.STACKINGLIMIT,
                FlavorDescription = DATA_MELEE_CROWBAR.FLAVOR,
                IsFragile = DATA_MELEE_CROWBAR.ISFRAGILE
            };

            this[IDs.UNIQUE_JASON_MYERS_AXE] = new ItemMeleeWeaponModel(DATA_MELEE_UNIQUE_JASON_MYERS_AXE.NAME, DATA_MELEE_UNIQUE_JASON_MYERS_AXE.PLURAL, GameImages.ITEM_JASON_MYERS_AXE,
                new Attack(AttackKind.PHYSICAL, new Verb("slash", "slashes"), DATA_MELEE_UNIQUE_JASON_MYERS_AXE.ATK, DATA_MELEE_UNIQUE_JASON_MYERS_AXE.DMG, DATA_MELEE_UNIQUE_JASON_MYERS_AXE.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsProper = true,
                FlavorDescription = DATA_MELEE_UNIQUE_JASON_MYERS_AXE.FLAVOR,
                IsUnbreakable = true
            };

            this[IDs.MELEE_GOLFCLUB] = new ItemMeleeWeaponModel(DATA_MELEE_GOLFCLUB.NAME, DATA_MELEE_GOLFCLUB.PLURAL, GameImages.ITEM_GOLF_CLUB,
                new Attack(AttackKind.PHYSICAL, new Verb("strike"), DATA_MELEE_GOLFCLUB.ATK, DATA_MELEE_GOLFCLUB.DMG, DATA_MELEE_GOLFCLUB.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (DATA_MELEE_GOLFCLUB.STACKINGLIMIT > 1),
                StackingLimit = DATA_MELEE_GOLFCLUB.STACKINGLIMIT,
                FlavorDescription = DATA_MELEE_GOLFCLUB.FLAVOR,
                IsFragile = DATA_MELEE_GOLFCLUB.ISFRAGILE
            };

            this[IDs.MELEE_IRON_GOLFCLUB] = new ItemMeleeWeaponModel(DATA_MELEE_IRON_GOLFCLUB.NAME, DATA_MELEE_IRON_GOLFCLUB.PLURAL, GameImages.ITEM_IRON_GOLF_CLUB,
                new Attack(AttackKind.PHYSICAL, new Verb("strike"), DATA_MELEE_IRON_GOLFCLUB.ATK, DATA_MELEE_IRON_GOLFCLUB.DMG, DATA_MELEE_IRON_GOLFCLUB.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (DATA_MELEE_IRON_GOLFCLUB.STACKINGLIMIT > 1),
                StackingLimit = DATA_MELEE_IRON_GOLFCLUB.STACKINGLIMIT,
                FlavorDescription = DATA_MELEE_IRON_GOLFCLUB.FLAVOR,
                IsFragile = DATA_MELEE_IRON_GOLFCLUB.ISFRAGILE
            };

            this[IDs.MELEE_HUGE_HAMMER] = new ItemMeleeWeaponModel(DATA_MELEE_HUGE_HAMMER.NAME, DATA_MELEE_HUGE_HAMMER.PLURAL, GameImages.ITEM_HUGE_HAMMER,
                new Attack(AttackKind.PHYSICAL, new Verb("smash", "smashes"), DATA_MELEE_HUGE_HAMMER.ATK, DATA_MELEE_HUGE_HAMMER.DMG, DATA_MELEE_HUGE_HAMMER.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (DATA_MELEE_HUGE_HAMMER.STACKINGLIMIT > 1),
                StackingLimit = DATA_MELEE_HUGE_HAMMER.STACKINGLIMIT,
                FlavorDescription = DATA_MELEE_HUGE_HAMMER.FLAVOR,
                IsFragile = DATA_MELEE_HUGE_HAMMER.ISFRAGILE
            };

            this[IDs.MELEE_SHOVEL] = new ItemMeleeWeaponModel(DATA_MELEE_SHOVEL.NAME, DATA_MELEE_SHOVEL.PLURAL, GameImages.ITEM_SHOVEL,
                new Attack(AttackKind.PHYSICAL, new Verb("strike"), DATA_MELEE_SHOVEL.ATK, DATA_MELEE_SHOVEL.DMG, DATA_MELEE_SHOVEL.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (DATA_MELEE_SHOVEL.STACKINGLIMIT > 1),
                StackingLimit = DATA_MELEE_SHOVEL.STACKINGLIMIT,
                FlavorDescription = DATA_MELEE_SHOVEL.FLAVOR,
                IsFragile = DATA_MELEE_SHOVEL.ISFRAGILE
            };

            this[IDs.MELEE_SHORT_SHOVEL] = new ItemMeleeWeaponModel(DATA_MELEE_SHORT_SHOVEL.NAME, DATA_MELEE_SHORT_SHOVEL.PLURAL, GameImages.ITEM_SHORT_SHOVEL,
                new Attack(AttackKind.PHYSICAL, new Verb("strike"), DATA_MELEE_SHORT_SHOVEL.ATK, DATA_MELEE_SHORT_SHOVEL.DMG, DATA_MELEE_SHORT_SHOVEL.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (DATA_MELEE_SHORT_SHOVEL.STACKINGLIMIT > 1),
                StackingLimit = DATA_MELEE_SHORT_SHOVEL.STACKINGLIMIT,
                FlavorDescription = DATA_MELEE_SHORT_SHOVEL.FLAVOR,
                IsFragile = DATA_MELEE_SHORT_SHOVEL.ISFRAGILE
            };

            this[IDs.MELEE_TRUNCHEON] = new ItemMeleeWeaponModel(DATA_MELEE_TRUNCHEON.NAME, DATA_MELEE_TRUNCHEON.PLURAL, GameImages.ITEM_TRUNCHEON,
                new Attack(AttackKind.PHYSICAL, new Verb("strike"), DATA_MELEE_TRUNCHEON.ATK, DATA_MELEE_TRUNCHEON.DMG, DATA_MELEE_TRUNCHEON.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = true,
                StackingLimit = DATA_MELEE_TRUNCHEON.STACKINGLIMIT,
                FlavorDescription = DATA_MELEE_TRUNCHEON.FLAVOR,
                IsFragile = DATA_MELEE_TRUNCHEON.ISFRAGILE
            };

            MeleeWeaponData mwData;
            mwData = DATA_MELEE_IMPROVISED_CLUB;
            this[IDs.MELEE_IMPROVISED_CLUB] = new ItemMeleeWeaponModel(mwData.NAME, mwData.PLURAL, GameImages.ITEM_IMPROVISED_CLUB,
                new Attack(AttackKind.PHYSICAL, new Verb("strike"), mwData.ATK, mwData.DMG, mwData.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwData.STACKINGLIMIT > 1),
                StackingLimit = mwData.STACKINGLIMIT,
                FlavorDescription = mwData.FLAVOR,
                IsFragile = mwData.ISFRAGILE
            };

            mwData = DATA_MELEE_IMPROVISED_SPEAR;
            this[IDs.MELEE_IMPROVISED_SPEAR] = new ItemMeleeWeaponModel(mwData.NAME, mwData.PLURAL, GameImages.ITEM_IMPROVISED_SPEAR,
                new Attack(AttackKind.PHYSICAL, new Verb("pierce"), mwData.ATK, mwData.DMG, mwData.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwData.STACKINGLIMIT > 1),
                StackingLimit = mwData.STACKINGLIMIT,
                FlavorDescription = mwData.FLAVOR,
                IsFragile = mwData.ISFRAGILE
            };

            mwData = DATA_MELEE_SMALL_HAMMER;
            this[IDs.MELEE_SMALL_HAMMER] = new ItemMeleeWeaponModel(mwData.NAME, mwData.PLURAL, GameImages.ITEM_SMALL_HAMMER,
                new Attack(AttackKind.PHYSICAL, new Verb("smash"), mwData.ATK, mwData.DMG, mwData.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwData.STACKINGLIMIT > 1),
                StackingLimit = mwData.STACKINGLIMIT,
                FlavorDescription = mwData.FLAVOR,
                IsFragile = mwData.ISFRAGILE
            };

            mwData = DATA_MELEE_UNIQUE_FAMU_FATARU_KATANA;
            this[IDs.UNIQUE_FAMU_FATARU_KATANA] = new ItemMeleeWeaponModel(mwData.NAME, mwData.PLURAL, GameImages.ITEM_FAMU_FATARU_KATANA,
                new Attack(AttackKind.PHYSICAL, new Verb("slash", "slashes"), mwData.ATK, mwData.DMG, mwData.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwData.FLAVOR,
                IsProper = true,
                IsUnbreakable = true
            };

            mwData = DATA_MELEE_UNIQUE_BIGBEAR_BAT;
            this[IDs.UNIQUE_BIGBEAR_BAT] = new ItemMeleeWeaponModel(mwData.NAME, mwData.PLURAL, GameImages.ITEM_BIGBEAR_BAT,
                new Attack(AttackKind.PHYSICAL, new Verb("smash", "smashes"), mwData.ATK, mwData.DMG, mwData.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwData.FLAVOR,
                IsProper = true,
                IsUnbreakable = true
            };

            mwData = DATA_MELEE_UNIQUE_ROGUEDJACK_KEYBOARD;
            this[IDs.UNIQUE_ROGUEDJACK_KEYBOARD] = new ItemMeleeWeaponModel(mwData.NAME, mwData.PLURAL, GameImages.ITEM_ROGUEDJACK_KEYBOARD,
                new Attack(AttackKind.PHYSICAL, new Verb("bash", "bashes"), mwData.ATK, mwData.DMG, mwData.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwData.FLAVOR,
                IsProper = true,
                IsUnbreakable = true
            };

            //@@MP (Release 3)
            mwData = DATA_MELEE_TENNIS_RACKET;
            this[IDs.MELEE_TENNIS_RACKET] = new ItemMeleeWeaponModel(mwData.NAME, mwData.PLURAL, GameImages.ITEM_TENNIS_RACKET,
                new Attack(AttackKind.PHYSICAL, new Verb("bash", "bashes"), mwData.ATK, mwData.DMG, mwData.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwData.FLAVOR,
                IsStackable = (mwData.STACKINGLIMIT > 1),
                StackingLimit = mwData.STACKINGLIMIT,
                IsFragile = mwData.ISFRAGILE
            };

            mwData = DATA_MELEE_HOCKEY_STICK;
            this[IDs.MELEE_HOCKEY_STICK] = new ItemMeleeWeaponModel(mwData.NAME, mwData.PLURAL, GameImages.ITEM_HOCKEY_STICK,
                new Attack(AttackKind.PHYSICAL, new Verb("bash", "bashes"), mwData.ATK, mwData.DMG, mwData.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwData.FLAVOR,
                IsStackable = (mwData.STACKINGLIMIT > 1),
                StackingLimit = mwData.STACKINGLIMIT,
                IsFragile = mwData.ISFRAGILE
            };

            mwData = DATA_MELEE_MACHETE;
            this[IDs.MELEE_MACHETE] = new ItemMeleeWeaponModel(mwData.NAME, mwData.PLURAL, GameImages.ITEM_MACHETE,
                new Attack(AttackKind.PHYSICAL, new Verb("slash", "slashes"), mwData.ATK, mwData.DMG, mwData.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwData.FLAVOR,
                IsStackable = (mwData.STACKINGLIMIT > 1),
                StackingLimit = mwData.STACKINGLIMIT,
                IsFragile = mwData.ISFRAGILE
            };

            mwData = DATA_MELEE_STANDARD_AXE;
            this[IDs.MELEE_STANDARD_AXE] = new ItemMeleeWeaponModel(mwData.NAME, mwData.PLURAL, GameImages.ITEM_STANDARD_AXE,
                new Attack(AttackKind.PHYSICAL, new Verb("strike"), mwData.ATK, mwData.DMG, mwData.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwData.FLAVOR,
                IsStackable = (mwData.STACKINGLIMIT > 1),
                StackingLimit = mwData.STACKINGLIMIT,
                IsFragile = mwData.ISFRAGILE
            };

            mwData = DATA_MELEE_PICKAXE;
            this[IDs.MELEE_PICKAXE] = new ItemMeleeWeaponModel(mwData.NAME, mwData.PLURAL, GameImages.ITEM_PICKAXE,
                new Attack(AttackKind.PHYSICAL, new Verb("strike"), mwData.ATK, mwData.DMG, mwData.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwData.FLAVOR,
                IsStackable = (mwData.STACKINGLIMIT > 1),
                StackingLimit = mwData.STACKINGLIMIT,
                IsFragile = mwData.ISFRAGILE
            };

            mwData = DATA_MELEE_PIPE_WRENCH;
            this[IDs.MELEE_PIPE_WRENCH] = new ItemMeleeWeaponModel(mwData.NAME, mwData.PLURAL, GameImages.ITEM_PIPE_WRENCH,
                new Attack(AttackKind.PHYSICAL, new Verb("slash", "slashes"), mwData.ATK, mwData.DMG, mwData.STA))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwData.FLAVOR,
                IsStackable = (mwData.STACKINGLIMIT > 1),
                StackingLimit = mwData.STACKINGLIMIT,
                IsFragile = mwData.ISFRAGILE
            };
            #endregion

            #region Ranged weapons
            RangedWeaponData rwp;

            rwp = DATA_RANGED_ARMY_PISTOL;
            this[IDs.RANGED_ARMY_PISTOL] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_ARMY_PISTOL,
                new Attack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.DMG, 0, rwp.RANGE), 
                    rwp.MAXAMMO, AmmoType.HEAVY_PISTOL)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
                IsAn = true
            };

            rwp = DATA_RANGED_ARMY_RIFLE;
            this[IDs.RANGED_ARMY_RIFLE] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_ARMY_RIFLE,
                new Attack(AttackKind.FIREARM, new Verb("fire a salvo at", "fires a salvo at"), rwp.ATK, rwp.DMG, 0, rwp.RANGE), 
                     rwp.MAXAMMO, AmmoType.HEAVY_RIFLE) 
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
                IsAn = true
            };

            rwp = DATA_RANGED_HUNTING_CROSSBOW;
            this[IDs.RANGED_HUNTING_CROSSBOW] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_HUNTING_CROSSBOW,
                new Attack(AttackKind.BOW, new Verb("shoot"), rwp.ATK, rwp.DMG, 0, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.BOLT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR
            };

            rwp = DATA_RANGED_HUNTING_RIFLE;
            this[IDs.RANGED_HUNTING_RIFLE] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_HUNTING_RIFLE,
                new Attack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.DMG, 0, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.LIGHT_RIFLE)
                {
                    EquipmentPart = DollPart.RIGHT_HAND,
                    FlavorDescription = rwp.FLAVOR
                };

            rwp = DATA_RANGED_PISTOL;
            this[IDs.RANGED_PISTOL] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_PISTOL,
                new Attack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.DMG, 0, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.LIGHT_PISTOL)
                {
                    EquipmentPart = DollPart.RIGHT_HAND,
                    FlavorDescription =rwp.FLAVOR
                };

            rwp = DATA_RANGED_KOLT_REVOLVER;
            this[IDs.RANGED_KOLT_REVOLVER] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_KOLT_REVOLVER,
                new Attack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.DMG, 0, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.LIGHT_PISTOL)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR
            };

            rwp = DATA_RANGED_PRECISION_RIFLE;
            this[IDs.RANGED_PRECISION_RIFLE] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_PRECISION_RIFLE,
                new Attack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.DMG, 0, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.HEAVY_RIFLE)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR
            };

            rwp = DATA_RANGED_SHOTGUN;
            this[IDs.RANGED_SHOTGUN] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_SHOTGUN,
                new Attack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.DMG, 0, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.SHOTGUN)
                {
                    EquipmentPart = DollPart.RIGHT_HAND,
                    FlavorDescription = rwp.FLAVOR
                };

            rwp = DATA_UNIQUE_SANTAMAN_SHOTGUN;
            this[IDs.UNIQUE_SANTAMAN_SHOTGUN] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_SANTAMAN_SHOTGUN,
                new Attack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.DMG, 0, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.SHOTGUN)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
                IsProper = true,
                IsUnbreakable = true
            };

            rwp = DATA_UNIQUE_HANS_VON_HANZ_PISTOL;
            this[IDs.UNIQUE_HANS_VON_HANZ_PISTOL] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_HANS_VON_HANZ_PISTOL,
                new Attack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.DMG, 0, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.LIGHT_PISTOL)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
                IsProper = true,
                IsUnbreakable = true
            };
            #endregion

            #region Ammos
            this[IDs.AMMO_LIGHT_PISTOL] = new ItemAmmoModel("light pistol bullets", "light pistol bullets", GameImages.ITEM_AMMO_LIGHT_PISTOL,
                AmmoType.LIGHT_PISTOL, 20)
                {
                    IsPlural = true,
                    FlavorDescription = ""
                };

            this[IDs.AMMO_HEAVY_PISTOL] = new ItemAmmoModel("heavy pistol bullets", "heavy pistol bullets", GameImages.ITEM_AMMO_HEAVY_PISTOL,
                AmmoType.HEAVY_PISTOL, 12)
            {
                IsPlural = true,
                FlavorDescription = ""
            };

            this[IDs.AMMO_LIGHT_RIFLE] = new ItemAmmoModel("light rifle bullets", "light rifle bullets", GameImages.ITEM_AMMO_LIGHT_RIFLE,
                AmmoType.LIGHT_RIFLE, 14)
            {
                IsPlural = true,
                FlavorDescription = ""
            };

            this[IDs.AMMO_HEAVY_RIFLE] = new ItemAmmoModel("heavy rifle bullets", "heavy rifle bullets", GameImages.ITEM_AMMO_HEAVY_RIFLE,
                AmmoType.HEAVY_RIFLE, 20)
            {
                IsPlural = true,
                FlavorDescription = ""
            };

            this[IDs.AMMO_SHOTGUN] = new ItemAmmoModel("shotgun shells", "shotgun shells", GameImages.ITEM_AMMO_SHOTGUN,
                AmmoType.SHOTGUN, 10)
            {
                IsPlural = true,
                FlavorDescription = ""
            };

            this[IDs.AMMO_BOLTS] = new ItemAmmoModel("crossbow bolts", "crossbow bolts", GameImages.ITEM_AMMO_BOLTS,
                AmmoType.BOLT, 30)
            {
                IsPlural = true,
                FlavorDescription = ""
            };
            #endregion

            #region Explosives
            ExplosiveData exData;
            int[] exArray;

            //GRENADE
            exData = DATA_EXPLOSIVE_GRENADE;
            exArray = new int[exData.RADIUS + 1];
            for (int i = 0; i < exData.RADIUS + 1; i++)
                exArray[i] = exData.DMG[i];
            this[IDs.EXPLOSIVE_GRENADE] = new ItemGrenadeModel(exData.NAME, exData.PLURAL, GameImages.ITEM_GRENADE,
                exData.FUSE, new BlastAttack(exData.RADIUS, exArray, true, false), GameImages.ICON_BLAST, exData.MAXTHROW)
                {
                    EquipmentPart = DollPart.RIGHT_HAND,
                    IsStackable = true,
                    StackingLimit =exData.STACKLINGLIMIT,
                    FlavorDescription = exData.FLAVOR
                };

            this[IDs.EXPLOSIVE_GRENADE_PRIMED] = new ItemGrenadePrimedModel("primed " +exData.NAME, "primed "+exData.PLURAL, GameImages.ITEM_GRENADE_PRIMED, this[IDs.EXPLOSIVE_GRENADE] as ItemGrenadeModel)
            {
                EquipmentPart = DollPart.RIGHT_HAND
            };

            //@@MP - MOLOTOV (Release 4)
            exData = DATA_EXPLOSIVE_MOLOTOV;
            exArray = new int[exData.RADIUS + 1];
            for (int i = 0; i < exData.RADIUS + 1; i++)
                exArray[i] = exData.DMG[i];
            this[IDs.EXPLOSIVE_MOLOTOV] = new ItemGrenadeModel(exData.NAME, exData.PLURAL, GameImages.ITEM_MOLOTOV,
                exData.FUSE, new BlastAttack(exData.RADIUS, exArray, false, false), GameImages.ICON_BLAST, exData.MAXTHROW)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = true,
                StackingLimit = exData.STACKLINGLIMIT,
                FlavorDescription = exData.FLAVOR
            };

            this[IDs.EXPLOSIVE_MOLOTOV_PRIMED] = new ItemGrenadePrimedModel("primed " + exData.NAME, "primed " + exData.PLURAL, GameImages.ITEM_MOLOTOV_PRIMED, this[IDs.EXPLOSIVE_MOLOTOV] as ItemGrenadeModel)
            {
                EquipmentPart = DollPart.RIGHT_HAND
            };

            //@@MP - DYNAMITE (Release 4)
            exData = DATA_EXPLOSIVE_DYNAMITE;
            exArray = new int[exData.RADIUS + 1];
            for (int i = 0; i < exData.RADIUS + 1; i++)
                exArray[i] = exData.DMG[i];
            this[IDs.EXPLOSIVE_DYNAMITE] = new ItemGrenadeModel(exData.NAME, exData.PLURAL, GameImages.ITEM_DYNAMITE,
                exData.FUSE, new BlastAttack(exData.RADIUS, exArray, true, true), GameImages.ICON_BLAST, exData.MAXTHROW)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = true,
                StackingLimit = exData.STACKLINGLIMIT,
                FlavorDescription = exData.FLAVOR
            };

            this[IDs.EXPLOSIVE_DYNAMITE_PRIMED] = new ItemGrenadePrimedModel("primed " + exData.NAME, "primed " + exData.PLURAL, GameImages.ITEM_DYNAMITE_PRIMED, this[IDs.EXPLOSIVE_DYNAMITE] as ItemGrenadeModel)
            {
                EquipmentPart = DollPart.RIGHT_HAND
            };
            #endregion

            #region Barricade material
            BarricadingMaterialData barData;

            barData = DATA_BAR_WOODEN_PLANK;
            this[IDs.BAR_WOODEN_PLANK] = new ItemBarricadeMaterialModel(barData.NAME, barData.PLURAL, GameImages.ITEM_WOODEN_PLANK, barData.VALUE)
            {
                IsStackable = (barData.STACKINGLIMIT > 1),
                StackingLimit = barData.STACKINGLIMIT,
                FlavorDescription = barData.FLAVOR
            };
            #endregion

            #region Bodyarmors
            ArmorData armData;

            armData = DATA_ARMOR_ARMY;
            this[IDs.ARMOR_ARMY_BODYARMOR] = new ItemBodyArmorModel(armData.NAME, armData.PLURAL, GameImages.ITEM_ARMY_BODYARMOR, armData.PRO_HIT, armData.PRO_SHOT, armData.ENC, armData.WEIGHT)
            {
                EquipmentPart = DollPart.TORSO,
                FlavorDescription = armData.FLAVOR,
                IsAn = StartsWithVowel(armData.NAME)
            };

            armData = DATA_ARMOR_CHAR;
            this[IDs.ARMOR_CHAR_LIGHT_BODYARMOR] = new ItemBodyArmorModel(armData.NAME, armData.PLURAL, GameImages.ITEM_CHAR_LIGHT_BODYARMOR, armData.PRO_HIT, armData.PRO_SHOT, armData.ENC, armData.WEIGHT)
            {
                EquipmentPart = DollPart.TORSO,
                FlavorDescription = armData.FLAVOR,
                IsAn = StartsWithVowel(armData.NAME)
            };

            armData = DATA_ARMOR_HELLS_SOULS_JACKET;
            this[IDs.ARMOR_HELLS_SOULS_JACKET] = new ItemBodyArmorModel(armData.NAME, armData.PLURAL, GameImages.ITEM_HELLS_SOULS_JACKET, armData.PRO_HIT, armData.PRO_SHOT, armData.ENC, armData.WEIGHT)
            {
                EquipmentPart = DollPart.TORSO,
                FlavorDescription = armData.FLAVOR,
                IsAn = StartsWithVowel(armData.NAME)
            };

            armData = DATA_ARMOR_FREE_ANGELS_JACKET;
            this[IDs.ARMOR_FREE_ANGELS_JACKET] = new ItemBodyArmorModel(armData.NAME, armData.PLURAL, GameImages.ITEM_FREE_ANGELS_JACKET, armData.PRO_HIT, armData.PRO_SHOT, armData.ENC, armData.WEIGHT)
            {
                EquipmentPart = DollPart.TORSO,
                FlavorDescription = armData.FLAVOR,
                IsAn = StartsWithVowel(armData.NAME)
            };

            armData = DATA_ARMOR_POLICE_JACKET;
            this[IDs.ARMOR_POLICE_JACKET] = new ItemBodyArmorModel(armData.NAME, armData.PLURAL, GameImages.ITEM_POLICE_JACKET, armData.PRO_HIT, armData.PRO_SHOT, armData.ENC, armData.WEIGHT)
            {
                EquipmentPart = DollPart.TORSO,
                FlavorDescription = armData.FLAVOR,
                IsAn = StartsWithVowel(armData.NAME)
            };

            armData = DATA_ARMOR_POLICE_RIOT;
            this[IDs.ARMOR_POLICE_RIOT] = new ItemBodyArmorModel(armData.NAME, armData.PLURAL, GameImages.ITEM_POLICE_RIOT_ARMOR, armData.PRO_HIT, armData.PRO_SHOT, armData.ENC, armData.WEIGHT)
            {
                EquipmentPart = DollPart.TORSO,
                FlavorDescription = armData.FLAVOR,
                IsAn = StartsWithVowel(armData.NAME)
            };

            armData = DATA_ARMOR_HUNTER_VEST;
            this[IDs.ARMOR_HUNTER_VEST] = new ItemBodyArmorModel(armData.NAME, armData.PLURAL, GameImages.ITEM_HUNTER_VEST, armData.PRO_HIT, armData.PRO_SHOT, armData.ENC, armData.WEIGHT)
            {
                EquipmentPart = DollPart.TORSO,
                FlavorDescription = armData.FLAVOR,
                IsAn = StartsWithVowel(armData.NAME)
            };


            #endregion

            #region Trackers
            TrackerData traData;

            traData = DATA_TRACKER_CELL_PHONE;
            this[IDs.TRACKER_CELL_PHONE] = new ItemTrackerModel(traData.NAME, traData.PLURAL, GameImages.ITEM_CELL_PHONE,
                ItemTrackerModel.TrackingFlags.FOLLOWER_AND_LEADER,
                traData.BATTERIES * WorldTime.TURNS_PER_HOUR)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = traData.FLAVOR
            };

            traData = DATA_TRACKER_ZTRACKER;
            this[IDs.TRACKER_ZTRACKER] = new ItemTrackerModel(traData.NAME, traData.PLURAL, GameImages.ITEM_ZTRACKER,
                ItemTrackerModel.TrackingFlags.UNDEADS,
                traData.BATTERIES * WorldTime.TURNS_PER_HOUR)
                {
                    EquipmentPart = DollPart.LEFT_HAND,
                    FlavorDescription = traData.FLAVOR
                };

            traData = DATA_TRACKER_BLACKOPS_GPS;
            this[IDs.TRACKER_BLACKOPS] = new ItemTrackerModel(traData.NAME, traData.PLURAL, GameImages.ITEM_BLACKOPS_GPS,
                ItemTrackerModel.TrackingFlags.BLACKOPS_FACTION,
                traData.BATTERIES * WorldTime.TURNS_PER_HOUR)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = traData.FLAVOR
            };

            traData = DATA_TRACKER_POLICE_RADIO;
            this[IDs.TRACKER_POLICE_RADIO] = new ItemTrackerModel(traData.NAME, traData.PLURAL, GameImages.ITEM_POLICE_RADIO,
                ItemTrackerModel.TrackingFlags.POLICE_FACTION,
                traData.BATTERIES * WorldTime.TURNS_PER_HOUR)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = traData.FLAVOR
            };
            #endregion

            #region Spray Paint
            SprayPaintData spData;

            spData = DATA_SPRAY_PAINT1;
            this[IDs.SPRAY_PAINT1] = new ItemSprayPaintModel(spData.NAME, spData.PLURAL, GameImages.ITEM_SPRAYPAINT, spData.QUANTITY, GameImages.DECO_PLAYER_TAG1)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = spData.FLAVOR
            };

            spData = DATA_SPRAY_PAINT2;
            this[IDs.SPRAY_PAINT2] = new ItemSprayPaintModel(spData.NAME, spData.PLURAL, GameImages.ITEM_SPRAYPAINT2, spData.QUANTITY, GameImages.DECO_PLAYER_TAG2)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = spData.FLAVOR
            };

            spData = DATA_SPRAY_PAINT3;
            this[IDs.SPRAY_PAINT3] = new ItemSprayPaintModel(spData.NAME, spData.PLURAL, GameImages.ITEM_SPRAYPAINT3, spData.QUANTITY, GameImages.DECO_PLAYER_TAG3)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = spData.FLAVOR
            };

            spData = DATA_SPRAY_PAINT4;
            this[IDs.SPRAY_PAINT4] = new ItemSprayPaintModel(spData.NAME, spData.PLURAL, GameImages.ITEM_SPRAYPAINT4, spData.QUANTITY, GameImages.DECO_PLAYER_TAG4)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = spData.FLAVOR
            };
            #endregion

            #region Lights
            LightData ltData;

            ltData = DATA_LIGHT_FLASHLIGHT;
            this[IDs.LIGHT_FLASHLIGHT] = new ItemLightModel(ltData.NAME, ltData.PLURAL, GameImages.ITEM_FLASHLIGHT, ltData.FOV, ltData.BATTERIES * WorldTime.TURNS_PER_HOUR, GameImages.ITEM_FLASHLIGHT_OUT)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = ltData.FLAVOR
            };

            ltData = DATA_LIGHT_BIG_FLASHLIGHT;
            this[IDs.LIGHT_BIG_FLASHLIGHT] = new ItemLightModel(ltData.NAME, ltData.PLURAL, GameImages.ITEM_BIG_FLASHLIGHT, ltData.FOV, ltData.BATTERIES * WorldTime.TURNS_PER_HOUR, GameImages.ITEM_BIG_FLASHLIGHT_OUT)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = ltData.FLAVOR
            };

            #endregion

            #region Scent sprays
            ScentSprayData sspData;

            sspData = DATA_SCENT_SPRAY_STENCH_KILLER;
            this[IDs.SCENT_SPRAY_STENCH_KILLER] = new ItemSprayScentModel(sspData.NAME, sspData.PLURAL, GameImages.ITEM_STENCH_KILLER, 
                sspData.QUANTITY, Odor.PERFUME_LIVING_SUPRESSOR, sspData.STRENGTH * WorldTime.TURNS_PER_HOUR)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = sspData.FLAVOR
            };
            #endregion

            #region Traps
            TrapData trpData;

            trpData = DATA_TRAP_EMPTY_CAN;
            this[IDs.TRAP_EMPTY_CAN] = new ItemTrapModel(trpData.NAME, trpData.PLURAL, GameImages.ITEM_EMPTY_CAN,
                trpData.STACKING, trpData.CHANCE, trpData.DAMAGE,
                trpData.DROP_ACTIVATE, trpData.USE_ACTIVATE, trpData.IS_ONE_TIME, 
                trpData.BREAK_CHANCE, trpData.BLOCK_CHANCE, trpData.BREAK_CHANCE_ESCAPE,
                trpData.IS_NOISY, trpData.NOISE_NAME, trpData.IS_FLAMMABLE)
            {
                FlavorDescription = trpData.FLAVOR
            };

            trpData = DATA_TRAP_BEAR_TRAP;
            this[IDs.TRAP_BEAR_TRAP] = new ItemTrapModel(trpData.NAME, trpData.PLURAL, GameImages.ITEM_BEAR_TRAP,
                trpData.STACKING, trpData.CHANCE, trpData.DAMAGE,
                trpData.DROP_ACTIVATE, trpData.USE_ACTIVATE, trpData.IS_ONE_TIME,
                trpData.BREAK_CHANCE, trpData.BLOCK_CHANCE, trpData.BREAK_CHANCE_ESCAPE, 
                trpData.IS_NOISY, trpData.NOISE_NAME, trpData.IS_FLAMMABLE)
            {
                FlavorDescription = trpData.FLAVOR
            };

            trpData = DATA_TRAP_SPIKES;
            this[IDs.TRAP_SPIKES] = new ItemTrapModel(trpData.NAME, trpData.PLURAL, GameImages.ITEM_SPIKES,
                trpData.STACKING, trpData.CHANCE, trpData.DAMAGE,
                trpData.DROP_ACTIVATE, trpData.USE_ACTIVATE, trpData.IS_ONE_TIME, 
                trpData.BREAK_CHANCE, trpData.BLOCK_CHANCE, trpData.BREAK_CHANCE_ESCAPE, 
                trpData.IS_NOISY, trpData.NOISE_NAME, trpData.IS_FLAMMABLE)
            {
                FlavorDescription = trpData.FLAVOR
            };

            trpData = DATA_TRAP_BARBED_WIRE;
            this[IDs.TRAP_BARBED_WIRE] = new ItemTrapModel(trpData.NAME, trpData.PLURAL, GameImages.ITEM_BARBED_WIRE,
                trpData.STACKING, trpData.CHANCE, trpData.DAMAGE,
                trpData.DROP_ACTIVATE, trpData.USE_ACTIVATE, trpData.IS_ONE_TIME,
                trpData.BREAK_CHANCE, trpData.BLOCK_CHANCE, trpData.BREAK_CHANCE_ESCAPE,
                trpData.IS_NOISY, trpData.NOISE_NAME, trpData.IS_FLAMMABLE)
            {
                FlavorDescription = trpData.FLAVOR
            };

            #endregion

            #region Entertainment
            EntData entData;

            entData = DATA_ENT_BOOK;
            this[IDs.ENT_BOOK] = new ItemEntertainmentModel(entData.NAME, entData.PLURAL, GameImages.ITEM_BOOK, entData.VALUE, entData.BORECHANCE)
            {
                StackingLimit = entData.STACKING,
                FlavorDescription = entData.FLAVOR
            };

            entData = DATA_ENT_MAGAZINE;
            this[IDs.ENT_MAGAZINE] = new ItemEntertainmentModel(entData.NAME, entData.PLURAL, GameImages.ITEM_MAGAZINE, entData.VALUE, entData.BORECHANCE)
            {
                StackingLimit = entData.STACKING,
                FlavorDescription = entData.FLAVOR
            };
            #endregion

            #region Uniques
            this[IDs.UNIQUE_SUBWAY_BADGE] = new ItemModel("Subway Worker Badge", "Subways Worker Badges", GameImages.ITEM_SUBWAY_BADGE)
            {
                DontAutoEquip = true,
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = "You got yourself a new job!"
            };

            //@@MP (Release 3)
            this[IDs.UNIQUE_CHAR_DOCUMENT1] = new ItemModel("CHAR document", "CHAR documents", GameImages.ITEM_CHAR_DOCUMENT)
            {
                FlavorDescription = "Notes that suggest CHAR were trying mutation experiments on rats."
            };

            this[IDs.UNIQUE_CHAR_DOCUMENT2] = new ItemModel("CHAR document", "CHAR documents", GameImages.ITEM_CHAR_DOCUMENT)
            {
                FlavorDescription = @"""TEST #240 subjects showing violent tendencies yet decreased vital signs."""
            };

            this[IDs.UNIQUE_CHAR_DOCUMENT3] = new ItemModel("CHAR document", "CHAR documents", GameImages.ITEM_CHAR_DOCUMENT)
            {
                FlavorDescription = @"""Skin decay greatly accelerated in many but not all cases."""
            };

            this[IDs.UNIQUE_CHAR_DOCUMENT4] = new ItemModel("CHAR document", "CHAR documents", GameImages.ITEM_CHAR_DOCUMENT)
            {
                FlavorDescription = @"""Effects vary by subject; speculate genetic differences manifest in patterns."""
            };

            this[IDs.UNIQUE_CHAR_DOCUMENT5] = new ItemModel("CHAR document", "CHAR documents", GameImages.ITEM_CHAR_DOCUMENT)
            {
                FlavorDescription = @"""TEST #241 should alter marker 17 for enhanced stength and smell."""
            };
            this[IDs.UNIQUE_CHAR_DOCUMENT6] = new ItemModel("CHAR document", "CHAR documents", GameImages.ITEM_CHAR_DOCUMENT)
            {
                FlavorDescription = "Operation manual regarding restoring power to the facility."
            };
            #endregion

            #region Fixes/Post processing
            //for (int i = (int)IDs._FIRST; i < (int)IDs._COUNT; i++)
            for (int i = 0; i < (int)IDs._COUNT; i++) //@@MP - took _FIRST out because for some reason it became totally bugged (Release 4)
            {
                ItemModel model = this[i];

                // grammar.
                model.IsAn = StartsWithVowel(model.SingleName);

                // IsStackable
                model.IsStackable = model.StackingLimit > 1;
            }

            #endregion
        }

        #endregion

        #region Data loading

        #region Helpers
        void Notify(IRogueUI ui, string what, string stage)
        {
            ui.UI_Clear(Color.Black);
            ui.UI_DrawStringBold(Color.White, "Loading "+what+" data : " + stage, 0, 0);
            ui.UI_Repaint();
        }

        CSVLine FindLineForModel(CSVTable table, IDs modelID)
        {
            foreach (CSVLine l in table.Lines)
            {
                if (l[0].ParseText() == modelID.ToString())
                    return l;
            }

            return null;
        }

        _DATA_TYPE_ GetDataFromCSVTable<_DATA_TYPE_>(IRogueUI ui, CSVTable table, Func<CSVLine, _DATA_TYPE_> fn, IDs modelID, string path)
        {
            // get line for model in table.
            CSVLine line = FindLineForModel(table, modelID);
            if (line == null)
                throw new InvalidOperationException(String.Format("model {0} not found : path= {1}", modelID.ToString(), path));

            // get data from line.
            _DATA_TYPE_ data;
            try
            {
                data = fn(line);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(String.Format("invalid data format for model {0}; exception : {1}", modelID.ToString(), e.ToString()));
            }

            // ok.
            return data;
        }

        bool LoadDataFromCSV<_DATA_TYPE_>(IRogueUI ui, string path, string kind, int fieldsCount, Func<CSVLine, _DATA_TYPE_> fn, IDs[] idsToRead, out _DATA_TYPE_[] data)
        {
            //////////////////////////
            // Read & parse csv file.
            //////////////////////////
            Notify(ui, kind, "loading file...");
            // read the whole file.
            List<string> allLines = new List<string>();
            bool ignoreHeader = true;
            using (StreamReader reader = File.OpenText(path))
            {
                while (!reader.EndOfStream)
                {
                    string inLine = reader.ReadLine();
                    if (ignoreHeader)
                    {
                        ignoreHeader = false;
                        continue;
                    }
                    allLines.Add(inLine);
                }
                reader.Close();
            }
            // parse all the lines read.
            Notify(ui, kind, "parsing CSV...");
            CSVParser parser = new CSVParser();
            CSVTable table = parser.ParseToTable(allLines.ToArray(), fieldsCount);

            /////////////
            // Set data.
            /////////////
            Notify(ui, kind, "reading data...");

            data = new _DATA_TYPE_[idsToRead.Length];
            for (int i = 0; i < idsToRead.Length; i++)
            {
                data[i] = GetDataFromCSVTable<_DATA_TYPE_>(ui, table, fn, idsToRead[i], path);
            }

            //////////////
            // all fine.
            /////////////
            Notify(ui, kind, "done!");
            return true;
        }
        #endregion

        #region Medicine
        public bool LoadMedicineFromCSV(IRogueUI ui, string path)
        {
            MedecineData[] data;

            LoadDataFromCSV<MedecineData>(ui, path, "medicine items", MedecineData.COUNT_FIELDS, MedecineData.FromCSVLine,
                new IDs[] { IDs.MEDICINE_BANDAGES, IDs.MEDICINE_MEDIKIT, IDs.MEDICINE_PILLS_SLP, IDs.MEDICINE_PILLS_STA,  IDs.MEDICINE_PILLS_SAN,
                            IDs.MEDICINE_PILLS_ANTIVIRAL, IDs.MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN, IDs.MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN, IDs.MEDICINE_ALCOHOL_BEER_CAN_BLUE,
                            IDs.MEDICINE_ALCOHOL_BEER_CAN_RED, IDs.MEDICINE_ALCOHOL_LIQUOR_AMBER, IDs.MEDICINE_ALCOHOL_LIQUOR_CLEAR, IDs.MEDICINE_CIGARETTES },
                out data);

            DATA_MEDICINE_BANDAGE = data[0];
            DATA_MEDICINE_MEDIKIT = data[1];
            DATA_MEDICINE_PILLS_SLP = data[2];
            DATA_MEDICINE_PILLS_STA = data[3];
            DATA_MEDICINE_PILLS_SAN = data[4];
            DATA_MEDICINE_PILLS_ANTIVIRAL = data[5];
            //@@MP (Release 4)
            DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN = data[6];
            DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN = data[7];
            DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE = data[8];
            DATA_MEDICINE_ALCOHOL_BEER_CAN_RED = data[9];
            DATA_MEDICINE_ALCOHOL_LIQUOR_AMBER = data[10];
            DATA_MEDICINE_ALCOHOL_LIQUOR_CLEAR = data[11];
            DATA_MEDICINE_CIGARETTES = data[12];

            return true;
        }
        #endregion

        #region Food
        public bool LoadFoodFromCSV(IRogueUI ui, string path)
        {
            FoodData[] data;

            LoadDataFromCSV<FoodData>(ui, path, "food items", FoodData.COUNT_FIELDS, FoodData.FromCSVLine,
                new IDs[] { IDs.FOOD_ARMY_RATION, IDs.FOOD_CANNED_FOOD, IDs.FOOD_GROCERIES, IDs.FOOD_WILD_BERRIES },
                out data);

            DATA_FOOD_ARMY_RATION = data[0];
            DATA_FOOD_CANNED_FOOD = data[1];
            DATA_FOOD_GROCERIES = data[2];
            DATA_FOOD_WILD_BERRIES = data[3];

            return true;
        }
        #endregion

        #region Melee weapons
        public bool LoadMeleeWeaponsFromCSV(IRogueUI ui, string path)
        {
            MeleeWeaponData[] data;

            LoadDataFromCSV<MeleeWeaponData>(ui, path, "melee weapons items", MeleeWeaponData.COUNT_FIELDS, MeleeWeaponData.FromCSVLine,
                new IDs[] { IDs.MELEE_BASEBALLBAT, IDs.MELEE_COMBAT_KNIFE, IDs.MELEE_CROWBAR, IDs.MELEE_GOLFCLUB, IDs.MELEE_HUGE_HAMMER, IDs.MELEE_IRON_GOLFCLUB,
                            IDs.MELEE_SHOVEL, IDs.MELEE_SHORT_SHOVEL, IDs.MELEE_TRUNCHEON, IDs.UNIQUE_JASON_MYERS_AXE,
                            IDs.MELEE_IMPROVISED_CLUB, IDs.MELEE_IMPROVISED_SPEAR, IDs.MELEE_SMALL_HAMMER, 
                            IDs.UNIQUE_FAMU_FATARU_KATANA, IDs.UNIQUE_BIGBEAR_BAT, IDs.UNIQUE_ROGUEDJACK_KEYBOARD,
                            IDs.MELEE_TENNIS_RACKET, IDs.MELEE_HOCKEY_STICK, IDs.MELEE_MACHETE, IDs.MELEE_STANDARD_AXE, IDs.MELEE_PICKAXE, IDs.MELEE_PIPE_WRENCH }, //@@MP (Release 3)
                out data);

            DATA_MELEE_BASEBALLBAT = data[0];
            DATA_MELEE_COMBAT_KNIFE = data[1];
            DATA_MELEE_CROWBAR = data[2];
            DATA_MELEE_GOLFCLUB = data[3];
            DATA_MELEE_HUGE_HAMMER = data[4];
            DATA_MELEE_IRON_GOLFCLUB = data[5];
            DATA_MELEE_SHOVEL = data[6];
            DATA_MELEE_SHORT_SHOVEL = data[7];
            DATA_MELEE_TRUNCHEON = data[8];
            DATA_MELEE_UNIQUE_JASON_MYERS_AXE = data[9];
            DATA_MELEE_IMPROVISED_CLUB = data[10];
            DATA_MELEE_IMPROVISED_SPEAR = data[11];
            DATA_MELEE_SMALL_HAMMER = data[12];
            DATA_MELEE_UNIQUE_FAMU_FATARU_KATANA = data[13];
            DATA_MELEE_UNIQUE_BIGBEAR_BAT = data[14];
            DATA_MELEE_UNIQUE_ROGUEDJACK_KEYBOARD = data[15];
            //@@MP (Release 3)
            DATA_MELEE_TENNIS_RACKET = data[16];
            DATA_MELEE_HOCKEY_STICK = data[17];
            DATA_MELEE_MACHETE = data[18];
            DATA_MELEE_STANDARD_AXE = data[19];
            DATA_MELEE_PICKAXE = data[20];
            DATA_MELEE_PIPE_WRENCH = data[21];

            return true;
        }
        #endregion

        #region Ranged weapons
        public bool LoadRangedWeaponsFromCSV(IRogueUI ui, string path)
        {
            RangedWeaponData[] data;

            LoadDataFromCSV<RangedWeaponData>(ui, path, "ranged weapons items", RangedWeaponData.COUNT_FIELDS, RangedWeaponData.FromCSVLine,
                new IDs[] { IDs.RANGED_ARMY_PISTOL, IDs.RANGED_ARMY_RIFLE, IDs.RANGED_HUNTING_CROSSBOW, IDs.RANGED_HUNTING_RIFLE, IDs.RANGED_KOLT_REVOLVER, IDs.RANGED_PISTOL, IDs.RANGED_PRECISION_RIFLE, IDs.RANGED_SHOTGUN,
                            IDs.UNIQUE_SANTAMAN_SHOTGUN, IDs.UNIQUE_HANS_VON_HANZ_PISTOL },
                out data);

            DATA_RANGED_ARMY_PISTOL = data[0];
            DATA_RANGED_ARMY_RIFLE = data[1];
            DATA_RANGED_HUNTING_CROSSBOW = data[2];
            DATA_RANGED_HUNTING_RIFLE = data[3];
            DATA_RANGED_KOLT_REVOLVER = data[4];
            DATA_RANGED_PISTOL = data[5];
            DATA_RANGED_PRECISION_RIFLE = data[6];
            DATA_RANGED_SHOTGUN = data[7];
            DATA_UNIQUE_SANTAMAN_SHOTGUN = data[8];
            DATA_UNIQUE_HANS_VON_HANZ_PISTOL = data[9];

            return true;
        }
        #endregion

        #region Explosives
        public bool LoadExplosivesFromCSV(IRogueUI ui, string path)
        {
            ExplosiveData[] data;

            LoadDataFromCSV<ExplosiveData>(ui, path, "explosives items", ExplosiveData.COUNT_FIELDS, ExplosiveData.FromCSVLine,
                new IDs[] { IDs.EXPLOSIVE_GRENADE, IDs.EXPLOSIVE_MOLOTOV, IDs.EXPLOSIVE_DYNAMITE }, //@@MP (Release 4)
                out data);

            DATA_EXPLOSIVE_GRENADE = data[0];
            //@@MP (Release 4)
            DATA_EXPLOSIVE_MOLOTOV = data[1];
            DATA_EXPLOSIVE_DYNAMITE = data[2];

            return true;
        }
        #endregion

        #region Barricading material
        public bool LoadBarricadingMaterialFromCSV(IRogueUI ui, string path)
        {
            BarricadingMaterialData[] data;

            LoadDataFromCSV<BarricadingMaterialData>(ui, path, "barricading items", BarricadingMaterialData.COUNT_FIELDS, BarricadingMaterialData.FromCSVLine,
                new IDs[] { IDs.BAR_WOODEN_PLANK },
                out data);

            DATA_BAR_WOODEN_PLANK = data[0];

            return true;
        }
        #endregion

        #region Armors
        public bool LoadArmorsFromCSV(IRogueUI ui, string path)
        {
            ArmorData[] data;

            LoadDataFromCSV<ArmorData>(ui, path, "armors items", ArmorData.COUNT_FIELDS, ArmorData.FromCSVLine,
                new IDs[] { IDs.ARMOR_ARMY_BODYARMOR,IDs.ARMOR_CHAR_LIGHT_BODYARMOR, 
                            IDs.ARMOR_HELLS_SOULS_JACKET, IDs.ARMOR_FREE_ANGELS_JACKET, 
                            IDs.ARMOR_POLICE_JACKET, IDs.ARMOR_POLICE_RIOT, 
                            IDs.ARMOR_HUNTER_VEST },
                out data);

            DATA_ARMOR_ARMY = data[0];
            DATA_ARMOR_CHAR = data[1];
            DATA_ARMOR_HELLS_SOULS_JACKET = data[2];
            DATA_ARMOR_FREE_ANGELS_JACKET = data[3];
            DATA_ARMOR_POLICE_JACKET = data[4];
            DATA_ARMOR_POLICE_RIOT = data[5];
            DATA_ARMOR_HUNTER_VEST = data[6];

            return true;
        }
        #endregion

        #region Trackers
        public bool LoadTrackersFromCSV(IRogueUI ui, string path)
        {
            TrackerData[] data;

            LoadDataFromCSV<TrackerData>(ui, path, "trackers items", TrackerData.COUNT_FIELDS, TrackerData.FromCSVLine,
                new IDs[] { IDs.TRACKER_BLACKOPS, IDs.TRACKER_CELL_PHONE, IDs.TRACKER_ZTRACKER, IDs.TRACKER_POLICE_RADIO },
                out data);

            DATA_TRACKER_BLACKOPS_GPS = data[0];
            DATA_TRACKER_CELL_PHONE = data[1];
            DATA_TRACKER_ZTRACKER = data[2];
            DATA_TRACKER_POLICE_RADIO = data[3];

            return true;
        }
        #endregion

        #region Spray paints
        public bool LoadSpraypaintsFromCSV(IRogueUI ui, string path)
        {
            SprayPaintData[] data;

            LoadDataFromCSV<SprayPaintData>(ui, path, "spraypaint items", SprayPaintData.COUNT_FIELDS, SprayPaintData.FromCSVLine,
                new IDs[] { IDs.SPRAY_PAINT1, IDs.SPRAY_PAINT2, IDs.SPRAY_PAINT3, IDs.SPRAY_PAINT4 },
                out data);

            DATA_SPRAY_PAINT1 = data[0];
            DATA_SPRAY_PAINT2 = data[1];
            DATA_SPRAY_PAINT3 = data[2];
            DATA_SPRAY_PAINT4 = data[3];

            return true;
        }
        #endregion

        #region Lights
        public bool LoadLightsFromCSV(IRogueUI ui, string path)
        {
            LightData[] data;

            LoadDataFromCSV<LightData>(ui, path, "lights items", LightData.COUNT_FIELDS, LightData.FromCSVLine,
                new IDs[] { IDs.LIGHT_FLASHLIGHT, IDs.LIGHT_BIG_FLASHLIGHT },
                out data);

            DATA_LIGHT_FLASHLIGHT = data[0];
            DATA_LIGHT_BIG_FLASHLIGHT = data[1];

            return true;
        }
        #endregion

        #region Scentsprays
        public bool LoadScentspraysFromCSV(IRogueUI ui, string path)
        {
            ScentSprayData[] data;

            LoadDataFromCSV<ScentSprayData>(ui, path, "scentsprays items", ScentSprayData.COUNT_FIELDS, ScentSprayData.FromCSVLine,
                new IDs[] { IDs.SCENT_SPRAY_STENCH_KILLER },
                out data);

            DATA_SCENT_SPRAY_STENCH_KILLER = data[0];

            return true;
        }
        #endregion

        #region Traps
        public bool LoadTrapsFromCSV(IRogueUI ui, string path)
        {
            TrapData[] data;

            LoadDataFromCSV<TrapData>(ui, path, "traps items", TrapData.COUNT_FIELDS, TrapData.FromCSVLine,
                new IDs[] { IDs.TRAP_EMPTY_CAN, IDs.TRAP_BEAR_TRAP, IDs.TRAP_SPIKES, IDs.TRAP_BARBED_WIRE },
                out data);

            DATA_TRAP_EMPTY_CAN = data[0];
            DATA_TRAP_BEAR_TRAP = data[1];
            DATA_TRAP_SPIKES = data[2];
            DATA_TRAP_BARBED_WIRE = data[3];

            return true;
        }
        #endregion

        #region Entertainment
        public bool LoadEntertainmentFromCSV(IRogueUI ui, string path)
        {
            EntData[] data;

            LoadDataFromCSV<EntData>(ui, path, "entertainment items", EntData.COUNT_FIELDS, EntData.FromCSVLine,
                new IDs[] { IDs.ENT_BOOK, IDs.ENT_MAGAZINE },
                out data);

            DATA_ENT_BOOK = data[0];
            DATA_ENT_MAGAZINE = data[1];

            return true;
        }
        #endregion
        #endregion
    }
}
