using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//Dictionary defines the structure of individual dictionary entries + the language they belong to
[Serializable]
public class DictionaryStruct
{
    public string Key; //This key will be used as a lookup for the value. We will attach the asset file with a localizationManager. The manager will look for THIS key, compare with textboxKey, and return.
    [Multiline(lines: 2)]
    public string Value; //This will be dialogue for the .asset file, in the language defined by user in field Language (name of the .asset file).

}