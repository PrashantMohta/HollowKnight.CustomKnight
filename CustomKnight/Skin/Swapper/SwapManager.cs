using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static Satchel.IoUtils;

namespace CustomKnight
{
    public class SwapEvent : EventArgs
    {
        public GameObjectProxy gop { get; set; }
        public GameObject go { get; set; }
    }
    public class SwapManager
    {
        private bool alwaysReprocessMaterial = false; // should be false in release
        private string DATA_DIR;
        internal string SWAP_FOLDER = "Swap";
        private string SEPARATOR = "=>";
        internal float BACKOFF_MULTIPLIER = 1.3f;
        internal int INITAL_NEXT_CHECK = 1000;
        internal int nextCheck;
        internal Dictionary<string, Dictionary<string, GameObjectProxy>> Scenes;
        internal List<string> currentSkinnedSceneObjs;
        internal Dictionary<string, Texture2D> loadedTextures;

        internal Dictionary<string, Material> materials;
        internal Dictionary<string, Texture2D> defaultTextures;

        internal Dictionary<string, string> Strings;
        internal Dictionary<string, string> ReplaceStrings;
        internal Dictionary<string, List<string>> ReplaceCache;

        internal DateTime lastTime = DateTime.Now;

        internal bool active = false;
        internal bool enabled = false;
        public SwapManager()
        {
            Load();
            var g = new GameObject();
            GameObject.DontDestroyOnLoad(g);
            g.AddComponent<coroutineHelper>().StartCoroutine(processTk2d());
        }

        internal Queue<tk2dSprite> tk2dSpriteQueue = new();
        internal Dictionary<tk2dSprite, bool> processedGlobal = new();
        internal bool isValidForGlobalSwap(tk2dSprite tk)
        {
            if (tk == null) { return false; }
            if (alwaysReprocessMaterial) { return true; }
            var go = tk.gameObject;
            var colliders = go.GetComponent<Collider2D>();
            if (colliders == null)
            {
                return false;
            }
            /*
            if (go.GetComponent<DamageHero>() || go.LocateMyFSM("damages_hero"))
            {
                return true;
            } 
            else if (go.GetComponent<HealthManager>()||go.LocateMyFSM("health_manager_enemy") || go.LocateMyFSM("health_manager"))
            {
                return true;
            }*/
            return false;
        }
        internal void QueueTk2d(tk2dSprite tk)
        {
            var done = false;
            if (processedGlobal.TryGetValue(tk, out done))
            {
                return;
            }
            if (!done && isValidForGlobalSwap(tk))
            {
                processedGlobal[tk] = true;
                tk2dSpriteQueue.Enqueue(tk);
            }
        }

        internal void findAndQueueTk2d(GameObject go)
        {
            var tks = go.GetComponentsInChildren<tk2dSprite>();
            if (tks != null)
            {
                foreach (var tk in tks)
                {
                    QueueTk2d(tk);
                }
            }
        }

        internal IEnumerator processTk2d()
        {
            while (true)
            {
                yield return null;
                if (tk2dSpriteQueue.Count > 0)
                {
                    yield return null;
                    Log("processing global tk2ds count : " + tk2dSpriteQueue.Count);
                    while (true)
                    {
                        if (tk2dSpriteQueue.Count <= 0) { break; }
                        var tk = tk2dSpriteQueue.Dequeue();
                        if (tk != null && tk.gameObject != null)
                        {
                            applyGlobalTk2d(tk);
                        }
                    }
                    HashWithCache.saveIfUpdated();
                }
            }
        }

        internal bool SwapSkinRoutineRunning = false;

