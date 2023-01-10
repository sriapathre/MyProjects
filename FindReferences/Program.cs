using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace FindReferences
{
    class Program
    {

        static void Main(string[] args)
        {
            string rootdir = @"E:\move\src\Services\rwf";

            string[] files = Directory.GetFileSystemEntries(rootdir, "*.csproj", SearchOption.AllDirectories);
            XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
            foreach (var file in files)
            {
                XDocument projDefinition = XDocument.Load(file);

                IEnumerable<string> references = projDefinition.Descendants(msbuild + "ItemGroup")
                    .Descendants(msbuild + "Reference")
                    .Attributes("Include")
                    .Select(refr => refr.Value)
                    .Union(
                        projDefinition.Descendants("ItemGroup")
                        .Descendants("Reference")
                        .Attributes("Include")
                        .Select(refr => refr.Value)
                    );

                IEnumerable<string> pkgReferences = projDefinition.Descendants(msbuild + "ItemGroup")
                    .Descendants(msbuild + "PackageReference")
                    .Select(refr => $"{refr.Attribute("Include")?.Value?? "Unknown"} VersionOverride = {refr.Attribute("VersionOverride")?.Value?? "Unknown"}")
                    .Union(
                        projDefinition.Descendants("ItemGroup")
                        .Descendants("PackageReference")
                        .Select(refr => $"{refr.Attribute("Include")?.Value??"Unknown"} VersionOverride = {refr.Attribute("VersionOverride")?.Value??"Unknown"}")
                    );

                if (references.Any() && pkgReferences.Any())
                    references = references.Union(pkgReferences);
                else if (pkgReferences.Any())
                    references = pkgReferences;

                var matchedRefs = references?
                    .Where(refer => refer.Contains("System.Diagnostics.DiagnosticSource"));

                if (matchedRefs != null && matchedRefs.Any())
                {
                    Console.WriteLine($"\n{file}");
                    matchedRefs.ToList().ForEach(refr => Console.WriteLine($"\t{refr}"));
                }
            }
            Console.ReadLine();
        }
    }
}
