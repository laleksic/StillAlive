// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.TrustRecord
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal class TrustRecord
  {
    public Actor Actor { get; set; }

    public int Trust { get; set; }
  }
}
