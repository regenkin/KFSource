using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Security.AccessControl;
using System.IO;


namespace KfServerInstall
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
            string strCMDExe = path + @"\Kf.InstallIIS.exe ";
            string strAgr = WebDir;
            strAgr = "\"" + strAgr + "\"" + " TMWeb 1";

            AddSecurityControll2Folder(WebDir);//给目录设置权限

            System.Diagnostics.Process.Start(strCMDExe, strAgr);
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
    }
}
