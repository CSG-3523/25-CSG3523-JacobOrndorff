/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Game Management System
* FILE NAME       : PlayingState.cs
* DESCRIPTION     : Behaviours for Playing Game State
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
    public class PlayingState : GameState
    {
        private SceneData _gameSceneData;
        private SceneData _lastLoadedGameScene; //records the game scene 
        private bool _hasExited = false; //records if we've exited the state 
        
        public override void EnterGameState()
        {
            Debug.Log($"Entering Playing");
            
            // Check if gameplay has NOT already started
            if (!_gameManager.HasGameplayStarted)
            {
                _gameSceneData = SceneDirector.Instance.SceneLoader.GetGameLevel(); //get the first game scene
                LoadGameScene(_gameSceneData); //if not, load first game scene
                
            }
            // Else if we've exited playing state and returned, load the last game level 
            else if(_hasExited)
            {
                LoadGameScene(_lastLoadedGameScene);
                _hasExited = false; //Reset the exited
                
            }
            // Else just resume
            else{
                // Gameplay has already started, so just resume without reloading the scene
                Debug.Log("Gameplay already started, resuming.");
                
            }//end if(hasGameplayStarted)

        }//end EnterGameState()
        
        

        public override void ExitGameState()
        {
            Debug.Log("Exiting PLaying");
            _lastLoadedGameScene = _gameSceneData;
            _hasExited = true;
            

        }//end ExitGameState()


        private void LoadGameScene(SceneData sceneData)
        {
            //call the scene change 
            _sceneDirector.OnSceneChangeRequest(sceneData , SceneOperation.Load);
            
            //Notify GameManager that gameplay has started, first level loaded
            _gameManager.HasGameplayStarted = true;
        }
        
        

    }//end PlayingState

}//end namespace