using System;

namespace djack.RogueSurvivor.Data
{
    class ItemModel
    {
        #region Fields
        int m_ID;
        string m_SingleName;
        string m_PluralName;
        bool m_IsPlural;
        bool m_IsAn;
        bool m_IsProper;
        string m_ImageID;
        string m_FlavorDescription;
        bool m_IsStackable;
        int m_StackingLimit;
        DollPart m_EquipmentSlot;
        bool m_DontAutoEquip;
        bool m_IsUnbreakable;
        bool m_IsRecreational; //@@MP (Release 5-7)
        bool m_IsFlameWeapon; //@@MP (Release 5-7)
        bool m_IsThrowable; //@@MP (Release 7-1)
        bool m_IsBatteryPowered; //@@MP (Release 7-1)
        bool m_CausesTileFires; //@@MP (Release 7-3)
        #endregion

        #region Properties
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public string SingleName
        {
            get { return m_SingleName; }
        }

        public string PluralName
        {
            get { return m_PluralName; }
        }

        public bool IsPlural
        {
            get { return m_IsPlural; }
            set { m_IsPlural = value; }
        }

        public bool IsAn
        {
            get { return m_IsAn; }
            set { m_IsAn = value; }
        }

        /// <summary>
        /// Is a proper noun. ie it's a named item eg 'RoguedJack's keyboard' //@@MP
        /// </summary>
        public bool IsProper
        {
            get { return m_IsProper; }
            set { m_IsProper = value; }
        }

        public string ImageID
        {
            get { return m_ImageID; }
        }

        public string FlavorDescription
        {
            get { return m_FlavorDescription; }
            set { m_FlavorDescription = value; }
        }

        public bool IsStackable
        {
            get { return m_IsStackable; }
            set { m_IsStackable = value; }
        }

        public int StackingLimit
        {
            get { return m_StackingLimit; }
            set { m_StackingLimit = value; }
        }

        public DollPart EquipmentPart
        {
            get { return m_EquipmentSlot; }
            set { m_EquipmentSlot = value; }
        }

        public bool IsEquipable
        {
            get { return m_EquipmentSlot != DollPart.NONE; }
        }

        public bool DontAutoEquip
        {
            get { return m_DontAutoEquip; }
            set { m_DontAutoEquip = value; }
        }

        public bool IsUnbreakable
        {
            get { return m_IsUnbreakable; }
            set { m_IsUnbreakable = value; }
        }

        /// <summary>
        /// Chance to set target on fire for direct attacks (eg flamethrower)
        /// </summary>
        public bool IsFlameWeapon //@@MP (Release 5-2)
        {
            get { return m_IsFlameWeapon; } //@@MP - removed the hard-coded model IDs (Release 5-7)
            set { m_IsFlameWeapon = value; }
        }

        /// <summary>
        /// Chance to set tile on fire with explosions (eg fuel pumps), ie. not the direct attack but a secondary effect
        /// </summary>
        public bool CausesTileFires //@@MP (Release 7-3)
        {
            get { return m_CausesTileFires; }
            set { m_CausesTileFires = value; }
        }

        /// <summary>
        /// Allows player to use item even if it's wasteful, ie. RogueGame.DoUseMedicineItem()
        /// </summary>
        public bool IsRecreational //@@MP (Release 5-7)
        {
            get { return m_IsRecreational; }
            set { m_IsRecreational = value; }
        }

        /// <summary>
        /// Can be thrown instead of just dropped (non-grenades)
        /// </summary>
        public bool IsThrowable //@@MP (Release 7-1)
        {
            get { return m_IsThrowable; }
            set { m_IsThrowable = value; }
        }

        public bool IsBatteryPowered //@@MP (Release 7-1)
        {
            get { return m_IsBatteryPowered; }
            set { m_IsBatteryPowered = value; }
        }
        #endregion

        #region Init
        public ItemModel(string aName, string theNames, string imageID)
        {
            m_SingleName = aName;
            m_PluralName = theNames;
            m_ImageID = imageID;
        }
        #endregion
    }
}
