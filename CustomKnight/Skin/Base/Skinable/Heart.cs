
namespace CustomKnight
{
   public class Heart:Skinable_Sprite
    {
        public string HeartName;
        public int HeartPieces;
        public Heart(string heartname,int pieces) : base(heartname)
        {
            HeartPieces = pieces;
            HeartName = heartname;
        }
        public override void SaveDefaultTexture()
        {
            GameObject heart = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Heart Pieces");
            switch(HeartPieces)
            {
                case 0:
                    ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(heart.GetComponent<SpriteRenderer>().sprite), heart.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit); ;
                    break;
                default:
                    ckTex.defaultSprite=heart.FindGameObjectInChildren($"Pieces {HeartPieces}").GetComponent<SpriteRenderer>().sprite;
                    break;
            }
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject heart = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Heart Pieces");
            switch (HeartPieces)
            {
                case 0:
                    CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
                    break;
                default:
                    heart.FindGameObjectInChildren($"Pieces {HeartPieces}").GetComponent<SpriteRenderer>().sprite = CreateSpritewithppu(sprite.texture, heart.FindGameObjectInChildren($"Pieces {HeartPieces}").GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
                    break;
            }
        }
    }
}
