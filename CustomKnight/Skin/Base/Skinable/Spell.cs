
namespace CustomKnight
{
    public class InvSpell:Skinable_Sprite
    {
        public string SpellName;
        public int SpellLevel;
        public InvSpell(string name,int level) : base(name)
        {
            SpellName = name;
            SpellLevel = level;
        }
        public override void SaveDefaultTexture()
        {
            if(SpellLevel>0)
            {
                
                string basename = SpellName.Substring(0, SpellName.Length - 2);
                GameObject spell = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Inv_Items").FindGameObjectInChildren($"Spell {basename}");
                PlayMakerFSM checkfsm = spell.LocateMyFSM("Check Active");
                Sprite sprite = checkfsm.GetAction<SetSpriteRendererSprite>($"Lv {SpellLevel}", 0).sprite.Value as Sprite;
                ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(sprite),sprite.pixelsPerUnit) ;
            }
            else
            {
                GameObject spell = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Inv_Items").FindGameObjectInChildren($"Spell {SpellName}");
                ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(spell.GetComponent<SpriteRenderer>().sprite), spell.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
            }
        }
        public override void ApplySprite(Sprite sprite)
        {
           if(SpellLevel>0)
            {
                string basename = SpellName.Substring(0, SpellName.Length - 2);
                GameObject spell = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Inv_Items").FindGameObjectInChildren($"Spell {basename}");
                PlayMakerFSM checkfsm = spell.LocateMyFSM("Check Active");
                checkfsm.GetAction<SetSpriteRendererSprite>($"Lv {SpellLevel}", 0).sprite.Value = CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
            }
            else
            {
                GameObject spell = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Inv_Items").FindGameObjectInChildren($"Spell {SpellName}");
                spell.GetComponent<SpriteRenderer>().sprite= CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
            }

        }
    }
}
