using System;
using System.Collections.Generic;
using System.Drawing;

using djack.RogueSurvivor.Engine;

namespace djack.RogueSurvivor.Gameplay
{
    static class GameImages
    {
        #region Constants
        //@@MP - tiles visited but not within current FOV are now darker (Release 6-2)
        const float GRAYLEVEL_DIM_FACTOR_DAYTIME = 0.40f; //@@MP this was the one and only in vanilla
        const float GRAYLEVEL_DIM_FACTOR_NIGHTTIME_CLEAR = 0.20f;
        const float GRAYLEVEL_DIM_FACTOR_NIGHTTIME_CLOUDED = 0.15f;
        const float GRAYLEVEL_DIM_FACTOR_UNDERGROUND_LITTORCH = 0.20f;
        const float GRAYLEVEL_DIM_FACTOR_UNDERGROUND_NOTORCH = 0.10f;
        #endregion

        #region Images IDs

        #region Icons
        public const string ACTIVITY_CHASING = @"Activities\chasing";
        public const string ACTIVITY_CHASING_PLAYER = @"Activities\chasing_player";
        public const string ACTIVITY_TRACKING = @"Activities\tracking";
        public const string ACTIVITY_FLEEING = @"Activities\fleeing";
        public const string ACTIVITY_FLEEING_FROM_EXPLOSIVE = @"Activities\fleeing_explosive";
        public const string ACTIVITY_FOLLOWING = @"Activities\following";
        public const string ACTIVITY_FOLLOWING_ORDER = @"Activities\following_order";
        public const string ACTIVITY_FOLLOWING_PLAYER = @"Activities\following_player";
        public const string ACTIVITY_FOLLOWING_LEADER = @"Activities\following_leader";  // alpha10
        public const string ACTIVITY_SLEEPING = @"Activities\sleeping";

        public const string ICON_BLAST = @"Icons\blast";
        public const string ICON_CAN_TRADE = @"Icons\can_trade";
        public const string ICON_THREAT_SAFE = @"Icons\threat_safe";
        public const string ICON_THREAT_DANGER = @"Icons\threat_danger";
        public const string ICON_THREAT_HIGH_DANGER = @"Icons\threat_high_danger";
        public const string ICON_CANT_RUN = @"Icons\cant_run";
        public const string ICON_DRUNK = @"Icons\drunk"; //@@MP (Release 7-1)
        public const string ICON_EXPIRED_FOOD = @"Icons\expired_food";
        public const string ICON_FOOD_ALMOST_HUNGRY = @"Icons\food_almost_hungry";
        public const string ICON_FOOD_HUNGRY = @"Icons\food_hungry";
        public const string ICON_FOOD_STARVING = @"Icons\food_starving";
        public const string ICON_HEALING = @"Icons\healing";
        public const string ICON_INCAPACITATED = @"Icons\incapacitated"; //@@MP (Release 7-1)
        public const string ICON_IS_TARGET = @"Icons\is_target";
        public const string ICON_IS_TARGETTED = @"Icons\is_targetted";
        public const string ICON_IS_TARGETING = @"Icons\is_targeting";  // alpha10
        public const string ICON_IS_IN_GROUP = @"Icons\is_in_group";  // alpha10
        public const string ICON_KILLED = @"Icons\killed";
        public const string ICON_LEADER = @"Icons\leader";
        public const string ICON_MELEE_ATTACK = @"Icons\melee_attack";
        public const string ICON_MELEE_MISS = @"Icons\melee_miss";
        public const string ICON_MELEE_DAMAGE = @"Icons\melee_damage";
        public const string ICON_ODOR_SUPPRESSED = @"Icons\odor_suppressed";  // alpha10
        public const string ICON_OUT_OF_AMMO = @"Icons\out_of_ammo";
        public const string ICON_OUT_OF_BATTERIES = @"Icons\out_of_batteries";
        public const string ICON_RANGED_ATTACK = @"Icons\ranged_attack";
        public const string ICON_RANGED_MISS = @"Icons\ranged_miss";
        public const string ICON_RANGED_DAMAGE = @"Icons\ranged_damage";
        public const string ICON_RUNNING = @"Icons\running";
        public const string ICON_ROT_ALMOST_HUNGRY = @"Icons\rot_almost_hungry";
        public const string ICON_ROT_HUNGRY = @"Icons\rot_hungry";
        public const string ICON_ROT_STARVING = @"Icons\rot_starving";
        public const string ICON_SLEEP_ALMOST_SLEEPY = @"Icons\sleep_almost_sleepy";
        public const string ICON_SLEEP_EXHAUSTED = @"Icons\sleep_exhausted";
        public const string ICON_SLEEP_SLEEPY = @"Icons\sleep_sleepy";
        public const string ICON_SPOILED_FOOD = @"Icons\spoiled_food";
        public const string ICON_TARGET = @"Icons\target";
        public const string ICON_LINE_BLOCKED = @"Icons\line_blocked";
        public const string ICON_LINE_CLEAR = @"Icons\line_clear";
        public const string ICON_LINE_BAD = @"Icons\line_bad";
        public const string ICON_SCENT_LIVING = @"Icons\scent_living";
        public const string ICON_SCENT_ZOMBIEMASTER = @"Icons\scent_zm";
        //public const string ICON_SCENT_LIVING_SUPRESSOR = @"Icons\scent_living_supressor"; //alpha 10 obsolete
        public const string ICON_AGGRESSOR = @"Icons\enemy_you_aggressor";
        public const string ICON_INDIRECT_ENEMIES = @"Icons\enemy_indirect";
        public const string ICON_SELF_DEFENCE = @"Icons\enemy_you_self_defence";
        public const string ICON_TRAP_ACTIVATED = @"Icons\trap_activated";
        public const string ICON_TRAP_ACTIVATED_SAFE_GROUP = @"Icons\trap_activated_safe_group";  // alpha10
        public const string ICON_TRAP_ACTIVATED_SAFE_PLAYER = @"Icons\trap_activated_safe_player";  // alpha10
        public const string ICON_TRAP_TRIGGERED = @"Icons\trap_triggered";
        public const string ICON_TRAP_TRIGGERED_SAFE_GROUP = @"Icons\trap_triggered_safe_group";  // alpha10
        public const string ICON_TRAP_TRIGGERED_SAFE_PLAYER = @"Icons\trap_triggered_safe_player";  // alpha10
        public const string ICON_SANITY_DISTURBED = @"Icons\sanity_disturbed";
        public const string ICON_SANITY_INSANE = @"Icons\sanity_insane";
        public const string ICON_BORING_ITEM = @"Icons\boring_item";
        public const string ICON_ZGRAB = @"Icons\zgrab";  // alpha10
        #endregion

        #region Tiles
        public const string TILE_FLOOR_ASPHALT = @"Tiles\floor_asphalt";
        public const string TILE_FLOOR_CONCRETE = @"Tiles\floor_concrete";
        public const string TILE_FLOOR_GRASS = @"Tiles\floor_grass";
        public const string TILE_FLOOR_OFFICE = @"Tiles\floor_office";
        public const string TILE_FLOOR_PLANKS = @"Tiles\floor_planks";
        public const string TILE_FLOOR_PLANTED = @"Tiles\floor_planted"; //@@MP (Release 5-5)
        public const string TILE_FLOOR_SEWER_WATER = @"Tiles\floor_sewer_water";
        public const string TILE_FLOOR_SEWER_WATER_ANIM1 = @"Tiles\floor_sewer_water_anim1";
        public const string TILE_FLOOR_SEWER_WATER_ANIM2 = @"Tiles\floor_sewer_water_anim2";
        public const string TILE_FLOOR_SEWER_WATER_ANIM3 = @"Tiles\floor_sewer_water_anim3";
        public const string TILE_FLOOR_SEWER_WATER_COVER = @"Tiles\floor_sewer_water_cover";
        public const string TILE_FLOOR_TILES = @"Tiles\floor_tiles";
        public const string TILE_FLOOR_WALKWAY = @"Tiles\floor_walkway";
        public const string TILE_FLOOR_WHITE_TILE = @"Tiles\floor_white_tile";
        //@@MP (Release 4)
        public const string TILE_FLOOR_RED_CARPET = @"Tiles\floor_red_carpet";
        public const string TILE_FLOOR_BLUE_CARPET = @"Tiles\floor_blue_carpet";
        public const string TILE_FLOOR_DIRT = @"Tiles\floor_dirt";

        //@@MP (Release 7-3)
        public const string TILE_FLOOR_FOOD_COURT_POOL = @"Tiles\floor_food_court_pool";
        public const string TILE_FLOOR_POOL_WATER_COVER = @"Tiles\floor_pool_water_cover";
        #region -Tennis court
        public const string TILE_FLOOR_TENNIS_COURT_OUTER = @"Tiles\tennis_court\floor_tennis_court_outer";
        public const string TILE_FLOOR_TENNIS_COURT_10 = @"Tiles\tennis_court\floor_tennis_court_10";
        public const string TILE_FLOOR_TENNIS_COURT_11 = @"Tiles\tennis_court\floor_tennis_court_11";
        public const string TILE_FLOOR_TENNIS_COURT_12 = @"Tiles\tennis_court\floor_tennis_court_12";
        public const string TILE_FLOOR_TENNIS_COURT_13 = @"Tiles\tennis_court\floor_tennis_court_13";
        public const string TILE_FLOOR_TENNIS_COURT_14 = @"Tiles\tennis_court\floor_tennis_court_14";
        public const string TILE_FLOOR_TENNIS_COURT_15 = @"Tiles\tennis_court\floor_tennis_court_15";
        public const string TILE_FLOOR_TENNIS_COURT_18 = @"Tiles\tennis_court\floor_tennis_court_18";
        public const string TILE_FLOOR_TENNIS_COURT_19 = @"Tiles\tennis_court\floor_tennis_court_19";
        public const string TILE_FLOOR_TENNIS_COURT_20 = @"Tiles\tennis_court\floor_tennis_court_20";
        public const string TILE_FLOOR_TENNIS_COURT_21 = @"Tiles\tennis_court\floor_tennis_court_21";
        public const string TILE_FLOOR_TENNIS_COURT_22 = @"Tiles\tennis_court\floor_tennis_court_22";
        public const string TILE_FLOOR_TENNIS_COURT_23 = @"Tiles\tennis_court\floor_tennis_court_23";
        public const string TILE_FLOOR_TENNIS_COURT_26 = @"Tiles\tennis_court\floor_tennis_court_26";
        public const string TILE_FLOOR_TENNIS_COURT_27 = @"Tiles\tennis_court\floor_tennis_court_27";
        public const string TILE_FLOOR_TENNIS_COURT_28 = @"Tiles\tennis_court\floor_tennis_court_28";
        public const string TILE_FLOOR_TENNIS_COURT_29 = @"Tiles\tennis_court\floor_tennis_court_29";
        public const string TILE_FLOOR_TENNIS_COURT_30 = @"Tiles\tennis_court\floor_tennis_court_30";
        public const string TILE_FLOOR_TENNIS_COURT_31 = @"Tiles\tennis_court\floor_tennis_court_31";
        public const string TILE_FLOOR_TENNIS_COURT_34 = @"Tiles\tennis_court\floor_tennis_court_34";
        public const string TILE_FLOOR_TENNIS_COURT_35 = @"Tiles\tennis_court\floor_tennis_court_35";
        public const string TILE_FLOOR_TENNIS_COURT_36 = @"Tiles\tennis_court\floor_tennis_court_36";
        public const string TILE_FLOOR_TENNIS_COURT_37 = @"Tiles\tennis_court\floor_tennis_court_37";
        public const string TILE_FLOOR_TENNIS_COURT_38 = @"Tiles\tennis_court\floor_tennis_court_38";
        public const string TILE_FLOOR_TENNIS_COURT_39 = @"Tiles\tennis_court\floor_tennis_court_39";
        public const string TILE_FLOOR_TENNIS_COURT_42 = @"Tiles\tennis_court\floor_tennis_court_42";
        public const string TILE_FLOOR_TENNIS_COURT_43 = @"Tiles\tennis_court\floor_tennis_court_43";
        public const string TILE_FLOOR_TENNIS_COURT_44 = @"Tiles\tennis_court\floor_tennis_court_44";
        public const string TILE_FLOOR_TENNIS_COURT_45 = @"Tiles\tennis_court\floor_tennis_court_45";
        public const string TILE_FLOOR_TENNIS_COURT_46 = @"Tiles\tennis_court\floor_tennis_court_46";
        public const string TILE_FLOOR_TENNIS_COURT_47 = @"Tiles\tennis_court\floor_tennis_court_47";
        public const string TILE_FLOOR_TENNIS_COURT_50 = @"Tiles\tennis_court\floor_tennis_court_50";
        public const string TILE_FLOOR_TENNIS_COURT_51 = @"Tiles\tennis_court\floor_tennis_court_51";
        public const string TILE_FLOOR_TENNIS_COURT_52 = @"Tiles\tennis_court\floor_tennis_court_52";
        public const string TILE_FLOOR_TENNIS_COURT_53 = @"Tiles\tennis_court\floor_tennis_court_53";
        public const string TILE_FLOOR_TENNIS_COURT_54 = @"Tiles\tennis_court\floor_tennis_court_54";
        public const string TILE_FLOOR_TENNIS_COURT_55 = @"Tiles\tennis_court\floor_tennis_court_55";
        public const string TILE_FLOOR_TENNIS_COURT_58 = @"Tiles\tennis_court\floor_tennis_court_58";
        public const string TILE_FLOOR_TENNIS_COURT_59 = @"Tiles\tennis_court\floor_tennis_court_59";
        public const string TILE_FLOOR_TENNIS_COURT_60 = @"Tiles\tennis_court\floor_tennis_court_60";
        public const string TILE_FLOOR_TENNIS_COURT_61 = @"Tiles\tennis_court\floor_tennis_court_61";
        public const string TILE_FLOOR_TENNIS_COURT_62 = @"Tiles\tennis_court\floor_tennis_court_62";
        public const string TILE_FLOOR_TENNIS_COURT_63 = @"Tiles\tennis_court\floor_tennis_court_63";
        public const string TILE_FLOOR_TENNIS_COURT_66 = @"Tiles\tennis_court\floor_tennis_court_66";
        public const string TILE_FLOOR_TENNIS_COURT_67 = @"Tiles\tennis_court\floor_tennis_court_67";
        public const string TILE_FLOOR_TENNIS_COURT_68 = @"Tiles\tennis_court\floor_tennis_court_68";
        public const string TILE_FLOOR_TENNIS_COURT_69 = @"Tiles\tennis_court\floor_tennis_court_69";
        public const string TILE_FLOOR_TENNIS_COURT_70 = @"Tiles\tennis_court\floor_tennis_court_70";
        public const string TILE_FLOOR_TENNIS_COURT_71 = @"Tiles\tennis_court\floor_tennis_court_71";
        #endregion
        #region -Basketball court
        public const string TILE_FLOOR_BASKETBALL_COURT_OUTER = @"Tiles\basketball_court\floor_basketball_court_outer";
        public const string TILE_FLOOR_BASKETBALL_COURT_18 = @"Tiles\basketball_court\floor_basketball_court_18";
        public const string TILE_FLOOR_BASKETBALL_COURT_19 = @"Tiles\basketball_court\floor_basketball_court_19";
        public const string TILE_FLOOR_BASKETBALL_COURT_20 = @"Tiles\basketball_court\floor_basketball_court_20";
        public const string TILE_FLOOR_BASKETBALL_COURT_21 = @"Tiles\basketball_court\floor_basketball_court_21";
        public const string TILE_FLOOR_BASKETBALL_COURT_22 = @"Tiles\basketball_court\floor_basketball_court_22";
        public const string TILE_FLOOR_BASKETBALL_COURT_23 = @"Tiles\basketball_court\floor_basketball_court_23";
        public const string TILE_FLOOR_BASKETBALL_COURT_24 = @"Tiles\basketball_court\floor_basketball_court_24";
        public const string TILE_FLOOR_BASKETBALL_COURT_25 = @"Tiles\basketball_court\floor_basketball_court_25";
        public const string TILE_FLOOR_BASKETBALL_COURT_27 = @"Tiles\basketball_court\floor_basketball_court_27";
        public const string TILE_FLOOR_BASKETBALL_COURT_28 = @"Tiles\basketball_court\floor_basketball_court_28";
        public const string TILE_FLOOR_BASKETBALL_COURT_29 = @"Tiles\basketball_court\floor_basketball_court_29";
        public const string TILE_FLOOR_BASKETBALL_COURT_30 = @"Tiles\basketball_court\floor_basketball_court_30";
        public const string TILE_FLOOR_BASKETBALL_COURT_31 = @"Tiles\basketball_court\floor_basketball_court_31";
        public const string TILE_FLOOR_BASKETBALL_COURT_32 = @"Tiles\basketball_court\floor_basketball_court_32";
        public const string TILE_FLOOR_BASKETBALL_COURT_33 = @"Tiles\basketball_court\floor_basketball_court_33";
        public const string TILE_FLOOR_BASKETBALL_COURT_34 = @"Tiles\basketball_court\floor_basketball_court_34";
        public const string TILE_FLOOR_BASKETBALL_COURT_36 = @"Tiles\basketball_court\floor_basketball_court_36";
        public const string TILE_FLOOR_BASKETBALL_COURT_37 = @"Tiles\basketball_court\floor_basketball_court_37";
        public const string TILE_FLOOR_BASKETBALL_COURT_38 = @"Tiles\basketball_court\floor_basketball_court_38";
        public const string TILE_FLOOR_BASKETBALL_COURT_39 = @"Tiles\basketball_court\floor_basketball_court_39";
        public const string TILE_FLOOR_BASKETBALL_COURT_40 = @"Tiles\basketball_court\floor_basketball_court_40";
        public const string TILE_FLOOR_BASKETBALL_COURT_41 = @"Tiles\basketball_court\floor_basketball_court_41";
        public const string TILE_FLOOR_BASKETBALL_COURT_42 = @"Tiles\basketball_court\floor_basketball_court_42";
        public const string TILE_FLOOR_BASKETBALL_COURT_43 = @"Tiles\basketball_court\floor_basketball_court_43";
        public const string TILE_FLOOR_BASKETBALL_COURT_45 = @"Tiles\basketball_court\floor_basketball_court_45";
        public const string TILE_FLOOR_BASKETBALL_COURT_46 = @"Tiles\basketball_court\floor_basketball_court_46";
        public const string TILE_FLOOR_BASKETBALL_COURT_47 = @"Tiles\basketball_court\floor_basketball_court_47";
        public const string TILE_FLOOR_BASKETBALL_COURT_48 = @"Tiles\basketball_court\floor_basketball_court_48";
        public const string TILE_FLOOR_BASKETBALL_COURT_49 = @"Tiles\basketball_court\floor_basketball_court_49";
        public const string TILE_FLOOR_BASKETBALL_COURT_50 = @"Tiles\basketball_court\floor_basketball_court_50";
        public const string TILE_FLOOR_BASKETBALL_COURT_51 = @"Tiles\basketball_court\floor_basketball_court_51";
        public const string TILE_FLOOR_BASKETBALL_COURT_52 = @"Tiles\basketball_court\floor_basketball_court_52";
        public const string TILE_FLOOR_BASKETBALL_COURT_54 = @"Tiles\basketball_court\floor_basketball_court_54";
        public const string TILE_FLOOR_BASKETBALL_COURT_55 = @"Tiles\basketball_court\floor_basketball_court_55";
        public const string TILE_FLOOR_BASKETBALL_COURT_56 = @"Tiles\basketball_court\floor_basketball_court_56";
        public const string TILE_FLOOR_BASKETBALL_COURT_57 = @"Tiles\basketball_court\floor_basketball_court_57";
        public const string TILE_FLOOR_BASKETBALL_COURT_58 = @"Tiles\basketball_court\floor_basketball_court_58";
        public const string TILE_FLOOR_BASKETBALL_COURT_59 = @"Tiles\basketball_court\floor_basketball_court_59";
        public const string TILE_FLOOR_BASKETBALL_COURT_60 = @"Tiles\basketball_court\floor_basketball_court_60";
        public const string TILE_FLOOR_BASKETBALL_COURT_61 = @"Tiles\basketball_court\floor_basketball_court_61";
        public const string TILE_FLOOR_BASKETBALL_COURT_63 = @"Tiles\basketball_court\floor_basketball_court_63";
        public const string TILE_FLOOR_BASKETBALL_COURT_64 = @"Tiles\basketball_court\floor_basketball_court_64";
        public const string TILE_FLOOR_BASKETBALL_COURT_65 = @"Tiles\basketball_court\floor_basketball_court_65";
        public const string TILE_FLOOR_BASKETBALL_COURT_66 = @"Tiles\basketball_court\floor_basketball_court_66";
        public const string TILE_FLOOR_BASKETBALL_COURT_67 = @"Tiles\basketball_court\floor_basketball_court_67";
        public const string TILE_FLOOR_BASKETBALL_COURT_68 = @"Tiles\basketball_court\floor_basketball_court_68";
        public const string TILE_FLOOR_BASKETBALL_COURT_69 = @"Tiles\basketball_court\floor_basketball_court_69";
        public const string TILE_FLOOR_BASKETBALL_COURT_70 = @"Tiles\basketball_court\floor_basketball_court_70";
        #endregion

