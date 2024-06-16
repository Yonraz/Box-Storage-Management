using BoxStorage.Models.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxStorage.Services
{
    public class JsonHandler
    {
        private static readonly string _documentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static readonly string _fileFolderPath = Path.Combine(_documentsFolderPath, ConstantStrings.FolderName);
        private static readonly string _jsonFilePath = Path.Combine(_fileFolderPath, ConstantStrings.FileName);
        public static string JsonFilePath { get { return _jsonFilePath; } }

        public static void Save(IEnumerable<BoxQueue> collection = null)
        {
            CheckDirectory();
            if (collection == null) collection = Storage.GetAllBoxes();
            else collection = collection.Except(Storage.GetAllBoxes());
            string json = JsonConvert.SerializeObject(collection, Formatting.Indented);
            File.WriteAllText(Path.Combine(_fileFolderPath, ConstantStrings.FileName), json);
        }

        private static void CheckDirectory()
        {
            if (!Directory.Exists(_fileFolderPath))
            {
                Directory.CreateDirectory(_fileFolderPath);
            }
        }

        public static IEnumerable<List<Box>> Load()
        {
            if (!File.Exists(_jsonFilePath)) return null;
            string json = File.ReadAllText(Path.Combine(_fileFolderPath, ConstantStrings.FileName));
            List<List<Box>> items = JsonConvert.DeserializeObject<List<List<Box>>>(json);
            return items;
        }
    }
}
