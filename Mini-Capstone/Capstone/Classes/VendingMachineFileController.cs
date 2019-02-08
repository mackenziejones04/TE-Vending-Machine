using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Capstone.Classes
{
    public static class VendingMachineFileController
    {
        public static List<VendingMachineSlot> ReadIn(List<VendingMachineSlot> slots)
        {
            string filePath = @"C:\VendingMachine\";
            string fileName = "vendingmachine.csv";
            string fullFilePath = Path.Combine(filePath, fileName);

            if (slots.Count == 0)
            {
                throw new VendingMachineFileControllerException(
                    "Readin() Error: Slots list not already initialized");
            }

            //Possible exceptions while reading!
            using (StreamReader sr = new StreamReader(fullFilePath))
            {
                while (!sr.EndOfStream)
                {
                    //read in each line
                    string lineIn = sr.ReadLine();                

                    // parse slot name | item name | item price
                    string[] parsedLine = lineIn.Split('|');

                    //Will throw exceptions if any errors are found
                    CheckReadInErrors(parsedLine, lineIn);

                    //Create slot name, item name and decimal price from parsed line
                    string slotName = parsedLine[0];
                    string itemName = parsedLine[1];
                    decimal.TryParse(parsedLine[2], out decimal itemPrice); //guaranteed to work at this point because of CheckReadInErrors

                    //Look for matching slot in list
                    foreach (VendingMachineSlot vms in slots)
                    {
                        if (vms.NameOfSlot == slotName)
                        {
                            //make new item with name and price
                            //put item into matching slot in list
                            vms.PlaceItemInSlot(new VendingMachineItem(itemName, itemPrice));
                        }
                    }
                }
            }

            //After reading in, check that enough unique items were found
            foreach (VendingMachineSlot vms in slots )
            {
                if (vms.ItemInSlot == null)
                {
                    throw new VendingMachineFileControllerException(
                        $"ReadIn() Error : Not enough items given. There must be 16 unique items");
                }
            }
            return slots;
        }
    
        public static void UpdateLogFile(string action, decimal balance, decimal endingBalance)
        {
            string filePath = @"C:\VendingMachine\";
            string fileName = "Log.txt";
            string fullFilePath = Path.Combine(filePath, fileName);

            using (StreamWriter sw = new StreamWriter(fullFilePath, true))
            {
                string dateTimeString = DateTime.UtcNow.ToString();
                string balanceString = String.Format("{0:C2}", balance);
                string endingBalanceString = String.Format("{0:C2}", endingBalance);

                string output = String.Format(
                    "{0,-21}{1,-25}{2,-8}{3,-8}",
                    dateTimeString,
                    action,
                    balanceString,
                    endingBalanceString);
                sw.WriteLine(output);
            }
        }

        private static void CheckReadInErrors(string[] parsedLine, string lineIn)
        {
            //Check if two '|' exist (should give 3 entries)
            if (parsedLine.Length != 3)
            {
                throw new VendingMachineFileControllerException(
                    $"ReadIn() Error : Wrong delimiter or number of delimiters on line \'{lineIn}\'");
            }
            //Check if slot name is length 2
            if (parsedLine[0].Length != 2)
            {
                throw new VendingMachineFileControllerException(
                    $"ReadIn() Error : Slot name is incorrect length with line \'{lineIn}\'");
            }
            else
            {
                //Check if slot name begins with A,B,C or D
                if (parsedLine[0][0] != 'A' &&
                    parsedLine[0][0] != 'B' &&
                    parsedLine[0][0] != 'C' &&
                    parsedLine[0][0] != 'D')
                {
                    throw new VendingMachineFileControllerException(
                        $"ReadIn() Error : Slot name does not begin with A,B,C or D with line \'{lineIn}\'");
                }
                //Check if slot name ends with 1,2,3 or 4
                if (parsedLine[0][1] != '1' &&
                    parsedLine[0][1] != '2' &&
                    parsedLine[0][1] != '3' &&
                    parsedLine[0][1] != '4')
                {
                    throw new VendingMachineFileControllerException(
                        $"ReadIn() Error : Slot name does not end with 1,2,3 or 4 with line \'{lineIn}\'");
                }
            }
            //Check that item name is not empty
            if (string.IsNullOrEmpty(parsedLine[1]))
            {
                throw new VendingMachineFileControllerException(
                    $"ReadIn() Error : Item name is empty with line \'{lineIn}\'");
            }
            //Check that price is a number and not blank
            if (!(decimal.TryParse(parsedLine[2], out decimal priceFound)))
            {
                throw new VendingMachineFileControllerException(
                    $"ReadIn() Error : Item price is not a decimal number with line \'{lineIn}\'");
            }
            //Check that the price is positive
            if (priceFound <= 0.00M)
            {
                throw new VendingMachineFileControllerException(
                    $"ReadIn() Error : Item price is negative with line \'{lineIn}\'");
            }
        }

        public static void GenerateSalesReport(VendingMachine vm)
        {
            string filePath = @"C:\VendingMachine\";
            string dateTime = $"{DateTime.Now:MM-dd-yyyy_HH_mm_ss} Sales Report.txt";
            string fullFilePath = Path.Combine(filePath, dateTime);

            using (StreamWriter sw = new StreamWriter(fullFilePath, true))
            {
                decimal totalSales = 0.00M;
                foreach (VendingMachineSlot slot in vm.Slots)
                {
                    int qtySold = 5 - slot.QuantityOfItemInSlot;
                    totalSales += qtySold * slot.ItemInSlot.Price;
                    sw.WriteLine(slot.ItemInSlot.Name + "|" + qtySold);
                }

                sw.WriteLine($"\n** TOTAL SALES **\t {totalSales:C2}");
            }
        }
    }
}
