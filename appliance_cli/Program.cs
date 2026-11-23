using System.Security.Cryptography.X509Certificates;
using ApplianceManifestLib;
using System.Diagnostics;

namespace appliance_cli
{
    internal class Program
    {
        public static PackageHandler packageHandler;
        public static ServiceManager serviceManager;
        public static Appliance appliance;
        static void Main(string[] args)
        {
            try
            {
            packageHandler = GetPackageHandlerFromOS();

            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            try
            {
                serviceManager = GetServiceManagerFromOS();
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            if(File.Exists("manifest.json"))
            {
                string manifestContent = File.ReadAllText("manifest.json");
                appliance = Appliance.Parse(manifestContent);
            }
            else
            {
                Console.WriteLine("Manifest file not found");
                throw new FileNotFoundException("Manifest file not found");
            }
            if(appliance.networkSettings != null)
            {
                Console.WriteLine($"Connecting to WiFi network {appliance.networkSettings?.SSID}");
                // Connect to WiFi network using nmcli
                Process process = new Process();
                process.StartInfo.FileName = "nmcli";
                process.StartInfo.Arguments = $"dev wifi connect \"{appliance.networkSettings?.SSID}\" password \"{appliance.networkSettings?.Password}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    Console.WriteLine($"Failed to connect to WiFi network {appliance.networkSettings?.SSID}");
                    Console.WriteLine($"Error: {error}");
                    throw new Exception($"Failed to connect to WiFi network {appliance.networkSettings?.SSID}");
                }
            }
            InstallPackages();
            InstallStartupApp();
            serviceManager.CreateService(appliance.name, appliance.starting_command);
        }
        static void InstallStartupApp()
        {
            if (appliance.type == ApplicationType.folder)
            {
                if (Directory.Exists(appliance.startupApp))
                {
                    //move the folder to bin directory
                    string binPath = "/usr/local/bin/" + appliance.startupApp;
                    Directory.Move(appliance.startupApp, binPath);
                    //make the folder executable
                    Process process = new Process();
                    process.StartInfo.FileName = "chmod";
                    process.StartInfo.Arguments = $"-R +x {binPath}";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.UseShellExecute = false;
                    process.Start();
                }
                else
                {
                    Console.WriteLine($"Startup app {appliance.startupApp} folder not found");
                    throw new DirectoryNotFoundException($"Startup app {appliance.startupApp} folder not found");
                }
            }
            else if (appliance.type == ApplicationType.file)
            {
                if (packageHandler.PackageExtent.Contains(Path.GetExtension(appliance.startupApp)))
                {
                    packageHandler.InstallPackage(appliance.startupApp);
                } 
                else if (File.Exists(appliance.startupApp))
                {
                    //move the file to bin directory
                    string binPath = "/usr/local/bin/" + appliance.startupApp;
                    File.Move(appliance.startupApp, binPath);
                    //make the file executable
                    Process process = new Process();
                    process.StartInfo.FileName = "chmod";
                    process.StartInfo.Arguments = $"+x {binPath}";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.UseShellExecute = false;
                    process.Start();
                }
                else
                {
                    Console.WriteLine($"Startup app {appliance.startupApp} file not found");
                    throw new FileNotFoundException($"Startup app {appliance.startupApp} file not found");
                }
            }
            else if (appliance.type == ApplicationType.archive)
            {
                if (File.Exists(appliance.startupApp))
                {
                    Console.WriteLine($"Startup app {appliance.startupApp} is an archive, extracting...");
                    if (packageHandler.PackageExtent.Contains(Path.GetExtension(appliance.startupApp)))
                    {
                        packageHandler.InstallPackage(appliance.startupApp);
                    }
                    else
                    {
                        // 
                        System.IO.Compression.ZipFile.ExtractToDirectory(appliance.startupApp, "/usr/local/bin");
                    }
                }
                else
                {
                    Console.WriteLine($"Startup app {appliance.startupApp} archive not found");
                    throw new FileNotFoundException($"Startup app {appliance.startupApp} archive not found");
                }
            }
            else if (appliance.type == ApplicationType.Package)
            {
                if (File.Exists(appliance.startupApp))
                {
                    Console.WriteLine($"Startup app {appliance.startupApp} is a package, installing...");
                    // Install the package
                    packageHandler.InstallPackage(appliance.startupApp);
                }
                else
                {
                    Console.WriteLine($"Startup app {appliance.startupApp} package not found");
                    throw new FileNotFoundException($"Startup app {appliance.startupApp} package not found");
                }
            }
        }
        static void InstallPackages()
        {
            foreach (var package in appliance.packages)
            {
                if (package.type == PackageType.PackageHandler)
                {
                    packageHandler.InstallPackage(package.name);
                }
                else if (package.type == PackageType.Local)
                {
                    if(package.path != null)
                    {
                        if (File.Exists(package.path))
                        {
                            packageHandler.InstallPackage(package.path);
                        }
                        else
                        {
                            Console.WriteLine($"Package file {package.path} not found");
                            throw new FileNotFoundException($"Package file {package.path} not found");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Package path for {package.name} is null");
                        throw new ArgumentNullException($"Package path for {package.name} is null");
                    }
                }
            }
        }
        public static PackageHandler GetPackageHandlerFromOS()
        {
            if (OperatingSystem.IsLinux())
            {
                if (File.Exists("/usr/bin/apt"))
                {
                    return new apt();
                }
                else if (File.Exists("/usr/bin/snap"))
                {
                    return new snap();
                }
                else if(File.Exists("/usr/bin/flatpak"))
                {
                    return new flatpak();
                }
            }
            throw new NotImplementedException("Package handler not supported yet");

        }
        public static ServiceManager GetServiceManagerFromOS()
        {
            if (OperatingSystem.IsLinux())
            {
                if (File.Exists("/usr/bin/systemctl"))
                {
                    return new systemd();
                }
            }
            throw new NotImplementedException("Service manager not supported yet");
        }
    }
}
