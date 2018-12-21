using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Dictionary : ScriptableObject
{
    //I want THIS list
    public List<ListContainer> LanguageList = new List<ListContainer>(); //list of type DictionaryStruct, where each member of the list contains a key/value pair (KEY/APPROPRIATE TRANSLATION)   
}

//Wrapper class for our list of lists 
[Serializable]
public class ListContainer
{
    public string Language;
    public List<DictionaryStruct> KeyValuePairs = new List<DictionaryStruct>();
}

