/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Game Input System
* FILE NAME       : UIButtonHandler.cs
* DESCRIPTION     : Registers UI Button actions 
*                   Use reflection to invoke the corresponding input action method in GameStateInputHandler
*
* REVISION HISTORY:
* Date             Author                    Comments
* ---------------------------------------------------------------------------
* 2005/02/08      Akram Taghavi-Burris      Created Class
*
*
/******************************************************************/


using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using GameSystems.GameInputSystem.InputHandlers;
using GameSystems.GameManagementSystem.Manager;

namespace GameSystems.GameInputSystem.UI
{
    public abstract class UIButtonHandler : MonoBehaviour
    {
    
        [SerializeField] protected UIDocument _uiDoc;
        protected Dictionary<string, Button> _buttonDictionary = new();
        protected GameManager gameManager;
     

        // Awake is called once at instantiation
        private void Awake()
        {
            // Get the UIDocument component attached to this GameObject
            _uiDoc = GetComponent<UIDocument>();
            
            if (_uiDoc == null)
            {
                Debug.LogError($"{nameof(UIButtonHandler)} requires a UIDocument reference.");
                return;
                
            }//end if(_uiDoc)

        }//end Awake()
        
        //Start is called once at the first fame
        private void Start()
        {
            gameManager = GameManager.Instance;
            
            // Get all the buttons in the UI Document
            InitializeButtons();
        
        }//end Start()
        

        private void InitializeButtons()
        {
            var rootVisualElement = _uiDoc.rootVisualElement;
            var buttons = rootVisualElement.Query<Button>().ToList();

            //Exit if there are no buttons
            if (buttons.Count == 0)
            {
                Debug.LogWarning("No buttons found in the UI document.");
                return;
            }

            // Loop through all buttons and bind their click event dynamically
            foreach (var button in buttons)
            {
                // Use the button in the loop, no need to query again by name
                string buttonName = button.name; // Assuming each button has a unique name
                
                //Check if the button has no name
                if (string.IsNullOrEmpty(buttonName))
                {
                    continue; // Skip buttons without a name
                    
                }//end if(IsNullOrEmpty(buttonName)
                
                // Store button in the dictionary
                _buttonDictionary[buttonName] = button;
                
                // Register click event listener
                button.RegisterCallback<ClickEvent>(evt => OnButtonClick(buttonName));
                
            }//end foreach(button)
            
        }//end InitializeButtons()

        
        private void OnButtonClick(string buttonName)
        {
            // Use reflection to invoke the corresponding input action method in GameStateInputHandler
           var methodInfo = GetType().GetMethod($"On{buttonName}",
               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
           

            //Check if method exists
            if (methodInfo != null)
            {
                // Invoke the method dynamically in the GameStateInputHandler
                methodInfo.Invoke(this, null);
            }
            else
            {
                Debug.LogWarning($"No method found for button: On{buttonName} in {nameof(UIButtonHandler)}.");
                
            }//end if(methodInfo != null)
            
        }//end OnButtonClick
        
    }//end ButtonHandler
    
}//end namespace
