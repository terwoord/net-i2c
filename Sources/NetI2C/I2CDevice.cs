using System;
using Microsoft.Win32.SafeHandles;

namespace TerWoord.NetI2C
{
    public class I2CDevice: I2CDeviceBase
    {
        private readonly SafeFileHandle mHandle;
        private readonly ushort mDeviceAddress;

        public I2CDevice(SafeFileHandle handle, ushort deviceAddress)
        {
            mHandle = handle;
            mDeviceAddress = deviceAddress;
        }

        public override void Dispose()
        {
            mHandle?.Dispose();
        }

        public byte ReadByte()
        {
            return DoReadByte(mHandle);
        }

        internal static byte DoReadByte(SafeFileHandle handle)
        {
            var xBuff = new byte[1];
            var xRead = LibC.Read(handle, xBuff, xBuff.Length);
            return xBuff[0];
        }

        public override void WriteByte(byte value)
        {
            var xBuff = new byte[1]
                        {
                            value
                        };
            Write(xBuff);
        }

        public override void Write(byte[] buff)
        {
            var xRead = LibC.Write(mHandle, buff, buff.Length);
        }

        public override unsafe void WriteRead(byte[] buff)
        {
            fixed (byte* xBufferPointer = &buff[0])
            {
                var xMessage = new I2CMessage();
                xMessage.Length = (ushort)buff.Length;
                xMessage.Buffer = xBufferPointer;
                xMessage.SlaveAddress = mDeviceAddress;
                I2CMessage* xMessagePointer = &xMessage;
                var xMessageContainer = new I2CReadWriteData();
                xMessageContainer.Messages = xMessagePointer;
                xMessageContainer.MessageCount = 1;

                I2CReadWriteData* xMessageContainerPtr = &xMessageContainer;
                var xResult = LibC.Ioctl(mHandle, Consts.I2C_RDWR, new IntPtr(xMessageContainerPtr));
                if (xResult < 0)
                {
                    throw new InvalidOperationException("Ioctl returned " + xResult);
                }
            }
        }
    }
}