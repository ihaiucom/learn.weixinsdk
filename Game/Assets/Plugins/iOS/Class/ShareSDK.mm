//
//  ShareAppController.h
//  Unity-iPhone
//
//  Created by ihaiu.com on 17/3/23.
//
//

#import "ShareSDK.h"
#import <TencentOpenAPI/TencentOAuth.h>
#import <TencentOpenAPI/QQApiInterfaceObject.h>
#import <TencentOpenAPI/QQApiInterface.h>


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


-(void)setMyDelegate:(ShareAppController *)delegate
{
    _delegate = delegate;
}


-(ShareAppController *)myDelegate
{
    return _delegate;
}

-(bool)ShareSDKSupportChannel:(int)channel
{
    switch (channel) {
        case CHANNEL_WeiXin:
        case CHANNEL_QQ:
            return true;
            
        default:
            return false;
    }
    return false;
}


-(void)ShareSDKInit:(int)channel andAppId:(const char*)appId
{
    NSString *appid =[NSString stringWithUTF8String:appId];
    UIAlertView *msgbox = [[UIAlertView alloc] initWithTitle:@"App未注册" message:[NSString stringWithFormat:@"channel=%d,  appid=%@",channel, appid] delegate:nil cancelButtonTitle:@"取消" otherButtonTitles:nil];
    [msgbox show];
    
    BOOL result;
    switch (channel) {
        // 向微信注册
        case CHANNEL_WeiXin:
            result = [WXApi registerApp:appid];
            break;
        // 向QQ注册
        case CHANNEL_QQ:
            self.tencentOAuth = [[TencentOAuth alloc] initWithAppId:appid andDelegate: self.myDelegate];
            break;
            
        default:
            break;
    }
}


-(void)ShareSDKLink:(int)channel andFunId:(int) funId andTitle:(const char*) title andDescription:(const char*) description andUrl:(const char*) url andImage:(const char*) image
{
    NSString *titleStr          = [NSString stringWithUTF8String:title];
    NSString *descriptionStr    = [NSString stringWithUTF8String:description];
    NSString *urlStr            = [NSString stringWithUTF8String:url];
    NSString *imageUrlStr       = [NSString stringWithUTF8String:image];
    
    switch (channel) {
            // 分享微信连接
        case CHANNEL_WeiXin:
            [self sendLinkWeiXin:funId andTitle:titleStr andDescription:descriptionStr andUrl:urlStr andImage:imageUrlStr];
            break;
            // 分享QQ连接
        case CHANNEL_QQ:
            [self sendLinkQQ:funId andTitle:titleStr andDescription:descriptionStr andUrl:urlStr andImage:imageUrlStr];
            break;
            
        default:
            break;
    }
}


// 分享QQ连接
- (void) sendLinkQQ:(int) funId andTitle:(NSString*) title andDescription:(NSString*) description andUrl:(NSString*) url andImage:(NSString*) image
{
    
//    NSString *path = [[[NSBundle mainBundle] resourcePath] stringByAppendingPathComponent:[self getAppIconName]];
//    NSData* data = [NSData dataWithContentsOfFile:path];
    
    NSURL *URL = [NSURL URLWithString:url];
    NSURL *imgURL = [NSURL URLWithString:image];
//    QQApiNewsObject* img = [QQApiNewsObject
//                            objectWithURL:URL
//                            title:title
//                            description:description
//                            previewImageData:data];
    
    
    QQApiNewsObject* img = [QQApiNewsObject
                            objectWithURL:URL
                            title:title
                            description:description
                            previewImageURL:imgURL
                            targetContentType: QQApiURLTargetTypeNews];
    
    
    SendMessageToQQReq* req = [SendMessageToQQReq reqWithContent:img];
    QQApiSendResultCode sent;
    switch (funId)
    {
        case FUNID_QQ_Session:
            //将内容分享到qq
            sent = [QQApiInterface sendReq:req];
            break;
            
        case FUNID_QQ_Qzone:
            //将内容分享到qzone
            sent = [QQApiInterface SendReqToQZone:req];
            break;
    }

    [self handleSendResult:sent];

}