        public const string TILE_PARKING_ASPHALT_NS = @"Tiles\parking_asphalt_ns";//@@MP (Release 7-3)
        public const string TILE_PARKING_ASPHALT_EW = @"Tiles\parking_asphalt_ew";//@@MP (Release 7-3)
        public const string TILE_ROAD_ASPHALT_NS = @"Tiles\road_asphalt_ns";
        public const string TILE_ROAD_ASPHALT_EW = @"Tiles\road_asphalt_ew";
        public const string TILE_RAIL_ES = @"Tiles\rail_ew";

        public const string TILE_WALL_BRICK = @"Tiles\wall_brick";
        public const string TILE_WALL_CHAR_OFFICE = @"Tiles\wall_char_office";
        public const string TILE_WALL_HOSPITAL = @"Tiles\wall_hospital";
        public const string TILE_WALL_SEWER = @"Tiles\wall_sewer";
        public const string TILE_WALL_STONE = @"Tiles\wall_stone";
        public const string TILE_WALL_LIGHT_BROWN = @"Tiles\wall_light_brown"; //@@MP (Release 4)
        public const string TILE_WALL_ARMY_BASE = @"Tiles\wall_army_base"; //@@MP (Release 6-3)
        public const string TILE_WALL_FUEL_STATION = @"Tiles\wall_fuel_station"; //@@MP (Release 7-3)
        public const string TILE_WALL_WOOD_PLANKS = @"Tiles\wall_wood_planks"; //@@MP (Release 7-3)
        public const string TILE_WALL_CONCRETE = @"Tiles\wall_concrete"; //@@MP (Release 7-3)
        public const string TILE_WALL_PILLAR_CONCRETE = @"Tiles\wall_pillar_concrete"; //@@MP (Release 7-3)
        public const string TILE_WALL_MALL = @"Tiles\wall_mall"; //@@MP (Release 7-3)
        public const string TILE_WALL_RED_CURTAINS = @"Tiles\wall_red_curtains"; //@@MP (Release 7-3)

        //@MP (Release 6-1)
        public const string TILE_FLOOR_POND_CENTER = @"Tiles\floor_pond_center";
        public const string TILE_FLOOR_POND_N_EDGE = @"Tiles\floor_pond_north-edge";
        public const string TILE_FLOOR_POND_NE_CORNER = @"Tiles\floor_pond_ne-corner";
        public const string TILE_FLOOR_POND_E_EDGE = @"Tiles\floor_pond_east-edge";
        public const string TILE_FLOOR_POND_SE_CORNER = @"Tiles\floor_pond_se-corner";
        public const string TILE_FLOOR_POND_S_EDGE = @"Tiles\floor_pond_south-edge";
        public const string TILE_FLOOR_POND_SW_CORNER = @"Tiles\floor_pond_sw-corner";
        public const string TILE_FLOOR_POND_W_EDGE = @"Tiles\floor_pond_west-edge";
        public const string TILE_FLOOR_POND_NW_CORNER = @"Tiles\floor_pond_nw-corner";
        public const string TILE_FLOOR_POND_WATER_COVER = @"Tiles\floor_pond_water_cover";
        #endregion

        #region Tile decorations
        public const string DECO_BLOODIED_FLOOR = @"Tiles\Decoration\bloodied_floor";
        public const string DECO_BLOODIED_WALL = @"Tiles\Decoration\bloodied_wall";
        public const string DECO_BLOODIED_FLOOR_SMALL = @"Tiles\Decoration\bloodied_floor_small"; //@@MP (Release 2)
        public const string DECO_BLOODIED_WALL_SMALL = @"Tiles\Decoration\bloodied_wall_small"; //@@MP (Release 2)
        public const string DECO_ZOMBIE_REMAINS = @"Tiles\Decoration\zombie_remains";
        public const string DECO_SKELETON_REMAINS = @"Tiles\Decoration\skeleton_remains"; //@@MP (Release 2)
        public const string DECO_VOMIT = @"Tiles\Decoration\vomit";
        public const string DECO_RAT_ZOMBIE_REMAINS = @"Tiles\Decoration\rat_zombie_remains"; //@@MP (Release 5-4)

        public const string DECO_POSTERS1 = @"Tiles\Decoration\posters1";
        public const string DECO_POSTERS2 = @"Tiles\Decoration\posters2";
        public const string DECO_TAGS1 = @"Tiles\Decoration\tags1";
        public const string DECO_TAGS2 = @"Tiles\Decoration\tags2";
        public const string DECO_TAGS3 = @"Tiles\Decoration\tags3";
        public const string DECO_TAGS4 = @"Tiles\Decoration\tags4";
        public const string DECO_TAGS5 = @"Tiles\Decoration\tags5";
        public const string DECO_TAGS6 = @"Tiles\Decoration\tags6";
        public const string DECO_TAGS7 = @"Tiles\Decoration\tags7";

        public const string DECO_SHOP_CONSTRUCTION = @"Tiles\Decoration\shop_construction";
        public const string DECO_SHOP_GENERAL_STORE = @"Tiles\Decoration\shop_general_store";
        public const string DECO_SHOP_GROCERY = @"Tiles\Decoration\shop_grocery";
        public const string DECO_SHOP_GUNSHOP = @"Tiles\Decoration\shop_gunshop";
        public const string DECO_SHOP_PHARMACY = @"Tiles\Decoration\shop_pharmacy";
        public const string DECO_SHOP_SPORTSWEAR = @"Tiles\Decoration\shop_sportswear";
        public const string DECO_SHOP_HUNTING = @"Tiles\Decoration\shop_hunting";
        public const string DECO_SHOP_LIQUOR = @"Tiles\Decoration\shop_liquor";//@@MP (Release 4)
        public const string DECO_SHOP_FUEL_STATION = @"Tiles\Decoration\shop_fuel_station";//@@MP (Release 7-3)

        public const string DECO_CHAR_OFFICE = @"Tiles\Decoration\char_office";
        public const string DECO_CHAR_FLOOR_LOGO = @"Tiles\Decoration\char_floor_logo";
        public const string DECO_CHAR_POSTER1 = @"Tiles\Decoration\char_poster1";
        public const string DECO_CHAR_POSTER2 = @"Tiles\Decoration\char_poster2";
        public const string DECO_CHAR_POSTER3 = @"Tiles\Decoration\char_poster3";

        //@@MP (Release 6-3)
        public const string DECO_ARMY_FLOOR_LOGO = @"Tiles\Decoration\army_floor_logo"; //@@MP (Release 6-3)
        public const string DECO_ARMY_POSTER1 = @"Tiles\Decoration\army_poster1";
        public const string DECO_ARMY_POSTER2 = @"Tiles\Decoration\army_poster2";
        public const string DECO_ARMY_POSTER3 = @"Tiles\Decoration\army_poster3";

        //@@MP (Release 4)
        public const string DECO_CHURCH_HANGING1 = @"Tiles\Decoration\hanging_purple";
        public const string DECO_CHURCH_HANGING2 = @"Tiles\Decoration\hanging_red";
        public const string DECO_CHURCH_HANGING3 = @"Tiles\Decoration\hanging_green";
        public const string DECO_CHURCH_HANGING4 = @"Tiles\Decoration\hanging_blue";

        public const string DECO_PLAYER_TAG1 = @"Tiles\Decoration\player_tag";
        public const string DECO_PLAYER_TAG2 = @"Tiles\Decoration\player_tag2";
        public const string DECO_PLAYER_TAG3 = @"Tiles\Decoration\player_tag3";
        public const string DECO_PLAYER_TAG4 = @"Tiles\Decoration\player_tag4";
        public const string DECO_ROGUEDJACK_TAG = @"Tiles\Decoration\roguedjack";

        public const string DECO_SEWER_LADDER = @"Tiles\Decoration\sewer_ladder";
        public const string DECO_SEWER_HOLE = @"Tiles\Decoration\sewer_hole";
        public const string DECO_STAIRS_UP = @"Tiles\Decoration\stairs_up";
        public const string DECO_STAIRS_DOWN = @"Tiles\Decoration\stairs_down";

        public const string DECO_SEWERS_BUILDING = @"Tiles\Decoration\sewers_building";
        public const string DECO_SUBWAY_BUILDING = @"Tiles\Decoration\subway_building";
        public const string DECO_POWER_SIGN_BIG = @"Tiles\Decoration\power_sign_big";
        public const string DECO_POLICE_STATION = @"Tiles\Decoration\police_station";
        public const string DECO_HOSPITAL = @"Tiles\Decoration\hospital";
        public const string DECO_FIRE_STATION = @"Tiles\Decoration\fire_station";//@@MP (Release 7-3)
        public const string DECO_ANIMAL_SHELTER = @"Tiles\Decoration\animal_shelter_sign";//@@MP (Release 7-3)
        public const string DECO_MALL_SIGN_THE = @"Tiles\Decoration\mall_sign_the";//@@MP (Release 7-3)
        public const string DECO_MALL_SIGN_MALL = @"Tiles\Decoration\mall_sign_mall";//@@MP (Release 7-3)

        //@@MP (Release 4)
        public const string DECO_CHURCH = @"Tiles\Decoration\church_sign";
        public const string DECO_LIBRARY = @"Tiles\Decoration\shop_library";
        public const string DECO_MECHANIC = @"Tiles\Decoration\shop_mechanic";
        public const string DECO_JUNKYARD = @"Tiles\Decoration\junkyard";
        public const string DECO_BAR = @"Tiles\Decoration\shop_bar";
        public const string DECO_VELVET_ROPE = @"Tiles\Decoration\velvet_rope";
        public const string DECO_BANK_SIGN = @"Tiles\Decoration\bank_sign";
        public const string DECO_CLINIC_SIGN = @"Tiles\Decoration\clinic_sign";

        //@@MP (Release 2)(Release 6-3)
        public const string DECO_SCORCH_MARK_CENTER_FLOOR = @"Tiles\Decoration\scorched_center_floor"; //will never be on a wall tile, so this is also flat for the floor
        public const string DECO_SCORCH_MARK_INNER_FLOOR = @"Tiles\Decoration\scorched_inner_floor"; //flat to give the appearance of being on a horizontal
        public const string DECO_SCORCH_MARK_OUTER_FLOOR = @"Tiles\Decoration\scorched_outer_floor";
        public const string DECO_SCORCH_MARK_INNER_WALL = @"Tiles\Decoration\scorched_inner_wall"; //slightly tilted to give the appearance of being on a vertical
        public const string DECO_SCORCH_MARK_OUTER_WALL = @"Tiles\Decoration\scorched_outer_wall";

