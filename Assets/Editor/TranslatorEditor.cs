using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Translator))]
public class TranslatorEditor : Editor
{
    int index;
    //string[] _options = new[] { "MENU/TITLE_OF_GAME", "MENU/TEXT_ON_BUTTON", "GAME/HERO_NAME", "GAME/PLAYER_NAME" };
    //string[] _options2 = new string[4];

    //So ive left both options arrays in. Im using _options2 because it is directly populated by the key elements in our language list. It will update even when then 
    //the editor changes the language data. (Ive left the array size at 4 for demonstration purposes)

    //_option is used to demonstrate that neat trick you told me about, adding the '/'s in. This is a static hardcore list which works all the same. You can use it by changing all instances of
    //_options2 to _options

    //The only weird chink in the armor here is that, because the index is defaulted to 0, the pop menu will revert to TITLE_OF_GAME by default when you select the component
    //because it rests at that index. So set the key, click off, and run it.
    //You are able to easily edit the keys from a popup menu and view the translations in runtime should you choose to. The values persist after clicking off, so everything saves
    //properly and all that. Hopefully you dig it. :)

    public override void OnInspectorGUI()
    {
        //Draw default field. We only need to add a dropdown for key selection, the rest stays the same
        DrawDefaultInspector();

        //Begin horizontal field so the label and popup share an axis.
        EditorGUILayout.BeginHorizontal();

        //GUILayout.Label("Text Key"); //label
        //index = EditorGUILayout.Popup(index, _options2); //popup displaying _options array. User chooses an option from the list.

        EditorGUILayout.EndHorizontal();

        //Update the selected option on the underlying instance of SomeClass
        var translator = target as Translator;
        var options = translator.DictionaryAssetFile;
        List<DictionaryStruct> keyValPairs = translator.Find(options);
        var listAsArray = keyValPairs.ToArray();

        //for (int i = 0; i < listAsArray.Length; i++)
        //{
        //    _options2[i] = "MENU/" + listAsArray[i].Key;
        //}

        //translator.text.text = _options2[index].Substring(5, _options2[index].Length - 5); for tools 3, we dont want this to pick our keys; the dialogue editor/manager handle what text goes where
        EditorUtility.SetDirty(translator);

    }
}
