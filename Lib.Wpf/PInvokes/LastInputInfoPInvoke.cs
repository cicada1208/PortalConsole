using System;
using System.Runtime.InteropServices;

namespace Lib.Wpf.PInvokes
{
    public class LastInputInfoPInvoke
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }

        /// <summary>
        /// Gets the time of the last user input (in milliseconds since the system started)
        /// <para>user input: e.g. mouse, keyboard  and touch</para>
        /// </summary>
        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        /// <summary>
        /// 獲取鼠標鍵盤觸控閒置時間
        /// </summary>
        /// <returns>milliseconds</returns>
        public static long GetIdleTime()
        {
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = Marshal.SizeOf(lastInputInfo);
            if (!GetLastInputInfo(ref lastInputInfo)) return 0;
            return Environment.TickCount - lastInputInfo.dwTime;
        }

    }
}
