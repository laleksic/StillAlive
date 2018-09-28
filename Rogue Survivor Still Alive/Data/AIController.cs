// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.AIController
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Data
{
  [Serializable]
  internal abstract class AIController : ActorController
  {
    public abstract ActorOrder Order { get; }

    public abstract ActorDirective Directives { get; set; }

    public abstract void SetOrder(ActorOrder newOrder);
  }
}
