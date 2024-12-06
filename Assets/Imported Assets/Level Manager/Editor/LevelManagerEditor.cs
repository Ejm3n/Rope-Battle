using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    private SerializedProperty _editorMode;
    private LevelManager _levelManager;
    private ReorderableList listLvl;

    private void Awake()
    {
        _levelManager = target as LevelManager;
    }

    private void OnEnable()
    {
        _editorMode = serializedObject.FindProperty("editorMode");
        listLvl = new ReorderableList(serializedObject, serializedObject.FindProperty("Levels"), true, true, true, true);
        listLvl.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = listLvl.serializedProperty.GetArrayElementAtIndex(index);

            EditorGUI.IntField(new Rect(rect.x, rect.y, 30, EditorGUIUtility.singleLineHeight), index + 1);

            if (GUI.Button(new Rect(rect.x + 36, rect.y, 50, EditorGUIUtility.singleLineHeight), new GUIContent("Select")))
            {
                serializedObject.FindProperty("CurrentLevelIndex").intValue = index;
                serializedObject.ApplyModifiedProperties();
                _levelManager.SelectLevel(index);
            }

            EditorGUI.PropertyField(
                new Rect(rect.x + 90, rect.y, 200, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("LevelPrefab"), GUIContent.none);


            if (GUI.Button(new Rect(rect.x + 300, rect.y, 50, EditorGUIUtility.singleLineHeight), new GUIContent("Clear")))
            {
                _levelManager.ClearListAtIndex(index);
            }

            if (GUI.Button(new Rect(rect.x + 347, rect.y, 50, EditorGUIUtility.singleLineHeight), new GUIContent("Delete")))
            {
                _levelManager.Levels.RemoveAt(index);
            }
        };

        listLvl.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Levels");
        };
    }

    public override void OnInspectorGUI()
    {
        _editorMode.boolValue = GUILayout.Toggle(_editorMode.boolValue, new GUIContent("Editor Mode"), GUILayout.Width(100), GUILayout.Height(20));
        serializedObject.ApplyModifiedProperties();
        _levelManager.editorMode = _editorMode.boolValue;
        if (_editorMode.boolValue) DrawSelectedLevel();

        serializedObject.Update();
        listLvl.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Clear Player Prefs", GUILayout.Width(200), GUILayout.Height(20)))
            PlayerPrefs.DeleteAll();
        if (GUILayout.Button("Add 500 Money", GUILayout.Width(200), GUILayout.Height(20)))
        {
            PlayerPrefs.SetString("MoneyCount", (ulong.Parse(PlayerPrefs.GetString("MoneyCount", "0")) + 500L).ToString());
        }
        if (GUILayout.Button("Add 1000000000 Money", GUILayout.Width(200), GUILayout.Height(20)))
        {
            PlayerPrefs.SetString("MoneyCount", (ulong.Parse(PlayerPrefs.GetString("MoneyCount", "0")) + 1000000000L).ToString());
        }
        if (GUILayout.Button("Set Max Money", GUILayout.Width(200), GUILayout.Height(20)))
        {
            PlayerPrefs.SetString("MoneyCount", (999999999999999999L).ToString());
        }
    }

    private void DrawSelectedLevel()
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUI.BeginChangeCheck();
        int index = EditorGUILayout.IntField("Current Level", _levelManager.CurrentLevelIndex + 1);
        if (EditorGUI.EndChangeCheck())
        {
            _levelManager.SelectLevel(index - 1);
            serializedObject.ApplyModifiedProperties();
        }

        if (GUILayout.Button("<<", GUILayout.Width(30), GUILayout.Height(20)))
        {
            _levelManager.PrevLevel();
        }
        if (GUILayout.Button(">>", GUILayout.Width(30), GUILayout.Height(20)))
        {
            _levelManager.NextLevel();
        }

        EditorGUILayout.EndHorizontal();
    }
}