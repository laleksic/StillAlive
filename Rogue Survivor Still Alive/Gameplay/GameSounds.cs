
namespace djack.RogueSurvivor.Gameplay
{
    /// <summary>
    /// TODO: Downmix sounds to mono to allow for spatialisation
    /// </summary>
    /// see https://www.sfml-dev.org/tutorials/2.5/audio-spatialization.php
    /// Only relevant for SFX, not music nor ambients.
    /// To keep better depth try MonoPlus for foobar https://hydrogenaud.io/index.php/topic,108037.0.html
    static class GameSounds
    {
        const string PATH = @"Resources\Sfx\";

        //vanilla SFX
        public const string UNDEAD_EAT_PLAYER = "player undead eats"; //@@MP - added a NEARBY (Release 3)
        public const string UNDEAD_EAT_PLAYER_FILE = PATH + "sfx - undead eat player";
        public const string NIGHTMARE = "nightmare";
        public const string NIGHTMARE_FILE = PATH + "sfx - nightmare";

        #region ranged weapons
        #region Player
        //@@MP (Release 2), added rapid-fire (Release 6-6)
        public const string PISTOL_SINGLE_SHOT_PLAYER = "player fires pistol";
        public const string PISTOL_SINGLE_SHOT_PLAYER_FILE = PATH + "pistol_single-shot_player";
        public const string PISTOL_RAPID_FIRE_PLAYER = "player fires pistol burst";
        public const string PISTOL_RAPID_FIRE_PLAYER_FILE = PATH + "pistol_rapid-fire_player";
        public const string HUNTING_RIFLE_FIRE_PLAYER = "player fires hunting rifle";
        public const string HUNTING_RIFLE_FIRE_PLAYER_FILE = PATH + "huntingrifle_player";
        public const string SHOTGUN_FIRE_PLAYER = "player fires shotgun";
        public const string SHOTGUN_FIRE_PLAYER_FILE = PATH + "shotgun-a2_player";
        public const string CROSSBOW_FIRE_PLAYER = "player fires crossbow";
        public const string CROSSBOW_FIRE_PLAYER_FILE = PATH + "crossbow_player";
        public const string REVOLVER_SINGLE_SHOT_PLAYER = "player fires revolver";
        public const string REVOLVER_SINGLE_SHOT_PLAYER_FILE = PATH + "revolver_single-shot_player";
        public const string REVOLVER_RAPID_FIRE_PLAYER = "player fires revolver rounds";
        public const string REVOLVER_RAPID_FIRE_PLAYER_FILE = PATH + "revolver_rapid-fire_player";
        public const string PRECISION_RIFLE_FIRE_PLAYER = "player fires precision rifle";
        public const string PRECISION_RIFLE_FIRE_PLAYER_FILE = PATH + "precision_rifle_player";
        public const string ARMY_RIFLE_SINGLE_SHOT_PLAYER = "player fires army rifle";
        public const string ARMY_RIFLE_SINGLE_SHOT_PLAYER_FILE = PATH + "army_rifle_single-shot_player";
        public const string ARMY_RIFLE_RAPID_FIRE_PLAYER = "player fires army rifle burst";
        public const string ARMY_RIFLE_RAPID_FIRE_PLAYER_FILE = PATH + "army_rifle_rapid-fire_player";
        public const string SMG_SINGLE_SHOT_PLAYER = "player fires SMG";
        public const string SMG_SINGLE_SHOT_PLAYER_FILE = PATH + "SMG_single-shot_player";
        public const string SMG_RAPID_FIRE_PLAYER = "player fires SMG burst";
        public const string SMG_RAPID_FIRE_PLAYER_FILE = PATH + "SMG_rapid-fire_player";
        public const string MINIGUN_RAPID_FIRE_PLAYER = "player fires minigun burst";
        public const string MINIGUN_RAPID_FIRE_PLAYER_FILE = PATH + "minigun_rapid-fire_player";
        public const string GRENADE_LAUNCHER_SINGLE_SHOT_PLAYER = "player fires grenade launcher";
        public const string GRENADE_LAUNCHER_SINGLE_SHOT_PLAYER_FILE = PATH + "grenade_launcher_single-shot_player";
        public const string BIO_FORCE_GUN_PLAYER = "player fires bio force gun";
        public const string BIO_FORCE_GUN_PLAYER_FILE = PATH + "bio_force_gun_player";
        #endregion

