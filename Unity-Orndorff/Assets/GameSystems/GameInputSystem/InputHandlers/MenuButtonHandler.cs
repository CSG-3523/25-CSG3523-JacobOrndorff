/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Game Input System
* FILE NAME       : MenuButtonHandler.cs
* DESCRIPTION     : Concrete subclass of UIButtonHandler for
*                   defining actual methods.
* REVISION HISTORY:
* Date             Author                    Comments
* ---------------------------------------------------------------------------
* 2005/02/08      Akram Taghavi-Burris      Created Class
*
*
/******************************************************************/

using UnityEngine;
using GameSystems.GameInputSystem.UI;



namespace GameSystems.GameInputSystem.InputHandlers
{
    public class MenuButtonHandler : UIButtonHandler
    {
        // Called by input action or button named "Play"
        public void OnPlayButton()
        {
            Debug.Log("Play button pressed or action performed.");
            gameManager.ChangeState(gameManager.PlayingState);
        }

        // Called by input action or button named "Restart"
        public void OnRestartButton()
        {
            Debug.Log("Restart button pressed or action performed.");
            // Start the game, load scene, etc.
        }
        
        // Called by input action or button named "Resume"
        public void OnResumeButton()
        {
            Debug.Log("Resume button pressed or action performed.");
            // Start the game, load scene, etc.
        }

        // Called by input action or button named "Quit"
        public void OnQuitButton()
        {
            Debug.Log("Quit button pressed or action performed.");
            Application.Quit();
        }

        // Called by input action or button named "Settings"
        public void OnSettings()
        {
            Debug.Log("Settings button pressed or action performed.");
            // Open settings menu
        }
        
        
        
    }//end MainMenuInput


}//end namespace