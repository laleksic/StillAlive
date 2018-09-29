using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace djack.RogueSurvivor.Gameplay
{
    static class GameSounds
    {
        static readonly string PATH = @"Resources\Sfx\";

        public static readonly string UNDEAD_EAT_PLAYER = "player undead eats"; //@@MP - added a NEARBY (Release 3)
        public static readonly string UNDEAD_EAT_PLAYER_FILE = PATH + "sfx - undead eat";
        public static readonly string UNDEAD_RISE_PLAYER = "player undead rises"; //@@MP - added a NEARBY (Release 3)
        public static readonly string UNDEAD_RISE_PLAYER_FILE = PATH + "sfx - undead rise player";
        public static readonly string NIGHTMARE = "nightmare";
        public static readonly string NIGHTMARE_FILE = PATH + "sfx - nightmare";

        //@@MP (Release 2)
        public static readonly string PISTOL_FIRE_PLAYER = "player fires pistol";
        public static readonly string PISTOL_FIRE_PLAYER_FILE = PATH + "pistol_player";
        public static readonly string HUNTING_RIFLE_FIRE_PLAYER = "player fires hunting rifle";
        public static readonly string HUNTING_RIFLE_FIRE_PLAYER_FILE = PATH + "huntingrifle_player";
        public static readonly string SHOTGUN_FIRE_PLAYER = "player fires shotgun";
        public static readonly string SHOTGUN_FIRE_PLAYER_FILE = PATH + "shotgun-a2_player";
        public static readonly string CROSSBOW_FIRE_PLAYER = "player fires crossbow";
        public static readonly string CROSSBOW_FIRE_PLAYER_FILE = PATH + "crossbow_player";

        public static readonly string PISTOL_FIRE_NEARBY = "pistol firing nearby";
        public static readonly string PISTOL_FIRE_NEARBY_FILE = PATH + "pistol_nearby";
        public static readonly string HUNTING_RIFLE_FIRE_NEARBY = "hunting rifle firing nearby";
        public static readonly string HUNTING_RIFLE_FIRE_NEARBY_FILE = PATH + "huntingrifle_nearby";
        public static readonly string SHOTGUN_FIRE_NEARBY = "shotgun firing nearby";
        public static readonly string SHOTGUN_FIRE_NEARBY_FILE = PATH + "shotgun-a2_nearby";
        public static readonly string CROSSBOW_FIRE_NEARBY = "crossbow firing nearby";
        public static readonly string CROSSBOW_FIRE_NEARBY_FILE = PATH + "crossbow_nearby";

        public static readonly string PISTOL_FIRE_FAR = "pistol firing somewhere";
        public static readonly string PISTOL_FIRE_FAR_FILE = PATH + "pistol_far";
        public static readonly string HUNTING_RIFLE_FIRE_FAR = "hunting rifle firing somewhere";
        public static readonly string HUNTING_RIFLE_FIRE_FAR_FILE = PATH + "huntingrifle_far";
        public static readonly string SHOTGUN_FIRE_FAR = "shotgun firing somewhere";
        public static readonly string SHOTGUN_FIRE_FAR_FILE = PATH + "shotgun-a2_far";

        public static readonly string MELEE_ATTACK_PLAYER = "player does melee attack";
        public static readonly string MELEE_ATTACK_PLAYER_FILE = PATH + "melee_attack_player";
        public static readonly string MELEE_ATTACK_NEARBY = "melee attack nearby";
        public static readonly string MELEE_ATTACK_NEARBY_FILE = PATH + "melee_attack_nearby";

        public static readonly string MELEE_ATTACK_MISS_PLAYER = "player misses melee attack";
        public static readonly string MELEE_ATTACK_MISS_PLAYER_FILE = PATH + "melee_attack_miss_player";
        public static readonly string MELEE_ATTACK_MISS_NEARBY = "missed melee attack nearby";
        public static readonly string MELEE_ATTACK_MISS_NEARBY_FILE = PATH + "melee_attack_miss_nearby";

        public static readonly string ARMOR_ZIPPER = "zips armor";
        public static readonly string ARMOR_ZIPPER_FILE = PATH + "zipper";
        public static readonly string EQUIP_GUN_PLAYER = "equips gun";
        public static readonly string EQUIP_GUN_PLAYER_FILE = PATH + "equip_gun_player";
        public static readonly string TORCH_CLICK_PLAYER = "clicks torch";
        public static readonly string TORCH_CLICK_PLAYER_FILE = PATH + "torch_click_player";

        public static readonly string CLIMB_FENCE_PLAYER = "climbing fence";
        public static readonly string CLIMB_FENCE_PLAYER_FILE = PATH + "climb_fence_player";
        public static readonly string CLIMB_FENCE_NEARBY = "climbing fence nearby";
        public static readonly string CLIMB_FENCE_NEARBY_FILE = PATH + "climb_fence_nearby";

        public static readonly string CLIMB_CAR_PLAYER = "climbing on car";
        public static readonly string CLIMB_CAR_PLAYER_FILE = PATH + "jump_on_car_player";
        public static readonly string CLIMB_CAR_NEARBY = "climbing on car nearby";
        public static readonly string CLIMB_CAR_NEARBY_FILE = PATH + "jump_on_car_nearby";

        public static readonly string SPRAY_SCENT = "sprays scent";
        public static readonly string SPRAY_SCENT_FILE = PATH + "spray_scent";
        public static readonly string SPRAY_TAG = "sprays tag";
        public static readonly string SPRAY_TAG_FILE = PATH + "spray_tag";
        public static readonly string EAT_FOOD = "eats food";
        public static readonly string EAT_FOOD_FILE = PATH + "eat_food";
        public static readonly string VOMIT_PLAYER = "vomits";
        public static readonly string VOMIT_PLAYER_FILE = PATH + "vomit_player";

        public static readonly string GLASS_DOOR = "opening glass door";
        public static readonly string GLASS_DOOR_FILE = PATH + "glass_door";
        public static readonly string WOODEN_DOOR_OPEN = "opening wooden door";
        public static readonly string WOODEN_DOOR_OPEN_FILE = PATH + "wooden_door_open";
        public static readonly string WOODEN_DOOR_CLOSE = "closing wooden door";
        public static readonly string WOODEN_DOOR_CLOSE_FILE = PATH + "wooden_door_close";

        public static readonly string USE_MEDICINE = "uses medicine";
        public static readonly string USE_MEDICINE_FILE = PATH + "medicine";
        public static readonly string USE_PILLS = "uses pills";
        public static readonly string USE_PILLS_FILE = PATH + "pills";

        public static readonly string SCREAM_NEARBY_01 = "scream nearby 01";
        public static readonly string SCREAM_NEARBY_01_FILE = PATH + "scream_nearby_01";
        public static readonly string SCREAM_NEARBY_02 = "scream nearby 02";
        public static readonly string SCREAM_NEARBY_02_FILE = PATH + "scream_nearby_02";
        public static readonly string SCREAM_NEARBY_03 = "scream nearby 03";
        public static readonly string SCREAM_NEARBY_03_FILE = PATH + "scream_nearby_03";
        public static readonly string SCREAM_NEARBY_04 = "scream nearby 04";
        public static readonly string SCREAM_NEARBY_04_FILE = PATH + "scream_nearby_04";
        public static readonly string SCREAM_NEARBY_05 = "scream nearby 05";
        public static readonly string SCREAM_NEARBY_05_FILE = PATH + "scream_nearby_05";
        public static readonly string SCREAM_NEARBY_06 = "scream nearby 06";
        public static readonly string SCREAM_NEARBY_06_FILE = PATH + "scream_nearby_06";
        public static readonly string SCREAM_NEARBY_07 = "scream nearby 07";
        public static readonly string SCREAM_NEARBY_07_FILE = PATH + "scream_nearby_07";

        public static readonly string SCREAM_FAR_01 = "scream far 01";
        public static readonly string SCREAM_FAR_01_FILE = PATH + "scream_far_01";
        public static readonly string SCREAM_FAR_02 = "scream far 02";
        public static readonly string SCREAM_FAR_02_FILE = PATH + "scream_far_02";
        public static readonly string SCREAM_FAR_03 = "scream far 03";
        public static readonly string SCREAM_FAR_03_FILE = PATH + "scream_far_03";
        public static readonly string SCREAM_FAR_04 = "scream far 04";
        public static readonly string SCREAM_FAR_04_FILE = PATH + "scream_far_04";
        public static readonly string SCREAM_FAR_05 = "scream far 05";
        public static readonly string SCREAM_FAR_05_FILE = PATH + "scream_far_05";
        public static readonly string SCREAM_FAR_06 = "scream far 06";
        public static readonly string SCREAM_FAR_06_FILE = PATH + "scream_far_06";
        public static readonly string SCREAM_FAR_07 = "scream far 07";
        public static readonly string SCREAM_FAR_07_FILE = PATH + "scream_far_07";

        //@@MP (Release 3)
        public static readonly string BASH_PLAYER = "bash something";
        public static readonly string BASH_PLAYER_FILE = PATH + "bash_player";
        public static readonly string BASH_NEARBY = "something bashed nearby";
        public static readonly string BASH_NEARBY_FILE = PATH + "bash_nearby";

        public static readonly string TURN_PAGE = "turns page";
        public static readonly string TURN_PAGE_FILE = PATH + "turn_page";

        public static readonly string UNDEAD_EAT_NEARBY = "nearby undead eats";
        public static readonly string UNDEAD_EAT_NEARBY_FILE = PATH + "sfx - undead eat nearby";
        public static readonly string UNDEAD_RISE_NEARBY = "nearby undead rises";
        public static readonly string UNDEAD_RISE_NEARBY_FILE = PATH + "sfx - undead rise nearby";

        public static readonly string VOMIT_NEARBY = "vomits nearby";
        public static readonly string VOMIT_NEARBY_FILE = PATH + "vomit_nearby";

        public static readonly string SHOVE_PLAYER = "shoves actor";
        public static readonly string SHOVE_PLAYER_FILE = PATH + "shove_player";
        public static readonly string SHOVE_NEARBY = "actor shoved nearby";
        public static readonly string SHOVE_NEARBY_FILE = PATH + "shove_nearby";

        public static readonly string PUSH_OBJECT_VISIBLE = "pushes visible object";
        public static readonly string PUSH_OBJECT_VISIBLE_FILE = PATH + "push_object_visible";
        public static readonly string PUSH_OBJECT_AUDIBLE = "pushes audible object";
        public static readonly string PUSH_OBJECT_AUDIBLE_FILE = PATH + "push_object_audible";

        public static readonly string BUILDING = "barricade repair fortify";
        public static readonly string BUILDING_FILE = PATH + "building";

        public static readonly string CAN_TRAP_PLAYER = "player steps on can";
        public static readonly string CAN_TRAP_PLAYER_FILE = PATH + "can_trap_player";
        public static readonly string BEAR_TRAP_PLAYER = "player triggers bear trap";
        public static readonly string BEAR_TRAP_PLAYER_FILE = PATH + "can_trap_player";
        public static readonly string SPIKE_TRAP = "player steps on spikes";
        public static readonly string SPIKE_TRAP_FILE = PATH + "spike_trap";
        public static readonly string BARBED_WIRE_TRAP_PLAYER = "player in barbed wire";
        public static readonly string BARBED_WIRE_TRAP_PLAYER_FILE = PATH + "barbed_wire_player";

        public static readonly string CAN_TRAP_NEARBY = "can trap nearby";
        public static readonly string CAN_TRAP_NEARBY_FILE = PATH + "can_trap_nearby";
        public static readonly string BEAR_TRAP_NEARBY = "bear trap nearby";
        public static readonly string BEAR_TRAP_NEARBY_FILE = PATH + "bear_trap_nearby";

        public static readonly string CAN_TRAP_FAR = "can trap somewhere";
        public static readonly string CAN_TRAP_FAR_FILE = PATH + "can_trap_far";
        public static readonly string BEAR_TRAP_FAR = "bear trap somewhere";
        public static readonly string BEAR_TRAP_FAR_FILE = PATH + "bear_trap_far";

        //@@MP (Release 4)
        public static readonly string DYNAMITE_VISIBLE = "dynamite explosion"; //@@MP - renamed from Release 2
        public static readonly string DYNAMITE_VISIBLE_FILE = PATH + "dynamite_visible"; //@@MP - renamed from Release 2
        public static readonly string DYNAMITE_AUDIBLE = "dynamite explosion nearby"; //@@MP - renamed from Release 2
        public static readonly string DYNAMITE_AUDIBLE_FILE = PATH + "dynamite_audible"; //@@MP - renamed from Release 2

        public static readonly string GRENADE_VISIBLE = "grenade explosion";
        public static readonly string GRENADE_VISIBLE_FILE = PATH + "grenade_visible";
        public static readonly string GRENADE_AUDIBLE = "grenade explosion nearby";
        public static readonly string GRENADE_AUDIBLE_FILE = PATH + "grenade_audible";

        public static readonly string MOLOTOV_VISIBLE = "molotov explosion";
        public static readonly string MOLOTOV_VISIBLE_FILE = PATH + "molotov_visible";
        public static readonly string MOLOTOV_AUDIBLE = "molotov explosion nearby";
        public static readonly string MOLOTOV_AUDIBLE_FILE = PATH + "molotov_audible";

        public static readonly string SMOKING = "player smokes";
        public static readonly string SMOKING_FILE = PATH + "smoking";

        public static readonly string ROLLER_DOOR = "roller door";
        public static readonly string ROLLER_DOOR_FILE = PATH + "roller_door";

        //@@MP (Release 5-1)
        public static readonly string NAIL_GUN = "player fires nail gun";
        public static readonly string NAIL_GUN_FILE = PATH + "nail_gun";

        //@@MP (Release 5-3)
        public static readonly string BREAKWOODENDOOR_PLAYER = "break wooden door";
        public static readonly string BREAKWOODENDOOR_PLAYER_FILE = PATH + "breakwoodendoor_player";
        public static readonly string BREAKWOODENDOOR_NEARBY = "wooden door breaks nearby";
        public static readonly string BREAKWOODENDOOR_NEARBY_FILE = PATH + "breakwoodendoor_nearby";
        public static readonly string BREAKGLASSDOOR_PLAYER = "break glass door";
        public static readonly string BREAKGLASSDOOR_PLAYER_FILE = PATH + "breakglassdoor_player";
        public static readonly string BREAKGLASSDOOR_NEARBY = "glass door breaks nearby";
        public static readonly string BREAKGLASSDOOR_NEARBY_FILE = PATH + "breakglassdoor_nearby";
        public static readonly string RAIN_OUTSIDE = "outside whilst raining";
        public static readonly string RAIN_OUTSIDE_FILE = PATH + "rain_outside_looped";
        public static readonly string RAIN_INSIDE = "inside whilst raining";
        public static readonly string RAIN_INSIDE_FILE = PATH + "rain_inside_looped";
    }
}