        #region Nearby
        public const string PISTOL_SINGLE_SHOT_NEARBY = "pistol firing nearby";
        public const string PISTOL_SINGLE_SHOT_NEARBY_FILE = PATH + "pistol_single-shot_nearby";
        public const string PISTOL_RAPID_FIRE_NEARBY = "pistol shots nearby";
        public const string PISTOL_RAPID_FIRE_NEARBY_FILE = PATH + "pistol_rapid-fire_nearby";
        public const string HUNTING_RIFLE_FIRE_NEARBY = "hunting rifle firing nearby";
        public const string HUNTING_RIFLE_FIRE_NEARBY_FILE = PATH + "huntingrifle_nearby";
        public const string SHOTGUN_FIRE_NEARBY = "shotgun firing nearby";
        public const string SHOTGUN_FIRE_NEARBY_FILE = PATH + "shotgun-a2_nearby";
        public const string CROSSBOW_FIRE_NEARBY = "crossbow firing nearby";
        public const string CROSSBOW_FIRE_NEARBY_FILE = PATH + "crossbow_nearby";
        public const string REVOLVER_SINGLE_SHOT_NEARBY = "revolver firing nearby";
        public const string REVOLVER_SINGLE_SHOT_NEARBY_FILE = PATH + "revolver_single-shot_nearby";
        public const string REVOLVER_RAPID_FIRE_NEARBY = "revolver shots nearby";
        public const string REVOLVER_RAPID_FIRE_NEARBY_FILE = PATH + "revolver_rapid-fire_nearby";
        public const string PRECISION_RIFLE_FIRE_NEARBY = "precision rifle firing nearby";
        public const string PRECISION_RIFLE_FIRE_NEARBY_FILE = PATH + "precision_rifle_nearby";
        public const string ARMY_RIFLE_SINGLE_SHOT_NEARBY = "army rifle firing nearby";
        public const string ARMY_RIFLE_SINGLE_SHOT_NEARBY_FILE = PATH + "army_rifle_single-shot_nearby";
        public const string ARMY_RIFLE_RAPID_FIRE_NEARBY = "army rifle burst nearby";
        public const string ARMY_RIFLE_RAPID_FIRE_NEARBY_FILE = PATH + "army_rifle_rapid-fire_nearby";
        public const string SMG_SINGLE_SHOT_NEARBY = "SMG firing nearby";
        public const string SMG_SINGLE_SHOT_NEARBY_FILE = PATH + "SMG_single-shot_nearby";
        public const string SMG_RAPID_FIRE_NEARBY = "SMG burst nearby";
        public const string SMG_RAPID_FIRE_NEARBY_FILE = PATH + "SMG_rapid-fire_nearby";
        public const string MINIGUN_RAPID_FIRE_NEARBY = "minigun burst nearby";
        public const string MINIGUN_RAPID_FIRE_NEARBY_FILE = PATH + "minigun_rapid-fire_nearby";
        public const string GRENADE_LAUNCHER_SINGLE_SHOT_NEARBY = "grenade launcher firing nearby";
        public const string GRENADE_LAUNCHER_SINGLE_SHOT_NEARBY_FILE = PATH + "grenade_launcher_single-shot_nearby";
        #endregion

