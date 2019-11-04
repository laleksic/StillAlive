using System;

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemMeleeWeapon : ItemWeapon
    {
        #region Properties
        public bool IsFragile
        {
            get { return (this.Model as ItemMeleeWeaponModel).IsFragile; }
        }

        public bool IsOneHanded //@@MP (Release 7-2)
        {
            get { return (this.Model as ItemMeleeWeaponModel).IsOneHanded; }
        }

        // alpha10
        public int ToolBashDamageBonus
        {
            get { return (this.Model as ItemMeleeWeaponModel).ToolBashDamageBonus; }
        }

        public float ToolBuildBonus
        {
            get { return (this.Model as ItemMeleeWeaponModel).ToolBuildBonus; }
        }

        public bool IsTool
        {
            get { return (this.Model as ItemMeleeWeaponModel).IsTool; }
        }
        #endregion

        #region Init
        public ItemMeleeWeapon(ItemModel model)
            : base(model)
        {
            if (!(model is ItemMeleeWeaponModel))
                throw new ArgumentException("model is not a MeleeWeaponModel");
        }
        #endregion
    }
}
