using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Gameplay.Generators;
using djack.RogueSurvivor.Engine.Items;

namespace djack.RogueSurvivor.Engine.Tasks
{
    [Serializable]
    class TaskRegrowWildBerries : TimedTask //@@MP - new to Release 4
    {
        private int m_X, m_Y, m_TimeNow, m_BestBeforeDays;
        private ItemFoodModel m_ItemFoodModel;

        public TaskRegrowWildBerries(int turns, int x, int y, int timeNow, int bestBeforeDays, ItemFoodModel itemFoodModel)
            : base(turns)
        {
            m_X = x;
            m_Y = y;
            m_TimeNow = timeNow;
            m_BestBeforeDays = bestBeforeDays;
            m_ItemFoodModel = itemFoodModel;
        }

        public override void Trigger(Map m)
        {
            System.Drawing.Point pt = new System.Drawing.Point(m_X, m_Y);
            int freshUntil = m_TimeNow + WorldTime.TURNS_PER_DAY * m_BestBeforeDays;
            Item berries = new ItemFood(m_ItemFoodModel, freshUntil)
            {
                Quantity = 3
            };
            m.DropItemAt(berries, pt);
        }
    }
}
