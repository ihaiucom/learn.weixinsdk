package com.ihaiu.share;


import java.util.ArrayList;

import com.tencent.connect.share.QQShare;
import com.tencent.connect.share.QzoneShare;
import com.tencent.tauth.IUiListener;
import com.tencent.tauth.Tencent;
import com.tencent.tauth.UiError;

import android.os.Bundle;

public class QQShareSDK
{
	public static Tencent mTencent;
	
	
	public static void init(String appId)
	{
		mTencent = Tencent.createInstance(appId, ShareSDK.getActivity());
	}

    private static int shareType = QQShare.SHARE_TO_QQ_TYPE_DEFAULT;
	public static void sendLink(int funId, String title, String description, String url, String image)
	{
		if(funId == ShareSDK.FunId.FUNID_QQ_Session)
		{
			sendLinkQQ(title, description, url, image);
		}
		else if(funId == ShareSDK.FunId.FUNID_QQ_Qzone)
		{
			sendLinkQZone(title, description, url, image);
		}
	}
	
	public static void sendLinkQQ(String title, String description, String url, String image)
	{
		final Bundle params = new Bundle();
		shareType = QQShare.SHARE_TO_QQ_TYPE_DEFAULT;
		params.putInt(QQShare.SHARE_TO_QQ_KEY_TYPE, QQShare.SHARE_TO_QQ_TYPE_DEFAULT);
		params.putString(QQShare.SHARE_TO_QQ_TITLE, title); 
		params.putString(QQShare.SHARE_TO_QQ_SUMMARY, description);
		params.putString(QQShare.SHARE_TO_QQ_TARGET_URL, url);
		params.putString(QQShare.SHARE_TO_QQ_IMAGE_URL, image);
		params.putString(QQShare.SHARE_TO_QQ_APP_NAME, ShareSDK.appName);
        params.putInt(QQShare.SHARE_TO_QQ_EXT_INT, 0x00);
        
        doShareToQQ(params);
	}
	

	public static void sendLinkQZone(String title, String description, String url, String image)
	{
		ShareSDK.log("sendLinkQZone  title="+ title);
		final Bundle params = new Bundle();
		shareType = QzoneShare.SHARE_TO_QZONE_TYPE_IMAGE_TEXT;
		params.putInt(QzoneShare.SHARE_TO_QZONE_KEY_TYPE, QzoneShare.SHARE_TO_QZONE_TYPE_IMAGE_TEXT);
		params.putString(QzoneShare.SHARE_TO_QQ_TITLE, title); 
		params.putString(QzoneShare.SHARE_TO_QQ_SUMMARY, description);
		params.putString(QzoneShare.SHARE_TO_QQ_TARGET_URL, url);
        ArrayList<String> imageUrls = new ArrayList<String>();
        imageUrls.add(image);
        params.putStringArrayList(QzoneShare.SHARE_TO_QQ_IMAGE_URL, imageUrls);
        
		doShareToQzone(params);
	}
	

//	public static void sendLinkWeibo(String title, String description, String url, String image)
//	{
//		Bundle bundle = new Bundle(); 
//		bundle.putString("format", "json");
//		bundle.putString("content", description); 
//		bundle.putByteArray("pic", Util.getHtmlByteArray(image));
//		
//		mTencent.requestAsync(SocialConstants.GRAPH_ADD_PIC_T, bundle, SocialConstants.HTTP_POST, new BaseApiListener("add_pic_t", false), null);
//	
//	}
	
	
	
	

   /**
    * 用异步方式启动分享
    * @param params
    */
   private static void doShareToQQ(final Bundle params) {
        // QQ分享要在主线程做
        ThreadManager.getMainHandler().post(new Runnable() {

            @Override
            public void run() {
                if (null != mTencent) {
            		mTencent.shareToQQ(ShareSDK.getActivity(), params, qqShareListener);
                }
            }
        });
    }
   
   private static void doShareToQzone(final Bundle params) {
       // QZone分享要在主线程做
	   ThreadManager.getMainHandler().post(new Runnable() {

           @Override
           public void run() {
               if (null != mTencent) {
            	   ShareSDK.log("QQShareSDK mTencent.shareToQzone:");
           		mTencent.shareToQzone(ShareSDK.getActivity(), params, qZoneShareListener);
               }
           }
       });
   }

	
   static IUiListener qqShareListener = new IUiListener() {
        @Override
        public void onCancel() {

        	ShareSDK.log("QQShareSDK onCannel:");
        }
        @Override
        public void onComplete(Object response) {
        	ShareSDK.log("QQShareSDK onComplete:" + response.toString());
        }
        @Override
        public void onError(UiError e) {
        	ShareSDK.log("QQShareSDK onError: " + e.errorMessage);
        }
    };
    
    static IUiListener qZoneShareListener = new IUiListener() {

        @Override
        public void onCancel() {
        	ShareSDK.log("QQShareSDK onCannel:");
        }

        @Override
        public void onError(UiError e) {

        	ShareSDK.log("QQShareSDK onComplete:" + e.errorMessage);
        }

		@Override
		public void onComplete(Object response) {
        	ShareSDK.log("QQShareSDK onComplete:" + response.toString());
		}

    };
}
