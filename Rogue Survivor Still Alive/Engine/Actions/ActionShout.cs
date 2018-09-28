// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionShout
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionShout : ActorAction
  {
    private string m_Text;

    public ActionShout(Actor actor, RogueGame game)
      : this(actor, game, (string) null)
    {
    }

    public ActionShout(Actor actor, RogueGame game, string text)
      : base(actor, game)
    {
      this.m_Text = text;
    }

    public override bool IsLegal()
    {
      return this.m_Game.Rules.CanActorShout(this.m_Actor, out this.m_FailReason);
    }

    public override void Perform()
    {
      this.m_Game.DoShout(this.m_Actor, this.m_Text);
    }
  }
}
