using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using djack.RogueSurvivor.Data;

namespace djack.RogueSurvivor.Engine.Items
{
    class ItemMeleeWeaponModel : ItemWeaponModel
    {
        #region Properties
        public bool IsFragile { get; set; }
        public bool IsOneHanded { get; set; } //@@MP (Release 7-2)
        //@@MP (Release 7-6)
        public bool CanUseForDigging { get; set; }
        public bool CanUseForButchering { get; set; }
        public bool CanCutDownTrees { get; set; }
        public int Weight { get; set; }
        // alpha10
        public int ToolBashDamageBonus { get; set; }
        public float ToolBuildBonus { get; set; }
        public bool IsTool { get { return ToolBashDamageBonus != 0 || ToolBuildBonus != 0; } }
        #endregion

        #region Init
        public ItemMeleeWeaponModel(string aName, string theNames, string imageID, Attack attack)
            : base(aName, theNames, imageID, attack)
        {
        }
        #endregion
    }
}
