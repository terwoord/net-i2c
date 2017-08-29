using System;
using System.Threading;
using NetI2C;

namespace TestApp
{
    public class AdaFruitPwmController
    {
        private static class Consts
        {
            public const int __MODE1 = 0x00;
            public const int __MODE2 = 0x01;
            public const int __SUBADR1 = 0x02;
            public const int __SUBADR2 = 0x03;
            public const int __SUBADR3 = 0x04;
            public const int __PRESCALE = 0xFE;
            public const int __LED0_ON_L = 0x06;
            public const int __LED0_ON_H = 0x07;
            public const int __LED0_OFF_L = 0x08;
            public const int __LED0_OFF_H = 0x09;
            public const int __ALL_LED_ON_L = 0xFA;
            public const int __ALL_LED_ON_H = 0xFB;
            public const int __ALL_LED_OFF_L = 0xFC;
            public const int __ALL_LED_OFF_H = 0xFD;

            public const byte BITS__RESTART = 0x80;
            public const byte BITS__SLEEP = 0x10;
            public const byte BITS__ALLCALL = 0x01;
            public const byte BITS__INVRT = 0x10;
            public const byte BITS__OUTDRV = 0x04;
        }
        private readonly I2CDeviceBase mDevice;

        public AdaFruitPwmController(I2CDeviceBase device)
        {
            mDevice = device ?? throw new ArgumentNullException(nameof(device));
        }


        public void SetFrequency(double frequency)
        {
            var xPrescaleValue = 25000000.0; // 25MHz
            xPrescaleValue /= 4096.0;
            xPrescaleValue /= frequency;
            xPrescaleValue -= 1.0;
            xPrescaleValue = Math.Floor(xPrescaleValue + 0.5);

            var xOldMode = Read8(Consts.__MODE1);
            var xNewMode = (byte)((xOldMode & 0x7F) | 0x10); // sleep
            Write8(Consts.__MODE1, xNewMode); // go to sleep
            Write8(Consts.__PRESCALE, (byte)Math.Floor(xPrescaleValue));
            Write8(Consts.__MODE1, xOldMode);
            Thread.Sleep(5);
            Write8(Consts.__MODE1, (byte)(xOldMode | 0x80));
        }

        private void Write8(byte register, byte value)
        {
            var xBuff = new[] { register, value };
            mDevice.Write(xBuff);
        }

        private byte Read8(byte register)
        {
            var xBuff = new[] { register };
            mDevice.WriteRead(xBuff);
            return xBuff[0];
        }

        public void Initialize()
        {
            //var settings = new I2cConnectionSettings((int)mConfiguration.I2cSlaveAddress);
            //settings.BusSpeed = I2cBusSpeed.FastMode;
            
            SetAllPwms(0, 0);
            Write8(Consts.__MODE2, Consts.BITS__OUTDRV);
            Write8(Consts.__MODE1, Consts.BITS__ALLCALL);
            Thread.Sleep(5);
            var xMode1 = Read8(Consts.__MODE1);
            xMode1 &= Consts.BITS__SLEEP;
            Write8(Consts.__MODE1, xMode1);
            Thread.Sleep(5);
        }

        public void SetAllPwms(short on, short off)
        {
            Write8(Consts.__ALL_LED_ON_L, (byte)(on & 0xFF));
            Write8(Consts.__ALL_LED_ON_H, (byte)(on >> 8));
            Write8(Consts.__ALL_LED_OFF_L, (byte)(off & 0xFF));
            Write8(Consts.__ALL_LED_OFF_H, (byte)(off >> 8));
        }

        public void SetPwm(byte channel, short on, short off)
        {
            Internal_SetChannel(channel, on, off);
        }

        private void Internal_SetChannel(byte channel, short on, short off)
        {
            Write8((byte)(Consts.__LED0_ON_L + 4 * channel), (byte)(on & 0xFF));
            Write8((byte)(Consts.__LED0_ON_H + 4 * channel), (byte)(on >> 8));
            Write8((byte)(Consts.__LED0_OFF_L + 4 * channel), (byte)(off & 0xFF));
            Write8((byte)(Consts.__LED0_OFF_H + 4 * channel), (byte)(off >> 8));
        }

        public void SetPwmOn(byte channel, short on)
        {
            SetPwm(channel, on, 0);
        }

        public int ChannelCount
        {
            get
            {
                return 16;
            }
        }

        public void SetChannel(byte channel, bool state)
        {
            if (channel > 15)
            {
                throw new InvalidOperationException("AdaFruit Pwm controller only has 16 channels!");
            }
            if (state)
            {
                SetPwm(channel, short.MaxValue, 0);
            }
            else
            {
                SetPwm(channel, 0, 0);
            }
        }
    }
}