        #region Far
        public const string PISTOL_SINGLE_SHOT_FAR = "pistol firing somewhere";
        public const string PISTOL_SINGLE_SHOT_FAR_FILE = PATH + "pistol_single-shot_far";
        public const string PISTOL_RAPID_FIRE_FAR = "pistol shots somewhere";
        public const string PISTOL_RAPID_FIRE_FAR_FILE = PATH + "pistol_rapid-fire_far";
        public const string HUNTING_RIFLE_FIRE_FAR = "hunting rifle firing somewhere";
        public const string HUNTING_RIFLE_FIRE_FAR_FILE = PATH + "huntingrifle_far";
        public const string SHOTGUN_FIRE_FAR = "shotgun firing somewhere";
        public const string SHOTGUN_FIRE_FAR_FILE = PATH + "shotgun-a2_far";
        public const string REVOLVER_SINGLE_SHOT_FAR = "revolver firing somewhere";
        public const string REVOLVER_SINGLE_SHOT_FAR_FILE = PATH + "revolver_single-shot_far";
        public const string REVOLVER_RAPID_FIRE_FAR = "revolver shots somewhere";
        public const string REVOLVER_RAPID_FIRE_FAR_FILE = PATH + "revolver_rapid-fire_far";
        public const string PRECISION_RIFLE_FIRE_FAR = "precision rifle firing somewhere";
        public const string PRECISION_RIFLE_FIRE_FAR_FILE = PATH + "precision_rifle_far";
        public const string ARMY_RIFLE_SINGLE_SHOT_FAR = "army rifle firing somewhere";
        public const string ARMY_RIFLE_SINGLE_SHOT_FAR_FILE = PATH + "army_rifle_single-shot_far";
        public const string ARMY_RIFLE_RAPID_FIRE_FAR = "army rifle burst somewhere";
        public const string ARMY_RIFLE_RAPID_FIRE_FAR_FILE = PATH + "army_rifle_rapid-fire_far";
        public const string SMG_SINGLE_SHOT_FAR = "SMG firing somewhere";
        public const string SMG_SINGLE_SHOT_FAR_FILE = PATH + "SMG_single-shot_far";
        public const string SMG_RAPID_FIRE_FAR = "SMG burst somewhere";
        public const string SMG_RAPID_FIRE_FAR_FILE = PATH + "SMG_rapid-fire_far";
        public const string MINIGUN_RAPID_FIRE_FAR = "minigun burst somewhere";
        public const string MINIGUN_RAPID_FIRE_FAR_FILE = PATH + "minigun_rapid-fire_far";
        #endregion
        #endregion

        public const string MELEE_ATTACK_PLAYER = "player does melee attack";
        public const string MELEE_ATTACK_PLAYER_FILE = PATH + "melee_attack_player";
        public const string MELEE_ATTACK_NEARBY = "melee attack nearby";
        public const string MELEE_ATTACK_NEARBY_FILE = PATH + "melee_attack_nearby";

        public const string MELEE_ATTACK_MISS_PLAYER = "player misses melee attack";
        public const string MELEE_ATTACK_MISS_PLAYER_FILE = PATH + "melee_attack_miss_player";
        public const string MELEE_ATTACK_MISS_NEARBY = "missed melee attack nearby";
        public const string MELEE_ATTACK_MISS_NEARBY_FILE = PATH + "melee_attack_miss_nearby";

        public const string ARMOR_ZIPPER = "zips armor";
        public const string ARMOR_ZIPPER_FILE = PATH + "zipper";
        public const string EQUIP_GUN_PLAYER = "equips gun";
        public const string EQUIP_GUN_PLAYER_FILE = PATH + "equip_gun_player";
        public const string TORCH_CLICK_PLAYER = "clicks torch";
        public const string TORCH_CLICK_PLAYER_FILE = PATH + "torch_click_player";

        public const string CLIMB_FENCE_PLAYER = "climbing fence";
        public const string CLIMB_FENCE_PLAYER_FILE = PATH + "climb_fence_player";
        public const string CLIMB_FENCE_NEARBY = "climbing fence nearby";
        public const string CLIMB_FENCE_NEARBY_FILE = PATH + "climb_fence_nearby";

        public const string CLIMB_CAR_PLAYER = "climbing on car";
        public const string CLIMB_CAR_PLAYER_FILE = PATH + "jump_on_car_player";
        public const string CLIMB_CAR_NEARBY = "climbing on car nearby";
        public const string CLIMB_CAR_NEARBY_FILE = PATH + "jump_on_car_nearby";

        public const string SPRAY_SCENT = "sprays scent";
        public const string SPRAY_SCENT_FILE = PATH + "spray_scent";
        public const string SPRAY_TAG = "sprays tag";
        public const string SPRAY_TAG_FILE = PATH + "spray_tag";
        public const string EAT_FOOD = "eats food";
        public const string EAT_FOOD_FILE = PATH + "eat_food";
        public const string VOMIT_PLAYER = "vomits";
        public const string VOMIT_PLAYER_FILE = PATH + "vomit_player";

