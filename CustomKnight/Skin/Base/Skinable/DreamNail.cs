namespace CustomKnight
{
    public class DreamNail:Skinable_Sprite
    {
        public string nailname;
        public DreamNail(string name) : base(name)
        {
            nailname = name;
        }
        
        public override void SaveDefaultTexture()
        {
            GameObject dreamnail = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Inv_Items").FindGameObjectInChildren("Dream Nail");
            if (nailname.EndsWith("0"))
            {
                ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(dreamnail.GetComponent<InvItemDisplay>().inactiveSprite), dreamnail.GetComponent<InvItemDisplay>().inactiveSprite.pixelsPerUnit);
            }
            if(nailname.EndsWith("1"))
            {
                ckTex.defaultSprite= CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(dreamnail.GetComponent<InvItemDisplay>().activeSprite), dreamnail.GetComponent<InvItemDisplay>().activeSprite.pixelsPerUnit);
            }
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject dreamnail = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Inv_Items").FindGameObjectInChildren("Dream Nail");
            if (nailname.EndsWith("0"))
            {
                dreamnail.GetComponent<InvItemDisplay>().inactiveSprite=CreateSpritewithppu(sprite.texture,dreamnail.GetComponent<InvItemDisplay>().inactiveSprite.pixelsPerUnit);
            }
            if (nailname.EndsWith("1"))
            {
               dreamnail.GetComponent<InvItemDisplay>().activeSprite= CreateSpritewithppu(sprite.texture, dreamnail.GetComponent<InvItemDisplay>().activeSprite.pixelsPerUnit); ;
            }
        }
    }
}
