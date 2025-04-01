using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Luban;
using UnityEditor;
using UnityEngine.Windows;
using YooAsset;

public class Game : MonoBehaviour
{
    public List<int> _a = new List<int>();

    IEnumerator Start()
    {
       YooAssets.Initialize();

       var package = YooAssets.CreatePackage("DefaultPackage");
       YooAssets.SetDefaultPackage(package);

       yield return InitPackage();
       yield return RequestPackageVersion();
       yield return UpdatePackageManifest();

        var tables = new cfg.Tables(LoadByteBuf);

        foreach (var hero in tables.TbHero.DataList)
            Debug.Log($">> {hero}");

    }

    private string _package_version = "";

    private IEnumerator InitPackage()
    {
        var buildResult = EditorSimulateModeHelper.SimulateBuild("DefaultPackage");
        var packageRoot = buildResult.PackageRootDirectory;
        var editorFileSystemParams = FileSystemParameters.CreateDefaultEditorFileSystemParameters(packageRoot);
        var initParameters = new EditorSimulateModeParameters();
        initParameters.EditorFileSystemParameters = editorFileSystemParams;
        var package = YooAssets.GetPackage("DefaultPackage");
        var initOperation = package.InitializeAsync(initParameters);
        yield return initOperation;

        if(initOperation.Status == EOperationStatus.Succeed)
            Debug.Log("资源包初始化成功！");
        else
            Debug.LogError($"资源包初始化失败：{initOperation.Error}");
    }

    private IEnumerator RequestPackageVersion()
    {
        var package = YooAssets.GetPackage("DefaultPackage");
        var operation = package.RequestPackageVersionAsync();
        yield return operation;

        if (operation.Status == EOperationStatus.Succeed)
        {
            //更新成功
            string packageVersion = operation.PackageVersion;
            Debug.Log($"Request package Version : {packageVersion}");
            _package_version = packageVersion;
        }
        else
        {
            //更新失败
            Debug.LogError(operation.Error);
        }
    }

    private IEnumerator UpdatePackageManifest()
    {
        var package = YooAssets.GetPackage("DefaultPackage");
        var operation = package.UpdatePackageManifestAsync(_package_version);
        yield return operation;

        if (operation.Status == EOperationStatus.Succeed)
        {
            //更新成功
        }
        else
        {
            //更新失败
            Debug.LogError(operation.Error);
        }
    }

    private static ByteBuf LoadByteBuf(string file)
    {
        var package = YooAssets.GetPackage("DefaultPackage");
        var handle = package.LoadAssetSync<TextAsset>(file);
        var asset = handle.AssetObject as TextAsset;

        return new ByteBuf(asset.bytes);

        return new ByteBuf(File.ReadAllBytes($"{Application.dataPath}/Packs/DataTables/{file}.bytes"));
    }

    private void OnDestroy()
    {
        StartCoroutine(DestroyPackage());
    }

    private IEnumerator DestroyPackage()
    {
        // 先销毁资源包
        var package = YooAssets.GetPackage("DefaultPackage");
        DestroyOperation operation = package.DestroyAsync();
        yield return operation;

        // 然后移除资源包
        if (YooAssets.RemovePackage(package))
        {
            Debug.Log("移除成功！");
        }
    }

    public void Init()
    {
        _a.Add(1);
        _a.Add(5);
    }
}
