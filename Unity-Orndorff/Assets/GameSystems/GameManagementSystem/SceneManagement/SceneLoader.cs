/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Game Management System
* FILE NAME       : SceneLoader.cs
* DESCRIPTION     : Manages the loading and unloading of scenes
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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameSystems.Shared.BaseClasses;



namespace GameSystems.GameManagementSystem.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        //Events are non-static since SceneLoader is a component on the SceneDirector which is subscribing
        public event Action<SceneData, float> OnSceneProgessUpdate; // Event to notify when a scene is Processed
        public event Action<SceneData> OnSceneLoaded; // Event to notify when a scene is Loaded
        public event Action<SceneData> OnSceneUnloaded; // Event to notify when a scene is Unloaded
        
        [Header("UTILITY SCENES")]
        public SceneData MainMenuScene; // Changed from string to SceneData
        public SceneData PauseMenuScene;
        public SceneData OptionsMenuScene;
        public SceneData HudScene;
        public SceneData GameOverScene;

        [Header("GAMEPLAY SCENES")]
        [Header("List of Game Levels")]
        [Tooltip("If sequential, list in order.")]
        public List<SceneData> GameLevels;
        
        [Header("OPTIONAL: First Game Level")]
        public SceneData FirstGameLevel; 
        
        private float _sceneLoadingProgress;
        private List<SceneData> _allScenes;
        private HashSet<SceneData> _loadedScenes = new();
        private int _gameLevelIndex = -1; // Set to -1 initially, as no level is loaded yet.
        private string _currentScene;
        
        public bool IsSceneLoaderReady { get; private set; } //Check if ready
        public string CurrentScene => SceneManager.GetActiveScene().name;

        private void Start()
        {
            IsSceneLoaderReady = false; //false before scenes are added
            
            // Add all scenes to the All Scenes list
            _allScenes = new List<SceneData>
            {
                MainMenuScene,
                PauseMenuScene,
                OptionsMenuScene,
                HudScene,
                GameOverScene
            };

            _allScenes.AddRange(GameLevels);

            IsSceneLoaderReady = true; //SceneLoader is ready

        }//end Start()

        //Check if the scene is already loaded
        public bool IsSceneAlreadyLoaded(SceneData scene)
        {
            if (_loadedScenes.Contains(scene))
            {
                Debug.LogWarning($"{scene.SceneName} is already loaded");
                return true;
            }
            
            return false;
            
        }//end IsSceneLoaded()


        ///<summary>
        /// Get a game level, either by name or by index using coroutines.
        ///</summary>
        ///<param name="gameLevel">The scene data for the next game level to get, null value allowed for loading sequentially.</param>
        public SceneData GetGameLevel(SceneData gameLevel = null)
        {
            Debug.Log($"Initial level index: {_gameLevelIndex}");
            Debug.Log($"Total game levels: {GameLevels.Count}");
            
            // If the game level scene data is null, set to the next level in the game level list
            if (gameLevel == null)
            {
                _gameLevelIndex++;
                
                Debug.Log($"Incremented level index: {_gameLevelIndex}");

                // If we're past the last level, loop back to the first level (or handle as needed)
                if (_gameLevelIndex >= GameLevels.Count)
                {
                    Debug.LogWarning("No more game levels");
                }
            }
            else if (GameLevels.Contains(gameLevel))
            {
                _gameLevelIndex = GameLevels.IndexOf(gameLevel); // Set the current level index to the passed level
            }
            else
            {
                Debug.LogWarning($"Game level '{gameLevel}' not found. Defaulting to the first level.");
                _gameLevelIndex = 0; // Default to the first level in the list
            }

            // Log for debugging
            Debug.Log("Level to load: " + GameLevels[_gameLevelIndex].SceneName);
            
            //return the game level scene data
            return GameLevels[_gameLevelIndex];

            
        }//end GetGameLevel()
        

        // Load a scene by assembly name and scene name
        private void LoadSceneFromAssembly(string assemblyName, string sceneName)
        {
            // Find the scene in AllScenes that matches both the assembly name and scene name
            SceneData sceneData = _allScenes.Find(scene => scene.SceneName == sceneName && scene.AssemblyName == assemblyName);
            
            // If the SceneData is null, log error early
            if (sceneData == null)
            {
                Debug.LogError($"Scene {sceneName} not found in assembly {assemblyName}");
                return;
            }
            
        }//end LoadScenesFromAssembly

        ///<summary>
        /// Load the scene asynchronously using coroutines 
        ///</summary>
        ///<param name="sceneData">The scene data to load.</param>
        ///<param name="loadMode">The scene load mode (i.e. subtractive or additive).</param>
        public IEnumerator LoadSceneAsync(SceneData sceneData, LoadSceneMode loadMode)
        {
            //get the scene name
            string sceneName = sceneData.SceneName;
            
            Debug.Log($"Loading scene: {sceneName}");

            // Reference to the scene being unloaded
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadMode);

            // Check to ensure there is no null reference
            if (asyncLoad == null)
            {
                Debug.LogError($"Failed to load scene '{sceneName}'");
                yield break;
            }

            // If loading in Single mode set scene as current scene
            if (loadMode == LoadSceneMode.Single)
            {
                _currentScene = sceneName;
            }

            // Wait until scene is fully loaded
            while (!asyncLoad.isDone)
            {
                _sceneLoadingProgress = asyncLoad.progress;
                Debug.Log($"Loading progress: {_sceneLoadingProgress}");
                OnSceneProgessUpdate?.Invoke(sceneData,_sceneLoadingProgress); //scene has processed 
                yield return null;
            }
            
            OnSceneLoaded?.Invoke(sceneData); //scene has processed 

            // Add scene to List after it successfully loads
            _loadedScenes.Add(sceneData);
            
            Debug.Log($"Scene '{sceneName}' successfully loaded.");

            yield return new WaitForSeconds(0.25f); // Optional polish delay
            
        }//end LoadSceneAsync()

        ///<summary>
        /// Load the next scene automatically from the assembly list using coroutines
        ///</summary>
        ///<param name="assemblyName">The name of the scene to unload.</param>
        public void LoadNextSceneInAssembly(string assemblyName)
        {
            // Get all scenes in the given assembly
            List<SceneData> scenes = _allScenes.FindAll(scene => scene.AssemblyName == assemblyName);

            // Find the index of the current scene in the assembly
            int index = scenes.FindIndex(scene => scene.SceneName == CurrentScene);

            // If the index of the scene is found
            if (index >= 0 && index < scenes.Count - 1)
            {
                // Set the next scene to the scene data from the scene list at that index
                SceneData nextScene = scenes[index + 1];

                SceneDirector.Instance.OnSceneChangeRequest(nextScene, SceneOperation.Load);
            }
            else
            {
                Debug.Log("No next scene available or scene not found in assembly.");
            }
            
        }//end LoadNextSceneInAssembly()

        ///<summary>
        /// Unload a scene and remove it from the loaded scenes list and unloads asynchronously using coroutines.
        ///</summary>
        ///<param name="sceneData">The scene data to unload.</param>
        public void UnloadScene(SceneData sceneData)
        {
            //get the scene name
            string sceneName = sceneData.SceneName;
            
            // Check if the scene is currently loaded
            if (_loadedScenes.Contains(sceneData))
            {
                StartCoroutine(UnloadSceneAsync(sceneData));
            }
            else
            {
                Debug.LogWarning($"Scene '{sceneName}' is not currently loaded.");
            }
        }//en UnloadScene()

        ///<summary>
        /// Coroutine to unload the scene asynchronously.
        ///</summary>
        ///<param name="sceneData">Get the scene data to unload</param>
        public IEnumerator UnloadSceneAsync(SceneData sceneData)
        {
            Debug.Log($"[Coroutine] Starting unload for scene: {sceneData.SceneName}");
            
            //get the scene name
            string sceneName = sceneData.SceneName;
            
            Debug.Log($"Unloading scene: {sceneName}");
            
            // Reference to the scene being unloaded
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

            // Check to ensure there is no null reference
            if (asyncUnload == null)
            {
                Debug.LogError($"Failed to unload scene '{sceneName}'");
                yield break;
            }

            // Wait until the scene is fully unloaded
            while (!asyncUnload.isDone)
            {
                _sceneLoadingProgress = asyncUnload.progress;
                Debug.Log($"Unloading progress: {_sceneLoadingProgress}");
                OnSceneProgessUpdate?.Invoke(sceneData,_sceneLoadingProgress); //scene has processed 
                yield return null;
                
            }
            
            OnSceneUnloaded?.Invoke(sceneData); //scene has processed 
            
            // Remove from the loaded scenes list after successfully unloading
            _loadedScenes.Remove(sceneData);
            
            Debug.Log($"Scene '{sceneName}' successfully unloaded.");
            
        }//end UnloadSceneAsync
        

        // Unload all scenes and reset game levels 
        public void UnloadAllScenes()
        {
            StartCoroutine(UnloadAllScenesCoroutine());
            
        }//end UnloadAllScenes()

        ///<summary>
        /// Coroutine to unload all scenes asynchronously.
        ///</summary>
        private IEnumerator UnloadAllScenesCoroutine()
        {
            // For each scene in the _loadedScenes list
            foreach (var sceneName in _loadedScenes)
            {
                yield return StartCoroutine(UnloadSceneAsync(sceneName)); // Unload each scene individually
            }

            _loadedScenes.Clear();
            Debug.Log("All scenes unloaded.");
        }
    } //end SceneLoader
    
}//end namespace
