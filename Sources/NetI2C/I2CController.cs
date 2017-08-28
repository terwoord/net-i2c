using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Schema;

namespace NetI2C
{
    public class I2CController
    {
        private readonly int mNumber;

        public I2CController(int number)
        {
            if (I2CControllerInfo.GetControllers().All(i => i.Number != number))
            {
                throw new ArgumentException("Specified controller not found!", nameof(number));
            }
            mNumber = number;
        }

        public IEnumerable<byte> FindDevices()
        {
            using (var xStream = OpenDevice(mNumber))
            {
                Console.WriteLine("Device opened!");
                for (byte i = 0; i < 128; i++)
                {
                    var xResult = LibC.Ioctl(xStream.SafeFileHandle, Consts.I2C_Slave, i);
                    if (xResult != 0)
                    {
                        continue;
                    }

                    try
                    {
                        var xByteRead = xStream.ReadByte();
                    }
                    catch
                    {
                        continue;
                    }
                    yield return i;

                }
                yield break;
            }
        }

        private static FileStream OpenDevice(int number)
        {
            return new FileStream("/dev/i2c-" + number, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite, 1);
        }
    }
}