namespace CustomKnight
{
    public class MAQ:Skinable_Sprite
    {
        public string Name;
        public MAQ(string name) : base(name)
        {
            Name = name;    
        }
        public override void SaveDefaultTexture()
        {
            GameObject eq = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment");
            PlayMakerFSM eqfsm = eq.LocateMyFSM("Build Equipment List");
            switch (Name)
            {
                case "Map":
                    Sprite Msprite = eqfsm.GetAction<SetSpriteRendererSprite>("Map", 1).sprite.Value as Sprite;
                    ckTex.defaultSprite =CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(Msprite),Msprite.pixelsPerUnit) ;
                    break;
                case "Quill":
                    Sprite Qsprite = eqfsm.GetAction<SetSpriteRendererSprite>("Quill", 1).sprite.Value as Sprite;
                    ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(Qsprite), Qsprite.pixelsPerUnit);
                    break;
                case "MaQ":
                    Sprite sprite = eqfsm.GetAction<SetSpriteRendererSprite>("Map and Quill", 1).sprite.Value as Sprite;
                    ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(sprite), sprite.pixelsPerUnit);
                    break;
                default:
                    break;
            }
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject eq = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment");
            PlayMakerFSM eqfsm = eq.LocateMyFSM("Build Equipment List");
            switch (Name)
            {
                case "Map":
                    eqfsm.GetAction<SetSpriteRendererSprite>("Map", 1).sprite.Value= CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
                    break;
                case "Quill":
                    eqfsm.GetAction<SetSpriteRendererSprite>("Quill", 1).sprite.Value= CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
                    break;
                case "MaQ":
                   eqfsm.GetAction<SetSpriteRendererSprite>("Map and Quill", 1).sprite.Value= CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
                    break;
                default:
                    break;
            }
        }
    }
}
