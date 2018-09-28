// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionBuildFortification
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System.Drawing;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionBuildFortification : ActorAction
  {
    private Point m_BuildPos;
    private bool m_IsLarge;

    public ActionBuildFortification(Actor actor, RogueGame game, Point buildPos, bool isLarge)
      : base(actor, game)
    {
      this.m_BuildPos = buildPos;
      this.m_IsLarge = isLarge;
    }

    public override bool IsLegal()
    {
      return this.m_Game.Rules.CanActorBuildFortification(this.m_Actor, this.m_BuildPos, this.m_IsLarge);
    }

    public override void Perform()
    {
      this.m_Game.DoBuildFortification(this.m_Actor, this.m_BuildPos, this.m_IsLarge);
    }
  }
}
