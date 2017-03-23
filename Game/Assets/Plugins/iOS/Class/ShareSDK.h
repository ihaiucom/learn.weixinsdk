//
//  ShareAppController.h
//  Unity-iPhone
//
//  Created by ihaiu.com on 17/3/23.
//
//


#import <Foundation/Foundation.h>
#import "WXApi.h"

extern "C"{
    bool _ShareSDKSupportChannel(int channel);
    void _ShareSDKInit(int channel, const char* appId);
    void _ShareSDKLink(int channel, int funId, const char* title, const char* description, const char* url, const char* image);
}


#define CHANNEL_WeiXin         1
#define CHANNEL_QQ             2
#define CHANNEL_SinaWeibo      3



#define FUNID_WeiXin_Session         11
#define FUNID_WeiXin_Timeline        12
#define FUNID_QQ_Session             21
#define FUNID_QQ_Qzone               22
#define FUNID_QQ_Weibo               23
#define FUNID_SinaWeibo_Session      31
#define FUNID_SinaWeibo_Timeline     32

@interface ShareSDK : NSObject
{
    
    
    @public
    NSDictionary *channelInitResultDict;
    
}

+(id)Instance;
+(void)SetInstance:(id)instance;

@property NSDictionary *channelInitResultDict;

-(bool)ShareSDKSupportChannel:(int)channel;
-(void)ShareSDKInit:(int)channel andAppId:(const char*)appId;
-(void)ShareSDKLink:(int)channel andFunId:(int) funId andTitle:(const char*) title andDescription:(const char*) description andUrl:(const char*) url andImage:(const char*) image;

@end
