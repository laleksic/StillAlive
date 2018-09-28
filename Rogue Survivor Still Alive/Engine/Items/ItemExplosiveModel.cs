// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemExplosiveModel
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
  internal class ItemExplosiveModel : ItemModel
  {
    private int m_FuseDelay;
    private BlastAttack m_Attack;
    private string m_BlastImageID;

    public int FuseDelay
    {
      get
      {
        return this.m_FuseDelay;
      }
    }

    public BlastAttack BlastAttack
    {
      get
      {
        return this.m_Attack;
      }
    }

    public string BlastImage
    {
      get
      {
        return this.m_BlastImageID;
      }
    }

    public ItemExplosiveModel(string aName, string theNames, string imageID, int fuseDelay, BlastAttack attack, string blastImageID)
      : base(aName, theNames, imageID)
    {
      this.m_FuseDelay = fuseDelay;
      this.m_Attack = attack;
      this.m_BlastImageID = blastImageID;
    }
  }
}
