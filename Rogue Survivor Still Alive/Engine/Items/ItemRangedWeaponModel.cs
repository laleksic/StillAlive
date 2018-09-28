// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.Items.ItemRangedWeaponModel
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
  internal class ItemRangedWeaponModel : ItemWeaponModel
  {
    private int m_MaxAmmo;
    private AmmoType m_AmmoType;

    public bool IsFireArm
    {
      get
      {
        return this.Attack.Kind == AttackKind.FIREARM;
      }
    }

    public bool IsBow
    {
      get
      {
        return this.Attack.Kind == AttackKind.BOW;
      }
    }

    public int MaxAmmo
    {
      get
      {
        return this.m_MaxAmmo;
      }
    }

    public AmmoType AmmoType
    {
      get
      {
        return this.m_AmmoType;
      }
    }

    public ItemRangedWeaponModel(string aName, string theNames, string imageID, Attack attack, int maxAmmo, AmmoType ammoType)
      : base(aName, theNames, imageID, attack)
    {
      this.m_MaxAmmo = maxAmmo;
      this.m_AmmoType = ammoType;
    }
  }
}
