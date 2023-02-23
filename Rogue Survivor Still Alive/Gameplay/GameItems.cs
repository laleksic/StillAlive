using System;
using System.Collections.Generic;
using System.IO;
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

            MEDICINE_SMALL_MEDIKIT,
            MEDICINE_LARGE_MEDIKIT,
            MEDICINE_PILLS_STA,
            MEDICINE_PILLS_SLP,
            MEDICINE_PILLS_SAN,
            MEDICINE_PILLS_ANTIVIRAL,
            //@@MP (Release 4)
            MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN,
            MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN,
            MEDICINE_ALCOHOL_BEER_CAN_BLUE,
            MEDICINE_ALCOHOL_BEER_CAN_RED,
            MEDICINE_CIGARETTES,
            MEDICINE_ENERGY_DRINK, //@@MP (Release 7-1)

            FOOD_ARMY_RATION,
            FOOD_GROCERIES,
            FOOD_CANNED_FOOD,
            FOOD_WILD_BERRIES, //@@MP (Release 4)
            FOOD_VEGETABLES, //@@MP (Release 5-3), (Release 5-5)
            FOOD_SNACK_BAR, //@@MP (Release 7-1)
            FOOD_PEANUTS, //@@MP (Release 7-3)
            FOOD_GRAPES, //@@MP (Release 7-3)
            //@@MP (Release 7-6)
            FOOD_RAW_FISH, 
            FOOD_COOKED_FISH,
            FOOD_RAW_RABBIT,
            FOOD_COOKED_RABBIT,
            FOOD_RAW_CHICKEN,
            FOOD_COOKED_CHICKEN,
            FOOD_RAW_DOG_MEAT,
            FOOD_COOKED_DOG_MEAT,
            FOOD_RAW_HUMAN_FLESH,
            FOOD_COOKED_HUMAN_FLESH,
            FOOD_CHICKEN_EGG,

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
            //@@MP (Release 3)
            MELEE_TENNIS_RACKET,
            MELEE_HOCKEY_STICK,
            MELEE_MACHETE,
            MELEE_STANDARD_AXE,
            MELEE_PICKAXE,
            MELEE_PIPE_WRENCH,
            //@@MP (Release 7-1)
            MELEE_CHAINSAW,
            //@@MP - the following were donated by Jess via the forums (Release 7-6)
            MELEE_CLEAVER,
            MELEE_BRASS_KNUCKLES,
            MELEE_FLAIL,
            MELEE_KITCHEN_KNIFE,
            MELEE_SCIMITAR,
            MELEE_MACE,
            MELEE_NUNCHAKU,
            MELEE_FRYING_PAN,
            MELEE_PITCH_FORK,
            MELEE_SCYTHE,
            MELEE_SICKLE,
            MELEE_SPEAR,
            MELEE_SPIKED_MACE,
            MELEE_FIRE_AXE,

            RANGED_ARMY_PISTOL,
            RANGED_ARMY_PRECISION_RIFLE, //@@MP (Release 7-6)
            RANGED_ARMY_RIFLE1,
            RANGED_HUNTING_CROSSBOW,
            RANGED_HUNTING_RIFLE,
            RANGED_PISTOL,
            RANGED_REVOLVER,
            RANGED_PRECISION_RIFLE,
            RANGED_SHOTGUN,
            RANGED_NAIL_GUN, //@@MP (Release 5-1)
            RANGED_FLAMETHROWER, //@@MP (Release 7-1)
            RANGED_STUN_GUN, //@@MP (Release 7-2)
            //@@MP - the following (up to the launcher) were donated by Jess via the forums (Release 7-6)
            RANGED_SMG,
            RANGED_DOUBLE_BARREL,
            RANGED_MINIGUN,
            RANGED_TACTICAL_SHOTGUN,
            RANGED_ARMY_RIFLE2,
            RANGED_ARMY_RIFLE3,
            RANGED_ARMY_RIFLE4,
            RANGED_GRENADE_LAUNCHER,
            RANGED_BIO_FORCE_GUN, //@@MP (Release 7-2)

            EXPLOSIVE_GRENADE,
            EXPLOSIVE_GRENADE_PRIMED,
            //@@MP (Release 4)
            EXPLOSIVE_MOLOTOV,
            EXPLOSIVE_MOLOTOV_PRIMED,
            EXPLOSIVE_DYNAMITE,
            EXPLOSIVE_DYNAMITE_PRIMED,
            //@@MP (Release 6-3)
            EXPLOSIVE_C4,
            EXPLOSIVE_C4_PRIMED,
            //@@MP (Release 7-1)
            EXPLOSIVE_FUEL_CAN,
            EXPLOSIVE_FUEL_CAN_PRIMED,
            EXPLOSIVE_FUEL_PUMP,
            EXPLOSIVE_FUEL_PUMP_PRIMED,
            //@@MP (Release 7-2)
            EXPLOSIVE_SMOKE_GRENADE,
            EXPLOSIVE_SMOKE_GRENADE_PRIMED,
            EXPLOSIVE_FLASHBANG,
            EXPLOSIVE_FLASHBANG_PRIMED,
            //@@MP (Release 7-6)
            EXPLOSIVE_HOLY_HAND_GRENADE,
            EXPLOSIVE_HOLY_HAND_GRENADE_PRIMED,
            EXPLOSIVE_PLASMA_CHARGE,
            EXPLOSIVE_PLASMA_CHARGE_PRIMED,

            BAR_WOODEN_PLANK,
            VEGETABLE_SEEDS, //@@MP (Release 5-3), (Release 5-5)
            SIPHON_KIT, //@@MP (Release 7-1)
            CANDLES_BOX, //@@MP (Release 7-1)
            FLARES_KIT, //@@MP (Release 7-1)
            GLOWSTICKS_BOX, //@@MP (Release 7-1)
            LIQUOR_AMBER,
            LIQUOR_CLEAR,
            POLICE_RIOT_SHIELD, //@@MP (Release 7-2)
            SLEEPING_BAG, //@@MP (Release 7-3)
            FISHING_ROD, //@@MP (Release 7-6)
            MATCHES, //@@MP (Release 7-6)

            ARMOR_ARMY_BODYARMOR,
            ARMOR_CHAR_LIGHT_BODYARMOR,
            ARMOR_HELLS_SOULS_JACKET,
            ARMOR_FREE_ANGELS_JACKET,
            ARMOR_POLICE_JACKET,
            ARMOR_POLICE_RIOT,
            ARMOR_HUNTER_VEST,
            ARMOR_FIRE_HAZARD_SUIT, //@@MP (Release 7-1)
            ARMOR_BIOHAZARD_SUIT, //@@MP (Release 7-6)

            TRACKER_BLACKOPS,
            TRACKER_CELL_PHONE,
            TRACKER_ZTRACKER,
            TRACKER_POLICE_RADIO,

            SPRAY_PAINT1,
            SPRAY_PAINT2,
            SPRAY_PAINT3,
            SPRAY_PAINT4,
            PAINT_THINNER, //@@MP (Release 7-6)
            FIRE_EXTINGUISHER, //@@MP (Release 7-6)

            SCENT_SPRAY_STENCH_KILLER,

            LIGHT_FLASHLIGHT,
            LIGHT_BIG_FLASHLIGHT,
            LIGHT_NIGHT_VISION, //@@MP (Release 6-3)
            LIGHT_BINOCULARS, //@@MP (Release 7-1)
            LIGHT_FLARE, //@@MP (Release 7-1)
            LIGHT_GLOWSTICK, //@@MP (Release 7-1)

            AMMO_LIGHT_PISTOL,
            AMMO_HEAVY_PISTOL,
            AMMO_LIGHT_RIFLE,
            AMMO_HEAVY_RIFLE,
            AMMO_SHOTGUN,
            AMMO_BOLTS,
            AMMO_NAILS, //@@MP (Release 5-1)
            AMMO_PRECISION_RIFLE, //@@MP (Release 6-6)
            AMMO_FUEL, //@@MP (Release 7-1)
            AMMO_MINIGUN, //@@MP (Release 7-6)
            AMMO_GRENADES, //@@MP (Release 7-6)
            AMMO_PLASMA, //@@MP (Release 7-6)

            TRAP_EMPTY_CAN,
            TRAP_BEAR_TRAP,
            TRAP_SPIKES,
            TRAP_BARBED_WIRE,

            ENT_BOOK_CHAR,
            ENT_BOOK_BLUE, //@@MP (Release 7-6)
            ENT_BOOK_GREEN, //@@MP (Release 7-6)
            ENT_BOOK_RED, //@@MP (Release 7-6)
            ENT_MAGAZINE1,
            ENT_MAGAZINE2, //@@MP (Release 7-6)
            ENT_MAGAZINE3, //@@MP (Release 7-6)
            ENT_MAGAZINE4, //@@MP (Release 7-6)

            UNIQUE_SUBWAY_BADGE,
            UNIQUE_FAMU_FATARU_KATANA,
            UNIQUE_BIGBEAR_BAT,
            UNIQUE_ROGUEDJACK_KEYBOARD,
            UNIQUE_SANTAMAN_SHOTGUN,
            UNIQUE_HANS_VON_HANZ_PISTOL,
            UNIQUE_ARMY_ACCESS_BADGE, //@@MP (Release 6-3)
            UNIQUE_BOOK_OF_ARMAMENTS, //@@MP (Release 7-6)

            //@@MP (Release 3)
            UNIQUE_CHAR_DOCUMENT1,
            UNIQUE_CHAR_DOCUMENT2,
            UNIQUE_CHAR_DOCUMENT3,
            UNIQUE_CHAR_DOCUMENT4,
            UNIQUE_CHAR_DOCUMENT5,
            UNIQUE_CHAR_DOCUMENT6,

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

        MedecineData DATA_MEDICINE_SMALL_MEDIKIT;
        public ItemMedicineModel SMALL_MEDIKIT { get { return this[IDs.MEDICINE_SMALL_MEDIKIT] as ItemMedicineModel; } }
        MedecineData DATA_MEDICINE_LARGE_MEDIKIT;
        public ItemMedicineModel LARGE_MEDIKIT { get { return this[IDs.MEDICINE_LARGE_MEDIKIT] as ItemMedicineModel; } }
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
        MedecineData DATA_MEDICINE_CIGARETTES;
        public ItemMedicineModel CIGARETTES { get { return this[IDs.MEDICINE_CIGARETTES] as ItemMedicineModel; } }
        //@@MP (Release 7-1)
        MedecineData DATA_MEDICINE_ENERGY_DRINK;
        public ItemMedicineModel ENERGY_DRINK { get { return this[IDs.MEDICINE_ENERGY_DRINK] as ItemMedicineModel; } }
        #endregion

        #region Food
        struct FoodData
        {
            public const int COUNT_FIELDS = 9;

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int NUTRITION { get; set; }
            public int BESTBEFORE { get; set; }
            public int STACKINGLIMIT { get; set; }
            public bool CANCAUSEFOODPOISONING { get; set; } //@@MP (Release 7-6)
            public bool CANBECOOKED { get; set; } //@@MP (Release 7-6)
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
                    CANCAUSEFOODPOISONING = line[6].ParseBool(),
                    CANBECOOKED = line[7].ParseBool(),
                    FLAVOR = line[8].ParseText()
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
        FoodData DATA_FOOD_VEGETABLES; //@MP (Release 5-3), (Release 5-5)
        public ItemFoodModel VEGETABLES { get { return this[IDs.FOOD_VEGETABLES] as ItemFoodModel; } }
        FoodData DATA_FOOD_SNACK_BAR; //@MP (Release 7-1)
        public ItemFoodModel SNACK_BAR { get { return this[IDs.FOOD_SNACK_BAR] as ItemFoodModel; } }
        FoodData DATA_FOOD_PEANUTS; //@MP (Release 7-3)
        public ItemFoodModel PEANUTS { get { return this[IDs.FOOD_PEANUTS] as ItemFoodModel; } }
        FoodData DATA_FOOD_GRAPES; //@MP (Release 7-3)
        public ItemFoodModel GRAPES { get { return this[IDs.FOOD_GRAPES] as ItemFoodModel; } }
        FoodData DATA_FOOD_RAW_FISH; //@MP (Release 7-6)
        public ItemFoodModel RAW_FISH { get { return this[IDs.FOOD_RAW_FISH] as ItemFoodModel; } }
        FoodData DATA_FOOD_COOKED_FISH; //@MP (Release 7-6)
        public ItemFoodModel COOKED_FISH { get { return this[IDs.FOOD_COOKED_FISH] as ItemFoodModel; } }
        FoodData DATA_FOOD_RAW_RABBIT; //@MP (Release 7-6)
        public ItemFoodModel RAW_RABBIT { get { return this[IDs.FOOD_RAW_RABBIT] as ItemFoodModel; } }
        FoodData DATA_FOOD_COOKED_RABBIT; //@MP (Release 7-6)
        public ItemFoodModel COOKED_RABBIT { get { return this[IDs.FOOD_COOKED_RABBIT] as ItemFoodModel; } }
        FoodData DATA_FOOD_RAW_CHICKEN; //@MP (Release 7-6)
        public ItemFoodModel RAW_CHICKEN { get { return this[IDs.FOOD_RAW_CHICKEN] as ItemFoodModel; } }
        FoodData DATA_FOOD_COOKED_CHICKEN; //@MP (Release 7-6)
        public ItemFoodModel COOKED_CHICKEN { get { return this[IDs.FOOD_COOKED_CHICKEN] as ItemFoodModel; } }
        FoodData DATA_FOOD_RAW_DOG_MEAT; //@MP (Release 7-6)
        public ItemFoodModel RAW_DOG_MEAT { get { return this[IDs.FOOD_RAW_DOG_MEAT] as ItemFoodModel; } }
        FoodData DATA_FOOD_COOKED_DOG_MEAT; //@MP (Release 7-6)
        public ItemFoodModel COOKED_DOG_MEAT { get { return this[IDs.FOOD_COOKED_DOG_MEAT] as ItemFoodModel; } }
        FoodData DATA_FOOD_RAW_HUMAN_FLESH; //@MP (Release 7-6)
        public ItemFoodModel RAW_HUMAN_FLESH { get { return this[IDs.FOOD_RAW_HUMAN_FLESH] as ItemFoodModel; } }
        FoodData DATA_FOOD_COOKED_HUMAN_FLESH; //@MP (Release 7-6)
        public ItemFoodModel COOKED_HUMAN_FLESH { get { return this[IDs.FOOD_COOKED_HUMAN_FLESH] as ItemFoodModel; } }
        FoodData DATA_FOOD_CHICKEN_EGG; //@MP (Release 7-6)
        public ItemFoodModel CHICKEN_EGG { get { return this[IDs.FOOD_CHICKEN_EGG] as ItemFoodModel; } }
        #endregion

        #region Melee weapons
        struct MeleeWeaponData
        {
            public const int COUNT_FIELDS = 13;  // alpha10

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int ATK { get; set; }
            public int DMG { get; set; }
            public int STA { get; set; }
            public int DISARM { get; set; }  // alpha10
            public int TOOLBASHDMGBONUS { get; set; }  // alpha10
            public float TOOLBUILDBONUS { get; set; } // alpha10
            public int STACKINGLIMIT { get; set; }
            public bool ISFRAGILE { get; set; }
            public int WEIGHT { get; set; } //@@MP (Release 7-6)
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
                    DISARM = line[6].ParseInt(),  // alpha10
                    TOOLBASHDMGBONUS = line[7].ParseInt(), // alpha10
                    TOOLBUILDBONUS = line[8].ParseFloat(),  // alpha10
                    STACKINGLIMIT = line[9].ParseInt(),
                    ISFRAGILE = line[10].ParseBool(),
                    WEIGHT = line[11].ParseInt(),
                    FLAVOR = line[12].ParseText()
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
        //@@MP (Release 7-1)
        MeleeWeaponData DATA_MELEE_CHAINSAW;
        public ItemMeleeWeaponModel CHAINSAW { get { return this[IDs.MELEE_CHAINSAW] as ItemMeleeWeaponModel; } }
        //@@MP (Release 7-6)
        MeleeWeaponData DATA_MELEE_CLEAVER;
        public ItemMeleeWeaponModel CLEAVER { get { return this[IDs.MELEE_CLEAVER] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_BRASS_KNUCKLES;
        public ItemMeleeWeaponModel BRASS_KNUCKLES { get { return this[IDs.MELEE_BRASS_KNUCKLES] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_FLAIL;
        public ItemMeleeWeaponModel FLAIL { get { return this[IDs.MELEE_FLAIL] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_KITCHEN_KNIFE;
        public ItemMeleeWeaponModel KITCHEN_KNIFE { get { return this[IDs.MELEE_KITCHEN_KNIFE] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_SCIMITAR;
        public ItemMeleeWeaponModel SCIMITAR { get { return this[IDs.MELEE_SCIMITAR] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_MACE;
        public ItemMeleeWeaponModel MACE { get { return this[IDs.MELEE_MACE] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_NUNCHAKU;
        public ItemMeleeWeaponModel NUNCHAKU { get { return this[IDs.MELEE_NUNCHAKU] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_FRYING_PAN;
        public ItemMeleeWeaponModel FRYING_PAN { get { return this[IDs.MELEE_FRYING_PAN] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_PITCH_FORK;
        public ItemMeleeWeaponModel PITCH_FORK { get { return this[IDs.MELEE_PITCH_FORK] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_SCYTHE;
        public ItemMeleeWeaponModel SCYTHE { get { return this[IDs.MELEE_SCYTHE] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_SICKLE;
        public ItemMeleeWeaponModel SICKLE { get { return this[IDs.MELEE_SICKLE] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_SPEAR;
        public ItemMeleeWeaponModel SPEAR { get { return this[IDs.MELEE_SPEAR] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_SPIKED_MACE;
        public ItemMeleeWeaponModel SPIKED_MACE { get { return this[IDs.MELEE_SPIKED_MACE] as ItemMeleeWeaponModel; } }
        MeleeWeaponData DATA_MELEE_FIRE_AXE;
        public ItemMeleeWeaponModel FIRE_AXE { get { return this[IDs.MELEE_FIRE_AXE] as ItemMeleeWeaponModel; } }
        #endregion

        #region Ranged weapons
        struct RangedWeaponData
        {
            public const int COUNT_FIELDS = 11; // alpha10

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int ATK { get; set; }
            public int RAPID1 { get; set; } // alpha10
            public int RAPID2 { get; set; } // alpha10
            public int DMG { get; set; }
            public int RANGE { get; set; }
            public int MAXAMMO { get; set; }
            public int WEIGHT { get; set; } //@@MP (Release 7-6)
            public string FLAVOR { get; set; }

            public static RangedWeaponData FromCSVLine(CSVLine line)
            {
                return new RangedWeaponData()
                {
                    NAME = line[1].ParseText(),
                    PLURAL = line[2].ParseText(),
                    ATK = line[3].ParseInt(),
                    RAPID1 = line[4].ParseInt(),
                    RAPID2 = line[5].ParseInt(),
                    DMG = line[6].ParseInt(),
                    RANGE = line[7].ParseInt(),
                    MAXAMMO = line[8].ParseInt(),
                    WEIGHT = line[9].ParseInt(),
                    FLAVOR = line[10].ParseText()
                };
            }
        }

        RangedWeaponData DATA_RANGED_ARMY_PISTOL;
        public ItemRangedWeaponModel ARMY_PISTOL { get { return this[IDs.RANGED_ARMY_PISTOL] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_ARMY_PRECISION_RIFLE;
        public ItemRangedWeaponModel ARMY_PRECISION_RIFLE { get { return this[IDs.RANGED_ARMY_PRECISION_RIFLE] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_ARMY_RIFLE1;
        public ItemRangedWeaponModel ARMY_RIFLE1 { get { return this[IDs.RANGED_ARMY_RIFLE1] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_HUNTING_CROSSBOW;
        public ItemRangedWeaponModel HUNTING_CROSSBOW { get { return this[IDs.RANGED_HUNTING_CROSSBOW] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_HUNTING_RIFLE;
        public ItemRangedWeaponModel HUNTING_RIFLE { get { return this[IDs.RANGED_HUNTING_RIFLE] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_REVOLVER;
        public ItemRangedWeaponModel REVOLVER { get { return this[IDs.RANGED_REVOLVER] as ItemRangedWeaponModel; } }
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
        RangedWeaponData DATA_RANGED_NAIL_GUN; //@@MP (Release 5-1)
        public ItemRangedWeaponModel NAIL_GUN { get { return this[IDs.RANGED_NAIL_GUN] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_FLAMETHROWER; //@@MP (Release 7-1)
        public ItemRangedWeaponModel FLAMETHROWER { get { return this[IDs.RANGED_FLAMETHROWER] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_STUN_GUN; //@@MP (Release 7-2)
        public ItemRangedWeaponModel STUN_GUN { get { return this[IDs.RANGED_STUN_GUN] as ItemRangedWeaponModel; } }
        //@@MP (Release 7-6)
        RangedWeaponData DATA_RANGED_SMG;
        public ItemRangedWeaponModel SMG { get { return this[IDs.RANGED_SMG] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_DOUBLE_BARREL;
        public ItemRangedWeaponModel DOUBLE_BARREL { get { return this[IDs.RANGED_DOUBLE_BARREL] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_MINIGUN;
        public ItemRangedWeaponModel MINIGUN { get { return this[IDs.RANGED_MINIGUN] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_TACTICAL_SHOTGUN;
        public ItemRangedWeaponModel TACTICAL_SHOTGUN { get { return this[IDs.RANGED_TACTICAL_SHOTGUN] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_ARMY_RIFLE2;
        public ItemRangedWeaponModel ARMY_RIFLE2 { get { return this[IDs.RANGED_ARMY_RIFLE2] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_ARMY_RIFLE3;
        public ItemRangedWeaponModel ARMY_RIFLE3 { get { return this[IDs.RANGED_ARMY_RIFLE3] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_ARMY_RIFLE4;
        public ItemRangedWeaponModel ARMY_RIFLE4 { get { return this[IDs.RANGED_ARMY_RIFLE4] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_GRENADE_LAUNCHER;
        public ItemRangedWeaponModel GRENADE_LAUNCHER { get { return this[IDs.RANGED_GRENADE_LAUNCHER] as ItemRangedWeaponModel; } }
        RangedWeaponData DATA_RANGED_BIO_FORCE_GUN;
        public ItemRangedWeaponModel BIO_FORCE_GUN { get { return this[IDs.RANGED_BIO_FORCE_GUN] as ItemRangedWeaponModel; } }
        #endregion

        #region Ammos
        public ItemAmmoModel AMMO_LIGHT_PISTOL { get { return this[IDs.AMMO_LIGHT_PISTOL] as ItemAmmoModel; } }
        public ItemAmmoModel AMMO_HEAVY_PISTOL { get { return this[IDs.AMMO_HEAVY_PISTOL] as ItemAmmoModel; } }
        public ItemAmmoModel AMMO_LIGHT_RIFLE { get { return this[IDs.AMMO_LIGHT_RIFLE] as ItemAmmoModel; } }
        public ItemAmmoModel AMMO_HEAVY_RIFLE { get { return this[IDs.AMMO_HEAVY_RIFLE] as ItemAmmoModel; } }
        public ItemAmmoModel AMMO_SHOTGUN { get { return this[IDs.AMMO_SHOTGUN] as ItemAmmoModel; } }
        public ItemAmmoModel AMMO_BOLTS { get { return this[IDs.AMMO_BOLTS] as ItemAmmoModel; } }
        public ItemAmmoModel AMMO_NAILS { get { return this[IDs.AMMO_NAILS] as ItemAmmoModel; } } //@@MP (Release 5-1)
        public ItemAmmoModel AMMO_PRECISION_RIFLE { get { return this[IDs.AMMO_PRECISION_RIFLE] as ItemAmmoModel; } } //@@MP (Release 6-6)
        public ItemAmmoModel AMMO_FUEL { get { return this[IDs.AMMO_FUEL] as ItemAmmoModel; } } //@@MP (Release 7-1)
        public ItemAmmoModel AMMO_MINIGUN { get { return this[IDs.AMMO_MINIGUN] as ItemAmmoModel; } } //@@MP (Release 7-6)
        public ItemAmmoModel AMMO_GRENADES { get { return this[IDs.AMMO_GRENADES] as ItemAmmoModel; } } //@@MP (Release 7-6)
        public ItemAmmoModel AMMO_PLASMA { get { return this[IDs.AMMO_PLASMA] as ItemAmmoModel; } } //@@MP (Release 7-6)
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
        //@@MP (Release 6-3)
        ExplosiveData DATA_EXPLOSIVE_C4;
        public ItemGrenadeModel C4 { get { return this[IDs.EXPLOSIVE_C4] as ItemGrenadeModel; } }
        public ItemGrenadePrimedModel C4_PRIMED { get { return this[IDs.EXPLOSIVE_C4_PRIMED] as ItemGrenadePrimedModel; } }
        //@@MP (Release 7-1)
        ExplosiveData DATA_EXPLOSIVE_FUEL_CAN;
        public ItemGrenadeModel FUEL_CAN { get { return this[IDs.EXPLOSIVE_FUEL_CAN] as ItemGrenadeModel; } }
        public ItemGrenadePrimedModel FUEL_CAN_PRIMED { get { return this[IDs.EXPLOSIVE_FUEL_CAN_PRIMED] as ItemGrenadePrimedModel; } }
        ExplosiveData DATA_EXPLOSIVE_FUEL_PUMP;
        public ItemGrenadeModel FUEL_PUMP { get { return this[IDs.EXPLOSIVE_FUEL_PUMP] as ItemGrenadeModel; } }
        public ItemGrenadePrimedModel FUEL_PUMP_PRIMED { get { return this[IDs.EXPLOSIVE_FUEL_PUMP_PRIMED] as ItemGrenadePrimedModel; } }
        //@@MP (Release 7-2)
        ExplosiveData DATA_EXPLOSIVE_SMOKE_GRENADE;
        public ItemGrenadeModel SMOKE_GRENADE { get { return this[IDs.EXPLOSIVE_SMOKE_GRENADE] as ItemGrenadeModel; } }
        public ItemGrenadePrimedModel SMOKE_GRENADE_PRIMED { get { return this[IDs.EXPLOSIVE_SMOKE_GRENADE_PRIMED] as ItemGrenadePrimedModel; } }
        ExplosiveData DATA_EXPLOSIVE_FLASHBANG;
        public ItemGrenadeModel FLASHBANG { get { return this[IDs.EXPLOSIVE_FLASHBANG] as ItemGrenadeModel; } }
        public ItemGrenadePrimedModel FLASHBANG_PRIMED { get { return this[IDs.EXPLOSIVE_FLASHBANG_PRIMED] as ItemGrenadePrimedModel; } }
        //@@MP (Release 7-6)
        ExplosiveData DATA_EXPLOSIVE_HOLY_HAND_GRENADE;
        public ItemGrenadeModel HOLY_HAND_GRENADE { get { return this[IDs.EXPLOSIVE_HOLY_HAND_GRENADE] as ItemGrenadeModel; } }
        public ItemGrenadePrimedModel HOLY_HAND_GRENADE_PRIMED { get { return this[IDs.EXPLOSIVE_HOLY_HAND_GRENADE_PRIMED] as ItemGrenadePrimedModel; } }
        ExplosiveData DATA_EXPLOSIVE_PLASMA_CHARGE;
        public ItemGrenadeModel PLASMA_CHARGE { get { return this[IDs.EXPLOSIVE_PLASMA_CHARGE] as ItemGrenadeModel; } }
        public ItemGrenadePrimedModel PLASMA_CHARGE_PRIMED { get { return this[IDs.EXPLOSIVE_PLASMA_CHARGE_PRIMED] as ItemGrenadePrimedModel; } }
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
            public const int COUNT_FIELDS = 10;

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int PRO_HIT { get; set; }
            public int PRO_SHOT { get; set; }
            public int ENC { get; set; }
            public int WEIGHT { get; set; }
            public int FIRE_RESIST { get; set; } //@@MP (Release 7-1)
            public int INFECTION_RESIST { get; set; } //@@MP (Release 7-6)
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
                    FIRE_RESIST = line[7].ParseInt(), //@@MP (Release 7-1)
                    INFECTION_RESIST = line[8].ParseInt(), //@@MP (Release 7-1)
                    FLAVOR = line[9].ParseText()
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
        ArmorData DATA_ARMOR_FIRE_HAZARD_SUIT; //@@MP (Release 7-1)
        public ItemBodyArmorModel FIRE_HAZARD_SUIT { get { return this[IDs.ARMOR_FIRE_HAZARD_SUIT] as ItemBodyArmorModel; } }
        ArmorData DATA_ARMOR_BIOHAZARD_SUIT; //@@MP (Release 7-6)
        public ItemBodyArmorModel BIOHAZARD_SUIT { get { return this[IDs.ARMOR_BIOHAZARD_SUIT] as ItemBodyArmorModel; } }
        #endregion

        #region Trackers
        struct TrackerData
        {
            public const int COUNT_FIELDS = 6; // alpha10

            public string NAME { get; set; }
            public string PLURAL { get; set; }
            public int BATTERIES { get; set; }
            public bool HASCLOCK { get; set; }  // alpha10
            public string FLAVOR { get; set; }

            public static TrackerData FromCSVLine(CSVLine line)
            {
                return new TrackerData()
                {
                    NAME = line[1].ParseText(),
                    PLURAL = line[2].ParseText(),
                    BATTERIES = line[3].ParseInt(),
                    HASCLOCK = line[4].ParseBool(),  // alpha10
                    FLAVOR = line[5].ParseText()
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
        SprayPaintData DATA_PAINT_THINNER;  //@@MP (Release 7-6)
        public ItemSprayPaintModel PAINT_THINNER { get { return this[IDs.PAINT_THINNER] as ItemSprayPaintModel; } }
        SprayPaintData DATA_FIRE_EXTINGUISHER;  //@@MP (Release 7-6)
        public ItemSprayPaintModel FIRE_EXTINGUISHER { get { return this[IDs.FIRE_EXTINGUISHER] as ItemSprayPaintModel; } }
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
        LightData DATA_LIGHT_NIGHT_VISION; //@@MP (Release 6-3)
        public ItemLightModel NIGHT_VISION { get { return this[IDs.LIGHT_NIGHT_VISION] as ItemLightModel; } }
        //@@MP (Release 7-1)
        LightData DATA_LIGHT_BINOCULARS;
        public ItemLightModel BINOCULARS { get { return this[IDs.LIGHT_BINOCULARS] as ItemLightModel; } }
        LightData DATA_LIGHT_FLARE;
        public ItemLightModel LIGHT_FLARE { get { return this[IDs.LIGHT_FLARE] as ItemLightModel; } }
        LightData DATA_LIGHT_GLOWSTICK;
        public ItemLightModel LIGHT_GLOWSTICK { get { return this[IDs.LIGHT_GLOWSTICK] as ItemLightModel; } }
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

        EntData DATA_ENT_BOOK_CHAR;
        public ItemModel BOOK_CHAR { get { return this[IDs.ENT_BOOK_CHAR]; } }
        EntData DATA_ENT_BOOK_BLUE;
        public ItemModel BOOK_BLUE { get { return this[IDs.ENT_BOOK_BLUE]; } }
        EntData DATA_ENT_BOOK_GREEN;
        public ItemModel BOOK_GREEN { get { return this[IDs.ENT_BOOK_GREEN]; } }
        EntData DATA_ENT_BOOK_RED;
        public ItemModel BOOK_RED { get { return this[IDs.ENT_BOOK_RED]; } }
        EntData DATA_ENT_MAGAZINE1;
        public ItemModel MAGAZINE1 { get { return this[IDs.ENT_MAGAZINE1]; } }
        EntData DATA_ENT_MAGAZINE2;
        public ItemModel MAGAZINE2 { get { return this[IDs.ENT_MAGAZINE2]; } }
        EntData DATA_ENT_MAGAZINE3;
        public ItemModel MAGAZINE3 { get { return this[IDs.ENT_MAGAZINE3]; } }
        EntData DATA_ENT_MAGAZINE4;
        public ItemModel MAGAZINE4 { get { return this[IDs.ENT_MAGAZINE4]; } }
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
        public ItemModel UNIQUE_ARMY_ACCESS_BADGE { get { return this[IDs.UNIQUE_ARMY_ACCESS_BADGE]; } } //@MP (Release 6-3)
        public ItemModel UNIQUE_BOOK_OF_ARMAMENTS { get { return this[IDs.UNIQUE_BOOK_OF_ARMAMENTS]; } } //@MP (Release 7-6)
        #endregion

        #region Miscellaneous
        public ItemModel VEGETABLE_SEEDS { get { return this[IDs.VEGETABLE_SEEDS]; } } //@MP (Release 5-5)
        public ItemModel SIPHON_KIT { get { return this[IDs.SIPHON_KIT]; } } //@MP (Release 7-1)
        public ItemModel CANDLES_BOX { get { return this[IDs.CANDLES_BOX]; } } //@MP (Release 7-1)
        public ItemModel FLARES_KIT { get { return this[IDs.FLARES_KIT]; } } //@MP (Release 7-1)
        public ItemModel GLOWSTICKS_BOX { get { return this[IDs.GLOWSTICKS_BOX]; } } //@MP (Release 7-1)
        public ItemModel LIQUOR_AMBER { get { return this[IDs.LIQUOR_AMBER]; } } //@MP (Release 7-1)
        public ItemModel LIQUOR_CLEAR { get { return this[IDs.LIQUOR_CLEAR]; } } //@MP (Release 7-1)
        public ItemModel POLICE_RIOT_SHIELD { get { return this[IDs.POLICE_RIOT_SHIELD]; } } //@MP (Release 7-2)
        public ItemModel SLEEPING_BAG { get { return this[IDs.SLEEPING_BAG]; } } //@MP (Release 7-3)
        public ItemModel FISHING_ROD { get { return this[IDs.FISHING_ROD]; } } //@MP (Release 7-6)
        public ItemModel MATCHES { get { return this[IDs.MATCHES]; } } //@MP (Release 7-6)
        #endregion
        #endregion

        #region Init
        public GameItems()
        {
            // bind
            Models.Items = this;
        }

        #region Grammar Helpers
        static bool StartsWithVowel(string name) //@@MP - made static (Release 5-7)
        {
            return name[0] == 'a' || name[0] == 'A' || 
                name[0] == 'e' || name[0] == 'E' ||
                name[0] == 'i' || name[0] == 'I' ||
                name[0] == 'y' || name[0] == 'Y';
        }

        static bool CheckPlural(string name, string plural) //@@MP - made static (Release 5-7)
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
            this[IDs.MEDICINE_SMALL_MEDIKIT] = new ItemMedicineModel(DATA_MEDICINE_SMALL_MEDIKIT.NAME, DATA_MEDICINE_SMALL_MEDIKIT.PLURAL, GameImages.ITEM_SMALL_MEDIKIT,
                DATA_MEDICINE_SMALL_MEDIKIT.HEALING, DATA_MEDICINE_SMALL_MEDIKIT.STAMINABOOST, DATA_MEDICINE_SMALL_MEDIKIT.SLEEPBOOST, DATA_MEDICINE_SMALL_MEDIKIT.INFECTIONCURE, DATA_MEDICINE_SMALL_MEDIKIT.SANITYCURE)
            {
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_SMALL_MEDIKIT.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_SMALL_MEDIKIT.FLAVOR
            };

            this[IDs.MEDICINE_LARGE_MEDIKIT] = new ItemMedicineModel(DATA_MEDICINE_LARGE_MEDIKIT.NAME, DATA_MEDICINE_LARGE_MEDIKIT.PLURAL, GameImages.ITEM_LARGE_MEDIKIT,
                DATA_MEDICINE_LARGE_MEDIKIT.HEALING, DATA_MEDICINE_LARGE_MEDIKIT.STAMINABOOST, DATA_MEDICINE_LARGE_MEDIKIT.SLEEPBOOST, DATA_MEDICINE_LARGE_MEDIKIT.INFECTIONCURE, DATA_MEDICINE_LARGE_MEDIKIT.SANITYCURE)
            {
                FlavorDescription = DATA_MEDICINE_LARGE_MEDIKIT.FLAVOR
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
                FlavorDescription = DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN.FLAVOR,
                IsRecreational = true //@@MP (Release 5-7)
            };
            this[IDs.MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN] = new ItemMedicineModel(DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.NAME, DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.PLURAL, GameImages.ITEM_BEER_BOTTLE_GREEN,
                DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.HEALING, DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.STAMINABOOST, DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.SLEEPBOOST, DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.INFECTIONCURE, DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.SANITYCURE)
            {
                IsPlural = true,
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN.FLAVOR,
                IsRecreational = true //@@MP (Release 5-7)
            };
            this[IDs.MEDICINE_ALCOHOL_BEER_CAN_BLUE] = new ItemMedicineModel(DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.NAME, DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.PLURAL, GameImages.ITEM_BEER_CAN_BLUE,
                DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.HEALING, DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.STAMINABOOST, DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.SLEEPBOOST, DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.INFECTIONCURE, DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.SANITYCURE)
            {
                IsPlural = true,
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE.FLAVOR,
                IsRecreational = true //@@MP (Release 5-7)
            };
            this[IDs.MEDICINE_ALCOHOL_BEER_CAN_RED] = new ItemMedicineModel(DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.NAME, DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.PLURAL, GameImages.ITEM_BEER_CAN_RED,
                DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.HEALING, DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.STAMINABOOST, DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.SLEEPBOOST, DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.INFECTIONCURE, DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.SANITYCURE)
            {
                IsPlural = true,
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_ALCOHOL_BEER_CAN_RED.FLAVOR,
                IsRecreational = true //@@MP (Release 5-7)
            };
            this[IDs.MEDICINE_CIGARETTES] = new ItemMedicineModel(DATA_MEDICINE_CIGARETTES.NAME, DATA_MEDICINE_CIGARETTES.PLURAL, GameImages.ITEM_CIGARETTES,
                DATA_MEDICINE_CIGARETTES.HEALING, DATA_MEDICINE_CIGARETTES.STAMINABOOST, DATA_MEDICINE_CIGARETTES.SLEEPBOOST, DATA_MEDICINE_CIGARETTES.INFECTIONCURE, DATA_MEDICINE_CIGARETTES.SANITYCURE)
            {
                IsPlural = true,
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_CIGARETTES.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_CIGARETTES.FLAVOR,
                IsRecreational = true //@@MP (Release 5-7)
            };
            //@@MP (Release 7-1)
            this[IDs.MEDICINE_ENERGY_DRINK] = new ItemMedicineModel(DATA_MEDICINE_ENERGY_DRINK.NAME, DATA_MEDICINE_ENERGY_DRINK.PLURAL, GameImages.ITEM_ENERGY_DRINK,
                DATA_MEDICINE_ENERGY_DRINK.HEALING, DATA_MEDICINE_ENERGY_DRINK.STAMINABOOST, DATA_MEDICINE_ENERGY_DRINK.SLEEPBOOST, DATA_MEDICINE_ENERGY_DRINK.INFECTIONCURE, DATA_MEDICINE_ENERGY_DRINK.SANITYCURE)
            {
                IsPlural = true,
                IsStackable = true,
                StackingLimit = DATA_MEDICINE_ENERGY_DRINK.STACKINGLIMIT,
                FlavorDescription = DATA_MEDICINE_ENERGY_DRINK.FLAVOR
            };
            #endregion

            #region Food
            this[IDs.FOOD_ARMY_RATION] = new ItemFoodModel(DATA_FOOD_ARMY_RATION.NAME, DATA_FOOD_ARMY_RATION.PLURAL, GameImages.ITEM_ARMY_RATION, DATA_FOOD_ARMY_RATION.NUTRITION, DATA_FOOD_ARMY_RATION.BESTBEFORE, DATA_FOOD_ARMY_RATION.CANCAUSEFOODPOISONING, DATA_FOOD_ARMY_RATION.CANBECOOKED)
            {
                IsAn = StartsWithVowel(DATA_FOOD_ARMY_RATION.NAME),
                IsPlural = CheckPlural(DATA_FOOD_ARMY_RATION.NAME, DATA_FOOD_ARMY_RATION.PLURAL),
                StackingLimit = DATA_FOOD_ARMY_RATION.STACKINGLIMIT,
                IsStackable = true,
                FlavorDescription = DATA_FOOD_ARMY_RATION.FLAVOR
            };

            this[IDs.FOOD_GROCERIES] = new ItemFoodModel(DATA_FOOD_GROCERIES.NAME, DATA_FOOD_GROCERIES.PLURAL, GameImages.ITEM_GROCERIES, DATA_FOOD_GROCERIES.NUTRITION, DATA_FOOD_GROCERIES.BESTBEFORE, DATA_FOOD_GROCERIES.CANCAUSEFOODPOISONING, DATA_FOOD_GROCERIES.CANBECOOKED)
            {
                IsAn = StartsWithVowel(DATA_FOOD_GROCERIES.NAME),
                IsPlural = CheckPlural(DATA_FOOD_GROCERIES.NAME, DATA_FOOD_GROCERIES.PLURAL),
                StackingLimit = DATA_FOOD_GROCERIES.STACKINGLIMIT,
                FlavorDescription = DATA_FOOD_GROCERIES.FLAVOR
            };

            this[IDs.FOOD_CANNED_FOOD] = new ItemFoodModel(DATA_FOOD_CANNED_FOOD.NAME, DATA_FOOD_CANNED_FOOD.PLURAL, GameImages.ITEM_CANNED_FOOD, DATA_FOOD_CANNED_FOOD.NUTRITION, DATA_FOOD_CANNED_FOOD.BESTBEFORE, DATA_FOOD_CANNED_FOOD.CANCAUSEFOODPOISONING, DATA_FOOD_CANNED_FOOD.CANBECOOKED)
            {
                IsAn = StartsWithVowel(DATA_FOOD_CANNED_FOOD.NAME),
                IsPlural = CheckPlural(DATA_FOOD_CANNED_FOOD.NAME, DATA_FOOD_CANNED_FOOD.PLURAL),
                StackingLimit = DATA_FOOD_CANNED_FOOD.STACKINGLIMIT,
                IsStackable = true,
                FlavorDescription = DATA_FOOD_CANNED_FOOD.FLAVOR
            };

            this[IDs.FOOD_WILD_BERRIES] = new ItemFoodModel(DATA_FOOD_WILD_BERRIES.NAME, DATA_FOOD_WILD_BERRIES.PLURAL, GameImages.ITEM_WILD_BERRIES, DATA_FOOD_WILD_BERRIES.NUTRITION, DATA_FOOD_WILD_BERRIES.BESTBEFORE, DATA_FOOD_WILD_BERRIES.CANCAUSEFOODPOISONING, DATA_FOOD_WILD_BERRIES.CANBECOOKED)
            {  //@MP (Release 4)
                IsAn = StartsWithVowel(DATA_FOOD_WILD_BERRIES.NAME),
                IsPlural = CheckPlural(DATA_FOOD_WILD_BERRIES.NAME, DATA_FOOD_WILD_BERRIES.PLURAL),
                StackingLimit = DATA_FOOD_WILD_BERRIES.STACKINGLIMIT,
                IsStackable = true,
                FlavorDescription = DATA_FOOD_WILD_BERRIES.FLAVOR
            };
            this[IDs.FOOD_VEGETABLES] = new ItemFoodModel(DATA_FOOD_VEGETABLES.NAME, DATA_FOOD_VEGETABLES.PLURAL, GameImages.ITEM_VEGETABLES, DATA_FOOD_VEGETABLES.NUTRITION, DATA_FOOD_VEGETABLES.BESTBEFORE, DATA_FOOD_VEGETABLES.CANCAUSEFOODPOISONING, DATA_FOOD_VEGETABLES.CANBECOOKED)
            { //@MP (Release 5-3), (Release 5-5)
                IsAn = StartsWithVowel(DATA_FOOD_VEGETABLES.NAME),
                IsPlural = CheckPlural(DATA_FOOD_VEGETABLES.NAME, DATA_FOOD_VEGETABLES.PLURAL),
                StackingLimit = DATA_FOOD_VEGETABLES.STACKINGLIMIT,
                IsStackable = true,
                FlavorDescription = DATA_FOOD_VEGETABLES.FLAVOR
            };
            this[IDs.FOOD_SNACK_BAR] = new ItemFoodModel(DATA_FOOD_SNACK_BAR.NAME, DATA_FOOD_SNACK_BAR.PLURAL, GameImages.ITEM_SNACK_BAR, DATA_FOOD_SNACK_BAR.NUTRITION, DATA_FOOD_SNACK_BAR.BESTBEFORE, DATA_FOOD_SNACK_BAR.CANCAUSEFOODPOISONING, DATA_FOOD_SNACK_BAR.CANBECOOKED)
            { //@MP (Release 7-1)
                IsAn = StartsWithVowel(DATA_FOOD_SNACK_BAR.NAME),
                IsPlural = CheckPlural(DATA_FOOD_SNACK_BAR.NAME, DATA_FOOD_SNACK_BAR.PLURAL),
                StackingLimit = DATA_FOOD_SNACK_BAR.STACKINGLIMIT,
                IsStackable = true,
                FlavorDescription = DATA_FOOD_SNACK_BAR.FLAVOR
            };
            this[IDs.FOOD_PEANUTS] = new ItemFoodModel(DATA_FOOD_PEANUTS.NAME, DATA_FOOD_PEANUTS.PLURAL, GameImages.ITEM_PEANUTS, DATA_FOOD_PEANUTS.NUTRITION, DATA_FOOD_PEANUTS.BESTBEFORE, DATA_FOOD_PEANUTS.CANCAUSEFOODPOISONING, DATA_FOOD_PEANUTS.CANBECOOKED)
            {  //@MP (Release 7-3)
                IsAn = StartsWithVowel(DATA_FOOD_PEANUTS.NAME),
                IsPlural = CheckPlural(DATA_FOOD_PEANUTS.NAME, DATA_FOOD_PEANUTS.PLURAL),
                StackingLimit = DATA_FOOD_PEANUTS.STACKINGLIMIT,
                IsStackable = true,
                FlavorDescription = DATA_FOOD_PEANUTS.FLAVOR
            };
            this[IDs.FOOD_GRAPES] = new ItemFoodModel(DATA_FOOD_GRAPES.NAME, DATA_FOOD_GRAPES.PLURAL, GameImages.ITEM_GRAPES, DATA_FOOD_GRAPES.NUTRITION, DATA_FOOD_GRAPES.BESTBEFORE, DATA_FOOD_GRAPES.CANCAUSEFOODPOISONING, DATA_FOOD_GRAPES.CANBECOOKED)
            {  //@MP (Release 7-3)
                IsAn = StartsWithVowel(DATA_FOOD_GRAPES.NAME),
                IsPlural = CheckPlural(DATA_FOOD_GRAPES.NAME, DATA_FOOD_GRAPES.PLURAL),
                StackingLimit = DATA_FOOD_GRAPES.STACKINGLIMIT,
                IsStackable = true,
                FlavorDescription = DATA_FOOD_GRAPES.FLAVOR
            };
            this[IDs.FOOD_RAW_FISH] = new ItemFoodModel(DATA_FOOD_RAW_FISH.NAME, DATA_FOOD_RAW_FISH.PLURAL, GameImages.ITEM_RAW_FISH, DATA_FOOD_RAW_FISH.NUTRITION, DATA_FOOD_RAW_FISH.BESTBEFORE, DATA_FOOD_RAW_FISH.CANCAUSEFOODPOISONING, DATA_FOOD_RAW_FISH.CANBECOOKED)
            {  //@MP (Release 7-6)
                IsAn = StartsWithVowel(DATA_FOOD_RAW_FISH.NAME),
                IsPlural = CheckPlural(DATA_FOOD_RAW_FISH.NAME, DATA_FOOD_RAW_FISH.PLURAL),
                StackingLimit = DATA_FOOD_RAW_FISH.STACKINGLIMIT,
                FlavorDescription = DATA_FOOD_RAW_FISH.FLAVOR
            };
            this[IDs.FOOD_COOKED_FISH] = new ItemFoodModel(DATA_FOOD_COOKED_FISH.NAME, DATA_FOOD_COOKED_FISH.PLURAL, GameImages.ITEM_COOKED_FISH, DATA_FOOD_COOKED_FISH.NUTRITION, DATA_FOOD_COOKED_FISH.BESTBEFORE, DATA_FOOD_COOKED_FISH.CANCAUSEFOODPOISONING, DATA_FOOD_COOKED_FISH.CANBECOOKED)
            {  //@MP (Release 7-6)
                IsAn = StartsWithVowel(DATA_FOOD_COOKED_FISH.NAME),
                IsPlural = CheckPlural(DATA_FOOD_COOKED_FISH.NAME, DATA_FOOD_COOKED_FISH.PLURAL),
                StackingLimit = DATA_FOOD_COOKED_FISH.STACKINGLIMIT,
                FlavorDescription = DATA_FOOD_COOKED_FISH.FLAVOR
            };
            this[IDs.FOOD_RAW_RABBIT] = new ItemFoodModel(DATA_FOOD_RAW_RABBIT.NAME, DATA_FOOD_RAW_RABBIT.PLURAL, GameImages.ITEM_RAW_RABBIT, DATA_FOOD_RAW_RABBIT.NUTRITION, DATA_FOOD_RAW_RABBIT.BESTBEFORE, DATA_FOOD_RAW_RABBIT.CANCAUSEFOODPOISONING, DATA_FOOD_RAW_RABBIT.CANBECOOKED)
            {  //@MP (Release 7-6)
                IsAn = StartsWithVowel(DATA_FOOD_RAW_RABBIT.NAME),
                IsPlural = CheckPlural(DATA_FOOD_RAW_RABBIT.NAME, DATA_FOOD_RAW_RABBIT.PLURAL),
                StackingLimit = DATA_FOOD_RAW_RABBIT.STACKINGLIMIT,
                FlavorDescription = DATA_FOOD_RAW_RABBIT.FLAVOR
            };
            this[IDs.FOOD_COOKED_RABBIT] = new ItemFoodModel(DATA_FOOD_COOKED_RABBIT.NAME, DATA_FOOD_COOKED_RABBIT.PLURAL, GameImages.ITEM_COOKED_RABBIT, DATA_FOOD_COOKED_RABBIT.NUTRITION, DATA_FOOD_COOKED_RABBIT.BESTBEFORE, DATA_FOOD_COOKED_RABBIT.CANCAUSEFOODPOISONING, DATA_FOOD_COOKED_RABBIT.CANBECOOKED)
            {  //@MP (Release 7-6)
                IsAn = StartsWithVowel(DATA_FOOD_COOKED_RABBIT.NAME),
                IsPlural = CheckPlural(DATA_FOOD_COOKED_RABBIT.NAME, DATA_FOOD_COOKED_RABBIT.PLURAL),
                StackingLimit = DATA_FOOD_COOKED_RABBIT.STACKINGLIMIT,
                FlavorDescription = DATA_FOOD_COOKED_RABBIT.FLAVOR
            };
            this[IDs.FOOD_RAW_CHICKEN] = new ItemFoodModel(DATA_FOOD_RAW_CHICKEN.NAME, DATA_FOOD_RAW_CHICKEN.PLURAL, GameImages.ITEM_RAW_CHICKEN, DATA_FOOD_RAW_CHICKEN.NUTRITION, DATA_FOOD_RAW_CHICKEN.BESTBEFORE, DATA_FOOD_RAW_CHICKEN.CANCAUSEFOODPOISONING, DATA_FOOD_RAW_CHICKEN.CANBECOOKED)
            {  //@MP (Release 7-6)
                IsAn = StartsWithVowel(DATA_FOOD_RAW_CHICKEN.NAME),
                IsPlural = CheckPlural(DATA_FOOD_RAW_CHICKEN.NAME, DATA_FOOD_RAW_CHICKEN.PLURAL),
                StackingLimit = DATA_FOOD_RAW_CHICKEN.STACKINGLIMIT,
                FlavorDescription = DATA_FOOD_RAW_CHICKEN.FLAVOR
            };
            this[IDs.FOOD_COOKED_CHICKEN] = new ItemFoodModel(DATA_FOOD_COOKED_CHICKEN.NAME, DATA_FOOD_COOKED_CHICKEN.PLURAL, GameImages.ITEM_COOKED_CHICKEN, DATA_FOOD_COOKED_CHICKEN.NUTRITION, DATA_FOOD_COOKED_CHICKEN.BESTBEFORE, DATA_FOOD_COOKED_CHICKEN.CANCAUSEFOODPOISONING, DATA_FOOD_COOKED_CHICKEN.CANBECOOKED)
            {  //@MP (Release 7-6)
                IsAn = StartsWithVowel(DATA_FOOD_COOKED_CHICKEN.NAME),
                IsPlural = CheckPlural(DATA_FOOD_COOKED_CHICKEN.NAME, DATA_FOOD_COOKED_CHICKEN.PLURAL),
                StackingLimit = DATA_FOOD_COOKED_CHICKEN.STACKINGLIMIT,
                FlavorDescription = DATA_FOOD_COOKED_CHICKEN.FLAVOR
            };
            this[IDs.FOOD_RAW_DOG_MEAT] = new ItemFoodModel(DATA_FOOD_RAW_DOG_MEAT.NAME, DATA_FOOD_RAW_DOG_MEAT.PLURAL, GameImages.ITEM_RAW_DOG_MEAT, DATA_FOOD_RAW_DOG_MEAT.NUTRITION, DATA_FOOD_RAW_DOG_MEAT.BESTBEFORE, DATA_FOOD_RAW_DOG_MEAT.CANCAUSEFOODPOISONING, DATA_FOOD_RAW_DOG_MEAT.CANBECOOKED)
            {  //@MP (Release 7-6)
                IsAn = StartsWithVowel(DATA_FOOD_RAW_DOG_MEAT.NAME),
                IsPlural = CheckPlural(DATA_FOOD_RAW_DOG_MEAT.NAME, DATA_FOOD_RAW_DOG_MEAT.PLURAL),
                StackingLimit = DATA_FOOD_RAW_DOG_MEAT.STACKINGLIMIT,
                FlavorDescription = DATA_FOOD_RAW_DOG_MEAT.FLAVOR
            };
            this[IDs.FOOD_COOKED_DOG_MEAT] = new ItemFoodModel(DATA_FOOD_COOKED_DOG_MEAT.NAME, DATA_FOOD_COOKED_DOG_MEAT.PLURAL, GameImages.ITEM_COOKED_DOG_MEAT, DATA_FOOD_COOKED_DOG_MEAT.NUTRITION, DATA_FOOD_COOKED_DOG_MEAT.BESTBEFORE, DATA_FOOD_COOKED_DOG_MEAT.CANCAUSEFOODPOISONING, DATA_FOOD_COOKED_DOG_MEAT.CANBECOOKED)
            {  //@MP (Release 7-6)
                IsAn = StartsWithVowel(DATA_FOOD_COOKED_DOG_MEAT.NAME),
                IsPlural = CheckPlural(DATA_FOOD_COOKED_DOG_MEAT.NAME, DATA_FOOD_COOKED_DOG_MEAT.PLURAL),
                StackingLimit = DATA_FOOD_COOKED_DOG_MEAT.STACKINGLIMIT,
                FlavorDescription = DATA_FOOD_COOKED_DOG_MEAT.FLAVOR
            };
            this[IDs.FOOD_RAW_HUMAN_FLESH] = new ItemFoodModel(DATA_FOOD_RAW_HUMAN_FLESH.NAME, DATA_FOOD_RAW_HUMAN_FLESH.PLURAL, GameImages.ITEM_RAW_HUMAN_FLESH, DATA_FOOD_RAW_HUMAN_FLESH.NUTRITION, DATA_FOOD_RAW_HUMAN_FLESH.BESTBEFORE, DATA_FOOD_RAW_HUMAN_FLESH.CANCAUSEFOODPOISONING, DATA_FOOD_RAW_HUMAN_FLESH.CANBECOOKED)
            {  //@MP (Release 7-6)
                IsAn = StartsWithVowel(DATA_FOOD_RAW_HUMAN_FLESH.NAME),
                IsPlural = CheckPlural(DATA_FOOD_RAW_HUMAN_FLESH.NAME, DATA_FOOD_RAW_HUMAN_FLESH.PLURAL),
                StackingLimit = DATA_FOOD_RAW_HUMAN_FLESH.STACKINGLIMIT,
                FlavorDescription = DATA_FOOD_RAW_HUMAN_FLESH.FLAVOR
            };
            this[IDs.FOOD_COOKED_HUMAN_FLESH] = new ItemFoodModel(DATA_FOOD_COOKED_HUMAN_FLESH.NAME, DATA_FOOD_COOKED_HUMAN_FLESH.PLURAL, GameImages.ITEM_COOKED_HUMAN_FLESH, DATA_FOOD_COOKED_HUMAN_FLESH.NUTRITION, DATA_FOOD_COOKED_HUMAN_FLESH.BESTBEFORE, DATA_FOOD_COOKED_HUMAN_FLESH.CANCAUSEFOODPOISONING, DATA_FOOD_COOKED_HUMAN_FLESH.CANBECOOKED)
            {  //@MP (Release 7-6)
                IsAn = StartsWithVowel(DATA_FOOD_COOKED_HUMAN_FLESH.NAME),
                IsPlural = CheckPlural(DATA_FOOD_COOKED_HUMAN_FLESH.NAME, DATA_FOOD_COOKED_HUMAN_FLESH.PLURAL),
                StackingLimit = DATA_FOOD_COOKED_HUMAN_FLESH.STACKINGLIMIT,
                FlavorDescription = DATA_FOOD_COOKED_HUMAN_FLESH.FLAVOR
            };
            this[IDs.FOOD_CHICKEN_EGG] = new ItemFoodModel(DATA_FOOD_CHICKEN_EGG.NAME, DATA_FOOD_CHICKEN_EGG.PLURAL, GameImages.ITEM_CHICKEN_EGG, DATA_FOOD_CHICKEN_EGG.NUTRITION, DATA_FOOD_CHICKEN_EGG.BESTBEFORE, DATA_FOOD_CHICKEN_EGG.CANCAUSEFOODPOISONING, DATA_FOOD_CHICKEN_EGG.CANBECOOKED)
            {  //@MP (Release 7-6)
                IsAn = StartsWithVowel(DATA_FOOD_CHICKEN_EGG.NAME),
                IsPlural = CheckPlural(DATA_FOOD_CHICKEN_EGG.NAME, DATA_FOOD_CHICKEN_EGG.PLURAL),
                StackingLimit = DATA_FOOD_CHICKEN_EGG.STACKINGLIMIT,
                FlavorDescription = DATA_FOOD_CHICKEN_EGG.FLAVOR
            };
            #endregion

            #region Melee weapons
            // alpha10 disarm chance added to attack, tool bonuses added to properties
            MeleeWeaponData mwdata;

            mwdata = DATA_MELEE_BASEBALLBAT;
            this[IDs.MELEE_BASEBALLBAT] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_BASEBALL_BAT,
                Attack.MeleeAttack(new Verb("smash", "smashes"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_COMBAT_KNIFE;
            this[IDs.MELEE_COMBAT_KNIFE] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_COMBAT_KNIFE,
                Attack.MeleeAttack(new Verb("stab", "stabs"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = true,
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true, //@@MP (Release 7-2)
                CanUseForButchering = true //@@MP (Release 7-6)
            };

            mwdata = DATA_MELEE_CROWBAR;
            this[IDs.MELEE_CROWBAR] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_CROWBAR,
                Attack.MeleeAttack(new Verb("strike"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = true,
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_UNIQUE_JASON_MYERS_AXE;
            this[IDs.UNIQUE_JASON_MYERS_AXE] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_JASON_MYERS_AXE,
                Attack.MeleeAttack(new Verb("chop"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsProper = true,
                FlavorDescription = mwdata.FLAVOR,
                IsUnbreakable = true,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false, //@@MP (Release 7-2)
                CanUseForButchering = true, //@@MP (Release 7-6)
                CanCutDownTrees = true //@@MP (Release 7-6)
            };

            mwdata = DATA_MELEE_GOLFCLUB;
            this[IDs.MELEE_GOLFCLUB] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_GOLF_CLUB,
                Attack.MeleeAttack(new Verb("strike"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_IRON_GOLFCLUB;
            this[IDs.MELEE_IRON_GOLFCLUB] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_IRON_GOLF_CLUB,
                Attack.MeleeAttack(new Verb("strike"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_HUGE_HAMMER;
            this[IDs.MELEE_HUGE_HAMMER] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_HUGE_HAMMER,
                Attack.MeleeAttack(new Verb("smash", "smashes"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_SHOVEL;
            this[IDs.MELEE_SHOVEL] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_SHOVEL,
                Attack.MeleeAttack(new Verb("strike"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false, //@@MP (Release 7-2)
                CanUseForDigging = true, //@@MP (Release 7-6)
            };

            mwdata = DATA_MELEE_SHORT_SHOVEL;
            this[IDs.MELEE_SHORT_SHOVEL] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_SHORT_SHOVEL,
                 Attack.MeleeAttack(new Verb("strike"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true, //@@MP (Release 7-2)
                CanUseForDigging = true, //@@MP (Release 7-6)
            };

            mwdata = DATA_MELEE_TRUNCHEON;
            this[IDs.MELEE_TRUNCHEON] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_TRUNCHEON,
                Attack.MeleeAttack(new Verb("strike"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = true,
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_IMPROVISED_CLUB;
            this[IDs.MELEE_IMPROVISED_CLUB] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_IMPROVISED_CLUB,
                Attack.MeleeAttack(new Verb("strike"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_IMPROVISED_SPEAR;
            this[IDs.MELEE_IMPROVISED_SPEAR] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_IMPROVISED_SPEAR,
                Attack.MeleeAttack(new Verb("pierce"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_SMALL_HAMMER;
            this[IDs.MELEE_SMALL_HAMMER] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_SMALL_HAMMER,
                Attack.MeleeAttack(new Verb("smash"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_UNIQUE_FAMU_FATARU_KATANA;
            this[IDs.UNIQUE_FAMU_FATARU_KATANA] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_FAMU_FATARU_KATANA,
                Attack.MeleeAttack(new Verb("slash", "slashes"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwdata.FLAVOR,
                IsProper = true,
                IsUnbreakable = true,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false, //@@MP (Release 7-2)
                CanUseForButchering = true //@@MP (Release 7-6)
            };

            mwdata = DATA_MELEE_UNIQUE_BIGBEAR_BAT;
            this[IDs.UNIQUE_BIGBEAR_BAT] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_BIGBEAR_BAT,
                Attack.MeleeAttack(new Verb("smash", "smashes"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwdata.FLAVOR,
                IsProper = true,
                IsUnbreakable = true,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_UNIQUE_ROGUEDJACK_KEYBOARD;
            this[IDs.UNIQUE_ROGUEDJACK_KEYBOARD] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_ROGUEDJACK_KEYBOARD,
                Attack.MeleeAttack(new Verb("bash", "bashes"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwdata.FLAVOR,
                IsProper = true,
                IsUnbreakable = true,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false //@@MP (Release 7-2)
            };

            //@@MP (Release 3)
            mwdata = DATA_MELEE_TENNIS_RACKET;
            this[IDs.MELEE_TENNIS_RACKET] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_TENNIS_RACKET,
                Attack.MeleeAttack(new Verb("bash", "bashes"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwdata.FLAVOR,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_HOCKEY_STICK;
            this[IDs.MELEE_HOCKEY_STICK] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_HOCKEY_STICK,
                Attack.MeleeAttack(new Verb("bash", "bashes"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwdata.FLAVOR,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_MACHETE;
            this[IDs.MELEE_MACHETE] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_MACHETE,
                Attack.MeleeAttack(new Verb("slash", "slashes"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwdata.FLAVOR,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true, //@@MP (Release 7-2)
                CanUseForButchering = true, //@@MP (Release 7-6)
                CanCutDownTrees = true //@@MP (Release 7-6)
            };

            mwdata = DATA_MELEE_STANDARD_AXE;
            this[IDs.MELEE_STANDARD_AXE] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_STANDARD_AXE,
                Attack.MeleeAttack(new Verb("chop"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwdata.FLAVOR,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false, //@@MP (Release 7-2)
                CanUseForButchering = true, //@@MP (Release 7-6)
                CanCutDownTrees = true //@@MP (Release 7-6)
            };

            mwdata = DATA_MELEE_PICKAXE;
            this[IDs.MELEE_PICKAXE] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_PICKAXE,
                Attack.MeleeAttack(new Verb("strike"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwdata.FLAVOR,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false, //@@MP (Release 7-2)
                CanUseForDigging = true, //@@MP (Release 7-6)
            };

            mwdata = DATA_MELEE_PIPE_WRENCH;
            this[IDs.MELEE_PIPE_WRENCH] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_PIPE_WRENCH,
                Attack.MeleeAttack(new Verb("bash", "bashes"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwdata.FLAVOR,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true //@@MP (Release 7-2)
            };

            //@@MP (Release 7-1)
            mwdata = DATA_MELEE_CHAINSAW;
            this[IDs.MELEE_CHAINSAW] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_CHAINSAW,
                Attack.MeleeAttack(new Verb("cut", "cuts"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwdata.FLAVOR,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false, //@@MP (Release 7-2)
                CanUseForButchering = true, //@@MP (Release 7-6)
                CanCutDownTrees = true //@@MP (Release 7-6)
            };

            //@@MP (Release 7-6)
            mwdata = DATA_MELEE_CLEAVER;
            this[IDs.MELEE_CLEAVER] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_CLEAVER,
                Attack.MeleeAttack(new Verb("chop"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true, //@@MP (Release 7-2)
                CanUseForButchering = true //@@MP (Release 7-6)
            };

            mwdata = DATA_MELEE_BRASS_KNUCKLES;
            this[IDs.MELEE_BRASS_KNUCKLES] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_BRASS_KNUCKLES,
                Attack.MeleeAttack(new Verb("strike"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_FLAIL;
            this[IDs.MELEE_FLAIL] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_FLAIL,
                Attack.MeleeAttack(new Verb("strike"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_KITCHEN_KNIFE;
            this[IDs.MELEE_KITCHEN_KNIFE] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_KITCHEN_KNIFE,
                Attack.MeleeAttack(new Verb("slash","slashes"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true, //@@MP (Release 7-2)
                CanUseForButchering = true //@@MP (Release 7-6)
            };

            mwdata = DATA_MELEE_SCIMITAR;
            this[IDs.MELEE_SCIMITAR] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_SCIMITAR,
                Attack.MeleeAttack(new Verb("slash","slashes"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true, //@@MP (Release 7-2)
                CanUseForButchering = true, //@@MP (Release 7-6)
                CanCutDownTrees = true //@@MP (Release 7-6)
            };

            mwdata = DATA_MELEE_MACE;
            this[IDs.MELEE_MACE] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_MACE,
                Attack.MeleeAttack(new Verb("smash","smashes"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_NUNCHAKU;
            this[IDs.MELEE_NUNCHAKU] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_NUNCHAKU,
                Attack.MeleeAttack(new Verb("strike"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_FRYING_PAN;
            this[IDs.MELEE_FRYING_PAN] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_FRYING_PAN,
                Attack.MeleeAttack(new Verb("bash","bashes"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_PITCH_FORK;
            this[IDs.MELEE_PITCH_FORK] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_PITCH_FORK,
                Attack.MeleeAttack(new Verb("pierce"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_SCYTHE;
            this[IDs.MELEE_SCYTHE] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_SCYTHE,
                Attack.MeleeAttack(new Verb("slash","slashes"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false, //@@MP (Release 7-2)
                CanUseForButchering = true //@@MP (Release 7-6)
            };

            mwdata = DATA_MELEE_SICKLE;
            this[IDs.MELEE_SICKLE] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_SICKLE,
                Attack.MeleeAttack(new Verb("slash", "slashes"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true, //@@MP (Release 7-2)
                CanUseForButchering = true //@@MP (Release 7-6)
            };

            mwdata = DATA_MELEE_SPEAR;
            this[IDs.MELEE_SPEAR] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_SPEAR,
                Attack.MeleeAttack(new Verb("pierce"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_SPIKED_MACE;
            this[IDs.MELEE_SPIKED_MACE] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_SPIKED_MACE,
                Attack.MeleeAttack(new Verb("smash","smashes"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                FlavorDescription = mwdata.FLAVOR,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = true //@@MP (Release 7-2)
            };

            mwdata = DATA_MELEE_FIRE_AXE;
            this[IDs.MELEE_FIRE_AXE] = new ItemMeleeWeaponModel(mwdata.NAME, mwdata.PLURAL, GameImages.ITEM_FIRE_AXE,
                Attack.MeleeAttack(new Verb("chop"), mwdata.ATK, mwdata.DMG, mwdata.STA, mwdata.DISARM))
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = mwdata.FLAVOR,
                IsStackable = (mwdata.STACKINGLIMIT > 1),
                StackingLimit = mwdata.STACKINGLIMIT,
                IsFragile = mwdata.ISFRAGILE,
                ToolBashDamageBonus = mwdata.TOOLBASHDMGBONUS, // alpha10
                ToolBuildBonus = mwdata.TOOLBUILDBONUS,  // alpha10
                Weight = mwdata.WEIGHT, //@@MP (Release 7-6)
                IsOneHanded = false, //@@MP (Release 7-2)
                CanUseForButchering = true, //@@MP (Release 7-6)
                CanCutDownTrees = true //@@MP (Release 7-6)
            };
            #endregion

            #region Ranged weapons
            // alpha10 added rapid fire properties
            //@@MP - added isSingleShot property (Release 6-6)
            RangedWeaponData rwp;

            rwp = DATA_RANGED_ARMY_PISTOL;
            this[IDs.RANGED_ARMY_PISTOL] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_ARMY_PISTOL,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.HEAVY_PISTOL, false, true, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
                IsAn = true
            };

            rwp = DATA_RANGED_ARMY_PRECISION_RIFLE;  //@@MP (Release 7-6)
            this[IDs.RANGED_ARMY_PRECISION_RIFLE] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_ARMY_PRECISION_RIFLE,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.PRECISION_RIFLE, true, false, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
                IsAn = true
            };

            rwp = DATA_RANGED_ARMY_RIFLE1;
            this[IDs.RANGED_ARMY_RIFLE1] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_ARMY_RIFLE1,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                     rwp.MAXAMMO, AmmoType.HEAVY_RIFLE, false, false, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
                IsAn = true
            };

            rwp = DATA_RANGED_HUNTING_CROSSBOW;
            this[IDs.RANGED_HUNTING_CROSSBOW] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_HUNTING_CROSSBOW,
                Attack.RangedAttack(AttackKind.BOW, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.BOLT, true, false, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR
            };

            rwp = DATA_RANGED_HUNTING_RIFLE;
            this[IDs.RANGED_HUNTING_RIFLE] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_HUNTING_RIFLE,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.LIGHT_RIFLE, true, false, rwp.WEIGHT)
                {
                    EquipmentPart = DollPart.RIGHT_HAND,
                    FlavorDescription = rwp.FLAVOR
                };

            rwp = DATA_RANGED_PISTOL;
            this[IDs.RANGED_PISTOL] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_PISTOL,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.LIGHT_PISTOL, false, true, rwp.WEIGHT)
                {
                    EquipmentPart = DollPart.RIGHT_HAND,
                    FlavorDescription =rwp.FLAVOR
                };

            rwp = DATA_RANGED_REVOLVER;
            this[IDs.RANGED_REVOLVER] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_REVOLVER,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.LIGHT_PISTOL, false, true, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR
            };

            rwp = DATA_RANGED_PRECISION_RIFLE;
            this[IDs.RANGED_PRECISION_RIFLE] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_PRECISION_RIFLE,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.PRECISION_RIFLE, true, false, rwp.WEIGHT) //@@MP - new ammo type for precision rifles (Release 6-6)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR
            };

            rwp = DATA_RANGED_SHOTGUN;
            this[IDs.RANGED_SHOTGUN] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_SHOTGUN,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.SHOTGUN, true, false, rwp.WEIGHT)
                {
                    EquipmentPart = DollPart.RIGHT_HAND,
                    FlavorDescription = rwp.FLAVOR
                };

            rwp = DATA_UNIQUE_SANTAMAN_SHOTGUN;
            this[IDs.UNIQUE_SANTAMAN_SHOTGUN] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_SANTAMAN_SHOTGUN,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.SHOTGUN, true, false, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
                IsProper = true,
                IsUnbreakable = true
            };

            rwp = DATA_UNIQUE_HANS_VON_HANZ_PISTOL;
            this[IDs.UNIQUE_HANS_VON_HANZ_PISTOL] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_HANS_VON_HANZ_PISTOL,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.LIGHT_PISTOL, false, true, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
                IsProper = true,
                IsUnbreakable = true
            };

            rwp = DATA_RANGED_NAIL_GUN; //@@MP (Release 5-1)
            this[IDs.RANGED_NAIL_GUN] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_NAIL_GUN,
                Attack.RangedAttack(AttackKind.OTHER, new Verb("nail"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE), //@@MP - was AttackKind.FIREARM (Release 6-6)
                    rwp.MAXAMMO, AmmoType.NAIL, true, true, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR
            };

            rwp = DATA_RANGED_FLAMETHROWER; //@@MP (Release 7-2)
            this[IDs.RANGED_FLAMETHROWER] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_FLAMETHROWER,
                Attack.RangedAttack(AttackKind.OTHER, new Verb("burn"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.FUEL, true, false, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
                IsFlameWeapon = true
            };

            rwp = DATA_RANGED_STUN_GUN; //@@MP (Release 7-2)
            this[IDs.RANGED_STUN_GUN] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_STUN_GUN,
                Attack.RangedAttack(AttackKind.OTHER, new Verb("paralyze"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.CHARGE, true, true, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
                IsBatteryPowered = true
            };

            //@@MP (Release 7-6)
            rwp = DATA_RANGED_SMG;
            this[IDs.RANGED_SMG] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_SMG,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.LIGHT_PISTOL, false, true, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
                IsAn = true
            };

            rwp = DATA_RANGED_DOUBLE_BARREL;
            this[IDs.RANGED_DOUBLE_BARREL] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_DOUBLE_BARREL,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.SHOTGUN, true, false, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR
            };

            rwp = DATA_RANGED_MINIGUN;
            this[IDs.RANGED_MINIGUN] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_MINIGUN,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.MINIGUN, false, false, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
                IsUnbreakable = true
            };

            rwp = DATA_RANGED_TACTICAL_SHOTGUN;
            this[IDs.RANGED_TACTICAL_SHOTGUN] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_TACTICAL_SHOTGUN,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.SHOTGUN, true, false, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR
            };

            rwp = DATA_RANGED_ARMY_RIFLE2;
            this[IDs.RANGED_ARMY_RIFLE2] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_ARMY_RIFLE2,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.HEAVY_RIFLE, false, false, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
                IsAn = true
            };

            rwp = DATA_RANGED_ARMY_RIFLE3;
            this[IDs.RANGED_ARMY_RIFLE3] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_ARMY_RIFLE3,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.HEAVY_RIFLE, false, false, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
                IsAn = true
            };

            rwp = DATA_RANGED_ARMY_RIFLE4;
            this[IDs.RANGED_ARMY_RIFLE4] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_ARMY_RIFLE4,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("shoot"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.HEAVY_RIFLE, false, false, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
                IsAn = true
            };

            rwp = DATA_RANGED_GRENADE_LAUNCHER;
            this[IDs.RANGED_GRENADE_LAUNCHER] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_GRENADE_LAUNCHER,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("lob a grenade at","lobs a grenade at"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.GRENADES, true, false, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
                IsUnbreakable = true
            };

            rwp = DATA_RANGED_BIO_FORCE_GUN;
            this[IDs.RANGED_BIO_FORCE_GUN] = new ItemRangedWeaponModel(rwp.NAME, rwp.FLAVOR, GameImages.ITEM_BIO_FORCE_GUN,
                Attack.RangedAttack(AttackKind.FIREARM, new Verb("disintegrate", "disintegrates"), rwp.ATK, rwp.RAPID1, rwp.RAPID2, rwp.DMG, rwp.RANGE),
                    rwp.MAXAMMO, AmmoType.PLASMA, true, false, rwp.WEIGHT)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                FlavorDescription = rwp.FLAVOR,
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
                AmmoType.BOLT, 40)
            {
                IsPlural = true,
                FlavorDescription = ""
            };

            //@@MP (Release 5-1)
            this[IDs.AMMO_NAILS] = new ItemAmmoModel("nails", "nails", GameImages.ITEM_AMMO_NAILS,
                AmmoType.NAIL, 99)
            {
                IsPlural = true,
                FlavorDescription = ""
            };

            //@@MP (Release 6-6)
            this[IDs.AMMO_PRECISION_RIFLE] = new ItemAmmoModel("precision rifle rounds", "precision rifle rounds", GameImages.ITEM_AMMO_PRECISION_RIFLE,
                AmmoType.PRECISION_RIFLE, 20)
            {
                IsPlural = true,
                FlavorDescription = ""
            };

            //@@MP (Release 7-1)
            this[IDs.AMMO_FUEL] = new ItemAmmoModel("fuel", "fuel", GameImages.ITEM_AMMO_FUEL,
                AmmoType.FUEL, 20)
            {
                IsPlural = true,
                FlavorDescription = ""
            };

            //@@MP (Release 7-6)
            this[IDs.AMMO_MINIGUN] = new ItemAmmoModel("minigun rounds", "minigun rounds", GameImages.ITEM_AMMO_MINIGUN,
                AmmoType.MINIGUN, 96)
            {
                IsPlural = true,
                FlavorDescription = ""
            };

            this[IDs.AMMO_GRENADES] = new ItemAmmoModel("launcher grenades", "launcher grenades", GameImages.ITEM_AMMO_GRENADES,
                AmmoType.GRENADES, 10)
            {
                IsPlural = true,
                FlavorDescription = ""
            };

            this[IDs.AMMO_PLASMA] = new ItemAmmoModel("bio force plasma", "bio force plasma", GameImages.ITEM_AMMO_PLASMA,
                AmmoType.PLASMA, 5)
            {
                IsPlural = true,
                FlavorDescription = "Warning: fire with caution. Wide discharge radius."
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
                exData.FUSE, new BlastAttack(exData.RADIUS, exArray, true, false, true), GameImages.ICON_BLAST, exData.MAXTHROW)
                {
                    EquipmentPart = DollPart.RIGHT_HAND,
                    IsStackable = true,
                    StackingLimit = exData.STACKLINGLIMIT,
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
                exData.FUSE, new BlastAttack(exData.RADIUS, exArray, false, false, true), GameImages.ICON_BLAST, exData.MAXTHROW)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = true,
                StackingLimit = exData.STACKLINGLIMIT,
                FlavorDescription = exData.FLAVOR,
                IsFlameWeapon = true, //@@MP - moved from GameItems.cs where it was hard-coded (Release 5-7)
                CausesTileFires = true //@@MP (Release 7-6)
            };

            this[IDs.EXPLOSIVE_MOLOTOV_PRIMED] = new ItemGrenadePrimedModel("primed " + exData.NAME, "primed " + exData.PLURAL, GameImages.ITEM_MOLOTOV_PRIMED, this[IDs.EXPLOSIVE_MOLOTOV] as ItemGrenadeModel)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsFlameWeapon = true, //@@MP - moved from GameItems.cs where it was hard-coded (Release 5-7)
                CausesTileFires = true //@@MP (Release 7-6)
            };

            //@@MP - DYNAMITE (Release 4)
            exData = DATA_EXPLOSIVE_DYNAMITE;
            exArray = new int[exData.RADIUS + 1];
            for (int i = 0; i < exData.RADIUS + 1; i++)
                exArray[i] = exData.DMG[i];
            this[IDs.EXPLOSIVE_DYNAMITE] = new ItemGrenadeModel(exData.NAME, exData.PLURAL, GameImages.ITEM_DYNAMITE,
                exData.FUSE, new BlastAttack(exData.RADIUS, exArray, true, true, true), GameImages.ICON_BLAST, exData.MAXTHROW)
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

            //@@MP - C4 (Release 6-3)
            exData = DATA_EXPLOSIVE_C4;
            exArray = new int[exData.RADIUS + 1];
            for (int i = 0; i < exData.RADIUS + 1; i++)
                exArray[i] = exData.DMG[i];
            this[IDs.EXPLOSIVE_C4] = new ItemGrenadeModel(exData.NAME, exData.PLURAL, GameImages.ITEM_C4,
                exData.FUSE, new BlastAttack(exData.RADIUS, exArray, true, true, true), GameImages.ICON_BLAST, exData.MAXTHROW)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = true,
                StackingLimit = exData.STACKLINGLIMIT,
                FlavorDescription = exData.FLAVOR
            };

            this[IDs.EXPLOSIVE_C4_PRIMED] = new ItemGrenadePrimedModel("primed " + exData.NAME, "primed " + exData.PLURAL, GameImages.ITEM_C4_PRIMED, this[IDs.EXPLOSIVE_C4] as ItemGrenadeModel)
            {
                EquipmentPart = DollPart.RIGHT_HAND
            };

            //@@MP - FUEL CANS (Release 7-1)
            exData = DATA_EXPLOSIVE_FUEL_CAN;
            exArray = new int[exData.RADIUS + 1];
            for (int i = 0; i < exData.RADIUS + 1; i++)
                exArray[i] = exData.DMG[i];
            this[IDs.EXPLOSIVE_FUEL_CAN] = new ItemGrenadeModel(exData.NAME, exData.PLURAL, GameImages.ITEM_AMMO_FUEL,
                exData.FUSE, new BlastAttack(exData.RADIUS, exArray, true, false, true), GameImages.ICON_BLAST, exData.MAXTHROW)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = true,
                StackingLimit = exData.STACKLINGLIMIT,
                FlavorDescription = exData.FLAVOR,
                IsFlameWeapon = true,
                CausesTileFires = true //@@MP (Release 7-6)
            };

            this[IDs.EXPLOSIVE_FUEL_CAN_PRIMED] = new ItemGrenadePrimedModel("primed " + exData.NAME, "primed " + exData.PLURAL, GameImages.ITEM_AMMO_FUEL, this[IDs.EXPLOSIVE_FUEL_CAN] as ItemGrenadeModel)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsFlameWeapon = true,
                CausesTileFires = true //@@MP (Release 7-6)
            };

            //@@MP - FUEL PUMPS (trickery in code) (Release 7-3)
            exData = DATA_EXPLOSIVE_FUEL_PUMP;
            exArray = new int[exData.RADIUS + 1];
            for (int i = 0; i < exData.RADIUS + 1; i++)
                exArray[i] = exData.DMG[i];
            this[IDs.EXPLOSIVE_FUEL_PUMP] = new ItemGrenadeModel(exData.NAME, exData.PLURAL, GameImages.OBJ_FUEL_PUMP,
                exData.FUSE, new BlastAttack(exData.RADIUS, exArray, true, true, false), GameImages.ICON_BLAST, exData.MAXTHROW)
            {
                EquipmentPart = DollPart.NONE,
                IsStackable = true,
                StackingLimit = exData.STACKLINGLIMIT,
                FlavorDescription = exData.FLAVOR,
                CausesTileFires = true
            };

            this[IDs.EXPLOSIVE_FUEL_PUMP_PRIMED] = new ItemGrenadePrimedModel("primed " + exData.NAME, "primed " + exData.PLURAL, GameImages.OBJ_FUEL_PUMP, this[IDs.EXPLOSIVE_FUEL_PUMP] as ItemGrenadeModel)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                CausesTileFires = true
            };

            //@@MP - SMOKE GRENADE (Release 7-2)
            exData = DATA_EXPLOSIVE_SMOKE_GRENADE;
            exArray = new int[exData.RADIUS + 1];
            for (int i = 0; i < exData.RADIUS + 1; i++)
                exArray[i] = exData.DMG[i];
            this[IDs.EXPLOSIVE_SMOKE_GRENADE] = new ItemGrenadeModel(exData.NAME, exData.PLURAL, GameImages.ITEM_SMOKE_GRENADE,
                exData.FUSE, new BlastAttack(exData.RADIUS, exArray, false, false, false), GameImages.ICON_BLAST, exData.MAXTHROW)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = true,
                StackingLimit = exData.STACKLINGLIMIT,
                FlavorDescription = exData.FLAVOR
            };

            this[IDs.EXPLOSIVE_SMOKE_GRENADE_PRIMED] = new ItemGrenadePrimedModel("primed " + exData.NAME, "primed " + exData.PLURAL, GameImages.ITEM_SMOKE_GRENADE_PRIMED, this[IDs.EXPLOSIVE_SMOKE_GRENADE] as ItemGrenadeModel)
            {
                EquipmentPart = DollPart.RIGHT_HAND
            };

            //@@MP - FLASHBANG (Release 7-2)
            exData = DATA_EXPLOSIVE_FLASHBANG;
            exArray = new int[exData.RADIUS + 1];
            for (int i = 0; i < exData.RADIUS + 1; i++)
                exArray[i] = exData.DMG[i];
            this[IDs.EXPLOSIVE_FLASHBANG] = new ItemGrenadeModel(exData.NAME, exData.PLURAL, GameImages.ITEM_FLASHBANG,
                exData.FUSE, new BlastAttack(exData.RADIUS, exArray, false, false, true), GameImages.ICON_BLAST, exData.MAXTHROW)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = true,
                StackingLimit = exData.STACKLINGLIMIT,
                FlavorDescription = exData.FLAVOR
            };

            this[IDs.EXPLOSIVE_FLASHBANG_PRIMED] = new ItemGrenadePrimedModel("primed " + exData.NAME, "primed " + exData.PLURAL, GameImages.ITEM_FLASHBANG_PRIMED, this[IDs.EXPLOSIVE_FLASHBANG] as ItemGrenadeModel)
            {
                EquipmentPart = DollPart.RIGHT_HAND
            };

            //@@MP - HOLY HAND GRENADE OF ANTIOCH (Release 7-6)
            exData = DATA_EXPLOSIVE_HOLY_HAND_GRENADE;
            exArray = new int[exData.RADIUS + 1];
            for (int i = 0; i < exData.RADIUS + 1; i++)
                exArray[i] = exData.DMG[i];
            this[IDs.EXPLOSIVE_HOLY_HAND_GRENADE] = new ItemGrenadeModel(exData.NAME, exData.PLURAL, GameImages.ITEM_HOLY_HAND_GRENADE,
                exData.FUSE, new BlastAttack(exData.RADIUS, exArray, true, false, true), GameImages.ICON_BLAST, exData.MAXTHROW)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = true,
                StackingLimit = exData.STACKLINGLIMIT,
                FlavorDescription = exData.FLAVOR
            };

            this[IDs.EXPLOSIVE_HOLY_HAND_GRENADE_PRIMED] = new ItemGrenadePrimedModel("primed " + exData.NAME, "primed " + exData.PLURAL, GameImages.ITEM_HOLY_HAND_GRENADE_PRIMED, this[IDs.EXPLOSIVE_HOLY_HAND_GRENADE] as ItemGrenadeModel)
            {
                EquipmentPart = DollPart.RIGHT_HAND
            };

            //@@MP - PLASMA CHARGE (Release 7-6)
            exData = DATA_EXPLOSIVE_PLASMA_CHARGE; //not actually a grenade. it's for in-code use only, as the burst caused by the bio force gun. that's also why it has no pre-primed image
            exArray = new int[exData.RADIUS + 1];
            for (int i = 0; i < exData.RADIUS + 1; i++)
                exArray[i] = exData.DMG[i];
            this[IDs.EXPLOSIVE_PLASMA_CHARGE] = new ItemGrenadeModel(exData.NAME, exData.PLURAL, GameImages.ITEM_PLASMA_BURST_PRIMED,
                exData.FUSE, new BlastAttack(exData.RADIUS, exArray, false, false, true), GameImages.ICON_PLASMA_BURST, exData.MAXTHROW)
            {
                EquipmentPart = DollPart.RIGHT_HAND,
                IsStackable = true,
                StackingLimit = exData.STACKLINGLIMIT,
                FlavorDescription = exData.FLAVOR
            };

            this[IDs.EXPLOSIVE_PLASMA_CHARGE_PRIMED] = new ItemGrenadePrimedModel("primed " + exData.NAME, "primed " + exData.PLURAL, GameImages.ITEM_PLASMA_BURST_PRIMED, this[IDs.EXPLOSIVE_PLASMA_CHARGE] as ItemGrenadeModel)
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
            this[IDs.ARMOR_ARMY_BODYARMOR] = new ItemBodyArmorModel(armData.NAME, armData.PLURAL, GameImages.ITEM_ARMY_BODYARMOR, armData.PRO_HIT, armData.PRO_SHOT, armData.ENC, armData.WEIGHT, armData.FIRE_RESIST, armData.INFECTION_RESIST)
            {
                EquipmentPart = DollPart.TORSO,
                FlavorDescription = armData.FLAVOR,
                IsAn = StartsWithVowel(armData.NAME)
            };

            armData = DATA_ARMOR_CHAR;
            this[IDs.ARMOR_CHAR_LIGHT_BODYARMOR] = new ItemBodyArmorModel(armData.NAME, armData.PLURAL, GameImages.ITEM_CHAR_LIGHT_BODYARMOR, armData.PRO_HIT, armData.PRO_SHOT, armData.ENC, armData.WEIGHT, armData.FIRE_RESIST, armData.INFECTION_RESIST)
            {
                EquipmentPart = DollPart.TORSO,
                FlavorDescription = armData.FLAVOR,
                IsAn = StartsWithVowel(armData.NAME)
            };

            armData = DATA_ARMOR_HELLS_SOULS_JACKET;
            this[IDs.ARMOR_HELLS_SOULS_JACKET] = new ItemBodyArmorModel(armData.NAME, armData.PLURAL, GameImages.ITEM_HELLS_SOULS_JACKET, armData.PRO_HIT, armData.PRO_SHOT, armData.ENC, armData.WEIGHT, armData.FIRE_RESIST, armData.INFECTION_RESIST)
            {
                EquipmentPart = DollPart.TORSO,
                FlavorDescription = armData.FLAVOR,
                IsAn = StartsWithVowel(armData.NAME)
            };

            armData = DATA_ARMOR_FREE_ANGELS_JACKET;
            this[IDs.ARMOR_FREE_ANGELS_JACKET] = new ItemBodyArmorModel(armData.NAME, armData.PLURAL, GameImages.ITEM_FREE_ANGELS_JACKET, armData.PRO_HIT, armData.PRO_SHOT, armData.ENC, armData.WEIGHT, armData.FIRE_RESIST, armData.INFECTION_RESIST)
            {
                EquipmentPart = DollPart.TORSO,
                FlavorDescription = armData.FLAVOR,
                IsAn = StartsWithVowel(armData.NAME)
            };

            armData = DATA_ARMOR_POLICE_JACKET;
            this[IDs.ARMOR_POLICE_JACKET] = new ItemBodyArmorModel(armData.NAME, armData.PLURAL, GameImages.ITEM_POLICE_JACKET, armData.PRO_HIT, armData.PRO_SHOT, armData.ENC, armData.WEIGHT, armData.FIRE_RESIST, armData.INFECTION_RESIST)
            {
                EquipmentPart = DollPart.TORSO,
                FlavorDescription = armData.FLAVOR,
                IsAn = StartsWithVowel(armData.NAME)
            };

            armData = DATA_ARMOR_POLICE_RIOT;
            this[IDs.ARMOR_POLICE_RIOT] = new ItemBodyArmorModel(armData.NAME, armData.PLURAL, GameImages.ITEM_POLICE_RIOT_ARMOR, armData.PRO_HIT, armData.PRO_SHOT, armData.ENC, armData.WEIGHT, armData.FIRE_RESIST, armData.INFECTION_RESIST)
            {
                EquipmentPart = DollPart.TORSO,
                FlavorDescription = armData.FLAVOR,
                IsAn = StartsWithVowel(armData.NAME)
            };

            armData = DATA_ARMOR_HUNTER_VEST;
            this[IDs.ARMOR_HUNTER_VEST] = new ItemBodyArmorModel(armData.NAME, armData.PLURAL, GameImages.ITEM_HUNTER_VEST, armData.PRO_HIT, armData.PRO_SHOT, armData.ENC, armData.WEIGHT, armData.FIRE_RESIST, armData.INFECTION_RESIST)
            {
                EquipmentPart = DollPart.TORSO,
                FlavorDescription = armData.FLAVOR,
                IsAn = StartsWithVowel(armData.NAME)
            };

            armData = DATA_ARMOR_FIRE_HAZARD_SUIT; //@@MP (Release 7-1)
            this[IDs.ARMOR_FIRE_HAZARD_SUIT] = new ItemBodyArmorModel(armData.NAME, armData.PLURAL, GameImages.ITEM_FIRE_HAZARD_SUIT, armData.PRO_HIT, armData.PRO_SHOT, armData.ENC, armData.WEIGHT, armData.FIRE_RESIST, armData.INFECTION_RESIST)
            {
                EquipmentPart = DollPart.TORSO,
                FlavorDescription = armData.FLAVOR,
                IsAn = StartsWithVowel(armData.NAME)
            };

            armData = DATA_ARMOR_BIOHAZARD_SUIT; //@@MP (Release 7-6)
            this[IDs.ARMOR_BIOHAZARD_SUIT] = new ItemBodyArmorModel(armData.NAME, armData.PLURAL, GameImages.ITEM_BIOHAZARD_SUIT, armData.PRO_HIT, armData.PRO_SHOT, armData.ENC, armData.WEIGHT, armData.FIRE_RESIST, armData.INFECTION_RESIST)
            {
                EquipmentPart = DollPart.TORSO,
                FlavorDescription = armData.FLAVOR,
                IsAn = StartsWithVowel(armData.NAME)
            };
            #endregion

            #region Trackers
            // alpha10 added clock prop to trackers
            TrackerData traData;

            traData = DATA_TRACKER_CELL_PHONE;
            this[IDs.TRACKER_CELL_PHONE] = new ItemTrackerModel(traData.NAME, traData.PLURAL, GameImages.ITEM_CELL_PHONE,
                ItemTrackerModel.TrackingFlags.FOLLOWER_AND_LEADER,
                traData.BATTERIES * WorldTime.TURNS_PER_HOUR,
                traData.HASCLOCK)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = traData.FLAVOR,
                IsBatteryPowered = true
            };

            traData = DATA_TRACKER_ZTRACKER;
            this[IDs.TRACKER_ZTRACKER] = new ItemTrackerModel(traData.NAME, traData.PLURAL, GameImages.ITEM_ZTRACKER,
                ItemTrackerModel.TrackingFlags.UNDEADS,
                traData.BATTERIES * WorldTime.TURNS_PER_HOUR,
                traData.HASCLOCK)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = traData.FLAVOR,
                IsBatteryPowered = true
            };

            traData = DATA_TRACKER_BLACKOPS_GPS;
            this[IDs.TRACKER_BLACKOPS] = new ItemTrackerModel(traData.NAME, traData.PLURAL, GameImages.ITEM_BLACKOPS_GPS,
                ItemTrackerModel.TrackingFlags.BLACKOPS_FACTION,
                traData.BATTERIES * WorldTime.TURNS_PER_HOUR,
                traData.HASCLOCK)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = traData.FLAVOR,
                IsBatteryPowered = true
            };

            traData = DATA_TRACKER_POLICE_RADIO;
            this[IDs.TRACKER_POLICE_RADIO] = new ItemTrackerModel(traData.NAME, traData.PLURAL, GameImages.ITEM_POLICE_RADIO,
                ItemTrackerModel.TrackingFlags.POLICE_FACTION,
                traData.BATTERIES * WorldTime.TURNS_PER_HOUR,
                traData.HASCLOCK)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = traData.FLAVOR,
                IsBatteryPowered = true
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

            spData = DATA_PAINT_THINNER;  //@@MP (Release 7-6)
            this[IDs.PAINT_THINNER] = new ItemSprayPaintModel(spData.NAME, spData.PLURAL, GameImages.ITEM_PAINT_THINNER, spData.QUANTITY, GameImages.UNDEF)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = spData.FLAVOR
            };

            spData = DATA_FIRE_EXTINGUISHER;  //@@MP (Release 7-6)
            this[IDs.FIRE_EXTINGUISHER] = new ItemSprayPaintModel(spData.NAME, spData.PLURAL, GameImages.ITEM_FIRE_EXTINGUISHER, spData.QUANTITY, GameImages.UNDEF)
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
                FlavorDescription = ltData.FLAVOR,
                IsBatteryPowered = true
            };

            ltData = DATA_LIGHT_BIG_FLASHLIGHT;
            this[IDs.LIGHT_BIG_FLASHLIGHT] = new ItemLightModel(ltData.NAME, ltData.PLURAL, GameImages.ITEM_BIG_FLASHLIGHT, ltData.FOV, ltData.BATTERIES * WorldTime.TURNS_PER_HOUR, GameImages.ITEM_BIG_FLASHLIGHT_OUT)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = ltData.FLAVOR,
                IsBatteryPowered = true
            };

            ltData = DATA_LIGHT_NIGHT_VISION; //@@MP (Release 6-3)
            this[IDs.LIGHT_NIGHT_VISION] = new ItemLightModel(ltData.NAME, ltData.PLURAL, GameImages.ITEM_NIGHT_VISION, ltData.FOV, ltData.BATTERIES * WorldTime.TURNS_PER_HOUR, GameImages.ITEM_NIGHT_VISION)
            {
                EquipmentPart = DollPart.EYES,
                FlavorDescription = ltData.FLAVOR,
                IsBatteryPowered = true
            };

            ltData = DATA_LIGHT_BINOCULARS; //@@MP (Release 7-1)
            this[IDs.LIGHT_BINOCULARS] = new ItemLightModel(ltData.NAME, ltData.PLURAL, GameImages.ITEM_BINOCULARS, ltData.FOV, ltData.BATTERIES * WorldTime.TURNS_PER_HOUR, GameImages.ITEM_BINOCULARS)
            {
                EquipmentPart = DollPart.EYES,
                FlavorDescription = ltData.FLAVOR,
                IsBatteryPowered = false
            };

            ltData = DATA_LIGHT_FLARE; //@@MP (Release 7-1)
            this[IDs.LIGHT_FLARE] = new ItemLightModel(ltData.NAME, ltData.PLURAL, GameImages.ITEM_LIT_FLARE, ltData.FOV, ltData.BATTERIES * WorldTime.TURNS_PER_HOUR, GameImages.ITEM_LIT_FLARE)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = ltData.FLAVOR,
                IsThrowable = true,
                IsBatteryPowered = false
            };

            ltData = DATA_LIGHT_GLOWSTICK; //@@MP (Release 7-1)
            this[IDs.LIGHT_GLOWSTICK] = new ItemLightModel(ltData.NAME, ltData.PLURAL, GameImages.ITEM_LIT_GLOWSTICK, ltData.FOV, ltData.BATTERIES * WorldTime.TURNS_PER_HOUR, GameImages.ITEM_LIT_GLOWSTICK)
            {
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = ltData.FLAVOR,
                IsThrowable = true,
                IsBatteryPowered = false
            };
            #endregion

            #region Scent sprays
            // alpha10 new way of using stench killer
            ScentSprayData sspData;

            sspData = DATA_SCENT_SPRAY_STENCH_KILLER;
            this[IDs.SCENT_SPRAY_STENCH_KILLER] = new ItemSprayScentModel(sspData.NAME, sspData.PLURAL, GameImages.ITEM_STENCH_KILLER, 
                sspData.QUANTITY, Odor.SUPPRESSOR, sspData.STRENGTH * WorldTime.TURNS_PER_HOUR)
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

            entData = DATA_ENT_BOOK_CHAR;
            this[IDs.ENT_BOOK_CHAR] = new ItemEntertainmentModel(entData.NAME, entData.PLURAL, GameImages.ITEM_BOOK_CHAR, entData.VALUE, entData.BORECHANCE)
            {
                StackingLimit = entData.STACKING,
                FlavorDescription = entData.FLAVOR
            };

            entData = DATA_ENT_BOOK_BLUE;
            this[IDs.ENT_BOOK_BLUE] = new ItemEntertainmentModel(entData.NAME, entData.PLURAL, GameImages.ITEM_BOOK_BLUE, entData.VALUE, entData.BORECHANCE)
            {
                StackingLimit = entData.STACKING,
                FlavorDescription = entData.FLAVOR
            };

            entData = DATA_ENT_BOOK_GREEN;
            this[IDs.ENT_BOOK_GREEN] = new ItemEntertainmentModel(entData.NAME, entData.PLURAL, GameImages.ITEM_BOOK_GREEN, entData.VALUE, entData.BORECHANCE)
            {
                StackingLimit = entData.STACKING,
                FlavorDescription = entData.FLAVOR
            };

            entData = DATA_ENT_BOOK_RED;
            this[IDs.ENT_BOOK_RED] = new ItemEntertainmentModel(entData.NAME, entData.PLURAL, GameImages.ITEM_BOOK_RED, entData.VALUE, entData.BORECHANCE)
            {
                StackingLimit = entData.STACKING,
                FlavorDescription = entData.FLAVOR
            };

            entData = DATA_ENT_MAGAZINE1;
            this[IDs.ENT_MAGAZINE1] = new ItemEntertainmentModel(entData.NAME, entData.PLURAL, GameImages.ITEM_MAGAZINE1, entData.VALUE, entData.BORECHANCE)
            {
                StackingLimit = entData.STACKING,
                FlavorDescription = entData.FLAVOR
            };

            entData = DATA_ENT_MAGAZINE2;
            this[IDs.ENT_MAGAZINE2] = new ItemEntertainmentModel(entData.NAME, entData.PLURAL, GameImages.ITEM_MAGAZINE2, entData.VALUE, entData.BORECHANCE)
            {
                StackingLimit = entData.STACKING,
                FlavorDescription = entData.FLAVOR
            };

            entData = DATA_ENT_MAGAZINE3;
            this[IDs.ENT_MAGAZINE3] = new ItemEntertainmentModel(entData.NAME, entData.PLURAL, GameImages.ITEM_MAGAZINE3, entData.VALUE, entData.BORECHANCE)
            {
                StackingLimit = entData.STACKING,
                FlavorDescription = entData.FLAVOR
            };

            entData = DATA_ENT_MAGAZINE4;
            this[IDs.ENT_MAGAZINE4] = new ItemEntertainmentModel(entData.NAME, entData.PLURAL, GameImages.ITEM_MAGAZINE4, entData.VALUE, entData.BORECHANCE)
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
                FlavorDescription = "A memo regarding using generators to power to the facility in an emergency."
            };

            //@@MP (Release 6-3)
            this[IDs.UNIQUE_ARMY_ACCESS_BADGE] = new ItemModel("Army office pass", "Army office pass", GameImages.ITEM_SUBWAY_BADGE)
            {
                DontAutoEquip = true,
                EquipmentPart = DollPart.LEFT_HAND,
                FlavorDescription = "Army office pass"
            };

            //@@MP (Release 7-6)
            this[IDs.UNIQUE_BOOK_OF_ARMAMENTS] = new ItemModel("Book of Armaments", "Book of Armaments", GameImages.ITEM_UNIQUE_BOOK)
            {
                FlavorDescription = "It's open at chapter 2, verses 9 through 21."
            };
            #endregion

            #region Miscellaneous
            //@@MP (Release 5-3), (Release 5-5)
            this[IDs.VEGETABLE_SEEDS] = new ItemModel("bunch of vegie seeds", "bunch of vegie seeds", GameImages.ITEM_VEGETABLE_SEEDS)
            {
                FlavorDescription = @"Use a shovel or pickaxe to plant seeds. Return later to harvest.",
                IsStackable = true,
                StackingLimit = 9
            };

            //@@MP (Release 7-1)
            this[IDs.SIPHON_KIT] = new ItemModel("siphon kit", "siphon kits", GameImages.ITEM_SIPHON_KIT)
            {
                FlavorDescription = @"Siphon fuel from cars for chainsaws and flamethrowers.",
                IsStackable = false
            };

            this[IDs.CANDLES_BOX] = new ItemModel("candles box", "candles boxes", GameImages.ITEM_CANDLES_BOX)
            {
                FlavorDescription = @"Place candles for long-lasting light.",
                IsStackable = true,
                StackingLimit = 40
            };

            this[IDs.FLARES_KIT] = new ItemModel("flares kit", "flares kits", GameImages.ITEM_FLARES_KIT)
            {
                FlavorDescription = @"Use flares for bright, throwable light.",
                IsStackable = true,
                StackingLimit = 40
            };

            this[IDs.GLOWSTICKS_BOX] = new ItemModel("glowsticks box", "glowsticks boxes", GameImages.ITEM_GLOWSTICKS_BOX)
            {
                FlavorDescription = @"Use glowsticks for long-lasting, throwable light.",
                IsStackable = true,
                StackingLimit = 60
            };

            this[IDs.LIQUOR_AMBER] = new ItemModel("liquor", "liquor", GameImages.ITEM_LIQUOR_BOTTLE_AMBER)
            {
                FlavorDescription = @"Use them to make molotovs.",
                IsStackable = true,
                StackingLimit = 3
            };

            this[IDs.LIQUOR_CLEAR] = new ItemModel("liquor", "liquor", GameImages.ITEM_LIQUOR_BOTTLE_CLEAR)
            {
                FlavorDescription = @"Use them to make molotovs.",
                IsStackable = true,
                StackingLimit = 3
            };

            this[IDs.POLICE_RIOT_SHIELD] = new ItemModel("police riot shield", "police riot shields", GameImages.ITEM_POLICE_RIOT_SHIELD) //@@MP (Release 7-2)
            {
                FlavorDescription = Rules.SHIELD_BASE_BLOCK_CHANCE.ToString() + "% base chance to block melee attacks.",
                IsStackable = false,
                EquipmentPart = DollPart.LEFT_ARM
            };

            this[IDs.SLEEPING_BAG] = new ItemModel("sleeping bag", "sleeping bags", GameImages.ITEM_SLEEPING_BAG) //@@MP (Release 7-3)
            {
                FlavorDescription = @"Drop it on the ground for a somewhat comfortable sleep.",
                IsStackable = false
            };

            this[IDs.FISHING_ROD] = new ItemModel("fishing rod", "fishing rods", GameImages.ITEM_FISHING_ROD) //@@MP (Release 7-6)
            {
                FlavorDescription = @"Stand next to a pond then equip it to catch fish.",
                IsStackable = false,
                EquipmentPart = DollPart.LEFT_HAND,
                DontAutoEquip = true
            };

            this[IDs.MATCHES] = new ItemModel("matches", "matches", GameImages.ITEM_MATCHES) //@@MP (Release 7-6)
            {
                //MP: Note to be confused with the ItemGrenade matches that I intended to be used with fuel poured/spilled on the ground in R7-1, but never implemented
                FlavorDescription = @"Use with wood to make a campfire or start a receptacle fire.",
                IsStackable = true,
                EquipmentPart = DollPart.LEFT_HAND,
                IsPlural = true
            };
            #endregion

            #region Fixes/Post processing
            //for (int i = (int)IDs._FIRST; i < (int)IDs._COUNT; i++)
            for (int i = 0; i < (int)IDs._COUNT; i++) //@@MP - took _FIRST out because for some reason it became totally bugged (Release 4)
            {
                ItemModel model = this[i];
                //Logger.WriteLine(Logger.Stage.INIT_MAIN, "creating model " + model.SingleName); //for troubleshooting

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
        static void Notify(IRogueUI ui, string what, string stage) //@@MP - made static (Release 5-7)
        {
            ui.UI_Clear(Color.Black);
            ui.UI_DrawStringBold(Color.White, "Loading "+what+" data : " + stage, 0, 0);
            ui.UI_Repaint();
        }

        static CSVLine FindLineForModel(CSVTable table, IDs modelID) //@@MP - made static (Release 5-7)
        {
            foreach (CSVLine l in table.Lines)
            {
                if (l[0].ParseText() == modelID.ToString())
                    return l;
            }

            return null;
        }

        _DATA_TYPE_ GetDataFromCSVTable<_DATA_TYPE_>(CSVTable table, Func<CSVLine, _DATA_TYPE_> fn, IDs modelID, string path)
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
                //reader.Close();
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
                data[i] = GetDataFromCSVTable<_DATA_TYPE_>(table, fn, idsToRead[i], path); //@@MP - unused parameter (Release 5-7)
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
                new IDs[] { IDs.MEDICINE_SMALL_MEDIKIT, IDs.MEDICINE_LARGE_MEDIKIT, IDs.MEDICINE_PILLS_SLP, IDs.MEDICINE_PILLS_STA,
                            IDs.MEDICINE_PILLS_SAN, IDs.MEDICINE_PILLS_ANTIVIRAL, IDs.MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN,
                            IDs.MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN, IDs.MEDICINE_ALCOHOL_BEER_CAN_BLUE,
                            IDs.MEDICINE_ALCOHOL_BEER_CAN_RED, IDs.MEDICINE_CIGARETTES, IDs.MEDICINE_ENERGY_DRINK },
                out data);

            DATA_MEDICINE_SMALL_MEDIKIT = data[0];
            DATA_MEDICINE_LARGE_MEDIKIT = data[1];
            DATA_MEDICINE_PILLS_SLP = data[2];
            DATA_MEDICINE_PILLS_STA = data[3];
            DATA_MEDICINE_PILLS_SAN = data[4];
            DATA_MEDICINE_PILLS_ANTIVIRAL = data[5];
            //@@MP (Release 4)
            DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_BROWN = data[6];
            DATA_MEDICINE_ALCOHOL_BEER_BOTTLE_GREEN = data[7];
            DATA_MEDICINE_ALCOHOL_BEER_CAN_BLUE = data[8];
            DATA_MEDICINE_ALCOHOL_BEER_CAN_RED = data[9];
            DATA_MEDICINE_CIGARETTES = data[10];
            DATA_MEDICINE_ENERGY_DRINK = data[11]; //@@MP (Release 7-1)

            return true;
        }
        #endregion

        #region Food
        public bool LoadFoodFromCSV(IRogueUI ui, string path)
        {
            FoodData[] data;

            LoadDataFromCSV<FoodData>(ui, path, "food items", FoodData.COUNT_FIELDS, FoodData.FromCSVLine,
                new IDs[] { IDs.FOOD_ARMY_RATION, IDs.FOOD_CANNED_FOOD, IDs.FOOD_GROCERIES, IDs.FOOD_WILD_BERRIES, IDs.FOOD_VEGETABLES, IDs.FOOD_SNACK_BAR, IDs.FOOD_PEANUTS, IDs.FOOD_GRAPES,
                IDs.FOOD_RAW_FISH, IDs.FOOD_COOKED_FISH, IDs.FOOD_RAW_RABBIT, IDs.FOOD_COOKED_RABBIT, IDs.FOOD_RAW_CHICKEN, IDs.FOOD_COOKED_CHICKEN, IDs.FOOD_RAW_DOG_MEAT,
                IDs.FOOD_COOKED_DOG_MEAT, IDs.FOOD_RAW_HUMAN_FLESH, IDs.FOOD_COOKED_HUMAN_FLESH, IDs.FOOD_CHICKEN_EGG},
                out data);

            DATA_FOOD_ARMY_RATION = data[0];
            DATA_FOOD_CANNED_FOOD = data[1];
            DATA_FOOD_GROCERIES = data[2];
            DATA_FOOD_WILD_BERRIES = data[3]; //MP (Release 4)
            DATA_FOOD_VEGETABLES = data[4]; //MP (Release 5-5)
            DATA_FOOD_SNACK_BAR = data[5]; //MP (Release 7-1)
            DATA_FOOD_PEANUTS = data[6]; //MP (Release 7-3)
            DATA_FOOD_GRAPES = data[7]; //MP (Release 7-3)
            DATA_FOOD_RAW_FISH = data[8]; //MP (Release 7-6)
            DATA_FOOD_COOKED_FISH = data[9]; //MP (Release 7-6)
            DATA_FOOD_RAW_RABBIT = data[10]; //MP (Release 7-6)
            DATA_FOOD_COOKED_RABBIT = data[11]; //MP (Release 7-6)
            DATA_FOOD_RAW_CHICKEN = data[12]; //MP (Release 7-6)
            DATA_FOOD_COOKED_CHICKEN = data[13]; //MP (Release 7-6)
            DATA_FOOD_RAW_DOG_MEAT = data[14]; //MP (Release 7-6)
            DATA_FOOD_COOKED_DOG_MEAT = data[15]; //MP (Release 7-6)
            DATA_FOOD_RAW_HUMAN_FLESH = data[16]; //MP (Release 7-6)
            DATA_FOOD_COOKED_HUMAN_FLESH = data[17]; //MP (Release 7-6)
            DATA_FOOD_CHICKEN_EGG = data[18]; //MP (Release 7-6)

            return true;
        }
        #endregion

        #region Melee weapons
        public bool LoadMeleeWeaponsFromCSV(IRogueUI ui, string path)
        {
            MeleeWeaponData[] data;

            LoadDataFromCSV<MeleeWeaponData>(ui, path, "melee weapons items", MeleeWeaponData.COUNT_FIELDS, MeleeWeaponData.FromCSVLine,
                new IDs[] { IDs.MELEE_BASEBALLBAT, IDs.MELEE_COMBAT_KNIFE, IDs.MELEE_CROWBAR, IDs.MELEE_GOLFCLUB, IDs.MELEE_HUGE_HAMMER, IDs.MELEE_IRON_GOLFCLUB,
                            IDs.MELEE_SHOVEL, IDs.MELEE_SHORT_SHOVEL, IDs.MELEE_TRUNCHEON, IDs.UNIQUE_JASON_MYERS_AXE, IDs.MELEE_IMPROVISED_CLUB, IDs.MELEE_IMPROVISED_SPEAR,
                            IDs.MELEE_SMALL_HAMMER, IDs.UNIQUE_FAMU_FATARU_KATANA, IDs.UNIQUE_BIGBEAR_BAT, IDs.UNIQUE_ROGUEDJACK_KEYBOARD, IDs.MELEE_TENNIS_RACKET,
                            IDs.MELEE_HOCKEY_STICK, IDs.MELEE_MACHETE, IDs.MELEE_STANDARD_AXE, IDs.MELEE_PICKAXE, IDs.MELEE_PIPE_WRENCH, IDs.MELEE_CHAINSAW,
                            IDs.MELEE_CLEAVER, IDs.MELEE_BRASS_KNUCKLES, IDs.MELEE_FLAIL, IDs.MELEE_KITCHEN_KNIFE, IDs.MELEE_SCIMITAR, IDs.MELEE_MACE, IDs.MELEE_NUNCHAKU,
                            IDs.MELEE_FRYING_PAN, IDs.MELEE_PITCH_FORK, IDs.MELEE_SCYTHE, IDs.MELEE_SICKLE, IDs.MELEE_SPEAR, IDs.MELEE_SPIKED_MACE, IDs.MELEE_FIRE_AXE},
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
            //@@MP (Release 7-1)
            DATA_MELEE_CHAINSAW = data[22];
            //@@MP (Release 7-6)
            DATA_MELEE_CLEAVER = data[23];
            DATA_MELEE_BRASS_KNUCKLES = data[24];
            DATA_MELEE_FLAIL = data[25];
            DATA_MELEE_KITCHEN_KNIFE = data[26];
            DATA_MELEE_SCIMITAR = data[27];
            DATA_MELEE_MACE = data[28];
            DATA_MELEE_NUNCHAKU = data[29];
            DATA_MELEE_FRYING_PAN = data[30];
            DATA_MELEE_PITCH_FORK = data[31];
            DATA_MELEE_SCYTHE = data[32];
            DATA_MELEE_SICKLE = data[33];
            DATA_MELEE_SPEAR = data[34];
            DATA_MELEE_SPIKED_MACE = data[35];
            DATA_MELEE_FIRE_AXE = data[36];

            return true;
        }
        #endregion

        #region Ranged weapons
        public bool LoadRangedWeaponsFromCSV(IRogueUI ui, string path)
        {
            RangedWeaponData[] data;

            LoadDataFromCSV<RangedWeaponData>(ui, path, "ranged weapons items", RangedWeaponData.COUNT_FIELDS, RangedWeaponData.FromCSVLine,
                new IDs[] { IDs.RANGED_ARMY_PISTOL, IDs.RANGED_ARMY_PRECISION_RIFLE, IDs.RANGED_ARMY_RIFLE1, IDs.RANGED_HUNTING_CROSSBOW, IDs.RANGED_HUNTING_RIFLE,
                            IDs.RANGED_PISTOL, IDs.RANGED_REVOLVER, IDs.RANGED_PRECISION_RIFLE, IDs.RANGED_SHOTGUN, IDs.UNIQUE_SANTAMAN_SHOTGUN,
                            IDs.UNIQUE_HANS_VON_HANZ_PISTOL, IDs.RANGED_NAIL_GUN, IDs.RANGED_FLAMETHROWER, IDs.RANGED_STUN_GUN, IDs.RANGED_SMG, IDs.RANGED_DOUBLE_BARREL,
                            IDs.RANGED_MINIGUN, IDs.RANGED_TACTICAL_SHOTGUN, IDs.RANGED_ARMY_RIFLE2, IDs.RANGED_ARMY_RIFLE3, IDs.RANGED_ARMY_RIFLE4, IDs.RANGED_GRENADE_LAUNCHER,
                            IDs.RANGED_BIO_FORCE_GUN },
                out data);

            DATA_RANGED_ARMY_PISTOL = data[0];
            DATA_RANGED_ARMY_PRECISION_RIFLE = data[1]; //@@MP (Release 7-6)
            DATA_RANGED_ARMY_RIFLE1 = data[2];
            DATA_RANGED_HUNTING_CROSSBOW = data[3];
            DATA_RANGED_HUNTING_RIFLE = data[4];
            DATA_RANGED_PISTOL = data[5];
            DATA_RANGED_REVOLVER = data[6];
            DATA_RANGED_PRECISION_RIFLE = data[7];
            DATA_RANGED_SHOTGUN = data[8];
            DATA_UNIQUE_SANTAMAN_SHOTGUN = data[9];
            DATA_UNIQUE_HANS_VON_HANZ_PISTOL = data[10];
            DATA_RANGED_NAIL_GUN = data[11]; //@@MP (Release 5-1)
            DATA_RANGED_FLAMETHROWER = data[12]; //@@MP (Release 7-1)
            DATA_RANGED_STUN_GUN = data[13]; //@@MP (Release 5-1)
            //@@MP (Release 7-6)
            DATA_RANGED_SMG = data[14];
            DATA_RANGED_DOUBLE_BARREL = data[15];
            DATA_RANGED_MINIGUN = data[16];
            DATA_RANGED_TACTICAL_SHOTGUN = data[17];
            DATA_RANGED_ARMY_RIFLE2 = data[18];
            DATA_RANGED_ARMY_RIFLE3 = data[19];
            DATA_RANGED_ARMY_RIFLE4 = data[20];
            DATA_RANGED_GRENADE_LAUNCHER = data[21];
            DATA_RANGED_BIO_FORCE_GUN = data[22];

            return true;
        }
        #endregion

        #region Explosives
        public bool LoadExplosivesFromCSV(IRogueUI ui, string path)
        {
            ExplosiveData[] data;

            LoadDataFromCSV<ExplosiveData>(ui, path, "explosives items", ExplosiveData.COUNT_FIELDS, ExplosiveData.FromCSVLine,
                new IDs[] { IDs.EXPLOSIVE_GRENADE, IDs.EXPLOSIVE_MOLOTOV, IDs.EXPLOSIVE_DYNAMITE, IDs.EXPLOSIVE_C4, IDs.EXPLOSIVE_FUEL_CAN, IDs.EXPLOSIVE_FUEL_PUMP,
                    IDs.EXPLOSIVE_SMOKE_GRENADE, IDs.EXPLOSIVE_FLASHBANG, IDs.EXPLOSIVE_HOLY_HAND_GRENADE, IDs.EXPLOSIVE_PLASMA_CHARGE },
                out data);

            DATA_EXPLOSIVE_GRENADE = data[0];
            //@@MP (Release 4)
            DATA_EXPLOSIVE_MOLOTOV = data[1];
            DATA_EXPLOSIVE_DYNAMITE = data[2];
            DATA_EXPLOSIVE_C4 = data[3]; //@@MP (Release 6-3)
            DATA_EXPLOSIVE_FUEL_CAN = data[4]; //@@MP (Release 7-1)
            DATA_EXPLOSIVE_FUEL_PUMP = data[5]; //@@MP (Release 7-1)
            DATA_EXPLOSIVE_SMOKE_GRENADE = data[6]; //@@MP (Release 7-2)
            DATA_EXPLOSIVE_FLASHBANG = data[7]; //@@MP (Release 7-2)
            DATA_EXPLOSIVE_HOLY_HAND_GRENADE = data[8]; //@@MP (Release 7-6)
            DATA_EXPLOSIVE_PLASMA_CHARGE = data[9]; //@@MP (Release 7-6)

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
                new IDs[] { IDs.ARMOR_ARMY_BODYARMOR,IDs.ARMOR_CHAR_LIGHT_BODYARMOR, IDs.ARMOR_HELLS_SOULS_JACKET, IDs.ARMOR_FREE_ANGELS_JACKET, IDs.ARMOR_POLICE_JACKET,
                    IDs.ARMOR_POLICE_RIOT, IDs.ARMOR_HUNTER_VEST, IDs.ARMOR_FIRE_HAZARD_SUIT, IDs.ARMOR_BIOHAZARD_SUIT },
                out data);

            DATA_ARMOR_ARMY = data[0];
            DATA_ARMOR_CHAR = data[1];
            DATA_ARMOR_HELLS_SOULS_JACKET = data[2];
            DATA_ARMOR_FREE_ANGELS_JACKET = data[3];
            DATA_ARMOR_POLICE_JACKET = data[4];
            DATA_ARMOR_POLICE_RIOT = data[5];
            DATA_ARMOR_HUNTER_VEST = data[6];
            DATA_ARMOR_FIRE_HAZARD_SUIT = data[7]; //@@MP (Release 7-1)
            DATA_ARMOR_BIOHAZARD_SUIT = data[8]; //@@MP (Release 7-6)

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
                new IDs[] { IDs.SPRAY_PAINT1, IDs.SPRAY_PAINT2, IDs.SPRAY_PAINT3, IDs.SPRAY_PAINT4, IDs.PAINT_THINNER, IDs.FIRE_EXTINGUISHER },
                out data);

            DATA_SPRAY_PAINT1 = data[0];
            DATA_SPRAY_PAINT2 = data[1];
            DATA_SPRAY_PAINT3 = data[2];
            DATA_SPRAY_PAINT4 = data[3];
            DATA_PAINT_THINNER = data[4]; //@@MP (Release 7-6)
            DATA_FIRE_EXTINGUISHER = data[5]; //@@MP (Release 7-6)

            return true;
        }
        #endregion

        #region Lights
        public bool LoadLightsFromCSV(IRogueUI ui, string path)
        {
            LightData[] data;

            LoadDataFromCSV<LightData>(ui, path, "lights items", LightData.COUNT_FIELDS, LightData.FromCSVLine,
                new IDs[] { IDs.LIGHT_FLASHLIGHT, IDs.LIGHT_BIG_FLASHLIGHT, IDs.LIGHT_NIGHT_VISION, IDs.LIGHT_BINOCULARS, IDs.LIGHT_FLARE, IDs.LIGHT_GLOWSTICK },
                out data);

            DATA_LIGHT_FLASHLIGHT = data[0];
            DATA_LIGHT_BIG_FLASHLIGHT = data[1];
            DATA_LIGHT_NIGHT_VISION = data[2]; //@@MP (Release 6-3)
            DATA_LIGHT_BINOCULARS = data[3]; //@@MP (Release 7-1)
            DATA_LIGHT_FLARE = data[4]; //@@MP (Release 7-1)
            DATA_LIGHT_GLOWSTICK = data[5]; //@@MP (Release 7-1)

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
                new IDs[] { IDs.ENT_BOOK_CHAR, IDs.ENT_BOOK_BLUE, IDs.ENT_BOOK_GREEN, IDs.ENT_BOOK_RED, IDs.ENT_MAGAZINE1, IDs.ENT_MAGAZINE2, IDs.ENT_MAGAZINE3, IDs.ENT_MAGAZINE4 },
                out data);

            DATA_ENT_BOOK_CHAR = data[0];
            DATA_ENT_BOOK_BLUE = data[1];
            DATA_ENT_BOOK_GREEN = data[2];
            DATA_ENT_BOOK_RED = data[3];
            DATA_ENT_MAGAZINE1 = data[4];
            DATA_ENT_MAGAZINE2 = data[5];
            DATA_ENT_MAGAZINE3 = data[6];
            DATA_ENT_MAGAZINE4 = data[7];

            return true;
        }
        #endregion
        #endregion
    }
}
