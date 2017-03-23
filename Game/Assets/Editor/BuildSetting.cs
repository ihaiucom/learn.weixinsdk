using UnityEngine;
using System.Collections;
using UnityEditor;

using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;
using System.IO;


public class BuildSetting 
{

    //构建完毕处理
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS) 
        {
            OnPostProcessBuild_Share_WeiXin(buildTarget, path);
        }
    }

    public static void OnPostProcessBuild_Share_WeiXin(BuildTarget buildTarget, string path)
    {

        string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject proj = new PBXProject();
        proj.ReadFromString(File.ReadAllText(projPath));
        string target = proj.TargetGuidByName("Unity-iPhone");


        // 添加依赖库 
        proj.AddFrameworkToProject(target, "CoreTelephony.framework", true);
        proj.AddFrameworkToProject(target, "Security.framework", true);
        proj.AddFrameworkToProject(target, "SystemConfiguration.framework", true);
        proj.AddFrameworkToProject(target, "CFNetwork.framework", true);


        proj.AddFileToBuild(target, proj.AddFile("usr/lib/libsqlite3.dylib", "Frameworks/libsqlite3.dylib", PBXSourceTree.Sdk));
        proj.AddFileToBuild(target, proj.AddFile("usr/lib/libz.dylib", "Frameworks/libz.dylib", PBXSourceTree.Sdk));
        proj.AddFileToBuild(target, proj.AddFile("usr/lib/libstdc++.dylib", "Frameworks/libstdc++.dylib", PBXSourceTree.Sdk));

        // 链接器
        proj.AddBuildProperty(target, "OTHER_LDFLAGS", "$(inherited)\n" + "-Objc -all_load");; 

        // 对所有的编译配置设置选项  
        proj.SetBuildProperty(target, "ENABLE_BITCODE", "NO"); 

        File.WriteAllText(projPath, proj.WriteToString());

        Debug.Log(projPath);


        //Info.plist  
        PlistElementArray arr;
        PlistElementDict dict;
        PlistElementArray arr2;

        string plistPath = path + "/Info.plist";  
        PlistDocument plist = new PlistDocument();  
        plist.ReadFromString(File.ReadAllText(plistPath));  
        PlistElementDict rootDict = plist.root;  


        // WeiXin URLSchemes
        arr = rootDict.CreateArray("CFBundleURLTypes");
        dict = arr.AddDict();
        dict.SetString("CFBundleTypeRole", "Editor");
        dict.SetString("CFBundleURLName", "weixing");
        arr2 = dict.CreateArray("CFBundleURLSchemes");
        arr2.AddString(ShareSDK.AppId.WeiXin);

        File.WriteAllText(plistPath, plist.WriteToString());  

        Debug.Log(plistPath);
    }
}
