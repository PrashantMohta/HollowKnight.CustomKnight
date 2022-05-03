using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public void TraverseGameObjectPath(string path,string rootPath,string name){
            Modding.Logger.LogDebug($"{path}:{rootPath}:{name}");
            var pathSplit = path.Split(new Char[] {'/'},3);
            GameObjectProxy GOP = null;
            hasChildren = false;
            if(pathSplit.Length > 1){
                hasChildren = true;
                if(!children.TryGetValue(pathSplit[1],out GOP)){
                    GOP = new GameObjectProxy(){
                        name = pathSplit[1],
                        hasTexture = false,
                    };
                }
                children[pathSplit[1]] = GOP;
            }
            if(GOP != null){
                if(pathSplit.Length > 2){
                    GOP.TraverseGameObjectPath($"{pathSplit[1]}/{pathSplit[2]}",rootPath,name);
                } else {
                    if(!GOP.hasTexture){ // do not over ride existing texture
                        GOP.hasTexture = true;
                        GOP.rootPath = rootPath;
                        GOP.name = name;
                    }
                }
            } else {
                if(!this.hasTexture){
                    this.hasTexture = true;
                    this.rootPath = rootPath;
                    this.name = name;
                }
            }

            Modding.Logger.LogDebug($"{this.hasTexture}:{this.rootPath}:{this.name}");
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
                //Log(filename);
                if(filename.EndsWith(".png")){
                    string objectName = filename.Replace(".png","");
                    GameObjectProxy GOP = new GameObjectProxy(){
                        name = objectName,
                        hasTexture = true,
                        rootPath = Path.Combine(rootPath,name),
                        hasChildren = false
                    };
                    hasChildren = true;
                    children[objectName] = GOP;
                }
            }
            // check if it has directories
            foreach(string directory in Directory.GetDirectories(path)){
                string directoryName = new DirectoryInfo(directory).Name;
                //Log(directoryName);
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
                children[directoryName] = GOP;
                GOP.TraverseGameObjectDirectory(basePath);
            }

        }
    }
}