using CustomKnight.Canvas;
using CustomKnight.Skin.Swapper;
using System.IO;
using System.Linq;
using static Satchel.GameObjectUtils;
using static Satchel.IoUtils;

namespace CustomKnight
{
    /// <summary>
    /// DumpEvent
    /// </summary>
    public class DumpEvent : EventArgs
    {
        /// <summary>
        /// Scene of the dump object
        /// </summary>
        public Scene scene { get; set; }
        /// <summary>
        /// The dump GameObject
        /// </summary>
        public GameObject go { get; set; }
    }

    /// <summary>
    /// Main class for managing dumping sprites
    /// </summary>
    public class DumpManager
    {
        /// <summary>
        /// Event raised jsut before a game object is dumped
        /// </summary>
        public static event EventHandler<DumpEvent> BeforeDumpingGameObject;

        private bool enabled = false;

        internal Dictionary<string, bool> isTextureDumped = new();
        internal Dictionary<GameObject, bool> isProcessed = new();
        internal Dictionary<int, string> MaterialProcessed = new();

        internal Coroutine dumpAllSpritesCoroutineRef;
        internal bool pending = false;
        internal int detected = 0, done = 0;
        internal bool DontDestroyOnLoadScene = true;

        internal DumpManager()
        {

        }
        /// <summary>
        /// GetIsEnabled
        /// </summary>
        /// <returns></returns>
        public bool GetIsEnabled()
        {
            return enabled;
        }
        internal void Hook()
        {
            enabled = true;
            ModHooks.LanguageGetHook += SaveTextDump;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += DumpAllSprites;
            On.HutongGames.PlayMaker.Actions.ActivateGameObject.DoActivateGameObject += DumpAllSprites;
        }

        internal void Unhook()
        {
            enabled = false;
            ModHooks.LanguageGetHook -= SaveTextDump;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= DumpAllSprites;
            On.HutongGames.PlayMaker.Actions.ActivateGameObject.DoActivateGameObject -= DumpAllSprites;
        }
        /// <summary>
        /// Main entrypoint to start the dumping process
        /// </summary>
        internal void DumpAllSprites()
        {
            if (!enabled) { return; }
            pending = true;
            if (dumpAllSpritesCoroutineRef == null)
            {
                dumpAllSpritesCoroutineRef = CoroutineHelper.GetRunner().StartCoroutine(dumpAllSpritesCoroutine());
            }
        }

        /// <summary>
        /// Function that actually determines what to dump given a scene and gameobject
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="go"></param>
        internal void dumpSpriteForGo(Scene scene, GameObject go)
        {
            if (go == null) { return; }
            if (isProcessed.TryGetValue(go, out var done))
            {
                return;
            }
            var baseName = go.GetPath(true);
            var goPath = go.GetPath();

            Animator anim = go.GetComponent<Animator>();
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            tk2dSprite tk2ds = go.GetComponent<tk2dSprite>();
            ParticleSystemRenderer particle = go.GetComponent<ParticleSystemRenderer>();

            var mat = sr != null ? sr.material : (tk2ds != null ? tk2ds.GetCurrentSpriteDef()?.material : null);
            int crc = 0;
            var isMaterialUnprocessed = true;
            if (mat != null)
            {
                crc = mat.ComputeCRC();
                if (MaterialProcessed.TryGetValue(crc, out var _hash))
                {
                    isMaterialUnprocessed = false;
                }
            }

            BeforeDumpingGameObject?.Invoke(CustomKnight.Instance, new DumpEvent() { scene = scene, go = go });
            Log($"dumping {done}/{detected}");
            Log($"game object to be dumped - {go.name}");
            Log($"gameobject path {baseName}");
            isProcessed[go] = true;
            if(CustomKnight.GlobalSettings.EnableParticleSwap && particle != null && particle.material != null)
            {
                var tex = (Texture2D) particle.material.mainTexture;
                var dupe = TextureUtils.duplicateTexture(tex);
                var hash = dupe.getHash();
                SaveTextureDump(scene, hash, dupe);
                ObjectNameResolver.Add(scene.name, goPath, hash);
                Log($"GO with particle renderer {hash}.png");
            }
            if (sr != null && sr.sprite != null)
            {
                // Case for Sprites
                if (scene.name == "DontDestroyOnLoad" && !SpecialCases.AllowedDontDestroyOnLoad(goPath))
                {
                    return; // dont dump sprites from DontDestroyOnLoad
                }
                if (!CustomKnight.GlobalSettings.DumpOldSwaps)
                {   // New style dumps will save sprites with hash mode for names.json
                    var tex = SpriteUtils.ExtractTextureFromSprite(sr.sprite);
                    var hash = tex.getHash();
                    MaterialProcessed[crc] = hash;
                    ObjectNameResolver.Add(scene.name, goPath, hash);
                    SaveTextureDump(scene, hash, tex);
                    GameObject.Destroy(tex);
                    if (anim != null || SpecialCases.ChildSpriteAnimatedByParent(baseName))
                    {
                        ObjectNameResolver.Add(scene.name, goPath, hash);
                        SaveTextureDump(scene, hash, tex);
                    }
                }
                else
                {   // old style dump will dump with directory structure
                    if (anim != null || SpecialCases.ChildSpriteAnimatedByParent(baseName))
                    {
                        // remove the animation component
                        //GameObject.Destroy(anim);
                        //go.AddComponent<Animator>();
                        var tex1 = sr.sprite.texture;
                        if (CustomKnight.GlobalSettings.DumpOldSwaps)
                        {
                            SaveTextureDump(scene, baseName, tex1);
                        }
                        return;
                    }
                    SaveSpriteDump(scene, baseName, sr.sprite);
                }
                return;
            }
            if (tk2ds != null)
            {
                // Case for tk2d 
                var sdef = tk2ds.GetCurrentSpriteDef();
                var tex = (Texture2D)sdef?.material?.mainTexture;
                if (tex == null)
                {
                    return;
                }
                if (!CustomKnight.GlobalSettings.DumpOldSwaps)
                {
                    //New style dumps will save textures with hash mode for names.json
                    if (!isMaterialUnprocessed)
                    {
                        return;
                    }
                    var dupe = TextureUtils.duplicateTexture(tex);
                    var hash = HashWithCache.getTk2dSpriteHash(tk2ds);
                    MaterialProcessed[crc] = hash;
                    ObjectNameResolver.Add(scene.name, goPath, hash);
                    if (scene.name != "DontDestroyOnLoad" || SpecialCases.AllowedDontDestroyOnLoad(goPath))
                    {
                        SaveTextureDump(scene, hash, dupe);
                    }
                    SaveTextureByPath("Global", hash, dupe); // Also dump tk2ds in global
                    GameObject.Destroy(dupe);
                }
                else
                {
                    // old style dump will dump with directory structure
                    if (isMaterialUnprocessed)
                    {
                        var dupe = TextureUtils.duplicateTexture(tex);
                        var hash = HashWithCache.getTk2dSpriteHash(tk2ds);
                        MaterialProcessed[crc] = hash;
                        SaveTextureByPath("Global", hash, dupe);
                        GameObject.Destroy(dupe);
                    }
                    if (scene.name != "DontDestroyOnLoad" || SpecialCases.AllowedDontDestroyOnLoad(goPath))
                    {
                        SaveTextureDump(scene, baseName, tex);
                    }
                }



                return;
            }
        }

