using System;

namespace TerWoord.NetI2C
{
    public abstract class I2CDeviceBase: IDisposable
    {
        public abstract void Write(byte[] buff);
        public abstract void WriteRead(byte[] buff);
        public abstract void WriteByte(byte value);

        public virtual void Dispose()
        {
        }
    }
}