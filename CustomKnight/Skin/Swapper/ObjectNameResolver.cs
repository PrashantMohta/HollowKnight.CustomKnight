using System.IO;
using System.Linq;
using System.Security.Policy;
using static Satchel.IoUtils;

namespace CustomKnight.Skin.Swapper
{
    internal static class ObjectNameResolver
    {
        internal static string DUMP_DIR = Path.Combine(SkinManager.DATA_DIR, "Dump");
        internal static Dictionary<string, Dictionary<string, List<string>>> NameDb = new();
        internal static SettingsLoader<Dictionary<string, List<string>>> NamesLoader = new();
        internal static Dictionary<string, string> PathToHash = new();
        internal static void EnsureNameDb(string sceneName, string hash)
        {
            if (NameDb == null)
            {
                NameDb = new Dictionary<string, Dictionary<string, List<string>>>();
            }
            if (!NameDb.TryGetValue(sceneName, out var SceneData))
            {
                NameDb[sceneName] = new Dictionary<string, List<string>>();
            }
            if (!NameDb[sceneName].TryGetValue(hash, out var objectPaths))
            {
                NameDb[sceneName][hash] = new List<string>();
            }
        }
        internal static bool Contains(string sceneName, string objectPath)
        {
            return GetPathsForScene(sceneName).Contains(objectPath);
        }
        internal static void Add(string sceneName, string objectPath, string hash)
        {
            EnsureNameDb(sceneName, hash);
            if (!NameDb[sceneName][hash].Contains(objectPath))
            {
                NameDb[sceneName][hash].Add(objectPath);
                DumpNameDb(sceneName);
            }
        }

        internal static void Clear()
        {
            NameDb = new();
            PathToHash = new();
        }
        internal static List<string> GetPathAsList(this GameObject go, bool useBaseName = false)
        {
            Transform rootGoT = go.transform;
            List<string> path = [rootGoT.gameObject.GetName(useBaseName)];
            while (rootGoT.parent != null)
            {
                rootGoT = rootGoT.parent;
                path.Add(rootGoT.gameObject.GetName(useBaseName));
            }
            path.Reverse();
            return path;
        }
        internal static void LoadNameDb(string path, string sceneName)
        {
            if (!File.Exists(path)) { return; }
            var newData = NamesLoader.Load(path);
            if (!NameDb.ContainsKey(sceneName))
            {
                NameDb[sceneName] = newData;
            }
            foreach (var kvp in newData)
            {
                EnsureNameDb(sceneName, kvp.Key);
                foreach (var goPath in kvp.Value)
                {
                    Add(sceneName, goPath, kvp.Key);
                }
                foreach (var kv in kvp.Value)
                {
                    PathToHash[sceneName + "/" + kv] = kvp.Key;
                }
            }
        }
        internal static void DumpNameDb(string sceneName)
        {
            string scenePath = Path.Combine(DUMP_DIR, sceneName);
            EnsureDirectory(DUMP_DIR);
            EnsureDirectory(scenePath);
            string outpath = Path.Combine(scenePath, "names.json");
            NamesLoader.Save(outpath, NameDb[sceneName]);
        }
        internal static void DumpNameDb()
        {
            foreach (var sceneName in NameDb.Keys)
            {
                DumpNameDb(sceneName);
            }
        }

        internal static List<string> GetScenes()
        {
            return NameDb.Keys.ToList();
        }

        internal static List<string> GetPathsForScene(string scn)
        {
            List<string> paths = new List<string>();
            foreach (var hash in NameDb[scn])
            {
                paths.AddRange(hash.Value);
            }
            return paths;
        }

        internal static string GetHashFromPath(string hashPath)
        {
            if (PathToHash.ContainsKey(hashPath))
            {
                return PathToHash[hashPath];
            }
            else return "spl_ck_dndy";
        }
    }
}
