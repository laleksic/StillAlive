// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionUseExit
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System.Drawing;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionUseExit : ActorAction
  {
    private Point m_ExitPoint;

    public ActionUseExit(Actor actor, Point exitPoint, RogueGame game)
      : base(actor, game)
    {
      this.m_ExitPoint = exitPoint;
    }

    public override bool IsLegal()
    {
      return this.m_Game.Rules.CanActorUseExit(this.m_Actor, this.m_ExitPoint, out this.m_FailReason);
    }

    public override void Perform()
    {
      this.m_Game.DoUseExit(this.m_Actor, this.m_ExitPoint);
    }
  }
}
