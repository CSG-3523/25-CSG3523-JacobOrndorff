/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Dynamic Inventory 
* FILE NAME       : Item.cs
* DESCRIPTION     : The runtime-specific logic for inventory items
*                    
* REVISION HISTORY:
* Date             Author                    Comments
* ---------------------------------------------------------------------------
* 2005/04/08      Akram Taghavi-Burris      Created Class
* 
*
/******************************************************************/
using System;
using UnityEngine;
using GameSystems.Shared.Interfaces;

namespace GameSystems.InventorySystem.Items
{
    [RequireComponent(typeof(BoxCollider))]
    public class Item : MonoBehaviour, IInteractable
    {
        
        [SerializeField] private ItemData _itemData;  // Reference to item data
        [SerializeField] private int _quantity = 1;   // How many items this represents
        protected GameObject _interactor; // Reference to the game object that triggered the interaction

        // Event is triggered when an item is picked up
        public static event Action<ItemData, int> OnItemPickedUp;

        // Required by IInteractable Interface
        // Called when the player (or another object) interacts with the item. 
        public void OnInteract(GameObject interactor)
        {
            // Trigger the event, notifying any listeners (like InventoryManager)
            OnItemPickedUp?.Invoke(_itemData, _quantity);
            _interactor = interactor; //set the interact game object
            
            Debug.Log($"Picked up item {_itemData.name}");
            
            Destroy(gameObject);  // Remove the item from the world (destroy pickup)
            
        }//end Interact()


        //When the item awake set the box collider to trigger
        private void Awake()
        {
            GetComponent<BoxCollider>().isTrigger = true;
            
        }//end Awake()

        // Initializes the item with its data and quantity when the item is created or respawned in the world.
        public void Initialize(ItemData data, int quantity = 1)
        {
            _itemData = data;
            _quantity = quantity;
            
        }//end Initialize()
        
        //Method to return the item data and quality
        public ItemData GetItemData() => _itemData;
        public int GetQuantity() => _quantity;

        //Call interaction when trigger is entered
        private void OnTriggerEnter(Collider other)
        {
            if (CanInteract(other))
            {
                OnInteract(other.gameObject); //pass the game object of the interactor to Interact()
            }
            
        }//end OnTriggerEnter()
        

        // Virtual method to be overridden by child classes
        protected virtual bool CanInteract(Collider other)
        {
            // Default interaction logic, only allowing the player
            return other.CompareTag("Player");
            
        }//end CanInteract
        
    }//end Item
    
}//end Namespace
