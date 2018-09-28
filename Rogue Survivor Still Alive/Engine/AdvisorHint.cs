// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.AdvisorHint
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Engine
{
  [Serializable]
  internal enum AdvisorHint
  {
    MOVE_BASIC = 0,
    _FIRST = 0,
    MOUSE_LOOK = 1,
    KEYS_OPTIONS = 2,
    NIGHT = 3,
    RAIN = 4,
    ACTOR_MELEE = 5,
    MOVE_RUN = 6,
    MOVE_RESTING = 7,
    MOVE_JUMP = 8,
    ITEM_GRAB_CONTAINER = 9,
    ITEM_GRAB_FLOOR = 10, // 0x0000000A
    ITEM_UNEQUIP = 11, // 0x0000000B
    ITEM_EQUIP = 12, // 0x0000000C
    ITEM_TYPE_BARRICADING = 13, // 0x0000000D
    ITEM_DROP = 14, // 0x0000000E
    ITEM_USE = 15, // 0x0000000F
    FLASHLIGHT = 16, // 0x00000010
    CELLPHONES = 17, // 0x00000011
    SPRAYS_PAINT = 18, // 0x00000012
    SPRAYS_SCENT = 19, // 0x00000013
    WEAPON_FIRE = 20, // 0x00000014
    WEAPON_RELOAD = 21, // 0x00000015
    GRENADE = 22, // 0x00000016
    DOORWINDOW_OPEN = 23, // 0x00000017
    DOORWINDOW_CLOSE = 24, // 0x00000018
    OBJECT_PUSH = 25, // 0x00000019
    OBJECT_BREAK = 26, // 0x0000001A
    BARRICADE = 27, // 0x0000001B
    EXIT_STAIRS_LADDERS = 28, // 0x0000001C
    EXIT_LEAVING_DISTRICT = 29, // 0x0000001D
    STATE_SLEEPY = 30, // 0x0000001E
    STATE_HUNGRY = 31, // 0x0000001F
    NPC_TRADE = 32, // 0x00000020
    NPC_GIVING_ITEM = 33, // 0x00000021
    NPC_SHOUTING = 34, // 0x00000022
    BUILD_FORTIFICATION = 35, // 0x00000023
    LEADING_NEED_SKILL = 36, // 0x00000024
    LEADING_CAN_RECRUIT = 37, // 0x00000025
    LEADING_GIVE_ORDERS = 38, // 0x00000026
    LEADING_SWITCH_PLACE = 39, // 0x00000027
    GAME_SAVE_LOAD = 40, // 0x00000028
    CITY_INFORMATION = 41, // 0x00000029
    CORPSE_BUTCHER = 42, // 0x0000002A
    CORPSE_EAT = 43, // 0x0000002B
    CORPSE_DRAG_START = 44, // 0x0000002C
    CORPSE_DRAG_MOVE = 45, // 0x0000002D
    _COUNT = 46, // 0x0000002E
  }
}
