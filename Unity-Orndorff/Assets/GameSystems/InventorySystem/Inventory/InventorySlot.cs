/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Dynamic Inventory
* FILE NAME       : InventorySlot.cs
* DESCRIPTION     : Behaviors for each inventory slot
*
* REVISION HISTORY:
* Date             Author                    Comments
* ---------------------------------------------------------------------------
* 2005/04/15      Akram Taghavi-Burris      Created Class
*
*
/******************************************************************/

using UnityEngine;
using GameSystems.InventorySystem.Items;

namespace GameSystems.InventorySystem.Inventory
{
    [System.Serializable]
    public class InventorySlot
    {
        
        [SerializeField] private ItemData _itemData; // Reference to the ItemData
        [SerializeField] private int _slotQuantity;  //Quantity of items in this slot
        
        
        // Public access to slot data and quantity
        public int SlotQuantity => _slotQuantity;
        public ItemData ItemData => _itemData;
        
        
        //Constructor method for creating slot
        public InventorySlot(ItemData itemData, int pickupQuantity = 1)
        {
            _itemData = itemData;
            _slotQuantity = pickupQuantity;
            
        }//end InventorySlot()
        
        //Add picked up quantity to slot 
        public void AddQuantity(int pickupQuantity)
        {
            _slotQuantity += pickupQuantity;
            
            if (_slotQuantity > _itemData.MaxStackSize)
            {
                _slotQuantity = _itemData.MaxStackSize;
            }
            
        }//end AddQuantity()
        
        
        //Remove used quantity from slot
        public void RemoveQuantity(int usedQuantity)
        {
            _slotQuantity -= usedQuantity;
            
            if (_slotQuantity < 0) _slotQuantity = 0;
            
        }//end RemoveQuantity
        
        
    } //end InventorySlot
    
}//end Namespace