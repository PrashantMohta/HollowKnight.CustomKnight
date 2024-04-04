
namespace CustomKnight
{
    internal class SpecialCases
    {
        internal static bool childSpriteAnimatedByParent(string path)
        {
            if (path.Contains("boss control") && path.Contains("shade_lord"))
            {
                CustomKnight.Instance.Log("Matched Special case : " + path);
                return true;
            }
            return false;
        }
    }
}