        //@@MP (Release 3)
        public const string DECO_WALL_BRICK_DAMAGED = @"Tiles\Decoration\wall_brick_damaged";
        public const string DECO_WALL_CHAR_OFFICE_DAMAGED = @"Tiles\Decoration\wall_char_office_damaged";
        public const string DECO_WALL_HOSPITAL_DAMAGED = @"Tiles\Decoration\wall_hospital_damaged";
        public const string DECO_WALL_SEWER_DAMAGED = @"Tiles\Decoration\wall_sewer_damaged";
        public const string DECO_WALL_STONE_DAMAGED = @"Tiles\Decoration\wall_stone_damaged";
        public const string DECO_WALL_LIGHT_BROWN_DAMAGED = @"Tiles\Decoration\wall_light_brown_damaged"; //@@MP (Release 4)
        public const string DECO_WALL_ARMY_BASE_DAMAGED = @"Tiles\Decoration\wall_army_base_damaged"; //@@MP (Release 6-3)
        public const string DECO_WALL_FUEL_STATION_DAMAGED = @"Tiles\Decoration\wall_fuel_station_damaged"; //@@MP (Release 7-3)
        public const string DECO_WALL_MALL_DAMAGED = @"Tiles\Decoration\wall_mall_damaged"; //@@MP (Release 7-3)

        public const string DECO_LIT_CANDLE = @"Tiles\Decoration\lit_candle"; //@@MP (Release 7-1)
        public const string DECO_KENNEL = @"Tiles\Decoration\kennel"; //@@MP (Release 7-3)

        //@@MP (Release 7-3)
        public const string DECO_FOOD_COURT_PRICEBOARD1 = @"Tiles\Decoration\food_court_priceboard1";
        public const string DECO_FOOD_COURT_PRICEBOARD2 = @"Tiles\Decoration\food_court_priceboard2";
        public const string DECO_FOOD_COURT_PRICEBOARD3 = @"Tiles\Decoration\food_court_priceboard3";
        public const string DECO_FOOD_COURT_PRICEBOARD4 = @"Tiles\Decoration\food_court_priceboard4";
        public const string DECO_FOOD_COURT_PRICEBOARD5 = @"Tiles\Decoration\food_court_priceboard5";
        public const string DECO_SHOP_BOOKSTORE = @"Tiles\Decoration\shop_bookstore";
        public const string DECO_SHOP_DEALERSHIP = @"Tiles\Decoration\shop_dealership";
        public const string DECO_SHOP_MOBILES = @"Tiles\Decoration\shop_mobiles";
        public const string DECO_SHOP_ELECTRONICS = @"Tiles\Decoration\shop_electronics";
        public const string DECO_SHOP_CLOTHES_STORE = @"Tiles\Decoration\shop_clothes_store";
        public const string DECO_CINEMA_SIGN = @"Tiles\Decoration\cinema_sign";
        public const string DECO_CINEMA1 = @"Tiles\Decoration\cinema1";
        public const string DECO_CINEMA2 = @"Tiles\Decoration\cinema2";
        public const string DECO_GENERIC_OFFICE = @"Tiles\Decoration\generic_offices";
        #endregion

        #region Map objects
        public const string OBJ_TREE = @"MapObjects\tree";
        public const string OBJ_TREE1 = @"MapObjects\tree1";
        public const string OBJ_TREE2 = @"MapObjects\tree2";
        public const string OBJ_TREE3 = @"MapObjects\tree3";
        public const string OBJ_TREE4 = @"MapObjects\tree4";

        //@@MP (Release 4)
        public const string OBJ_PLAIN_TOMBSTONE = @"MapObjects\plain_tombstone";
        public const string OBJ_CROSS_TOMBSTONE = @"MapObjects\cross_tombstone";

        public const string OBJ_WOODEN_DOOR_CLOSED = @"MapObjects\wooden_door_closed";
        public const string OBJ_WOODEN_DOOR_OPEN = @"MapObjects\wooden_door_open";
        public const string OBJ_WOODEN_DOOR_BROKEN = @"MapObjects\wooden_door_broken";

        public const string OBJ_GLASS_DOOR_CLOSED = @"MapObjects\glass_door_closed";
        public const string OBJ_GLASS_DOOR_OPEN = @"MapObjects\glass_door_open";
        public const string OBJ_GLASS_DOOR_BROKEN = @"MapObjects\glass_door_broken";

        public const string OBJ_CHAR_DOOR_CLOSED = @"MapObjects\dark_door_closed";
        public const string OBJ_CHAR_DOOR_OPEN = @"MapObjects\dark_door_open";
        public const string OBJ_CHAR_DOOR_BROKEN = @"MapObjects\dark_door_broken";

        public const string OBJ_WINDOW_CLOSED = @"MapObjects\window_closed";
        public const string OBJ_WINDOW_OPEN = @"MapObjects\window_open";
        public const string OBJ_WINDOW_BROKEN = @"MapObjects\window_broken";

        //@@MP (Release 4)
        public const string OBJ_ROLLER_DOOR_CLOSED = @"MapObjects\roller_door_closed";
        public const string OBJ_ROLLER_DOOR_OPEN = @"MapObjects\roller_door_open";
        public const string OBJ_ROLLER_DOOR_BROKEN = @"MapObjects\roller_door_broken";

        public const string OBJ_PICKET_FENCE_EW = @"MapObjects\picket_fence_EW"; //@@MP - based on alpha 10 (Release 6-1)
        public const string OBJ_PICKET_FENCE_NS_RIGHT = @"MapObjects\picket_fence_NS_right"; //@@MP (Release 7-3)
        public const string OBJ_PICKET_FENCE_NS_LEFT = @"MapObjects\picket_fence_NS_left"; //@@MP (Release 7-3)
        
        public const string OBJ_CHAINWIRE_FENCE = @"MapObjects\chainwire_fence";
        public const string OBJ_CHAINWIRE_GATE_OPEN = @"MapObjects\chainwire_gate_open"; //@@MP (Release 7-3)
        public const string OBJ_CHAINWIRE_GATE_CLOSED = @"MapObjects\chainwire_gate_closed"; //@@MP (Release 7-3)
        public const string OBJ_CHAINWIRE_GATE_BROKEN = @"MapObjects\chainwire_gate_broken"; //@@MP (Release 7-3)

        public const string OBJ_FARM_FENCE_EW = @"MapObjects\farm_fence_EW"; //@@MP (Release 7-3)
        public const string OBJ_FARM_FENCE_NS_RIGHT = @"MapObjects\farm_fence_NS_right"; //@@MP (Release 7-3)
        public const string OBJ_FARM_FENCE_NS_LEFT = @"MapObjects\farm_fence_NS_left"; //@@MP (Release 7-3)

        //@@MP (Release 4)
        public const string OBJ_GRAVEYARD_FENCE = @"MapObjects\graveyard_fence";
        public const string OBJ_CHURCH_PEW = @"MapObjects\church_pew";
        public const string OBJ_LECTERN = @"MapObjects\lectern";
        public const string OBJ_BAR_STOOL = @"MapObjects\bar_stool";
        public const string OBJ_BAR_SHELVES = @"MapObjects\bar_shelves";
        public const string OBJ_WORKBENCH = @"MapObjects\workbench";
        public const string OBJ_BANK_TELLER = @"MapObjects\bank_teller";
        public const string OBJ_BERRY_BUSH = @"MapObjects\berry_bush";
        public const string OBJ_CLINIC_BED = @"MapObjects\clinic_bed";
        public const string OBJ_CLINIC_CUPBOARD = @"MapObjects\clinic_cupboard";
        public const string OBJ_CLINIC_CURTAIN = @"MapObjects\clinic_curtain";
        public const string OBJ_CLINIC_DESK = @"MapObjects\clinic_desk";
        public const string OBJ_CLINIC_MACHINERY = @"MapObjects\clinic_machinery";

        public const string OBJ_CAR1 = @"MapObjects\car1";
        public const string OBJ_CAR2 = @"MapObjects\car2";
        public const string OBJ_CAR3 = @"MapObjects\car3";
        public const string OBJ_CAR4 = @"MapObjects\car4";
        public const string OBJ_VAN = @"MapObjects\van";//@@MP (Release 7-3)

        //@@MP (Release 6-4)
        public const string OBJ_HELICOPTER1 = @"MapObjects\helicopter1";
        public const string OBJ_HELICOPTER2 = @"MapObjects\helicopter2";
        public const string OBJ_HELICOPTER3 = @"MapObjects\helicopter3";

        //@@MP (Release 7-3)
        public const string OBJ_FIRE_TRUCK_EW_BACK = @"MapObjects\fire_truck_EW_back";
        public const string OBJ_FIRE_TRUCK_EW_FRONT = @"MapObjects\fire_truck_EW_front";
        public const string OBJ_FIRE_TRUCK_NS_BACK = @"MapObjects\fire_truck_NS_back";
        public const string OBJ_FIRE_TRUCK_NS_FRONT = @"MapObjects\fire_truck_NS_front";

        public const string OBJ_SHOP_SHELF = @"MapObjects\shop_shelf";
        public const string OBJ_BED = @"MapObjects\bed";
        public const string OBJ_WARDROBE = @"MapObjects\wardrobe";
        public const string OBJ_TABLE = @"MapObjects\table";
        public const string OBJ_FRIDGE = @"MapObjects\fridge";
        public const string OBJ_DRAWER = @"MapObjects\drawer";
        public const string OBJ_CHAIR = @"MapObjects\chair";
        public const string OBJ_NIGHT_TABLE = @"MapObjects\nighttable";
        //@@MP (Release 3)
        public const string OBJ_HOUSE_DRAWERS = @"MapObjects\house_drawers";
        public const string OBJ_HOUSE_SHELVES = @"MapObjects\house_shelves";
        public const string OBJ_PIANO = @"MapObjects\piano";
        public const string OBJ_POTTED_PLANT = @"MapObjects\potted_plant";
        public const string OBJ_TELEVISION = @"MapObjects\television";
        public const string OBJ_STANDING_LAMP = @"MapObjects\standing_lamp";
        public const string OBJ_BOOK_SHELVES = @"MapObjects\bookshelves";
        public const string OBJ_STOVEOVEN = @"MapObjects\stoveoven";
        public const string OBJ_KITCHEN_SINK = @"MapObjects\kitchen_sink";
        public const string OBJ_COUCH = @"MapObjects\couch";
        public const string OBJ_KITCHEN_COUNTER = @"MapObjects\kitchen_counter";
        public const string OBJ_CASH_REGISTER = @"MapObjects\cash_register";

        public const string OBJ_CHAR_CHAIR = @"MapObjects\char_chair";
        public const string OBJ_CHAR_TABLE = @"MapObjects\char_table";
        public const string OBJ_CHAR_DESKTOP = @"MapObjects\char_desktop";//@@MP (Release 3)
        public const string OBJ_CHAR_VAT = @"MapObjects\piped_vat";//@@MP (Release 3)

        public const string OBJ_IRON_BENCH = @"MapObjects\iron_bench";
        public const string OBJ_IRON_DOOR_OPEN = @"MapObjects\iron_door_open";
        public const string OBJ_IRON_DOOR_CLOSED = @"MapObjects\iron_door_closed";
        public const string OBJ_IRON_DOOR_BROKEN = @"MapObjects\iron_door_broken";
        public const string OBJ_IRON_FENCE = @"MapObjects\iron_fence";

        public const string OBJ_BENCH = @"MapObjects\bench";
        public const string OBJ_BARRELS = @"MapObjects\barrels";
        public const string OBJ_JUNK = @"MapObjects\junk";
        public const string OBJ_BOARD = @"MapObjects\announcement_board";
        public const string OBJ_SMALL_WOODEN_FORTIFICATION = @"MapObjects\wooden_small_fortification";
        public const string OBJ_LARGE_WOODEN_FORTIFICATION = @"MapObjects\wooden_large_fortification";

        public const string OBJ_POWERGEN_OFF = @"MapObjects\power_generator_off";
        public const string OBJ_POWERGEN_ON = @"MapObjects\power_generator_on";
        public const string OBJ_FIRE_EXTINGUISHER_REFILLER = @"MapObjects\fire_extinguisher_refiller"; //@@MP (Release 7-1)

        public const string OBJ_GATE_CLOSED = @"MapObjects\gate_closed";
        public const string OBJ_GATE_OPEN = @"MapObjects\gate_open";      

        public const string OBJ_HOSPITAL_BED = @"MapObjects\hospital_bed";
        public const string OBJ_HOSPITAL_CHAIR = @"MapObjects\hospital_chair";
        public const string OBJ_HOSPITAL_NIGHT_TABLE = @"MapObjects\hospital_nighttable";
        public const string OBJ_HOSPITAL_WARDROBE = @"MapObjects\hospital_wardrobe";
        public const string OBJ_HOSPITAL_DOOR_OPEN = @"MapObjects\hospital_door_open";
        public const string OBJ_HOSPITAL_DOOR_CLOSED = @"MapObjects\hospital_door_closed";
        public const string OBJ_HOSPITAL_DOOR_BROKEN = @"MapObjects\hospital_door_broken";

        //@@MP (Release 6-3)
        public const string OBJ_ARMY_RADIO_CUPBOARD = @"MapObjects\army_radio_cupboard";
        public const string OBJ_ARMY_COMPUTER_STATION = @"MapObjects\army_computer_station";
        public const string OBJ_ARMY_BUNK_BED = @"MapObjects\army_bunk_bed";
        public const string OBJ_ARMY_FOOTLOCKER = @"MapObjects\army_footlocker";
        public const string OBJ_ARMY_TABLE = @"MapObjects\army_table";

        //@@MP (Release 6-5)
        public const string OBJ_BANK_SAFE_CLOSED = @"MapObjects\bank_safe_closed";
        public const string OBJ_BANK_SAFE_OPEN = @"MapObjects\bank_safe_open";
        public const string OBJ_BANK_SAFE_OPEN_OWNED = @"MapObjects\bank_safe_open_owned";

        //@@MP (Release 7-3)
        public const string OBJ_FUEL_PRICE_BOARD = @"MapObjects\fuel_price_board";
        public const string OBJ_PEANUT_PLANT = @"MapObjects\peanut_plant";
        public const string OBJ_GRAPE_VINE = @"MapObjects\grape_vine";
        public const string OBJ_TRACTOR = @"MapObjects\tractor";
        public const string OBJ_BASKETBALL_RING = @"MapObjects\basketball_ring";
        public const string OBJ_FUEL_PUMP = @"MapObjects\fuel_pump";
        public const string OBJ_FUEL_PUMP_BROKEN = @"MapObjects\fuel_pump_broken";
        public const string OBJ_TOILET = @"MapObjects\toilet";
        public const string OBJ_BATHROOM_BASIN = @"MapObjects\bathroom_basin";
        public const string OBJ_CLOTHES_WALL1 = @"MapObjects\clothes_wall1";
        public const string OBJ_CLOTHES_WALL2 = @"MapObjects\clothes_wall2";
        public const string OBJ_SHOES_WALL = @"MapObjects\shoes_wall";
        public const string OBJ_MOBILES_TABLE = @"MapObjects\mobiles_table";
        public const string OBJ_LAPTOPS_TABLE = @"MapObjects\laptops_table";
        public const string OBJ_DISHWASHER = @"MapObjects\dishwasher";
        public const string OBJ_WASHING_MACHINE = @"MapObjects\washing_machine";
        public const string OBJ_DRYER = @"MapObjects\dryer";
        public const string OBJ_CINEMA_SEAT = @"MapObjects\cinema_seat";
        public const string OBJ_FOOD_COURT_TABLE = @"MapObjects\food_court_table";
        public const string OBJ_FOOD_COURT_CHAIR = @"MapObjects\food_court_chair";
        public const string OBJ_FOOD_COURT_PALM_TREE = @"MapObjects\food_court_palm_tree";
        public const string OBJ_SUPERMARKET_CHECKOUT = @"MapObjects\supermarket_checkout";
        public const string OBJ_CINEMA_SCREEN = @"MapObjects\cinema_screen";
        public const string OBJ_RAILING = @"MapObjects\railing";
        public const string OBJ_DESKTOP_COMPUTER = @"MapObjects\desktop_computer";

