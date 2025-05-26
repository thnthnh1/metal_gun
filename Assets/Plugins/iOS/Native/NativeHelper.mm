#import "StoreKit/StoreKit.h"
#import "StringHelper.h"

extern "C" 
{
    char* _getBuildVersion()
    {
        return cStringCopy([[[NSBundle mainBundle] objectForInfoDictionaryKey:@"CFBundleVersion"] UTF8String]);
    }

    bool _showRating()
    {
        if (@available(iOS 10.3, *)) {
            [SKStoreReviewController requestReview];
            return true;
        }
        return false;
    }
}
