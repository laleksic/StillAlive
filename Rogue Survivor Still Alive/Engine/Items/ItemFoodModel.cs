using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
    class ItemFoodModel : ItemModel
    {
        #region Fields
        int m_Nutrition;
        bool m_IsPerishable;
        int m_BestBeforeDays;
        bool m_CanCauseFoodPoisoning; //@@MP - added meats to the game (Release 7-6)
        bool m_CanBeCooked; //@@MP - added meats to the game (Release 7-6)
        #endregion

        #region Properties
        public int Nutrition
        {
            get { return m_Nutrition; }
        }

        public bool IsPerishable
        {
            get { return m_IsPerishable; }
        }

        public int BestBeforeDays
        {
            get { return m_BestBeforeDays; }
        }

        public bool CanCauseFoodPoisoning //@@MP - added meats to the game (Release 7-6)
        {
            get { return m_CanCauseFoodPoisoning; }
        }

        public bool CanBeCooked //@@MP - added meats to the game (Release 7-6)
        {
            get { return m_CanBeCooked; }
        }
        #endregion

        #region Init
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aName"></param>
        /// <param name="theNames"></param>
        /// <param name="imageID"></param>
        /// <param name="nutrition"></param>
        /// <param name="bestBeforeDays">-1 for non perishable food.</param>
        /// <param name="canCauseFoodPoisoning">eg. raw meats</param>
        /// <param name="canBeCooked">eg. raw meats</param>
        public ItemFoodModel(string aName, string theNames, string imageID, int nutrition, int bestBeforeDays, bool canCauseFoodPoisoning, bool canBeCooked)
            : base(aName, theNames, imageID)
        {
            m_Nutrition = nutrition;
            if (bestBeforeDays < 0)
                m_IsPerishable = false;
            else
            {
                m_IsPerishable = true;
                m_BestBeforeDays = bestBeforeDays;
            }

            m_CanCauseFoodPoisoning = canCauseFoodPoisoning; //@@MP (Release 7-6)
            m_CanBeCooked = canBeCooked; //@@MP (Release 7-6)
        }
        #endregion
    }
}
