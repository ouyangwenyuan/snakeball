//
//  MyCountryTool.m
//  gifkeyboard
//
//  Created by FOTOABLE on 2017/12/27.
//  Copyright © 2017年 北京云图微动科技有限公司. All rights reserved.
//

#import "MyCountryTool.h"


@implementation MyCountryTool

char *_getCountryCode(){
    NSLocale *locale = [NSLocale currentLocale];
    NSString *countrycode = [locale localeIdentifier];
    NSLog(@"国家代码：%@",countrycode);
    
    const char *country = [countrycode UTF8String];
    char *back =malloc(countrycode.length + 1);
    char *back2 = back;
    for (int i = 0;i<countrycode.length; i++) {
        *back2 = country[i];
        back2++;
    }
    *back2 = '\0';
    return back;
}
@end
