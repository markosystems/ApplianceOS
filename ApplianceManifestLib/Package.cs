using System;
using System.Collections.Generic;
using System.Text;

namespace ApplianceManifestLib
{
    public struct Package
    {
        public string name { get; set; }
        public string? version { get; set; }
        public string? path { get; set; }
        public PackageType type { get; set; }
    }
}
