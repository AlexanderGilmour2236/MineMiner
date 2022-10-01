using System;
using System.IO;
using DefaultNamespace;
using SimpleJSON;
using UnityEngine;

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
        
        public static T LoadObjectFromResources<T>(string path, Func<JSONNode, T> constructor)
        {
            string dataPath = path;
         
            Debug.Log(dataPath);
         
            string loadedText = ((TextAsset)Resources.Load(dataPath)).text;
            Debug.Log(loadedText);
            if (loadedText != null)
            {
                JSONNode playerNode = JSONNode.Parse(loadedText);
                return constructor(playerNode);
            }
            throw new Exception($"File {path} not exist!");
        }
        
        public static T LoadObjectFromString<T>(string text, Func<JSONNode, T> constructor)
        {
            JSONNode playerNode = JSONNode.Parse(text);
            return constructor(playerNode);
        }
        
        private static JSONNode GetJSONByPath(string path)
        {
            return JSONNode.Parse(File.ReadAllText(path));
        }        
        
        private static JSONNode GetJSONFromResources(string path)
        {
            return JSONNode.Parse(Resources.Load(path).ToString());
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