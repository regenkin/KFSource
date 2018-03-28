using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Security.AccessControl;
using System.IO;
using System.Diagnostics;


namespace InternetInstall
{
    [RunInstaller(true)]
    public partial class KfInstaller : System.Configuration.Install.Installer
    {
        public KfInstaller()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 应用程序入口
        /// </summary>
        public static void Main(string[] args)
        {

        }

        /// <summary>
        /// 重写安装完成后函数
        /// 实现安装完成后自动启动已安装的程序
        /// </summary>
        /// <param name="savedState"></param>
        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);
            //string setDir =Context.Parameters["targetdir"].ToString();
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            string path = asm.Location.Remove(asm.Location.LastIndexOf("\\"));
            string WebDir = path.Remove(path.LastIndexOf("\\"));
            //string strCMDExe = path.Replace("(", @"^(").Replace(")", @"^)") + @"\Kf.InstallIIS.exe ";
            //string strAgr = WebDir.Replace("(", @"^(").Replace(")", @"^)") ;

            //安装IIS
            string strCMDExe = path + @"\Kf.InstallIIS.exe ";
            string strAgr = WebDir;
            strAgr = "\"" + strAgr + "\"" + " KFWeb 1";
            AddSecurityControll2Folder(WebDir);//给目录设置权限
            System.Diagnostics.Process.Start(strCMDExe, strAgr);
            System.Threading.Thread.Sleep(1000);

