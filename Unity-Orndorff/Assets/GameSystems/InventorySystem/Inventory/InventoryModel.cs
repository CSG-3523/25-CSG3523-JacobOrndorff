/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Dynamic Inventory
* FILE NAME       : InventoryModel.cs
* DESCRIPTION     : Manages the collection of items in the inventory
*
* REVISION HISTORY:
* Date             Author                    Comments
* ---------------------------------------------------------------------------
* 2005/04/15      Akram Taghavi-Burris      Created Class
*
*
/******************************************************************/

using System.Collections.Generic;
using UnityEngine;
using GameSystems.InventorySystem.Items;

namespace GameSystems.InventorySystem.Inventory
{
    [System.Serializable]
    public class InventoryModel 
    {
        [SerializeField] private List<InventorySlot> _inventorySlots; //list of all inventory slots
        [SerializeField] private int _maxSlots = 20; //Maximum number of slots
        private List<int> _unoccupiedSlots; //List of unoccupied (empty) slots
        private List<int> _occupiedSlots; //List of occupied slots and their contents
        
        public int MaxSlots => _maxSlots; //Public getter for max slots
        public List<int> UnOccupiedSlots => _unoccupiedSlots; //Public getter for Queue of unoccupied (empty) slots
        public List<int> OccupiedSlots => _occupiedSlots; //Public getter for dictionary of occupied slots
        
        // Returns true if there is at least one empty slot in the inventory.
        public bool HasFreeSlot =>  _unoccupiedSlots.Count > 0;

        
        //Constructor for InventoryModel, create slots 
        public InventoryModel()
        {
            _inventorySlots = new List<InventorySlot>(_maxSlots); // All inventory slots
            _unoccupiedSlots = new List<int>(); // Track empty slots
            _occupiedSlots = new List<int>(); // Track occupied slots
            
            //For each slot
            for (int i = 0; i < MaxSlots; i++)
            {
                _inventorySlots.Add(null); // Start with empty slots
                _unoccupiedSlots.Add(i); // Initially all slots are available (empty)
                
            }//end for
            
        }//end InventoryModel()
        
        
        //Get a specific inventory slot
        public InventorySlot GetSlot(int index)
        {
            return (index >= 0 && index < _inventorySlots.Count) ? _inventorySlots[index] : null;
            
        }//end GetSlot()
        

        // Assign an item to an inventory slot at the available index
        public void AssignItemToSlot(InventorySlot newSlot)
        {
            //If there is no free slot, secondary check
            if (!HasFreeSlot)
            {
                Debug.LogWarning("No available inventory slots.");
                return;
            }

            // Get the first available slot index
            int index = _unoccupiedSlots[0]; 

            // Update the slot at index with the new slot data
            UpdateSlot(index, newSlot); 
             
        }//end AssignItemToSlot()


        // Unassign an item from an inventory slot
        public void UnAssignItemFromSlot(InventorySlot itemSlot)
        {
            //Get the index of the item slot to unassign
            int index = _inventorySlots.IndexOf(itemSlot);

            // Update the slot at index to null 
            UpdateSlot(index, null); 
            
        }//end UnAssignItemFromSlot()
        
        
        // Sets a slot at a specific index (used for drag/drop or swapping)
        private void UpdateSlot(int index, InventorySlot itemSlot)
        {
            //Check if the index is valid
            if (!IsIndexValid(index))
            {
                Debug.LogWarning($"Invalid index: {index}");
                return;
            }
            
            // Check if the current slot at the index is empty
            bool wasEmpty = _inventorySlots[index] == null;

            // If the inventory slot was previously empty and the item slot is not null
            if (wasEmpty && itemSlot != null)
            {
                //set the inventory slot at this index to the value of our item slot
                _inventorySlots[index] = itemSlot;  
                Debug.Log($"Slot {index} is contains {_inventorySlots[index].ItemData.name}.");
                
                _unoccupiedSlots.Remove(index); //Remove the index from the unoccupied list
                _occupiedSlots.Add(index); //Add the index to the occuppiped list
                
            } else if(!wasEmpty && itemSlot == null)
            {
                //Otherwise reset the inventory slot to null 
                _inventorySlots[index] = null;
                _unoccupiedSlots.Add(index); //Add the index back to the unoccupied list
                _occupiedSlots.Remove(index); // Remove the index from the occupied lis
            }
            
        }//end UpdateSlot()
        
        
        // Checks if the given if the index is within te bounds of the max slots
        public bool IsIndexValid(int index)
        {
            return index >= 0 && index < _maxSlots;
            
        }//end IsIndexValid


        // Returns all inventory slots (both filled and empty).
        public IEnumerable<InventorySlot> GetAllSlots()
        {
            return _inventorySlots;
        }//end GetAllSlots()

        
        // Get the total quantity of a given item across all inventory slots.
        public int GetTotalQuantity(ItemData itemData)
        {
            int totalQuantity = 0; 
            
            //For each slot in the inventory slots
            foreach (InventorySlot slot in _inventorySlots)
            {
                // Check if the slot holds the same item type
                if (slot != null && slot.ItemData == itemData)
                {
                    totalQuantity += slot.SlotQuantity;
                    
                }//end if
                
            }//end for each
            
            return totalQuantity;
            
        }//end GetTotalQuantity
        

    } //end InventoryModel
    
}//end Namespace