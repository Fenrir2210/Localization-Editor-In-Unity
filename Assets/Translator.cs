using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Translator : MonoBehaviour {
    //Not sure why im proud of this one. Its a simple dictionary really, but the data structure exposes everything in our Inspector and keep all key/value pairs nested inside language lists.

    //We need to be able to pass our Translator a language file, before we can find the appropriate key/search for its value
    public Dictionary DictionaryAssetFile;

    public Text text;

    private void Start()
    {
        //These two lines fetch the text from the field we attach this script to
        text = GetComponent<Text>(); // TITLE_OF_GAME text.text

        List<DictionaryStruct> keyValPairs = Find(DictionaryAssetFile); //Search our attached language file for the set of key/value pairs belonging to our current syslanguage 
        var listAsArray = keyValPairs.ToArray();
        
        for(int i = 0; i < listAsArray.Length; i++)
        {
            if (listAsArray[i].Key.Equals(text.text.ToString()))
            {
                text.text = listAsArray[i].Value;
            }
        }
    }

    private void Update()
    {
        text = GetComponent<Text>(); // TITLE_OF_GAME text.text

        List<DictionaryStruct> keyValPairs = Find(DictionaryAssetFile); //Search our attached language file for the set of key/value pairs belonging to our current syslanguage 
        var listAsArray = keyValPairs.ToArray();

        for (int i = 0; i < listAsArray.Length; i++)
        {
            if (listAsArray[i].Key.Equals(text.text.ToString()))
            {
                text.text = listAsArray[i].Value;
            }
        }
    }

    //function for finding the outer list. Outer list is defined by string indicating name of language. 
    //Returns List<DictionaryStruct> object, which contains all our key/value pairs in the language matching sysLang 
    public List<DictionaryStruct> Find(Dictionary langList)
    {
        string sysLang = Application.systemLanguage.ToString(); //English
        var langs = langList.LanguageList.ToArray();

        //langList.LanguageList.Count
        for (int i = 0; i < langs.Length; i++) //For each language in the language list
        {
            if(langs[i].Language.Equals(sysLang)) //if the language of our inner list matches the language of our system...
            {
                return langs[i].KeyValuePairs;//return inner list; search this inner list for the key that matches textbox text
            }
        }

        return null;
    }
}
