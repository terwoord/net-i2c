using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace TerWoord.NetI2C
{
    public static class LibC
    {
        [DllImport("libc.so.6", EntryPoint = "open")]
        public static extern int Open(string fileName, int mode);

        [DllImport("libc.so.6", EntryPoint = "ioctl", SetLastError = true)]
        public static extern int Ioctl(SafeFileHandle fd, int request, IntPtr data);

        [DllImport("libc.so.6", EntryPoint = "read", SetLastError = true)]
        public static extern int Read(SafeFileHandle handle, byte[] data, int length);

        [DllImport("libc.so.6", EntryPoint = "write", SetLastError = true)]
        public static extern int Write(SafeFileHandle handle, byte[] data, int length);
    }
}