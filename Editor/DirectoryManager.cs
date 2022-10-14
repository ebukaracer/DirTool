using System;
using System.IO;
using UnityEngine;

namespace Racer.DirTool
{
    internal static class DirectoryManager
    {
        public static void CreateDirectory(string rootDir, string subDir)
        {
            try
            {
                if (!GetValidPath(rootDir, subDir, out var subPathNames))
                    return;

                var rootPath = Path.Combine(Application.dataPath, rootDir.Trim(' '));

                foreach (var subPathName in subPathNames)
                {
                    if (string.IsNullOrWhiteSpace(subPathName))
                        continue;

                    var subPaths = Path.Combine(rootPath, subPathName.Trim(' '));

                    if (Directory.Exists(subPaths))
                        continue;

                    Directory.CreateDirectory(subPaths);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }

        public static void DeleteDirectory(string rootDir, string subDir)
        {
            try
            {
                if (!GetValidPath(rootDir, subDir, out var subPathNames))
                    return;

                var rootPath = Path.Combine(Application.dataPath, rootDir.Trim(' '));

                foreach (var subPathName in subPathNames)
                {
                    if (string.IsNullOrWhiteSpace(subPathName))
                        continue;

                    var subPaths = Path.Combine(rootPath, subPathName.Trim(' '));
                    var subPathsMeta = subPaths + ".meta";

                    if (!Directory.Exists(subPaths)) continue;

                    Directory.Delete(subPaths, true);
                    File.Delete(subPathsMeta);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }

        private static bool GetValidPath(string rootDir, string subDir, out string[] subPathNames)
        {
            if (string.IsNullOrWhiteSpace(rootDir) || string.IsNullOrWhiteSpace(subDir))
            {
                Debug.LogWarning($"{nameof(rootDir)} or {nameof(subDir)} cannot be Empty!");

                subPathNames = null;

                return false;
            }

            subPathNames = subDir.Split(',');

            return true;
        }
    }
}