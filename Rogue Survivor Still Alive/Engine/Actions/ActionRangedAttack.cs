// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionRangedAttack
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionRangedAttack : ActorAction
  {
    private List<Point> m_LoF = new List<Point>();
    private Actor m_Target;
    private FireMode m_Mode;

    public ActionRangedAttack(Actor actor, RogueGame game, Actor target, FireMode mode)
      : base(actor, game)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      this.m_Target = target;
      this.m_Mode = mode;
    }

    public ActionRangedAttack(Actor actor, RogueGame game, Actor target)
      : this(actor, game, target, FireMode.DEFAULT)
    {
    }

    public override bool IsLegal()
    {
      this.m_LoF.Clear();
      return this.m_Game.Rules.CanActorFireAt(this.m_Actor, this.m_Target, this.m_LoF, out this.m_FailReason);
    }

    public override void Perform()
    {
      this.m_Game.DoSingleRangedAttack(this.m_Actor, this.m_Target, this.m_LoF, this.m_Mode);
    }
  }
}
