// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionSwitchPowerGenerator
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine.MapObjects;
using System;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionSwitchPowerGenerator : ActorAction
  {
    private PowerGenerator m_PowGen;

    public ActionSwitchPowerGenerator(Actor actor, RogueGame game, PowerGenerator powGen)
      : base(actor, game)
    {
      if (powGen == null)
        throw new ArgumentNullException(nameof (powGen));
      this.m_PowGen = powGen;
    }

    public override bool IsLegal()
    {
      return this.m_Game.Rules.IsSwitchableFor(this.m_Actor, this.m_PowGen, out this.m_FailReason);
    }

    public override void Perform()
    {
      this.m_Game.DoSwitchPowerGenerator(this.m_Actor, this.m_PowGen);
    }
  }
}
