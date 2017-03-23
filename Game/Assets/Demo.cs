using UnityEngine;
using System.Collections;

public class Demo : MonoBehaviour {



    public void Init_WeiXin()
    {
        ShareSDK.Init(ShareSDK.Channel.CHANNEL_WeiXin, ShareSDK.AppId.WeiXin);
    }

    public void SendLink_WeiXin_Friend()
    {
        ShareSDK.SendLink(ShareSDK.FunId.FUNID_WeiXin_Session);
    }



    public void SendLink_WeiXin_Timeline()
    {
        ShareSDK.SendLink(ShareSDK.FunId.FUNID_WeiXin_Timeline);
    }
}
