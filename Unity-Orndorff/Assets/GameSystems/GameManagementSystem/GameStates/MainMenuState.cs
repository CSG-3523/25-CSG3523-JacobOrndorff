/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Game Management System
* FILE NAME       : MainMenuState.cs
* DESCRIPTION     : Behaviours for MainMenu Game State
*
* REVISION HISTORY:
* Date             Author                    Comments
* ---------------------------------------------------------------------------
* 2005/02/08      Akram Taghavi-Burris      Created Class
*
*
/******************************************************************/

using UnityEngine;
using GameSystems.GameManagementSystem.SceneManagement;

namespace GameSystems.GameManagementSystem.GameStates
{ 
    public class MainMenuState : GameState
    {

        public override void EnterGameState()
        {
            Debug.Log($"Entering {SceneDirector.Instance.SceneLoader.MainMenuScene.SceneName}");
            
           _sceneDirector.OnSceneChangeRequest(SceneDirector.Instance.SceneLoader.MainMenuScene , SceneOperation.Load, true);

        }//end EnterGameState()

        public override void ExitGameState()
        {
            Debug.Log("Exiting Main Menu");
            _sceneDirector.OnSceneChangeRequest(SceneDirector.Instance.SceneLoader.MainMenuScene , SceneOperation.Unload);
            
        }//end ExitGameState()

    }//end MainMenu

}//end namespace