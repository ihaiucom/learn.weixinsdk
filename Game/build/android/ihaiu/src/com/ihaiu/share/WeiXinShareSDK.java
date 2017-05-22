package com.ihaiu.share;

import com.ihaiu.share.ShareSDK.FunId;
import com.tencent.mm.opensdk.modelmsg.SendMessageToWX;
import com.tencent.mm.opensdk.modelmsg.WXMediaMessage;
import com.tencent.mm.opensdk.modelmsg.WXWebpageObject;
import com.tencent.mm.opensdk.openapi.IWXAPI;
import com.tencent.mm.opensdk.openapi.WXAPIFactory;

public class WeiXinShareSDK 
{
	private static IWXAPI api;
	
	
	public static void init(String appId)
	{
		api = WXAPIFactory.createWXAPI(ShareSDK.getActivity(), appId);
		api.registerApp(appId);
	}
	
	
	
	public static void sendLink(int funId, String title, String description, String url, String image)
	{
		int scene = funId == FunId.FUNID_WeiXin_Session ? SendMessageToWX.Req.WXSceneSession : SendMessageToWX.Req.WXSceneTimeline;
		
				
		WXWebpageObject webpage = new WXWebpageObject();
		webpage.webpageUrl = url;
		WXMediaMessage msg = new WXMediaMessage(webpage);
		msg.title = title;
		msg.description = description;
		
		msg.thumbData = Util.getHtmlByteArray(image);
		
		SendMessageToWX.Req req = new SendMessageToWX.Req();
		req.transaction = buildTransaction("webpage");
		req.message = msg;
		req.scene = scene;
		api.sendReq(req);
	}
	
	

	private static String buildTransaction(final String type) {
		return (type == null) ? String.valueOf(System.currentTimeMillis()) : type + System.currentTimeMillis();
	}
}
