# In Editor Interactive Tutorials 
---------
This package is needed to have In Editor Tutorials working.

**This package relies on Unity internals and is not available on production**

## Setup
Follow those steps to add support to in Editor tutorials to your project/template:

- Add `com.unity.learn.iet-framework` to your `dependencies` list to your project `manifest.json`
- To add authoring tools check the readme of [com.unity.learn.iet-framework.authoring](../com.unity.learn.iet-framework.authoring)

Example:

    {
        "dependencies": {
            "com.unity.learn.iet-framework": "0.1.6-preview"
        }
    }
Make sure to use the latest version available of the package.
