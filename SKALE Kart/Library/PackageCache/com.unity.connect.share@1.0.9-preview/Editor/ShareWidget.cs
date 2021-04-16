using System;
using System.IO;
using System.Collections.Generic;
using Unity.Connect.Share.Editor.store;
using Unity.Connect.Share.UIWidgets.Redux;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Color = Unity.UIWidgets.ui.Color;
using TextStyle = Unity.UIWidgets.painting.TextStyle;
using Image = Unity.UIWidgets.widgets.Image;
using System.Linq;

namespace Unity.Connect.Share.Editor
{
    public static class Utils
    {
        // supports GB, MB, KB, or B
        public static string FormatBytes(ulong bytes)
        {
            double gb = bytes / (1024.0 * 1024.0 * 1024.0);
            double mb = bytes / (1024.0 * 1024.0);
            double kb = bytes / 1024.0;
            // Use :#.000 to specify further precision if wanted
            if (mb >= 1000) return $"{gb/*:#.000*/} GB";
            if (kb >= 1000) return $"{mb/*:#.000*/} MB";
            if (kb >= 1) return $"{kb/*:*#.000*/} KB";
            return $"{bytes} B";
        }
    }

    public class ShareReduxWidget:StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return new StoreConnector<AppState, ShareWidget>(
                converter: (state, dispatch) => new ShareWidget(
                    onUpload: (title) => dispatch(new ShareStartAction()  {title = title}),
                    onThumbnailSelect: (thumbnailDir) => dispatch(new ThumbnailSelectAction() { thumbnailDir = thumbnailDir }),
                    onDestroy: () => dispatch(new DestroyAction()),
                    stopUploadAction: () => dispatch(new StopUploadAction()),
                    onErrorAction: (errorMsg) => dispatch(new OnErrorAction() {errorMsg = errorMsg}),
                    shareState: state.shareState),
                builder: (_context, widget) => widget
            );
        }
    }
    
    public class ShareWidget : StatefulWidget
    {
        public readonly Action<string> onUpload;
        public readonly Action<string> onThumbnailSelect;
        public readonly Action onDestroy;
        public readonly Action stopUploadAction;
        public readonly Action<string> onErrorAction;
        public readonly ShareState shareState;

        public ShareWidget(Key key = null, Action<string> onUpload = null, 
            Action<string> onThumbnailSelect = null, Action onDestroy = null, 
            Action stopUploadAction = null, Action<string> onErrorAction = null, ShareState shareState = null) : base(key)
        {
            this.onUpload = onUpload;
            this.onThumbnailSelect = onThumbnailSelect;
            this.onDestroy = onDestroy;
            this.stopUploadAction = stopUploadAction;
            this.onErrorAction = onErrorAction;
            this.shareState = shareState;

        }

        public override State createState()
        {
            return new _WebGLUploadWidgetState();
        }
    }

    class CustomBuildProcessor : IPostprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform == BuildTarget.WebGL)
            {
                StoreFactory.get().Dispatch(new BuildFinishAction
                {
                    outputDir = report.summary.outputPath,
                    buildGUID = report.summary.guid.ToString()
                });

                // Write metadate files into the build directory
                try
                {
                    // depdendencies.txt: list of "depepedency@version"
                    var depFile = $"{report.summary.outputPath}/dependencies.txt";
                    using(var sw = new StreamWriter(depFile, false))
                    {
                        PackageManagerProxy.GetAllVisiblePackages()
                            .Select(pkg => $"{pkg.name}@{pkg.version}")
                            // We probably don't have the package.json of the used Microgame available
                            // so add the information manually.
                            .Concat(new [] { $"{PackageManagerProxy.GetApplicationIdentifier()}@{Application.version}" })
                            .Distinct()
                            .ToList()
                            .ForEach(depStr => sw.WriteLine(depStr));
                    }

                    // The used Unity version.
                    var verFile = $"{report.summary.outputPath}/ProjectVersion.txt";
                    File.Copy("ProjectSettings/ProjectVersion.txt", verFile, overwrite: true);
                }
                catch(Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }

    class _WebGLUploadWidgetState : State<ShareWidget>
    {
        private TextEditingController _controller;
        private FocusNode _focusNode;
        const long ThumbnailLimitBytes = 5 * 1024 * 1024;

        public override void initState() {
            base.initState();
            _controller = new TextEditingController(Application.productName);
            _focusNode = new FocusNode();
        }

        public override void dispose()
        {
            _focusNode.dispose();
            base.dispose();
        }

        private Widget renderInfoText(string text) {
            var label = new Text(data: text, style: new TextStyle(fontSize: 15f, color: new Color(0xFF2F2F2F), fontWeight: FontWeight.w700));
            return new Container(
                    padding: EdgeInsets.only(top: 15, bottom: 10),
                    child:
                        new Row(
                            children: new List<Widget>()
                            {
                                label
                            }
                        )
             );
        }


        private Widget renderLabel(string text) {
            var label = new Text(data: text, style: new TextStyle(fontSize: 13f, color: new Color(0xFF8B8B8B), fontWeight: FontWeight.w700));
            return new Container(
                    padding: EdgeInsets.only(top: 10, bottom: 10),
                    child:
                        new Row(
                            children: new List<Widget>()
                            {
                                label
                            }
                        )
             );
        }

        private Widget renderTitleInput() {
            return new Flexible(
                    child: new Container(
                            padding: EdgeInsets.all(10),
                            decoration: new BoxDecoration(border: Border.all(color: new Color(0xFFCFCFCF))),
                            child: new EditableText(
                                _controller, _focusNode,
                                new TextStyle(fontSize: 15f, height: 1.5f, fontWeight: FontWeight.w700),
                                strutStyle: null,
                                new Color(0xFF424242))
                            )
                   );
        }

        private Widget renderUploadButton()
        {
            return new GestureDetector(
                child: new Container(color: new Color(0xFF2196F3),
                    padding: EdgeInsets.all(20),
                    margin: EdgeInsets.only(top: 20),
                    alignment: Alignment.center,
                    child: new Text(data: "Share", style: new TextStyle(fontSize: 15f, color: new Color(0xFFFFFFFF), fontWeight: FontWeight.w700)),
                    height: 58f

                ),
                onTap: () =>
                {
                    var title = _controller.text;

                    if (!string.IsNullOrEmpty(title))
                    {
                        widget.onUpload(_controller.text);
                    }
                }
            );
        }

        private Widget renderUpload() {
            var thumbnailPath = widget.shareState.thumbnailDir;
            Widget image = new Icon(Icons.add_photo_alternate, color: new Color(0xFFCECECE), size: 72f);

            if (!string.IsNullOrEmpty(thumbnailPath)) {
                image = Image.file(thumbnailPath, fit: BoxFit.cover, width: 72, height: 72);
            }

            var imgSelectButton = new FlatButton(
                                    child: new Text("Select image file...", style: new TextStyle(fontSize: 15f, color: new Color(0xFF2196F3), fontWeight: FontWeight.w700)),
                                    onPressed: () =>
                                    {
                                        string path = EditorUtility.OpenFilePanel("Select thumbnail", "", "jpg,jpeg,png");
                                        if (path.Length != 0)
                                        {
                                            var fileInfo = new System.IO.FileInfo(path);
                        
                                            if (fileInfo.Length > ThumbnailLimitBytes) {
                                                widget.onErrorAction($"Max. file size {Utils.FormatBytes(ThumbnailLimitBytes)}");
                                            } else {
                                                widget.onThumbnailSelect(path);
                                            }
                                            
                                        }
                                    }
                                );

            var label = new Text(
                data: $"Max. file size {Utils.FormatBytes(ThumbnailLimitBytes)} (PNG or JPG)",
                style: new TextStyle(fontSize: 13f, color: new Color(0xFF888888))
            );

            return new Container(
                    color: new Color(0xFFF6F6F6),
                    padding: EdgeInsets.only(top: 10, bottom: 10),
                    child:
                        new Column(
                            children: new List<Widget>()
                            {
                                image,
                                imgSelectButton,
                                label
                            }
                    ),
                    constraints: BoxConstraints.expand().widthConstraints()
             );
        }

        private Widget renderUploadingHead() {
            var imagePath = widget.shareState.thumbnailDir;
            Widget image = new Icon(Icons.image, color: new Color(0xFFCECECE), size: 72f);

            if (!string.IsNullOrEmpty(imagePath)) {
                image = Image.file(imagePath, fit: BoxFit.cover, width: 72, height: 50);
            }
            
            var title = new Text(data: widget.shareState.title ?? "Untitled", style: new TextStyle(height: 1.2f, fontSize: 15f, color: new Color(0xFF424242), fontWeight: FontWeight.w700));
            var userName = UnityEditor.CloudProjectSettings.userName;
            var userNameWidget = new Text(data: "By " + userName, style: new TextStyle(height: 1.2f, fontSize: 15f, color: new Color(0xFF888888)));

            return new Container(
                    padding: EdgeInsets.only(top: 10, bottom: 20, left: 20, right: 20),
                    color: new Color(0xFFFFFFFF),
                    child:
                        new Row(
                            children: new List<Widget>()
                            {
                                new Container(
                                    margin: EdgeInsets.only(right: 20),
                                    child: image
                                    ),
                                new Column(
                                    children: new List<Widget>(){
                                        title,
                                        userNameWidget,
                                    },
                                    crossAxisAlignment: CrossAxisAlignment.start
                                )
                            }
                        )
             );
        }

        private Widget renderIcon(IconData icon ) {
            return new Icon(icon, color: new Color(0xFFCECECE), size: 72f);
        }

        private Widget renderProgress()
        {
            int progress = widget.shareState.progress;
            progress = Math.Min(100, Math.Max(progress, 0));

            return new Container(
                margin: EdgeInsets.only(top: 10),
                child: new LinearProgressIndicator(
                    backgroundColor: new Color(0xFFD6D6D6),
                    value: (progress / 100.0f)
                )
            );
        }

        private Widget renderProgressLabel(string prefix) {
            int progress = widget.shareState.progress;
            return renderHighlightedLabel(prefix + " (" + progress + "%)...");
        }

        private Widget renderCancel() {
            return new GestureDetector(
                child: new Container(
                            margin: EdgeInsets.only(top: 20),
                            alignment: Alignment.center,
                            child: new Text("Cancel", style: new TextStyle(fontSize: 15f, color: new Color(0xFF2196F3), fontWeight: FontWeight.w700))
                ),
                onTap: () => { widget.stopUploadAction(); }
            );
        }

        private Widget renderHighlightedLabel(string label)
        {
            int progress = widget.shareState.progress;

            return new Container(
                margin: EdgeInsets.only(top: 20),
                child: new Text(data: label, style: new TextStyle(height: 1.2f, fontSize: 15f, color: new Color(0xFF424242), fontWeight: FontWeight.w700))
            );
        }
        
        private Widget renderNormalLabel(string label)
        {
            var text = new Text(data: label, style: new TextStyle(fontSize: 13f, color: new Color(0xFF888888)));
            return new Container(
                child:
                new Row(
                    children: new List<Widget>()
                    {
                        new Container(padding: EdgeInsets.only(top:20), child: text)
                    },
                    mainAxisAlignment: MainAxisAlignment.center
                )
            );
        }

        private Widget renderProjectUrl() {
            var url = widget.shareState.url;
            return new GestureDetector(
                child:
                        new Container(
                            margin: EdgeInsets.only(top: 20),
                            child: new Text(url, style: new TextStyle(height: 1.2f, fontSize: 15f, color: new Color(0xFF2196F3), fontWeight: FontWeight.w700))
                        ), 
                onTap: () => { Application.OpenURL(url);}
           );
        }
        
        private Widget renderGoBack()
        {
            return new GestureDetector(
                child: new Container(
                            margin: EdgeInsets.only(top: 20),
                            alignment: Alignment.center,
                            child: new Text("Go Back", style: new TextStyle(fontSize: 15f, color: new Color(0xFF2196F3), fontWeight: FontWeight.w700))
                ),
                onTap: () => { widget.onDestroy(); }
            );
        }
        
        private Widget renderNoBuildLogo() 
        {
            var logo = Image.asset( "html5-logo", fit: BoxFit.cover, width: 34, height: 34);
            var label = new Text(data: "WebGL", style: new TextStyle(fontSize: 15f, color: new Color(0xFF424242), fontWeight: FontWeight.w700));
            var text = new Container(
                    child:
                        new Row(
                            children: new List<Widget>()
                            {
                                new Container(padding: EdgeInsets.only(top:8), child: label)
                            },
                            mainAxisAlignment: MainAxisAlignment.center
                        )
                );

            return new Unity.UIWidgets.material.Material(
                elevation: 3f,
                shadowColor: Color.fromRGBO(0, 0, 0, 0.1f),
                shape: new CircleBorder(
                        side: new BorderSide(
                            width: 2.0f,
                            style: BorderStyle.none
                        )
                    ),
                child: new Container(
                        child:
                            new Column(
                                children: new List<Widget>()
                                {
                                    logo,
                                    text
                                },
                                mainAxisAlignment: MainAxisAlignment.center
                            ),
                        height: 110,
                        width: 110,
                        decoration: new BoxDecoration(color: new Color(0xFFFFFFFF), shape: BoxShape.circle)
                    )
            );
        }

        private Widget renderNoBuildButtons() {
            var button = new Container(color: Color.fromRGBO(33, 150, 243, 0.3f),
                            padding: EdgeInsets.all(20),
                            alignment: Alignment.bottomCenter,
                            child: new Text(data: "Share", style: new TextStyle(fontSize: 15f, color: new Color(0xFFFFFFFF), fontWeight: FontWeight.w700)),
                            height: 58f
                        
                );

            return new Container(color: new Color(0x00000000),
                alignment: Alignment.bottomCenter,
                child: new Container(color: new Color(0xFFFFFFFF),
                            padding: EdgeInsets.all(20),
                            alignment: Alignment.bottomCenter,
                            child: new Center(child: button),
                            height: 98f
                        )
                );
        }

        private Widget renderUploadWidget() {
            var list = new List<Widget>
                {
                    renderInfoText("Upload and Share your WebGL Game to Unity Connect"),
                    renderLabel("Project Name"),
                    renderTitleInput(),
                    renderLabel("Project Thumbnail (Optional)"),
                    renderUpload(),
                    renderUploadButton()
                };

            return new Container(
                color: new Color(0xFFFFFFFF),
                padding: EdgeInsets.only(top: 10, left: 20, right: 20, bottom: 20),
                child: new Column(children: list, mainAxisAlignment: MainAxisAlignment.spaceBetween)
            );
        }
       
        private Widget renderUploadingWidget() {
            var list = new List<Widget>
                {
                    renderUploadingHead(),
                    new Container(
                        color: new Color(0xFFF6F6F6),
                        padding: EdgeInsets.only(left: 40, right: 40, bottom: 60),
                        child: new Column(
                            children: new List<Widget>{
                                renderIcon(Icons.cloud_upload),
                                renderProgress(),
                                renderProgressLabel("Uploading"),
                                renderCancel()
                            }
                        )
                    )
                };
            return new Container(
                color: new Color(0xFFF6F6F6),
                padding: EdgeInsets.only(top: 10),
                child: new Column(children: list, mainAxisAlignment: MainAxisAlignment.spaceBetween)
            );
        }

        private Widget renderProcessingWidget()
        {
            var list = new List<Widget>
                {
                    renderUploadingHead(),
                    new Container(
                        color: new Color(0xFFF6F6F6),
                        padding: EdgeInsets.only(left: 40, right: 40, bottom: 80),
                        child: new Column(
                            children: new List<Widget>{
                                renderIcon(Icons.settings),
                                renderProgress(),
                                renderProgressLabel("Processing")
                            }
                        )
                    )
                };

            return new Container(
                color: new Color(0xFFF6F6F6),
                padding: EdgeInsets.only(top: 10),
                child: new Column(children: list, mainAxisAlignment: MainAxisAlignment.spaceBetween)
            );
        }

        private Widget renderSuccessWidget()
        {
            var list = new List<Widget>
                {
                    renderUploadingHead(),
                    new Container(
                        color: new Color(0xFFF6F6F6),
                        padding: EdgeInsets.only(left: 40, right: 40, bottom: 70),
                        child: new Column(
                            children: new List<Widget>{
                                renderIcon(Icons.cloud_done),
                                renderHighlightedLabel("Upload completed:"),
                                renderProjectUrl(),
                                renderNormalLabel("You can play your game in a web browser and share it with your friends!")
                            }
                        ),
                        constraints: BoxConstraints.expand().widthConstraints(),
                        alignment: Alignment.center
                    )
                };
            return new Container(
                color: new Color(0xFFF6F6F6),
                padding: EdgeInsets.only(top: 10),
                child: new Column(children: list, mainAxisAlignment: MainAxisAlignment.spaceBetween)
            );
        }

        private Widget renderErrorWidget()
        {
            var list = new List<Widget>
                {
                    renderUploadingHead(),
                    new Container(
                        color: new Color(0xFFF6F6F6),
                        padding: EdgeInsets.all(40),
                        child: new Column(
                            children: new List<Widget>{
                                renderIcon(Icons.error),
                                renderHighlightedLabel("Something went wrong"),
                                renderNormalLabel(widget.shareState.errorMsg),
                                renderGoBack()
                            },
                            mainAxisAlignment: MainAxisAlignment.center
                        ),
                        constraints: BoxConstraints.expand().widthConstraints()
                    )
                };
            return new Container(
                color: new Color(0xFFF6F6F6),
                padding: EdgeInsets.only(top: 10),
                child: new Column(children: list, mainAxisAlignment: MainAxisAlignment.spaceBetween)
            );
        }
        
        private Widget renderNoBuildWidget()
        {
            return new Stack(
                            children: new List<Widget>{
                                 new Container(color: new Color(0xFFF6F6F6),
                                            padding: EdgeInsets.only(20, top: 40, right: 20, bottom: 20),
                                            child: new Column(
                                                children: new List<Widget>
                                                {
                                                    renderNoBuildLogo(),
                                                    renderHighlightedLabel("You need to build a WebGL build to share your game"),
                                                    renderNormalLabel("Go to: File > Build Settings > Select WebGL Platform > Build")
                                                }
                                            )),
                                 renderNoBuildButtons()
                             }
                            
                    );
        }

        private Widget renderNotLoginWidget()
        {
            return new Stack(
                children: new List<Widget>{
                    new Container(color: new Color(0xFFF6F6F6),
                        padding: EdgeInsets.only(top: 65, left: 20, right: 20, bottom: 20),
                        child: new Column(
                            children: new List<Widget>
                            {
                                renderIcon(Icons.account_circle),
                                renderHighlightedLabel("You need to be signed in to share your game"),
                                renderNormalLabel("In Unity Editor go to: Account > Sign In...")
                            }
                        )),
                    renderNoBuildButtons()
                }
                            
            );
        }

        public override Widget build(BuildContext context)
        {
            var buildOutputDir = widget.shareState.buildOutputDir;
            var projectUrl = widget.shareState.url;
            var errorMsg = widget.shareState.errorMsg;
            var step = widget.shareState.step;
 
            Widget root;
            if (step == ShareStep.login)
            {
                root = renderNotLoginWidget();
                
            } else if (string.IsNullOrEmpty(buildOutputDir) || !Directory.Exists(buildOutputDir))
            {
                root = renderNoBuildWidget();
                
            } 
            else if (!string.IsNullOrEmpty(projectUrl))
            {
                root = renderSuccessWidget();
            }
            else if (!string.IsNullOrEmpty(errorMsg))
            {
                root = renderErrorWidget();
            }
            else if (step == ShareStep.upload)
            {
                root = renderUploadingWidget();
   
            } else if (step == ShareStep.process)
            {
                root = renderProcessingWidget();
            }
            else
            {
                root = renderUploadWidget();
            }

            return new DefaultTextStyle(
                child: root,
                style: new TextStyle(decoration: TextDecoration.none)
            );
        }
    }
}
