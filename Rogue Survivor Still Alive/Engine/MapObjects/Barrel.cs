using System;

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.MapObjects
{
    [Serializable]
    class Barrel : MapObject   //@@MP (Release 7-6)
    {
        #region Fields
        int m_FuelUnits;
        int m_MaxFuelUnits;
        #endregion

        #region Properties
        /// <summary>
        /// Wood left to burn before fire goes out
        /// </summary>
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
        public Barrel(string name, string imageID, MapObject.Break breakState, int fuelUnits)
            : base(name, imageID, breakState, Fire.BURNABLE, DoorWindow.BASE_HITPOINTS)
        {
            m_FuelUnits = fuelUnits;
            m_MaxFuelUnits = WorldTime.TURNS_PER_DAY; //720
        }
        #endregion
    }
}
