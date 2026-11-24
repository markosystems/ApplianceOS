using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace appliancecli
{
    public abstract class ServiceManager
    {
        public abstract void StartService(string serviceName);
        public abstract void StopService(string serviceName);
        public abstract void RestartService(string serviceName);
        public abstract void EnableService(string serviceName);
        public abstract void DisableService(string serviceName);
        public abstract void CreateService(string serviceName, string StartupCommand);
        
    }

    public class systemd : ServiceManager
    {

        public Process Process = new Process();
        public systemd()
        {
            Process.StartInfo.FileName = "systemctl";
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.RedirectStandardError = true;
            Process.StartInfo.UseShellExecute = false;
        }
        public override void StartService(string serviceName)
        {
            Process.StartInfo.Arguments = $"start {serviceName}";
        }
        public override void StopService(string serviceName)
        {
            Process.StartInfo.Arguments = $"stop {serviceName}";
        }
        public override void RestartService(string serviceName)
        {
            Process.StartInfo.Arguments = $"restart {serviceName}";
        }
        public override void EnableService(string serviceName)
        {
            Process.StartInfo.Arguments = $"enable {serviceName}";
        }
        public override void DisableService(string serviceName)
        {
            Process.StartInfo.Arguments = $"disable {serviceName}";
        }
        public override void CreateService(string serviceName, string StartupCommand)
        {
            CreateServiceFile(serviceName, StartupCommand);
        }
        void CreateServiceFile(string serviceName, string StartupCommand)
        {
            string serviceFileContent = $"[Unit]\nDescription={serviceName}\n\n[Service]\nExecStart={StartupCommand}\n\n[Install]\nWantedBy=multi-user.target";
            string serviceFilePath = $"/etc/systemd/system/{serviceName}.service";
            System.IO.File.WriteAllText(serviceFilePath, serviceFileContent);
        }
    }
}
