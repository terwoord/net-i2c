using System;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;

namespace TerWoord.NetI2C
{
    public class I2CController: I2CControllerBase
    {
        private readonly int mNumber;

        public I2CController(int number)
        {
            //if (I2CControllerInfo.GetControllers().All(i => i.Number != number))
            //{
            //    throw new ArgumentException("Specified controller not found!", nameof(number));
            //}
            mNumber = number;
        }

        public IEnumerable<byte> FindDevices()
        {
            using (var xHandle = OpenController())
            {
                Console.WriteLine("Device opened!");
                for (byte i = 0; i < 128; i++)
                {
                    var xResult = LibC.Ioctl(xHandle, Consts.I2C_Slave, new IntPtr(i));
                    if (xResult != 0)
                    {
                        continue;
                    }

                    try
                    {
                        var xByteRead = I2CDevice.DoReadByte(xHandle);
                    }
                    catch
                    {
                        continue;
                    }
                    yield return i;

                }
            }
        }

        private SafeFileHandle OpenController()
        {
            var xHandle = LibC.Open("/dev/i2c-" + mNumber, Consts.OPEN_READ_WRITE);
            if (xHandle < 0)
            {
                throw new InvalidOperationException("Open return " + xHandle);
            }

            var xSafeHandle = new SafeFileHandle(new IntPtr(xHandle), true);
            return xSafeHandle;
        }

        public override I2CDeviceBase OpenDevice(ushort number)
        {
            var xHandle = OpenController();
            var xError = LibC.Ioctl(xHandle, Consts.I2C_Slave, new IntPtr(number));

            if (xError != 0)
            {
                throw new InvalidOperationException("Ioctl returned " + xError);
            }

            return new I2CDevice(xHandle, number);
        }
    }
}