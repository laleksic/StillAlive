// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionBreak
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionBreak : ActorAction
  {
    private MapObject m_Obj;

    public ActionBreak(Actor actor, RogueGame game, MapObject obj)
      : base(actor, game)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      this.m_Obj = obj;
    }

    public override bool IsLegal()
    {
      return this.m_Game.Rules.IsBreakableFor(this.m_Actor, this.m_Obj, out this.m_FailReason);
    }

    public override void Perform()
    {
      this.m_Game.DoBreak(this.m_Actor, this.m_Obj);
    }
  }
}
