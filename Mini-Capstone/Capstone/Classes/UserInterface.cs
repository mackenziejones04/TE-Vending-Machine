using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Capstone.Classes
{
    public class UserInterface
    {
        //Initialize vending machine
        private VendingMachine vendingMachine = new VendingMachine();

        public void RunInterface()
        {
            bool done = false;

            try
            {
                vendingMachine.Slots = VendingMachineFileController.ReadIn(vendingMachine.Slots);
            }
            catch (IOException e)
            {
                done = true;
                Console.WriteLine("FATAL ERROR : File access error, quitting program!");
                Console.WriteLine(e.Message);
            }
            catch (VendingMachineFileControllerException e)
            {
                done = true;
                Console.WriteLine("FATAL ERROR: Problems parsing the read in file!");
                Console.WriteLine(e.Message);
            }

            while (!done)
            {
                Console.WriteLine("(1) Display Vending Machine Items\n(2) Purchase\n(3) End");
                string inputEntry = Console.ReadLine();

                if (!(inputEntry == "1" || inputEntry == "2" || inputEntry == "3" || inputEntry == "9"))
                {
                    Console.WriteLine("Invalid entry, try again");
                    continue;
                }

                switch (inputEntry)
                {
                    case "1":
                        DisplayVendingMachineItems();
                        break;
                    case "2":
                        Purchase();
                        break;
                    case "3":
                        End();
                        done = true;
                        break;
                    case "9":
                        SalesReport();
                        break;
                    default:
                        continue;
                }
            }

            Console.ReadLine();
        }

        public void DisplayVendingMachineItems()
        {
            foreach (VendingMachineSlot vms in vendingMachine.Slots)
            {
                string qty = vms.QuantityOfItemInSlot.ToString();
                if (qty == "0")
                {
                    qty = "SOLD OUT";
                }
                Console.WriteLine($"{vms.NameOfSlot}|{vms.ItemInSlot.Name}|" +
                    $"{vms.ItemInSlot.Price:C2}|QTY={qty}");
            }
            Console.WriteLine();
        }

        public void Purchase()
        {
            bool done = false;

            while (!done)
            {
                Console.WriteLine("(1) Feed Money\n(2) Select Product\n(3) Finish Transaction");
                Console.WriteLine($"Current Money Provided: {vendingMachine.MoneyInTheVendingMachine:C2}");
                string inputEntry = Console.ReadLine();

                if (!(inputEntry == "1" || inputEntry == "2" || inputEntry == "3"))
                {
                    Console.WriteLine("Invalid entry, try again");
                    continue;
                }

                switch (inputEntry)
                {
                    case "1":
                        FeedMoney();
                        break;
                    case "2":
                        SelectProduct();
                        break;
                    case "3":
                        FinishTransaction();
                        done = true;
                        break;
                    default:
                        continue;
                }
            }
        }

        public void FeedMoney()
        {
            Console.Write("Enter a bill: ");
            string billEntered = Console.ReadLine();
            if (!(int.TryParse(billEntered, out int bill)))
            {
                Console.WriteLine("Bill entered is not a number");
            }
            else
            {
                if (!(bill == 1 || bill == 2 || bill == 5 || bill == 10))
                {
                    Console.WriteLine("Bill entered must be: 1, 2, 5 or 10");
                }
                else
                {
                    vendingMachine.AddMoneyToTheVendingMachine(bill);
                    try
                    {
                        VendingMachineFileController.UpdateLogFile(
                            "FEED MONEY:",
                            bill * 1.00M,
                            vendingMachine.MoneyInTheVendingMachine);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ERROR updating Log file : " + e.Message);
                    }
                }
            }
        }

        public void SelectProduct()
        {
            DisplayVendingMachineItems();
            Console.WriteLine("Enter your slot name (A2 for example):");
            string selection = Console.ReadLine();

            if (!vendingMachine.SelectSlot(selection))
            {
                Console.WriteLine("Invalid entry selected");
            }
            else if (vendingMachine.SelectedSlot.IsEmpty)
            {
                Console.WriteLine("Item is sold out!");
            }
            else
            {
                decimal balanceBefore = vendingMachine.MoneyInTheVendingMachine;
                Console.WriteLine(vendingMachine.DispenseItem() + "\n");
                try
                {
                    VendingMachineFileController.UpdateLogFile(
                           vendingMachine.SelectedSlot.ItemInSlot.Name,
                           balanceBefore,
                           vendingMachine.MoneyInTheVendingMachine);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR updating Log file : " + e.Message);
                }
            }
        }

        public void FinishTransaction()
        {
            decimal returnChange = vendingMachine.ReturnChangeToUser();

            try
            {
                VendingMachineFileController.UpdateLogFile(
                       "GIVE CHANGE:",
                       returnChange,
                       vendingMachine.MoneyInTheVendingMachine);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR updating Log file : " + e.Message);
            }
            int quarters = 0;
            int dimes = 0;
            int nickels = 0;

            while (returnChange != 0.00M)
            {
                if (returnChange >= 0.25M)
                {
                    quarters++;
                    returnChange -= 0.25M;
                }
                else if (returnChange >= 0.10M)
                {
                    dimes++;
                    returnChange -= 0.10M;
                }
                else if (returnChange >= 0.05M)
                {
                    nickels++;
                    returnChange -= 0.05M;
                }
            }
            Console.WriteLine($"Returning {quarters} quarters, {dimes} dimes, and {nickels} nickels");
        }

        public void SalesReport()
        {
            try
            {
                VendingMachineFileController.GenerateSalesReport(vendingMachine);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR in generate sales report : " + e.Message);
            }
        }

        public void End()
        {
            Console.WriteLine("Ending interface!");
        }
    }
}
