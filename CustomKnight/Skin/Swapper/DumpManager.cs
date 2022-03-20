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
using Satchel;
using static Satchel.SpriteUtils;
using static Satchel.GameObjectUtils;
using static Satchel.IoUtils;

namespace CustomKnight {

    public class coroutineHelper : MonoBehaviour{}
    public class DumpManager{

        internal bool enabled = false;
        internal DumpManager(){
            if(CustomKnight.isSatchelInstalled()){
                ModHooks.LanguageGetHook += SaveTextDump;
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += dumpAllSprites;
                On.HutongGames.PlayMaker.Actions.ActivateGameObject.DoActivateGameObject += dumpAllSprites;
            }
        }

        internal Dictionary<string,bool> isTextureDumped = new Dictionary<string,bool>();
        internal Dictionary<int,string> MaterialProcessed = new();

        internal void dumpSpriteForGo(Scene scene,GameObject go){  
            if(go == null) {return;}          
            var name = go.GetPath(true);
            Log("game object to be dumped -" + go.name);
            Log($"gameobject path {name}");
            Animator anim = go.GetComponent<Animator>();
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            tk2dSprite tk2ds = go.GetComponent<tk2dSprite>();

            var mat = sr != null ? sr.material :  (tk2ds != null ? tk2ds.GetCurrentSpriteDef()?.material : null);
            if(mat != null && MaterialProcessed.TryGetValue(mat.ComputeCRC(),out var _hash)){
                return;
            }
            if(anim != null && sr != null && false){ //since custom animation frames dont work anyway lets disable them for now
                var caf = go.GetAddComponent<CustomAnimationFrames>();
                caf.dumpPath = Path.Combine(SkinManager.DATA_DIR,"Dump");
                caf.dump = true;
                return;
            }
            if(sr != null && sr.sprite != null){
                if(scene.name == "DontDestroyOnLoad"){
                    return;
                    // do not attempt to dump DontDestroyOnLoad sprites
                    var tex = SpriteUtils.ExtractTextureFromSprite(sr.sprite);
                    var hash = tex.getHash();
                    SaveTextureByPath("Global",hash,tex);
                    GameObject.Destroy(tex);
                } else {
                    SaveSpriteDump(scene,name, sr.sprite);
                }
                return;
            } 
            if(tk2ds != null){
                //dump as texture hash 
                var sdef = tk2ds.GetCurrentSpriteDef();
                var tex = (Texture2D) sdef.material.mainTexture;
                var dupe = TextureUtils.duplicateTexture(tex);
                var hash = dupe.getHash();
                SaveTextureByPath("Global",hash,dupe);
                if(scene.name != "DontDestroyOnLoad"){
                    SaveTextureDump(scene,name, dupe);
                }
                GameObject.Destroy(dupe);
                return;
            }
        }

        internal Coroutine dumpAllSpritesCoroutineRef;
        internal bool pending = false;
        internal IEnumerator dumpAllSpritesCoroutine(){
           do{
            yield return null;
            var scenes = SceneUtils.GetAllLoadedScenes(true);
            foreach(var scene in scenes){ 
                    if(scene == null || !scene.IsValid()){continue;}
                    var GOList = scene.GetAllGameObjects();
                    foreach(var go in GOList){
                        try{
                            dumpSpriteForGo(scene,go);
                        } catch(Exception e){
                            Log(e.ToString());
                        }
                        yield return null;
                    }
            }
            pending = false;
           } while(pending); // handle the case where a new go is spawned while the coro is still dumping
           dumpAllSpritesCoroutineRef = null;
        }
        internal IEnumerator walkScenes(){
            yield return null;
            var sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
            var i = 0;
            while(true){
                if(dumpAllSpritesCoroutineRef == null || !pending || i == 3){
                    //load next scene    
                    if( i < sceneCount){
                        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(i);
                        // Wait until the asynchronous scene fully loads
                        while (!asyncLoad.isDone)
                        {
                            yield return null;
                        }
                        i++;
                    }
                }
                yield return new WaitForSeconds(1);
            }
        } 
        internal void walk(){
            var g = new GameObject();
            GameObject.DontDestroyOnLoad(g);
            g.AddComponent<coroutineHelper>().StartCoroutine(walkScenes());
        }
        private GameObject coroHelperObj;
        internal void dumpAllSprites(){
            if(!enabled) {return;} 
            pending = true;
            if(coroHelperObj == null){
                coroHelperObj = new GameObject();
                GameObject.DontDestroyOnLoad(coroHelperObj);
            }
            if(dumpAllSpritesCoroutineRef == null){
                dumpAllSpritesCoroutineRef = coroHelperObj.GetAddComponent<coroutineHelper>().StartCoroutine(dumpAllSpritesCoroutine());
            }
        }

