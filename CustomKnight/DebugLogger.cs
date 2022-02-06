
namespace CustomKnight
{   
    public static class DebugLogger{
        public static bool enabled = false; 

        internal static void Log(string s){
            if(enabled){
                CustomKnight.Instance.Log(s);
            }
        }
    }
}