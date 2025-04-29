/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Game Management System
* FILE NAME       : PausedState.cs
* DESCRIPTION     : Behaviours for Pause Menu Game State
*
* REVISION HISTORY:
* Date             Author                    Comments
* ---------------------------------------------------------------------------
* 2005/02/08      Akram Taghavi-Burris      Created Class
*
*
/******************************************************************/

using GameSystems.GameManagementSystem.Manager;
using UnityEngine;
using GameSystems.GameManagementSystem.SceneManagement;

namespace GameSystems.GameManagementSystem.GameStates
{ 
    public class PausedState : GameState
    {

        public override void EnterGameState()
        {
            Debug.Log($"Entering {SceneDirector.Instance.SceneLoader.PauseMenuScene.SceneName}");
            _sceneDirector.OnSceneChangeRequest(SceneDirector.Instance.SceneLoader.PauseMenuScene , SceneOperation.Load);
            Time.timeScale = 0; //pause game time

        }//end EnterGameState()

        public override void ExitGameState()
        {
            Debug.Log("Exiting Pause Menu");
            _sceneDirector.OnSceneChangeRequest(SceneDirector.Instance.SceneLoader.PauseMenuScene , SceneOperation.Unload);
            Time.timeScale = 1; //reset game time
            
        }//end ExitGameState()

    }//end PausedState

}//end namespace