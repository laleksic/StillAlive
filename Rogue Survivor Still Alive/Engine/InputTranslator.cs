// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.InputTranslator
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System.Windows.Forms;

namespace djack.RogueSurvivor.Engine
{
  internal static class InputTranslator
  {
    public static PlayerCommand KeyToCommand(KeyEventArgs key)
    {
      PlayerCommand playerCommand = RogueGame.KeyBindings.Get(key.KeyData);
      if (playerCommand != PlayerCommand.NONE)
        return playerCommand;
      if (key.Modifiers != Keys.None)
      {
        Keys keyData = key.KeyData;
        if ((key.Modifiers & Keys.Control) != Keys.None)
          keyData ^= Keys.Control;
        if ((key.Modifiers & Keys.Shift) != Keys.None)
          keyData ^= Keys.Shift;
        if ((key.Modifiers & Keys.Alt) != Keys.None)
          keyData ^= Keys.Alt;
        if (keyData == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_0))
          return PlayerCommand.ITEM_SLOT_0;
        if (keyData == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_1))
          return PlayerCommand.ITEM_SLOT_1;
        if (keyData == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_2))
          return PlayerCommand.ITEM_SLOT_2;
        if (keyData == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_3))
          return PlayerCommand.ITEM_SLOT_3;
        if (keyData == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_4))
          return PlayerCommand.ITEM_SLOT_4;
        if (keyData == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_5))
          return PlayerCommand.ITEM_SLOT_5;
        if (keyData == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_6))
          return PlayerCommand.ITEM_SLOT_6;
        if (keyData == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_7))
          return PlayerCommand.ITEM_SLOT_7;
        if (keyData == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_8))
          return PlayerCommand.ITEM_SLOT_8;
        if (keyData == RogueGame.KeyBindings.Get(PlayerCommand.ITEM_SLOT_9))
          return PlayerCommand.ITEM_SLOT_9;
      }
      return PlayerCommand.NONE;
    }
  }
}
