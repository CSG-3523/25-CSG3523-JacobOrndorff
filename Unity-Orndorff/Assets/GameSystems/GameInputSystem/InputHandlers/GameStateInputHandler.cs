/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Game Input System
* FILE NAME       : GameStateInputHandler.cs
* DESCRIPTION     : Abstract class for state-specific input handlers.
*                   Each state handler enables its own input map.
* REVISION HISTORY:
* Date             Author                    Comments
* ---------------------------------------------------------------------------
* 2005/02/08      Akram Taghavi-Burris      Created Class
*
*
/******************************************************************/

using System;
using System.Collections.Generic;
using System.Reflection;
using GameSystems.GameManagementSystem.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameSystems.GameInputSystem.InputHandlers
{
    public abstract class GameStateInputHandler : MonoBehaviour
    {
        [Header("Input System Asset")]
        [Tooltip("Reference to the InputActionAsset (from the Input System)")]
        public InputActionAsset ActionInputAsset;

        private InputActionMap _actionMap;
        
        // Keep a reference to delegates so we can unsubscribe them
        private readonly Dictionary<InputAction, Action<InputAction.CallbackContext>> _actionHandlers = new();
        
        protected GameManager gameManager;

        
       // Called when the GameObject becomes enabled.
       private void OnEnable()
       {
           gameManager = GameManager.Instance;
           
            // Check if ActionInputAsset is not assigned
            if (ActionInputAsset == null)
            {
                Debug.LogError($"{nameof(GameStateInputHandler)} requires an InputActionAsset.");
                return;
                
            }//end if(GameInputActionAsset == null)

            // Get the first action map from the InputActionAsset
            _actionMap = ActionInputAsset.actionMaps.Count > 0 ? ActionInputAsset.actionMaps[0] : null;

            //Check if action map is not assigned
            if (_actionMap == null)
            {
                Debug.LogError("No action map found in InputActionAsset.");
                return;
                
            }//end if(_actionMap == null)

            // Loop through all actions in the selected map and enable them
            foreach (var action in _actionMap.actions)
            {
                action.Enable();
                // Register the dynamic callback for each action
                action.performed += ctx => OnActionPerformed(ctx);
                
            }//end foreach(action)
            
        }//end OnEnable()


        // Called when the GameObject is disabled.
        private void OnDisable()
        {
            //Check if no action map is assigned
            if (_actionMap == null) return;

            // Loop through all actions and unregister the callback
            foreach (var action in _actionMap.actions)
            {
                action.performed -= ctx => OnActionPerformed(ctx);
                action.Disable();
            }//end foreach(action)
            
        }//end OnDisable()
        
        // Generic method to handle action performance based on action name
        private void OnActionPerformed(InputAction.CallbackContext context)
        {
            string actionName = context.action.name;
            
            // Dynamically call method based on action name
            var method = this.GetType().GetMethod($"On{actionName}", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (method != null)
            {
                // Check if the method has parameters
                var parameters = method.GetParameters();

                if (parameters.Length == 0)
                {
                    // Method doesn't require parameters, invoke without arguments
                    method.Invoke(this, null);
                }
                else
                {
                    // If method requires parameters, pass the callback context
                    method.Invoke(this, new object[] { context });
                }
            }
            else
            {
                Debug.LogWarning($"No method found for action: {actionName}");
                
            }//end if(method)
            
        }//end OnActionPerformed()

   
    }//end GameStateInputHandler
    
}//end namespace