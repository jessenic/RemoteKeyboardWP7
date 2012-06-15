using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace RemoteKeyboardServer
{
    class Program
    {
        public static bool byteMode = false; //Change this manually to change the mode
        static void Main(string[] args)
        {
            Debug.WriteLine("Starting listener");
            //Tried to make toasts work, but they need a proper appid and I don't know what an appid would look like as an int32.
            //ShellToast st = new ShellToast();
            //st.Title = "Remote Keyboard";
            //st.Content = "Starting listener";
            //st.Show();
            var tcpListener = new TcpListener(IPAddress.Any, 23);
            tcpListener.Start();
            //Debug.WriteLine("Modifying registry");
            //return IsRegistryKeyBitSet(HKey.LOCAL_MACHINE, @"Hardware\DeviceMap\Keybd", "Status", 0x80);
            //Registry.LocalMachine.SetValue(@"Hardware\DeviceMap\Keybd\Status", 0x80);
            Debug.WriteLine("Starting loop");
            while (true)
            {
                var tcpClient = tcpListener.AcceptTcpClient();
                using (var stream = tcpClient.GetStream())
                using (var streamReader = new StreamReader(stream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        if (byteMode)
                        {
                            int b = streamReader.BaseStream.ReadByte();
                            Debug.WriteLine("Got key " + b);
                            SystemCalls.SendKeyboardKey((byte)b);
                        }
                        else
                        {
                            string currentLine = streamReader.ReadLine();
                            Debug.WriteLine("Got Line " + currentLine);
                            SystemCalls.SendKeyboardString(currentLine);
                        }
                    }
                }
            }
        }
    };
    public class SystemCalls
    {
        #region ----------------- Keyboard functions ------------------
        /// <summary>
        /// Send a string to the keyboard
        /// </summary>
        /// <param name="Keys"></param>
        public static void SendKeyboardString(string Keys)
        {
            SendKeyboardString(Keys, KeyStateFlags.Down, IntPtr.Zero);
        }

        /// <summary>
        /// Send a string to the keyboard
        /// </summary>
        /// <param name="Keys"></param>
        /// <param name="Flags"></param>
        public static void SendKeyboardString(string Keys, KeyStateFlags Flags)
        {
            SendKeyboardString(Keys, Flags, IntPtr.Zero);
        }

        /// <summary>
        /// Send a string to the keyboard
        /// </summary>
        /// <param name="Keys"></param>
        /// <param name="Flags"></param>
        /// <param name="hWnd"></param>
        public static void SendKeyboardString(string Keys, KeyStateFlags Flags, IntPtr hWnd)
        {
            uint[] keys = new uint[Keys.Length];
            KeyStateFlags[] states = new KeyStateFlags[Keys.Length];
            KeyStateFlags[] dead = { KeyStateFlags.Dead };

            for (int k = 0; k < Keys.Length; k++)
            {
                states[k] = Flags;
                keys[k] = Convert.ToUInt32(Keys[k]);
            }

            if (!PostKeybdMessage(hWnd, 0, Flags, (uint)keys.Length, states, keys))
            {
                Debug.WriteLine(Marshal.GetLastWin32Error());
            }
            if (!PostKeybdMessage(hWnd, 0, dead[0], 1, dead, keys))
            {
                Debug.WriteLine(Marshal.GetLastWin32Error());
            }
        }

        /// <summary>
        /// Send a key to the keyboard
        /// </summary>
        /// <param name="VirtualKey"></param>
        public static void SendKeyboardKey(byte VirtualKey)
        {

            keybd_eventEx(VirtualKey, 0, 0, 0);
            keybd_eventEx(VirtualKey, 0, (int)KeyEvents.KeyUp, 0);
        }
        private enum KeyEvents
        {
            ExtendedKey = 0x0001,
            KeyUp = 0x0002,
            Silent = 0x0004
        }
        #endregion
        [DllImport("coredll", SetLastError = true)]
        private static extern void keybd_eventEx(byte bVk, byte bScan, int dwFlags, int guidPDD);

        [DllImport("coredll", SetLastError = true)]
        internal static extern bool PostKeybdMessage(IntPtr hwnd, uint vKey, KeyStateFlags flags, uint cCharacters, KeyStateFlags[] pShiftStateBuffer, uint[] pCharacterBuffer);
    }
    #region KeyStateFlags
    /// <summary>
    /// KeyStateFlags for Keyboard methods
    /// </summary>
    [Flags()]
    public enum KeyStateFlags : int
    {
        Toggled = 0x0001,		//	 is toggled.
        AsyncDown = 0x0002,		//	 went down since last GetAsync call.
        PrevDown = 0x0040,		//	 was previously down.
        Down = 0x0080,		//	 is currently down.
        AnyCtrl = 0x40000000,	//  L or R control is down.
        AnyShift = 0x20000000,	//  L or R shift is down.
        AnyAlt = 0x10000000,	//  L or R alt is down.
        Capital = 0x08000000,	//  VK_CAPITAL is toggled.
        LeftCtrl = 0x04000000,	//  L control is down.
        LeftShift = 0x02000000,	//  L shift is down.
        LeftAlt = 0x01000000,	//  L alt is down.
        LeftWin = 0x00800000,	//  L Win  is down.
        RightCtrl = 0x00400000,	//  R control is down.
        RightShift = 0x00200000,	//  R shift is down.
        RightAlt = 0x00100000,	//  R alt is down.
        RightWin = 0x00080000,	//  R Win  is down.
        Dead = 0x00020000,	//  Corresponding char is dead char.
        NoCharacter = 0x00010000,	//  No corresponding char.
        Language1 = 0x00008000,	//  Use for language specific shifts.
        NumLock = 0x00001000	//  NumLock toggled state.
    }
    #endregion
}
