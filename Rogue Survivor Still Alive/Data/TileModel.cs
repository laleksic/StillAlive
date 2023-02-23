using System.Drawing;

namespace djack.RogueSurvivor.Data
{
    class TileModel
    {
        #region Blank tile model
        public static readonly TileModel UNDEF = new TileModel("", Color.Pink, false, true, false);
        #endregion

        #region Fields
        int m_ID;
        string m_ImageID;
        bool m_IsWalkable;
        bool m_IsTransparent;
        Color m_MinimapColor;
        bool m_IsFlammable; //@@MP (Release 7-6)
        #endregion

        #region Properties
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public string ImageID
        {
            get { return m_ImageID; }
        }

        public bool IsWalkable
        {
            get { return m_IsWalkable; }
        }

        public bool IsTransparent
        {
            get { return m_IsTransparent; }
        }

        public Color MinimapColor
        {
            get { return m_MinimapColor; }
        }

        public bool IsWater
        {
            get;
            set;
        }

        public string WaterCoverImageID
        {
            get;
            set;
        }

        /// <summary>
        /// Is a model of tile that is prone to decay as time goes by in the world
        /// </summary>
        public bool CanDecay //@@MP (Release 7-6)
        {
            get;
            set;
        }

        public bool IsFlammable //@@MP (Release 7-6)
        {
            get { return m_IsFlammable; }
        }

        #endregion

        #region Init
        public TileModel(string imageID, Color minimapColor, bool IsWalkable, bool IsTransparent, bool IsFlammable)
        {
            m_ImageID = imageID;
            m_IsWalkable = IsWalkable;
            m_IsTransparent = IsTransparent;
            m_MinimapColor = minimapColor;
            m_IsFlammable = IsFlammable; //@@MP (Release 7-6)
        }
        #endregion
    }
}
