using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class PostProcess : IPostprocessBuildWithReport, IPreprocessBuildWithReport
{
    public int callbackOrder { get { return int.MaxValue; } }

    public void OnPreprocessBuild(BuildReport report)
    {
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        string buildPath = report.summary.outputPath;
        UpdateProject(buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj");
        UpdateProjectPlist(buildPath + "/Info.plist");
        UpdateCapabilities(buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj", "metalslug.entitlements");
    }

    void UpdateProject(string projectPath)
    {
#if UNITY_IOS
        PBXProject project = new PBXProject();
        project.ReadFromString(File.ReadAllText(projectPath));

        string mainTargetId = project.GetUnityMainTargetGuid();
        string frameworkTargetId = project.GetUnityFrameworkTargetGuid();

        project.SetBuildProperty(frameworkTargetId, "ENABLE_BITCODE", "NO");
        project.AddFrameworkToProject(mainTargetId, "UserNotifications.framework", false);

        File.WriteAllText(projectPath, project.WriteToString());
#endif
    }

    void UpdateProjectPlist(string plistPath)
    {
#if UNITY_IOS
        PlistDocument plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));

        PlistElementDict rootDict = plist.root;
        rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);
        rootDict.SetString("gad_preferred_webview", "wkwebview");
        rootDict.SetString("NSUserTrackingUsageDescription", "Your data will be used to provide you a better and personalized ad experience.");
        File.WriteAllText(plistPath, plist.WriteToString());
#endif
    }

    void UpdateCapabilities(string buildPath, string entitlementPath)
    {
#if UNITY_IOS
        var pcm = new ProjectCapabilityManager(buildPath, entitlementPath, "Unity-iPhone");
        pcm.AddPushNotifications(false);
        pcm.AddInAppPurchase();
        pcm.WriteToFile();
#endif
    }
}