using UnityEngine;
using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(CustomScrollRect), true)]
public class CustomScrollRectEditor : ScrollRectEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (serializedObject.targetObject is CustomScrollRect customScrollRect)
        {
            Type classType = customScrollRect.GetType();
            FieldInfo[] fieldInfos = classType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                FieldInfo fieldInfo = fieldInfos[i];
                object[] attributes = fieldInfo.GetCustomAttributes(true);
                bool isSerializeField = false;
                foreach (var attribute in attributes)
                {
                    if (attribute.GetType() == typeof(SerializeField))
                    {
                        isSerializeField = true;
                        break;
                    }
                }
                if (fieldInfo.Attributes != FieldAttributes.Public && !isSerializeField) continue;
                SerializedProperty serializedProperty = serializedObject.FindProperty(fieldInfo.Name);
                EditorGUILayout.PropertyField(serializedProperty);
            }
        }

        serializedObject.ApplyModifiedProperties();

        // 区切り線
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));

        base.OnInspectorGUI();
    }
}