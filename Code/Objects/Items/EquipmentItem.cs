using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Objects.Items
{
    class EquipmentItem : Item
    {
        public EquipmentItem()
        {
            this.Stackable = false;
        }

        public override void Use()
        {
            Owner.Equipment.ToggleEquip();
        }
    }
}
