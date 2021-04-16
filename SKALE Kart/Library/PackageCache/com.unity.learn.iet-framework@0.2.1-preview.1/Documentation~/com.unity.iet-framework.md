# About Framework for In-Editor Tutorials

This package is mandatory for In-Editor Tutorials as it contains the main logic to display them.

# Installing Framework for In-Editor Tutorials

To install this package, follow the instructions in the [Package Manager documentation](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@latest/index.html). 

# Using Framework for In-Editor Tutorials

You have to install a Tutorial now, you can see them in the [Package Manager](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@latest/index.html). 


# Technical details
## Requirements

This version of Framework for In-Editor Tutorials is compatible with the following versions of the Unity Editor:

* 2018.2 and later (recommended)

## Known limitations

## Package contents

The following table indicates the &lt;describe the breakdown you used here&gt;:

|Location|Description|
|---|---|
|`Documentation~`|Contains the documentation of this package.|
|---|`com.unity.iet-framework.md`|This file.|
|`Editor Resources`|Contains the UI elements and sounds.|
|---|`GUI`||
|---|---|`Roboto Font`||
|---|---|---|`Apache License.txt`||
|---|---|---|`Roboto-Black.ttf`||
|---|---|---|`Roboto-BlackItalic.ttf`||
|---|---|---|`Roboto-Bold.ttf`||
|---|---|---|`Roboto-BoldItalic.ttf`||
|---|---|---|`RobotoCondensed-Bold.ttf`||
|---|---|---|`RobotoCondensed-BoldItalic.ttf`||
|---|---|---|`RobotoCondensed-Italic.ttf`||
|---|---|---|`RobotoCondensed-Light.ttf`||
|---|---|---|`RobotoCondensed-LightItalic.ttf`||
|---|---|---|`RobotoCondensed-Regular.ttf`||
|---|---|---|`Roboto-Italic.ttf`||
|---|---|---|`Roboto-Light.ttf`||
|---|---|---|`Roboto-LightItalic.ttf`||
|---|---|---|`Roboto-Medium.ttf`||
|---|---|---|`Roboto-MediumItalic.ttf`||
|---|---|---|`Roboto-Regular.ttf`||
|---|---|---|`Roboto-Thin.ttf`||
|---|---|---|`Roboto-ThinItalic.ttf`||
|---|---|`Textures`||
|---|---|---|`ActiveColor.png`||
|---|---|---|`Back.png`||
|---|---|---|`back@2x.png`||
|---|---|---|`Background.png`||
|---|---|---|`book.png`||
|---|---|---|`Button_Active.png`||
|---|---|---|`Button_Active_Pressed.png`||
|---|---|---|`Button_InActive.png`||
|---|---|---|`Button_Normal.png`||
|---|---|---|`CompletedColor.png`||
|---|---|---|`Home.png`||
|---|---|---|`home@2x.png`||
|---|---|---|`InActiveColor.png`||
|---|---|---|`InActiveColor_Light.png`||
|---|---|---|`ProgressBar.png`||
|---|---|---|`Reset.png`||
|---|---|---|`reset@2x.png`||
|---|---|---|`scrollbar_8px.png`||
|---|---|---|`scrollbar_8pxActive.png`||
|---|---|---|`scrollbar_8pxHover.png`||
|---|---|---|`scrollbar_9px.png`||
|---|---|---|`scrollbar_9pxActive.png`||
|---|---|---|`scrollbar_9pxHover.png`||
|---|---|---|`Swatch - Cyan.png`||
|---|---|---|`Swatch - Deep Blue.png`||
|---|---|---|`Swatch - Graphyte.png`||
|---|---|---|`Swatch - Green.png`||
|---|---|---|`Swatch - Light Gray.png`||
|---|---|---|`Swatch - Magenta.png`||
|---|---|---|`Swatch - Teal.png`||
|---|---|---|`task_completed.png`||
|---|---|---|`task_completed@2x.png`||
|---|---|---|`task_to_do.png`||
|---|---|---|`task_to_do@2x.png`||
|---|`Layouts`||
|---|---|`BasicTutorialLayout.wlt`||
|---|`Sound Effects`||
|---|---|`Pop.aiff`||
|---|---|`Success.wav`||
|`Plugins`||
|---|`UnityEditor.InteractiveTutorialsFramework.dll`||
|`Scripts`||
|---|`Editor`||
|---|---|`Criteria`||
|---|---|---|`ComponentAddedCriterion.cs`||
|---|---|---|`Criterion.cs`||
|---|---|---|`InstantiatePrefabCriterion.cs`||
|---|---|---|`PlayModeStateCriterion.cs`||
|---|---|---|`PrefabInstanceCountCriterion.cs`||
|---|---|---|`PropertyModificationCriterion.cs`||
|---|---|---|`RequiredSelectionCriterion.cs`||
|---|---|---|`TriggerTaskCriterion.cs`||
|---|---|`Editor Windows`||
|---|---|---|`TutorialModalWindow.cs`||
|---|---|---|`TutorialParagraphView.cs`||
|---|---|---|`TutorialWindow.cs`||
|---|---|`Editors`||
|---|---|---|`TutorialEditor.cs`||
|---|---|---|`TutorialPageEditor.cs`||
|---|---|`Masking`||
|---|---|---|`MaskingSettings.cs`||
|---|---|`Models`||
|---|---|---|`AutoCompletion.cs`||
|---|---|---|`InlineIcon.cs`||
|---|---|---|`SceneViewCameraSettings.cs`||
|---|---|---|`Tutorial.cs`||
|---|---|---|`TutorialPage.cs`||
|---|---|---|`TutorialParagraph.cs`||
|---|---|---|`TutorialStyles.cs`||
|---|---|---|`TutorialWelcomePage.cs`||
|---|---|---|`TypedCriterion.cs`||
|---|---|`Property Drawers`||
|---|---|---|`CollectionWrapperDrawer.cs`||
|---|---|---|`CollectionWrapperDrawer.cs`||
|---|---|---|`ComponentAddedCriterionDrawers.cs`||
|---|---|---|`FlushChildrenDrawer.cs`||
|---|---|---|`GUIControlSelectorDrawer.cs`||
|---|---|---|`InlineIconDrawer.cs`||
|---|---|---|`InstantiatePrefabCriterionDrawers.cs`||
|---|---|---|`MaskingSettingsDrawer.cs`||
|---|---|---|`ObjectReferencePropertyDrawer.cs`||
|---|---|---|`SceneObjectReferencePropertyDrawer.cs`||
|---|---|---|`SceneViewCameraSettingsDrawer.cs`||
|---|---|---|`SerializedTypeDrawer.cs`||
|---|---|---|`TutorialParagraphCollectionDrawer.cs`||
|---|---|---|`TutorialParagraphDrawer.cs`||
|---|---|---|`TypedCriterionCollectionDrawer.cs`||
|---|---|---|`TypedCriterionDrawer.cs`||
|---|---|---|`UnmaskedViewDrawer.cs`||
|---|---|`Shims`||
|---|---|---|`PrefabUtilityShim.cs`||
|---|---|`AnalyticsHelper.cs`||
|---|---|`CollectionWrapper.cs`||
|---|---|`FutureObjectReference.cs`||
|---|---|`ObjectReference.cs`||
|---|---|`SceneObjectReference.cs`||
|---|---|`Unity.InteractiveTutorials.Editor.asmdef`||
|---|---|`UserStartupCode.cs`||
|---|`Runtime`||
|---|---|`BaseCollisionBroadcaster.cs`||
|---|---|`CollisionBroadcaster2D.cs`||
|---|---|`CollisionBroadcaster3D.cs`||
|---|---|`IPlayerAvatar.cs`||
|---|---|`SceneObjectGUIDComponent.cs`||
|---|---|`SceneObjectGUIDManager.cs`||
|---|---|`SelectionRoot.cs`||


## Document revision history
This section includes the revision history of the document. The revision history tracks when a document is created, edited, and updated. If you create or update a document, you must add a new row describing the revision.
 
|Date|Reason|
|---|---|
|June 15, 2017|Document created. Matches package version 1.0.|