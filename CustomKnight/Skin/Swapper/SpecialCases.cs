
namespace CustomKnight
{
    internal class SpecialCases
    {
        internal static bool ChildSpriteAnimatedByParent(string path)
        {
            if ((path.Contains("boss control") && path.Contains("shade_lord")) || (path.Contains("Boss Control") && path.Contains("Shade_Lord")))
            {
                CustomKnight.Instance.Log("Matched Special case : " + path);
                return true;
            }
            return false;
        }

        internal static bool AllowedDontDestroyOnLoad(string goPath)
        {
            if (!CustomKnight.GlobalSettings.EnableParticleSwap)
            {
                return false;
            }
            return goPath.Contains("_GameManager") || goPath.Contains("_GameCameras") || goPath.Contains("CameraParent"); // || goPath.Contains("_UIManager")
        }
    }
}