using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Modding.Logger;
namespace CustomKnight {
    public class GameObjectProxy
    {
        public string name;
        public string rootPath; //rootPath/name.png
        public bool hasTexture;
        public bool hasChildren;
        public Dictionary<string,GameObjectProxy> children = new Dictionary<string,GameObjectProxy>();

        public string getTexturePath(){
            return Path.Combine(rootPath,name+".png");
        }
        public void TraverseGameObjectDirectory(string basePath){
            var path = Path.Combine(basePath,Path.Combine(rootPath,name));
            if(!Directory.Exists(path)){
                hasChildren = false;
                return;
            }
            // check if it has files
            foreach(string file in Directory.GetFiles(path)){
                string filename = Path.GetFileName(file);
                Log(filename);
                if(filename.EndsWith(".png")){
                    string objectName = filename.Replace(".png","");
                    GameObjectProxy GOP = new GameObjectProxy(){
                        name = objectName,
                        hasTexture = true,
                        rootPath = Path.Combine(rootPath,name),
                        hasChildren = false
                    };
                    hasChildren = true;
                    children.Add(objectName,GOP);
                }
            }
            // check if it has directories
            foreach(string directory in Directory.GetDirectories(path)){
                string directoryName = new DirectoryInfo(directory).Name;
                Log(directoryName);
                GameObjectProxy GOP;
                if(!children.TryGetValue(directoryName,out GOP)){
                    GOP = new GameObjectProxy(){
                        name = directoryName,
                        hasTexture = false,
                        rootPath = Path.Combine(rootPath,name),
                        hasChildren = true
                    };
                }
                hasChildren = true;
                GOP.TraverseGameObjectDirectory(basePath);
            }

        }
    }
}