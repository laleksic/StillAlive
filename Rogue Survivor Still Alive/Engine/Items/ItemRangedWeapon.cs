using System;

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemRangedWeapon : ItemWeapon
    {
        #region Fields
        int m_Ammo;
        AmmoType m_AmmoType;
        #endregion

        #region Properties
        public int Ammo
        {
            get { return m_Ammo; }
            set { m_Ammo = value; }
        }

        public AmmoType AmmoType
        {
            get { return m_AmmoType; }
        }

        public bool IsOneHanded //@@MP (Release 7-2)
        {
            get { return (this.Model as ItemRangedWeaponModel).IsOneHanded; }
        }
        #endregion

        #region Init
        public ItemRangedWeapon(ItemModel model)
            : base(model)
        {
            ItemRangedWeaponModel m = model as ItemRangedWeaponModel;
            if (m == null)
                throw new ArgumentException("model is not RangedWeaponModel");

            m_Ammo = m.MaxAmmo;
            m_AmmoType = m.AmmoType;
        }
        #endregion
    }
}
