// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionThrowGrenade
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine.Items;
using System.Collections.Generic;
using System.Drawing;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionThrowGrenade : ActorAction
  {
    private Point m_ThrowPos;

    public ActionThrowGrenade(Actor actor, RogueGame game, Point throwPos)
      : base(actor, game)
    {
      this.m_ThrowPos = throwPos;
    }

    public override bool IsLegal()
    {
      return this.m_Game.Rules.CanActorThrowTo(this.m_Actor, this.m_ThrowPos, (List<Point>) null, out this.m_FailReason);
    }

    public override void Perform()
    {
      if (this.m_Actor.GetEquippedWeapon() is ItemPrimedExplosive)
        this.m_Game.DoThrowGrenadePrimed(this.m_Actor, this.m_ThrowPos);
      else
        this.m_Game.DoThrowGrenadeUnprimed(this.m_Actor, this.m_ThrowPos);
    }
  }
}