        public const string OBJ_FOOD_COURT_COUNTER1 = @"MapObjects\food_court_counter1";
        public const string OBJ_FOOD_COURT_COUNTER2 = @"MapObjects\food_court_counter2";
        public const string OBJ_FOOD_COURT_COUNTER3 = @"MapObjects\food_court_counter3";
        public const string OBJ_FOOD_COURT_COUNTER4 = @"MapObjects\food_court_counter4";
        public const string OBJ_FOOD_COURT_COUNTER5 = @"MapObjects\food_court_counter5";
        #endregion

        #region Actors
        public const string PLAYER_FOLLOWER = @"Actors\player_follower";
        public const string PLAYER_FOLLOWER_TRUST = @"Actors\player_follower_trust";
        public const string PLAYER_FOLLOWER_BOND = @"Actors\player_follower_bond";

        public const string ACTOR_SKELETON = @"Actors\skeleton";
        public const string ACTOR_RED_EYED_SKELETON = @"Actors\red_eyed_skeleton";
        public const string ACTOR_RED_SKELETON = @"Actors\red_skeleton";
        public const string ACTOR_ZOMBIE = @"Actors\zombie";
        public const string ACTOR_DARK_EYED_ZOMBIE = @"Actors\dark_eyed_zombie";
        public const string ACTOR_DARK_ZOMBIE = @"Actors\dark_zombie";
        public const string ACTOR_MALE_NEOPHYTE = @"Actors\male_neophyte";
        public const string ACTOR_FEMALE_NEOPHYTE = @"Actors\female_neophyte";
        public const string ACTOR_MALE_DISCIPLE = @"Actors\male_disciple";
        public const string ACTOR_FEMALE_DISCIPLE = @"Actors\female_disciple";
        public const string ACTOR_ZOMBIE_MASTER = @"Actors\zombie_master";
        public const string ACTOR_ZOMBIE_LORD = @"Actors\zombie_lord";
        public const string ACTOR_ZOMBIE_PRINCE = @"Actors\zombie_prince";
        public const string ACTOR_RAT_ZOMBIE = @"Actors\rat_zombie";
        public const string ACTOR_SEWERS_THING = @"Actors\sewers_thing";
        public const string ACTOR_JASON_MYERS = @"Actors\jason_myers";
        public const string ACTOR_BIG_BEAR = @"Actors\big_bear";
        public const string ACTOR_FAMU_FATARU = @"Actors\famu_fataru";
        public const string ACTOR_SANTAMAN = @"Actors\santaman";
        public const string ACTOR_ROGUEDJACK = @"Actors\roguedjack";
        public const string ACTOR_DUCKMAN = @"Actors\duckman";
        public const string ACTOR_HANS_VON_HANZ = @"Actors\hans_von_hanz";
        #endregion

        #region Actor decorations
        public const string BLOODIED = @"Actors\Decoration\bloodied";
        //@@MP (Release 5-7)
        public const string MALE_ON_FIRE = @"Actors\Decoration\male_on_fire";
        public const string FEMALE_ON_FIRE = @"Actors\Decoration\female_on_fire";
        public const string ZOMBIE_ON_FIRE = @"Actors\Decoration\zombie_on_fire";
        public const string OTHER_UNDEAD_ON_FIRE = @"Actors\Decoration\other_undead_on_fire";

        public const string MALE_SKIN1 = @"Actors\Decoration\male_skin1";
        public const string MALE_SKIN2 = @"Actors\Decoration\male_skin2";
        public const string MALE_SKIN3 = @"Actors\Decoration\male_skin3";
        public const string MALE_SKIN4 = @"Actors\Decoration\male_skin4";
        public const string MALE_SKIN5 = @"Actors\Decoration\male_skin5";
        public const string MALE_HAIR1 = @"Actors\Decoration\male_hair1";
        public const string MALE_HAIR2 = @"Actors\Decoration\male_hair2";
        public const string MALE_HAIR3 = @"Actors\Decoration\male_hair3";
        public const string MALE_HAIR4 = @"Actors\Decoration\male_hair4";
        public const string MALE_HAIR5 = @"Actors\Decoration\male_hair5";
        public const string MALE_HAIR6 = @"Actors\Decoration\male_hair6";
        public const string MALE_HAIR7 = @"Actors\Decoration\male_hair7";
        public const string MALE_HAIR8 = @"Actors\Decoration\male_hair8";
        public const string MALE_SHIRT1 = @"Actors\Decoration\male_shirt1";
        public const string MALE_SHIRT2 = @"Actors\Decoration\male_shirt2";
        public const string MALE_SHIRT3 = @"Actors\Decoration\male_shirt3";
        public const string MALE_SHIRT4 = @"Actors\Decoration\male_shirt4";
        public const string MALE_SHIRT5 = @"Actors\Decoration\male_shirt5";
        public const string MALE_PANTS1 = @"Actors\Decoration\male_pants1";
        public const string MALE_PANTS2 = @"Actors\Decoration\male_pants2";
        public const string MALE_PANTS3 = @"Actors\Decoration\male_pants3";
        public const string MALE_PANTS4 = @"Actors\Decoration\male_pants4";
        public const string MALE_PANTS5 = @"Actors\Decoration\male_pants5";
        public const string MALE_SHOES1 = @"Actors\Decoration\male_shoes1";
        public const string MALE_SHOES2 = @"Actors\Decoration\male_shoes2";
        public const string MALE_SHOES3 = @"Actors\Decoration\male_shoes3";
        public const string MALE_EYES1 = @"Actors\Decoration\male_eyes1";
        public const string MALE_EYES2 = @"Actors\Decoration\male_eyes2";
        public const string MALE_EYES3 = @"Actors\Decoration\male_eyes3";
        public const string MALE_EYES4 = @"Actors\Decoration\male_eyes4";
        public const string MALE_EYES5 = @"Actors\Decoration\male_eyes5";
        public const string MALE_EYES6 = @"Actors\Decoration\male_eyes6";

        public const string FEMALE_SKIN1 = @"Actors\Decoration\female_skin1";
        public const string FEMALE_SKIN2 = @"Actors\Decoration\female_skin2";
        public const string FEMALE_SKIN3 = @"Actors\Decoration\female_skin3";
        public const string FEMALE_SKIN4 = @"Actors\Decoration\female_skin4";
        public const string FEMALE_SKIN5 = @"Actors\Decoration\female_skin5";
        public const string FEMALE_HAIR1 = @"Actors\Decoration\female_hair1";
        public const string FEMALE_HAIR2 = @"Actors\Decoration\female_hair2";
        public const string FEMALE_HAIR3 = @"Actors\Decoration\female_hair3";
        public const string FEMALE_HAIR4 = @"Actors\Decoration\female_hair4";
        public const string FEMALE_HAIR5 = @"Actors\Decoration\female_hair5";
        public const string FEMALE_HAIR6 = @"Actors\Decoration\female_hair6";
        public const string FEMALE_HAIR7 = @"Actors\Decoration\female_hair7";
        public const string FEMALE_SHIRT1 = @"Actors\Decoration\female_shirt1";
        public const string FEMALE_SHIRT2 = @"Actors\Decoration\female_shirt2";
        public const string FEMALE_SHIRT3 = @"Actors\Decoration\female_shirt3";
        public const string FEMALE_SHIRT4 = @"Actors\Decoration\female_shirt4";
        public const string FEMALE_PANTS1 = @"Actors\Decoration\female_pants1";
        public const string FEMALE_PANTS2 = @"Actors\Decoration\female_pants2";
        public const string FEMALE_PANTS3 = @"Actors\Decoration\female_pants3";
        public const string FEMALE_PANTS4 = @"Actors\Decoration\female_pants4";
        public const string FEMALE_PANTS5 = @"Actors\Decoration\female_pants5";
        public const string FEMALE_SHOES1 = @"Actors\Decoration\female_shoes1";
        public const string FEMALE_SHOES2 = @"Actors\Decoration\female_shoes2";
        public const string FEMALE_SHOES3 = @"Actors\Decoration\female_shoes3";
        public const string FEMALE_EYES1 = @"Actors\Decoration\female_eyes1";
        public const string FEMALE_EYES2 = @"Actors\Decoration\female_eyes2";
        public const string FEMALE_EYES3 = @"Actors\Decoration\female_eyes3";
        public const string FEMALE_EYES4 = @"Actors\Decoration\female_eyes4";
        public const string FEMALE_EYES5 = @"Actors\Decoration\female_eyes5";
        public const string FEMALE_EYES6 = @"Actors\Decoration\female_eyes6";

        public const string ARMY_HELMET = @"Actors\Decoration\army_helmet";
        public const string ARMY_PANTS = @"Actors\Decoration\army_pants";
        public const string ARMY_SHIRT = @"Actors\Decoration\army_shirt";
        public const string ARMY_SHOES = @"Actors\Decoration\army_shoes";

        public const string BIKER_HAIR1 = @"Actors\Decoration\biker_hair1";
        public const string BIKER_HAIR2 = @"Actors\Decoration\biker_hair2";
        public const string BIKER_HAIR3 = @"Actors\Decoration\biker_hair3";
        public const string BIKER_PANTS = @"Actors\Decoration\biker_pants";
        public const string BIKER_SHOES = @"Actors\Decoration\biker_shoes";

        public const string GANGSTA_HAT = @"Actors\Decoration\gangsta_hat";
        public const string GANGSTA_PANTS = @"Actors\Decoration\gangsta_pants";
        public const string GANGSTA_SHIRT = @"Actors\Decoration\gangsta_shirt";

        public const string CHARGUARD_HAIR = @"Actors\Decoration\charguard_hair";
        public const string CHARGUARD_PANTS = @"Actors\Decoration\charguard_pants";

        public const string POLICE_HAT = @"Actors\Decoration\police_hat";
        public const string POLICE_UNIFORM = @"Actors\Decoration\police_uniform";
        public const string POLICE_PANTS = @"Actors\Decoration\police_pants";
        public const string POLICE_SHOES = @"Actors\Decoration\police_shoes";

        public const string BLACKOP_SUIT = @"Actors\Decoration\blackop_suit";

        public const string HOSPITAL_DOCTOR_UNIFORM = @"Actors\Decoration\hospital_doctor_uniform";
        public const string HOSPITAL_NURSE_UNIFORM = @"Actors\Decoration\hospital_nurse_uniform";
        public const string HOSPITAL_PATIENT_UNIFORM = @"Actors\Decoration\hospital_patient_uniform";

        public const string SURVIVOR_MALE_BANDANA = @"Actors\Decoration\survivor_male_bandana";
        public const string SURVIVOR_FEMALE_BANDANA = @"Actors\Decoration\survivor_female_bandana";

        //@@MP (Release 7-3)
        public const string DOG_SKIN1_EAST = @"Actors\Decoration\dog_skin1_east";
        public const string DOG_SKIN2_EAST = @"Actors\Decoration\dog_skin2_east";
        public const string DOG_SKIN3_EAST = @"Actors\Decoration\dog_skin3_east";
        public const string DOG_SKIN1_WEST = @"Actors\Decoration\dog_skin1_west";
        public const string DOG_SKIN2_WEST = @"Actors\Decoration\dog_skin2_west";
        public const string DOG_SKIN3_WEST = @"Actors\Decoration\dog_skin3_west";
        #endregion

        #region Items
        public const string ITEM_SLOT = @"Items\itemslot";
        public const string ITEM_EQUIPPED = @"Items\itemequipped";

        public const string ITEM_AMMO_LIGHT_PISTOL = @"Items\item_ammo_light_pistol";
        public const string ITEM_AMMO_HEAVY_PISTOL = @"Items\item_ammo_heavy_pistol";
        public const string ITEM_AMMO_LIGHT_RIFLE = @"Items\item_ammo_light_rifle";
        public const string ITEM_AMMO_HEAVY_RIFLE = @"Items\item_ammo_heavy_rifle";
        public const string ITEM_AMMO_SHOTGUN = @"Items\item_ammo_shotgun";
        public const string ITEM_AMMO_BOLTS =  @"Items\item_ammo_bolts";
        public const string ITEM_AMMO_NAILS = @"Items\item_ammo_nail_gun"; //@@MP (Release 5-1)
        public const string ITEM_AMMO_PRECISION_RIFLE = @"Items\item_ammo_precision_rifle"; //@@MP (Release 6-6)
        public const string ITEM_AMMO_FUEL = @"Items\item_ammo_fuel"; //@@MP (Release 7-1)