        public const string GLASS_DOOR = "opening glass door";
        public const string GLASS_DOOR_FILE = PATH + "glass_door";
        public const string WOODEN_DOOR_OPEN = "opening wooden door";
        public const string WOODEN_DOOR_OPEN_FILE = PATH + "wooden_door_open";
        public const string WOODEN_DOOR_CLOSE = "closing wooden door";
        public const string WOODEN_DOOR_CLOSE_FILE = PATH + "wooden_door_close";

        public const string USE_MEDICINE = "uses medicine";
        public const string USE_MEDICINE_FILE = PATH + "medicine";
        public const string USE_PILLS = "uses pills";
        public const string USE_PILLS_FILE = PATH + "pills";

        public const string SCREAM_NEARBY_01 = "scream nearby 01";
        public const string SCREAM_NEARBY_01_FILE = PATH + "scream_nearby_01";
        public const string SCREAM_NEARBY_02 = "scream nearby 02";
        public const string SCREAM_NEARBY_02_FILE = PATH + "scream_nearby_02";
        public const string SCREAM_NEARBY_03 = "scream nearby 03";
        public const string SCREAM_NEARBY_03_FILE = PATH + "scream_nearby_03";
        public const string SCREAM_NEARBY_04 = "scream nearby 04";
        public const string SCREAM_NEARBY_04_FILE = PATH + "scream_nearby_04";
        public const string SCREAM_NEARBY_05 = "scream nearby 05";
        public const string SCREAM_NEARBY_05_FILE = PATH + "scream_nearby_05";
        //06 was removed. too low quality //@@MP (Release 7-4)
        public const string SCREAM_NEARBY_07 = "scream nearby 07";
        public const string SCREAM_NEARBY_07_FILE = PATH + "scream_nearby_07";
        public const string SCREAM_NEARBY_08 = "scream nearby 08";
        public const string SCREAM_NEARBY_08_FILE = PATH + "scream_nearby_08";
        public const string SCREAM_NEARBY_09 = "scream nearby 09";
        public const string SCREAM_NEARBY_09_FILE = PATH + "scream_nearby_09";

        public const string SCREAM_FAR_01 = "scream far 01";
        public const string SCREAM_FAR_01_FILE = PATH + "scream_far_01";
        public const string SCREAM_FAR_02 = "scream far 02";
        public const string SCREAM_FAR_02_FILE = PATH + "scream_far_02";
        public const string SCREAM_FAR_03 = "scream far 03";
        public const string SCREAM_FAR_03_FILE = PATH + "scream_far_03";
        public const string SCREAM_FAR_04 = "scream far 04";
        public const string SCREAM_FAR_04_FILE = PATH + "scream_far_04";
        public const string SCREAM_FAR_05 = "scream far 05";
        public const string SCREAM_FAR_05_FILE = PATH + "scream_far_05";
        //06 was removed. too low quality //@@MP (Release 7-4)
        public const string SCREAM_FAR_07 = "scream far 07";
        public const string SCREAM_FAR_07_FILE = PATH + "scream_far_07";
        public const string SCREAM_FAR_08 = "scream far 08";
        public const string SCREAM_FAR_08_FILE = PATH + "scream_far_08";
        public const string SCREAM_FAR_09 = "scream far 09";
        public const string SCREAM_FAR_09_FILE = PATH + "scream_far_09";

        //@@MP (Release 3)
        public const string BASH_WOOD_PLAYER = "bash something wooden";
        public const string BASH_WOOD_PLAYER_FILE = PATH + "bash_wood_player";
        public const string BASH_WOOD_NEARBY = "something wooden bashed nearby";
        public const string BASH_WOOD_NEARBY_FILE = PATH + "bash_wood_nearby";

        public const string TURN_PAGE = "turns page";
        public const string TURN_PAGE_FILE = PATH + "turn_page";

        public const string UNDEAD_EAT_NEARBY = "nearby undead eats";
        public const string UNDEAD_EAT_NEARBY_FILE = PATH + "sfx - undead eat nearby";

        public const string VOMIT_NEARBY = "vomits nearby";
        public const string VOMIT_NEARBY_FILE = PATH + "vomit_nearby";