        internal void updateDumpProgressText()
        {
            DumpUIMain.UpdateDumpProgressText(detected, done);
        }
        internal void dumpAllSpritesInScene(Scene scene)
        {
            if (scene == null || !scene.IsValid()) { return; }
            var GOList = scene.GetAllGameObjects();
            detected += GOList.Count();
            foreach (var go in GOList)
            {
                try
                {
                    dumpSpriteForGo(scene, go);
                }
                catch (Exception e)
                {
                    Log(e.ToString());
                }
                done += 1;
            }
            HashWithCache.saveIfUpdated();
        }
        internal IEnumerator dumpAllSpritesCoroutine()
        {
            done = 0;
            detected = done;
            yield return new WaitForSecondsRealtime(1f);
            do
            {
                yield return null;
                var scenes = SceneUtils.GetAllLoadedScenes(false);
                pending = false;
                foreach (var scene in scenes)
                {
                    if (scene == null || !scene.IsValid()) { continue; }
                    var GOList = scene.GetAllGameObjects();
                    detected += GOList.Count();
                    foreach (var go in GOList)
                    {
                        try
                        {
                            dumpSpriteForGo(scene, go);
                        }
                        catch (Exception e)
                        {
                            Log(e.ToString());
                        }
                        done += 1;
                        yield return null;
                        updateDumpProgressText();
                    }
                }
            } while (pending); // handle the case where a new go is spawned while the coro is still dumping
            if (DontDestroyOnLoadScene)
            {
                dumpAllSpritesInScene(SceneUtils.GetDontDestroyOnLoadScene());
                DontDestroyOnLoadScene = false;
            }
            HashWithCache.saveIfUpdated();
            ObjectNameResolver.DumpNameDb();
            dumpAllSpritesCoroutineRef = null;
        }
        internal AsyncOperation loadScene(int i)
        {
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(i);
            asyncLoad.priority = i;
            return asyncLoad;
        }

