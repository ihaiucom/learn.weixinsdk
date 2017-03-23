//
//  ShareAppController.h
//  Unity-iPhone
//
//  Created by ihaiu.com on 17/3/23.
//
//

#import "ShareSDK.h"


bool _ShareSDKSupportChannel(int channel)
{
    return [[ShareSDK Instance] ShareSDKSupportChannel:channel];
}


void _ShareSDKInit(int channel, const char* appId)
{
    [[ShareSDK Instance] ShareSDKInit:channel andAppId:appId];
}



void _ShareSDKLink(int channel, int funId, const char* title, const char* description, const char* url, const char* image)
{
    [[ShareSDK Instance] ShareSDKLink:channel andFunId:funId andTitle:title andDescription:description andUrl:url andImage:image];
}

static ShareSDK* _Instance = nullptr;

@implementation ShareSDK

@synthesize channelInitResultDict;

+(id)Instance
{
    return _Instance;
}

+(void)SetInstance:(id)instance
{
    _Instance = instance;
}

-(bool)ShareSDKSupportChannel:(int)channel
{
    switch (channel) {
        case CHANNEL_WeiXin:
            return true;
            
        default:
            return false;
    }
    return false;
}


-(void)ShareSDKInit:(int)channel andAppId:(const char*)appId
{
    BOOL result;
    switch (channel) {
        // 向微信注册
        case CHANNEL_WeiXin:
            result = [WXApi registerApp:[NSString stringWithUTF8String:appId]];
            break;
            
        default:
            break;
    }
}


-(void)ShareSDKLink:(int)channel andFunId:(int) funId andTitle:(const char*) title andDescription:(const char*) description andUrl:(const char*) url andImage:(const char*) image
{
    switch (channel)
    {
        // 分享微信连接
        case CHANNEL_WeiXin:
            WXMediaMessage *message = [WXMediaMessage message];
            message.title = [NSString stringWithUTF8String:title];
            message.description = [NSString stringWithUTF8String:description];
            // [message setThumbImage:[UIImage imageNamed:[NSString stringWithUTF8String:image]]];
            [message setThumbImage:[UIImage imageNamed:[self getAppIconName]]];
            
            
            WXWebpageObject *ext = [WXWebpageObject object];
            ext.webpageUrl = [NSString stringWithUTF8String:url];
            message.mediaObject = ext;
            SendMessageToWXReq* req = [[SendMessageToWXReq alloc] init];
            req.bText = NO;
            req.message = message;
            
            switch (funId) {
                case FUNID_WeiXin_Session:
                    req.scene = WXSceneSession;
                    break;
                    
                case FUNID_WeiXin_Timeline:
                    req.scene = WXSceneTimeline;
                    break;
            }
            [WXApi sendReq:req];
            break;
    }
}


/** 获取app的icon图标名称 */
- (NSString*)getAppIconName{
    NSDictionary *infoDict = [[NSBundle mainBundle] infoDictionary];
    //获取app中所有icon名字数组
    NSArray *iconsArr = infoDict[@"CFBundleIcons"][@"CFBundlePrimaryIcon"][@"CFBundleIconFiles"];
    //取最后一个icon的名字
    NSString *iconLastName = [iconsArr lastObject];
    //打印icon名字
    NSLog(@"iconsArr: %@", iconsArr);
    NSLog(@"iconLastName: %@", iconLastName);
    /*
     打印日志：
     iconsArr: (
     AppIcon29x29,
     AppIcon40x40,
     AppIcon60x60
     )
     */
    
    return iconLastName;
}


@end
