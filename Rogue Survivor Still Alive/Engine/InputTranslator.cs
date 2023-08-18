using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using djack.RogueSurvivor.Engine;

using SFML;
using KeyEventArgs = SFML.Window.KeyEventArgs;
using Keys = SFML.Window.Keyboard.Key;

namespace djack.RogueSurvivor.Engine
{
    static class InputTranslator
    {
        public static PlayerCommand KeyToCommand(KeyEventArgs keyargs)
        {
            KeyCombo key = new KeyCombo(keyargs);
            PlayerCommand command = RogueGame.KeyBindings.Get(key);

            if (command != PlayerCommand.NONE)
                return command;

            // check special case for item slot keys.
            // slots keys are always used with Ctrl, Shift or Alt modifiers so they are unbound in keybindings.
            if (key.Control || key.Alt || key.Shift)
            {
                // slot key?
                var code = key.Code;
                if (code == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_0).Code)
                    return PlayerCommand.ITEM_SLOT_0;
                if (code == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_1).Code)
                    return PlayerCommand.ITEM_SLOT_1;
                if (code == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_2).Code)
                    return PlayerCommand.ITEM_SLOT_2;
                if (code == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_3).Code)
                    return PlayerCommand.ITEM_SLOT_3;
                if (code == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_4).Code)
                    return PlayerCommand.ITEM_SLOT_4;
                if (code == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_5).Code)
                    return PlayerCommand.ITEM_SLOT_5;
                if (code == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_6).Code)
                    return PlayerCommand.ITEM_SLOT_6;
                if (code == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_7).Code)
                    return PlayerCommand.ITEM_SLOT_7;
                if (code == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_8).Code)
                    return PlayerCommand.ITEM_SLOT_8;
                if (code == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_9).Code)
                    return PlayerCommand.ITEM_SLOT_9;
            }

            // no command.
            return PlayerCommand.NONE;
        }
    }
}