        public const string ITEM_ARMY_BODYARMOR = @"Items\item_army_bodyarmor";
        public const string ITEM_ARMY_PISTOL = @"Items\item_army_pistol";
        public const string ITEM_ARMY_RATION = @"Items\item_army_ration";
        public const string ITEM_ARMY_RIFLE = @"Items\item_army_rifle";
        public const string ITEM_BANDAGES = @"Items\item_bandages";
        public const string ITEM_BARBED_WIRE = @"Items\item_barbed_wire";
        public const string ITEM_BASEBALL_BAT = @"Items\item_baseballbat";
        public const string ITEM_BEAR_TRAP = @"Items\item_bear_trap";
        public const string ITEM_BEER_BOTTLE_BROWN = @"Items\item_beer_bottle_brown"; //@@MP (Release 4)
        public const string ITEM_BEER_BOTTLE_GREEN = @"Items\item_beer_bottle_green"; //@@MP (Release 4)
        public const string ITEM_BEER_CAN_BLUE = @"Items\item_beer_can_blue"; //@@MP (Release 4)
        public const string ITEM_BEER_CAN_RED = @"Items\item_beer_can_red"; //@@MP (Release 4)
        public const string ITEM_BIGBEAR_BAT = @"Items\item_bigbear_bat";
        public const string ITEM_BIG_FLASHLIGHT = @"Items\item_big_flashlight";
        public const string ITEM_BIG_FLASHLIGHT_OUT = @"Items\item_big_flashlight_out";
        public const string ITEM_BINOCULARS = @"Items\item_binoculars"; //@MP (Release 7-1)
        public const string ITEM_BOOK = @"Items\item_book";
        public const string ITEM_BLACKOPS_GPS = @"Items\item_blackops_gps";
        public const string ITEM_C4 = @"Items\item_c4"; //@@MP (Release 6-3)
        public const string ITEM_C4_PRIMED = @"Items\item_c4_primed"; //@@MP (Release 6-3)
        public const string ITEM_CANDLES_BOX = @"Items\item_candles_box"; //@@MP (Release 7-1)
        public const string ITEM_CANNED_FOOD = @"Items\item_canned_food";
        public const string ITEM_CELL_PHONE = @"Items\item_cellphone";
        public const string ITEM_CHAINSAW = @"Items\item_chainsaw"; //@@MP (Release 7-1)
        public const string ITEM_CHAR_DOCUMENT = @"Items\item_CHAR_document"; //@@MP (Release 3)
        public const string ITEM_CHAR_LIGHT_BODYARMOR = @"Items\item_CHAR_light_bodyarmor";
        public const string ITEM_CIGARETTES = @"Items\item_cigarettes"; //@@MP (Release 4)
        public const string ITEM_CROWBAR = @"Items\item_crowbar";
        public const string ITEM_COMBAT_KNIFE = @"Items\item_combat_knife";
        public const string ITEM_DYNAMITE = @"Items\item_dynamite"; //@@MP (Release 4)
        public const string ITEM_DYNAMITE_PRIMED = @"Items\item_dynamite_primed"; //@@MP (Release 4)
        public const string ITEM_EMPTY_CAN = @"Items\item_empty_can";
        public const string ITEM_ENERGY_DRINK = @"Items\item_energy_drink"; //@@MP (Release 7-1)
        public const string ITEM_FAMU_FATARU_KATANA = @"Items\item_famu_fataru_katana";
        public const string ITEM_FIRE_EXTINGUISHER = @"Items\item_fire_extinguisher"; //@@MP (Release 7-1)
        public const string ITEM_FIRE_HAZARD_SUIT = @"Items\item_fire_hazard_suit"; //@@MP (Release 7-1)
        public const string ITEM_FLAMETHROWER = @"Items\item_flamethrower"; //@@MP (Release 7-1)
        public const string ITEM_FLARES_KIT = @"Items\item_flares_kit"; //@@MP (Release 7-1)
        public const string ITEM_FLASHBANG = @"Items\item_flashbang"; //@@MP (Release 7-2)
        public const string ITEM_FLASHBANG_PRIMED = @"Items\item_flashbang_primed"; //@@MP (Release 7-2)
        public const string ITEM_FLASHLIGHT = @"Items\item_flashlight";
        public const string ITEM_FLASHLIGHT_OUT = @"Items\item_flashlight_out";
        public const string ITEM_FREE_ANGELS_JACKET = @"Items\item_free_angels_jacket";
        public const string ITEM_GLOWSTICKS_BOX = @"Items\item_glowsticks_box"; //@@MP (Release 7-1)
        public const string ITEM_GOLF_CLUB = @"Items\item_golfclub";
        public const string ITEM_GRAPES = @"Items\item_grapes"; //@@MP (Release 7-3)
        public const string ITEM_GRENADE = @"Items\item_grenade";
        public const string ITEM_GRENADE_PRIMED = @"Items\item_grenade_primed";
        public const string ITEM_GROCERIES = @"Items\item_groceries";
        public const string ITEM_HANS_VON_HANZ_PISTOL = @"Items\item_hans_von_hanz_pistol";
        public const string ITEM_HELLS_SOULS_JACKET = @"Items\item_hells_souls_jacket";
        public const string ITEM_HOCKEY_STICK = @"Items\item_hockey_stick"; //@@MP (Release 3)
        public const string ITEM_HUGE_HAMMER = @"Items\item_huge_hammer";
        public const string ITEM_HUNTER_VEST = @"Items\item_hunter_vest";
        public const string ITEM_HUNTING_CROSSBOW = @"Items\item_hunting_crossbow";
        public const string ITEM_HUNTING_RIFLE = @"Items\item_hunting_rifle";
        public const string ITEM_IMPROVISED_CLUB = @"Items\item_improvised_club";
        public const string ITEM_IMPROVISED_SPEAR = @"Items\item_improvised_spear";
        public const string ITEM_IRON_GOLF_CLUB = @"Items\item_iron_golfclub";
        public const string ITEM_JASON_MYERS_AXE = @"Items\item_jason_myers_axe";
        public const string ITEM_KOLT_REVOLVER = @"Items\item_kolt_revolver";
        public const string ITEM_LIQUOR_BOTTLE_AMBER = @"Items\item_liquor_bottle_amber"; //@@MP (Release 4)
        public const string ITEM_LIQUOR_BOTTLE_CLEAR = @"Items\item_liquor_bottle_clear"; //@@MP (Release 4)
        public const string ITEM_LIT_FLARE = @"Items\item_lit_flare"; //@@MP (Release 7-1)
        public const string ITEM_LIT_GLOWSTICK = @"Items\item_lit_glowstick"; //@@MP (Release 7-1)
        public const string ITEM_MACHETE = @"Items\item_machete"; //@@MP (Release 3)
        public const string ITEM_MAGAZINE = @"Items\item_magazine";
        public const string ITEM_MATCHES = @"Items\item_matchbox"; //@@MP (Release 7-1)
        public const string ITEM_MATCHES_PRIMED = @"Items\item_match_primed"; //@@MP (Release 7-1)
        public const string ITEM_MEDIKIT = @"Items\item_medikit";
        public const string ITEM_MOLOTOV = @"Items\item_molotov"; //@@MP (Release 4)
        public const string ITEM_MOLOTOV_PRIMED = @"Items\item_molotov_primed"; //@@MP (Release 4)
        public const string ITEM_NAIL_GUN = @"Items\item_nail_gun"; //@@MP (Release 5-1)
        public const string ITEM_NIGHT_VISION = @"Items\item_night_vision"; //@MP (Release 6-3)
        public const string ITEM_PEANUTS = @"Items\item_peanuts"; //@@MP (Release 7-3)
        public const string ITEM_PICKAXE = @"Items\item_pickaxe"; //@@MP (Release 3)
        public const string ITEM_PILLS_ANTIVIRAL = @"Items\item_pills_antiviral";
        public const string ITEM_PILLS_BLUE = @"Items\item_pills_blue";
        public const string ITEM_PILLS_GREEN = @"Items\item_pills_green";
        public const string ITEM_PILLS_SAN = @"Items\item_pills_san";
        public const string ITEM_PIPE_WRENCH = @"Items\item_pipe_wrench"; //@@MP (Release 3)
        public const string ITEM_PISTOL = @"Items\item_pistol";
        public const string ITEM_POLICE_JACKET = @"Items\item_police_jacket";
        public const string ITEM_POLICE_RADIO = @"Items\item_police_radio";
        public const string ITEM_POLICE_RIOT_ARMOR = @"Items\item_police_riot_armor";
        public const string ITEM_POLICE_RIOT_SHIELD = @"Items\item_police_riot_shield"; //@@MP (Release 7-2)
        public const string ITEM_PRECISION_RIFLE = @"Items\item_precision_rifle";
        public const string ITEM_ROGUEDJACK_KEYBOARD = @"Items\item_roguedjack_keyboard";
        public const string ITEM_SANTAMAN_SHOTGUN = @"Items\item_santaman_shotgun";
        public const string ITEM_SHORT_SHOVEL = @"Items\item_short_shovel";
        public const string ITEM_SHOTGUN = @"Items\item_shotgun";
        public const string ITEM_SHOVEL = @"Items\item_shovel";
        public const string ITEM_SIPHON_KIT = @"Items\item_siphon_kit"; //@@MP (Release 7-1)
        public const string ITEM_SLEEPING_BAG = @"Items\item_sleeping_bag"; //@@MP (Release 7-3)
        public const string ITEM_SMALL_HAMMER = @"Items\item_small_hammer";
        public const string ITEM_SMOKE_GRENADE = @"Items\item_smoke_grenade"; //@@MP (Release 7-2)
        public const string ITEM_SMOKE_GRENADE_PRIMED = @"Items\item_smoke_grenade_primed"; //@@MP (Release 7-2)
        public const string ITEM_SNACK_BAR = @"Items\item_snack_bar"; //@@MP (Release 7-1)
        public const string ITEM_SPIKES = @"Items\item_spikes";
        public const string ITEM_SPRAYPAINT = @"Items\item_spraypaint";
        public const string ITEM_SPRAYPAINT2 = @"Items\item_spraypaint2";
        public const string ITEM_SPRAYPAINT3 = @"Items\item_spraypaint3";
        public const string ITEM_SPRAYPAINT4 = @"Items\item_spraypaint4";
        public const string ITEM_STANDARD_AXE = @"Items\item_standard_axe"; //@@MP (Release 3)
        public const string ITEM_STENCH_KILLER = @"Items\item_stench_killer";
        public const string ITEM_STUN_GUN = @"Items\item_stun_gun"; //@@MP (Release 7-2)
        public const string ITEM_SUBWAY_BADGE = @"Items\item_subway_badge";
        public const string ITEM_TENNIS_RACKET = @"Items\item_tennis_racket"; //@@MP (Release 3)
        public const string ITEM_TRUNCHEON = @"Items\item_truncheon";
        public const string ITEM_VEGETABLE_SEEDS = @"Items\item_vegetable_seeds"; //@@MP (Release 5-5)
        public const string ITEM_VEGETABLES = @"Items\item_vegetables"; //@@MP (Release 5-5)
        public const string ITEM_WILD_BERRIES = @"Items\item_wild_berries"; //@@MP (Release 4)
        public const string ITEM_WOODEN_PLANK = @"Items\item_wooden_plank";
        public const string ITEM_ZTRACKER = @"Items\item_ztracker";
        #endregion

        #region Effects
        public const string EFFECT_BARRICADED = @"Effects\barricaded";
        public const string EFFECT_ONFIRE = @"Effects\onFire";
        public const string EFFECT_ROT1_1 = @"Effects\rot1_1";
        public const string EFFECT_ROT1_2 = @"Effects\rot1_2";
        public const string EFFECT_ROT2_1 = @"Effects\rot2_1";
        public const string EFFECT_ROT2_2 = @"Effects\rot2_2";
        public const string EFFECT_ROT3_1 = @"Effects\rot3_1";
        public const string EFFECT_ROT3_2 = @"Effects\rot3_2";
        public const string EFFECT_ROT4_1 = @"Effects\rot4_1";
        public const string EFFECT_ROT4_2 = @"Effects\rot4_2";
        public const string EFFECT_ROT5_1 = @"Effects\rot5_1";
        public const string EFFECT_ROT5_2 = @"Effects\rot5_2";
        public const string EFFECT_FLASHBANG_020 = @"Effects\flashbang_effect_020"; //@@MP (Release 7-2)
        public const string EFFECT_FLASHBANG_040 = @"Effects\flashbang_effect_040"; //@@MP (Release 7-2)
        public const string EFFECT_FLASHBANG_060 = @"Effects\flashbang_effect_060"; //@@MP (Release 7-2)
        public const string EFFECT_FLASHBANG_080 = @"Effects\flashbang_effect_080"; //@@MP (Release 7-2)
        public const string EFFECT_FLASHBANG_100 = @"Effects\flashbang_effect_100"; //@@MP (Release 7-2)
        public const string EFFECT_LIGHT_TINT_CANDLE = @"Effects\light_tint_candle"; //@@MP (Release 7-1)
        public const string EFFECT_LIGHT_TINT_FLARE = @"Effects\light_tint_flare"; //@@MP (Release 7-1)
        public const string EFFECT_LIGHT_TINT_GLOWSTICK = @"Effects\light_tint_glowstick"; //@@MP (Release 7-1)
        public const string EFFECT_NIGHTVISION = @"Effects\nightvision_effect"; //@@MP (Release 7-2)
        public const string EFFECT_SMOKE_SCREEN = @"Effects\smoke_screen"; //@@MP (Release 7-2)
        public const string EFFECT_WEATHER_RAIN1 = @"Effects\weather_rain1";
        public const string EFFECT_WEATHER_RAIN2 = @"Effects\weather_rain2";
        public const string EFFECT_WEATHER_HEAVY_RAIN1 = @"Effects\weather_heavy_rain1";
        public const string EFFECT_WEATHER_HEAVY_RAIN2 = @"Effects\weather_heavy_rain2";
        public const string EFFECT_DISTURBED_LOW = @"Effects\disturbed_effect_low"; //@@MP (Release 7-3)
        public const string EFFECT_DISTURBED_MED = @"Effects\disturbed_effect_med"; //@@MP (Release 7-3)
        public const string EFFECT_DISTURBED_HIGH = @"Effects\disturbed_effect_high"; //@@MP (Release 7-3)
        public const string EFFECT_TIPSY = @"Effects\drunk_effect_tipsy"; //@@MP (Release 7-3)
        public const string EFFECT_DRUNK = @"Effects\drunk_effect_drunk"; //@@MP (Release 7-3)
        public const string EFFECT_HAMMERED = @"Effects\drunk_effect_hammered"; //@@MP (Release 7-3)
        #endregion

        #region Misc
        public const string UNDEF = @"undef";
        public const string MAP_EXIT = @"map_exit";
        public const string MINI_PLAYER_POSITION = @"mini_player_position";
        public const string MINI_PLAYER_TAG1 = @"mini_player_tag";
        public const string MINI_PLAYER_TAG2 = @"mini_player_tag2";
        public const string MINI_PLAYER_TAG3 = @"mini_player_tag3";
        public const string MINI_PLAYER_TAG4 = @"mini_player_tag4";
        public const string MINI_FOLLOWER_POSITION = @"mini_follower_position";
        public const string MINI_UNDEAD_POSITION = @"mini_undead_position";
        public const string MINI_BLACKOPS_POSITION = @"mini_blackops_position";
        public const string MINI_POLICE_POSITION = @"mini_police_position";
        public const string TRACK_FOLLOWER_POSITION = @"track_follower_position";
        public const string TRACK_UNDEAD_POSITION = @"track_undead_position";
        public const string TRACK_BLACKOPS_POSITION = @"track_blackops_position";
        public const string TRACK_POLICE_POSITION = @"track_police_position";

        public const string CORPSE_DRAGGED = @"corpse_dragged";

        public const string ICONS_LEGEND = @"icons_legend"; //@@MP (Release 6-1)
        public const string INSPECTION_MODE_HIGHLIGHT = @"inspection_mode_highlight"; //@@MP (Release 7-1)
        #endregion

        #endregion

        #region Static fields
        const string FOLDER = @"Resources\Images\";
        static readonly Dictionary<string, Image> s_Images = new Dictionary<string, Image>();
        static readonly Dictionary<string, Image> s_GrayLevelImages_Daytime = new Dictionary<string, Image>(); //@@MP (Release 6-2)
        static readonly Dictionary<string, Image> s_GrayLevelImages_NighttimeClear = new Dictionary<string, Image>(); //@@MP (Release 6-2)
        static readonly Dictionary<string, Image> s_GrayLevelImages_NighttimeClouded = new Dictionary<string, Image>(); //@@MP (Release 6-2)
        static readonly Dictionary<string, Image> s_GrayLevelImages_UndergroundNoTorch = new Dictionary<string, Image>(); //@@MP (Release 6-2)
        static readonly Dictionary<string, Image> s_GrayLevelImages_UndergroundLitTorch = new Dictionary<string, Image>(); //@@MP (Release 6-2)
        #endregion

