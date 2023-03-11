using System.IO;
using static Satchel.IoUtils;
namespace CustomKnight
{

    /// <summary>
    ///     The Interface that a Class must implement to be treated as a skin.
    /// </summary>
    public interface ISelectableSkin{
        /// <summary>
        ///  GetId
        /// </summary>
        /// <returns>The unique id of the skin as a <c>string</c></returns>
        public string GetId();
        
        /// <summary>
        ///  GetName
        /// </summary>
        /// <returns>The Name to be displayed in the menu as a <c>string</c></returns>
        public string GetName(); 
        
        /// <summary>
        ///  shouldCache
        /// </summary>
        /// <returns>A <c>bool</c> representing if the texture can be cached in memory or not.</returns>
        public bool shouldCache();
        
        /// <summary>
        ///  hasSwapper
        /// </summary>
        /// <returns>A <c>bool</c> representing if the skin has support for swapper files.</returns>
        public bool hasSwapper();
        
        /// <summary>
        ///  getSwapperPath
        /// </summary>
        /// <returns>A <c>string</c> representing the absolute path to the directory containing the swap folder.</returns>
        public string getSwapperPath();
        
        /// <summary>
        ///  Exists
        /// </summary>
        /// <param name="FileName">A <c>string</c> that identifies the file</param>
        /// <returns>A <c>bool</c> representing if the file exists in this skin.</returns>
        public bool Exists(string FileName);

 
        
        /// <summary>
        ///  GetTexture
        /// </summary>
        /// <param name="FileName">A <c>string</c> that identifies the texture file</param>
        /// <returns>A <c>Texture2D</c>, the texture for that filename.</returns>
        public Texture2D GetTexture(string FileName);   

        /// <summary>
        ///  GetFile reads an arbitrary file from the skin
        /// </summary>
        /// <param name="FileName">A <c>string</c> that identifies the file</param>
        /// <returns>A <c>byte[]</c>, the contents of that filename.</returns>
        public byte[] GetFile(string FileName);


        /// <summary>
        ///  HasCinematic returns if a given ciematic exists
        /// </summary>
        /// <param name="CinematicName">A <c>string</c> that identifies the Cinematic</param>
        /// <returns>A <c>bool</c>, representing  if a given ciematic exists</returns>
        public bool HasCinematic(string CinematicName);

        /// <summary>
        ///  GetCinematicUrl returns the Url for the given Cinematic
        /// </summary>
        /// <param name="CinematicName">A <c>string</c> that identifies the Cinematic</param>
        /// <returns>A <c>string</c>, the Url for the given Cinematic (a file://  url for local files).</returns>
        public string GetCinematicUrl(string CinematicName);

    }

    
    /// <summary>
    ///     The Class that represents all static skins that CustomKnight manages.
    /// </summary>
    public class StaticSkin : ISelectableSkin{
        public string SkinDirectory = "";
        public StaticSkin(string DirectoryName) {
            SkinDirectory = DirectoryName;
        }
        public bool shouldCache() => true;
        public string GetId() => SkinDirectory;
        public string GetName() => SkinDirectory;
        public bool hasSwapper() => true;
        public string getSwapperPath() => Path.Combine(SkinManager.SKINS_FOLDER,SkinDirectory);

        private Dictionary<string,string> CinematicFileUrlCache = new();
        public bool Exists(string FileName){
            string file = ($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/{FileName}").Replace("\\", "/");
            return File.Exists(file);
        }
        public Texture2D GetTexture(string FileName){
            Texture2D texture = null;
            try{
                string file = ($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/{FileName}").Replace("\\", "/");
                byte[] texBytes = File.ReadAllBytes(file);
                texture = new Texture2D(2, 2);
                texture.LoadImage(texBytes);
            } catch(Exception e){
                CustomKnight.Instance.Log(e.ToString());
            }
            return texture;
        }
        public byte[] GetFile(string FileName){
            byte[] data = null;
            try{
                string file = ($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/{FileName}").Replace("\\", "/");
                data = File.ReadAllBytes(file);
            } catch(Exception e){
                CustomKnight.Instance.Log(e.ToString());
            }
            return data;
        }

        public bool HasCinematic(string CinematicName){
            if(CinematicFileUrlCache.TryGetValue(CinematicName,out var url)){
                return url.Length > 0;
            } else {
                EnsureDirectory($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/Cinematics/");
                string file = ($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/Cinematics/{CinematicName}").Replace("\\", "/");
                CinematicFileUrlCache[CinematicName] = GetCinematicUrl(CinematicName);
                return CinematicFileUrlCache[CinematicName].Length > 0;
            }
            
        }

        public string GetCinematicUrl(string CinematicName){
            string path = "";
            string file = ($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/Cinematics/{CinematicName}").Replace("\\", "/");
            if(File.Exists(file+".webm")){
                path = file+".webm";
            }
            CustomKnight.Instance.LogFine("[GetCinematicUrl]" + CinematicName+":"+path);
            return path;
        }
        
    }
    
}