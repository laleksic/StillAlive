// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionLeaveMap
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System.Drawing;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionLeaveMap : ActorAction
  {
    private Point m_ExitPoint;

    public Point ExitPoint
    {
      get
      {
        return this.m_ExitPoint;
      }
    }

    public ActionLeaveMap(Actor actor, RogueGame game, Point exitPoint)
      : base(actor, game)
    {
      this.m_ExitPoint = exitPoint;
    }

    public override bool IsLegal()
    {
      return this.m_Game.Rules.CanActorLeaveMap(this.m_Actor, out this.m_FailReason);
    }

    public override void Perform()
    {
      this.m_Game.DoLeaveMap(this.m_Actor, this.m_ExitPoint, true);
    }
  }
}
