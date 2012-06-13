using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace RemoteKeyboardServer
{
    class ShellToast
    {
        public ShellToast() { }
        public void Show()
        {
            if (this.Title == null)
            {
                throw new InvalidOperationException("Title is null or empty");
            }
            LocalNotificationInterop.LNTaskInfo lnTaskInfo = new LocalNotificationInterop.LNTaskInfo();
            lnTaskInfo.AppBaseUri = "http://digiex.net/?sharks#true";
            //TODO: Figure out a appid so this might work
            lnTaskInfo.AppId = 1234;
            lnTaskInfo.IsAppInForeground = 0;
            //if ((LocalNotificationInterop.LNGetAppTaskInfo(ref lnTaskInfo) >= 0) && (lnTaskInfo.IsAppInForeground == 0))
            {
                StringBuilder builder = new StringBuilder(lnTaskInfo.AppBaseUri);
                if (this.NavigationUri != null)
                {
                    if (this.NavigationUri.ToString().IndexOf('/') == 0)
                    {
                        if (builder.ToString().IndexOf('#') == -1)
                        {
                            builder.Append('#');
                        }
                        builder.Append(this.NavigationUri.ToString());
                    }
                    else
                    {
                        if (this.NavigationUri.ToString().IndexOf('?') != 0)
                        {
                            throw new UriFormatException();
                        }
                        builder.Append(this.NavigationUri.ToString());
                    }
                }
                Uri uri = new Uri(builder.ToString(), UriKind.Absolute);
                LocalNotificationInterop.MessageToastData mtData = new LocalNotificationInterop.MessageToastData
                {
                    AppId = lnTaskInfo.AppId
                };
                if (this.Title.Length > 0x40)
                {
                    mtData.Content = this.Title.Substring(0, 0x40);
                }
                else
                {
                    mtData.Title = this.Title;
                }
                if (this.Content.Length > 0x100)
                {
                    mtData.Content = this.Content.Substring(0, 0x100);
                }
                else
                {
                    mtData.Content = this.Content;
                }
                mtData.TaskUri = uri.ToString();
                mtData.SoundFile = null;
                Debug.WriteLine("Toasting!");
                LocalNotificationInterop.SHPostMessageToast(ref mtData);
            }
        }
        public string Content
        {
            get
            {
                return this._content;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Content");
                }
                this._content = value;
            }
        }
        public Uri NavigationUri
        {
            get
            {
                return this._navigationUri;
            }
            set
            {
                if ((value != null) && value.IsAbsoluteUri)
                {
                    throw new ArgumentOutOfRangeException("NavigationUri");
                }
                this._navigationUri = value;
            }
        }
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Title");
                }
                if (value == null)
                {
                    throw new ArgumentOutOfRangeException("Title");
                }
                this._title = value;
            }
        }
        private string _content;
        private Uri _navigationUri;
        private string _title;
        private const string ContentPropertyName = "Content";
        private const int MaxContentLength = 0x100;
        private const int MaxTitleLength = 0x40;
        private const string NavigationUriPropertyName = "NavigationUri";
        private const char PageNameSeparator = '#';
        private const string TitlePropertyName = "Title";

    }
    internal class LocalNotificationInterop
    {
            internal const int LN_MAX_APP_BASE_URI_SIZE = 0x100;
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal struct LNTaskInfo
            {
                internal int IsAppInForeground;
                internal ulong AppId;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x100)]
                internal string AppBaseUri;
            }
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal struct MessageToastData
            {
                public ulong AppId;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string Title;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string Content;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string TaskUri;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string SoundFile;
            }
            public LocalNotificationInterop()
            {
            }
            //[DllImport("PlatformInterop.dll")]
            //internal static extern int LNGetAppTaskInfo(ref LNTaskInfo lnTaskInfo);
            [DllImport("aygshell.dll")]
            internal static extern int SHPostMessageToast(ref MessageToastData mtData);
    }
}
