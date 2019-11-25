﻿/*
 *	Adventure Creator
 *	by Chris Burton, 2013-2019
 *	
 *	"iSaveFileHandler.cs"
 * 
 *	An interface for classes that handle the creation, deletion, and loading of save-game files.  This is can be overriden in the SaveSystem to allow for custom save file handling.
 * 
 */

using System.Collections.Generic;

namespace AC
{

	/**
	 * An interface for classes that handle the creation, deletion, and loading of save-game files.
	 * This is can be overriden in the SaveSystem to allow for custom save file handling.
	 * 
	 * To override the handling of save files, create a new class that implements iSaveFileHandler, and assign it with:
	 * 
	 * \code
	 * SaveSystem.SaveFileHandler = new MyClassName ();
	 * \endcode
	 *
	 * Where MyClassName is the name of your class.
	 *
	 * To have this code run when the game begins, place it in the Awake function of a script in your game's first scene.
	 */
	public interface iSaveFileHandler
	{

		/**
		 * <summary>Returns the default label of a save file. This is not the same as the filename - it is what's displayed in SavesList menus</summary>
		 * <param name = "saveID">The ID number of the save game</param>
		 * <returns>The save file's default label</returns>
		 */
		string GetDefaultSaveLabel (int saveID);

		/**
		 * <summary>Deletes all save files associated with a given profile</summary>
		 * <param name = "profileID">The ID number of the profile to delete saves for, or 0 if profiles are not enabled.</param>
		 */
		void DeleteAll (int profileID);

		/**
		 * <summary>Deletes a single save file</summary>
		 * <param name = "saveFile">The SaveFile container which stores information about the save to delete</param>
		 * <returns>True if the deletion was succesful</returns>
		 */
		bool Delete (SaveFile saveFile);

		/**
		 * <summary>Requests that game data be saved to disk.  When saving is complete, it needs to confirm the output to SaveSystem.OnCompleteSave for the save to be recorded.</summary>
		 * <param name = "saveFile">The SaveFile container which stores information about the file.  Note that only the saveID, profileID and label have been correctly assigned by this point</param>
		 * <param name = "dataToSave">The data to save, as a serialized string</param>
		 */
		void Save (SaveFile saveFile, string dataToSave);

		/**
		 * <summary>Requests that save game data be loaded from disk.  When loading is complete, it needs to be sent back to SaveSystem.ReceiveLoadedData for it to actually be processed.</summary>
		 * <param name = "saveFile">The SaveFile container which stores information about the save to load</param>
		 * <param name = "doLog">If True, a log should be shown in the Console once the data has been read</param>
		 */
		void Load (SaveFile saveFile, bool doLog);

		/**
		 * <summary>Requests that save game data from another game be loaded from disk (to be imported).  When loading is complete, it needs to be sent back to SaveSystem.ReceiveImportedData for it to actually be processed.</summary>
		 * <param name = "saveFile">The SaveFile container which stores information about the save to import</param>
		 * <param name = "doLog">If True, a log should be shown in the Console once the data has been read</param>
		 */
		void Import (SaveFile saveFile, bool doLog);

		/**
		 * <summary>Reads the disk for all save files associated with a given profile</summary>
		 * <param name = "profileID">The ID number of the profile to search save files for, or 0 if profiles are not enabled</param>
		 * <returns>A List of SaveFile instances, with each instance storing information about each found save file</returns>
		 */
		List<SaveFile> GatherSaveFiles (int profileID);

		/**
		 * <summary>Reads the disk for all save files associated with a given profile from another game, to allow for the importing of data between games</summary>
		 * <param name = "profileID">The ID number of the profile to search save files for, or 0 if profiles are not enabled</param>
		 * <param name = "boolID">The ID number of the Global Boolean variable that must be true in the file's data in order for it to be included, or -1 if this feature is not used</param>
		 * <param name = "separateProductName">The 'Product name' of the Unity product to import files from</param>
		 * <param name = "separateFilePrefix">The 'Save filename' field of the AC project to import files from, as set in the Settings Manager</param>
		 * <returns>A List of SaveFile instances, with each instance storing information about each found save file</returns>
		 */
		List<SaveFile> GatherImportFiles (int profileID, int boolID, string separateProductName, string separateFilePrefix);

		/**
		 * <summary>Saves a screenshot associated with a save file</summary>
		 * <param name = "saveFile">A data container for both the texture, and the associate save/profile IDs</param>
		 */
		void SaveScreenshot (SaveFile saveFile);

	}

}