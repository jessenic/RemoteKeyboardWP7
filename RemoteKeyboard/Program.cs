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
        static void Main(string[] args)
        {
            Debug.WriteLine("Starting listener");
            //Tried to make toasts work, but they need a proper appid and I don't know what an appid would look like as an int32.
            ShellToast st = new ShellToast();
            st.Title = "Remote Keyboard";
            st.Content = "Starting listener";
            st.Show();
            var tcpListener = new TcpListener(IPAddress.Any, 23);
            tcpListener.Start();
            Debug.WriteLine("Modifying registry");
            //return IsRegistryKeyBitSet(HKey.LOCAL_MACHINE, @"Hardware\DeviceMap\Keybd", "Status", 0x80);
            Registry.LocalMachine.SetValue(@"Hardware\DeviceMap\Keybd\Status", 0x80);
            Debug.WriteLine("Starting loop");
            while (true)
            {
                var tcpClient = tcpListener.AcceptTcpClient();
                var clientStream = tcpClient.GetStream();
                while(true){
                    if (clientStream.DataAvailable)
                    {
                        int key = clientStream.ReadByte();
                        Debug.WriteLine(key);
                        SystemCalls.SendKey((byte)key);
                    }
                }
            }
        }
    };
    public class SystemCalls
    {
        //See more at http://msdn2.microsoft.com/en-us/library/ms927178.aspx
        //http://msdn.microsoft.com/en-us/library/bb431750.aspx
        public const byte VK_NONAME = 0xFC;  // Do nothing
        public const byte VK_ESC = 0x1B;  // Smartphone back-button
        public const byte VK_F4 = 0x73;  // Home Screen
        public const byte VK_APP6 = 0xC6;  // Lock the keys on Smartphone
        public const byte VK_F22 = 0x85;  // Lock the keys on PocketPC (VK_KEYLOCK)
        public const byte VK_F16 = 0x7F;  // Toggle Speakerphone
        public const byte VK_OFF = 0xDF;  // Power button

        /// <summary>
        /// Puts `key` into to global keyboard buffer
        /// </summary>
        /// <param name="key"></param>
        public static void SendKey(byte key)
        {
            const int KEYEVENTF_KEYUP = 0x02;
            const int KEYEVENTF_KEYDOWN = 0x00;
            try
            {
                keybd_eventEx(key, 0, KEYEVENTF_KEYDOWN, 0);
                keybd_eventEx(key, 0, KEYEVENTF_KEYUP, 0);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);
                sb.AppendLine(ex.ToString());
                Debug.Write(sb.ToString());
                using (StreamWriter outfile =
    new StreamWriter(@"\Program Files\sharks.txt"))
                {
                    outfile.Write(sb.ToString());
                    outfile.Close();
                }
            }
        }

        [DllImport("coredll", SetLastError = true)]
        private static extern void keybd_eventEx(byte bVk, byte bScan, int dwFlags, int guidPDD);
    }
}