        internal IEnumerator walkScenes()
        {
            yield return null;
            var sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
            var i = 0;
            while (true)
            {
                if (dumpAllSpritesCoroutineRef == null || !pending)
                {
                    Log($"loading next scene : id {i}");
                    //load next scenes    
                    if (i < sceneCount)
                    {
                        try
                        {
                            AsyncOperation asyncLoad = loadScene(i);
                        }
                        catch (Exception e)
                        {
                            CustomKnight.Instance.Log($"Error in dumpng scene {i} with {e}");
                        }
                        yield return new WaitForSeconds(5);
                        DumpAllSprites();
                        yield return new WaitForSeconds(10);
                        // Wait until the asynchronous scene fully loads & dumps
                        /*while (detected > done)
                        {
                            yield return null;
                        }*/
                        try
                        {
                            if (i > 3)
                            {
                                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(i);
                            }
                        }
                        catch (Exception e)
                        {
                            CustomKnight.Instance.Log($"error in dumpng scene {i} with {e}");
                        }
                        i++;
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        internal void walk()
        {
            CoroutineHelper.GetRunner().StartCoroutine(walkScenes());
        }



        internal void DumpAllSprites(Scene scene, LoadSceneMode mode)
        {
            if (!enabled) { return; }
            Log($"Entered scene : Name {scene.name}");
            DumpAllSprites();
        }

        internal void DumpAllSprites(On.HutongGames.PlayMaker.Actions.ActivateGameObject.orig_DoActivateGameObject orig, HutongGames.PlayMaker.Actions.ActivateGameObject self)
        {
            orig(self);
            if (!enabled) { return; }
            if (self.gameObject.GameObject.Value != null)
            {
                DumpAllSprites();
            }
        }
        internal void SaveSpriteDump(Scene scene, string objectName, Sprite sprite)
        {
            if (!enabled) { return; }
            string DUMP_DIR = Path.Combine(SkinManager.DATA_DIR, "Dump");
            string scenePath = Path.Combine(DUMP_DIR, scene.name);
            EnsureDirectory(DUMP_DIR);
            EnsureDirectory(scenePath);

            string outpath = Path.Combine(scenePath, objectName.Replace('/', Path.DirectorySeparatorChar) + ".png");
            try
            {
                EnsureDirectory(Path.GetDirectoryName(outpath));
            }
            catch (IOException e)
            {
                Log(e.ToString());
            }
            if (!isTextureDumped.TryGetValue(outpath, out bool path) && !File.Exists(outpath))
            {
                Texture2D dupe = SpriteUtils.ExtractTextureFromSprite(sprite);
                byte[] texBytes = dupe.EncodeToPNG();
                try
                {
                    File.WriteAllBytes(outpath, texBytes);
                }
                catch (IOException e)
                {
                    Log(e.ToString());
                }
                isTextureDumped[outpath] = true;
            }
        }

        internal void SaveTextureByPath(string sceneName, string objectName, Texture2D texture)
        {
            string DUMP_DIR = Path.Combine(SkinManager.DATA_DIR, "Dump");
            string scenePath = Path.Combine(DUMP_DIR, sceneName);

            EnsureDirectory(DUMP_DIR);
            EnsureDirectory(scenePath);

            string outpath = Path.Combine(scenePath, objectName.Replace('/', Path.DirectorySeparatorChar) + ".png");
            try
            {
                EnsureDirectory(Path.GetDirectoryName(outpath));
            }
            catch (IOException e)
            {
                Log(e.ToString());
            }
            if (!isTextureDumped.TryGetValue(outpath, out bool path) && !File.Exists(outpath))
            {
                Texture2D dupe = texture.isReadable ? texture : TextureUtils.duplicateTexture(texture);
                byte[] texBytes = dupe.EncodeToPNG();
                if (dupe != texture)
                {
                    GameObject.Destroy(dupe);
                }
                try
                {
                    File.WriteAllBytes(outpath, texBytes);
                }
                catch (IOException e)
                {
                    Log(e.ToString());
                }
                isTextureDumped[outpath] = true;
            }
        }
        internal void SaveTextureDump(Scene scene, string objectName, Texture2D texture)
        {
            if (!enabled) { return; }
            SaveTextureByPath(scene.name, objectName, texture);
        }
        internal void SaveTextDump(string key, string value)
        {
            if (!enabled) { return; }
            string DUMP_DIR = Path.Combine(SkinManager.DATA_DIR, "Dump");
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            string scenePath = Path.Combine(DUMP_DIR, scene.name);
            EnsureDirectory(DUMP_DIR);
            EnsureDirectory(scenePath);
            var outpath = (Path.Combine(scenePath, key + ".txt"));

            try
            {
                EnsureDirectory(Path.GetDirectoryName(outpath));
            }
            catch (IOException e)
            {
                Log(e.ToString());
            }
            if (!File.Exists(outpath))
            {
                File.WriteAllText(outpath, value);
            }
        }
        internal string SaveTextDump(string key, string sheet, string value)
        {
            SaveTextDump(sheet + key, value);
            return value;
        }

        internal void Log(string str)
        {
            CustomKnight.Instance.Log("[DumpManager] " + str);
        }
        internal static void debugDumpSprite(Sprite sprite)
        {
            Texture2D dupe = Satchel.SpriteUtils.ExtractTextureFromSprite(sprite);
            Satchel.TextureUtils.WriteTextureToFile(dupe, $"{Satchel.AssemblyUtils.getCurrentDirectory()}/{sprite.name}.png");
        }
        internal static void debugDumpTex(Texture2D tex, string name)
        {
            Satchel.TextureUtils.WriteTextureToFile(tex, $"{Satchel.AssemblyUtils.getCurrentDirectory()}/{name}.png");
        }
    }
}