        internal void dumpAllSprites(Scene scene,LoadSceneMode mode){
            dumpAllSprites();
        }
        
        internal void dumpAllSprites(On.HutongGames.PlayMaker.Actions.ActivateGameObject.orig_DoActivateGameObject orig, HutongGames.PlayMaker.Actions.ActivateGameObject self){
            orig(self);
            if(self.gameObject.GameObject.Value != null){
                dumpAllSprites();
            }
        }
        internal void SaveSpriteDump(Scene scene,string objectName, Sprite sprite){
            if(!enabled) {return;} 
            string DUMP_DIR = Path.Combine(SkinManager.DATA_DIR,"Dump");
            string scenePath = Path.Combine(DUMP_DIR,scene.name);
            EnsureDirectory(DUMP_DIR);
            EnsureDirectory(scenePath);
            
            string outpath = Path.Combine(scenePath,objectName.Replace('/',Path.DirectorySeparatorChar)+".png");
            try{
                EnsureDirectory(Path.GetDirectoryName(outpath));
            } catch (IOException e){
                Log(e.ToString());
            }
            if(!isTextureDumped.TryGetValue(outpath,out bool path) && !File.Exists(outpath)){
                Texture2D dupe = (Texture2D) SpriteUtils.ExtractTextureFromSprite(sprite);
                byte[] texBytes = dupe.EncodeToPNG();
                try{
                    File.WriteAllBytes(outpath,texBytes);
                } catch (IOException e){
                    Log(e.ToString());
                }
                isTextureDumped[outpath] = true;
            }            
        }

        internal void SaveTextureByPath(string sceneName,string objectName, Texture2D texture){
            string DUMP_DIR = Path.Combine(SkinManager.DATA_DIR,"Dump");
            string scenePath = Path.Combine(DUMP_DIR,sceneName);

            EnsureDirectory(DUMP_DIR);
            EnsureDirectory(scenePath);
            
            string outpath = Path.Combine(scenePath,objectName.Replace('/',Path.DirectorySeparatorChar)+".png");
            try{
                EnsureDirectory(Path.GetDirectoryName(outpath));
            } catch (IOException e){
                Log(e.ToString());
            }
            if(!isTextureDumped.TryGetValue(outpath,out bool path) && !File.Exists(outpath)){
                Texture2D dupe = texture.isReadable ? texture : TextureUtils.duplicateTexture(texture);
                byte[] texBytes = dupe.EncodeToPNG();
                try{
                    File.WriteAllBytes(outpath,texBytes);
                } catch (IOException e){
                    Log(e.ToString());
                }
                isTextureDumped[outpath] = true;
            }            
        }
        internal void SaveTextureDump(Scene scene,string objectName, Texture2D texture){
            if(!enabled) {return;} 
            SaveTextureByPath(scene.name,objectName,texture);
        }
        internal void SaveTextDump( string key, string value){
            if(!enabled) {return;} 
            string DUMP_DIR = Path.Combine(SkinManager.DATA_DIR,"Dump");
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            string scenePath = Path.Combine(DUMP_DIR,scene.name);
            EnsureDirectory(DUMP_DIR);
            EnsureDirectory(scenePath);
            var outpath = (Path.Combine(scenePath,key+".txt"));

            try{
                EnsureDirectory(Path.GetDirectoryName(outpath));
            } catch (IOException e){
                Log(e.ToString());
            }
            if(!File.Exists(outpath)){
                File.WriteAllText(outpath,value);
            }
        }
        internal string SaveTextDump( string key, string sheet , string value){
            SaveTextDump(sheet+key, value);
            return value;
        }

        internal void Unload(){
            ModHooks.LanguageGetHook -= SaveTextDump;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= dumpAllSprites;
            On.HutongGames.PlayMaker.Actions.ActivateGameObject.DoActivateGameObject -= dumpAllSprites;
        }    
        internal void Log(string str) {
            CustomKnight.Instance.Log("[DumpManager] " +str);
        }
        internal static void debugDumpSprite(Sprite sprite){
            Texture2D dupe = (Texture2D) Satchel.SpriteUtils.ExtractTextureFromSprite(sprite);
            Satchel.TextureUtils.WriteTextureToFile(dupe,$"{Satchel.AssemblyUtils.getCurrentDirectory()}/{sprite.name}.png");
        }
        internal static void debugDumpTex(Texture2D tex,string name){
            Satchel.TextureUtils.WriteTextureToFile(tex,$"{Satchel.AssemblyUtils.getCurrentDirectory()}/{name}.png");
        }
    }
}