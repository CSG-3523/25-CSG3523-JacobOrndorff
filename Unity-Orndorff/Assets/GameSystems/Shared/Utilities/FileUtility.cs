/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Dynamic Inventory
* FILE NAME       : FilesUtility.cs
* DESCRIPTION     : Utility class to handle the file path and asset directory logic
*
* REVISION HISTORY:
* Date             Author                    Comments
* ---------------------------------------------------------------------------
* 2005/04/05      Akram Taghavi-Burris      Created Interface
*
*
/******************************************************************/

using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameSystems.Shared.Utilities
{
    //Utility classes are typically static as there are no instances
    public static class FileUtility
    {
        // Returns the full path to the file inside the StreamingAssets folder.
        public static string GetFilePath(string fileName)
        {
            return Path.Combine(Application.streamingAssetsPath, fileName);
        }

        // Check and ensure the directory exists, if not, create it
        private static string CheckAssetDirectory(string assetDirectory)
        {
            // Split the directory path by slashes to get each folder in the path
            string[] folders = assetDirectory.Split('/');

            // Ensure that the parent folder "Assets" exists (should always exist in Unity)
            string currentPath = "Assets";

            // Iterate through each folder in the path and create it if it doesn't exist
            foreach (string folder in folders)
            {
                // Combine the current path with the next folder
                currentPath = Path.Combine(currentPath, folder);

                // Check if the folder exists; if not, create it
                if (!AssetDatabase.IsValidFolder(currentPath))
                {
                    // Extract the parent folder and the folder name to create
                    string parentFolder = Path.GetDirectoryName(currentPath);
                    string folderName = Path.GetFileName(currentPath);

                    // Create the folder inside its parent folder
                    AssetDatabase.CreateFolder(parentFolder, folderName);
                } //end 

            } //end For(folder)
        
            return currentPath;
        
        }//end CheckAssetDirectory()


    }//end FileUtility

    
}//end Namespace