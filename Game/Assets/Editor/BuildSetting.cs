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
        proj.AddBuildProperty(target, "OTHER_LDFLAGS", "$(inherited)\n" + "-Objc -all_load" + "\n-fobjc-arc");; 

        // 对所有的编译配置设置选项  
        proj.SetBuildProperty(target, "ENABLE_BITCODE", "NO"); 

        File.WriteAllText(projPath, proj.WriteToString());

        Debug.Log(projPath);


        //Info.plist  
        PlistElementArray arr;
        PlistElementDict dict;
        PlistElementArray arr2;
        PlistElement element;

        string plistPath = path + "/Info.plist";  
        PlistDocument plist = new PlistDocument();  
        plist.ReadFromString(File.ReadAllText(plistPath));  
        PlistElementDict rootDict = plist.root;  

        // QQ LSApplicationQueriesSchemes

        element = rootDict["LSApplicationQueriesSchemes"];
        if (element != null)
        {
            arr = element.AsArray();
        }
        else
        {
            arr = null;
        }
        if(arr == null) arr = rootDict.CreateArray("LSApplicationQueriesSchemes");
        arr.AddString("mqq");
        arr.AddString("mqqapi");
        arr.AddString("mqqbrowser");
        arr.AddString("mttbrowser");
        arr.AddString("mqqOpensdkSSoLogin");
        arr.AddString("mqqopensdkapiV2");
        arr.AddString("mqqopensdkapiV3");
        arr.AddString("mqqopensdkapiV4");
        arr.AddString("wtloginmqq2");
        arr.AddString("mqzone");
        arr.AddString("mqzoneopensdk");
        arr.AddString("mqzoneopensdkapi");
        arr.AddString("mqzoneopensdkapi19");
        arr.AddString("mqzoneopensdkapiV2");
        arr.AddString("mqqapiwallet");
        arr.AddString("mqqopensdkfriend");
        arr.AddString("mqqopensdkdataline");
        arr.AddString("mqqgamebindinggroup");
        arr.AddString("mqqopensdkgrouptribeshare");
        arr.AddString("tencentapi.qq.reqContent");
        arr.AddString("tencentapi.qzone.reqContent");


        // WeiXin URLSchemes
        element = rootDict["CFBundleURLTypes"];
        if (element != null)
        {
            arr = element.AsArray();
        }
        else
        {
            arr = null;
        }
        if(arr == null) arr = rootDict.CreateArray("CFBundleURLTypes");
        dict = arr.AddDict();
        dict.SetString("CFBundleTypeRole", "Editor");
        dict.SetString("CFBundleURLName", "weixing");
        arr2 = dict.CreateArray("CFBundleURLSchemes");
        arr2.AddString(ShareSDK.AppId.WeiXin);


        // QQ URLSchemes
        dict = arr.AddDict();
        dict.SetString("CFBundleTypeRole", "Editor");
        dict.SetString("CFBundleURLName", "tencentopenapi");
        arr2 = dict.CreateArray("CFBundleURLSchemes");
        arr2.AddString("tencent" + ShareSDK.AppId.QQ);

        File.WriteAllText(plistPath, plist.WriteToString());  

        Debug.Log(plistPath);
    }
}
