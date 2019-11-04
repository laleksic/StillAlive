using System;

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
    class ItemRangedWeaponModel : ItemWeaponModel
    {
        #region Fields
        int m_MaxAmmo;
        AmmoType m_AmmoType;
        bool m_IsSingleShot; //@@MP (Release 6-6)
        bool m_IsOneHanded; //@@MP (Release 7-2)
        #endregion

        #region Properties
        public bool IsFireArm
        {
            get { return this.Attack.Kind == AttackKind.FIREARM; }
        }

        public bool IsBow
        {
            get { return this.Attack.Kind == AttackKind.BOW; }
        }

        public bool IsSingleShot //@@MP - not capable of Rapid Fire (Release 6-6)
        {
            get { return m_IsSingleShot; }
        }

        public int MaxAmmo
        {
            get { return m_MaxAmmo; }
        }

        public AmmoType AmmoType
        {
            get { return m_AmmoType; }
        }

        public bool IsOneHanded //@@MP (Release 7-2)
        {
            get { return m_IsOneHanded; }
        }

        // alpha10
        public int RapidFireHit1Value
        {
            get { return Attack.Hit2Value; }
        }

        public int RapidFireHit2Value
        {
            get { return Attack.Hit3Value; }
        }
        #endregion

        #region Init
        public ItemRangedWeaponModel(string aName, string theNames, string imageID, Attack attack, int maxAmmo, AmmoType ammoType, bool isSingleShot, bool isOneHanded) //@@MP - added isSingleShot (Release 6-6), added isOneHanded (Release 7-2)
            : base(aName, theNames, imageID, attack)
        {
            m_MaxAmmo = maxAmmo;
            m_AmmoType = ammoType;
            m_IsSingleShot = isSingleShot;
            m_IsOneHanded = isOneHanded;
        }
        #endregion
    }
}
