package com.ihaiu.share;


import android.app.Activity;
import android.widget.Toast;

import com.unity3d.player.UnityPlayer;


public class ShareSDK 
{
	public class Channel
	{
        public static final int CHANNEL_WeiXin     = 1;
        public static final int CHANNEL_QQ         = 2;
        public static final int CHANNEL_SinaWeibo  = 3;
	}
	
	 public class FunId
    {

        public static final int FUNID_WeiXin_Session       = 11;
        public static final int FUNID_WeiXin_Timeline      = 12;

        public static final int FUNID_QQ_Session           = 21;
        public static final int FUNID_QQ_Qzone             = 22;
        public static final int FUNID_QQ_Weibo             = 23;

        public static final int FUNID_SinaWeibo_Session    = 31;
        public static final int FUNID_SinaWeibo_Timeline   = 32;
    }
	 
	 public static Boolean supportChannel(int channel)
	 {
		 switch (channel) {
			case Channel.CHANNEL_WeiXin:
				return true;

			default:
				return false;
			}
	 }
	
	private static Activity _activity;
	public static Activity getActivity()
	{
		if(_activity == null)
		{
			return UnityPlayer.currentActivity;
		}
		
		return _activity;
	}
	
	public static String appName = "爱海游";
	public static void initApp(String appName)
	{
		ShareSDK.appName = appName;
	}
	
	public static void init(int channel, String appId)
	{
		_activity = UnityPlayer.currentActivity;
		log("int channel="+channel + ", appId="+ appId);
		switch (channel) {
		case Channel.CHANNEL_WeiXin:
			WeiXinShareSDK.init(appId);
			break;
		case Channel.CHANNEL_QQ:
			QQShareSDK.init(appId);
			break;

		default:
			break;
		}
	}

	 
	public static void sendLink(int funId, String title, String description, String url, String image)
	{
		log("sendLink funId="+funId + ", title="+ title);
		switch (funId) {
		case FunId.FUNID_WeiXin_Session:
		case FunId.FUNID_WeiXin_Timeline:
			WeiXinShareSDK.sendLink(funId, title, description, url, image);
			break;
			
		case FunId.FUNID_QQ_Session:
		case FunId.FUNID_QQ_Qzone:
		case FunId.FUNID_QQ_Weibo:
			QQShareSDK.sendLink(funId, title, description, url, image);
			break;

		default:
			break;
		}
	}
	
	public static void log(final String msg)
	{
//		UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
//			
//			@Override
//			public void run() {
//
//				Toast.makeText(UnityPlayer.currentActivity, msg, Toast.LENGTH_SHORT).show();
//			}
//		});
	}
	
	
	
}
