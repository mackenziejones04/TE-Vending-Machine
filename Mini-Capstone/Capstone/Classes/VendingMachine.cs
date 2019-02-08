using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class VendingMachine
    {
        public List<VendingMachineSlot> Slots { get; set; }
        public VendingMachineSlot SelectedSlot { get; private set; }
        public decimal MoneyInTheVendingMachine { get; private set; }

        public VendingMachine()
        {
            Initialize();
        }

        private void Initialize()
        {
            SelectedSlot = new VendingMachineSlot();
            Slots = new List<VendingMachineSlot>
            {
                new VendingMachineSlot("A1"),
                new VendingMachineSlot("A2"),
                new VendingMachineSlot("A3"),
                new VendingMachineSlot("A4"),
                new VendingMachineSlot("B1"),
                new VendingMachineSlot("B2"),
                new VendingMachineSlot("B3"),
                new VendingMachineSlot("B4"),
                new VendingMachineSlot("C1"),
                new VendingMachineSlot("C2"),
                new VendingMachineSlot("C3"),
                new VendingMachineSlot("C4"),
                new VendingMachineSlot("D1"),
                new VendingMachineSlot("D2"),
                new VendingMachineSlot("D3"),
                new VendingMachineSlot("D4")
            };
        }

        public void AddMoneyToTheVendingMachine(int amount)
        {
            if (amount > 0.0M)
            {
                MoneyInTheVendingMachine += (decimal)amount;
            }
        }

        public decimal ReturnChangeToUser()
        {
            decimal returnAmount = MoneyInTheVendingMachine;
            ResetBalanceToZero();
            return returnAmount;
        }

        public bool SelectSlot(string slotName)
        {
            foreach (VendingMachineSlot slot in Slots)
            {
                if (slot.NameOfSlot == slotName)
                {
                    SelectedSlot = slot;
                    return true;
                }
            }
            return false;
        }

        public string DispenseItem()
        {
            string result = "";

            if (SelectedSlot.ItemInSlot != null)
            {
                if (MoneyInTheVendingMachine >= SelectedSlot.ItemInSlot.Price)
                {
                    //Check the slot row for the result string;
                    if (SelectedSlot.NameOfSlot != "")
                    {
                        switch (SelectedSlot.NameOfSlot[0])
                        {
                            case 'A':
                                result = "Crunch Crunch, Yum!";
                                break;
                            case 'B':
                                result = "Munch Munch, Yum!";
                                break;
                            case 'C':
                                result = "Glug Glug, Yum!";
                                break;
                            case 'D':
                                result = "Chew Chew, Yum!";
                                break;
                            default:
                                break;
                        }
                    }

                    //Decrement money in the vending machine by item price
                    MoneyInTheVendingMachine -= SelectedSlot.ItemInSlot.Price;       
                    
                    //Take an item from the slot
                    SelectedSlot.TakeItemFromSlot();
                }
                else
                {
                    result = "Not enough money to make the purchase!";
                }
            }
            return result;
        }

        private void ResetBalanceToZero()
        {
            MoneyInTheVendingMachine = 0.0M;
        }
    }
}
