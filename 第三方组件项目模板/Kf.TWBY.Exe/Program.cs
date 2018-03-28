using System;
using System.Windows.Forms;

namespace Kf.TWBY
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Kf.Util.Constants.SysVersionToWrite = "Kf-TWBY";
            Kf.Util.Constants.InitCustomesConfig();
            if (args.Length == 10)
            {
                try
                {
                    if (Kf.Login.Login.NoUIDoLogin.DoLogin(args[0], Convert.ToInt32(args[1]), args[2], args[3], Convert.ToInt32(args[4]), args[5], args[6], args[7], args[8], args[9]))
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run((Form)new Kf.Runtime.MenuUI.MainUI(true));
                    }
                }
                catch
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run((Form)new Kf.Runtime.MenuUI.MainUI());
                }
            }
            else if ((args.Length >= 1) && (args.Length <= 2))
            {
                try
                {
                    Kf.Util.Constants.SetConfigData("User", args[0]);
                    if (args.Length == 2)
                    {
                        Kf.Util.Base.EncryptTripleDes des = new Kf.Util.Base.EncryptTripleDes();
                        Kf.Util.Constants.SetConfigData("Password", des.Encrypt(args[1].Trim()));
                    }
                    Kf.Util.Constants.SetConfigData("AutoLogin", "True");
                }
                catch
                {
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run((Form)new Kf.Runtime.MenuUI.MainUI());
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run((Form)new Kf.Runtime.MenuUI.MainUI());
            }
        }

    }
}
