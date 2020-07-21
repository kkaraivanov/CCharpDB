namespace ProductShop.Extensions
{
    using System;
    using System.IO;
    using System.Reflection;
    using Newtonsoft.Json;

    public static class DirectoryFileExtension
    {
        public static string Read(this string file)
        {
            string dbSetsPath = "Datasets";
            var readPath = $"{AssemblyDirectory}/{dbSetsPath}/{file}";

            return File.ReadAllText(readPath);
        }

        public static string Write(this string file, string inputJson)
        {
            string dbSetsPath = "ExportDatasets";
            var writePath = $"{AssemblyDirectory}/{dbSetsPath}";
            if (!Directory.Exists(writePath))
                Directory.CreateDirectory(writePath);
            File.WriteAllText($"{writePath}/{file}", inputJson);
            
            return File.ReadAllText($"{writePath}/{file}");
        }

        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}