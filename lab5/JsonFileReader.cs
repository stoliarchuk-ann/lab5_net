using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace lab5
{
    public class JsonFileReader : IFileLoader
    {
        public async Task<List<T>?> LoadAsync<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<T>>(json);
        }
    }
}