        public const string SHOVE_PLAYER = "shoves actor";
        public const string SHOVE_PLAYER_FILE = PATH + "shove_player";
        public const string SHOVE_NEARBY = "actor shoved nearby";
        public const string SHOVE_NEARBY_FILE = PATH + "shove_nearby";

        public const string PUSH_METAL_OBJECT_VISIBLE = "pushes visible metal object";
        public const string PUSH_METAL_OBJECT_VISIBLE_FILE = PATH + "push_metal_object_visible";
        public const string PUSH_METAL_OBJECT_AUDIBLE = "pushes audible metal object";
        public const string PUSH_METAL_OBJECT_AUDIBLE_FILE = PATH + "push_metal_object_audible";

        public const string BUILDING_PLAYER = "player constructs something";
        public const string BUILDING_PLAYER_FILE = PATH + "building_player";
        public const string BUILDING_NEARBY = "nearby construction"; //@@MP (Release 7-4)
        public const string BUILDING_NEARBY_FILE = PATH + "building_nearby";

        public const string CAN_TRAP_PLAYER = "player steps on can";
        public const string CAN_TRAP_PLAYER_FILE = PATH + "can_trap_player";
        public const string BEAR_TRAP_PLAYER = "player triggers bear trap";
        public const string BEAR_TRAP_PLAYER_FILE = PATH + "bear_trap_player";
        public const string SPIKE_TRAP = "player steps on spikes";
        public const string SPIKE_TRAP_FILE = PATH + "spike_trap";
        public const string BARBED_WIRE_TRAP_PLAYER = "player in barbed wire";
        public const string BARBED_WIRE_TRAP_PLAYER_FILE = PATH + "barbed_wire_player";

        public const string CAN_TRAP_NEARBY = "can trap nearby";
        public const string CAN_TRAP_NEARBY_FILE = PATH + "can_trap_nearby";
        public const string BEAR_TRAP_NEARBY = "bear trap nearby";
        public const string BEAR_TRAP_NEARBY_FILE = PATH + "bear_trap_nearby";

        public const string CAN_TRAP_FAR = "can trap somewhere";
        public const string CAN_TRAP_FAR_FILE = PATH + "can_trap_far";
        public const string BEAR_TRAP_FAR = "bear trap somewhere";
        public const string BEAR_TRAP_FAR_FILE = PATH + "bear_trap_far";

        //@@MP (Release 4)
        public const string DYNAMITE_VISIBLE = "dynamite explosion"; //@@MP - renamed from Release 2
        public const string DYNAMITE_VISIBLE_FILE = PATH + "dynamite_visible"; //@@MP - renamed from Release 2
        public const string DYNAMITE_AUDIBLE = "dynamite explosion nearby"; //@@MP - renamed from Release 2
        public const string DYNAMITE_AUDIBLE_FILE = PATH + "dynamite_audible"; //@@MP - renamed from Release 2

        public const string GRENADE_VISIBLE = "grenade explosion";
        public const string GRENADE_VISIBLE_FILE = PATH + "grenade_visible";
        public const string GRENADE_AUDIBLE = "grenade explosion nearby";
        public const string GRENADE_AUDIBLE_FILE = PATH + "grenade_audible";

        public const string MOLOTOV_VISIBLE = "molotov explosion";
        public const string MOLOTOV_VISIBLE_FILE = PATH + "molotov_visible";
        public const string MOLOTOV_AUDIBLE = "molotov explosion nearby";
        public const string MOLOTOV_AUDIBLE_FILE = PATH + "molotov_audible";

        public const string SMOKING = "player smokes";
        public const string SMOKING_FILE = PATH + "smoking";

        public const string ROLLER_DOOR = "roller door";
        public const string ROLLER_DOOR_FILE = PATH + "roller_door";

        //@@MP (Release 5-1)
        public const string NAIL_GUN = "player fires nail gun";
        public const string NAIL_GUN_FILE = PATH + "nail_gun";

