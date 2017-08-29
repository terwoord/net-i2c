using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NetI2C;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Console.WriteLine("Available controllers: ");
            //foreach (var xItem in I2CControllerInfo.GetControllers())
            //{
            //    Console.WriteLine(" - #{0} {1}: {2}", xItem.Number, xItem.Name, xItem.Description);
            //}

            //Console.WriteLine();
            //Console.WriteLine("Now iterating controller:");
            I2CControllerBase xController = new I2CController(1);
            //var xItems = xController.FindDevices().ToArray();
            var xDevice = xController.OpenDevice(0x40);
            var xPwm = new AdaFruitPwmController(xDevice);
            xPwm.Initialize();
            xPwm.SetFrequency(1000);
            //xPwm.SetAllPwms(4096, 0);
            Console.WriteLine("Done");

        }

        private static void WriteRegister(I2CDevice device, byte register, byte value)
        {
            device.WriteByte(register);
            device.WriteByte(value);
        }
    }
}
