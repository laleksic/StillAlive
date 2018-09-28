// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Gameplay.GameGangs
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Gameplay
{
  internal static class GameGangs
  {
    public static readonly GameGangs.IDs[] GANGSTAS = new GameGangs.IDs[2]
    {
      GameGangs.IDs.GANGSTA_CRAPS,
      GameGangs.IDs.GANGSTA_FLOODS
    };
    public static readonly GameGangs.IDs[] BIKERS = new GameGangs.IDs[2]
    {
      GameGangs.IDs.BIKER_HELLS_SOULS,
      GameGangs.IDs.BIKER_FREE_ANGELS
    };
    public static readonly string[] NAMES = new string[5]
    {
      "(no gang)",
      "Hell's Souls",
      "Free Angels",
      "Craps",
      "Floods"
    };
    public static readonly GameItems.IDs[][] BAD_GANG_OUTFITS = new GameItems.IDs[5][]
    {
      new GameItems.IDs[0],
      new GameItems.IDs[3]
      {
        GameItems.IDs.ARMOR_FREE_ANGELS_JACKET,
        GameItems.IDs.ARMOR_POLICE_JACKET,
        GameItems.IDs.ARMOR_POLICE_RIOT
      },
      new GameItems.IDs[3]
      {
        GameItems.IDs.ARMOR_HELLS_SOULS_JACKET,
        GameItems.IDs.ARMOR_POLICE_JACKET,
        GameItems.IDs.ARMOR_POLICE_RIOT
      },
      new GameItems.IDs[4]
      {
        GameItems.IDs.ARMOR_FREE_ANGELS_JACKET,
        GameItems.IDs.ARMOR_HELLS_SOULS_JACKET,
        GameItems.IDs.ARMOR_POLICE_JACKET,
        GameItems.IDs.ARMOR_POLICE_RIOT
      },
      new GameItems.IDs[4]
      {
        GameItems.IDs.ARMOR_FREE_ANGELS_JACKET,
        GameItems.IDs.ARMOR_HELLS_SOULS_JACKET,
        GameItems.IDs.ARMOR_POLICE_JACKET,
        GameItems.IDs.ARMOR_POLICE_RIOT
      }
    };
    public static readonly GameItems.IDs[][] GOOD_GANG_OUTFITS = new GameItems.IDs[5][]
    {
      new GameItems.IDs[0],
      new GameItems.IDs[1]
      {
        GameItems.IDs.ARMOR_HELLS_SOULS_JACKET
      },
      new GameItems.IDs[1]
      {
        GameItems.IDs.ARMOR_FREE_ANGELS_JACKET
      },
      new GameItems.IDs[0],
      new GameItems.IDs[0]
    };

    [Serializable]
    public enum IDs
    {
      NONE,
      BIKER_HELLS_SOULS,
      BIKER_FREE_ANGELS,
      GANGSTA_CRAPS,
      GANGSTA_FLOODS,
    }
  }
}
