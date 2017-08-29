namespace NetI2C
{
    public unsafe struct I2CMessage
    {
        public ushort SlaveAddress;
        public ushort Flags;
        public ushort Length;
        public byte* Buffer;
    }
}