- (void)handleSendResult:(QQApiSendResultCode)sendResult
{
    switch (sendResult)
    {
        case EQQAPIAPPNOTREGISTED:
        {
            UIAlertView *msgbox = [[UIAlertView alloc] initWithTitle:@"Error" message:@"App未注册" delegate:nil cancelButtonTitle:@"取消" otherButtonTitles:nil];
            [msgbox show];
            break;
        }
        case EQQAPIMESSAGECONTENTINVALID:
        case EQQAPIMESSAGECONTENTNULL:
        case EQQAPIMESSAGETYPEINVALID:
        {
            UIAlertView *msgbox = [[UIAlertView alloc] initWithTitle:@"Error" message:@"发送参数错误" delegate:nil cancelButtonTitle:@"取消" otherButtonTitles:nil];
            [msgbox show];
            
            break;
        }
        case EQQAPIQQNOTINSTALLED:
        {
            UIAlertView *msgbox = [[UIAlertView alloc] initWithTitle:@"Error" message:@"未安装手Q" delegate:nil cancelButtonTitle:@"取消" otherButtonTitles:nil];
            [msgbox show];
            
            break;
        }
        case EQQAPIQQNOTSUPPORTAPI:
        {
            UIAlertView *msgbox = [[UIAlertView alloc] initWithTitle:@"Error" message:@"API接口不支持" delegate:nil cancelButtonTitle:@"取消" otherButtonTitles:nil];
            [msgbox show];
            
            break;
        }
        case EQQAPISENDFAILD:
        {
            UIAlertView *msgbox = [[UIAlertView alloc] initWithTitle:@"Error" message:@"发送失败" delegate:nil cancelButtonTitle:@"取消" otherButtonTitles:nil];
            [msgbox show];
            
            break;
        }
        case EQQAPIVERSIONNEEDUPDATE:
        {
            UIAlertView *msgbox = [[UIAlertView alloc] initWithTitle:@"Error" message:@"当前QQ版本太低，需要更新" delegate:nil cancelButtonTitle:@"取消" otherButtonTitles:nil];
            [msgbox show];
            break;
        }
        case EQQAPIQZONENOTSUPPORTTEXT:
        {
            UIAlertView *msgbox = [[UIAlertView alloc] initWithTitle:@"Error" message:@"空间分享不支持QQApiTextObject，请使用QQApiImageArrayForQZoneObject分享" delegate:nil cancelButtonTitle:@"取消" otherButtonTitles:nil];
            [msgbox show];
            break;
        }
        case EQQAPIQZONENOTSUPPORTIMAGE:
        {
            UIAlertView *msgbox = [[UIAlertView alloc] initWithTitle:@"Error" message:@"空间分享不支持QQApiImageObject，请使用QQApiImageArrayForQZoneObject分享" delegate:nil cancelButtonTitle:@"取消" otherButtonTitles:nil];
            [msgbox show];
            break;
        }
        default:
        {
            break;
        }
    }
}


// 分享微信连接
- (void) sendLinkWeiXin:(int) funId andTitle:(NSString*) title andDescription:(NSString*) description andUrl:(NSString*) url andImage:(NSString*) image
{
    WXMediaMessage *message = [WXMediaMessage message];
    message.title = title;
    message.description = description;
    // [message setThumbImage:[UIImage imageNamed:[NSString stringWithUTF8String:image]]];
    [message setThumbImage:[UIImage imageNamed:[self getAppIconName]]];
    
    
    WXWebpageObject *ext = [WXWebpageObject object];
    ext.webpageUrl = url;
    message.mediaObject = ext;
    SendMessageToWXReq* req = [[SendMessageToWXReq alloc] init];
    req.bText = NO;
    req.message = message;
    
    switch (funId)
    {
        case FUNID_WeiXin_Session:
            req.scene = WXSceneSession;
            break;
            
        case FUNID_WeiXin_Timeline:
            req.scene = WXSceneTimeline;
            break;
    }
    [WXApi sendReq:req];
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
