/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Game Management System
* FILE NAME       : SceneDirector.cs
* DESCRIPTION     : Directs the timing of scene transitions and loading
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
using GameSystems.GameManagementSystem.Manager;


namespace GameSystems.GameManagementSystem.SceneManagement
{
    [RequireComponent(typeof(SceneLoader))]
    public class SceneDirector : Singleton<SceneDirector>
    {
        public static event Action OnSceneDirectorReady; // Event to notify when SceneManager is ready
        public static event Action OnGameLevelLoad; // Event to notify when a game level loads
        
        //Non-static forwarded event of progress
        public event Action<SceneData, float> OnSceneProgressUpdate
        {
            add => SceneLoader.OnSceneProgessUpdate += value;
            remove => SceneLoader.OnSceneProgessUpdate -= value;
        }
        
        //References to Components
        private SceneLoader _sceneLoader; //reference to the scene loader
        public SceneLoader SceneLoader => _sceneLoader;

        
        [SerializeField] 
        [Tooltip("The main camera that will be persistant")]
        private Camera _mainCamera; //reference to camera from bootstrap scene

        [SerializeField] 
        [Tooltip("GameObject for boot screen background.")]
        private GameObject _bootScreen; 

        //References to scenes
        private SceneData _currentScene;
        private SceneData _lastSceneLoaded;
        private SceneData _lastGameplayScene;
        private float _sceneLoadingProgress;
        
        public float SceneLoadingProgress => _sceneLoadingProgress;

        
        public void Start()
        {
            _sceneLoader = GetComponent<SceneLoader>();
            if(_mainCamera==null)_mainCamera = Camera.main;
            
            // Wait for transition to be fully initialized
            StartCoroutine(WaitInitialization());

        }//end Awake()
        
        
        //Check for initialization parameters before Scene Manager is ready
        private IEnumerator WaitInitialization()
        {
            // Wait until both the SceneTransitionHandler and SceneLoader are ready
            yield return new WaitUntil(() =>
                _sceneLoader.IsSceneLoaderReady
            );

            SubscribeToComponentEvents();

            // Trigger when Scene Manager is ready 
            OnSceneDirectorReady?.Invoke();
            
        }//end WaitInitialization()
        
        
        // Subscribe to the SceneLoading Events event
        private void SubscribeToComponentEvents()
        {
            _sceneLoader.OnSceneProgessUpdate += HandleSceneProgress;
            _sceneLoader.OnSceneLoaded += HandleSceneLoaded;
            _sceneLoader.OnSceneUnloaded += HandleSceneUnloaded;
            
        }

        // Unsubscribe to avoid memory leaks
        private void OnDisable()
        { 
            _sceneLoader.OnSceneProgessUpdate -= HandleSceneProgress;
            _sceneLoader.OnSceneLoaded -= HandleSceneLoaded;
            _sceneLoader.OnSceneUnloaded -= HandleSceneUnloaded;
        }


        // Method to request a scene change asynchronously using coroutines.
        //<param name="sceneData">The scene data for requested scene change.</param>
        //<param name="sceneOperation">The operation for the scene (e.g. load or unload) .</param>
        //<param name="unloadAllScenes">Before change scenes should all scenes be unloaded.</param>
        public void OnSceneChangeRequest(SceneData sceneData, SceneOperation sceneOperation, bool unloadAllScenes = false)
        {
            if (unloadAllScenes) _sceneLoader.UnloadAllScenes();
            
            //Condition for applying transition
            bool shouldUseTransition = (sceneData.Type != SceneType.UIOverlay );
            
            switch (sceneOperation)
            {
                case SceneOperation.Load:
                    // If the scene is already loaded, exit (do nothing) 
                    if (_sceneLoader.IsSceneAlreadyLoaded(sceneData)) return;
                    
                    StartCoroutine(_sceneLoader.LoadSceneAsync(sceneData, sceneData.LoadMode));
                    _lastSceneLoaded = sceneData;
                    break;
                
                case SceneOperation.Unload:
                    // Exit early if scene isn't loaded
                    if (!_sceneLoader.IsSceneAlreadyLoaded(sceneData)) return;
                    
                    StartCoroutine(_sceneLoader.UnloadSceneAsync(sceneData)) ;

                    break;
                
                case SceneOperation.Reload:
                    // Exit early if scene isn't loaded
                    if (!_sceneLoader.IsSceneAlreadyLoaded(sceneData)) return;
                    
                    StartCoroutine(_sceneLoader.UnloadSceneAsync(sceneData));
                    
                    StartCoroutine(_sceneLoader.LoadSceneAsync(sceneData, sceneData.LoadMode));
                    
                    break;
                    
                default:
                    break;
                
            }//end switch(sceneOperation)
            
        }//end OnSceneChangeRequest
        
        
        //Update the scene loading progresss
        private void HandleSceneProgress(SceneData sceneData, float sceneLoadingProgress)
        {
            Debug.Log($"{sceneData.SceneName} progress is at {sceneLoadingProgress}");
            
            _sceneLoadingProgress = sceneLoadingProgress;
            
        }//end HandleSceneProgress()
        

        
        //Handel behaviors when a scene is Loaded
        private void HandleSceneLoaded(SceneData sceneData)
        {
            Debug.Log($"Scene Loaded {sceneData.SceneName}");
            
            //If boot screen is active, disable. Should only run at start
            if(_bootScreen.activeSelf)_bootScreen.SetActive(false);
            
            PurgeCameras();
            
            TrackSceneLoaded(sceneData);

        }//end HandleSceneLoaded()
        
        
        //Handel behaviors when a scene is Loaded
        private void HandleSceneUnloaded(SceneData sceneData)
        {
            Debug.Log($"Scene Unloaded {sceneData.SceneName}");

        }//end HandleSceneUnloaded()
        
        
        // Purge all scene cameras and only use the Scene Director mainCamera 
        private void PurgeCameras()
        {
            // Get all cameras in the scene
            Camera[] allCameras = UnityEngine.Object.FindObjectsByType<Camera>(FindObjectsSortMode.None);

            // Loop through all cameras
            foreach (Camera camera in allCameras)
            {
                // Check if the camera is not the main camera
                if (camera != _mainCamera)
                {
                    Destroy(camera.gameObject); // Destroy any camera that isn't the main camera
                }
            }//end foreach(camera)

            // Ensure the main camera is active
            if (_mainCamera != null)
            {
                _mainCamera.gameObject.SetActive(true); // Enable the main camera if it's disabled
            }
            
        }//end PurgeCameras()

        //Keep track of the current scene loaded and the last gameplay scene
        private void TrackSceneLoaded(SceneData sceneData)
        {
            //Record the scenes being loaded
            if (_lastSceneLoaded != sceneData) _lastSceneLoaded = _currentScene;
            if (sceneData.Type == SceneType.GamePlay) _lastGameplayScene = sceneData;
            _currentScene = sceneData;
            
        }//end TrackSceneLoaded
        

    }//end SceneDirector
    
}//end namespace