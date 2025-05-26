using UnityEditor;
using UnityEngine;
namespace ET.Saveload
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SaveLoadManager))]
    public class SaveLoadManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            SaveLoadManager myScript = (SaveLoadManager)target;
            if (GUILayout.Button("CleanData"))
            {
                myScript.CleanData();
            }
        }
    }
#endif
}