﻿/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2019
 *	
 *	"ActionDialogOptionRename.cs"
 * 
 *	This action changes the text of dialogue options.
 * 
*/

using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionDialogOptionRename : Action, ITranslatable
	{
		
		public int optionID;
		public string newLabel;
		public int lineID;

		public int constantID;
		public Conversation linkedConversation;
		protected Conversation runtimeLinkedConversation;
		
		
		public ActionDialogOptionRename ()
		{
			this.isDisplayed = true;
			category = ActionCategory.Dialogue;
			title = "Rename option";
			description = "Renames the label of a dialogue option.";
		}


		override public void AssignValues ()
		{
			runtimeLinkedConversation = AssignFile <Conversation> (constantID, linkedConversation);
		}

		
		override public float Run ()
		{
			if (runtimeLinkedConversation != null)
			{
				runtimeLinkedConversation.RenameOption (optionID, newLabel, lineID);
			}
			
			return 0f;
		}
		
		
		#if UNITY_EDITOR
		
		public override void ShowGUI ()
		{
			linkedConversation = (Conversation) EditorGUILayout.ObjectField ("Conversation:", linkedConversation, typeof (Conversation), true);

			if (linkedConversation)
			{
				linkedConversation.Upgrade ();
			}

			constantID = FieldToID <Conversation> (linkedConversation, constantID);
			linkedConversation = IDToField <Conversation> (linkedConversation, constantID, true);

			if (linkedConversation)
			{
				optionID = ShowOptionGUI (linkedConversation.options, optionID);
				newLabel = EditorGUILayout.TextField ("New label text:", newLabel);
			}

			AfterRunningOption ();
		}


		private int ShowOptionGUI (List<ButtonDialog> options, int optionID)
		{
			// Create a string List of the field's names (for the PopUp box)
			List<string> labelList = new List<string>();
			
			int i = 0;
			int tempNumber = -1;

			if (options.Count > 0)
			{
				foreach (ButtonDialog option in options)
				{
					string label = option.ID.ToString () + ": " + option.label;
					if (option.label == "")
					{
						label += "(Untitled option)";
					}
					labelList.Add (label);
					
					if (option.ID == optionID)
					{
						tempNumber = i;
					}
					
					i ++;
				}
				
				if (tempNumber == -1)
				{
					// Wasn't found (variable was deleted?), so revert to zero
					if (optionID != 0)
						ACDebug.LogWarning ("Previously chosen option no longer exists!");
					tempNumber = 0;
					optionID = 0;
				}

				tempNumber = EditorGUILayout.Popup (tempNumber, labelList.ToArray());
				optionID = options [tempNumber].ID;
			}
			else
			{
				EditorGUILayout.HelpBox ("No options exist!", MessageType.Info);
				optionID = 0;
				tempNumber = 0;
			}
			
			return optionID;
		}


		override public void AssignConstantIDs (bool saveScriptsToo, bool fromAssetFile)
		{
			if (saveScriptsToo)
			{
				AddSaveScript <RememberConversation> (linkedConversation);
			}
			AssignConstantID <Conversation> (linkedConversation, constantID, 0);
		}


		override public string SetLabel ()
		{
			if (linkedConversation != null)
			{
				return linkedConversation.name;
			}
			return string.Empty;
		}


		public override bool ConvertLocalVariableToGlobal (int oldLocalID, int newGlobalID)
		{
			bool wasAmended = base.ConvertLocalVariableToGlobal (oldLocalID, newGlobalID);

			string updatedLabel = AdvGame.ConvertLocalVariableTokenToGlobal (newLabel, oldLocalID, newGlobalID);
			if (newLabel != updatedLabel)
			{
				wasAmended = true;
				newLabel = updatedLabel;
			}
			return wasAmended;
		}


		public override bool ConvertGlobalVariableToLocal (int oldGlobalID, int newLocalID, bool isCorrectScene)
		{
			bool isAffected = base.ConvertGlobalVariableToLocal (oldGlobalID, newLocalID, isCorrectScene);

			string updatedLabel = AdvGame.ConvertGlobalVariableTokenToLocal (newLabel, oldGlobalID, newLocalID);
			if (newLabel != updatedLabel)
			{
				isAffected = true;
				if (isCorrectScene)
				{
					newLabel = updatedLabel;
				}
			}
			return isAffected;
		}


		public override int GetVariableReferences (List<ActionParameter> parameters, VariableLocation location, int varID, Variables _variables)
		{
			int thisCount = 0;
			string tokenText = AdvGame.GetVariableTokenText (location, varID);

			if (!string.IsNullOrEmpty (tokenText) && newLabel.Contains (tokenText))
			{
				thisCount ++;
			}
			thisCount += base.GetVariableReferences (parameters, location, varID, _variables);
			return thisCount;
		}

		#endif


		#region ITranslatable

		public string GetTranslatableString (int index)
		{
			return newLabel;
		}


		public int GetTranslationID (int index)
		{
			return lineID;
		}


		#if UNITY_EDITOR

		public int GetNumTranslatables ()
		{
			return 1;
		}


		public bool HasExistingTranslation (int index)
		{
			return (lineID > -1);
		}


		public void SetTranslationID (int index, int _lineID)
		{
			lineID = _lineID;
		}


		public string GetOwner (int index)
		{
			return string.Empty;
		}


		public bool OwnerIsPlayer (int index)
		{
			return false;
		}


		public AC_TextType GetTranslationType (int index)
		{
			return AC_TextType.DialogueOption;
		}


		public bool CanTranslate (int index)
		{
			return (!string.IsNullOrEmpty (newLabel));
		}

		#endif

		#endregion


		/**
		 * <summary>Creates a new instance of the 'Conversation: Rename option' Action</summary>
		 * <param name = "conversationToModify">The Conversation to modify</param>
		 * <param name = "dialogueOptionID">The ID number of the dialogue option to rename</param>
		 * <param name = "newLabelText">The option's new display text</param>
		 * <param name = "translationID">The option's translation ID number, as generated by the Speech Manager</param>
		 * <returns>The generated Action</returns>
		 */
		public static ActionDialogOptionRename CreateNew (Conversation conversationToModify, int dialogueOptionID, string newLabelText, int translationID = -1)
		{
			ActionDialogOptionRename newAction = (ActionDialogOptionRename) CreateInstance <ActionDialogOptionRename>();
			newAction.linkedConversation = conversationToModify;
			newAction.optionID = dialogueOptionID;
			newAction.newLabel = newLabelText;
			newAction.lineID = translationID;
			return newAction;
		}

	}

}