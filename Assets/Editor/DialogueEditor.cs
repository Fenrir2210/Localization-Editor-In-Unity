using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(DialogueLists))]
public class DialogueEditor : Editor
{
    SerializedProperty ThisList;
    SerializedProperty DictionaryAssetFile;
    SerializedProperty LanguageList;
    SerializedObject GetTarget;
    DialogueLists t;
    DialogueLists y;

    //We need to bring in our data from TOOLS 2 and populate a popup so dialogue keys are EZ to choose from
    int index;
    string[] _options;
    List<string> _options2 = new List<string>();

    void OnEnable()
    {
        t = (DialogueLists)target;
        GetTarget = new SerializedObject(t);
        ThisList = GetTarget.FindProperty("DialogueList"); // Find the List in our script and create a reference of it
        DictionaryAssetFile = GetTarget.FindProperty("LocalizedData"); //The localized data from TOOLS 2

        //Set up another Object for our LocalizedData.
        y = (DialogueLists)target;
        SerializedObject o = new SerializedObject (y.LocalizedData);

        LanguageList = o.FindProperty("LanguageList");   //<---- HERE I am updating the SerializedProperty to be our language list found in the LocalizedData of type Dictionary

        var keyValuePairsProperty = LanguageList.GetArrayElementAtIndex(0).FindPropertyRelative("KeyValuePairs"); //Get the keyValue list of ENGLISH localization data

        _options = new string[keyValuePairsProperty.arraySize];

        for (int i = 0; i < keyValuePairsProperty.arraySize; ++i)
        {
            var itemProperty = keyValuePairsProperty.GetArrayElementAtIndex(i);
            var keyProperty = itemProperty.FindPropertyRelative("Key");
            //var valueProperty = itemProperty.FindPropertyRelative("Value");

            _options[i] = keyProperty.stringValue; //Sets up our _options list, populated at each index with a key from our localized data contained in ENGLISH
            _options2.Insert(i, keyProperty.stringValue); //Sets up our _options2 list
        }

    }

    //Custom Inspector Window For our Dialogue ScriptableObject
    bool isExpanded = true;
    public override void OnInspectorGUI()
    {
        GetTarget.Update(); //Call to prepare for editing

        EditorGUILayout.ObjectField(DictionaryAssetFile, GUIContent.none);

        GUI.backgroundColor = Color.green;

        if (GUILayout.Button("Add Dialogue", GUILayout.MaxWidth(130), GUILayout.MaxHeight(20)))
        {
            t.DialogueList.Add(new ListContainer2());
        }

        GUI.backgroundColor = Color.white;

        for (int i = 0; i < ThisList.arraySize; i++)
        {
            SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);

            //Dialog Stuff
            SerializedProperty MyDialogBox = MyListRef.FindPropertyRelative("DialogueBoxName");
            SerializedProperty MyDialogueContents = MyListRef.FindPropertyRelative("DialogueContents");
            SerializedProperty MyDialogueOptions = MyListRef.FindPropertyRelative("DialogueOptions"); //List of dialogue objects

            EditorGUILayout.BeginHorizontal(); //Begin Horizontal 
            EditorGUILayout.PropertyField(MyDialogBox, GUIContent.none); //Prints language to editor.
            EditorGUI.indentLevel += 1;

            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("+", GUILayout.MaxWidth(80), GUILayout.MaxHeight(20)) && MyDialogueOptions.arraySize < 3) //button to add a new key/val pair inside a lang list
            {
                MyDialogueOptions.InsertArrayElementAtIndex(MyDialogueOptions.arraySize);
                MyDialogueOptions.GetArrayElementAtIndex(MyDialogueOptions.arraySize - 1);
            }

            GUI.backgroundColor = Color.red;

            if (GUILayout.Button("-", GUILayout.MaxWidth(80), GUILayout.MaxHeight(20)) && MyDialogueOptions.arraySize != 0) //button to remove a new key/val pair inside a lang list
            {
                MyDialogueOptions.DeleteArrayElementAtIndex(MyDialogueOptions.arraySize - 1);
                MyDialogueOptions.GetArrayElementAtIndex(MyDialogueOptions.arraySize - 1);
            }

            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal(); //End Horizontal

            EditorGUI.indentLevel -= 1;

            EditorGUILayout.PropertyField(MyDialogueContents, GUIContent.none); //Contents of the text being read by the player

            EditorGUI.indentLevel += 1;

            //popup\\
            isExpanded = EditorGUILayout.Foldout(isExpanded, "Keys/GoTos");
            if(isExpanded)
            {
                EditorGUI.indentLevel += 1;
                for (int j = 0; j < MyDialogueOptions.arraySize; j++)
                {
                    SerializedProperty MyDialogueRef = MyDialogueOptions.GetArrayElementAtIndex(j); //reference to each pair of values in the list
                    SerializedProperty MyKey = MyDialogueRef.FindPropertyRelative("Key");
                    SerializedProperty MyGoTo = MyDialogueRef.FindPropertyRelative("DialogueToGoTo");

                    //index needs a way to be unique for each MyKey in MyDialogueOptions. Right now, it will always be whatever was selected last for ALL dropdowns.

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Key"); //label

                    index = _options2.IndexOf(MyKey.stringValue);
                    int newIndex = EditorGUILayout.Popup(index, _options2.ToArray());

                    if (newIndex != index)
                        MyKey.stringValue = _options2[newIndex];

                    GUILayout.Label("Go To: "); //label
                    EditorGUILayout.PropertyField(MyGoTo, GUIContent.none);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel -= 1;
            }
            //end popup stuff\\

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


//D'oh...My inability to get these inner properties the first time around is, I think, an exposure of a still existing lack of confidence in all this scriptableObject stuff.
//Im gonna keep this block around as a reference, and as an example to stepping through complex data types, while appropriating its function for use on my "English" list only. Keepin' it simple
//with the popup in the editor and the later dialogueManager this way. 

//I think to extend the functionality over other languages I could get my language property and print it in that "LANGUAGE/" format you told me about. This would
//seperate all the keys into menus by their language. However, because that would require some refactoring for the translator, and the intention of the assignment
//is to demonstrate a working dialogue editor, I will leave that as a hypothetical for now.

//for (int l = 0; l < LanguageList.arraySize; ++l)
//{
//    var languageListElementProperty = LanguageList.GetArrayElementAtIndex(l);

//    var languageNameProperty = languageListElementProperty.FindPropertyRelative("Language");
//    var keyValuePairsProperty = languageListElementProperty.FindPropertyRelative("KeyValuePairs");
//    for (int i = 0; i < keyValuePairsProperty.arraySize; ++i)
//    {
//        var itemProperty = keyValuePairsProperty.GetArrayElementAtIndex(i);
//        var keyProperty = itemProperty.FindPropertyRelative("Key");
//        var valueProperty = itemProperty.FindPropertyRelative("Value");
//        Debug.LogFormat("{0}: {1}", keyProperty.stringValue, valueProperty.stringValue);
//    }
//}