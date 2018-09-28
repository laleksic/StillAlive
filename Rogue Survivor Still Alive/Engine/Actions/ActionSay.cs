// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionSay
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionSay : ActorAction
  {
    private Actor m_Target;
    private string m_Text;
    private RogueGame.Sayflags m_Flags;

    public ActionSay(Actor actor, RogueGame game, Actor target, string text, RogueGame.Sayflags flags)
      : base(actor, game)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      this.m_Target = target;
      this.m_Text = text;
      this.m_Flags = flags;
    }

    public override bool IsLegal()
    {
      return true;
    }

    public override void Perform()
    {
      this.m_Game.DoSay(this.m_Actor, this.m_Target, this.m_Text, this.m_Flags);
    }
  }
}