        #region Loading resources
        public static void LoadResources(IRogueUI ui)
        {
            #region Icons
            Notify(ui, "icons...");
            Load(ACTIVITY_CHASING);
            Load(ACTIVITY_CHASING_PLAYER);
            Load(ACTIVITY_TRACKING);
            Load(ACTIVITY_FLEEING);
            Load(ACTIVITY_FLEEING_FROM_EXPLOSIVE);
            Load(ACTIVITY_FOLLOWING);
            Load(ACTIVITY_FOLLOWING_LEADER); // alpha10
            Load(ACTIVITY_FOLLOWING_ORDER);
            Load(ACTIVITY_FOLLOWING_PLAYER);
            Load(ACTIVITY_SLEEPING);

            Load(ICON_TARGET);
            Load(ICON_MELEE_ATTACK);
            Load(ICON_MELEE_MISS);
            Load(ICON_MELEE_DAMAGE);
            Load(ICON_RANGED_ATTACK);
            Load(ICON_RANGED_DAMAGE);
            Load(ICON_RANGED_MISS);
            Load(ICON_KILLED);
            Load(ICON_LEADER);
            Load(ICON_RUNNING);
            Load(ICON_CANT_RUN);
            Load(ICON_CAN_TRADE);
            Load(ICON_OUT_OF_AMMO);
            Load(ICON_OUT_OF_BATTERIES);
            Load(ICON_SLEEP_ALMOST_SLEEPY);
            Load(ICON_SLEEP_SLEEPY);
            Load(ICON_SLEEP_EXHAUSTED);
            Load(ICON_DRUNK); //@@MP (Release 7-1)
            Load(ICON_EXPIRED_FOOD);
            Load(ICON_SPOILED_FOOD);
            Load(ICON_FOOD_ALMOST_HUNGRY);
            Load(ICON_FOOD_HUNGRY);
            Load(ICON_FOOD_STARVING);
            Load(ICON_LINE_BAD);
            Load(ICON_LINE_BLOCKED);
            Load(ICON_LINE_CLEAR);
            Load(ICON_BLAST);
            Load(ICON_HEALING);
            Load(ICON_INCAPACITATED); //@@MP (Release 7-2)
            Load(ICON_IS_TARGET);
            Load(ICON_IS_TARGETTED);
            Load(ICON_IS_TARGETING); // alpha10
            Load(ICON_IS_IN_GROUP);  // alpha10
            Load(ICON_THREAT_DANGER);
            Load(ICON_THREAT_HIGH_DANGER);
            Load(ICON_THREAT_SAFE);
            Load(ICON_SCENT_LIVING);
            Load(ICON_SCENT_ZOMBIEMASTER);
            //Load(ICON_SCENT_LIVING_SUPRESSOR);  // alpha 10 obsolete
            Load(ICON_ODOR_SUPPRESSED);  // alpha10
            Load(ICON_SELF_DEFENCE);
            Load(ICON_INDIRECT_ENEMIES);
            Load(ICON_AGGRESSOR);
            Load(ICON_TRAP_ACTIVATED);
            Load(ICON_TRAP_ACTIVATED_SAFE_GROUP);  // alpha10
            Load(ICON_TRAP_ACTIVATED_SAFE_PLAYER);  // alpha10
            Load(ICON_TRAP_TRIGGERED);
            Load(ICON_TRAP_TRIGGERED_SAFE_GROUP);  // alpha10
            Load(ICON_TRAP_TRIGGERED_SAFE_PLAYER);  // alpha10
            Load(ICON_ROT_ALMOST_HUNGRY);
            Load(ICON_ROT_HUNGRY);
            Load(ICON_ROT_STARVING);
            Load(ICON_SANITY_INSANE);
            Load(ICON_SANITY_DISTURBED);
            Load(ICON_BORING_ITEM);
            Load(ICON_ZGRAB);  // alpha10
            #endregion

            #region Tiles
            Notify(ui, "tiles...");
            Load(TILE_FLOOR_ASPHALT);
            Load(TILE_FLOOR_CONCRETE);
            Load(TILE_FLOOR_GRASS);
            Load(TILE_FLOOR_OFFICE);
            Load(TILE_FLOOR_PLANKS);
            Load(TILE_FLOOR_PLANTED); //@@MP (Release 5-5)
            Load(TILE_FLOOR_SEWER_WATER);
            Load(TILE_FLOOR_SEWER_WATER_ANIM1);
            Load(TILE_FLOOR_SEWER_WATER_ANIM2);
            Load(TILE_FLOOR_SEWER_WATER_ANIM3);
            Load(TILE_FLOOR_SEWER_WATER_COVER);
            Load(TILE_FLOOR_TILES);
            Load(TILE_FLOOR_WALKWAY);
            Load(TILE_FLOOR_WHITE_TILE); //@@MP (Release 7-3)
            //@@MP (Release 4)
            Load(TILE_FLOOR_RED_CARPET);
            Load(TILE_FLOOR_BLUE_CARPET);
            Load(TILE_FLOOR_DIRT);
            //@@MP (Release 6-1)
            Load(TILE_FLOOR_POND_CENTER);
            Load(TILE_FLOOR_POND_N_EDGE);
            Load(TILE_FLOOR_POND_NE_CORNER);
            Load(TILE_FLOOR_POND_E_EDGE);
            Load(TILE_FLOOR_POND_SE_CORNER);
            Load(TILE_FLOOR_POND_S_EDGE);
            Load(TILE_FLOOR_POND_SW_CORNER);
            Load(TILE_FLOOR_POND_W_EDGE);
            Load(TILE_FLOOR_POND_NW_CORNER);
            Load(TILE_FLOOR_POND_WATER_COVER);
            //@@MP (Release 7-3)
            Load(TILE_FLOOR_FOOD_COURT_POOL);
            Load(TILE_FLOOR_POOL_WATER_COVER);
            #region -Tennis court
            Load(TILE_FLOOR_TENNIS_COURT_OUTER);
            Load(TILE_FLOOR_TENNIS_COURT_10);
            Load(TILE_FLOOR_TENNIS_COURT_11);
            Load(TILE_FLOOR_TENNIS_COURT_12);
            Load(TILE_FLOOR_TENNIS_COURT_13);
            Load(TILE_FLOOR_TENNIS_COURT_14);
            Load(TILE_FLOOR_TENNIS_COURT_15);
            Load(TILE_FLOOR_TENNIS_COURT_18);
            Load(TILE_FLOOR_TENNIS_COURT_19);
            Load(TILE_FLOOR_TENNIS_COURT_20);
            Load(TILE_FLOOR_TENNIS_COURT_21);
            Load(TILE_FLOOR_TENNIS_COURT_22);
            Load(TILE_FLOOR_TENNIS_COURT_23);
            Load(TILE_FLOOR_TENNIS_COURT_26);
            Load(TILE_FLOOR_TENNIS_COURT_27);
            Load(TILE_FLOOR_TENNIS_COURT_28);
            Load(TILE_FLOOR_TENNIS_COURT_29);
            Load(TILE_FLOOR_TENNIS_COURT_30);
            Load(TILE_FLOOR_TENNIS_COURT_31);
            Load(TILE_FLOOR_TENNIS_COURT_34);
            Load(TILE_FLOOR_TENNIS_COURT_35);
            Load(TILE_FLOOR_TENNIS_COURT_36);
            Load(TILE_FLOOR_TENNIS_COURT_37);
            Load(TILE_FLOOR_TENNIS_COURT_38);
            Load(TILE_FLOOR_TENNIS_COURT_39);
            Load(TILE_FLOOR_TENNIS_COURT_42);
            Load(TILE_FLOOR_TENNIS_COURT_43);
            Load(TILE_FLOOR_TENNIS_COURT_44);
            Load(TILE_FLOOR_TENNIS_COURT_45);
            Load(TILE_FLOOR_TENNIS_COURT_46);
            Load(TILE_FLOOR_TENNIS_COURT_47);
            Load(TILE_FLOOR_TENNIS_COURT_50);
            Load(TILE_FLOOR_TENNIS_COURT_51);
            Load(TILE_FLOOR_TENNIS_COURT_52);
            Load(TILE_FLOOR_TENNIS_COURT_53);
            Load(TILE_FLOOR_TENNIS_COURT_54);
            Load(TILE_FLOOR_TENNIS_COURT_55);
            Load(TILE_FLOOR_TENNIS_COURT_58);
            Load(TILE_FLOOR_TENNIS_COURT_59);
            Load(TILE_FLOOR_TENNIS_COURT_60);
            Load(TILE_FLOOR_TENNIS_COURT_61);
            Load(TILE_FLOOR_TENNIS_COURT_62);
            Load(TILE_FLOOR_TENNIS_COURT_63);
            Load(TILE_FLOOR_TENNIS_COURT_66);
            Load(TILE_FLOOR_TENNIS_COURT_67);
            Load(TILE_FLOOR_TENNIS_COURT_68);
            Load(TILE_FLOOR_TENNIS_COURT_69);
            Load(TILE_FLOOR_TENNIS_COURT_70);
            Load(TILE_FLOOR_TENNIS_COURT_71);
            #endregion
            #region -Basketball court
            Load(TILE_FLOOR_BASKETBALL_COURT_OUTER);
            Load(TILE_FLOOR_BASKETBALL_COURT_18);
            Load(TILE_FLOOR_BASKETBALL_COURT_19);
            Load(TILE_FLOOR_BASKETBALL_COURT_20);
            Load(TILE_FLOOR_BASKETBALL_COURT_21);
            Load(TILE_FLOOR_BASKETBALL_COURT_22);
            Load(TILE_FLOOR_BASKETBALL_COURT_23);
            Load(TILE_FLOOR_BASKETBALL_COURT_24);
            Load(TILE_FLOOR_BASKETBALL_COURT_25);
            Load(TILE_FLOOR_BASKETBALL_COURT_27);
            Load(TILE_FLOOR_BASKETBALL_COURT_28);
            Load(TILE_FLOOR_BASKETBALL_COURT_29);
            Load(TILE_FLOOR_BASKETBALL_COURT_30);
            Load(TILE_FLOOR_BASKETBALL_COURT_31);
            Load(TILE_FLOOR_BASKETBALL_COURT_32);
            Load(TILE_FLOOR_BASKETBALL_COURT_33);
            Load(TILE_FLOOR_BASKETBALL_COURT_34);
            Load(TILE_FLOOR_BASKETBALL_COURT_36);
            Load(TILE_FLOOR_BASKETBALL_COURT_37);
            Load(TILE_FLOOR_BASKETBALL_COURT_38);
            Load(TILE_FLOOR_BASKETBALL_COURT_39);
            Load(TILE_FLOOR_BASKETBALL_COURT_40);
            Load(TILE_FLOOR_BASKETBALL_COURT_41);
            Load(TILE_FLOOR_BASKETBALL_COURT_42);
            Load(TILE_FLOOR_BASKETBALL_COURT_43);
            Load(TILE_FLOOR_BASKETBALL_COURT_45);
            Load(TILE_FLOOR_BASKETBALL_COURT_46);
            Load(TILE_FLOOR_BASKETBALL_COURT_47);
            Load(TILE_FLOOR_BASKETBALL_COURT_48);
            Load(TILE_FLOOR_BASKETBALL_COURT_49);
            Load(TILE_FLOOR_BASKETBALL_COURT_50);
            Load(TILE_FLOOR_BASKETBALL_COURT_51);
            Load(TILE_FLOOR_BASKETBALL_COURT_52);
            Load(TILE_FLOOR_BASKETBALL_COURT_54);
            Load(TILE_FLOOR_BASKETBALL_COURT_55);
            Load(TILE_FLOOR_BASKETBALL_COURT_56);
            Load(TILE_FLOOR_BASKETBALL_COURT_57);
            Load(TILE_FLOOR_BASKETBALL_COURT_58);
            Load(TILE_FLOOR_BASKETBALL_COURT_59);
            Load(TILE_FLOOR_BASKETBALL_COURT_60);
            Load(TILE_FLOOR_BASKETBALL_COURT_61);
            Load(TILE_FLOOR_BASKETBALL_COURT_63);
            Load(TILE_FLOOR_BASKETBALL_COURT_64);
            Load(TILE_FLOOR_BASKETBALL_COURT_65);
            Load(TILE_FLOOR_BASKETBALL_COURT_66);
            Load(TILE_FLOOR_BASKETBALL_COURT_67);
            Load(TILE_FLOOR_BASKETBALL_COURT_68);
            Load(TILE_FLOOR_BASKETBALL_COURT_69);
            Load(TILE_FLOOR_BASKETBALL_COURT_70);
            #endregion

            Load(TILE_PARKING_ASPHALT_NS);//@@MP (Release 7-3)
            Load(TILE_PARKING_ASPHALT_EW);//@@MP (Release 7-3)
            Load(TILE_ROAD_ASPHALT_NS);
            Load(TILE_ROAD_ASPHALT_EW);
            Load(TILE_RAIL_ES);

            Load(TILE_WALL_BRICK);
            Load(TILE_WALL_CHAR_OFFICE);
            Load(TILE_WALL_HOSPITAL);
            Load(TILE_WALL_SEWER);
            Load(TILE_WALL_STONE);
            Load(TILE_WALL_LIGHT_BROWN); //@@MP (Release 4)
            Load(TILE_WALL_ARMY_BASE); //@@MP (Release 6-3)
            Load(TILE_WALL_FUEL_STATION); //@@MP (Release 7-3)
            Load(TILE_WALL_WOOD_PLANKS); //@@MP (Release 7-3)
            Load(TILE_WALL_CONCRETE); //@@MP (Release 7-3)
            Load(TILE_WALL_PILLAR_CONCRETE); //@@MP (Release 7-3)
            Load(TILE_WALL_MALL); //@@MP (Release 7-3)
            Load(TILE_WALL_RED_CURTAINS); //@@MP (Release 7-3)
            #endregion

            #region Tile decorations
            Notify(ui, "tile decorations...");
            Load(DECO_BLOODIED_FLOOR);
            Load(DECO_BLOODIED_WALL);
            Load(DECO_BLOODIED_FLOOR_SMALL); //@@MP (Release 2)
            Load(DECO_BLOODIED_WALL_SMALL); //@@MP (Release 2)
            Load(DECO_ZOMBIE_REMAINS);
            Load(DECO_SKELETON_REMAINS); //@@MP (Release 2)
            Load(DECO_VOMIT);
            Load(DECO_RAT_ZOMBIE_REMAINS); //@@MP (Release 5-4)

            Load(DECO_POSTERS1);
            Load(DECO_POSTERS2);
            Load(DECO_TAGS1);
            Load(DECO_TAGS2);
            Load(DECO_TAGS3);
            Load(DECO_TAGS4);
            Load(DECO_TAGS5);
            Load(DECO_TAGS6);
            Load(DECO_TAGS7);

            Load(DECO_SHOP_CONSTRUCTION);
            Load(DECO_SHOP_GENERAL_STORE);
            Load(DECO_SHOP_GROCERY);
            Load(DECO_SHOP_GUNSHOP);
            Load(DECO_SHOP_PHARMACY);
            Load(DECO_SHOP_SPORTSWEAR);
            Load(DECO_SHOP_HUNTING);
            Load(DECO_SHOP_LIQUOR); //@@MP (Release 4)
            Load(DECO_SHOP_FUEL_STATION); //@@MP (Release 7-3)

            Load(DECO_CHAR_OFFICE);
            Load(DECO_CHAR_FLOOR_LOGO);
            Load(DECO_CHAR_POSTER1);
            Load(DECO_CHAR_POSTER2);
            Load(DECO_CHAR_POSTER3);

            //@@MP (Release 6-3)
            Load(DECO_ARMY_FLOOR_LOGO);
            Load(DECO_ARMY_POSTER1);
            Load(DECO_ARMY_POSTER2);
            Load(DECO_ARMY_POSTER3);

            //@@MP (Release 4)
            Load(DECO_CHURCH_HANGING1);
            Load(DECO_CHURCH_HANGING2);
            Load(DECO_CHURCH_HANGING3);
            Load(DECO_CHURCH_HANGING4);

            Load(DECO_PLAYER_TAG1);
            Load(DECO_PLAYER_TAG2);
            Load(DECO_PLAYER_TAG3);
            Load(DECO_PLAYER_TAG4);
            Load(DECO_ROGUEDJACK_TAG);

            Load(DECO_SEWER_LADDER);
            Load(DECO_SEWER_HOLE);
            Load(DECO_SEWERS_BUILDING);
            Load(DECO_SUBWAY_BUILDING);

            Load(DECO_STAIRS_DOWN);
            Load(DECO_STAIRS_UP);

            Load(DECO_POWER_SIGN_BIG);

            Load(DECO_POLICE_STATION);
            Load(DECO_HOSPITAL);
            Load(DECO_FIRE_STATION); //@@MP (Release 7-3)
            Load(DECO_ANIMAL_SHELTER); //@@MP (Release 7-3)
            Load(DECO_MALL_SIGN_THE); //@@MP (Release 7-3)
            Load(DECO_MALL_SIGN_MALL); //@@MP (Release 7-3)
            //@@MP (Release 4)
            Load(DECO_CHURCH);
            Load(DECO_MECHANIC);
            Load(DECO_LIBRARY);
            Load(DECO_JUNKYARD);
            Load(DECO_BAR);
            Load(DECO_VELVET_ROPE);
            Load(DECO_BANK_SIGN);
            Load(DECO_CLINIC_SIGN);

            //@@MP (Release 2)(Release 6-3)
            Load(DECO_SCORCH_MARK_CENTER_FLOOR);
            Load(DECO_SCORCH_MARK_INNER_FLOOR);
            Load(DECO_SCORCH_MARK_OUTER_FLOOR);
            Load(DECO_SCORCH_MARK_INNER_WALL);
            Load(DECO_SCORCH_MARK_OUTER_WALL);

            //@@MP (Release 3)
            Load(DECO_WALL_BRICK_DAMAGED);
            Load(DECO_WALL_CHAR_OFFICE_DAMAGED);
            Load(DECO_WALL_HOSPITAL_DAMAGED);
            Load(DECO_WALL_SEWER_DAMAGED);
            Load(DECO_WALL_STONE_DAMAGED);
            Load(DECO_WALL_LIGHT_BROWN_DAMAGED); //@@MP (Release 4)
            Load(DECO_WALL_ARMY_BASE_DAMAGED); //@@MP (Release 6-3)
            Load(DECO_WALL_FUEL_STATION_DAMAGED); //@@MP (Release 7-3)
            Load(DECO_WALL_MALL_DAMAGED); //@@MP (Release 7-3)

            Load(DECO_LIT_CANDLE); //@@MP (Release 7-1)
            Load(DECO_KENNEL); //@@MP (Release 7-3)

            //@@MP (Release 7-3)
            Load(DECO_FOOD_COURT_PRICEBOARD1);
            Load(DECO_FOOD_COURT_PRICEBOARD2);
            Load(DECO_FOOD_COURT_PRICEBOARD3);
            Load(DECO_FOOD_COURT_PRICEBOARD4);
            Load(DECO_FOOD_COURT_PRICEBOARD5);
            Load(DECO_SHOP_BOOKSTORE);
            Load(DECO_SHOP_MOBILES);
            Load(DECO_SHOP_DEALERSHIP);
            Load(DECO_SHOP_ELECTRONICS);
            Load(DECO_SHOP_CLOTHES_STORE);
            Load(DECO_CINEMA_SIGN);
            Load(DECO_CINEMA1);
            Load(DECO_CINEMA2);
            Load(DECO_GENERIC_OFFICE);
            #endregion

            #region Map objects
            Notify(ui, "map objects...");
            Load(OBJ_TREE);
            Load(OBJ_TREE1);
            Load(OBJ_TREE2);
            Load(OBJ_TREE3);
            Load(OBJ_TREE4);
            //@@MP (Release 4)
            Load(OBJ_PLAIN_TOMBSTONE);
            Load(OBJ_CROSS_TOMBSTONE);

            Load(OBJ_WOODEN_DOOR_CLOSED);
            Load(OBJ_WOODEN_DOOR_OPEN);
            Load(OBJ_WOODEN_DOOR_BROKEN);

            Load(OBJ_GLASS_DOOR_CLOSED);
            Load(OBJ_GLASS_DOOR_OPEN);
            Load(OBJ_GLASS_DOOR_BROKEN);

            Load(OBJ_CHAR_DOOR_BROKEN);
            Load(OBJ_CHAR_DOOR_CLOSED);
            Load(OBJ_CHAR_DOOR_OPEN);

            Load(OBJ_WINDOW_CLOSED);
            Load(OBJ_WINDOW_OPEN);
            Load(OBJ_WINDOW_BROKEN);

            //@@MP (Release 4)
            Load(OBJ_ROLLER_DOOR_CLOSED);
            Load(OBJ_ROLLER_DOOR_OPEN);
            Load(OBJ_ROLLER_DOOR_BROKEN);

            Load(OBJ_PICKET_FENCE_EW); //@@MP - based on alpha10 (Release 6-1)
            Load(OBJ_PICKET_FENCE_NS_RIGHT); //@@MP (Release 7-3)
            Load(OBJ_PICKET_FENCE_NS_LEFT); //@@MP (Release 7-3)
            Load(OBJ_BENCH);
            Load(OBJ_CHAINWIRE_FENCE);
            Load(OBJ_CHAINWIRE_GATE_OPEN); //@@MP (Release 7-3)
            Load(OBJ_CHAINWIRE_GATE_CLOSED); //@@MP (Release 7-3)
            Load(OBJ_CHAINWIRE_GATE_BROKEN); //@@MP (Release 7-3)
            Load(OBJ_FARM_FENCE_EW); //@@MP (Release 7-3)
            Load(OBJ_FARM_FENCE_NS_LEFT); //@@MP (Release 7-3)
            Load(OBJ_FARM_FENCE_NS_RIGHT); //@@MP (Release 7-3)
            //@@MP (Release 4)
            Load(OBJ_GRAVEYARD_FENCE);
            Load(OBJ_CHURCH_PEW);
            Load(OBJ_LECTERN);
            Load(OBJ_BAR_STOOL);
            Load(OBJ_BAR_SHELVES);
            Load(OBJ_WORKBENCH);
            Load(OBJ_BANK_TELLER);
            Load(OBJ_BERRY_BUSH);
            Load(OBJ_CLINIC_BED);
            Load(OBJ_CLINIC_CUPBOARD);
            Load(OBJ_CLINIC_CURTAIN);
            Load(OBJ_CLINIC_DESK);
            Load(OBJ_CLINIC_MACHINERY);

            Load(OBJ_CAR1);
            Load(OBJ_CAR2);
            Load(OBJ_CAR3);
            Load(OBJ_CAR4);
            Load(OBJ_VAN); //@@MP (Release 7-3)

            //@@MP (Release 6-4)
            Load(OBJ_HELICOPTER1);
            Load(OBJ_HELICOPTER2);
            Load(OBJ_HELICOPTER3);

            //@@MP (Release 7-3)
            Load(OBJ_FIRE_TRUCK_EW_BACK);
            Load(OBJ_FIRE_TRUCK_EW_FRONT);
            Load(OBJ_FIRE_TRUCK_NS_BACK);
            Load(OBJ_FIRE_TRUCK_NS_FRONT);

            Load(OBJ_SHOP_SHELF);
            Load(OBJ_BED);
            Load(OBJ_WARDROBE);
            Load(OBJ_TABLE);
            Load(OBJ_FRIDGE);
            Load(OBJ_DRAWER);
            Load(OBJ_CHAIR);
            Load(OBJ_NIGHT_TABLE);
            //@@MP (Release 3)
            Load(OBJ_HOUSE_DRAWERS);
            Load(OBJ_HOUSE_SHELVES);
            Load(OBJ_PIANO);
            Load(OBJ_POTTED_PLANT);
            Load(OBJ_TELEVISION);
            Load(OBJ_STANDING_LAMP);
            Load(OBJ_BOOK_SHELVES);
            Load(OBJ_STOVEOVEN);
            Load(OBJ_KITCHEN_SINK);
            Load(OBJ_COUCH);
            Load(OBJ_KITCHEN_COUNTER);
            Load(OBJ_CASH_REGISTER);

            Load(OBJ_CHAR_CHAIR);
            Load(OBJ_CHAR_TABLE);
            Load(OBJ_CHAR_DESKTOP);//@@MP (Release 3)
            Load(OBJ_CHAR_VAT);//@@MP (Release 3)

            Load(OBJ_IRON_BENCH);
            Load(OBJ_IRON_FENCE);
            Load(OBJ_IRON_DOOR_BROKEN);
            Load(OBJ_IRON_DOOR_CLOSED);
            Load(OBJ_IRON_DOOR_OPEN);

            Load(OBJ_BARRELS);
            Load(OBJ_JUNK);
            Load(OBJ_BOARD);
            Load(OBJ_SMALL_WOODEN_FORTIFICATION);
            Load(OBJ_LARGE_WOODEN_FORTIFICATION);

            Load(OBJ_POWERGEN_OFF);
            Load(OBJ_POWERGEN_ON);
            Load(OBJ_FIRE_EXTINGUISHER_REFILLER); //@@MP (Release 7-1)

            Load(OBJ_GATE_CLOSED);
            Load(OBJ_GATE_OPEN);

            Load(OBJ_HOSPITAL_BED);
            Load(OBJ_HOSPITAL_CHAIR);
            Load(OBJ_HOSPITAL_DOOR_BROKEN);
            Load(OBJ_HOSPITAL_DOOR_CLOSED);
            Load(OBJ_HOSPITAL_DOOR_OPEN);
            Load(OBJ_HOSPITAL_NIGHT_TABLE);
            Load(OBJ_HOSPITAL_WARDROBE);

            //@@MP (Release 6-4)
            Load(OBJ_ARMY_RADIO_CUPBOARD);
            Load(OBJ_ARMY_COMPUTER_STATION);
            Load(OBJ_ARMY_BUNK_BED);
            Load(OBJ_ARMY_FOOTLOCKER);
            Load(OBJ_ARMY_TABLE);

            //@@MP (Release 6-5)
            Load(OBJ_BANK_SAFE_CLOSED);
            Load(OBJ_BANK_SAFE_OPEN);
            Load(OBJ_BANK_SAFE_OPEN_OWNED);

            //@@MP (Release 7-3)
            Load(OBJ_FUEL_PRICE_BOARD);
            Load(OBJ_PEANUT_PLANT);
            Load(OBJ_GRAPE_VINE);
            Load(OBJ_TRACTOR);
            Load(OBJ_BASKETBALL_RING);
            Load(OBJ_FUEL_PUMP);
            Load(OBJ_FUEL_PUMP_BROKEN);
            Load(OBJ_TOILET);
            Load(OBJ_BATHROOM_BASIN);
            Load(OBJ_CLOTHES_WALL1);
            Load(OBJ_CLOTHES_WALL2);
            Load(OBJ_SHOES_WALL);
            Load(OBJ_MOBILES_TABLE);
            Load(OBJ_LAPTOPS_TABLE);
            Load(OBJ_DISHWASHER);
            Load(OBJ_WASHING_MACHINE);
            Load(OBJ_DRYER);
            Load(OBJ_CINEMA_SEAT);
            Load(OBJ_FOOD_COURT_CHAIR);
            Load(OBJ_FOOD_COURT_TABLE);
            Load(OBJ_FOOD_COURT_PALM_TREE);
            Load(OBJ_SUPERMARKET_CHECKOUT);
            Load(OBJ_CINEMA_SCREEN);
            Load(OBJ_RAILING);
            Load(OBJ_DESKTOP_COMPUTER);

            Load(OBJ_FOOD_COURT_COUNTER1);
            Load(OBJ_FOOD_COURT_COUNTER2);
            Load(OBJ_FOOD_COURT_COUNTER3);
            Load(OBJ_FOOD_COURT_COUNTER4);
            Load(OBJ_FOOD_COURT_COUNTER5);
            #endregion

            #region Actors
            Notify(ui, "actors...");
            Load(PLAYER_FOLLOWER);
            Load(PLAYER_FOLLOWER_TRUST);
            Load(PLAYER_FOLLOWER_BOND);

            Load(ACTOR_SKELETON);
            Load(ACTOR_RED_EYED_SKELETON);
            Load(ACTOR_RED_SKELETON);
            Load(ACTOR_ZOMBIE);
            Load(ACTOR_DARK_EYED_ZOMBIE);
            Load(ACTOR_DARK_ZOMBIE);
            Load(ACTOR_MALE_NEOPHYTE);
            Load(ACTOR_FEMALE_NEOPHYTE);
            Load(ACTOR_MALE_DISCIPLE);
            Load(ACTOR_FEMALE_DISCIPLE);
            Load(ACTOR_ZOMBIE_MASTER);
            Load(ACTOR_ZOMBIE_LORD);
            Load(ACTOR_ZOMBIE_PRINCE);
            Load(ACTOR_RAT_ZOMBIE);
            Load(ACTOR_SEWERS_THING);
            Load(ACTOR_JASON_MYERS);
            Load(ACTOR_BIG_BEAR);
            Load(ACTOR_FAMU_FATARU);
            Load(ACTOR_SANTAMAN);
            Load(ACTOR_ROGUEDJACK);
            Load(ACTOR_DUCKMAN);
            Load(ACTOR_HANS_VON_HANZ);
            #endregion

            #region Actor decorations
            Notify(ui, "actor decorations...");

            Load(BLOODIED);
            //@@MP (Release 5-7)
            Load(MALE_ON_FIRE);
            Load(FEMALE_ON_FIRE);
            Load(ZOMBIE_ON_FIRE);
            Load(OTHER_UNDEAD_ON_FIRE);

            Load(MALE_SKIN1);
            Load(MALE_SKIN2);
            Load(MALE_SKIN3);
            Load(MALE_SKIN4);
            Load(MALE_SKIN5);
            Load(MALE_SHIRT1);
            Load(MALE_SHIRT2);
            Load(MALE_SHIRT3);
            Load(MALE_SHIRT4);
            Load(MALE_SHIRT5);
            Load(MALE_HAIR1);
            Load(MALE_HAIR2);
            Load(MALE_HAIR3);
            Load(MALE_HAIR4);
            Load(MALE_HAIR5);
            Load(MALE_HAIR6);
            Load(MALE_HAIR7);
            Load(MALE_HAIR8);
            Load(MALE_PANTS1);
            Load(MALE_PANTS2);
            Load(MALE_PANTS3);
            Load(MALE_PANTS4);
            Load(MALE_PANTS5);
            Load(MALE_SHOES1);
            Load(MALE_SHOES2);
            Load(MALE_SHOES3);
            Load(MALE_EYES1);
            Load(MALE_EYES2);
            Load(MALE_EYES3);
            Load(MALE_EYES4);
            Load(MALE_EYES5);
            Load(MALE_EYES6);

            Load(FEMALE_SKIN1);
            Load(FEMALE_SKIN2);
            Load(FEMALE_SKIN3);
            Load(FEMALE_SKIN4);
            Load(FEMALE_SKIN5);
            Load(FEMALE_SHIRT1);
            Load(FEMALE_SHIRT2);
            Load(FEMALE_SHIRT3);
            Load(FEMALE_SHIRT4);
            Load(FEMALE_HAIR1);
            Load(FEMALE_HAIR2);
            Load(FEMALE_HAIR3);
            Load(FEMALE_HAIR4);
            Load(FEMALE_HAIR5);
            Load(FEMALE_HAIR6);
            Load(FEMALE_HAIR7);
            Load(FEMALE_PANTS1);
            Load(FEMALE_PANTS2);
            Load(FEMALE_PANTS3);
            Load(FEMALE_PANTS4);
            Load(FEMALE_PANTS5);
            Load(FEMALE_SHOES1);
            Load(FEMALE_SHOES2);
            Load(FEMALE_SHOES3);
            Load(FEMALE_EYES1);
            Load(FEMALE_EYES2);
            Load(FEMALE_EYES3);
            Load(FEMALE_EYES4);
            Load(FEMALE_EYES5);
            Load(FEMALE_EYES6);

            Load(ARMY_HELMET);
            Load(ARMY_PANTS);
            Load(ARMY_SHIRT);
            Load(ARMY_SHOES);

            Load(BIKER_HAIR1);
            Load(BIKER_HAIR2);
            Load(BIKER_HAIR3);
            Load(BIKER_PANTS);
            Load(BIKER_SHOES);

            Load(GANGSTA_HAT);
            Load(GANGSTA_PANTS);
            Load(GANGSTA_SHIRT);

            Load(CHARGUARD_HAIR);
            Load(CHARGUARD_PANTS);

            Load(POLICE_HAT);
            Load(POLICE_PANTS);
            Load(POLICE_SHOES);
            Load(POLICE_UNIFORM);

            Load(BLACKOP_SUIT);

            Load(HOSPITAL_DOCTOR_UNIFORM);
            Load(HOSPITAL_NURSE_UNIFORM);
            Load(HOSPITAL_PATIENT_UNIFORM);

            Load(SURVIVOR_FEMALE_BANDANA);
            Load(SURVIVOR_MALE_BANDANA);

            Load(DOG_SKIN1_EAST);
            Load(DOG_SKIN2_EAST);
            Load(DOG_SKIN3_EAST);
            Load(DOG_SKIN1_WEST);
            Load(DOG_SKIN2_WEST);
            Load(DOG_SKIN3_WEST);
            #endregion

            #region Items
            Notify(ui, "items...");
            Load(ITEM_SLOT);
            Load(ITEM_EQUIPPED);

            Load(ITEM_AMMO_BOLTS);
            Load(ITEM_AMMO_FUEL); //@@MP (Release 7-1)
            Load(ITEM_AMMO_HEAVY_PISTOL);
            Load(ITEM_AMMO_HEAVY_RIFLE);
            Load(ITEM_AMMO_LIGHT_PISTOL);
            Load(ITEM_AMMO_LIGHT_RIFLE);
            Load(ITEM_AMMO_NAILS); //@@MP (Release 5-1)
            Load(ITEM_AMMO_PRECISION_RIFLE); //@@MP (Release 6-6)
            Load(ITEM_AMMO_SHOTGUN);
            Load(ITEM_ARMY_BODYARMOR);
            Load(ITEM_ARMY_PISTOL);
            Load(ITEM_ARMY_RATION);
            Load(ITEM_ARMY_RIFLE);
            Load(ITEM_BANDAGES);
            Load(ITEM_BARBED_WIRE);
            Load(ITEM_BASEBALL_BAT);
            Load(ITEM_BEAR_TRAP);
            Load(ITEM_BEER_BOTTLE_BROWN); //@@MP (Release 4)
            Load(ITEM_BEER_BOTTLE_GREEN); //@@MP (Release 4)
            Load(ITEM_BEER_CAN_BLUE); //@@MP (Release 4)
            Load(ITEM_BEER_CAN_RED); //@@MP (Release 4)
            Load(ITEM_BIGBEAR_BAT);
            Load(ITEM_BIG_FLASHLIGHT);
            Load(ITEM_BIG_FLASHLIGHT_OUT);
            Load(ITEM_BINOCULARS); //@@MP (Release 7-1)
            Load(ITEM_BLACKOPS_GPS);
            Load(ITEM_BOOK);
            Load(ITEM_C4); //@@MP (Release 6-3)
            Load(ITEM_C4_PRIMED); //@@MP (Release 6-3)
            Load(ITEM_CANDLES_BOX); //@@MP (Release 7-1)
            Load(ITEM_CANNED_FOOD);
            Load(ITEM_CELL_PHONE);
            Load(ITEM_CHAINSAW); //@@MP (Release 7-1)
            Load(ITEM_CHAR_DOCUMENT); //@@MP (Release 3)
            Load(ITEM_CHAR_LIGHT_BODYARMOR);
            Load(ITEM_CIGARETTES); //@@MP (Release 4)
            Load(ITEM_COMBAT_KNIFE);
            Load(ITEM_CROWBAR);
            Load(ITEM_DYNAMITE); //@@MP (Release 4)
            Load(ITEM_DYNAMITE_PRIMED); //@@MP (Release 4)
            Load(ITEM_EMPTY_CAN);
            Load(ITEM_ENERGY_DRINK); //@@MP (Release 7-1)
            Load(ITEM_FAMU_FATARU_KATANA);
            Load(ITEM_FIRE_EXTINGUISHER); //@@MP (Release 7-1)
            Load(ITEM_FIRE_HAZARD_SUIT); //@@MP (Release 7-1)
            Load(ITEM_FLAMETHROWER); //@@MP (Release 7-1)
            Load(ITEM_FLARES_KIT); //@@MP (Release 7-1)
            Load(ITEM_FLASHBANG); //@@MP (Release 7-2)
            Load(ITEM_FLASHBANG_PRIMED); //@@MP (Release 7-2)
            Load(ITEM_FLASHLIGHT);
            Load(ITEM_FLASHLIGHT_OUT);
            Load(ITEM_FREE_ANGELS_JACKET);
            Load(ITEM_GLOWSTICKS_BOX); //@@MP (Release 7-1)
            Load(ITEM_GOLF_CLUB);
            Load(ITEM_GRAPES); //@@MP (Release 7-3)
            Load(ITEM_GRENADE);
            Load(ITEM_GRENADE_PRIMED);
            Load(ITEM_GROCERIES);
            Load(ITEM_HANS_VON_HANZ_PISTOL);
            Load(ITEM_HELLS_SOULS_JACKET);
            Load(ITEM_HOCKEY_STICK); //@@MP (Release 3)
            Load(ITEM_HUGE_HAMMER);
            Load(ITEM_HUNTER_VEST);
            Load(ITEM_HUNTING_CROSSBOW);
            Load(ITEM_HUNTING_RIFLE);
            Load(ITEM_IMPROVISED_CLUB);
            Load(ITEM_IMPROVISED_SPEAR);
            Load(ITEM_IRON_GOLF_CLUB);
            Load(ITEM_JASON_MYERS_AXE);
            Load(ITEM_KOLT_REVOLVER);
            Load(ITEM_LIQUOR_BOTTLE_AMBER); //@@MP (Release 4)
            Load(ITEM_LIQUOR_BOTTLE_CLEAR); //@@MP (Release 4)
            Load(ITEM_LIT_FLARE); //@@MP (Release 7-1)
            Load(ITEM_LIT_GLOWSTICK); //@@MP (Release 7-1)
            Load(ITEM_MACHETE); //@@MP (Release 3)
            Load(ITEM_MATCHES); //@@MP (Release 7-1)
            Load(ITEM_MAGAZINE);
            Load(ITEM_MEDIKIT);
            Load(ITEM_MOLOTOV); //@@MP (Release 4)
            Load(ITEM_MOLOTOV_PRIMED); //@@MP (Release 4)
            Load(ITEM_NAIL_GUN); //@@MP (Release 5-1)
            Load(ITEM_NIGHT_VISION); //@@MP (Release 6-3)
            Load(ITEM_PEANUTS); //@@MP (Release 7-3)
            Load(ITEM_PICKAXE); //@@MP (Release 3)
            Load(ITEM_PILLS_ANTIVIRAL);
            Load(ITEM_PILLS_BLUE);
            Load(ITEM_PILLS_GREEN);
            Load(ITEM_PILLS_SAN);
            Load(ITEM_PIPE_WRENCH); //@@MP (Release 3)
            Load(ITEM_PISTOL);
            Load(ITEM_POLICE_JACKET);
            Load(ITEM_POLICE_RADIO);
            Load(ITEM_POLICE_RIOT_ARMOR);
            Load(ITEM_POLICE_RIOT_SHIELD); //@@MP (Release 7-2)
            Load(ITEM_PRECISION_RIFLE);
            Load(ITEM_ROGUEDJACK_KEYBOARD);
            Load(ITEM_SANTAMAN_SHOTGUN);
            Load(ITEM_SHORT_SHOVEL);
            Load(ITEM_SHOTGUN);
            Load(ITEM_SHOVEL);
            Load(ITEM_SIPHON_KIT); //@@MP (Release 7-1)
            Load(ITEM_SLEEPING_BAG); //@@MP (Release 7-3)
            Load(ITEM_SMALL_HAMMER);
            Load(ITEM_SMOKE_GRENADE); //@@MP (Release 7-2)
            Load(ITEM_SMOKE_GRENADE_PRIMED); //@@MP (Release 7-2)
            Load(ITEM_SNACK_BAR); //@@MP (Release 7-1)
            Load(ITEM_SPIKES);
            Load(ITEM_SPRAYPAINT);
            Load(ITEM_SPRAYPAINT2);
            Load(ITEM_SPRAYPAINT3);
            Load(ITEM_SPRAYPAINT4);
            Load(ITEM_STANDARD_AXE); //@@MP (Release 3)
            Load(ITEM_STENCH_KILLER);
            Load(ITEM_STUN_GUN); //@@MP (Release 7-2)
            Load(ITEM_SUBWAY_BADGE);
            Load(ITEM_TENNIS_RACKET); //@@MP (Release 3)
            Load(ITEM_TRUNCHEON);
            Load(ITEM_VEGETABLE_SEEDS); //@@MP (Release 5-5)
            Load(ITEM_VEGETABLES); //@@MP (Release 5-5)
            Load(ITEM_WILD_BERRIES); //@@MP (Release 4)
            Load(ITEM_WOODEN_PLANK);
            Load(ITEM_ZTRACKER);
            #endregion

            #region Effects
            Notify(ui, "effects...");

            Load(EFFECT_BARRICADED);
            Load(EFFECT_ONFIRE);
            Load(EFFECT_ROT1_1);
            Load(EFFECT_ROT1_2);
            Load(EFFECT_ROT2_1);
            Load(EFFECT_ROT2_2);
            Load(EFFECT_ROT3_1);
            Load(EFFECT_ROT3_2);
            Load(EFFECT_ROT4_1);
            Load(EFFECT_ROT4_2);
            Load(EFFECT_ROT5_1);
            Load(EFFECT_ROT5_2);
            Load(EFFECT_FLASHBANG_020); //@@MP (Release 7-2)
            Load(EFFECT_FLASHBANG_040); //@@MP (Release 7-2)
            Load(EFFECT_FLASHBANG_060); //@@MP (Release 7-2)
            Load(EFFECT_FLASHBANG_080); //@@MP (Release 7-2)
            Load(EFFECT_FLASHBANG_100); //@@MP (Release 7-2)
            Load(EFFECT_LIGHT_TINT_CANDLE); //@@MP (Release 7-1)
            Load(EFFECT_LIGHT_TINT_FLARE); //@@MP (Release 7-1)
            Load(EFFECT_LIGHT_TINT_GLOWSTICK); //@@MP (Release 7-1)
            Load(EFFECT_NIGHTVISION); //@@MP (Release 7-2)
            Load(EFFECT_SMOKE_SCREEN); //@@MP (Release 7-2)
            Load(EFFECT_WEATHER_RAIN1);
            Load(EFFECT_WEATHER_RAIN2);
            Load(EFFECT_WEATHER_HEAVY_RAIN1);
            Load(EFFECT_WEATHER_HEAVY_RAIN2);
            Load(EFFECT_DISTURBED_LOW); //@@MP (Release 7-3)
            Load(EFFECT_DISTURBED_MED); //@@MP (Release 7-3)
            Load(EFFECT_DISTURBED_HIGH); //@@MP (Release 7-3)
            Load(EFFECT_TIPSY); //@@MP (Release 7-3)
            Load(EFFECT_DRUNK); //@@MP (Release 7-3)
            Load(EFFECT_HAMMERED); //@@MP (Release 7-3)
            #endregion

            #region Misc
            Notify(ui, "misc...");
            Load(UNDEF);
            Load(MAP_EXIT);
            Load(MINI_BLACKOPS_POSITION);
            Load(MINI_FOLLOWER_POSITION);
            Load(MINI_PLAYER_POSITION);
            Load(MINI_PLAYER_TAG1);
            Load(MINI_PLAYER_TAG2);
            Load(MINI_PLAYER_TAG3);
            Load(MINI_PLAYER_TAG4);
            Load(MINI_POLICE_POSITION);
            Load(MINI_UNDEAD_POSITION);
            Load(TRACK_BLACKOPS_POSITION);
            Load(TRACK_FOLLOWER_POSITION);
            Load(TRACK_POLICE_POSITION);
            Load(TRACK_UNDEAD_POSITION);
            Load(CORPSE_DRAGGED);
            Load(ICONS_LEGEND);  //@@MP (Release 6-1)
            Load(INSPECTION_MODE_HIGHLIGHT); //@@MP (Release 7-1)
            #endregion

            Notify(ui, "done!");
        }

