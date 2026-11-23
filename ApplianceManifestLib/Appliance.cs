using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ApplianceManifestLib
{
    public class Appliance
    {
        public string name { get; set; }
        public List<Package> packages { get; set; }
        public string startupApp { get; set; }
        public ApplicationType type { get; set; }
        public string starting_command { get; set; }
        public Appliance() { }
        public static Appliance Parse(string content)
        {
            return JsonSerializer.Deserialize<Appliance>(content);
        }

        public NetworkSettings? networkSettings { get; set; }
    }
}
