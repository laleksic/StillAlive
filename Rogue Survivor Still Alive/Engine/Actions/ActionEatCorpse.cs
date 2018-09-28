// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionEatCorpse
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionEatCorpse : ActorAction
  {
    private readonly Corpse m_Target;

    public ActionEatCorpse(Actor actor, RogueGame game, Corpse target)
      : base(actor, game)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      this.m_Target = target;
    }

    public override bool IsLegal()
    {
      return this.m_Game.Rules.CanActorEatCorpse(this.m_Actor, this.m_Target, out this.m_FailReason);
    }

    public override void Perform()
    {
      this.m_Game.DoEatCorpse(this.m_Actor, this.m_Target);
    }
  }
}
