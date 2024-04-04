using Newtonsoft.Json;
using System.IO;

namespace CustomKnight
{
    internal class CacheObj
    {
        internal Dictionary<string, string> PathToHash = new();
        public Dictionary<string, List<string>> HashToPaths = new();
    }
    internal class AliasObj
    {
        internal Dictionary<string, string> HashToAlias = new();
        public Dictionary<string, string> AliasToHash = new();
    }
    internal static class HashWithCache
    {
        private static CacheObj cache = new CacheObj();
        private static bool updated = false;
        private static AliasObj aliases = new AliasObj();

        internal static List<string> GetPathsFromHash(string hashOrAlias)
        {
            if (aliases.AliasToHash.TryGetValue(hashOrAlias, out var hash) && cache.HashToPaths.TryGetValue(hash, out var paths))
            {
                Modding.Logger.Log(hash + "///" + hashOrAlias);
                return paths;
            }
            if (cache.HashToPaths.TryGetValue(hashOrAlias, out var paths2))
            {
                return paths2;
            }
            return null;
        }

        internal static string GetAliasFromHash(string hash)
        {
            if (aliases.HashToAlias.TryGetValue(hash, out var alias))
            {
                return alias;
            }
            return null;
        }
        internal static string GetHashFromPath(string path)
        {
            if (cache.PathToHash.TryGetValue(path, out var hash))
            {
                return hash;
            }
            return hash;
        }
        static HashWithCache()
        {
            loadCacheObjFromFile();
            //saveAliasObjToFile();
            loadCacheAliasesFromFile();
            foreach (var kvp in cache.HashToPaths)
            {
                foreach (var path in kvp.Value)
                {
                    cache.PathToHash[path] = kvp.Key;
                }
            }
            foreach (var kvp in aliases.AliasToHash)
            {
                aliases.HashToAlias[kvp.Value] = kvp.Key;
            }
        }

        private static void AddHashToPath(string hash, string path)
        {
            List<string> pathList;
            if (!cache.HashToPaths.TryGetValue(hash, out pathList))
            {
                pathList = new List<string>();
            }
            pathList.Add(path);
            cache.HashToPaths[hash] = pathList;
            cache.PathToHash[path] = hash;
        }

        private static void loadCacheObjFromFile()
        {
            var path = Path.Combine(AssemblyUtils.getCurrentDirectory(), "hashcache.json");
            if (File.Exists(path))
            {
                cache = JsonConvert.DeserializeObject<CacheObj>(File.ReadAllText(path), new JsonSerializerSettings() { ObjectCreationHandling = ObjectCreationHandling.Replace });
            }
        }

        private static void loadCacheAliasesFromFile()
        {
            var path = Path.Combine(AssemblyUtils.getCurrentDirectory(), "aliases.json");
            if (File.Exists(path))
            {
                aliases = JsonConvert.DeserializeObject<AliasObj>(File.ReadAllText(path), new JsonSerializerSettings() { ObjectCreationHandling = ObjectCreationHandling.Replace });
            }
        }
        private static void saveAliasObjToFile()
        {
            var path = Path.Combine(AssemblyUtils.getCurrentDirectory(), "aliases.json");
            var json = JsonConvert.SerializeObject(aliases, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        private static void saveCacheObjToFile()
        {
            var path = Path.Combine(AssemblyUtils.getCurrentDirectory(), "hashcache.json");
            var json = JsonConvert.SerializeObject(cache, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        internal static void saveIfUpdated()
        {
            if (updated)
            {
                saveCacheObjToFile();
            }
            updated = false;
        }
        private static string getTk2dSpriteHashInternal(tk2dSprite tk)
        {
            var mat = tk?.GetCurrentSpriteDef()?.material;
            if (mat == null) { return ""; }
            var tex = mat.mainTexture;
            var dupe = tex.isReadable ? (Texture2D)tex : TextureUtils.duplicateTexture((Texture2D)mat.mainTexture);
            var hash = dupe.getHash();
            if (!tex.isReadable)
            {
                GameObject.Destroy(dupe);
            }
            return hash;
        }
        internal static string getTk2dSpriteHash(tk2dSprite tk)
        {
            var path = tk.gameObject.scene.name + "/" + tk.gameObject.GetPath(true);
            string hash;
            if (!cache.PathToHash.TryGetValue(path, out hash))
            {
                hash = getTk2dSpriteHashInternal(tk);
                AddHashToPath(hash, path);
                updated = true;
            }
            return hash;
        }

    }
}