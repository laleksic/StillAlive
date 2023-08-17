using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using djack.RogueSurvivor.Engine;

using SFML;
using KeyEventArgs = SFML.Window.KeyEventArgs;
using Keys = SFML.Window.Keyboard.Key;

namespace djack.RogueSurvivor.Engine
{
    static class InputTranslator
    {
        public static PlayerCommand KeyToCommand(KeyEventArgs key)
        {
            PlayerCommand command = RogueGame.KeyBindings.Get(key);

            Logger.WriteLine(Logger.Stage.RUN_MAIN, "Key: " + key.ToString() + " --> Command: " + command.ToString());

            if (command != PlayerCommand.NONE)
                return command;

            // check special case for item slot keys.
            // slots keys are always used with Ctrl, Shift or Alt modifiers so they are unbound in keybindings.
            if (key.Control || key.Alt || key.Shift)
            {
                // clear modifiers.
                key.Control = false;
                key.Alt = false;
                key.Shift = false;

                // slot key?
                if (key == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_0))
                    return PlayerCommand.ITEM_SLOT_0;
                if (key == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_1))
                    return PlayerCommand.ITEM_SLOT_1;
                if (key == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_2))
                    return PlayerCommand.ITEM_SLOT_2;
                if (key == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_3))
                    return PlayerCommand.ITEM_SLOT_3;
                if (key == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_4))
                    return PlayerCommand.ITEM_SLOT_4;
                if (key == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_5))
                    return PlayerCommand.ITEM_SLOT_5;
                if (key == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_6))
                    return PlayerCommand.ITEM_SLOT_6;
                if (key == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_7))
                    return PlayerCommand.ITEM_SLOT_7;
                if (key == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_8))
                    return PlayerCommand.ITEM_SLOT_8;
                if (key == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_9))
                    return PlayerCommand.ITEM_SLOT_9;
            }

            // no command.
            return PlayerCommand.NONE;
        }
    }
}
