using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace appliancecli
{
    public abstract class PackageHandler
    {
        public abstract string[] PackageExtent { get; }
        public abstract void InstallPackage(string packageName);
        public abstract void InstallPackage(string packageName, string[] args);
        public abstract void RemovePackage(string packageName);
        public abstract void UpdatePackage(string packageName);
        public abstract void UpdateAllPackages();
    }

    public class apt : PackageHandler
    {
        Process Process = new Process();
        public apt()
        {
            Process.StartInfo.FileName = "apt";
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.RedirectStandardError = true;
            Process.StartInfo.UseShellExecute = false;
        }

        public override string[] PackageExtent => [".deb", ".deb-src", ".tar", ".tar.gz", ".tar.bz2", ".tar.xz", ".zip", ".7z"];

        public override void InstallPackage(string packageName)
        {
            Process.StartInfo.Arguments = $"install {packageName} y";
            Process.Start();
        }

        public override void InstallPackage(string packageName, string[] args)
        {
            Process.StartInfo.Arguments = $"install {packageName} {string.Join(" ", args)}";
            Process.Start();
        }

        public override void RemovePackage(string packageName)
        {
            Process.StartInfo.Arguments = $"remove {packageName}";
            Process.Start();
        }

        public override void UpdateAllPackages()
        {
            Process.StartInfo.Arguments = "update";
            Process.Start();
        }

        public override void UpdatePackage(string packageName)
        {
            Process.StartInfo.Arguments = $"upgrade {packageName}";
            Process.Start();
        }
    }

    public class snap : PackageHandler
    {
        Process Process = new Process();
        public snap()
        {
            Process.StartInfo.FileName = "snap";
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.RedirectStandardError = true;
            Process.StartInfo.UseShellExecute = false;
        }

        public override string[] PackageExtent => [".snap"];

        public override void InstallPackage(string packageName)
        {
            Process.StartInfo.Arguments = $"install {packageName}";
            Process.Start();
        }
        public override void InstallPackage(string packageName, string[] args)
        {
            Process.StartInfo.Arguments = $"install {packageName} {string.Join(" ", args)}";
            Process.Start();
        }
        public override void RemovePackage(string packageName)
        {
            Process.StartInfo.Arguments = $"remove {packageName}";
            Process.Start();
        }
        public override void UpdateAllPackages()
        {
            Process.StartInfo.Arguments = "refresh";
            Process.Start();
        }
        public override void UpdatePackage(string packageName)
        {
            Process.StartInfo.Arguments = $"refresh {packageName}";
            Process.Start();
        }
    }

    public class flatpak : PackageHandler
    {
        Process Process = new Process();
        public flatpak()
        {
            Process.StartInfo.FileName = "flatpak";
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.RedirectStandardError = true;
            Process.StartInfo.UseShellExecute = false;
        }

        public override string[] PackageExtent => [".flatpak"];

        public override void InstallPackage(string packageName)
        {
            Process.StartInfo.Arguments = $"install {packageName}";
            Process.Start();
        }
        public override void InstallPackage(string packageName, string[] args)
        {
            Process.StartInfo.Arguments = $"install {packageName} {string.Join(" ", args)}";
            Process.Start();
        }
        public override void RemovePackage(string packageName)
        {
            Process.StartInfo.Arguments = $"uninstall {packageName}";
            Process.Start();
        }
        public override void UpdateAllPackages()
        {
            Process.StartInfo.Arguments = "update";
            Process.Start();
        }
        public override void UpdatePackage(string packageName)
        {
            Process.StartInfo.Arguments = $"update {packageName}";
            Process.Start();
        }
    }
}
