using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
    class ItemBackpackModel : ItemModel
    {
        #region Fields
        readonly int m_Inventory_Slots;
        readonly int m_Encumbrance;
        readonly int m_Weight;
        #endregion

        #region Properties
        public int Inventory_Slots
        {
            get { return m_Inventory_Slots; }
        }
        public int Encumbrance
        {
            get { return m_Encumbrance; }
        }
        public int Weight
        {
            get { return m_Weight; }
        }
        #endregion

        #region Init
        public ItemBackpackModel(string aName, string theNames, string imageID, int inventory_slots, int encumbrance, int weight)
            : base(aName, theNames, imageID)
        {
            m_Inventory_Slots = Math.Min(inventory_slots, 10);
            m_Encumbrance = encumbrance;
            m_Weight = weight;
        }
        #endregion

    }
}
