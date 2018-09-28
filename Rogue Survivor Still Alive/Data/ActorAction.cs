// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.ActorAction
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Engine;
using System;

namespace djack.RogueSurvivor.Data
{
  internal abstract class ActorAction
  {
    protected readonly RogueGame m_Game;
    protected readonly Actor m_Actor;
    protected string m_FailReason;

    public string FailReason
    {
      get
      {
        return this.m_FailReason;
      }
      set
      {
        this.m_FailReason = value;
      }
    }

    protected ActorAction(Actor actor, RogueGame game)
    {
      if (actor == null)
        throw new ArgumentNullException(nameof (actor));
      if (game == null)
        throw new ArgumentNullException(nameof (game));
      this.m_Actor = actor;
      this.m_Game = game;
    }

    public abstract bool IsLegal();

    public abstract void Perform();
  }
}