        //@@MP (Release 5-3)
        public const string BREAK_WOODENDOOR_PLAYER = "break wooden door";
        public const string BREAK_WOODENDOOR_PLAYER_FILE = PATH + "break_woodendoor_player";
        public const string BREAK_WOODENDOOR_NEARBY = "wooden door breaks nearby";
        public const string BREAK_WOODENDOOR_NEARBY_FILE = PATH + "break_woodendoor_nearby";
        public const string BREAK_GLASSDOOR_PLAYER = "break glass door";
        public const string BREAK_GLASSDOOR_PLAYER_FILE = PATH + "break_glassdoor_player";
        public const string BREAK_GLASSDOOR_NEARBY = "glass door breaks nearby";
        public const string BREAK_GLASSDOOR_NEARBY_FILE = PATH + "break_glassdoor_nearby";

        //@@MP (Release 5-4)
        public const string BREAK_METALDOOR_PLAYER = "break metal door";
        public const string BREAK_METALDOOR_PLAYER_FILE = PATH + "break_metaldoor_player";
        public const string BREAK_METALDOOR_NEARBY = "metal door breaks nearby";
        public const string BREAK_METALDOOR_NEARBY_FILE = PATH + "break_metaldoor_nearby";
        public const string BREAK_CERAMIC_VISIBLE = "something ceramic breaks";
        public const string BREAK_CERAMIC_VISIBLE_FILE = PATH + "break_ceramic_visible";
        public const string BASH_OTHER_OBJECTS_PLAYER = "player bashes other object";
        public const string BASH_OTHER_OBJECTS_PLAYER_FILE = PATH + "bash_other_objects_player";
        public const string BASH_OTHER_OBJECTS_NEARBY = "bash other objects nearby";
        public const string BASH_OTHER_OBJECTS_NEARBY_FILE = PATH + "bash_other_objects_nearby";
        public const string BASH_CERAMIC_VISIBLE = "something ceramic is bashed";
        public const string BASH_CERAMIC_VISIBLE_FILE = PATH + "bash_ceramic_visible";
        public const string BASH_METALDOOR_PLAYER = "bash metal door";
        public const string BASH_METALDOOR_PLAYER_FILE = PATH + "bash_metaldoor_player";
        public const string BASH_METALDOOR_NEARBY = "metal door bashed nearby";
        public const string BASH_METALDOOR_NEARBY_FILE = PATH + "bash_metaldoor_nearby";
        public const string PUSH_WOODEN_OBJECT_VISIBLE = "pushes visible wooden object";
        public const string PUSH_WOODEN_OBJECT_VISIBLE_FILE = PATH + "push_wooden_object_visible";
        public const string PUSH_WOODEN_OBJECT_AUDIBLE = "pushes audible wooden object";
        public const string PUSH_WOODEN_OBJECT_AUDIBLE_FILE = PATH + "push_wooden_object_audible";

        //@@MP - relocated from music to sfx (Release 6-1)
        public const string REINCARNATE = "reincarnate";
        public const string REINCARNATE_FILE = PATH + "RS - Reincarnate";

        //@@MP (Release 6-2)
        public const string NIGHT_VISION = "night vision";
        public const string NIGHT_VISION_FILE = PATH + "night_vision";

        //@@MP (Release 6-3)
        public const string ACCESS_DENIED = "access denied";
        public const string ACCESS_DENIED_FILE = PATH + "access_denied";
        public const string ACCESS_GRANTED = "access granted";
        public const string ACCESS_GRANTED_FILE = PATH + "access_granted";

        //@@MP (Release 6-4)
        public const string ACHIEVEMENT = "achievement";
        public const string ACHIEVEMENT_FILE = PATH + "achievement";

        //@@MP (Release 6-6)
        public const string FLASHBANG_VISIBLE = "flashbang explosion";
        public const string FLASHBANG_VISIBLE_FILE = PATH + "flashbang_visible";
        public const string FLASHBANG_AUDIBLE = "flashbang explosion nearby";
        public const string FLASHBANG_AUDIBLE_FILE = PATH + "flashbang_audible";

