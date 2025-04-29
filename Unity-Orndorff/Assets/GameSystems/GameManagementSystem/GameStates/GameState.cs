/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Game Management System
* FILE NAME       : GameState.cs
* DESCRIPTION     : Abstract base class for GameStates
*
* REVISION HISTORY:
* Date             Author                    Comments
* ---------------------------------------------------------------------------
* 2005/02/08      Akram Taghavi-Burris      Created Class
*
*
/******************************************************************/


using System.Collections.Generic;
using UnityEngine;
using GameSystems.GameManagementSystem.SceneManagement;
using GameSystems.GameManagementSystem.Manager;


namespace GameSystems.GameManagementSystem.GameStates
{
    public abstract class GameState
    {
        //Reference to the managers
        protected SceneDirector _sceneDirector;
        protected GameManager _gameManager;

        // Constructor or initialization method to inject dependencies
        public void Initialize(SceneDirector sceneDirector, GameManager gameManager)
        {
            this._sceneDirector = sceneDirector;
            this._gameManager = gameManager;
            
        }//end Initialize()
        
        // Called when entering the state (e.g., transitioning to this state).
        public virtual void EnterGameState()
        {
            //do something
        }

        // Called when exiting the state (e.g., transitioning away from this state).
        public virtual void ExitGameState()
        {
            //do something
        }
        
    }//end IGameState
    
}//end namespace