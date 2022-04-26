using System;
using System.IO;
using DefaultNamespace;
using SimpleJSON;

namespace MineMiner
{
    public static class SaveLoadController
    {
        public static T LoadObject<T>(string path, Func<JSONNode, T> constructor)
        {
            if (IsSaveFileExist(path))
            {
                JSONNode playerNode = GetJSONByPath(path);
                return constructor(playerNode);
            }
            throw new Exception($"File {path} not exist!");
        }
        
        private static JSONNode GetJSONByPath(string path)
        {
            return JSONNode.Parse(File.ReadAllText(path));
        }
        
        public static void SaveObject(string path, IJSONObject jsonObject)
        {
            JSONNode objectJson = jsonObject.ToJson();
            File.WriteAllText(path, objectJson.ToString());
        }

        public static bool IsSaveFileExist(string path)
        {
            return File.Exists(path);
        }
    }
}