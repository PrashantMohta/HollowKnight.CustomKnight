using Modding;

namespace CustomKnight.Skin.Swapper
{
    public class PreloadedObjectData : MonoBehaviour
    {
        public string goPath;
        public string origScene;

        public string Hash
        {
            get
            {
                return HashWithCache.GetHashFromPath($"{origScene}/{goPath.ToLower()}");
            }
        }

        void Awake()
        {
            if (CustomKnight.swapManager.Scenes.TryGetValue("Global", out var CurrentSceneDict))
            {
                if (!string.IsNullOrEmpty(Hash) && CurrentSceneDict.TryGetValue(Hash, out GameObjectProxy gop))
                {
                    CustomKnight.swapManager.applySkinsUsingProxy(gop, this.gameObject);
                }
            }
        }
    }
}
