namespace CustomKnight
{
    internal class SpriteLoader {
        private static bool texRoutineRunning;
        private static Coroutine setTexRoutine;
        internal static bool LoadComplete { get; private set; }
        internal static void PullDefaultTextures() {
            if (!SkinManager.savedDefaultTextures) {                
                foreach(KeyValuePair<string,Skinable> kvp in SkinManager.Skinables){
                    kvp.Value?.SaveTexture();
                }
            }
            SkinManager.savedDefaultTextures = true;
        }
        internal static void UnloadAll()
        {    
            if (HeroController.instance != null)
            {
                foreach(KeyValuePair<string,Skinable> kvp in SkinManager.Skinables){
                    kvp.Value.Reset();
                }
            }
            
            if (texRoutineRunning && GameManager.instance != null)
            {
                CoroutineHelper.GetRunner().StopCoroutine(setTexRoutine);
                texRoutineRunning = false;
            }

            DestroyObjects();
        }
        internal static void Load()
        {
            CoroutineHelper.GetRunner().StartCoroutine(Start());
        }
        internal static void ModifyHeroTextures(SaveGameData data = null)
        {
            if (!texRoutineRunning)
            {
                setTexRoutine = CoroutineHelper.GetRunner().StartCoroutine(SetHeroTex());
                texRoutineRunning = true;
            }
        }
        
        internal static IEnumerator Start()
        {
            CustomKnight.swapManager.SkinChangeSwap(SkinManager.CurrentSkin);
            yield return new WaitWhile(
                () => HeroController.instance == null || GameManager.instance == null || GameManager.instance.gameMap == null
            );
            LoadSprites();
        }

        private static void DestroyObjects()
        {
            foreach (KeyValuePair<string, Skinable> pair in SkinManager.Skinables)
            {
                CustomKnightTexture texture = pair.Value.ckTex;
                if (texture.tex != null)
                {
                    GameObject.Destroy(texture.tex);
                }
            }
            LoadComplete = false;
        }

        internal static void SetSkin(Dictionary<string, Skinable> SkinableMap){
            SkinManager.Skinables = SkinableMap;
            ModifyHeroTextures();
        }
        internal static void LoadSprites()
        {
            LoadComplete = false;
            if (SkinManager.CurrentSkin == null)
            {
                SkinManager.CurrentSkin = SkinManager.GetDefaultSkin();
            }
            if(SkinManager.CurrentSkin.shouldCache()){
                TextureCache.recentSkins.Add(SkinManager.CurrentSkin.GetId());
            }
            foreach (KeyValuePair<string,Skinable> kvp in SkinManager.Skinables)
            {
                kvp.Value.prepare();
                CustomKnightTexture texture = kvp.Value.ckTex;
                if(TextureCache.skinTextureCache.TryGetValue(SkinManager.CurrentSkin.GetId(),out var skinCache) && skinCache.TryGetValue(texture.fileName,out var cachedTex)){
                    texture.tex = cachedTex.tex;
                    texture.missing = cachedTex.missing;
                    continue;
                }
                texture.missing = !SkinManager.CurrentSkin.Exists(texture.fileName);
                if (!texture.missing)
                {
                    texture.tex = SkinManager.CurrentSkin.GetTexture(texture.fileName);
                    if(SkinManager.CurrentSkin.shouldCache()){
                        TextureCache.setSkinTextureCache(SkinManager.CurrentSkin.GetId(),texture.fileName,new CustomKnightTexture(texture.fileName,texture.missing,texture.defaultTex,texture.tex));
                    }
                } else {
                    texture.tex = null;
                }
            }
            TextureCache.trimTextureCache();
            SaveHud.GenerateSaveHud(SkinManager.Skinables[Hud.NAME].ckTex.currentTexture, SkinManager.Skinables[OrbFull.NAME].ckTex.currentTexture);
            SetSkin(SkinManager.Skinables);
            LoadComplete = true;
        }

        private static IEnumerator SetHeroTex()
        {
            yield return new WaitWhile(() => !LoadComplete || HeroController.instance == null || CharmIconList.Instance == null);
            /*foreach(var psr in GameObject.FindObjectsOfType<ParticleSystemRenderer>())
            {
                psr.gameObject.LogWithChildren();
                DumpManager.debugDumpTex((Texture2D)psr.material.mainTexture,psr.name);
            }*/
            PullDefaultTextures();
            CustomKnight.swapManager.SkinChangeSwap(SkinManager.CurrentSkin);
            foreach(KeyValuePair<string,Skinable> kvp in SkinManager.Skinables){
                kvp.Value.Apply();
            }
            texRoutineRunning = false;
        }
       


    }
}
