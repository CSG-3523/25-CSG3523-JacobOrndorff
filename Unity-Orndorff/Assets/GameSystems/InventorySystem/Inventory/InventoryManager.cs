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

using UnityEngine;
using GameSystems.Shared.BaseClasses;
using GameSystems.InventorySystem.Items;

namespace GameSystems.InventorySystem.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        
        private InventoryModel _inventory; // Reference to the inventory model containing all the slots and item data
        public InventoryModel Inventory => _inventory; // Public getter for the InventoryModel
        
        // Awake is called once at instantiation
        protected override void Awake() 
        {
            base.Awake(); //run the base awake from singleton
            _inventory = new InventoryModel(); //Define a new inventory model for the inventory 
        }
        
        // Subscribe to the static event from the Item class 
        public void OnEnable()
        {
            Item.OnItemPickedUp += HandlePickedUp;
        }
        
        // Unsubscribe to the static event from the Item class 
        public void OnDisable()
        {
            Item.OnItemPickedUp -= HandlePickedUp;
        }
        
        // Method to add an item to the inventory
        private void HandlePickedUp(ItemData itemData, int pickupQuantity)
        {

            // Check if there’s an existing slot that contains the same item
            InventorySlot existingItemSlot = CheckForExistingItem(itemData);

            //If there is an available slot
            if (existingItemSlot != null)
            {
                AddItemToSlot(existingItemSlot, itemData, pickupQuantity); //add to slot
            }
            else
            {
                CreateNewItemSlot(itemData, pickupQuantity); //create a new slot 
                
            }//end if(existingItemSlot)
            
        }//end HandlePickedUp   
        
        
        //Check if there is already a slot for this item
        private InventorySlot CheckForExistingItem(ItemData itemData)
        {
            // If there are no occupied slots, return null early
            if (_inventory.OccupiedSlots == null) return null;
            
            // Loop through each occupied slot index
            foreach (var index in _inventory.OccupiedSlots)
            {
                // Get the slot at the current index
                InventorySlot slot = _inventory.GetSlot(index);
    
                // Check if the slot contains the same item and has room for more
                if (slot.ItemData == itemData && slot.SlotQuantity < itemData.MaxStackSize)
                {
                    return slot; // Return the matching slot
                }
                
            } //end for
            
            // No matching slot found
            return null;
            
        }//end CheckForExistingItem()
        
        
        // Add item to specified slot and return the remainder
        private void AddItemToSlot(InventorySlot slot, ItemData itemData, int pickupQuantity)
        {
            // Calculate how many items the slot can hold
            int availableSpace = itemData.MaxStackSize - slot.SlotQuantity;

            // Calculate how many the picked up item can be placed
            int quantityToAdd = Mathf.Min(availableSpace, pickupQuantity);

            // Add the item quantity to the slot
            slot.AddQuantity(quantityToAdd);
            Debug.Log($"{quantityToAdd} {itemData.Name} has been added to inventory.");

            // Calculate if there's any leftover quantity
            int remainingQuantity = pickupQuantity - quantityToAdd;
            Debug.Log($"There are {remainingQuantity} {itemData.Name} remaining");

            // If there are leftover items, create a new slot for them
            if (remainingQuantity > 0)
            {
                CreateNewItemSlot(itemData, remainingQuantity);
                
            }//end if(remainingQuantity) 
            
        }//end AddItemToSlot()
        
        
        // Create a new Item slot 
        private void CreateNewItemSlot(ItemData itemData, int pickupQuantity)
        {
            // Check if there is no free slot
            if (!_inventory.HasFreeSlot)
            {
                Debug.LogWarning("Inventory is full!");
                return;
                
            }//end if(inventory.MaxSlots)
            
            // Create a new InventorySlot for the item
            InventorySlot newSlot = new InventorySlot(itemData, pickupQuantity);
            
            // Now set the slot at the available index to this new InventorySlot
            _inventory.AssignItemToSlot(newSlot);
            
            // Log the item addition
            Debug.Log($"{itemData.Name} has been added to inventory.");
        }//end CreateNewItemSlot

        private void RemoveItemFromSlot(ItemData itemData,int usedQuantity)
        {
            // Check if there’s an existing slot that contains the same item
            InventorySlot existingItemSlot = CheckForExistingItem(itemData);

            //If the item slot does not exist, exit.
            if (existingItemSlot == null) return;
            
            // Remove used quantity from the existing item slot
            existingItemSlot.RemoveQuantity(usedQuantity);
            
            // if the item quantity becomes zero or less after subtracting the used quantity
            if (existingItemSlot.SlotQuantity >= 0)
            {
                _inventory.UnAssignItemFromSlot(existingItemSlot); //unassign from inventory 
                
            }//end if(remainingQuantity)
            
        }//end RemoveItemFromSlot
        

    } //end InventoryManager
    
}//end Namespace
