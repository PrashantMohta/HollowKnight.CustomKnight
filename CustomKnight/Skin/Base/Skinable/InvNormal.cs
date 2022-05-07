
namespace CustomKnight
{
    public class InvNormal:Skinable_Sprite
    {
        public string ItemName;
        public InvNormal(string name) : base(name)
        {
            ItemName = name;
        }
        public override void SaveDefaultTexture()
        {
            GameObject invitem = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Inv_Items");
            switch(ItemName)
            {
                case "DSlash":
                    ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(invitem.FindGameObjectInChildren("Art Uppercut").GetComponent<InvItemDisplay>().activeSprite), invitem.FindGameObjectInChildren("Art Uppercut").GetComponent<InvItemDisplay>().activeSprite.pixelsPerUnit);//so confused
                    break;
                case "CSlash":
                    ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(invitem.FindGameObjectInChildren("Art Cyclone").GetComponent<InvItemDisplay>().activeSprite), invitem.FindGameObjectInChildren("Art Cyclone").GetComponent<InvItemDisplay>().activeSprite.pixelsPerUnit);
                    break;
                case "GSlash":
                    ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(invitem.FindGameObjectInChildren("Art Dash").GetComponent<InvItemDisplay>().activeSprite), invitem.FindGameObjectInChildren("Art Dash").GetComponent<InvItemDisplay>().activeSprite.pixelsPerUnit);
                    break;
                case "DreamGate":
                    ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(invitem.FindGameObjectInChildren("Dream Gate").GetComponent<InvItemDisplay>().activeSprite), invitem.FindGameObjectInChildren("Dream Gate").GetComponent<InvItemDisplay>().activeSprite.pixelsPerUnit);
                    break;
                case "InvGeo":
                    ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(invitem.FindGameObjectInChildren("Geo").GetComponent<SpriteRenderer>().sprite), invitem.FindGameObjectInChildren("Geo").GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
                    break;
                default:
                    break;

            }
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject invitem = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Inv_Items");
            switch (ItemName)
            {
                case "DSlash":
                    invitem.FindGameObjectInChildren("Art Uppercut").GetComponent<InvItemDisplay>().activeSprite=CreateSpritewithppu(sprite.texture,ckTex.defaultSprite.pixelsPerUnit);
                    break;
                case "CSlash":
                    invitem.FindGameObjectInChildren("Art Cyclone").GetComponent<InvItemDisplay>().activeSprite= CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
                    break;
                case "GSlash":
                    invitem.FindGameObjectInChildren("Art Dash").GetComponent<InvItemDisplay>().activeSprite= CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
                    break;
                case "DreamGate":
                    invitem.FindGameObjectInChildren("Dream Gate").GetComponent<InvItemDisplay>().activeSprite= CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
                    break;
                case "InvGeo":
                    invitem.FindGameObjectInChildren("Geo").GetComponent<SpriteRenderer>().sprite= CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
                    break;
                default:
                    break;

            }
        }
    }
}
