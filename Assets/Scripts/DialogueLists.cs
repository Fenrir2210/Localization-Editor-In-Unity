using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create/New Dialogue")]
[Serializable]
public class DialogueLists : ScriptableObject
{
    public Dictionary LocalizedData;
    public List<ListContainer2> DialogueList = new List<ListContainer2>(); //list of type DictionaryStruct, where each member of the list contains a key/value pair (KEY/APPROPRIATE TRANSLATION)   
}

//Wrapper class for our list of lists 
[Serializable]
public class ListContainer2
{
    public string DialogueBoxName;
    [TextArea(3, 7)]
    public string DialogueContents;
    public List<Dialogue> DialogueOptions = new List<Dialogue>();
}

