namespace CustomKnight
{
    public class GodFinder : Skinable_Sprite
    {
        public string Name;
        public GodFinder(string name) : base(name)
        {
            Name = name;
        }
        public override void SaveDefaultTexture()
        {
            GodfinderInvIcon invIcon = SkinManager.invitem.FindGameObjectInChildren("Godfinder").GetComponent<GodfinderInvIcon>();
            if (Name.EndsWith("0"))
            {
                ckTex.defaultSprite = invIcon.normalSprite;
            }
            if (Name.EndsWith("1"))
            {
                ckTex.defaultSprite = invIcon.newBossSprite;
            }
            if (Name.EndsWith("2"))
            {
                ckTex.defaultSprite = invIcon.allBossesSprite;
            }
        }
        public override void ApplySprite(Sprite sprite)
        {
            GodfinderInvIcon invIcon = SkinManager.invitem.FindGameObjectInChildren("Godfinder").GetComponent<GodfinderInvIcon>();
            if (Name.EndsWith("0"))
            {
                invIcon.normalSprite = sprite;
            }
            if (Name.EndsWith("1"))
            {
                invIcon.newBossSprite = sprite;
            }
            if (Name.EndsWith("2"))
            {
                invIcon.allBossesSprite = sprite;
            }

        }
    }
}
