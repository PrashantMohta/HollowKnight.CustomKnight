
namespace CustomKnight
{
    public class Heart : Skinable_Sprite
    {
        public string HeartName;
        public int HeartPieces;
        public Heart(string heartname, int pieces) : base(heartname)
        {
            HeartPieces = pieces;
            HeartName = heartname;
        }
        public override void SaveDefaultTexture()
        {
            GameObject heart = SkinManager.inv.FindGameObjectInChildren("Heart Pieces");
            ckTex.defaultSprite = HeartPieces switch
            {
                0 => heart.GetComponent<SpriteRenderer>().sprite,
                _ => heart.FindGameObjectInChildren($"Pieces {HeartPieces}").GetComponent<SpriteRenderer>().sprite,
            };
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject heart = SkinManager.inv.FindGameObjectInChildren("Heart Pieces");
            switch (HeartPieces)
            {
                case 0:
                    heart.GetComponent<SpriteRenderer>().sprite = sprite;
                    break;
                default:
                    heart.FindGameObjectInChildren($"Pieces {HeartPieces}").GetComponent<SpriteRenderer>().sprite = sprite;
                    break;
            }
        }
    }
}
