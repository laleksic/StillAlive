// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionBump
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionBump : ActorAction
  {
    private readonly Direction m_Direction;
    private readonly Location m_NewLocation;
    private readonly ActorAction m_ConcreteAction;

    public Direction Direction
    {
      get
      {
        return this.m_Direction;
      }
    }

    public ActorAction ConcreteAction
    {
      get
      {
        return this.m_ConcreteAction;
      }
    }

    public ActionBump(Actor actor, RogueGame game, Direction direction)
      : base(actor, game)
    {
      this.m_Direction = direction;
      this.m_NewLocation = actor.Location + direction;
      this.m_ConcreteAction = game.Rules.IsBumpableFor(this.m_Actor, game, this.m_NewLocation, out this.m_FailReason);
    }

    public override bool IsLegal()
    {
      if (this.m_ConcreteAction == null)
        return false;
      return this.m_ConcreteAction.IsLegal();
    }

    public override void Perform()
    {
      if (this.m_ConcreteAction == null)
        return;
      this.m_ConcreteAction.Perform();
    }
  }
}