        private void loadTexture(GameObjectProxy gop)
        {
            this.Log(gop.name + gop.getTexturePath() + gop.getAliasPath());
            string objectPath = gop.getTexturePath();
            string aliasPath = gop.getAliasPath();
            if (loadedTextures.TryGetValue(objectPath, out var tex))
            {
                return;
            }
            byte[] buffer;
            string defaultDirectory = Path.Combine(SkinManager.DATA_DIR, SWAP_FOLDER);
            string currentDirectory = DATA_DIR;
            if (File.Exists(Path.Combine(currentDirectory, objectPath)))
            {
                buffer = File.ReadAllBytes(Path.Combine(currentDirectory, objectPath));
            }
            else if (File.Exists(Path.Combine(currentDirectory, aliasPath)))
            {
                buffer = File.ReadAllBytes(Path.Combine(currentDirectory, aliasPath));
            }
            else if (File.Exists(Path.Combine(defaultDirectory, objectPath)))
            {
                buffer = File.ReadAllBytes(Path.Combine(defaultDirectory, objectPath));
            }
            else if (File.Exists(Path.Combine(defaultDirectory, aliasPath)))
            {
                buffer = File.ReadAllBytes(Path.Combine(defaultDirectory, aliasPath));
            }
            else
            {
                return;
            }

            var texture = new Texture2D(2, 2);
            texture.LoadImage(buffer.ToArray(), true);
            loadedTextures[objectPath] = texture;
        }

