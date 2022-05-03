namespace CustomKnight
{
    public class PinsScarab : Skinable_Sprite
    {
        public static string NAME = "PinsScarab";
        public PinsScarab() : base(PinsScarab.NAME){}

        public override void SaveDefaultTexture(){
            try{
                GameMap Map = GameManager.instance.gameMap?.GetComponent<GameMap>();
                ckTex.defaultSprite = Map.mapMarkersBlue[0].GetComponent<SpriteRenderer>().sprite;
            } catch(Exception e){
                CustomKnight.Instance.Log($"skinable {name} : {e}");
            }
        }
        public override void ApplySprite(Sprite sprite){
            GameMap Map = GameManager.instance.gameMap?.GetComponent<GameMap>();
            foreach(var pin in Map.mapMarkersBlue){
                var anim = pin.GetComponent<Animator>();
                if(anim != null){
                    GameObject.Destroy(anim);
                }
                pin.GetComponent<SpriteRenderer>().sprite = sprite;
            }
        }

    }
}