using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace djack.RogueSurvivor.Engine
{
    enum PlayerCommand
    {
        NONE,

        QUIT_GAME,
        HELP_MODE,
        ADVISOR,
        OPTIONS_MODE,
        KEYBINDING_MODE,
        HINTS_SCREEN_MODE,
        SCREENSHOT,
        SAVE_GAME,
        LOAD_GAME,
        ABANDON_GAME,
        ESC, //@@MP (Release 7-4)

        MOVE_N,
        MOVE_NE,
        MOVE_E,
        MOVE_SE,
        MOVE_S,
        MOVE_SW,
        MOVE_W,
        MOVE_NW,
        RUN_TOGGLE,
        WAIT_OR_SELF,
        WAIT_LONG,

        BREAK_MODE,
        BURY_CORPSE, //@@MP (Release 7-6)
        BUILD_MODE,
        CLOSE_DOOR,
        COOK_FOOD, //@@MP (Release 7-6)
        DESTROY_ITEM, //@@MP (Release 7-6)
        EAT_CORPSE,
        FIRE_MODE,
        GIVE_ITEM,
        ICONS_LEGEND, //@@MP (Release 6-1)
        NEGOTIATE_TRADE, // alpha 10 renamed
        LEAD_MODE,
        INSPECTION_MODE, //@@MP (Release 7-1)
        MAKE_COOKING_FIRE, //@@MP (Release 7-6)
        MARK_ENEMIES_MODE,
        ORDER_MODE,
        PULL_MODE, // alpha 10
        PUSH_MODE,
        REVIVE_CORPSE,
        SHOUT,
        SLEEP,
        SWITCH_PLACE,
        UNLOAD_AMMO, //@@MP (Release 7-6)
        USE_EXIT,
        USE_SPRAY,

        CITY_INFO,
        MESSAGE_LOG,

        ITEM_SLOT_0,
        ITEM_SLOT_1,
        ITEM_SLOT_2,
        ITEM_SLOT_3,
        ITEM_SLOT_4,
        ITEM_SLOT_5,
        ITEM_SLOT_6,
        ITEM_SLOT_7,
        ITEM_SLOT_8,
        ITEM_SLOT_9
    }
}
