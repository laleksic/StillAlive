using System;
using System.Collections.Generic;
using System.Text;

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.MapObjects
{
    [Serializable]
    class Car : MapObject   //@@MP (Release 7-1)
    {
        #region Fields
        int m_FuelUnits;
        #endregion

        #region Properties
        public int FuelUnits
        {
            get { return m_FuelUnits; }
            set { m_FuelUnits = value; }
        }
        #endregion

            #region Init
        public Car(string name, string imageID, int fuelUnits)
            : base(name, imageID, Break.BROKEN, Fire.UNINFLAMMABLE, 0)
        {
            m_FuelUnits = fuelUnits;
        }
        #endregion
    }
}
