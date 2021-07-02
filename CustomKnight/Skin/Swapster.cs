using System;
using System.IO;
using System.Linq;

using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;           

using GlobalEnums;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomKnight {
    public class Swapster
    {
        private string DATA_DIR;
        public string SWAPSTER_FOLDER = "Swapster";
        private string SEPARATOR = "=>";
        public float BACKOFF_MULTIPLIER = 1.3f;
        public int INITAL_NEXT_CHECK = 1000;

        public int nextCheck;

        public Dictionary<string,List<string>> Scenes = new Dictionary<string,List<string>>();
        public Scene currentScene;
        public List<string> currentSkinnedSceneObjs = new List<string>();
        public Dictionary<string,Texture2D> loadedTextures = new Dictionary<string, Texture2D>();
        public Dictionary<string,string> Strings  = new Dictionary<string,string>();         
        public Dictionary<string,string> ReplaceStrings  = new Dictionary<string,string>();         
        public DateTime lastTime = DateTime.Now;

        public bool firstInit = true;

        public bool SwapSkinRoutineRunning = false;
        private void loadTexture(Scene scene,string objectName){
                if(loadedTextures.TryGetValue(objectName, out var tex)){
                    return;
                }
                byte[] buffer = File.ReadAllBytes(Path.Combine(DATA_DIR,Path.Combine(scene.name,objectName+".png")).Replace("\\", "/"));
                var texture = new Texture2D(2, 2);
                texture.LoadImage(buffer.ToArray(),true);
                loadedTextures.Add(objectName,texture);
            
        }
        private void SwapSkin(Scene scene){
            if (Scenes.TryGetValue(scene.name, out var CurrentSceneList))
            {
                foreach(string objectName in CurrentSceneList){
                    if(currentSkinnedSceneObjs.Contains(objectName)){
                        continue;
                    }
                    try{
                        loadTexture(scene,objectName);
                    } catch( Exception e){
                        this.Log( objectName + " " + e.ToString());
                        continue;
                    }
                    var GO = GameObject.Find(objectName);
                    if(GO != null){
                        currentSkinnedSceneObjs.Add(objectName);
                        var _tk2dSprite = GO.GetComponent<tk2dSprite>();
                        if(_tk2dSprite == null){
                            this.Log("No tk2dSprite Component found in " + objectName + " in scene " + scene.name);
                        } else {
                            _tk2dSprite.GetCurrentSpriteDef().material.mainTexture = loadedTextures[objectName];
                        }
                    } else {
                        this.Log("GameObject " + objectName + " not found in scene " + scene.name + " Rechecking in : " + nextCheck + "ms");
                    }
                }
            } 
        }
        private IEnumerator SwapSkinRoutine(Scene scene){
            SwapSkinRoutineRunning = true;
            yield return null;
            SwapSkin(scene);
            SwapSkinRoutineRunning = false;
        }
        public void SwapSkinForScene(Scene scene,LoadSceneMode mode){
            currentSkinnedSceneObjs = new List<string>();
            currentScene = scene;
            nextCheck = INITAL_NEXT_CHECK;
            GameManager.instance.StartCoroutine(SwapSkinRoutine(scene));
        }

        public void checkForMissedObjects(){
            var currentTime = DateTime.Now;
            if(currentScene != null && nextCheck > 0 && (currentTime - lastTime).TotalMilliseconds > nextCheck){
                SwapSkin(currentScene);
                nextCheck = (int)Math.Round((float)nextCheck * BACKOFF_MULTIPLIER);
                lastTime = currentTime;
            }
        }

        public string LanguageGet( string key, string sheet , string orig ){
            string overrideText;
            if(Strings.TryGetValue(key, out overrideText)){
                return overrideText;
            }

            //string orig = Language.Language.GetInternal(key, sheet);
            string textValue = orig;

            foreach( KeyValuePair<string,string> kp in ReplaceStrings){
                textValue = Regex.Replace(textValue, Regex.Escape(kp.Key), kp.Value.Replace("$","$$"), RegexOptions.IgnoreCase);
            }
            //cache for next time
            Strings.Add(key,textValue);

            //Log(key + "|" + orig + "|" + textValue);
            return textValue;
        }
        public void LoadSwaps(){
            using(StreamReader reader = File.OpenText(Path.Combine(DATA_DIR,"replace.txt")))
            {
                while (!reader.EndOfStream)
                {
                   string line = reader.ReadLine();
                   int index = line.IndexOf(SEPARATOR);
                   if(index > 0 && index < line.Length - 1)
                   {
                       string replace = line.Substring(0, index).ToLower();
                       string with = line.Substring(index + SEPARATOR.Length);
                       
                       ReplaceStrings.Add(replace,with);
                   }
                }
            }
            
            foreach (string path in Directory.GetDirectories(DATA_DIR))
            {
                string directoryName = new DirectoryInfo(path).Name;
                List<string> objects = new List<string>();
                foreach(string file in Directory.GetFiles(path)){
                    string filename = Path.GetFileName(file);
                    if(filename.EndsWith(".png")){
                       objects.Add(filename.Replace(".png",""));
                    }
                    if(filename.EndsWith(".txt")){
                       try{
                           Strings.Add(filename.Replace(".txt",""),File.ReadAllText(file));
                       } catch( Exception e){
                           this.Log( filename + " " + e.ToString());
                           continue;
                       }
                    }
                }
                Scenes.Add(directoryName,objects);
            }
        }
        
        public void Swap(string skinpath)
        {
            DATA_DIR = Path.Combine(skinpath,SWAPSTER_FOLDER);

            if (!Directory.Exists(DATA_DIR))
            {
                Log("There is no Swapster folder in the Skin directory.");
                return;
            }

            if (Directory.GetDirectories(DATA_DIR).Length == 0)
            {
                Log("There are no Swapster skin folders in the Swapster directory.");
                return;
            }

            if (!File.Exists(Path.Combine(DATA_DIR,"replace.txt")))
            {
                File.Create(Path.Combine(DATA_DIR,"replace.txt"));
                Log("Created replace.txt in Swapster directory.");
            }

            LoadSwaps();
            if(firstInit){
                firstInit = false;
                ModHooks.LanguageGetHook += LanguageGet;
                ModHooks.HeroUpdateHook += checkForMissedObjects;
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += SwapSkinForScene;
                On.HutongGames.PlayMaker.Actions.ActivateGameObject.DoActivateGameObject += ActivateGameObject;
            }

        }
 
        public void ActivateGameObject(On.HutongGames.PlayMaker.Actions.ActivateGameObject.orig_DoActivateGameObject orig, HutongGames.PlayMaker.Actions.ActivateGameObject self){
            orig(self);
            if(self.activate.Value != true) {return;}

            if(!SwapSkinRoutineRunning){
                GameManager.instance.StartCoroutine(SwapSkinRoutine(currentScene));
            }
        }

        public void Log(string str) {
            CustomKnight.Instance.Log("[Swapster] " +str);
        }
    }
}