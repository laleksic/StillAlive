// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Actions.ActionEquipItem
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;
using System;

namespace djack.RogueSurvivor.Engine.Actions
{
  internal class ActionEquipItem : ActorAction
  {
    private Item m_Item;

    public ActionEquipItem(Actor actor, RogueGame game, Item it)
      : base(actor, game)
    {
      if (it == null)
        throw new ArgumentNullException("item");
      this.m_Item = it;
    }

    public override bool IsLegal()
    {
      return this.m_Game.Rules.CanActorEquipItem(this.m_Actor, this.m_Item, out this.m_FailReason);
    }

    public override void Perform()
    {
      this.m_Game.DoEquipItem(this.m_Actor, this.m_Item);
    }
  }
}
