using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine.Items;

namespace djack.RogueSurvivor.Engine.Actions
{
    class ActionCookFood : ActorAction    //@@MP - added (Release 7-6)
    {
        #region Fields
        ItemFood m_ItemFood;
        Point m_FirePoint;
        #endregion

        #region Init
        public ActionCookFood(Actor actor, RogueGame game, ItemFood item, Point fire)
            : base(actor, game)
        {
            m_ItemFood = item;
            m_FirePoint = fire;
        }
        #endregion

        #region Implementation
        public override bool IsLegal()
        {
            return true;
        }

        public override void Perform()
        {
            m_Game.DoCookFood(m_Actor, m_ItemFood, m_FirePoint);
        }
        #endregion
    }
}
