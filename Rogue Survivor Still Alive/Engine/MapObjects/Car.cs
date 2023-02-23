using System;

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.MapObjects
{
    [Serializable]
    class Car : MapObject   //@@MP (Release 7-1)
    {
        #region Fields
        int m_FuelUnits;
        int m_MaxFuelUnits;
        #endregion

        #region Properties
        public int FuelUnits
        {
            get { return m_FuelUnits; }
            set { m_FuelUnits = value; }
        }

        public int MaxFuelUnits
        {
            get { return m_MaxFuelUnits; }
            set { m_MaxFuelUnits = value; }
        }
        #endregion

        #region Init
        public Car(string name, string imageID, MapObject.Break breakState, int fuelUnits)
            : base(name, imageID, breakState, Fire.UNINFLAMMABLE, 0)
        {
            m_FuelUnits = fuelUnits;
            m_MaxFuelUnits = 99;
        }
        #endregion
    }
}