        private void SwapSkinForGo(string objectPath, GameObject GO)
        {
            CustomKnight.Instance.LogDebug($"op {objectPath} {GO.name}");
            Texture2D tex;// = loadedTextures[objectPath];
            if (!loadedTextures.TryGetValue(objectPath, out tex))
            {
                return;
            }
            var _tk2dSprite = GO.GetComponent<tk2dSprite>();
            if (_tk2dSprite == null)
            {
                var anim = GO.GetComponent<Animator>();
                var sr = GO.GetComponent<SpriteRenderer>();

                //assume sprite
                if (sr == null)
                {
                    this.Log("No tk2dSprite or SpriteRenderer Component found in " + objectPath);
                }
                else
                {
                    if (anim != null || SpecialCases.childSpriteAnimatedByParent(objectPath))
                    {
                        //maybe animated things can be replaced with a single sprite
                        if (anim != null)
                        {
                            this.LogFine($"Animation  : {anim.name}");
                        }
                        var behaviour = GO.GetAddComponent<SpriteRendererMaterialPropertyBlock>();
                        MaterialPropertyBlock block = new MaterialPropertyBlock();
                        block.AddTexture("_MainTex", tex);
                        behaviour.mpb = block;
                        //GameObject.Destroy(anim);
                        //go.AddComponent<Animator>();
                        // destroyed the animation, possibly add satchel customAnimation later
                    }
                    else
                    {
                        //currentSkinnedSceneObjs.Add(objectPath); re add sprites for a while
                        //some sprites are still not perfectly matched 
                        this.LogFine($"game object : {sr.name} ");
                        var pivot = new Vector2(0.5f, 0.5f); // this needs offset sometimes
                        sr.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), pivot, sr.sprite.pixelsPerUnit);
                        //Log($"pivot post application {sr.sprite.pivot/new Vector2(tex.width, tex.height)}");
                    }
                }

            }
            else
            {
                currentSkinnedSceneObjs.Add(objectPath);
                materials[objectPath] = _tk2dSprite.GetCurrentSpriteDef().material;
                defaultTextures[objectPath] = (Texture2D)materials[objectPath].mainTexture;
                _tk2dSprite.GetCurrentSpriteDef().material.mainTexture = tex;
            }
        }


        public static event EventHandler<SwapEvent> OnApplySkinUsingProxy;

        private void applySkinsUsingProxy(GameObjectProxy gop, GameObject go)
        {
            //CustomKnight.Instance.Log("Traversing : " + gop.getTexturePath());
            if (go == null)
            {
                CustomKnight.Instance.Log("Null Go : " + gop.getTexturePath());
                return;
            }
            if (gop.hasTexture)
            {
                var texPath = gop.getTexturePath();
                //CustomKnight.Instance.Log("hasTexture");
                try
                {
                    loadTexture(gop);
                }
                catch (Exception e)
                {
                    this.Log(gop.name + " " + e.ToString());
                }
                if (!currentSkinnedSceneObjs.Contains(texPath))
                {
                    SwapSkinForGo(texPath, go);
                }
            }
            OnApplySkinUsingProxy?.Invoke(CustomKnight.Instance, new SwapEvent() { gop = gop, go = go });
            //traverse this gop
            if (gop.hasChildren && go.transform.childCount > 0)
            {
                //CustomKnight.Instance.Log("hasChildren " + gop.children.Count() + " c " + go.transform.childCount);
                foreach (KeyValuePair<string, GameObjectProxy> kvp in gop.children)
                {
                    try
                    {
                        this.LogFine("Trying children with name : " + kvp.Key);
                        var children = go.FindGameObjectsInChildren(kvp.Key, true);
                        foreach (var child in children)
                        {
                            applySkinsUsingProxy(kvp.Value, child);
                        }
                    }
                    catch (Exception e)
                    {
                        this.Log(kvp.Key + " " + e.ToString());
                    }
                }
            }
        }
        private void SwapSkinForAllScenes()
        {
            var scenes = SceneUtils.GetAllLoadedScenes(false);
            foreach (var scene in scenes)
            {
                SwapSkin(scene);
            }
        }
        private void SwapSkin(Scene scene)
        {

            if (Scenes != null && Scenes.TryGetValue(scene.name, out var CurrentSceneDict))
            {
                var rootGos = scene.GetRootGameObjects();
                /*foreach(KeyValuePair<string,GameObjectProxy> kvp in CurrentSceneDict){
                    Log($"={kvp.Key}");
                }*/
                foreach (var go in rootGos)
                {
                    if (CurrentSceneDict.TryGetValue(go.GetName(true), out var gop))
                    {
                        //Log($"+{go.GetName(true)}");
                        applySkinsUsingProxy(gop, go);
                    }
                }
            }
        }
        private IEnumerator SwapSkinRoutine(Scene scene)
        {
            SwapSkinRoutineRunning = true;
            yield return null;
            SwapSkin(SceneUtils.GetDontDestroyOnLoadScene());
            SwapSkinForAllScenes();
            SwapSkinRoutineRunning = false;
        }
        internal void SwapSkinForScene(Scene scene, LoadSceneMode mode)
        {
            if (!active && !enabled) { return; }
            currentSkinnedSceneObjs = new List<string>();
            nextCheck = INITAL_NEXT_CHECK;
            CoroutineHelper.GetRunner().StartCoroutine(SwapSkinRoutine(scene));
        }

        internal void checkForMissedObjects()
        {
            if (!active && !enabled) { return; }
            var currentTime = DateTime.Now;
            if (nextCheck > 0 && (currentTime - lastTime).TotalMilliseconds > nextCheck)
            {
                SwapSkinForAllScenes();
                nextCheck = (int)Math.Round((float)nextCheck * BACKOFF_MULTIPLIER);
                lastTime = currentTime;
            }
        }

        internal string LanguageGet(string key, string sheet, string orig)
        {
            if (!enabled && !active)
            {
                return orig;
            }
            string overrideText;
            if (Strings != null && Strings.TryGetValue(sheet + key, out overrideText))
            {
                return overrideText;
            }
            if (ReplaceCache != null && ReplaceCache.TryGetValue(sheet + key, out var cachedText))
            {
                if (cachedText[0] == orig)
                { // only consider it valid cache if orig matches first element of list
                    return cachedText[1];
                }
            }
            string textValue = orig;
            if (ReplaceStrings != null)
            {
                foreach (KeyValuePair<string, string> kp in ReplaceStrings)
                {
                    textValue = Regex.Replace(textValue, Regex.Escape(kp.Key), kp.Value.Replace("$", "$$"), RegexOptions.IgnoreCase);
                }
                //cache for next time
                ReplaceCache[sheet + key] = new List<string> { orig, textValue };
            }
            return textValue;
        }

        internal void LoadSwapByPath(string pathToLoad)
        {
            EnsureDirectory(pathToLoad);
            if (!File.Exists(Path.Combine(pathToLoad, "replace.txt")))
            {
                using (FileStream fs = File.Create(Path.Combine(pathToLoad, "replace.txt")))
                {
                    //create and close the stream
                };
            }
            using (StreamReader reader = File.OpenText(Path.Combine(pathToLoad, "replace.txt")))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Log(line);
                    int index = line.IndexOf(SEPARATOR);
                    if (index > 0 && index < line.Length - 1)
                    {
                        string replace = line.Substring(0, index).ToLower();
                        string with = line.Substring(index + SEPARATOR.Length);

                        ReplaceStrings[replace] = with;
                    }
                }
            }
            List<string> hashPaths = new List<string>();

            foreach (string path in Directory.GetDirectories(pathToLoad))
            {
                string directoryName = new DirectoryInfo(path).Name;
                //Log(directoryName);
                Dictionary<string, GameObjectProxy> objects;
                if (!Scenes.TryGetValue(directoryName, out objects))
                {
                    objects = new Dictionary<string, GameObjectProxy>();
                }
                foreach (string file in Directory.GetFiles(path))
                {
                    string filename = Path.GetFileName(file);
                    //Log(filename);
                    if (filename.EndsWith(".txt"))
                    {
                        try
                        {
                            Strings[filename.Replace(".txt", "")] = File.ReadAllText(file);
                        }
                        catch (Exception e)
                        {
                            this.Log(filename + " " + e.ToString());
                            continue;
                        }
                    }
                    else
                    {
                        //fileType
                        string extension = Path.GetExtension(file);
                        string objectName = filename.Replace(extension, "");
                        GameObjectProxy GOP = new GameObjectProxy()
                        {
                            name = objectName,
                            hasTexture = true,
                            rootPath = directoryName,
                            hasChildren = false,
                            fileType = extension
                        };
                        objects[objectName] = GOP;
                        if (directoryName == "Global")
                        {
                            var hp = HashWithCache.GetPathsFromHash(objectName);
                            Log(filename + "|" + objectName);
                            if (hp != null)
                            {
                                Log(hp.Count.ToString());
                                hashPaths.AddRange(hp);
                            }
                        }
                    }
                }
                foreach (string childDirectory in Directory.GetDirectories(path))
                {
                    string childDirectoryName = new DirectoryInfo(childDirectory).Name;
                    Log(childDirectoryName);
                    GameObjectProxy GOP;
                    if (!objects.TryGetValue(childDirectoryName, out GOP))
                    {
                        GOP = new GameObjectProxy();
                    }
                    GOP.name = childDirectoryName;
                    GOP.rootPath = directoryName;
                    GOP.hasChildren = true;
                    objects[childDirectoryName] = GOP;
                    GOP.TraverseGameObjectDirectory(pathToLoad);
                }
                Scenes[directoryName] = objects;
            }
            foreach (var hp in hashPaths)
            {
                var hpSplit = hp.Split(new Char[] { '/' }, 2);
                Dictionary<string, GameObjectProxy> scene = null;
                if (Scenes.ContainsKey(hpSplit[0]))
                {
                    scene = Scenes[hpSplit[0]];
                }
                else
                {
                    scene = new Dictionary<string, GameObjectProxy>();
                    Scenes[hpSplit[0]] = scene;
                }
                var goSplit = hpSplit[1].Split(new Char[] { '/' }, 2);
                if (goSplit.Length > 0)
                {
                    GameObjectProxy currentGop;
                    if (!scene.TryGetValue(goSplit[0], out currentGop))
                    {
                        currentGop = new GameObjectProxy()
                        {
                            name = goSplit[0],
                            hasTexture = false,
                            hasChildren = true
                        };
                        scene.Add(goSplit[0], currentGop);
                    }
                    var hash = HashWithCache.GetHashFromPath(hp);
                    currentGop.TraverseGameObjectPath(hpSplit[1], "Global", hash, HashWithCache.GetAliasFromHash(hash));

                }
            }
        }

        internal void resetAndLoadGlobalSwaps()
        {

            Scenes = new Dictionary<string, Dictionary<string, GameObjectProxy>>();
            currentSkinnedSceneObjs = new List<string>();
            loadedTextures = new Dictionary<string, Texture2D>();

            materials = new Dictionary<string, Material>();
            defaultTextures = new Dictionary<string, Texture2D>();

            Strings = new Dictionary<string, string>();
            ReplaceStrings = new Dictionary<string, string>();
            ReplaceCache = new Dictionary<string, List<string>>();
            nextCheck = INITAL_NEXT_CHECK;

            LoadSwapByPath(Path.Combine(SkinManager.DATA_DIR, SWAP_FOLDER)); // global strings and skins
        }
        internal void Swap(string skinpath)
        {


            DATA_DIR = Path.Combine(skinpath, SWAP_FOLDER);

            EnsureDirectory(DATA_DIR);

            LoadSwapByPath(DATA_DIR); // over write global strings with local strings 

            // only need to do this once local strings are loaded
            UpdateTextMeshGameTexts();

            if (Directory.GetDirectories(DATA_DIR).Length == 0)
            {
                Log("There are no folders in the Swap directory. No textures to Swap.");
                return;
            }
            CoroutineHelper.GetRunner().StartCoroutine(SwapSkinRoutine(UnityEngine.SceneManagement.SceneManager.GetActiveScene()));
        }

        internal void resetAllTextures()
        {
            if (materials != null)
            {
                foreach (KeyValuePair<string, Material> kp in materials)
                {
                    if (kp.Value == null)
                    {
                        continue;
                    }
                    kp.Value.mainTexture = defaultTextures[kp.Key];
                }
            }
        }
        internal void tk2dSpriteAwake(On.tk2dSprite.orig_Awake orig, tk2dSprite self)
        {
            orig(self);
            if (active || enabled)
            {
                QueueTk2d(self);
            }
        }
        internal void Load()
        {
            ModHooks.LanguageGetHook += LanguageGet;
            ModHooks.HeroUpdateHook += checkForMissedObjects;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SwapSkinForScene;
            On.HutongGames.PlayMaker.Actions.ActivateGameObject.DoActivateGameObject += ActivateGameObject;
            On.tk2dSprite.Awake += tk2dSpriteAwake;
            On.SetTextMeshProGameText.Awake += SetTextMeshProGameText_Awake;
        }

        internal void Unload()
        {
            ModHooks.LanguageGetHook -= LanguageGet;
            ModHooks.HeroUpdateHook -= checkForMissedObjects;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SwapSkinForScene;
            On.HutongGames.PlayMaker.Actions.ActivateGameObject.DoActivateGameObject -= ActivateGameObject;
            On.tk2dSprite.Awake -= tk2dSpriteAwake;
            On.SetTextMeshProGameText.Awake -= SetTextMeshProGameText_Awake;
            resetAllTextures();
        }
        private List<SetTextMeshProGameText> setTextMeshProGameTexts = new List<SetTextMeshProGameText>();
        private void SetTextMeshProGameText_Awake(On.SetTextMeshProGameText.orig_Awake orig, SetTextMeshProGameText self)
        {
            orig(self);
            setTextMeshProGameTexts.Add(self);
        }
        private void UpdateTextMeshGameTexts()
        {
            foreach(var tmpro in setTextMeshProGameTexts)
            {
                if(tmpro != null)
                {
                    tmpro.UpdateText();
                }
            }
        }
        internal GameObjectProxy getGop(string sceneName, GameObject go)
        {
            if (go == null)
            {
                return null;
            }
            Transform rootGoT = go.transform;
            List<string> path = new();
            path.Add(rootGoT.gameObject.GetName(true));
            while (rootGoT.parent != null)
            {
                rootGoT = rootGoT.parent;
                path.Add(rootGoT.gameObject.GetName(true));
            }
            path.Reverse();
            GameObjectProxy Gop = null;
            if (Scenes != null && Scenes.TryGetValue(go.scene.name, out var CurrentSceneDict))
            {
                if (CurrentSceneDict.TryGetValue(rootGoT.gameObject.GetName(true), out var gop))
                {
                    Gop = gop;
                }
            }
            var i = 0;
            if (Gop != null)
            {
                while (Gop.hasChildren)
                {
                    if (!Gop.children.TryGetValue(path[i], out var _Gop))
                    {
                        Gop = null;
                        break;
                    }
                    Gop = _Gop;
                    i++;
                }
                if (Gop != null && Gop.name == go.GetName(true))
                {
                    return Gop;
                }
            }
            return null;
        }
        internal GameObjectProxy getGopGlobal(string sceneName, string hash)
        {
            GameObjectProxy Gop = null;
            if (Scenes != null && Scenes.TryGetValue(sceneName, out var CurrentSceneDict))
            {
                if (CurrentSceneDict.TryGetValue(hash, out var gop))
                {
                    Gop = gop;
                }
            }
            return Gop;
        }

        internal Dictionary<Material, string> MaterialProcessed = new();

        internal void applyGlobalTk2dByHash(string hash, tk2dSprite tk)
        {
            if (hash == null || hash == "")
            {
                return;
            }
            var mat = tk?.GetCurrentSpriteDef()?.material;
            if (mat == null)
            {
                return;
            }
            MaterialProcessed[mat] = hash;
            var Gop = getGopGlobal("Global", hash);
            if (Gop != null)
            {
                applySkinsUsingProxy(Gop, tk.gameObject);
            }
        }

        public void applyGlobalTk2dByPath(string path, tk2dSprite tk)
        {
            if (shouldProcessMaterial(tk))
            {
                var hash = HashWithCache.GetHashFromPath(path);
                applyGlobalTk2dByHash(hash, tk);
            }
        }

        internal bool shouldProcessMaterial(tk2dSprite tk)
        {
            var mat = tk?.GetCurrentSpriteDef()?.material;
            if (mat == null) { return false; }
            if (alwaysReprocessMaterial || !MaterialProcessed.TryGetValue(mat, out var hash))
            {
                return true;
            }
            return false;
        }

        internal void applyGlobalTk2d(tk2dSprite tk)
        {
            if (shouldProcessMaterial(tk))
            {
                var hash = HashWithCache.getTk2dSpriteHash(tk);
                applyGlobalTk2dByHash(hash, tk);
            }
        }
        internal void applyGlobalEntityForGo(GameObject go)
        {
            var tks = go.GetComponentsInChildren<tk2dSprite>();
            if (tks != null)
            {
                foreach (var tk in tks)
                {
                    QueueTk2d(tk);
                }
            }
            //do not load any kind of sprites via hash 
            return;
            if (go.scene.name == "DontDestroyOnLoad")
            {
                return;
            }
            // do not load DontDestroyOnLoad sprites
            var srs = go.GetComponentsInChildren<SpriteRenderer>();
            if (srs != null)
            {
                foreach (var sr in srs)
                {
                    var mat = sr.material;
                    var crc = mat;//.ComputeCRC();
                    string hash;
                    if (alwaysReprocessMaterial || !MaterialProcessed.TryGetValue(crc, out hash))
                    {
                        var tex = SpriteUtils.ExtractTextureFromSprite(sr.sprite);
                        hash = tex.getHash();
                        GameObject.Destroy(tex);
                        MaterialProcessed[crc] = hash;
                    }
                    var Gop = getGopGlobal("Global", hash);
                    if (Gop != null)
                    {
                        applySkinsUsingProxy(Gop, sr.gameObject);
                    }
                }
            }
        }
        internal void ActivateGameObject(On.HutongGames.PlayMaker.Actions.ActivateGameObject.orig_DoActivateGameObject orig, HutongGames.PlayMaker.Actions.ActivateGameObject self)
        {
            orig(self);
            if (!active && !enabled) { return; }
            if (self.activate.Value != true) { return; }
            var go = self.gameObject.GameObject.Value;
            if (go == null) { return; }
            //applyGlobalEntityForGo(go);
            //findAndQueueTk2d(go);
            var Gop = getGop(go.scene.name, go);
            if (Gop != null)
            {
                applySkinsUsingProxy(Gop, go);
            }
        }


        internal void SkinChangeSwap(ISelectableSkin currSkin)
        {
            resetAllTextures();
            resetAndLoadGlobalSwaps();
            if (currSkin.hasSwapper())
            {
                Swap(currSkin.getSwapperPath());
            }
        }
        internal void Log(string str)
        {
            CustomKnight.Instance.Log("[SwapManager] " + str);
        }

        internal void LogFine(string str)
        {
            CustomKnight.Instance.LogFine("[SwapManager] " + str);
        }

    }
}
