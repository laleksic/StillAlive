// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionWait
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionWait : ActorAction
  {
    public ActionWait(Actor actor, RogueGame game)
      : base(actor, game)
    {
    }

    public override bool IsLegal()
    {
      return true;
    }

    public override void Perform()
    {
      this.m_Game.DoWait(this.m_Actor);
    }
  }
}
