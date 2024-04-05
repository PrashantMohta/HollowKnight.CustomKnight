namespace CustomKnight
{
    public class MAQ : Skinable_Sprite
    {
        public string Name;
        public MAQ(string name) : base(name)
        {
            Name = name;
        }
        public override void SaveDefaultTexture()
        {

            PlayMakerFSM eqfsm = SkinManager.equipment.LocateMyFSM("Build Equipment List");
            switch (Name)
            {
                case "Inventory/Map":
                    ckTex.defaultSprite = eqfsm.GetAction<SetSpriteRendererSprite>("Map", 1).sprite.Value as Sprite;
                    break;
                case "Inventory/Quill":
                    ckTex.defaultSprite = eqfsm.GetAction<SetSpriteRendererSprite>("Quill", 1).sprite.Value as Sprite;
                    break;
                case "Inventory/MapQuill":
                    ckTex.defaultSprite = eqfsm.GetAction<SetSpriteRendererSprite>("Map and Quill", 1).sprite.Value as Sprite;
                    break;
                default:
                    break;
            }
        }
        public override void ApplySprite(Sprite sprite)
        {
            PlayMakerFSM eqfsm = SkinManager.equipment.LocateMyFSM("Build Equipment List");
            switch (Name)
            {
                case "Inventory/Map":
                    eqfsm.GetAction<SetSpriteRendererSprite>("Map", 1).sprite.Value = sprite;
                    break;
                case "Inventory/Quill":
                    eqfsm.GetAction<SetSpriteRendererSprite>("Quill", 1).sprite.Value = sprite;
                    break;
                case "Inventory/MapQuill":
                    eqfsm.GetAction<SetSpriteRendererSprite>("Map and Quill", 1).sprite.Value = sprite;
                    break;
                default:
                    break;
            }
        }
    }
}
