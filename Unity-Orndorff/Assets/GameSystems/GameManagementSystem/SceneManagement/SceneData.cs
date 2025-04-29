/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Game Management System
* FILE NAME       : SceneData.cs
* DESCRIPTION     : Scriptable Object of scene data 
*
* REVISION HISTORY:
* Date             Author                    Comments
* ---------------------------------------------------------------------------
* 2005/02/08      Akram Taghavi-Burris      Created Class
*
*
/******************************************************************/
using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using GameSystems.Shared.Interfaces;
using UnityEngine.SceneManagement;
//using GameSystems.GameManagementSystem.SceneManagement.SceneTransitions;

namespace GameSystems.GameManagementSystem.SceneManagement
{
    /// <summary>
    /// SceneType is used to categorize different types of scenes in your game.
    /// Allowing handling camera behavior and other scene-specific logic appropriately.
    /// </summary>
    public enum SceneType
    {
        Bootstrap,   // Initial loading scene, maybe just for loading resources or showing splash screen
        GamePlay,    // Main gameplay scenes where the player is actively interacting
        UIOverlay,   // Scenes that include menus, settings, pause screens, etc.
        Main,        // Scene specifically for main menu, removes all loaded scenes s
        Cutscene,    // Scenes for cinematic cutscenes
        Loading,     // Scenes that show loading screens or transitions
        Debug        // Any debug-specific scenes, such as debug menus or developer tools
        
    }//end SceneTypes
    
    
    [CreateAssetMenu(fileName = "SceneData", menuName = "SceneData/New SceneData")]
    [System.Serializable]
    public class SceneData : ScriptableObject, IParsable
    {
        private bool _isDataSaved = false;

        [Tooltip("Reference to the scene asset.")]
        public SceneAsset sceneAsset;
    
        [Tooltip("The name of the assembly that this scene belongs to.")]
        public string AssemblyName;
        
        [Tooltip("The name used to load the scene.")]
        public string SceneName; 
    
        [Tooltip("The User-facing scene display title.")]
        public string DisplayName; 
    
        [TextArea(3, 10)]
        [Tooltip("Description of the scene.")]
        public string SceneDescription; 

        [Tooltip("The LoadMode used to load the scene.")]
        public LoadSceneMode LoadMode; 
        
        [Tooltip("The type of scene, for behavioral use.")]
        public SceneType Type;
        
 //       [Tooltip("The type of scene transition to use.")]
 //       public SceneTransitionType TransitionType;


        // Parses fields from a CSV line and assigns them to this item
        public void Parse(string[] fields)
        {
            _isDataSaved = bool.Parse(fields[0]); // Parse _isDataSaved
            AssemblyName = fields[1];
            SceneName = fields[2];
            DisplayName = fields[3];
            SceneDescription = fields[4];
            
            //Parse the LoadMode from an Enum
            if (Enum.TryParse(fields[5], out LoadSceneMode parsedMode))
            {
                LoadMode = parsedMode;
            }
            else
            {
                Debug.LogWarning($"Invalid LoadSceneMode value: {fields[5]}, defaulting to Single.");
                LoadMode = LoadSceneMode.Single; // Default will load in Single mode
                
            }//end if(Enum) 
            //Parse the Scene Type from an Enum
            if (Enum.TryParse(fields[7], out SceneType parsedSceneType))
            {
                Type = parsedSceneType;
            }
            else
            {
                Debug.LogWarning($"Invalid SceneType value: {fields[6]}, defaulting to Gameplay.");
                Type = SceneType.GamePlay; // Default will load in Single mode
                
            }//end if(Enum) 
            
            
            
          /*  
            //Parse the Scene Transition Type from an Enum
            if (Enum.TryParse(fields[6], out SceneTransitionType parsedTransitionType))
            {
                TransitionType = parsedTransitionType;
            }
            else
            {
                Debug.LogWarning($"Invalid SceneTransitionType value: {fields[7]}, defaulting to None.");
                TransitionType = SceneTransitionType.None; // Default to none
                
            }//end if(Enum) 
            */

            
        } //end Parse()

        //Force the scene asset and scene Name to be the same, using OnValidate()
        //OnValidate runs whenever there is a change in the Editor
        private void OnValidate()
        {
            
            if (sceneAsset != null)
            {
                // Extract the scene name from the asset path
                string assetPath = UnityEditor.AssetDatabase.GetAssetPath(sceneAsset);
                SceneName = System.IO.Path.GetFileNameWithoutExtension(assetPath);  // Set the scene name
                this.name = SceneName; //ensure the SceneData asset has the same name as the scene asset
                
            } //end if

        } //end OnValidate()

    }//end SceneData
    
}//end namespace