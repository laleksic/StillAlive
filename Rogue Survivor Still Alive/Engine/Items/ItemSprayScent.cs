using System;
using System.Collections.Generic;
using System.Text;

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemSprayScent : Item
    {
        #region Properties
        public int SprayQuantity { get; set; }
        public Odor Odor { get { return (this.Model as ItemSprayScentModel).Odor; } } // alpha10
        public int Strength { get { return (this.Model as ItemSprayScentModel).Strength; } } // alpha10
        #endregion

        #region Init
        public ItemSprayScent(ItemModel model)
            : base(model)
        {
            ItemSprayScentModel itemModel = model as ItemSprayScentModel;
            //if (!(model is ItemSprayScentModel))
            if (itemModel == null)
                throw new ArgumentException("model is not a ItemScentSprayModel");

            //this.SprayQuantity = (model as ItemSprayScentModel).MaxSprayQuantity;
            this.SprayQuantity = itemModel.MaxSprayQuantity;
        }
        #endregion
    }
}
