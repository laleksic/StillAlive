using System;

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemTrap : Item
    {
        #region Fields
        bool m_IsActivated;
        bool m_IsTriggered;
        Actor m_Owner; // alpha10
        #endregion

        #region Properties
        public bool IsActivated
        {
            get { return m_IsActivated;}
            // set { m_IsActivated=value;} //alpha10
        }

        public bool IsTriggered
        {
            get { return m_IsTriggered; }
            set { m_IsTriggered = value; }
        }

        public ItemTrapModel TrapModel { get { return Model as ItemTrapModel; } }

        // alpha10
        public Actor Owner
        {
            get
            {
                // cleanup dead owner reference
                if (m_Owner != null && m_Owner.IsDead)
                    m_Owner = null;

                return m_Owner;
            }
        }
        #endregion

        #region Init
        public ItemTrap(ItemModel model)
            : base(model)
        {
            if (!(model is ItemTrapModel))
                throw new ArgumentException("model is not a TrapModel");
        }
        #endregion

        #region Cloning
        /// <summary>
        /// A new trap of the same model, un-activated, no owner, un-triggered.
        /// </summary>
        public ItemTrap Clone()
        {
            ItemTrap c = new ItemTrap(TrapModel);
            return c;
        }
        #endregion

        // alpha10
        #region Activating/Deactivating
        public void Activate(Actor owner)
        {
            m_Owner = owner;
            m_IsActivated = true;
        }

        public void Deactivate()
        {
            m_Owner = null;
            m_IsActivated = false;
        }
        #endregion

        // alpha10
        #region Pre-saving
        public override void OptimizeBeforeSaving()
        {
            base.OptimizeBeforeSaving();

            // cleanup dead owner ref
            if (m_Owner != null && m_Owner.IsDead)
                m_Owner = null;
        }
        #endregion
    }
}
