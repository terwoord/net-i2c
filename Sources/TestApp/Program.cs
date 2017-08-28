using System;
using System.Linq;
using NetI2C;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Available controllers: ");
            foreach (var xItem in I2CControllerInfo.GetControllers())
            {
                Console.WriteLine(" - #{0} {1}: {2}", xItem.Number, xItem.Name, xItem.Description);
            }

            Console.WriteLine();
            Console.WriteLine("Now iterating controller:");
            var xDevice = new I2CController(1);
            var xItems = xDevice.FindDevices().ToArray();
            Console.WriteLine("{0} devices found: {1}", xItems.Length, xItems.Aggregate(" ", (s, b) => s + " 0x" + b.ToString("X2")));
        }
    }
}
