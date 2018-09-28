// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemGrenadeModel
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
  internal class ItemGrenadeModel : ItemExplosiveModel
  {
    private int m_MaxThrowDistance;

    public int MaxThrowDistance
    {
      get
      {
        return this.m_MaxThrowDistance;
      }
    }

    public ItemGrenadeModel(string aName, string theNames, string imageID, int fuseDelay, BlastAttack attack, string blastImageID, int maxThrowDistance)
      : base(aName, theNames, imageID, fuseDelay, attack, blastImageID)
    {
      this.m_MaxThrowDistance = maxThrowDistance;
    }
  }
}
