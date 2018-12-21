using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Example))]
public class ExampleEditor : Editor
{
    // ---------------------------------------------------
    // this is just a placeholder, you need to change this to fit your project
    // by taking the data from your database
    class ItemData
    {
        public int id;
        public string name;
    }

    List<ItemData> database = new List<ItemData>()
     {
         new ItemData() { id = -1, name = "(Unset)" },   // might want to keep an invalid id for unset items
         new ItemData() { id = 1, name = "Small Healing Potion" },
         new ItemData() { id = 2, name = "Medium Healing Potion" },
         new ItemData() { id = 3, name = "Large Healing Potion" },
     };


    // ---------------------------------------------------
    // prepare a list of item names to use with Popup();
    // Might be more efficient to do this once somewhere than
    // rebuild it each time the component gets selected
    string[] itemNames;
    void OnEnable()
    {
        itemNames = new string[database.Count];
        for (int i = 0; i < database.Count; i++)
            itemNames[i] = database[i].name;
    }


    // ---------------------------------------------------
    // Do the actual drawing
    bool listExpanded = true;
    public override void OnInspectorGUI()
    {

        SerializedProperty materialIDs = serializedObject.FindProperty("materialIDs");

        listExpanded = EditorGUILayout.Foldout(listExpanded, "Required material IDs");
        if (listExpanded)
        {
            materialIDs.arraySize = EditorGUILayout.IntField("Size", materialIDs.arraySize);
            for (int i = 0; i < materialIDs.arraySize; i++)
            {
                // get serialized item id from component
                int itemID = materialIDs.GetArrayElementAtIndex(i).intValue;

                // assuming the array itemNames is in the same order as database, we can do this
                int index = database.FindIndex(item => item.id == itemID);

                // draw the combo box / popup / pulldown / whatchamacallit
                index = EditorGUILayout.Popup("Element " + i, index, itemNames);

                // now work backwards to update the property
                itemID = database[index].id;
                materialIDs.GetArrayElementAtIndex(i).intValue = itemID;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}