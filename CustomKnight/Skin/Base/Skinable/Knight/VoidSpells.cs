namespace CustomKnight
{
    public class VoidSpells : Skinable_noCache
    {
        public static string NAME = "VoidSpells";
        public VoidSpells() : base(NAME) { }

        public override Material GetMaterial()
        {
            Material VoidSpells = null;
            foreach (Transform child in HeroController.instance.gameObject.transform)
            {
                if (child.name == "Spells")
                {
                    foreach (Transform spellsChild in child)
                    {
                        if (spellsChild.name == "Scr Heads 2" || spellsChild.name == "Scr Base 2")
                        {
                            VoidSpells = spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
                        }
                    }
                }
            }
            return VoidSpells;
        }

    }
}