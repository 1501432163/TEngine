﻿using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TEngine
{
    public static class FileSystem
    {
        public const string ArtResourcePath = "Assets/ArtResources";
        public const string GameResourcePath = AssetConfig.AssetRootPath;
        internal static Dictionary<string, string> _fileFixList = new Dictionary<string, string>();
        private static string _persistentDataPath = null;
        private static string _resRootPath = null;
        private static string _resRootStreamAssetPath = null;
        public const string BuildPath = "Build";
        public const string AssetBundleBuildPath = BuildPath + "/AssetBundles";
        private const string AssetBundleTargetPath = "{0}/AssetBundles";
        /// <summary>
        /// 资源更新读取根目录
        /// </summary>
        /// <returns></returns>
        public static string ResourceRoot
        {
            get
            {
                if (string.IsNullOrEmpty(_resRootPath))
                {
                    _resRootPath = Path.Combine(PersistentDataPath, "TEngine");
                }

                if (!Directory.Exists(_resRootPath))
                {
                    Directory.CreateDirectory(_resRootPath);
                }

                return _resRootPath.FixPath();
            }
        }

        /// <summary>
        /// 持久化数据存储路径
        /// </summary>
        public static string PersistentDataPath
        {
            get
            {
                if (string.IsNullOrEmpty(_persistentDataPath))
                {
#if UNITY_EDITOR_WIN
                    _persistentDataPath = Application.dataPath + "/../TEnginePersistentDataPath";
                    if (!Directory.Exists(_persistentDataPath))
                    {
                        Directory.CreateDirectory(_persistentDataPath);
                    }
#else
                    _persistentDataPath = Application.persistentDataPath;
#endif
                }
                return _persistentDataPath.FixPath();
            }
        }

        /// <summary>
        /// 资源更新读取StreamAsset根目录
        /// </summary>
        /// <returns></returns>
        public static string ResourceRootInStreamAsset
        {
            get
            {
                if (string.IsNullOrEmpty(_resRootStreamAssetPath))
                {
                    _resRootStreamAssetPath = Path.Combine(Application.streamingAssetsPath, "TEngine");
                }
                return _resRootStreamAssetPath.FixPath();
            }
        }
        public static string GetAssetBundlePathInVersion(string bundlename)
        {
            //默认用外部目录
            string path = FilePath($"{ResourceRoot}/AssetBundles/{bundlename}");
            if (!File.Exists(path))
            {
                path = $"{ResourceRootInStreamAsset}/AssetBundles/{bundlename}";
            }

            return path;
        }

        public static string StreamAssetBundlePath
        {
            get { return string.Format(AssetBundleTargetPath, ResourceRootInStreamAsset); }
        }

        internal static string FilePath(string path)
        {
            path = path.FixPath();
#if UNITY_EDITOR_WIN
            path = path.Replace("Assets/../", "");
#endif
            if (_fileFixList.ContainsKey(path))
            {
                return _fileFixList[path];
            }
            else
            {
                return path;
            }
        }

        public static string FixPath(this string str)
        {
            str = str.Replace("\\", "/");
            return str;
        }

//        public static Stream OpenRead(string filePath)
//        {
//#if UNITY_ANDROID && !UNITY_EDITOR
//            byte[] bytes = ReadAllBytesFromOutOrInnerFolder(filePath);
//            if (bytes != null)
//                return new MemoryStream(bytes);
//            else
//                return null;
//#else
//            return File.OpenRead(filePath);
//#endif
//        }
    }
}