        static void Load(string id)
        {
            string file = FOLDER + id + ".png";
            Bitmap img = null;
            Bitmap imgFixed = null;
            try //@@MP - try/finally ensures that the stream is always closed (Release 5-7)
            {
                img = new Bitmap(file);

                // fixes retarded GDI+ display bug with some png 32 images.
                imgFixed = new Bitmap(img);

                s_Images.Add(id, imgFixed);
                //s_GrayLevelImages.Add(id, MakeGrayLevel(imgFixed)); //@@MP this was the one and only in vanilla (Release 6-2)
                s_GrayLevelImages_Daytime.Add(id, MakeGrayLevel(imgFixed, "daytime"));
                s_GrayLevelImages_NighttimeClear.Add(id, MakeGrayLevel(imgFixed, "nighttime_clear"));
                s_GrayLevelImages_NighttimeClouded.Add(id, MakeGrayLevel(imgFixed, "nighttime_clouded"));
                s_GrayLevelImages_UndergroundNoTorch.Add(id, MakeGrayLevel(imgFixed, "underground_notorch"));
                s_GrayLevelImages_UndergroundLitTorch.Add(id, MakeGrayLevel(imgFixed, "underground_littorch"));
            }
            catch (Exception)
            {
                throw new ArgumentException("coud not load image id=" + id + "; file=" + file);
            }
            finally
            {
                if (img != null)
                    img.Dispose();
                /*if (imgFixed != null)
                    imgFixed.Dispose();*/
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        static Image MakeGrayLevel(Bitmap img, string grayLevelType) //@@MP - added parameter to allow graylevels for different times of day/location (Release 6-2)
        {
            Bitmap grayed = new Bitmap(img);

            for(int x = 0; x < grayed.Width; x++)
                for (int y = 0; y < grayed.Height; y++)
                {
                    Color pixelColor = img.GetPixel(x, y);
                    float brightness = pixelColor.GetBrightness();
                    int rgb;
                    switch (grayLevelType)
                    {
                        case "daytime": rgb = (int)(255 * GRAYLEVEL_DIM_FACTOR_DAYTIME * brightness); break;
                        case "nighttime_clear": rgb = (int)(255 * GRAYLEVEL_DIM_FACTOR_NIGHTTIME_CLEAR * brightness); break;
                        case "nighttime_clouded": rgb = (int)(255 * GRAYLEVEL_DIM_FACTOR_NIGHTTIME_CLOUDED * brightness); break;
                        case "underground_notorch": rgb = (int)(255 * GRAYLEVEL_DIM_FACTOR_UNDERGROUND_NOTORCH * brightness); break;
                        case "underground_littorch": rgb = (int)(255 * GRAYLEVEL_DIM_FACTOR_UNDERGROUND_LITTORCH * brightness); break;
                        default: throw new ArgumentOutOfRangeException("grayLevelType", "unhandled grayLevelType");
                    }

                    grayed.SetPixel(x, y, Color.FromArgb(pixelColor.A, rgb, rgb, rgb));
                }

            return grayed;
        }

        static void Notify(IRogueUI ui, string stage)
        {
            ui.UI_Clear(Color.Black);
            ui.UI_DrawStringBold(Color.White, "Loading resources: " + stage, 0, 0);
            ui.UI_Repaint();
        }
        #endregion

        #region Retrieving resources
        public static Image Get(string imageID)
        {
            Image img;
            if (s_Images.TryGetValue(imageID, out img))
                return img;
            else
                return s_Images[UNDEF];
        }

        public static Image GetGrayLevel(string imageID, string grayLevelType) //@@MP - added parameter to allow graylevels for different times of day/location (Release 6-2)
        {
            Image img;
            switch (grayLevelType) //@@MP - returns the image with the graylevel suitable for the player time of day and location (Release 6-2)
            {
                case "daytime":
                    if (s_GrayLevelImages_Daytime.TryGetValue(imageID, out img))
                        return img;
                    else
                        return s_GrayLevelImages_Daytime[UNDEF];
                case "nighttime_clear":
                    if (s_GrayLevelImages_NighttimeClear.TryGetValue(imageID, out img))
                        return img;
                    else
                        return s_GrayLevelImages_NighttimeClear[UNDEF];
                case "nighttime_clouded":
                    if (s_GrayLevelImages_NighttimeClouded.TryGetValue(imageID, out img))
                        return img;
                    else
                        return s_GrayLevelImages_NighttimeClouded[UNDEF];
                case "underground_notorch":
                    if (s_GrayLevelImages_UndergroundNoTorch.TryGetValue(imageID, out img))
                        return img;
                    else
                        return s_GrayLevelImages_UndergroundNoTorch[UNDEF];
                case "underground_littorch":
                    if (s_GrayLevelImages_UndergroundLitTorch.TryGetValue(imageID, out img))
                        return img;
                    else
                        return s_GrayLevelImages_UndergroundLitTorch[UNDEF];
                default: throw new ArgumentOutOfRangeException("grayLevelType", "unhandled grayLevelType");
            }
        }
        #endregion
    }
}

