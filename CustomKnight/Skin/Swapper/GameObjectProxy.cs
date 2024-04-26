using System.IO;

namespace CustomKnight
{
    /// <summary>
    /// The proxy GameObject that is used to create the GO hierarchy that the swaps must be applied to
    /// </summary>
    public class GameObjectProxy
    {
        /// <summary>
        /// Name/path the file
        /// </summary>
        public string name;

        /// <summary>
        /// alias of the file
        /// </summary>
        public string alias = "spl_ck_dndy";

        /// <summary>
        /// root path of the file (name of the folder under swap)
        /// </summary>
        public string rootPath; //rootPath/name.png

        /// <summary>
        /// if the GOP has texture
        /// </summary>
        public bool hasTexture;

        /// <summary>
        /// if the GOP has Children
        /// </summary>
        public bool hasChildren;

        /// <summary>
        /// Type of the file that is detected
        /// </summary>
        public string fileType;

        /// <summary>
        /// Direct Childern of this GOP
        /// </summary>
        public Dictionary<string, GameObjectProxy> children = new Dictionary<string, GameObjectProxy>();

        /// <summary>
        /// Get the file Path
        /// </summary>
        /// <returns></returns>
        public string getObjectPath()
        {
            return Path.Combine(rootPath, name + fileType);
        }

        /// <summary>
        /// get the texture path
        /// </summary>
        /// <returns></returns>
        public string getTexturePath()
        {
            return Path.Combine(rootPath, name + ".png");
        }
        /// <summary>
        /// Get the alias Path
        /// </summary>
        /// <returns></returns>
        public string getAliasPath()
        {
            if (alias != "spl_ck_dndy")
            {
                CustomKnight.Instance.LogDebug(alias);
            }
            return Path.Combine(rootPath, alias + ".png");
        }
        /// <summary>
        /// Create the hierarchy defined by the given path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="rootPath"></param>
        /// <param name="name"></param>
        /// <param name="alias"></param>
        public void TraverseGameObjectPath(string path, string rootPath, string name, string alias = "spl_ck_dndy")
        {
            CustomKnight.Instance.LogDebug($"{path}:{rootPath}:{name}");
            var pathSplit = path.Split(new Char[] { '/' }, 3);
            GameObjectProxy GOP = null;
            hasChildren = false;
            if (pathSplit.Length > 1)
            {
                hasChildren = true;
                if (!children.TryGetValue(pathSplit[1], out GOP))
                {
                    GOP = new GameObjectProxy()
                    {
                        name = pathSplit[1],
                        hasTexture = false,
                    };
                }
                children[pathSplit[1]] = GOP;
            }
            if (GOP != null)
            {
                if (pathSplit.Length > 2)
                {
                    GOP.TraverseGameObjectPath($"{pathSplit[1]}/{pathSplit[2]}", rootPath, name, alias);
                }
                else
                {
                    if (!GOP.hasTexture)
                    { // do not over ride existing texture
                        GOP.hasTexture = true;
                        GOP.rootPath = rootPath;
                        GOP.name = name;
                        GOP.alias = alias;
                    }
                }
            }
            else
            {
                if (!this.hasTexture)
                {
                    this.hasTexture = true;
                    this.rootPath = rootPath;
                    this.name = name;
                    this.alias = alias;
                }
            }

            CustomKnight.Instance.LogDebug($"{this.hasTexture}:{this.rootPath}:{this.name}:{this.alias}:{(this.rootPath == null ? "null root" : "x")}");
        }

        /// <summary>
        /// Create the hierarchy based on detecting files and directories at a particular path
        /// </summary>
        /// <param name="basePath"></param>
        public void TraverseGameObjectDirectory(string basePath)
        {
            var path = Path.Combine(basePath, Path.Combine(rootPath, name));
            if (!Directory.Exists(path))
            {
                hasChildren = false;
                return;
            }
            // check if it has files
            foreach (string file in Directory.GetFiles(path))
            {
                string filename = Path.GetFileName(file);
                //Log(filename);
                if (!filename.EndsWith(".txt"))
                {
                    string extension = Path.GetExtension(file);
                    string objectName = filename.Replace(extension, "");
                    GameObjectProxy GOP = new GameObjectProxy()
                    {
                        name = objectName,
                        hasTexture = true,
                        rootPath = Path.Combine(rootPath, name),
                        hasChildren = false,
                        fileType = extension
                    };
                    hasChildren = true;
                    children[objectName] = GOP;
                }
            }
            // check if it has directories
            foreach (string directory in Directory.GetDirectories(path))
            {
                string directoryName = new DirectoryInfo(directory).Name;
                //Log(directoryName);
                GameObjectProxy GOP;
                if (!children.TryGetValue(directoryName, out GOP))
                {
                    GOP = new GameObjectProxy()
                    {
                        name = directoryName,
                        hasTexture = false,
                        rootPath = Path.Combine(rootPath, name),
                        hasChildren = true
                    };
                }
                hasChildren = true;
                children[directoryName] = GOP;
                if (GOP.rootPath == "" || GOP.rootPath == null)
                {
                    GOP.rootPath = Path.Combine(rootPath, name);
                }
                GOP.TraverseGameObjectDirectory(basePath);
            }

        }
    }
}