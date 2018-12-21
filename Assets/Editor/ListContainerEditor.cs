using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ListContainer))]
public class ListContainerEditor : Editor
{
    SerializedProperty ThisList;
    SerializedObject GetTarget;
    ListContainer t;

    void OnEnable()
    {
        //t = (ListContainer)target;
        //GetTarget = new SerializedObject(t);
        ThisList = GetTarget.FindProperty("KeyValuePairs"); // Find the List in our script and create a refrence of it
    }

    //Custom Inspector Window For our Dictionary ScriptableObject
    public override void OnInspectorGUI()
    {
        serializedObject.Update(); //Call to prepare for editing
        //EditorGUILayout.PropertyField(ThisList);

        for(int i = 0; i < ThisList.arraySize; i++)
        {
            SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);
            SerializedProperty MyKey = MyListRef.FindPropertyRelative("Key");
            SerializedProperty MyVal = MyListRef.FindPropertyRelative("Value");

            EditorGUILayout.PropertyField(MyKey);
            EditorGUILayout.PropertyField(MyVal);
        }

        serializedObject.ApplyModifiedProperties(); //Save changes made in editor

    }
}