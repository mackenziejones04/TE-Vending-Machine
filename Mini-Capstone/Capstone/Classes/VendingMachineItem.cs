using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class VendingMachineItem
    {
        private decimal price;

        public string Name { get; set; }
        public decimal Price
        {
            get
            {
                return price;
            }
            set
            {
                if (value < 0.0M)
                {
                    price = 0.0M;
                }
                else
                {
                    price = value;
                }
            }
        }

        public VendingMachineItem()
        {

        }

        public VendingMachineItem(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public bool Equals(VendingMachineItem otherItem)
        {
            return (otherItem.Name == this.Name);
        }
    }
}
