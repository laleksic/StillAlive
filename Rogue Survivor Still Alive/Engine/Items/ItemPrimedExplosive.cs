using System;
using System.Collections.Generic;
using System.Text;

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemPrimedExplosive : ItemExplosive
    {
        #region Fields
        Actor m_Owner; //@@MP (Release 6-6)
        #endregion

        #region Properties
        public int FuseTimeLeft { get; set; }

        public Actor Owner //@@MP (Release 6-6)
        {
            get
            {
                // cleanup dead owner reference
                if (m_Owner != null && m_Owner.IsDead)
                    m_Owner = null;

                return m_Owner;
            }
            set { m_Owner = value; }
        }
        #endregion

        #region Init
        public ItemPrimedExplosive(ItemModel model)
            : base(model, model)
        {
            ItemExplosiveModel itemModel = model as ItemExplosiveModel;
            //if (!(model is ItemExplosiveModel)) //@@MP (Release 5-7)
            if (itemModel == null)
                throw new ArgumentException("model is not ItemExplosiveModel");

            //this.FuseTimeLeft = (model as ItemExplosiveModel).FuseDelay;
            this.FuseTimeLeft = itemModel.FuseDelay;
        }
        #endregion

        #region Pre-saving
        //@@MP (Release 6-6)
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