        public const string DRINK = "drinks";
        public const string DRINK_FILE = PATH + "drink";
        public const string DIG_GROUND = "dig ground"; //@@MP - was called 'PLANT_SEEDS' before also being used when burying corpses (Release 7-6)
        public const string DIG_GROUND_FILE = PATH + "dig_ground";
        public const string PLACE_TRAP = "places trap";
        public const string PLACE_TRAP_FILE = PATH + "place_trap";
        public const string MAKE_MOLOTOV = "makes molotov";
        public const string MAKE_MOLOTOV_FILE = PATH + "make_molotov";
        public const string MALE_HURT = "male hurt";
        public const string MALE_HURT_FILE = PATH + "male_hurt";
        public const string FEMALE_HURT = "female hurt";
        public const string FEMALE_HURT_FILE = PATH + "female_hurt";

        //@@MP (Release 7-1)
        public const string CHAINSAW_PLAYER = "player uses chainsaw";
        public const string CHAINSAW_PLAYER_FILE = PATH + "chainsaw_player";
        public const string CHAINSAW_NEARBY = "chainsaw used nearby";
        public const string CHAINSAW_NEARBY_FILE = PATH + "chainsaw_nearby";
        public const string CHAINSAW_FAR = "chainsaw used somewhere";
        public const string CHAINSAW_FAR_FILE = PATH + "chainsaw_far";

        public const string FLAMETHROWER_VISIBLE = "flamethrower burst";
        public const string FLAMETHROWER_VISIBLE_FILE = PATH + "flamethrower_visible";
        public const string FLAMETHROWER_AUDIBLE = "flamethrower burst nearby";
        public const string FLAMETHROWER_AUDIBLE_FILE = PATH + "flamethrower_audible";

        public const string FUEL_CAN_VISIBLE = "fuel can explosion";
        public const string FUEL_CAN_VISIBLE_FILE = PATH + "fuel_can_visible";
        public const string FUEL_CAN_AUDIBLE = "fuel can explosion nearby";
        public const string FUEL_CAN_AUDIBLE_FILE = PATH + "fuel_can_audible";

        public const string FUEL_PUMP_VISIBLE = "fuel pump explosion";
        public const string FUEL_PUMP_VISIBLE_FILE = PATH + "fuel_pump_visible";
        public const string FUEL_PUMP_AUDIBLE = "fuel pump explosion nearby";
        public const string FUEL_PUMP_AUDIBLE_FILE = PATH + "fuel_pump_audible";

        public const string GLOWSTICK = "lights glowstick";
        public const string GLOWSTICK_FILE = PATH + "glowstick";
        public const string FLARE = "lights flare";
        public const string FLARE_FILE = PATH + "flare";

        public const string EQUIP = "equips";
        public const string EQUIP_FILE = PATH + "equip";

        //@@MP (Release 7-2)
        public const string SHIELD_BLOCK_PLAYER = "player uses shield";
        public const string SHIELD_BLOCK_PLAYER_FILE = PATH + "shield_block_player";
        public const string SHIELD_BLOCK_NEARBY = "shield used nearby";
        public const string SHIELD_BLOCK_NEARBY_FILE = PATH + "shield_block_nearby";
        public const string SMOKE_GRENADE = "smoke grenade burst";
        public const string SMOKE_GRENADE_FILE = PATH + "smoke_grenade";
        public const string STUN_GUN_PLAYER = "player fires stun gun";
        public const string STUN_GUN_PLAYER_FILE = PATH + "stun_gun_player";
        public const string STUN_GUN_NEARBY = "stun gun fired nearby";
        public const string STUN_GUN_NEARBY_FILE = PATH + "stun_gun_nearby";

