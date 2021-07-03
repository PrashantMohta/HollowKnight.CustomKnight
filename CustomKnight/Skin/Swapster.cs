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

        public Dictionary<string,List<string>> Scenes;
        public List<string> currentSkinnedSceneObjs;
        public Dictionary<string,Texture2D> loadedTextures;

        public Dictionary<string,Material> materials;
        public Dictionary<string,Texture2D> defaultTextures;

        public Dictionary<string,string> Strings;
        public Dictionary<string,string> ReplaceStrings;
        public DateTime lastTime = DateTime.Now;

        public bool firstInit = true;

        public static bool isEnabled = true;
        public static bool dumpingEnabled = false;
        public bool SwapSkinRoutineRunning = false;

        public static void setDumpEnabled(bool enabled){
            dumpingEnabled = enabled;
        }

        public static void setSwapsterEnabled(bool enabled){
            isEnabled = enabled;
            CustomKnight.GlobalSettings.swapsterEnabled = isEnabled;
        }
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
                            
                            materials[objectName] = _tk2dSprite.GetCurrentSpriteDef().material;
                            defaultTextures[objectName] = (Texture2D) materials[objectName].mainTexture;
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
            nextCheck = INITAL_NEXT_CHECK;
            GameManager.instance.StartCoroutine(SwapSkinRoutine(scene));
        }

        public void checkForMissedObjects(){
            var currentTime = DateTime.Now;
            if(nextCheck > 0 && (currentTime - lastTime).TotalMilliseconds > nextCheck){
                SwapSkin(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
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


            if(!firstInit){
                foreach(KeyValuePair<string,Material> kp in materials){
                    kp.Value.mainTexture =  defaultTextures[kp.Key];
                }
            }

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

            Scenes = new Dictionary<string,List<string>>();
            currentSkinnedSceneObjs = new List<string>();
            loadedTextures = new Dictionary<string, Texture2D>();

            materials = new Dictionary<string, Material>();
            defaultTextures = new Dictionary<string, Texture2D>();

            Strings  = new Dictionary<string,string>();         
            ReplaceStrings  = new Dictionary<string,string>();   

            LoadSwaps();
            if(firstInit){
                firstInit = false;
                ModHooks.LanguageGetHook += LanguageGet;
                ModHooks.HeroUpdateHook += checkForMissedObjects;
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += SwapSkinForScene;
                On.HutongGames.PlayMaker.Actions.ActivateGameObject.DoActivateGameObject += ActivateGameObject;

                if(Swapster.dumpingEnabled){
                    ModHooks.LanguageGetHook += SaveTextDump;
                    UnityEngine.SceneManagement.SceneManager.sceneLoaded += dumpAllSprites;
                    On.HutongGames.PlayMaker.Actions.ActivateGameObject.DoActivateGameObject += dumpAllSprites;
                }
            }
            GameManager.instance.StartCoroutine(SwapSkinRoutine(UnityEngine.SceneManagement.SceneManager.GetActiveScene()));

        }

        public void Unload(){
            ModHooks.LanguageGetHook -= LanguageGet;
            ModHooks.HeroUpdateHook -= checkForMissedObjects;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SwapSkinForScene;
            On.HutongGames.PlayMaker.Actions.ActivateGameObject.DoActivateGameObject -= ActivateGameObject;

            if(Swapster.dumpingEnabled){
                ModHooks.LanguageGetHook -= SaveTextDump;
                UnityEngine.SceneManagement.SceneManager.sceneLoaded -= dumpAllSprites;
                On.HutongGames.PlayMaker.Actions.ActivateGameObject.DoActivateGameObject -= dumpAllSprites;
            }

            if(materials != null){
                foreach(KeyValuePair<string,Material> kp in materials){
                    if(kp.Value == null) {
                        continue;
                    }
                    kp.Value.mainTexture =  defaultTextures[kp.Key];
                }      
            }
        }
 
        public void ActivateGameObject(On.HutongGames.PlayMaker.Actions.ActivateGameObject.orig_DoActivateGameObject orig, HutongGames.PlayMaker.Actions.ActivateGameObject self){
            orig(self);
            if(self.activate.Value != true) {return;}
            if(!SwapSkinRoutineRunning){
                GameManager.instance.StartCoroutine(SwapSkinRoutine(UnityEngine.SceneManagement.SceneManager.GetActiveScene()));
            }
        }

        public void Log(string str) {
            CustomKnight.Instance.Log("[Swapster] " +str);
        }


        public Dictionary<string,bool> isTextureDumped = new Dictionary<string,bool>();
        public void dumpAllSprites(){
           if(!Swapster.dumpingEnabled) {return;} 
           tk2dSprite[] tk2ds =  Utils.FindAllTk2dSprite();
           foreach(var sprite in tk2ds){
                SaveTextureDump(sprite.gameObject.name, (Texture2D) sprite.GetCurrentSpriteDef().material.mainTexture);
           }
        }

        public void dumpAllSprites(Scene scene,LoadSceneMode mode){
            dumpAllSprites();
        }
        
        public void dumpAllSprites(On.HutongGames.PlayMaker.Actions.ActivateGameObject.orig_DoActivateGameObject orig, HutongGames.PlayMaker.Actions.ActivateGameObject self){
            orig(self);
            dumpAllSprites();
        }
        public void SaveTextureDump(string objectName, Texture2D texture){
            if(!Swapster.dumpingEnabled) {return;} 
            string DUMP_DIR = Path.Combine(SkinManager.DATA_DIR,"Swapster_Dump");
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            string tk2d = Path.Combine(DUMP_DIR,"tk2d");
            string scenePath = Path.Combine(DUMP_DIR,scene.name);
            if (!Directory.Exists(DUMP_DIR))
            {
                Directory.CreateDirectory(DUMP_DIR);
            }
             if (!Directory.Exists(tk2d))
            {
                Directory.CreateDirectory(tk2d);
            }
            if(!Directory.Exists(scenePath)){
                Directory.CreateDirectory(scenePath);
            }
            
            string outpath = Path.Combine(tk2d,objectName+".png");
            if(!isTextureDumped.TryGetValue(outpath,out bool path) && !File.Exists(outpath)){
                Texture2D dupe = Utils.duplicateTexture(texture);
                byte[] texBytes = dupe.EncodeToPNG();
        
                File.WriteAllBytes(outpath,texBytes);
                isTextureDumped[outpath] = true;
            }
            SaveTextDump( objectName+".tk2d", "Texture:" +outpath);
            
        }
        public void SaveTextDump( string key, string value){
            if(!Swapster.dumpingEnabled) {return;} 
            string DUMP_DIR = Path.Combine(SkinManager.DATA_DIR,"Swapster_Dump");
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            string scenePath = Path.Combine(DUMP_DIR,scene.name);
            if (!Directory.Exists(DUMP_DIR))
            {
                Directory.CreateDirectory(DUMP_DIR);
            }
            if(!Directory.Exists(scenePath)){
                Directory.CreateDirectory(scenePath);
            }
            if(!File.Exists(Path.Combine(scenePath,key+".txt"))){
                File.WriteAllText(Path.Combine(scenePath,key+".txt"),value);
            }
        }
        public string SaveTextDump( string key, string sheet , string value){
            SaveTextDump( key, value);
            return value;
        }
    }
}