namespace TerWoord.NetI2C
{
    public abstract class I2CControllerBase
    {
        public abstract I2CDeviceBase OpenDevice(ushort number);

    }
}