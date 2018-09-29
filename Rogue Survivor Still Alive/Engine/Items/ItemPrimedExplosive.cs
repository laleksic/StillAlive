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

        #endregion

        #region Properties
        public int FuseTimeLeft { get; set; }
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
    }
}
