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
            GameObject dreamnail = SkinManager.invitem.FindGameObjectInChildren("Dream Nail");
            if (nailname.EndsWith("0"))
            {
                ckTex.defaultSprite = dreamnail.GetComponent<InvItemDisplay>().inactiveSprite;
            }
            if(nailname.EndsWith("1"))
            {
                ckTex.defaultSprite = dreamnail.GetComponent<InvItemDisplay>().activeSprite;
            }
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject dreamnail = SkinManager.invitem.FindGameObjectInChildren("Dream Nail");
            if (nailname.EndsWith("0"))
            {
                dreamnail.GetComponent<InvItemDisplay>().inactiveSprite = sprite;
            }
            if (nailname.EndsWith("1"))
            {
                dreamnail.GetComponent<InvItemDisplay>().activeSprite = sprite;
            }
        }
    }
}