            //安装 KfService服务
            strCMDExe = path + @"\Kf.StartService.exe";
            strAgr = "Kf.Service.exe KfService";
            //AddSecurityControll2Folder(path);//给目录设置权限
            Process pro = System.Diagnostics.Process.Start(strCMDExe, strAgr);
            System.Threading.Thread.Sleep(1000);
            //安装 KfAgent代理服务
            strCMDExe = path + @"\Kf.StartService.exe";
            strAgr = "Kf.Agent.exe KFAgent";
            //AddSecurityControll2Folder(path);//给目录设置权限
            System.Diagnostics.Process.Start(strCMDExe, strAgr);
            System.Threading.Thread.Sleep(1000);
            //string setDir =Context.Parameters["targetdir"].ToString();
            //System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            //string path = asm.Location.Remove(asm.Location.LastIndexOf("\\"));
            //string WebDir = path.Remove(path.LastIndexOf("\\"));
            //string strCMDExe = path.Replace("(", @"^(").Replace(")", @"^)") + @"\Kf.InstallIIS.exe ";
            //string strAgr = WebDir.Replace("(", @"^(").Replace(")", @"^)") ;
            //SetUpKfService();
        }
        /// <summary>
        /// 重写安装过程方法
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }
        /// <summary>
        /// 重写安装之前方法
        /// </summary>
        /// <param name="savedState"></param>
        protected override void OnBeforeInstall(IDictionary savedState)
        {
            base.OnBeforeInstall(savedState);
        }
        /// <summary>
        /// 重写卸载之前方法
        /// </summary>
        /// <param name="savedState"></param>
        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            string strCMDExe;
            string strAgr;
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            string path = asm.Location.Remove(asm.Location.LastIndexOf("\\"));

            //卸载TCPHTTP服务
            strCMDExe = path + @"\Kf.StartService.exe ";
            strAgr = " -u Kf.Service.exe";
            System.Diagnostics.Process.Start(strCMDExe, strAgr);
            System.Threading.Thread.Sleep(1000);
            //卸载代理服务
            strCMDExe = path + @"\Kf.StartService.exe ";
            strAgr = " -u Kf.Agent.exe";
            System.Diagnostics.Process.Start(strCMDExe, strAgr);
            System.Threading.Thread.Sleep(8000);

            base.OnBeforeUninstall(savedState);
        }
        /// <summary>
        /// 重写卸载方法
        /// </summary>
        /// <param name="savedState"></param>
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
        }

        /// <summary>
        /// 重写回滚方法
        /// </summary>
        /// <param name="savedState"></param>
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        /// <summary>
        ///为文件夹添加users，everyone用户组的完全控制权限
        /// </summary>
        /// <param name="dirPath"></param>
        public void AddSecurityControll2Folder(string dirPath)
        {
            //获取文件夹信息
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            //获得该文件夹的所有访问权限
            System.Security.AccessControl.DirectorySecurity dirSecurity = dir.GetAccessControl(AccessControlSections.All);
            //设定文件ACL继承
            InheritanceFlags inherits = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
            //添加ereryone用户组的访问权限规则 完全控制权限
            FileSystemAccessRule everyoneFileSystemAccessRule = new FileSystemAccessRule("NETWORK SERVICE", FileSystemRights.FullControl, inherits, PropagationFlags.None, AccessControlType.Allow);
            //添加Users用户组的访问权限规则 完全控制权限
            FileSystemAccessRule usersFileSystemAccessRule = new FileSystemAccessRule("Users", FileSystemRights.FullControl, inherits, PropagationFlags.None, AccessControlType.Allow);
            bool isModified = false;
            dirSecurity.ModifyAccessRule(AccessControlModification.Add, everyoneFileSystemAccessRule, out isModified);
            dirSecurity.ModifyAccessRule(AccessControlModification.Add, usersFileSystemAccessRule, out isModified);
            //设置访问权限
            dir.SetAccessControl(dirSecurity);
        }

        void SetUpKfService()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            //重新定向标准输入，输入，错误输出 
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            //设置cmd窗口不显示 
            //process.StartInfo.CreateNoWindow = true;
            //开始 
            process.Start();
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.Trim();
            process.StandardInput.WriteLine(path.Substring(0, 2));
            process.StandardInput.WriteLine("cd " + path);
            process.StandardInput.WriteLine("Kf.StartService.exe Kf.Service.exe KfService");
            process.StandardInput.WriteLine("exit");
            process.Close();
            System.Threading.Thread.Sleep(10000);
            process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            //重新定向标准输入，输入，错误输出 
            process.StartInfo.RedirectStandardInput = true;
            //process.StartInfo.RedirectStandardOutput = true;
            //process.StartInfo.RedirectStandardError = true;
            ////设置cmd窗口不显示 
            //process.StartInfo.CreateNoWindow = true;
            //开始 
            process.Start();
            process.StandardInput.WriteLine("net  start KfService");
            System.Threading.Thread.Sleep(5000);
            process.StandardInput.WriteLine("net  start KfService");
            process.StandardInput.WriteLine("exit");
            process.Close();
            Console.WriteLine("安装完成");
        }

        void UnLoadKfService()
        {
            Console.WriteLine("正在安装服务:KfService");
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            //重新定向标准输入，输入，错误输出 
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            //设置cmd窗口不显示 
            //process.StartInfo.CreateNoWindow = true;
            //开始 
            process.Start();
            process.StandardInput.WriteLine("/c net stop KfService");
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.Trim();
            process.StandardInput.WriteLine(path.Substring(0, 2));
            process.StandardInput.WriteLine("cd " + path);
            process.StandardInput.WriteLine("Kf.StartService.exe -U Kf.Service.exe");
            process.StandardInput.WriteLine("exit");
            process.Close();
            Console.WriteLine("卸载服务完成");
        }

        public static bool Is64Sys()
        {
            return Environment.Is64BitOperatingSystem;
        }

        private void CreateAndWriteToFile(string fileName, string fileContent)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            byte[] file;
            file = System.Text.Encoding.Default.GetBytes(fileContent);
            fs.Write(file, 0, file.Length);
            fs.Close();
        }

        /// <summary>
        /// 创建安装服务批处理
        /// </summary>
        private string BuildUninstallServcieBatch(string ServiceName)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(1024);
            sb.AppendFormat("@echo off\r\n");
            sb.AppendFormat("net stop {0} \r\n", ServiceName);
            sb.AppendFormat("sc delete \"{0}\" \r\n", ServiceName);
            //sb.AppendFormat("pause \r\n");
            return sb.ToString();
        }
    }
}
