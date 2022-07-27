
namespace CustomKnight
{
    public class BigItem:Skinable_Sprite
    {
        public Func<GameObject> preloadGet;
        public string fsmname;
        public int actionindex;
        public string statename;
        public Sprite sprite;
        public string scenename;
        public string goname;
        public BigItem(string PreloadName,string SceneName,string GOname,string fsmname,string statename, Func<GameObject> preloadGet,int actionindex) : base(PreloadName)
        {
            this.preloadGet = preloadGet;
            this.fsmname= fsmname;
            this.actionindex= actionindex;
            this.statename= statename;
            this.scenename = SceneName;
            this.goname = GOname;
        }
        public override void SaveDefaultTexture()
        {
            if (!CustomKnight.GlobalSettings.Preloads) { return; }
            ckTex.defaultSprite = (Sprite)preloadGet().LocateMyFSM(fsmname).GetState(statename).GetAction<CreateUIMsgGetItem>(actionindex).sprite.Value;
        }
        public override void ApplySprite(Sprite sprite)
        {
            if (!CustomKnight.GlobalSettings.Preloads) { return; }
            this.sprite = sprite;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= ChangeSprite;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += ChangeSprite;
            preloadGet().LocateMyFSM(fsmname).GetState(statename).GetAction<CreateUIMsgGetItem>(actionindex).sprite=sprite;
        }

        private void ChangeSprite(Scene arg0, LoadSceneMode arg1)
        {
            if(arg0.name==scenename)
            {
                arg0.GetGameObjectByName(goname).LocateMyFSM(fsmname).GetState(statename).GetAction<CreateUIMsgGetItem>(actionindex).sprite = sprite;
            }
        }

    }
}
