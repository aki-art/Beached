using System.IO;
using UnityEditor;
using UnityEngine;

public class ExportAssets : MonoBehaviour
{
    static string AssetsBundlePath = Path.Combine(Application.dataPath, "generated assets", "assetbundles");
    static string ExportPath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Application.dataPath)), "assets", "assetbundles");

    static string WIN = "windows";
    static string MAC = "mac";
    static string LINUX = "linux";

    [MenuItem("Export/Export to all platforms")]
    static void BuildAll()
    {
        BuildWindows();
        BuildLinux();
        BuildIOs();
    }

    [MenuItem("Export/Windows export")]
    static void BuildWindows() => BuildForPlatform(BuildTarget.StandaloneWindows64);

    static void BuildLinux() => BuildForPlatform(BuildTarget.StandaloneLinux64);

    static void BuildIOs() => BuildForPlatform(BuildTarget.StandaloneOSX);

    static void BuildForPlatform(BuildTarget target)
    {
        var id = "";
        switch (target)
        {
            case BuildTarget.StandaloneWindows64:
                id = WIN;
                break;
            case BuildTarget.StandaloneOSX:
                id = MAC;
                break;
            case BuildTarget.StandaloneLinux64:
                id = LINUX;
                break;
        }

        var path = Path.Combine(AssetsBundlePath, id);
        var exportPath = Path.Combine(ExportPath, id);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ChunkBasedCompression, target);
        SyncBundles(path, exportPath, id);
    }

    private static void SyncBundles(string sourcePath, string targetPath, string ignoreId)
    {
        if (Directory.Exists(sourcePath))
        {
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            var source = new DirectoryInfo(sourcePath);
            var target = new DirectoryInfo(targetPath);

            foreach (FileInfo fi in source.GetFiles())
            {
                if (fi.Extension == "" && fi.Name != ignoreId)
                {
                    Debug.Log(string.Format("Copying {0}/{1}", target.FullName, fi.Name));
                    fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                }
            }

            return;
        }
    }
}
