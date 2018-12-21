using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dictionary))]
public class LocalizationWindow : Editor
{
    SerializedProperty ThisList;
    SerializedObject GetTarget;
    Dictionary t;

    void OnEnable()
    {
        t = (Dictionary)target;
        GetTarget = new SerializedObject(t); //SerializedObject Dictionary
        ThisList = GetTarget.FindProperty("LanguageList"); //SerializedProperty LanguageList
    }

    //Custom Inspector Window For our Dictionary ScriptableObject
    public override void OnInspectorGUI()
    {
        GetTarget.Update(); //Call to prepare for editing

        //Display list and all child lists. Pretty convenient in the inspector with our list of lists.

        GUI.backgroundColor = Color.green;

        if (GUILayout.Button("Add Language", GUILayout.MaxWidth(130), GUILayout.MaxHeight(20)))
        {
            t.LanguageList.Add(new ListContainer());
        }

        GUI.backgroundColor = Color.white;

        for (int i = 0; i < ThisList.arraySize; i++)
        {
            SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);
            SerializedProperty MyLang = MyListRef.FindPropertyRelative("Language");
            SerializedProperty MyKeyValPairs = MyListRef.FindPropertyRelative("KeyValuePairs");

            EditorGUILayout.BeginHorizontal(); //Begin Horizontal 
            EditorGUILayout.PropertyField(MyLang, GUIContent.none); //Prints language to editor.
            EditorGUI.indentLevel += 1;

            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("+", GUILayout.MaxWidth(80), GUILayout.MaxHeight(20))) //button to add a new key/val pair inside a lang list
            {
                MyKeyValPairs.InsertArrayElementAtIndex(MyKeyValPairs.arraySize);
                MyKeyValPairs.GetArrayElementAtIndex(MyKeyValPairs.arraySize - 1);
            }

            GUI.backgroundColor = Color.red;

            if (GUILayout.Button("-", GUILayout.MaxWidth(80), GUILayout.MaxHeight(20)) && MyKeyValPairs.arraySize != 0) //button to remove a new key/val pair inside a lang list
            {
                MyKeyValPairs.DeleteArrayElementAtIndex(MyKeyValPairs.arraySize - 1);
                MyKeyValPairs.GetArrayElementAtIndex(MyKeyValPairs.arraySize - 1);
            }

            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal(); //End Horizontal

            EditorGUILayout.PropertyField(MyKeyValPairs, GUIContent.none, true); //Displays the keyvalpair lists and the key/vals inside

            EditorGUI.indentLevel -= 1;

            EditorGUILayout.Space(); EditorGUILayout.Space(); EditorGUILayout.Space();

            GUI.backgroundColor = Color.red;

            if (GUILayout.Button("Remove Index (" + i.ToString() + ")", EditorStyles.miniButtonMid))
            {
                ThisList.DeleteArrayElementAtIndex(i);
            }

            GUI.backgroundColor = Color.white;

            EditorGUILayout.Space(); EditorGUILayout.Space(); EditorGUILayout.Space();
        }
        GetTarget.ApplyModifiedProperties(); //Save changes made in editor
    }
}