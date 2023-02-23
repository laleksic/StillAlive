using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemFood : Item
    {
        #region Properties
        public int Nutrition { get; private set; }
        public bool IsPerishable { get; private set; }
        public WorldTime BestBefore { get; private set; }
        public bool CanCauseFoodPoisoning { get; private set; } //@@MP - added meats to the game (Release 7-6)
        public bool CanBeCooked { get; private set; } //@@MP - added meats to the game (Release 7-6)
        public int CookedDegree { get; set; } //@@MP - added meats to the game (Release 7-6)
        public int MaxCookedDegree { get; set; } //@@MP - added meats to the game (Release 7-6)
        #endregion

        #region Init
        /// <summary>
        /// Not perishable.
        /// </summary>
        public ItemFood(ItemModel model)
            : base(model)
        {
            ItemFoodModel itemModel = model as ItemFoodModel;
            //if (!(model is ItemFoodModel)) //@@MP (Release 5-7)
            if (itemModel == null)
                throw new ArgumentException("model is not a FoodModel");

            //this.Nutrition = (model as ItemFoodModel).Nutrition;
            this.Nutrition = itemModel.Nutrition;
            this.IsPerishable = false;
            this.CanCauseFoodPoisoning = false;
            this.CanBeCooked = false;
        }

        /// <summary>
        /// Perishable food.
        /// </summary>
        /// <param name="model">ItemFoodModel</param>
        /// <param name="bestBefore">-1 for non perishable food.</param>
        /// <param name="canCauseFoodPoisoning">eg. raw meats</param>
        /// <param name="canBeCooked">eg. raw meats</param>
        public ItemFood(ItemModel model, int bestBefore, bool canCauseFoodPoisoning, bool canBeCooked)
            : base(model)
        {
            ItemFoodModel itemModel = model as ItemFoodModel;
            //if (!(model is ItemFoodModel)) //@@MP (Release 5-7)
            if (itemModel == null)
                throw new ArgumentException("model is not a FoodModel");

            //this.Nutrition = (model as ItemFoodModel).Nutrition;
            this.Nutrition = itemModel.Nutrition;
            this.BestBefore = new WorldTime(bestBefore);
            this.IsPerishable = true;
            this.CanCauseFoodPoisoning = canCauseFoodPoisoning;
            this.CanBeCooked = canBeCooked;
            if (canBeCooked)
            {
                this.CookedDegree = 0;
                this.MaxCookedDegree = 4; //it takes four turns to turn raw meat into cooked meat on a fire
            }
        }
        #endregion
    }
}
