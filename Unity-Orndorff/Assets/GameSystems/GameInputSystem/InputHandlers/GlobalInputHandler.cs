/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Game Input System
* FILE NAME       : GlobalInputHandler.cs
* DESCRIPTION     : Global actions that are always active.Should be placed on a persistent
*                   object (like a GameManager, InputManager, or Player) that exists across scenes.
* REVISION HISTORY:
* Date             Author                    Comments
* ---------------------------------------------------------------------------
* 2005/02/08      Jacob Orndorff      Created Class
*
*
/******************************************************************/

using UnityEngine;
using UnityEngine.InputSystem;

namespace GameSystems.GameInputSystem.InputHandlers {
    public class GlobalInputHandler : GameStateInputHandler {
        
        // Called by input action named "Pause"
        private void OnPause() {
            Debug.Log("Pause button pressed or action performed.");
            //Initiate Pause
            gameManager.TogglePause();
        }
        
        // Called by input action named "OpenInventory"
        private void OnOpenInventory() {
            Debug.Log("Inventory opened.");
            gameManager.TogglePause(); 
        }
        
        //Called by input action named "OnQuit"
        private void OnQuit() {
            //If MainMenu Quit Game
            if (gameManager.CurrentState == gameManager.MainMenuState) {
                Application.Quit();
            } else {
                //Toggle Pause
                gameManager.TogglePause();
            }//end if(MainMenu)
        }//end OnQuit()
    }//end GlobalInputInput
}//end namespace