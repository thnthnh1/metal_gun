using UnityEditor;
using UnityEngine;
namespace ET.Saveload
{
#if UNITY_EDITOR
    [CustomEditor(typeof(S))]
    public class SEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            S myScript = (S)target;
            if (GUILayout.Button("CleanData"))
            {
                myScript.CleanData();
            }
        }
    }
#endif
}