// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionMoveStep
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System.Drawing;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionMoveStep : ActorAction
  {
    private Location m_NewLocation;

    public ActionMoveStep(Actor actor, RogueGame game, Direction direction)
      : base(actor, game)
    {
      this.m_NewLocation = actor.Location + direction;
    }

    public ActionMoveStep(Actor actor, RogueGame game, Point to)
      : base(actor, game)
    {
      this.m_NewLocation = new Location(actor.Location.Map, to);
    }

    public override bool IsLegal()
    {
      return this.m_Game.Rules.IsWalkableFor(this.m_Actor, this.m_NewLocation, out this.m_FailReason);
    }

    public override void Perform()
    {
      this.m_Game.DoMoveActor(this.m_Actor, this.m_NewLocation);
    }
  }
}
