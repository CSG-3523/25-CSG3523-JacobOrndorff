/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Dynamic Inventory 
* FILE NAME       : ItemData.cs
* DESCRIPTION     : ScriptableObject for inventory items.
*                    
* REVISION HISTORY:
* Date             Author                    Comments
* ---------------------------------------------------------------------------
* 2005/04/05      Akram Taghavi-Burris      Created Class
* 
*
/******************************************************************/

using UnityEditor;
using UnityEngine;
using GameSystems.Shared.Interfaces;

namespace GameSystems.InventorySystem.Items
{
    //Item Type, used for categorizing items in inventory UI
    public enum ItemTypes
    {
        Resource,
        Tool,
        Food,
        Weapon,
        Special
    }

    [CreateAssetMenu(fileName = "ItemData", menuName = "ItemData/New ItemData")]
    public class ItemData : ScriptableObject, IParsable
    {
        private bool _isDataSaved = false;
        public string Name;

        [TextArea(3, 10)] public string Description;

        public ItemTypes ItemType;

        public Sprite Icon;

        public int MaxStackSize;
        
        
        // Parses fields from a CSV line and assigns them to this item
        public void Parse(string[] fields)
        {
            _isDataSaved = bool.Parse(fields[0]); // Parse _isDataSaved
            Name = fields[1]; // Name
            Description = fields[2]; // Description
            ItemType = (ItemTypes)System.Enum.Parse(typeof(ItemTypes), fields[3]); // Parse ItemType enum
            Icon = Resources.Load<Sprite>(fields[4]); // Load Sprite by name from Resources folder
            MaxStackSize = int.Parse(fields[5]); // Parse maxStackSize

        } //end Parse()


        //Force the asset and data Name to be the same, using OnValidate()
        //OnValidate runs whenever there is a change in the Editor
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(Name))
            {
                //Set data Name to the name of the asset
                Name = this.name;
            }
            else
            {
                //set asset name to the name of the data
#if UNITY_EDITOR
                //Get the file name of the asset
                string path = AssetDatabase.GetAssetPath(this);
                string currentName = System.IO.Path.GetFileNameWithoutExtension(path);

                if (currentName != Name)
                {
                    AssetDatabase.RenameAsset(path, Name);
                    AssetDatabase.SaveAssets();
                }
#endif
            } //end if

        } //end OnValidate()

    }//end ItemData
    
}//end Namespace