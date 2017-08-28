using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace NetI2C
{
    public static class LibC
    {
        [DllImport("libc.so.6", EntryPoint = "open")]
        public static extern int Open(string fileName, int mode);

        [DllImport("libc.so.6", EntryPoint = "ioctl", SetLastError = true)]
        public extern static int Ioctl(SafeFileHandle fd, int request, int data);

        [DllImport("libc.so.6", EntryPoint = "read", SetLastError = true)]
        public static extern int Read(int handle, byte[] data, int length);
    }
}