        //@@MP (Release 7-2)
        public const string DOG_BARK_NEARBY = "dog barks nearby";
        public const string DOG_BARK_NEARBY_FILE = PATH + "dog_bark_nearby";
        public const string DOG_BARK_FAR = "dog barks somewhere";
        public const string DOG_BARK_FAR_FILE = PATH + "dog_bark_far";
        public const string DOG_FLEE = "dog flees";
        public const string DOG_FLEE_FILE = PATH + "dog_yips";
        public const string DOG_GROWL = "dog growls";
        public const string DOG_GROWL_FILE = PATH + "dog_growl";
        public const string RAT_SCREECH = "rat screeches";
        public const string RAT_SCREECH_FILE = PATH + "rat_screech";
        public const string SEWERS_THING_GROWL = "sewers thing growl";
        public const string SEWERS_THING_GROWL_FILE = PATH + "sewers_thing_growl";
        public const string SKELETON_GROWL = "skeleton growls";
        public const string SKELETON_GROWL_FILE = PATH + "skeleton_growl";
        public const string SHAMBLER_GROWL = "shambler growls";
        public const string SHAMBLER_GROWL_FILE = PATH + "shambler_growl";
        public const string ZOMBIE_MASTER_GROWL = "zombie master growls";
        public const string ZOMBIE_MASTER_GROWL_FILE = PATH + "zombie_master_growl";
        public const string ZOMBIFIED_GROAN = "zombified groans";
        public const string ZOMBIFIED_GROAN_FILE = PATH + "zombified_groan";
        public const string PSST_WHISPER = "whisper";
        public const string PSST_WHISPER_FILE = PATH + "psst2";
        public const string MALE_SHOUT_PLAYER = "male player shouts";
        public const string MALE_SHOUT_PLAYER_FILE = PATH + "male_shout_player";
        public const string MALE_SHOUT_NEARBY = "man nearby shouts";
        public const string MALE_SHOUT_NEARBY_FILE = PATH + "male_shout_nearby";
        public const string FEMALE_SHOUT_PLAYER = "female player shouts";
        public const string FEMALE_SHOUT_PLAYER_FILE = PATH + "female_shout_player";
        public const string FEMALE_SHOUT_NEARBY = "woman nearby shouts";
        public const string FEMALE_SHOUT_NEARBY_FILE = PATH + "female_shout_nearby";

        //@@MP (Release 7-4)
        public const string METAL_DOOR_OPEN = "opening metal door";
        public const string METAL_DOOR_OPEN_FILE = PATH + "metal_door_open";
        public const string METAL_DOOR_CLOSE = "closing metal door";
        public const string METAL_DOOR_CLOSE_FILE = PATH + "metal_door_close";

        //@@MP (Release 7-6)
        public const string FISHING_CAST_PLAYER = "fishing cast";
        public const string FISHING_CAST_PLAYER_FILE = PATH + "fishing_cast_player";
        public const string FISHING_CAST_NEARBY = "fishing cast nearby";
        public const string FISHING_CAST_NEARBY_FILE = PATH + "fishing_cast_nearby";
        public const string FISHING_REEL_PLAYER = "fishing reel";
        public const string FISHING_REEL_PLAYER_FILE = PATH + "fishing_reel_player";
        public const string FISHING_REEL_NEARBY = "fishing reel nearby";
        public const string FISHING_REEL_NEARBY_FILE = PATH + "fishing_reel_nearby";
        public const string COOKING_SIZZLE_PLAYER = "cooking sizzle";
        public const string COOKING_SIZZLE_PLAYER_FILE = PATH + "cooking_sizzle_player";
        public const string COOKING_SIZZLE_NEARBY = "cooking sizzle nearby";
        public const string COOKING_SIZZLE_NEARBY_FILE = PATH + "cooking_sizzle_nearby";
        public const string FIRE_EXTINGUISHER_PLAYER = "fire extinguisher";
        public const string FIRE_EXTINGUISHER_PLAYER_FILE = PATH + "fire_extinguisher_player";
        public const string FIRE_EXTINGUISHER_NEARBY = "fire extinguisher nearby";
        public const string FIRE_EXTINGUISHER_NEARBY_FILE = PATH + "fire_extinguisher_nearby";
        public const string MATCH_STRIKE_START_FIRE_PLAYER = "matches start fire";
        public const string MATCH_STRIKE_START_FIRE_PLAYER_FILE = PATH + "match_strike_start_fire_player";
        public const string EQUIP_BFG_PLAYER = "equips BFG";
        public const string EQUIP_BFG_PLAYER_FILE = PATH + "equip_BFG_player";
        public const string PLASMA_BURST_VISIBLE = "plasma burst";
        public const string PLASMA_BURST_VISIBLE_FILE = PATH + "plasma_burst_visible";
        public const string PLASMA_BURST_AUDIBLE = "plasma burst nearby";
        public const string PLASMA_BURST_AUDIBLE_FILE = PATH + "plasma_burst_audible";
    }
}
