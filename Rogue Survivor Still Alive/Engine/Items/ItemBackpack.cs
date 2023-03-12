using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Gameplay;

namespace djack.RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemBackpack : Item
    {
        #region State
        Inventory m_Inventory = null;
        #endregion

        #region Properties
        public int Inventory_Slots { get; private set; }
        public int Encumbrance { get; private set; }
        public int Weight { get; private set; }

        public Inventory Inventory
        {
            get { return m_Inventory; }
            set { m_Inventory = value; }
        }

        #endregion

        #region Init
        public ItemBackpack(ItemModel model)
            : base(model)
        {
            ItemBackpackModel m = model as ItemBackpackModel;
            if (m == null)
                throw new ArgumentException("model is not a BackpackModel");

            this.Inventory_Slots = m.Inventory_Slots;
            this.Encumbrance = m.Encumbrance;
            this.Weight = m.Weight;
            m_Inventory = new Inventory(m.Inventory_Slots);
        }
        #endregion
    }
}