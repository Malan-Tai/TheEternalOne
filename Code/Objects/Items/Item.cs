using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Objects.Items
{
    public class Item
    {
        public GameObject Owner { get; set; }

        public string Name
        {
            get
            {
                return Owner.Name;
            }
            set
            {
                Owner.Name = value;
            }
        }

        public Coord Position { get; set; }

        public bool Stackable { get; set; }
        public int Amount { get; set; }

        public Item(bool stackable = false, int amount = 0)
        {
            this.Stackable = stackable;
            this.Amount = amount;
        }

        public void PickUp()
        {
            if (GameManager.PlayerObject.Player.Inventory.Count < Player.MAX_INVENTORY_SLOTS && GameManager.PlayerObject.Player.CanPickUp)
            {
                if (Stackable)
                {
                    GameObject found = GameManager.PlayerObject.Player.FindObjectInInventory(Name);
                    if (found != null)
                    {
                        found.Item.Amount += this.Amount;
                    }
                    else
                    {
                        GameManager.PlayerObject.Player.Inventory.Add(Owner);
                    }
                }
                else
                {
                    GameManager.PlayerObject.Player.Inventory.Add(Owner);
                }
                if (GameManager.Objects.Contains(Owner))
                {
                    GameManager.Objects.Remove(Owner);
                }
            }
        }

        public virtual void Use()
        {

        }
    }
}
