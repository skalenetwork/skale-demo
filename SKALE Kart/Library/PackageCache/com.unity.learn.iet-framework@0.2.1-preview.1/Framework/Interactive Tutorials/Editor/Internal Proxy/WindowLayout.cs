using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

namespace Unity.InteractiveTutorials
{
    public static class WindowLayoutProxy
    {
        public static void SaveWindowLayout(string path)
        {
            WindowLayout.SaveWindowLayout(path);
        }

        public static string ReplaceAfter(string before, string replaceWithThis, string lineToRead)
        {
            int index = -1;
            index = lineToRead.IndexOf(before, System.StringComparison.Ordinal);
            if (index > -1)
            {
                lineToRead = lineToRead.Substring(0, index + before.Length) + replaceWithThis;
            }
            return lineToRead;
        }

        // TODO Clean up and give a better name
        public static bool RemoveStuff(string filePath)
        {
            var fileContents = new List<string>();
            try
            {
                using(StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    string toReplace = "m_LastProjectPath: ";
                    string replaceWith = Directory.GetCurrentDirectory();
                    while((line = reader.ReadLine()) != null)
                    {
                        line = ReplaceAfter(toReplace, replaceWith, line);
                        line = ReplaceAfter("m_Readme: ", "{fileID: 0}", line);
                        fileContents.Add(line);
                    }
                }

                using(StreamWriter writer = new StreamWriter(filePath, false))
                {
                    for(int i = 0; i < fileContents.Count; i++)
                    {
                        writer.WriteLine(fileContents[i]);
                    }
                }
                return true;
            }
            catch(Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        public static bool ReplaceLastProjectPathWithCurrentProjectPath(string layoutPath)
        {
            var success = false;
            //var layout = InternalEditorUtility.LoadSerializedFileAndForget(layoutPath);
            RemoveStuff(layoutPath);
            /*
            foreach(var wnd in layout)
            {
                using(var so = new SerializedObject(wnd))
                {
                    // Having the layout a readme stored is unwanted.

                    var readme = so.FindProperty("m_Readme");
                    if (readme != null)
                    {
                        readme.objectReferenceValue = null;
                        so.ApplyModifiedProperties();
                    }
                    if (!success)
                    {
                        var lastProjectPath = so.FindProperty("m_LastProjectPath");
                        if (lastProjectPath != null)
                        {
                            lastProjectPath.stringValue = System.IO.Directory.GetCurrentDirectory();
                            so.ApplyModifiedProperties();
                            success = true;
                        }
                    }
                }
            }
            */
            if (success)
            {
                //InternalEditorUtility.SaveToSerializedFileAndForget(layout, layoutPath, true);
            }

            return success;
        }
    }
}
