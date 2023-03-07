using System.IO;
namespace CustomKnight
{
    internal static class FixSkinStructure
    {
        
        internal static bool dirHasPng(string sourceDirectory, SearchOption op){
           return IoUtils.DirectoryHas(sourceDirectory, "*.png", op);
        }

        internal static void getPngsToRoot(string currentPath, string root){
            try {
                List<string> queue = new List<string>();
                string[] dirs = Directory.GetDirectories(currentPath);
                foreach (string dir in dirs){
                    CustomKnight.Instance.Log("Looking in" + dir + "|" + Path.GetDirectoryName(dir));

                    if (Path.GetFileName(dir) == "Swap")
                    {
                        CustomKnight.Instance.Log("Found Swap Directory");
                        IoUtils.DirectoryCopyAllFiles(dir, Path.Combine(root, "Swap"), true, true);
                    }
                    else if(Path.GetFileName(dir) == "Cinematics")
                    {
                        CustomKnight.Instance.Log("Found Cinematics Directory");
                        IoUtils.DirectoryCopyAllFiles(dir, Path.Combine(root, "Cinematics"), true, true);
                    } else if (dirHasPng(dir,SearchOption.TopDirectoryOnly)){
                        IoUtils.DirectoryCopyAllFiles(dir,root);
                    } else if(dirHasPng(dir,SearchOption.AllDirectories)){
                        queue.Add(dir);
                    }
                }
                foreach(string dir in queue){
                    getPngsToRoot(dir,root);
                }
            } catch (Exception e) {
                CustomKnight.Instance.Log("The Skin could not be fixed : " + e.ToString());
            }
        }
        
        internal static void FixSkins(){
            try{
                string[] skinDirectories = Directory.GetDirectories(SkinManager.SKINS_FOLDER);
                foreach (string dir in skinDirectories)
                {
                    if(!dirHasPng(dir,SearchOption.TopDirectoryOnly) && dirHasPng(dir,SearchOption.AllDirectories)){
                        CustomKnight.Instance.Log("A broken skin found! " + dir);
                        getPngsToRoot(dir,dir);
                    }
                }
            } catch (Exception e) {
                CustomKnight.Instance.Log("Failed to fix : "+ e.ToString());
            }

        }


    }
}