using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DialogueManager : MonoBehaviour {

    public DialogueLists dialogue;
    public Text dialogueContent;
    public Button response1;
    public Button response2;
    public Button response3;
    public Dialogue[] dialogueOptions;

    private bool isDirty;
    private string key;
    private string currentDialogue;
    private string nextDialogue;

    // Use this for initialization
    void Start () {

        var dialogueLists = dialogue.DialogueList.ToArray(); //ListContainer2; [0] is dialog box at top of editor
        currentDialogue = dialogueLists[0].DialogueBoxName; //DialogBox1

        List <Dialogue> keyGoToPairs = Find(dialogue);  //get content from dialogueLists and update text to reflect that value
        dialogueOptions = keyGoToPairs.ToArray(); //express the keygoto list as an array. Each index contains a ref to a key and a go to (this is DIALOGUE OPTIONS List<Dialogue>)

        dialogueContent.text = dialogueLists[0].DialogueContents; //Initializes the player-read textbox contents to the beginning window in the provided dialogue file (Dialog1)


        //Create a list of buttons
        //We initially deactivate all the buttons and render them in only if we have an option for that button to take on.
        response1.gameObject.SetActive(false); response2.gameObject.SetActive(false); response3.gameObject.SetActive(false);
        List<Button> buttonList = new List<Button>()
        {
            response1, response2, response3
        };

        for(int i = 0; i < dialogueOptions.Length; i++)
        {
            buttonList[i].gameObject.SetActive(true);
            buttonList[i].GetComponentInChildren<Text>().text = dialogueOptions[i].Key;
        }

        
    }

    // Update is called once per frame
    void Update () {

        if (isDirty)
        {
            var dialogueLists = dialogue.DialogueList.ToArray(); //ListContainer2; [0] is dialog box at top of editor

            for (int i = 0; i < dialogueLists.Length; i++)
            {
                if(dialogueLists[i].DialogueBoxName == currentDialogue) //when we find "dialogBox1" for example..
                {
                    //at this index...
                    for (int l = 0; l < dialogueOptions.Length; l++)
                    {
                        if(dialogueOptions[l].Key == key) //If we find the key
                        {
                            nextDialogue = dialogueOptions[l].DialogueToGoTo; //we save the value of the dialogue we want to go to in var keyGoTo: eg "Dialog2"
                        }
                    }
                }
            }

            //In the second part of this "if Update", we utilize the keyGoTo and load the appropriate OUTER LIST, redrawing the dialogue box as we did in Start() but using this new data source

            for (int i = 0; i < dialogueLists.Length; i++) //searching ListContainer2
            {
                if (dialogueLists[i].DialogueBoxName == nextDialogue) //when we find "dialogBox2" for example
                {
                    //at this index..
                    dialogueContent.text = dialogueLists[i].DialogueContents; //Initializes the player-read textbox contents to the beginning window in the provided dialogue file (Dialog1)
                    dialogueOptions = dialogueLists[i].DialogueOptions.ToArray(); //use a new data set for dialogueOptions as determined by keyGoTo
                }
            }

            response1.gameObject.SetActive(false); response2.gameObject.SetActive(false); response3.gameObject.SetActive(false);
            List<Button> buttonList = new List<Button>()
            {
                response1, response2, response3
            };

            for (int i = 0; i < dialogueOptions.Length; i++)
            {
                buttonList[i].gameObject.SetActive(true);
                buttonList[i].GetComponentInChildren<Text>().text = dialogueOptions[i].Key;
            }
        }//end of if
        isDirty = false;
    }

    public void OnClick()
    {
        var textValue = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text; // this gets our text from our button. 
        var outerLangList = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Translator>().DictionaryAssetFile.LanguageList.ToArray(); //This gets the langList from translator
        var keyValPairs = outerLangList[0].KeyValuePairs.ToArray();// this turns the English list into an array of keyVal pairs

        for (int i = 0; i < keyValPairs.Length; i++) //for each keyValue pair
        {
            if (keyValPairs[i].Value == textValue) //if the value of the text box is found in our inner list
            {
                key = keyValPairs[i].Key; //we return its sibling. We want to use THIS key to search our inner dialogueList and return sibling DialogToGoTo
                isDirty = true;
            }
        }
    }

    //function for finding the outer list. Outer list is defined by string indicating name of the dialogue box
    //Returns List<Dialogue> object, which contains all our key/goto pairs 
    public List<Dialogue> Find(DialogueLists outerList)
    {
        var dialogueBoxes = outerList.DialogueList.ToArray(); //DialogueList in array format; each DialogueList is ListContainer2

        for (int i = 0; i < dialogueBoxes.Length; i++) //For each dialogBoxName in the list
        {
            return dialogueBoxes[i].DialogueOptions;//return inner list; search this inner list for the key that matches textbox text
        }

        return null;
    }
}
