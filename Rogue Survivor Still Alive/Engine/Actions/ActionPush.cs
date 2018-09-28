// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionPush
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;
using System.Drawing;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionPush : ActorAction
  {
    private readonly MapObject m_Object;
    private readonly Direction m_Direction;
    private readonly Point m_To;

    public Direction Direction
    {
      get
      {
        return this.m_Direction;
      }
    }

    public Point To
    {
      get
      {
        return this.m_To;
      }
    }

    public ActionPush(Actor actor, RogueGame game, MapObject pushObj, Direction pushDir)
      : base(actor, game)
    {
      if (pushObj == null)
        throw new ArgumentNullException(nameof (pushObj));
      this.m_Object = pushObj;
      this.m_Direction = pushDir;
      this.m_To = pushObj.Location.Position + pushDir;
    }

    public override bool IsLegal()
    {
      if (this.m_Game.Rules.CanActorPush(this.m_Actor, this.m_Object))
        return this.m_Game.Rules.CanPushObjectTo(this.m_Object, this.m_To, out this.m_FailReason);
      return false;
    }

    public override void Perform()
    {
      this.m_Game.DoPush(this.m_Actor, this.m_Object, this.m_To);
    }
  }
}
