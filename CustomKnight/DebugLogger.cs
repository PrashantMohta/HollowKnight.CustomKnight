
namespace CustomKnight
{   
    public static class DebugLogger{
        private static bool enabled = true; // should be disabled in a future release

        internal static void Log(string s){
            if(enabled){
                CustomKnight.Instance.Log(s);
            }
        }
    }
}