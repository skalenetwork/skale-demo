namespace Unity.Connect.Share.Editor.store
{
    public class ShareAction {}
    public class ShareStartAction : ShareAction
    {
        public string title;
    }
    public class BuildFinishAction : ShareAction
    {
        public string outputDir;
        public string buildGUID;
    }
    
    public class ZipPathChangeAction : ShareAction
    {
        public string zipPath;
    }
    
    public class UploadStartAction : ShareAction
    {
    }
    
    public class UploadProgressAction : ShareAction
    {
        public int progress;
    }
    
    public class QueryProgressAction : ShareAction
    {
        public string key;
    }
    
    public class QueryProgressResponseAction : ShareAction
    {
        public GetProgressResponse response;
    }

    public class GetProcessProgressAction : ShareAction
    {
    }

    
    public class ShareEndAction : ShareAction
    {
    }
    
    public class ShareProgressAction : ShareAction
    {
    }

    public class ThumbnailSelectAction : ShareAction
    {
        public string thumbnailDir;
    }

    public class TitleChangeAction : ShareAction
    {
        public string title;
    }

    public class DestroyAction : ShareAction
    {
    }

    public class OnErrorAction : ShareAction
    {
        public string errorMsg;
    }

    public class StopUploadAction : ShareAction
    {
    }
    
    public class NotLoginAction : ShareAction
    {
    }

    public class LoginAction : ShareAction
    {
    }
    public class ShareReducer
    {
        public static AppState reducer(AppState old, object action)
        {
            var shareState = reducer(old.shareState, action);
            if (shareState == old.shareState)
            {
                return old;
            }
            return new AppState(shareState: shareState);
        }
        
        public static ShareState reducer(ShareState old, object action)
        {
            
            switch (action)
            {
                case BuildFinishAction build:
                    return old.copyWith(
                        buildOutputDir: build.outputDir,
                        buildGUID: build.buildGUID                       
                    );
                  
                case ZipPathChangeAction zip:
                    return old.copyWith(
                        zipPath: zip.zipPath,
                        step: ShareStep.zip
                        );
                
                case UploadStartAction upload:
                    return old.copyWith(
                        step: ShareStep.upload
                    );
                
                case QueryProgressAction query:
                    int? progress = null;
                    if (old.step != ShareStep.process)
                    {
                        progress = 0;
                    }
                    return old.copyWith(
                        step: ShareStep.process,
                        key: query.key,
                        progress: progress
                    );
                
                case UploadProgressAction upload:
                    return old.copyWith(progress: upload.progress);
 
                case QueryProgressResponseAction queryResponse:
                    ShareStep? step = null;
                    if (queryResponse.response.progress == 100)
                    {
                        step = ShareStep.idle;
                    }
                    return old.copyWith(
                        progress: queryResponse.response.progress,
                        url: queryResponse.response.url,
                        step: step
                        );
                case ThumbnailSelectAction thumbnailAction:
                    return old.copyWith(thumbnailDir: thumbnailAction.thumbnailDir);

                case TitleChangeAction titleChangeAction:
                    return old.copyWith(title: titleChangeAction.title);

                case DestroyAction destroyAction:
                    return new ShareState(buildOutputDir: old.buildOutputDir, buildGUID: old.buildGUID);

                case OnErrorAction errorAction:
                    return old.copyWith(errorMsg: errorAction.errorMsg);
                
                case StopUploadAction stopUploadAction:
                    return new ShareState(buildOutputDir: old.buildOutputDir, buildGUID: old.buildGUID);
                case NotLoginAction login:
                    return old.copyWith(
                        step: ShareStep.login
                    );
                case LoginAction login:
                    return old.copyWith(
                        step: ShareStep.idle
                    );
            }
            return old;
        }
    }
}