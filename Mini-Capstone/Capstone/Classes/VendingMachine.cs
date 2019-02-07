using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class VendingMachine
    {
        // TODO: put this somewhere => private string filePath = @"C:\VendingMachine";

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

        public void AddMoneyToTheVendingMachine(int bill)
        {
            if (bill > 0.0M)
            {
                MoneyInTheVendingMachine += (decimal)bill;
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
            return result;
        }

        private void ResetBalanceToZero()
        {
            MoneyInTheVendingMachine = 0.0M;
        }
    }
}
