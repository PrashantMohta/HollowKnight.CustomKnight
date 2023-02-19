using CustomKnight.Canvas;
using System.IO;
using System.Linq;
using static Satchel.GameObjectUtils;
using static Satchel.IoUtils;

namespace CustomKnight
{

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
            Animator anim = go.GetComponent<Animator>();
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            tk2dSprite tk2ds = go.GetComponent<tk2dSprite>();

            var mat = sr != null ? sr.material :  (tk2ds != null ? tk2ds.GetCurrentSpriteDef()?.material : null);
            int crc = 0;
            var validForGlobal = true;
            if(mat != null){
                crc = mat.ComputeCRC();
                if(MaterialProcessed.TryGetValue(crc,out var _hash)){
                    validForGlobal = false;
                }
            }
            Log($"dumping {done}/{detected}");
            Log("game object to be dumped -" + go.name);
            Log($"gameobject path {name}");
            
            if(sr != null && sr.sprite != null){
                if(scene.name == "DontDestroyOnLoad"){
                    return; // dont dump sprites only tk2d from DontDestroyOnLoad
                    if(validForGlobal){
                        var tex = SpriteUtils.ExtractTextureFromSprite(sr.sprite);
                        var hash = tex.getHash();
                        MaterialProcessed[crc] = hash;
                        SaveTextureByPath("Global",hash,tex);
                        GameObject.Destroy(tex);
                    }
                } else {    
                    if(anim != null || SpecialCases.childSpriteAnimatedByParent(name)){ 
                        // remove the animation component
                        //GameObject.Destroy(anim);
                        //go.AddComponent<Animator>();
                        var tex = sr.sprite.texture;
                        SaveTextureDump(scene,name, tex);
                        return;
                    }
                    SaveSpriteDump(scene,name, sr.sprite);
                }
                return;
            } 
            if(tk2ds != null){
                //dump as texture hash 
                
                var sdef = tk2ds.GetCurrentSpriteDef();
                var tex = (Texture2D) sdef?.material?.mainTexture;
                if(tex == null){
                    return;
                }
                if(validForGlobal){
                    var dupe = TextureUtils.duplicateTexture(tex);
                    var hash = HashWithCache.getTk2dSpriteHash(tk2ds);
                    MaterialProcessed[crc] = hash;
                    SaveTextureByPath("Global",hash,dupe);
                    GameObject.Destroy(dupe);
                }
                if(scene.name != "DontDestroyOnLoad"){
                    SaveTextureDump(scene,name, tex);
                }
                return;
            }
        }

        internal Coroutine dumpAllSpritesCoroutineRef;
        internal bool pending = false;
        internal int detected = 0 , done = 0;
        internal bool DontDestroyOnLoadScene = true; 
        internal void updateDumpProgressText(){
            SkinSwapperPanel.UpdateDumpProgressText(detected,done);
        }
        internal void dumpAllSpritesInScene(Scene scene){
            if(scene == null || !scene.IsValid()){return;}
            var GOList = scene.GetAllGameObjects();
            detected += GOList.Count();
            foreach(var go in GOList){
                try{
                    dumpSpriteForGo(scene,go);
                } catch(Exception e){
                    Log(e.ToString());
                }
                done += 1;
            }
            HashWithCache.saveIfUpdated();
        }
        internal IEnumerator dumpAllSpritesCoroutine(){
           done = 0;
           detected = done;
           yield return new WaitForSecondsRealtime(1f);
           do{
            yield return null;
            var scenes = SceneUtils.GetAllLoadedScenes(false);
            pending = false;
            foreach(var scene in scenes){ 
                if(scene == null || !scene.IsValid()){continue;}
                var GOList = scene.GetAllGameObjects();
                detected += GOList.Count();
                foreach(var go in GOList){
                    try{
                        dumpSpriteForGo(scene,go);
                    } catch(Exception e){
                        Log(e.ToString());
                    }
                    done += 1;
                    yield return null;
                    updateDumpProgressText();
                }
            }
           } while(pending); // handle the case where a new go is spawned while the coro is still dumping
           if(DontDestroyOnLoadScene){
                dumpAllSpritesInScene(SceneUtils.GetDontDestroyOnLoadScene());
                DontDestroyOnLoadScene = false;
           }
           HashWithCache.saveIfUpdated();
           dumpAllSpritesCoroutineRef = null;
        }
        internal AsyncOperation loadScene(int i){
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(i);
            asyncLoad.priority = i;
            return asyncLoad;
        }
        
        internal IEnumerator walkScenes(){
            yield return null;
            var sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
            var i = 0; 
            while(true){
                if(dumpAllSpritesCoroutineRef == null || !pending){
                    Log($"loading next scene : id {i}");
                    //load next scenes    
                    if( i < sceneCount){                    
                        try{
                            AsyncOperation asyncLoad = loadScene(i);
                        } catch(Exception e){
                            Modding.Logger.Log($"error in dumpng scene {i} with {e.ToString()}");
                        }
                            yield return new WaitForSeconds(5);            
                            dumpAllSprites();
                            yield return new WaitForSeconds(10);
                            // Wait until the asynchronous scene fully loads & dumps
                            /*while (detected > done)
                            {
                                yield return null;
                            }*/
                        try{
                            if(i > 3){
                                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(i);
                            }
                        } catch(Exception e){
                            Modding.Logger.Log($"error in dumpng scene {i} with {e.ToString()}");
                        }
                        i++;
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }
        } 
        internal void walk(){
            CoroutineHelper.GetRunner().StartCoroutine(walkScenes());
        }
        internal void dumpAllSprites(){
            if(!enabled) {return;} 
            pending = true;
            if(dumpAllSpritesCoroutineRef == null){
                dumpAllSpritesCoroutineRef = CoroutineHelper.GetRunner().StartCoroutine(dumpAllSpritesCoroutine());
            }
        }

        internal void dumpAllSprites(Scene scene,LoadSceneMode mode){
            if(!enabled) {return;} 
            Log($"Entered scene : Name {scene.name}");
            dumpAllSprites();
        }
        
        internal void dumpAllSprites(On.HutongGames.PlayMaker.Actions.ActivateGameObject.orig_DoActivateGameObject orig, HutongGames.PlayMaker.Actions.ActivateGameObject self){
            orig(self);
            if(!enabled) {return;} 
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
                if(dupe != texture){
                    GameObject.Destroy(dupe);
                }
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