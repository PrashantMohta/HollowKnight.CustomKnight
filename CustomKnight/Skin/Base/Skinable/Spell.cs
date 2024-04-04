
namespace CustomKnight
{
    public class InvSpell : Skinable_Sprite
    {
        public string SpellName;
        public int SpellLevel;
        public InvSpell(string name, int level) : base(name)
        {
            SpellName = name;
            SpellLevel = level;
        }
        public override void SaveDefaultTexture()
        {
            if (SpellLevel > 0)
            {

                string basename = SpellName.Substring(10, SpellName.Length - 12);
                GameObject spell = SkinManager.invitem.FindGameObjectInChildren($"Spell {basename}");
                PlayMakerFSM checkfsm = spell.LocateMyFSM("Check Active");
                Sprite sprite = checkfsm.GetAction<SetSpriteRendererSprite>($"Lv {SpellLevel}", 0).sprite.Value as Sprite;
                ckTex.defaultSprite = sprite;
            }
            else
            {
                string basename = SpellName.Substring(10, SpellName.Length - 10);
                GameObject spell = SkinManager.invitem.FindGameObjectInChildren($"Spell {basename}");
                ckTex.defaultSprite = spell.GetComponent<SpriteRenderer>().sprite;
            }
        }
        public override void ApplySprite(Sprite sprite)
        {
            if (SpellLevel > 0)
            {
                string basename = SpellName.Substring(10, SpellName.Length - 12);
                GameObject spell = SkinManager.invitem.FindGameObjectInChildren($"Spell {basename}");
                PlayMakerFSM checkfsm = spell.LocateMyFSM("Check Active");
                checkfsm.GetAction<SetSpriteRendererSprite>($"Lv {SpellLevel}", 0).sprite.Value = sprite;
            }
            else
            {
                string basename = SpellName.Substring(10, SpellName.Length - 10);
                GameObject spell = SkinManager.invitem.FindGameObjectInChildren($"Spell {basename}");
                spell.GetComponent<SpriteRenderer>().sprite = sprite;
            }

        }
    }
}
