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


    public void Init_QQ()
    {
        ShareSDK.Init(ShareSDK.Channel.CHANNEL_QQ, ShareSDK.AppId.QQ);
    }

    public void SendLink_QQ_Friend()
    {
        ShareSDK.SendLink(ShareSDK.FunId.FUNID_QQ_Session);
    }



    public void SendLink_QQ_Timeline()
    {
        ShareSDK.SendLink(ShareSDK.FunId.FUNID_QQ_Qzone);
    }
}
