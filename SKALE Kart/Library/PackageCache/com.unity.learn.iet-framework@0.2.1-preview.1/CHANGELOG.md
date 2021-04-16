# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this package adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.2.1] - 2019-11-11
### Fixed
 - Do not show the Welcome dialog and load the IET window layout every time an IET project is started.
 - Fixed IET initialization when a Microgame is loaded from the Asset Store.

### Changed
 - Do not clear the description of a tutorial card when a tutorial is marked as completed.
 
## [0.2.0] - 2019-10-21
### Changed
 - `Readme` class renamed to more suitable `TutorialContainer`.
 
## [0.1.18] - 2019-10-21
### Changed
 - New single-panel approach, Readme and Tutorials are shown in the same window which is always visible.
 - Ability to save the Project window's state for the end-user when saving layouts for tutorials.
 - `Readme` class moved into `Unity.InteractiveTutorials` namespace.

## [0.1.17] - 2019-07-19
### Changed
 - Updated UI styles.

## [0.1.16] - 2019-05-15
### Changed
 - Updated warning message when the user is about to exit the tutorial.

## [0.1.15] - 2019-03-04
### Added
 - Adding the ability to unmask elements based on the name of the GUIStyle used to draw them.
 - Warning message when the user is about to exit the tutorial.

### Changed
 - When clicking on *Help > Template Walkthroughs* if the inspector window is not visible, the Inspector window will be shown

## [0.1.14] - 2019-02-12
### Fixed
 - If the user opens an Undocked window, that is not part of the tutorial, the window tabs are unmasked, so they can close or move the window.
 - Improved compatibility with old content.

## [0.1.13] - 2019-02-04
### Added
- Add support for specifying alternate EditorWindow types when configuring unmasked views
- Expand unmask region to include foldout arrow when unmasking property that is collapsed

### Removed
- Remove "Couldn't find a readme" message when there is no Readme asset in project

## [0.1.12] - 2019-01-24
### Fixed
- Fix editor entering and exiting play mode on project load
- Fix unmasked property unmasking entire window when ancestor property is collapsed

## [0.1.11] - 2019-01-17
### Fixed
- Fix 2019.1 compilation errors
- Improve invalid ScriptableObject reference workaround to always exit play mode after project load

## [0.1.10] - 2019-01-11
### Fixed
- Fix invalid CHANGELOG formatting.

## [0.1.9] - 2019-01-11
### Changed
- SceneViewCameraMovedCriterion will also complete if the user changes the camera orientation.

### Fixed
- Added work around for issue where tutorial is not loaded initial project load

## [0.1.8] - 2018-12-11
### Fixed
- Fixed build script

## [0.1.7] - 2018-12-10
### Fixed
- Fixed authoring of scene object references.
### Removed
- Remove *Window > Tutorials* menu item.

## [0.1.6] - 2018-12-06
### Fixed
- Fix AudioClip import errors.
- Fix compilation errors at build time due to incorrectly configured Assembly Definition asset.
- Fix inconsistent line endings.
- Fix CS0649 warnings.
- Fix *Help > Template Walkthroughs* menu item not finding Readme asset.

## [0.1.5] - 2018-12-04
### Fixed
- Fixed ReflectionTypeLoadException when inspecting TutorialPage
- Fixed GUI layout errors when starting tutorial from Readme asset

## [0.1.4] - 2018-12-03
### Added
- Integrated the readme asset with the Tutorials
- Ability to have more than a single Tutorial in a project
- Propper flow for users to go into and out of a tutorial
- Ability to add Images, Video to a tutorial
- New Color type added to PropertyModificaitonCriterion
- PropertyModificationCriterion has a new mode where it will complete if the user changes a property to a different value than initial
- Added option to the masking system to prevent interactions to the unmasked area
- New Criterions: FrameSelectedCriterion, MaterialPropertyChanged, ActiveToolCriterion, SceneCameraViewMovedCritertion
- Ability for Tutorials to reference each other
- "Home/Skip" button have 2 modes. Legacy will open the Hub, and CloseWindow will close the Tutorial window
- Ability to choose the name of the Tutorial Window

### Fixed
- Updated usages of obsolete APIs

### Changed
- Initial version of the in editor tutorial framework as a package.
- Contained the use of internals to a single folder.
