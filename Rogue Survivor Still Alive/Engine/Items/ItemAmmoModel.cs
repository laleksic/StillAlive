using System;
using System.Collections.Generic;
using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
    enum AmmoType
    {
        _FIRST = 0,

        LIGHT_PISTOL = _FIRST,
        HEAVY_PISTOL,
        SHOTGUN,
        LIGHT_RIFLE,
        HEAVY_RIFLE,
        BOLT,
        NAIL, //@@MP (Release 5-1)
        PRECISION_RIFLE, //@@MP (Release 6-6)
        FUEL, //@@MP (Release 7-1)
        CHARGE, //@@MP (Release 7-2)
        MINIGUN, //@@MP (Release 7-6)
        GRENADES, //@@MP (Release 7-6)
        PLASMA, //@@MP (Release 7-6)

        _COUNT
    }

    class ItemAmmoModel : ItemModel
    {
        #region Fields
        AmmoType m_AmmoType;
        #endregion

        #region Properties
        public AmmoType AmmoType
        {
            get { return m_AmmoType; }
        }

        public int MaxQuantity
        {
            get { return this.StackingLimit; }
        }
        #endregion

        #region Init
        public ItemAmmoModel(string aName, string theNames, string imageID, AmmoType ammoType, int maxQuantity)
            : base(aName, theNames, imageID)
        {
            m_AmmoType = ammoType;
            this.IsStackable = true;
            this.StackingLimit = maxQuantity;
        }
        #endregion
    }
}
