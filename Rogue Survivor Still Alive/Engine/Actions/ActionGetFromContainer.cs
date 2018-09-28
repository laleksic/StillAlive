// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionGetFromContainer
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System.Drawing;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionGetFromContainer : ActorAction
  {
    private Point m_Position;

    public Item Item
    {
      get
      {
        return this.m_Actor.Location.Map.GetItemsAt(this.m_Position).TopItem;
      }
    }

    public ActionGetFromContainer(Actor actor, RogueGame game, Point position)
      : base(actor, game)
    {
      this.m_Position = position;
    }

    public override bool IsLegal()
    {
      return this.m_Game.Rules.CanActorGetItemFromContainer(this.m_Actor, this.m_Position, out this.m_FailReason);
    }

    public override void Perform()
    {
      this.m_Game.DoTakeFromContainer(this.m_Actor, this.m_Position);
    }
  }
}
