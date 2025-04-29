/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Dynamic Inventory 
* FILE NAME       : ItemDataLoader.cs
* DESCRIPTION     : The data loader for inventory items
*                    
* REVISION HISTORY:
* Date            Author                  Comments
* ---------------------------------------------------------------------------
* 2005/04/05      Akram Taghavi-Burris      Created Class
* 
*
/******************************************************************/

using UnityEngine;
using GameSystems.Shared.BaseClasses;

namespace GameSystems.InventorySystem.Items
{
    public class ItemDataLoader : DataLoader<ItemData>
    {
        [SerializeField] private Sprite _defaultSprite;

        // Override if you need special logic when creating ItemData from fields
        protected override ItemData CreateDataFromFields(string[] fields)
        {
            ItemData itemData = base.CreateDataFromFields(fields);

            // Additional custom logic for ItemData (e.g., validation, defaults, transformation)
            if (itemData != null)
            {
                //Unity's Resources system is a special system that lets you load assets dynamically at runtime, but it only works with assets that are placed in the Resources folder.

                Debug.Log(itemData.Icon);
                if (itemData.Icon == null)
                {
                    itemData.Icon = _defaultSprite; //set default sprite icon
                } //end if(icon)
                else
                {
                    Debug.Log(itemData.Icon);
                }

            } //end if(itemData)

            return itemData;
        } //end CreateDataFromFields()


        //Method for loading data in editor
        [ContextMenu("Load Data from CSV")]
        public void LoadDataInEditor()
        {
            //Must call a non-generic method
            LoadData();

        } //end LoadDataInEditor()

    }//end ItemDataLoader
    
}//end Namespace