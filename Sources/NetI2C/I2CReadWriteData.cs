namespace NetI2C
{
    public unsafe struct I2CReadWriteData
    {
        public I2CMessage* Messages;
        public uint MessageCount;
    }
}