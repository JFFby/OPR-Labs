using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace OPR.KP.Logger
{
    public sealed class Logger
    {
        private readonly string LogFolder;

        public Logger(string folder)
        {
            LogFolder = folder;
        }

        public void Log(object key, LogValue value)
        {
            CheckLogFolder();
            var serializedObject = GetValueToSerialize(key, value);
            var content = JsonConvert.SerializeObject(serializedObject);
            File.WriteAllText(Path(key), content);
        }

        public Dictionary<string, List<LogValue>> ReadResults()
        {
            var files = Directory.GetFiles(LogFolder);
            var result = new Dictionary<string, List<LogValue>>();
            foreach (var file in files)
            { 
                //Dictionary<string, List<LogValue>>
                var config = JsonConvert
                    .DeserializeObject<KeyValuePair<string, List<LogValue>>>(File.ReadAllText(file));
                result.Add(config.Key, config.Value);
            }

            return result;
        } 

        private Dictionary<string, List<LogValue>> GetValueToSerialize(object key, LogValue value)
        {
            var dictionary = LoadSerializedDictionary(key);
            if (!dictionary.ContainsKey(key.ToString()))
            {
                dictionary.Add(key.ToString(), new List<LogValue>());
            }

            dictionary[key.ToString()].Add(value);
            return dictionary;
        }

        Dictionary<string, List<LogValue>> LoadSerializedDictionary(object key)
        {
            Dictionary<string, List<LogValue>> dictionary = null;
            if (File.Exists(Path(key)))
            {
                var content = File.ReadAllText(Path(key));
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, List<LogValue>>>(content);
            }

            dictionary = dictionary ?? new Dictionary<string, List<LogValue>>();

            return dictionary;
        }

        private string Path(object key)
        {
            return string.Format("{0}\\{1}.json", LogFolder, key.ToString());
        }

        private void CheckLogFolder()
        {
            if (!Directory.Exists(LogFolder))
            {
                Directory.CreateDirectory(LogFolder);
            }
        }
    }
}
