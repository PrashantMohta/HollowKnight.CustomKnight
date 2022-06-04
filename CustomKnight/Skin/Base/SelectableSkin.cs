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
        ///  hasInv
        /// </summary>
        /// <returns>A <c>bool</c> representing if the skin has support for inventory files.</returns>
        public bool hasInv();
        
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
        public bool hasInv() => DirectoryExists($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/Inventory");
        public string getSwapperPath() => Path.Combine(SkinManager.SKINS_FOLDER,SkinDirectory);

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
        
    }
    
}