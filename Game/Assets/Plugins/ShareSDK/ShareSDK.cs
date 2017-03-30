using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;



public class ShareSDK 
{
    public static class AppId
    {
        public static string WeiXin = "wx0b34173e8d4b4ef7";
        public static string QQ = "1105537897";
    }

    public static class Channel
    {
        public const int CHANNEL_WeiXin     = 1;
        public const int CHANNEL_QQ         = 2;
        public const int CHANNEL_SinaWeibo  = 3;


    }

    public static class FunId
    {

        public const int FUNID_WeiXin_Session       = 11;
        public const int FUNID_WeiXin_Timeline      = 12;

        public const int FUNID_QQ_Session           = 21;
        public const int FUNID_QQ_Qzone             = 22;
        public const int FUNID_QQ_Weibo             = 23;

        public const int FUNID_SinaWeibo_Session    = 31;
        public const int FUNID_SinaWeibo_Timeline   = 32;
    }


    #if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern bool _ShareSDKSupportChannel(int channel);


    [DllImport("__Internal")]
    private static extern void _ShareSDKInit(int channel, string appId);


    [DllImport("__Internal")]
    private static extern void _ShareSDKLink(int channel, int funId, string title, string  description, string url, string image);


    public static bool SupportChannel(int channel)
    {
        return _ShareSDKSupportChannel(channel);
    }

    public static void InitApp(string appName)
    {
    }

    public static void Init(int channel, string appId)
    {
        _ShareSDKInit(channel, appId);
    }


    public static void SendLink(int channel, int funId, string title, string description, string url, string image)
    {
        _ShareSDKLink(channel, funId, title, description, url, image);
    }

    #elif UNITY_ANDROID && !UNITY_EDITOR

    public static bool SupportChannel(int channel)
    {
        AndroidJavaClass    androidJavaClass = new AndroidJavaClass("com.ihaiu.share.ShareSDK");
        return androidJavaClass.CallStatic<bool>("supportChannel", channel);
    }

    public static void InitApp(string appName)
    {
        AndroidJavaClass    androidJavaClass = new AndroidJavaClass("com.ihaiu.share.ShareSDK");
        androidJavaClass.CallStatic("initApp", appName);
    }

    public static void Init(int channel, string appId)
    {
        AndroidJavaClass    androidJavaClass = new AndroidJavaClass("com.ihaiu.share.ShareSDK");
        androidJavaClass.CallStatic("init", channel, appId);
    }


    public static void SendLink(int channel, int funId, string title, string description, string url, string image)
    {
        AndroidJavaClass    androidJavaClass = new AndroidJavaClass("com.ihaiu.share.ShareSDK");
        androidJavaClass.CallStatic("sendLink", funId, title, description, url, image);
    }
    #else
    public static bool SupportChannel(int channel)
    {
        return false;
    }


    public static void InitApp(string appName)
    {
    }

    public static void Init(int channel, string appId)
    {
    }


    public static void SendLink(int channel, int funId, string title, string description, string url, string image)
    {
        
    }
    #endif


    public static void SendLink(int funId, string title, string description, string url, string image)
    {
        int channel = funId / 10;
        SendLink(channel, funId, title, description, url, image);
    }

    public static void SendLink(int funId)
    {
        SendLink(funId, "爱海游", "爱海游博客，用作学习游戏开发技术的笔记和技术分享。", "http://blog.ihaiu.com", "http://blog.ihaiu.com/assets/icon/icon76_white.png");
    }





}
