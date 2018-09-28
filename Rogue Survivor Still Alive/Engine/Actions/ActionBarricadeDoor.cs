// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionBarricadeDoor
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine.MapObjects;
using System;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionBarricadeDoor : ActorAction
  {
    private DoorWindow m_Door;

    public ActionBarricadeDoor(Actor actor, RogueGame game, DoorWindow door)
      : base(actor, game)
    {
      if (door == null)
        throw new ArgumentNullException(nameof (door));
      this.m_Door = door;
    }

    public override bool IsLegal()
    {
      return this.m_Game.Rules.CanActorBarricadeDoor(this.m_Actor, this.m_Door, out this.m_FailReason);
    }

    public override void Perform()
    {
      this.m_Game.DoBarricadeDoor(this.m_Actor, this.m_Door);
    }
  }
}
