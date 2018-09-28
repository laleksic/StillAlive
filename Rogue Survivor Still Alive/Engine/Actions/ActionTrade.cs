// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionTrade
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionTrade : ActorAction
  {
    private readonly Actor m_Target;

    public ActionTrade(Actor actor, RogueGame game, Actor target)
      : base(actor, game)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      this.m_Target = target;
    }

    public override bool IsLegal()
    {
      return this.m_Game.Rules.CanActorInitiateTradeWith(this.m_Actor, this.m_Target);
    }

    public override void Perform()
    {
      this.m_Game.DoTrade(this.m_Actor, this.m_Target);
    }
  }
}
