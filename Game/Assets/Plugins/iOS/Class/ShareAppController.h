//
//  ShareAppController.h
//  Unity-iPhone
//
//  Created by ihaiu.com on 17/3/23.
//
//

#import "UnityAppController.h"
#import "WXApi.h"
#import <TencentOpenAPI/TencentOAuth.h>

@interface ShareAppController : UnityAppController<WXApiDelegate, TencentSessionDelegate>



@end
