/*******************************************************************
* COPYRIGHT       : 2025
* PROJECT         : Game Systems
* FILE NAME       : DataLoader.cs
* DESCRIPTION     : Base class or loading data from csv files to scriptable objects
*                    
* REVISION HISTORY:
* Date            Author                  Comments
* ---------------------------------------------------------------------------
* 2005/04/05      Akram Taghavi-Burris      Created Class
* 
*
/******************************************************************/

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using GameSystems.Shared.Interfaces;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameSystems.Shared.BaseClasses 
{
    public class DataLoader<T> : MonoBehaviour where T : ScriptableObject
    {
        [Tooltip("CSV file name including extension (e.g., data.csv)")] [SerializeField]
        private string _fileName;

        [Tooltip("The directory to save new data assets (e.g. Inventory/Data")] [SerializeField]
        private string _assetDirectory;

        private string _filePath;


        // Returns the full path to the CSV file inside the StreamingAssets folder.
        private string GetFilePath()
        {
            return Path.Combine(Application.streamingAssetsPath, _fileName);
        } //end GetFilePath()


        // Loads data from a CSV file and creates ScriptableObject assets
        protected List<T> LoadData()
        {
            //Get the csv file path
            _filePath = GetFilePath();

            //Create a list for the data
            List<T> dataList = new List<T>();


            if (!File.Exists(_filePath))
            {
                Debug.LogError($"CSV file not found at: {_filePath}");
                return dataList;

            } //end if(!File)

            //Place all lines of the csv file into a string array
            string[] csvLines = File.ReadAllLines(_filePath);

            //Add to dataList after processing the lines
            dataList = ProcessCSVLines(csvLines);

            //if in editor create an asset and refresh 
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif

            Debug.Log("Data loaded and assets created successfully.");
            return dataList;
        } //end LoadData()


        //Process each line of the csv file into individual fields
        private List<T> ProcessCSVLines(string[] csvLines)
        {
            List<T> dataList = new List<T>();

            //For each line of the csv file
            for (int i = 1; i < csvLines.Length; i++)
            {
                //Add line contents into a string array, for each item separated by a comma
                string[] fields = csvLines[i].Split(',');

                //Create a new asset for the data entry, passing the csv fields into the appropriate fields of the data
                T dataEntry = CreateDataFromFields(fields);

                //If the data entry (asset) is not null save the asset and add it to the data list
                if (dataEntry != null)
                {
                    // Save the asset to the directory
                    SaveAsset(dataEntry);
                    dataList.Add(dataEntry);

                } //end if(!null)
            } //end for

            return dataList;
        } //end ProcessCSVLines()


        // Using T as the type and calling the Parse method for specific field assignment
        protected virtual T CreateDataFromFields(string[] fields)
        {
            // Use reflection to get the properties of T
            var properties = typeof(T).GetProperties();

            if (fields.Length < properties.Length) // Ensure there are enough fields
            {
                Debug.LogWarning("Invalid CSV row: not enough fields.");
                return null;
            }

            // Create a new instance of the generic ScriptableObject type T
            T dataEntry = ScriptableObject.CreateInstance<T>();

            // Check if the dataEntry implements IParsable and call Parse if it does
            if (dataEntry is IParsable parsableData)
            {
                parsableData.Parse(fields); // Parse the fields for the specific ScriptableObject
            }

#if UNITY_EDITOR
            // Specify asset directory and ensure it exists
            string assetDirectory = CheckAssetDirectory(_assetDirectory);
            Debug.Log($"Asset directory path: {assetDirectory}");
#endif

            // Set the name for the data entry (e.g., using the name field)
            dataEntry.name = fields[1]; // Assuming the second field is a valid name

            return dataEntry;
        } //end CreateDataFromFields()


        private string CheckAssetDirectory(string assetDirectory)
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

        } //end CheckAssetDirectory()

        //Save new asset from csv file data 
        private void SaveAsset(ScriptableObject dataEntry)
        {
#if UNITY_EDITOR
            // Ensure the asset directory exists
            string assetDirectory = CheckAssetDirectory(_assetDirectory);

            // Create the path where the asset will be saved
            string path = Path.Combine(assetDirectory, $"{dataEntry.name}.asset");

            // Create the asset at the specified path
            AssetDatabase.CreateAsset(dataEntry, path);

            // Save the asset
            AssetDatabase.SaveAssets();
#endif
        } //end SaveAsset()

    } //end DataLoader
    
}//end Namespace