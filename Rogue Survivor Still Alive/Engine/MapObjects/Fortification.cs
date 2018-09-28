// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.MapObjects.Fortification
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;

namespace djack.RogueSurvivor.Engine.MapObjects
{
  [Serializable]
  internal class Fortification : MapObject
  {
    public const int SMALL_BASE_HITPOINTS = 20;
    public const int LARGE_BASE_HITPOINTS = 40;

    public Fortification(string name, string imageID, int hitPoints)
      : base(name, imageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, hitPoints)
    {
    }
  }
}
