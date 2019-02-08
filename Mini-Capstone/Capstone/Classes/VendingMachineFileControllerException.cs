using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class VendingMachineFileControllerException : Exception
    {
        public VendingMachineFileControllerException(string message) :
            base(message)
        {

        }
    }
}
