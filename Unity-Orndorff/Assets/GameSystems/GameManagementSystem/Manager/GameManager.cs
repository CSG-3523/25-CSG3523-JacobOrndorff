/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Game Management System
* FILE NAME       : GameManager.cs
* DESCRIPTION     : Handles behaviors for the Main Menu State
*
* REVISION HISTORY:
* Date             Author                    Comments
* ---------------------------------------------------------------------------
* 2005/02/08      Akram Taghavi-Burris      Created Class
*
*
/******************************************************************/

using System;
using System.Collections;
using UnityEngine;
using GameSystems.Shared.BaseClasses;
using GameSystems.GameManagementSystem.GameStates;
using GameSystems.GameManagementSystem.SceneManagement;
using UnityEditor;
//Alias to Unity's SceneManager
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace GameSystems.GameManagementSystem.Manager
{
    public class GameManager : Singleton<GameManager>
    {

        //Game States
        private GameState _mainMenuState;
        private GameState _playingState;
        private GameState _pausedState;
        
        public GameState MainMenuState => _mainMenuState;
        public GameState PausedState => _pausedState;
        public GameState PlayingState => _playingState;
        
        
        private SceneDirector _sceneDirector; //Reference to SceneDirector
        
        //References to current and last state
        private GameState _currentState;
        private GameState _lastState;
        private GameState _stateBeforePause;
        
        public GameState CurrentState => _currentState;
        public GameState LastState => _lastState;
        
        [HideInInspector]
        public bool HasGameplayStarted = false; // Flag to track if gameplay has started
        [HideInInspector]
        public bool IsGamePaused = false; //Flag to track game pause
        
        private bool hasPauseProcessed = false; //Check if pause has processes before toggling again


        private void InitializeStates()
        {
            _mainMenuState = new MainMenuState();
            _playingState = new PlayingState();
            _pausedState = new PausedState();

            _mainMenuState.Initialize(_sceneDirector, this);
            _playingState.Initialize(_sceneDirector, this);
            _pausedState.Initialize(_sceneDirector, this);
            
        }//end InitializeStates()

        // Subscribe to the SceneDirectorReady event
        private void OnEnable()
        {
            SceneDirector.OnSceneDirectorReady += HandleSceneDirectorReady;
        }

        // Unsubscribe to avoid memory leaks
        private void OnDisable()
        {
            SceneDirector.OnSceneDirectorReady -= HandleSceneDirectorReady;
        }

        
        //Change to the first state of the game when the scene loader is reader
        private void HandleSceneDirectorReady()
        {
            _sceneDirector = SceneDirector.Instance; 
            
            // Initialize all game states
            InitializeStates();
            
            //Change state to main menu
            ChangeState(_mainMenuState);
            
        }//end HandleSceneLoaderReady()
        
        
        public void ChangeState(GameState newGameState)
        {
            Debug.Log($"Switch state to: {newGameState.ToString()}");

            //record the current state as the last state
            _lastState = _currentState; 
            
            //If current stat is not null Exist the game state
            _currentState?.ExitGameState();
            
            //Assign the new game state as the current state 
            _currentState = newGameState;
            Debug.Log($"The current state is {_currentState.ToString()}");
            
            //Enter the new game state
            _currentState.EnterGameState();
            
        }//end ChangeState
        

        
        //Method for toggling the paused state
        public void TogglePause()
        {
            Debug.Log($"Game is paused: {IsGamePaused}");
            Debug.Log("Toggle Pause");

            if (_currentState != PausedState && !IsGamePaused)
            {
                _stateBeforePause = _currentState;
                Debug.Log($"Switching to {PausedState} from {_currentState}");
                ChangeState(PausedState);
                IsGamePaused = true;
                Debug.Log($"Paused. IsGamePaused: {IsGamePaused}, CurrentState: {_currentState}");
            }
            else
            {
                Debug.Log($"Unpause and returning to {_stateBeforePause}");
                ChangeState(_stateBeforePause);
                IsGamePaused = false;
                Debug.Log($"Unpaused. IsGamePaused: {IsGamePaused}, CurrentState: {_currentState}");
            }
        }//end TogglePause()
        
    } //end GameManager
    
}//end namespace