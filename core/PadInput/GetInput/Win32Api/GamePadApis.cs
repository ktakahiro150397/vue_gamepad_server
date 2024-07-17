using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PadInput.Win32Api
{

    /// <summary>
    /// Win32APIを呼び出す静的クラス。
    /// </summary>
    public static class NativeMethods
    {
        /// <summary>
        /// Queries the joystick driver for the number of joysticks it supports.
        /// https://docs.microsoft.com/ja-jp/windows/win32/api/joystickapi/nf-joystickapi-joygetnumdevs
        /// </summary>
        /// <returns></returns>
        [DllImport("winmm.dll")]
        public static extern int joyGetNumDevs();

        /// <summary>
        /// Queries a joystick for its position and button status.
        /// https://docs.microsoft.com/ja-jp/windows/win32/api/joystickapi/nf-joystickapi-joygetposex
        /// </summary>
        /// <param name="uJoyID"></param>
        /// <param name="pji"></param>
        /// <returns></returns>
        [DllImport("winmm.dll")]
        public static extern int joyGetPosEx(int uJoyID, ref JOYINFOEX pji);

        /// <summary>
        /// Queries a joystick to determine its capabilities.
        /// https://learn.microsoft.com/en-us/windows/win32/api/joystickapi/nf-joystickapi-joyGetDevCaps
        /// </summary>
        /// <param name="uJoyID"></param>
        /// <param name="pjc"></param>
        /// <param name="cbjc"></param>
        /// <returns></returns>
        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        public static extern uint joyGetDevCaps(int uJoyID, ref JOYCAPS pjc, int cbjc);

        /// <summary>
        /// The timeBeginPeriod function requests a minimum resolution for periodic timers.
        /// https://learn.microsoft.com/en-us/windows/win32/api/timeapi/nf-timeapi-timebeginperiod
        /// </summary>
        /// <param name="uuPeriod"></param>
        /// <returns></returns>
        [DllImport("Winmm.dll")]
        public static extern uint timeBeginPeriod(uint uuPeriod);

        /// <summary>
        /// The timeEndPeriod function clears a previously set minimum timer resolution.
        /// https://learn.microsoft.com/en-us/windows/win32/api/timeapi/nf-timeapi-timeendperiod
        /// </summary>
        /// <param name="uuPeriod"></param>
        /// <returns></returns>
        [DllImport("Winmm.dll")]

        public static extern uint timeEndPeriod(uint uuPeriod);


    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JOYINFOEX
    {
        public int dwSize { get; set; }

        public int dwFlags { get; set; }
        public int dwXpos { get; set; }
        public int dwYpos { get; set; }
        public int dwZpos { get; set; }
        public int dwRpos { get; set; }
        public int dwUpos { get; set; }
        public int dwVpos { get; set; }
        public int dwButtons { get; set; }
        public int dwButtonNumber { get; set; }
        public int dwPOV { get; set; }
        public int dwReserved1 { get; set; }
        public int dwReserved2 { get; set; }

        /// <summary>
        /// 現在のインスタンスに設定されている値で、新しいインスタンスを生成して返します。
        /// </summary>
        /// <returns></returns>
        public JOYINFOEX GetCopyStructure()
        {
            var ret = new JOYINFOEX();

            ret.dwSize = dwSize;
            ret.dwFlags = dwFlags;
            ret.dwXpos = dwXpos;
            ret.dwYpos = dwYpos;
            ret.dwZpos = dwZpos;
            ret.dwRpos = dwRpos;
            ret.dwUpos = dwUpos;
            ret.dwVpos = dwVpos;
            ret.dwButtons = dwButtons;
            ret.dwButtonNumber = dwButtonNumber;
            ret.dwPOV = dwPOV;
            ret.dwReserved1 = dwReserved1;
            ret.dwReserved2 = dwReserved2;

            return ret;
        }

        public static bool operator ==(JOYINFOEX left, JOYINFOEX right)
        {
            //すべての項目の値が一致する場合true。
            if (left.dwSize == right.dwSize &&
                left.dwFlags == right.dwFlags &&
                left.dwXpos == right.dwXpos &&
                left.dwYpos == right.dwYpos &&
                left.dwZpos == right.dwZpos &&
                left.dwRpos == right.dwRpos &&
                left.dwUpos == right.dwUpos &&
                left.dwVpos == right.dwVpos &&
                left.dwButtons == right.dwButtons &&
                left.dwButtonNumber == right.dwButtonNumber &&
                left.dwPOV == right.dwPOV &&
                left.dwReserved1 == right.dwReserved1 &&
                left.dwReserved2 == right.dwReserved2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(JOYINFOEX left, JOYINFOEX right)
        {
            return !(left == right);
        }

        public override readonly bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is JOYINFOEX jOYINFOEX)
            {
                return this == jOYINFOEX;
            }

            return false;

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct JOYCAPS
    {
        public ushort wMid;
        public ushort wPid;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szPname;
        public uint wXmin;
        public uint wXmax;
        public uint wYmin;
        public uint wYmax;
        public uint wZmin;
        public uint wZmax;
        public uint wNumButtons;
        public uint wPeriodMin;
        public uint wPeriodMax;
        public uint wRmin;
        public uint wRmax;
        public uint wUmin;
        public uint wUmax;
        public uint wVmin;
        public uint wVmax;
        public uint wCaps;
        public uint wMaxAxes;
        public uint wNumAxes;
        public uint wMaxButtons;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szRegKey;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] // MAX_JOYSTICKOEMVXDNAME is typically 260
        public string szOEMVxD;
    }



    /// <summary>
    /// JoyStickIDの定数郡。
    /// </summary>
    public static class JoyStickIDs
    {
        public const int JOYSTICKID1 = 0;
        public const int JOYSTICKID2 = 1;
        public const int JOYSTICKID3 = 2;
        public const int JOYSTICKID4 = 3;
    }

    /// <summary>
    /// キー入力を取得するフラグの定数郡。
    /// </summary>
    public static class JoyReturnFlagValues
    {
        public const int JOY_RETURNX = 0x001;
        public const int JOY_RETURNY = 0x002;
        public const int JOY_RETURNZ = 0x004;
        public const int JOY_RETURNR = 0x008;
        public const int JOY_RETURNU = 0x010;
        public const int JOY_RETURNV = 0x020;
        public const int JOY_RETURNPOV = 0x040;
        public const int JOY_RETURNBUTTONS = 0x080;
        public const int JOY_RETURNALL = 0x0FF;

        public const int JOY_RETURNRAWDATA = 0x100;
        public const int JOY_RETURNPOVCTS = 0x200;
        public const int JOY_RETURNCENTERED = 0x400;
    }

    public static class JoyGetPosExReturnValue
    {
        public const int JOYERR_NOERROR = 0;      // No Error.
        public const int JOYERR_PARMS = 165;      // The specified joystick identifier is invalid.
        public const int JOYERR_UNPLUGGED = 167; // The specified joystick is not connected to the system.
        public const int MMSYSTEM_NODRIVER = 6;  // The joystick driver is not present.
    }



    public static class KeyConsts
    {
        public const int MM_JOY1MOVE = 0x3A0;//  Joystick JOYSTICKID1 changed position in the x- or y-direction.
        public const int MM_JOY2MOVE = 0x3A1;//  Joystick JOYSTICKID2 changed position in the x- or y-direction
        public const int MM_JOY1ZMOVE = 0x3A2;//  Joystick JOYSTICKID1 changed position in the z-direction.
        public const int MM_JOY2ZMOVE = 0x3A3;//  Joystick JOYSTICKID2 changed position in the z-direction.
        public const int MM_JOY1BUTTONDOWN = 0x3B5;//  A button on joystick JOYSTICKID1 has been pressed.
        public const int MM_JOY2BUTTONDOWN = 0x3B6;//  A button on joystick JOYSTICKID2 has been pressed.
        public const int MM_JOY1BUTTONUP = 0x3B7;//  A button on joystick JOYSTICKID1 has been released.
        public const int MM_JOY2BUTTONUP = 0x3B8;//  A button on joystick JOYSTICKID2 has been released.

        public const int MMSYSERR_BADDEVICEID = 2; // The specified joystick identifier is invalid.
        public const int MMSYSERR_NODRIVER = 6;
        public const int MMSYSERR_INVALPARAM = 11; // An invalid parameter was passed.
        public const int JOYERR_PARMS = 165;
        public const int JOYERR_NOCANDO = 166;     // Cannot capture joystick input because a required service (such as a Windows timer) is unavailable.
        public const int JOYERR_UNPLUGGED = 167;


    }

}
