using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
    class ItemBodyArmorModel : ItemModel
    {
        #region Fields
        int m_Protection_Hit;
        int m_Protection_Shot;
        int m_Encumbrance;
        int m_Weight;
        int m_Fire_Resistance; //@@MP (Release 7-1)
        #endregion

        #region Properties
        public int Protection_Hit
        {
            get { return m_Protection_Hit; }
        }
        public int Protection_Shot
        {
            get { return m_Protection_Shot; }
        }
        public int Encumbrance
        {
            get { return m_Encumbrance; }
        }
        public int Weight
        {
            get { return m_Weight; }
        }
        public int Fire_Resistance //@@MP (Release 7-1)
        {
            get { return m_Fire_Resistance; }
        }
        #endregion

        #region Init
        public ItemBodyArmorModel(string aName, string theNames, string imageID, int protection_hit, int protection_shot, int encumbrance, int weight, int fire_resistance)
            : base(aName, theNames, imageID)
        {
            m_Protection_Hit = protection_hit;
            m_Protection_Shot = protection_shot;
            m_Encumbrance = encumbrance;
            m_Weight = weight;
            m_Fire_Resistance = fire_resistance; //@@MP (Release 7-1)
        }
        #endregion

        #region Conversion
        public Defence ToDefence()
        {
            return new Defence(-m_Encumbrance, m_Protection_Hit, m_Protection_Shot);
        }
        #endregion
    }
}
