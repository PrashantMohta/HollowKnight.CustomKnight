
namespace CustomKnight
{   
    public static class DebugLogger{
        private static bool enabled = true; // should be disabled in a future release

        public static void Log(string s){
            if(enabled){
                CustomKnight.Instance.Log(s);
            }
        }
    }
}