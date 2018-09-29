using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace djack.RogueSurvivor.Gameplay
{
    static class GameSounds
    {
        static readonly string PATH = @"Resources\Sfx\";

        public static readonly string UNDEAD_EAT = "undead eat";
        public static readonly string UNDEAD_EAT_FILE = PATH + "sfx - undead eat";

        public static readonly string UNDEAD_RISE = "undead rise";
        public static readonly string UNDEAD_RISE_FILE = PATH + "sfx - undead rise";

        public static readonly string NIGHTMARE = "nightmare";
        public static readonly string NIGHTMARE_FILE = PATH + "sfx - nightmare";

        public static readonly string COMMOTION = "commotion";
        public static readonly string COMMOTION_FILE = PATH + "commotion";

        //@@MP (Release 2)
        public static readonly string PISTOL_FIRE_PLAYER = "player fires pistol";
        public static readonly string PISTOL_FIRE_PLAYER_FILE = PATH + "pistol_player";
        public static readonly string HUNTING_RIFLE_FIRE_PLAYER = "player fires hunting rifle";
        public static readonly string HUNTING_RIFLE_FIRE_PLAYER_FILE = PATH + "huntingrifle_player";
        public static readonly string SHOTGUN_FIRE_PLAYER = "player fires shotgun";
        public static readonly string SHOTGUN_FIRE_PLAYER_FILE = PATH + "shotgun_player";
        public static readonly string CROSSBOW_FIRE_PLAYER = "player fires crossbow";
        public static readonly string CROSSBOW_FIRE_PLAYER_FILE = PATH + "crossbow_player";

        public static readonly string PISTOL_FIRE_NEARBY = "pistol firing nearby";
        public static readonly string PISTOL_FIRE_NEARBY_FILE = PATH + "pistol_nearby";
        public static readonly string HUNTING_RIFLE_FIRE_NEARBY = "hunting rifle firing nearby";
        public static readonly string HUNTING_RIFLE_FIRE_NEARBY_FILE = PATH + "huntingrifle_nearby";
        public static readonly string SHOTGUN_FIRE_NEARBY = "shotgun firing nearby";
        public static readonly string SHOTGUN_FIRE_NEARBY_FILE = PATH + "shotgun_nearby";
        public static readonly string CROSSBOW_FIRE_NEARBY = "crossbow firing nearby";
        public static readonly string CROSSBOW_FIRE_NEARBY_FILE = PATH + "crossbow_nearby";

        public static readonly string PISTOL_FIRE_FAR = "pistol firing somewhere";
        public static readonly string PISTOL_FIRE_FAR_FILE = PATH + "pistol_far";
        public static readonly string HUNTING_RIFLE_FIRE_FAR = "hunting rifle firing somewhere";
        public static readonly string HUNTING_RIFLE_FIRE_FAR_FILE = PATH + "huntingrifle_far";
        public static readonly string SHOTGUN_FIRE_FAR = "shotgun firing somewhere";
        public static readonly string SHOTGUN_FIRE_FAR_FILE = PATH + "shotgun_far";
        public static readonly string CROSSBOW_FIRE_FAR = "crossbow firing somewhere";
        public static readonly string CROSSBOW_FIRE_FAR_FILE = PATH + "crossbow_far";

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

        public static readonly string EXPLOSION_VISIBLE = "loud explosion";
        public static readonly string EXPLOSION_VISIBLE_FILE = PATH + "explosion";
        public static readonly string EXPLOSION_NEARBY = "explosion nearby";
        public static readonly string EXPLOSION_NEARBY_FILE = PATH + "explosion_nearby";

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
        public static readonly string VOMIT = "vomits";
        public static readonly string VOMIT_FILE = PATH + "vomit";

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
    }
}
