using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ET.ETool.ETAssetNameModifierSP;

namespace ET.ETool
{
#if UNITY_EDITOR
    public class ETAssetNameModifier : EditorWindow
    {
        [SerializeField]
        private DefaultAsset folderAsset; // Use DefaultAsset to accept folders
    
        [SerializeField] private List<string> fileTypes = new List<string> { "jpg", "png" }; // List of accepted file extensions
        [SerializeField] private string curInputFileType;

        [SerializeField]
        private string prefix; // List of Prefix
    
        private float progress = 0f; // Progress bar variable
    
        [MenuItem("ETools/Asset Modifer/ET Asset Name Modifier")]
        public static void ShowWindow()
        {
            GetWindow<ETAssetNameModifier>("ET Asset Name Modifier");
        }
    
        private void OnGUI()
        {
            GUILayout.Label("Rename Images in Folder", EditorStyles.boldLabel);
    
            folderAsset = EditorGUILayout.ObjectField("Select Folder:", folderAsset, typeof(DefaultAsset), false) as DefaultAsset;


            #region list file type
            GUILayout.Label("File types", EditorStyles.boldLabel);

            // Display existing strings in the list
            for (int i = 0; i < fileTypes.Count; i++)
            {
                GUILayout.BeginHorizontal();
                fileTypes[i] = EditorGUILayout.TextField("String " + i + ":", fileTypes[i]);

                // Add a button to remove the string from the list
                if (GUILayout.Button("-"))
                {
                    fileTypes.RemoveAt(i);
                    i--; // Decrement the index to account for the removed item
                }
                GUILayout.EndHorizontal();
            }

            // Input field for adding new strings
            GUILayout.BeginHorizontal();
            curInputFileType = EditorGUILayout.TextField("New File Type:", curInputFileType);

            // Add button to add the new string to the list
            if (GUILayout.Button("Add"))
            {
                fileTypes.Add(curInputFileType);
                curInputFileType = "";
            }
            GUILayout.EndHorizontal();
            #endregion
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Prefix:");
            string prefixString = EditorGUILayout.TextField(prefix);
            prefix = prefixString;
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Add Prefix"))
            {
                if (folderAsset != null)
                {
                    string folderPath = AssetDatabase.GetAssetPath(folderAsset);
                    RenameAllFileInFolder(folderPath, RenamingType.AddPreFix);
                }
                else
                {
                    Debug.LogError("Please select a folder.");
                }
            }
            if (GUILayout.Button("Remove Prefix"))
            {
                if (folderAsset != null)
                {
                    string folderPath = AssetDatabase.GetAssetPath(folderAsset);
                    RenameAllFileInFolder(folderPath, RenamingType.RemovePreFix);
                }
                else
                {
                    Debug.LogError("Please select a folder.");
                }
            }
            if (GUILayout.Button("To Camel Style"))
            {
                if (folderAsset != null)
                {
                    string folderPath = AssetDatabase.GetAssetPath(folderAsset);
                    RenameAllFileInFolder(folderPath, RenamingType.ToCamelStyle);
                }
                else
                {
                    Debug.LogError("Please select a folder.");
                }
            }
            if (GUILayout.Button("To Snake Style"))
            {
                if (folderAsset != null)
                {
                    string folderPath = AssetDatabase.GetAssetPath(folderAsset);
                    RenameAllFileInFolder(folderPath, RenamingType.ToSnakeStyle);
                }
                else
                {
                    Debug.LogError("Please select a folder.");
                }
            }



        }
        public void RenameAllFileInFolder(string folderPath, RenamingType renamingType)
        {
    
            DirectoryInfo directory = new DirectoryInfo(folderPath);
    
            if (directory.Exists)
            {
                float totalFiles = EFile.FilesCount(directory, fileTypes);
                float currentFile = 0;
                foreach (string fileType in fileTypes)
                {
                    FileInfo[] imageFiles = directory.GetFiles("*." + fileType);
                    foreach (FileInfo file in imageFiles)
                    {
                        string newFilePath = "";
                        string newFileName = "";
                        bool gotNewName = true;
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);
                        newFileName = fileNameWithoutExtension;
                        string fileExtension = file.Extension;
                        switch (renamingType)
                        {
                            case RenamingType.AddPreFix:
                                if (string.IsNullOrEmpty(prefix))
                                {
                                    gotNewName = false;
                                }
                                else
                                {
                                    newFileName = prefix + fileNameWithoutExtension;
                                }
                                break;
                            case RenamingType.RemovePreFix:
                                if (string.IsNullOrEmpty(prefix))
                                {
                                    gotNewName = false;
                                }
                                else
                                {
                                    newFileName = fileNameWithoutExtension.RemovePrefix(prefix);
                                }
                                break;
                            case RenamingType.ToCamelStyle:
                                newFileName = fileNameWithoutExtension.ToCamelStyle();
                                break;
                            case RenamingType.ToSnakeStyle:
                                newFileName = fileNameWithoutExtension.ToSnakeStyle();
                                break;
                            default:
                                gotNewName = false;
                                break;
                        }
                        if (gotNewName)
                        {
                            newFileName += fileExtension;
                            newFilePath = Path.Combine(file.Directory.FullName, newFileName);
                            EFile.Rename(file, newFilePath);
                            currentFile++;
                            progress = currentFile / totalFiles;
                            EditorUtility.DisplayProgressBar("Renaming Images", "Processing...", progress);
                        }
                        else
                        {
    
                        }
                    }
                }
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("Directory not found: " + folderPath);
            }
            // Clear the progress bar when finished
            EditorUtility.ClearProgressBar();
        }
        public enum RenamingType
        {
            AddPreFix,
            RemovePreFix,
            AddSubFix,
            RemoveSubFix,
            ToCamelStyle,
            ToSnakeStyle,
        }
    }
#endif
}
/// <summary>
/// ET support editor window
/// </summary>