﻿/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2019
 *	
 *	"ActionMenuState.cs"
 * 
 *	This Action alters various variables of menus and menu elements.
 * 
 */

using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{
	
	[System.Serializable]
	public class ActionMenuState : Action, ITranslatable
	{
		
		public enum MenuChangeType { TurnOnMenu, TurnOffMenu, HideMenuElement, ShowMenuElement, LockMenu, UnlockMenu, AddJournalPage, RemoveJournalPage };
		public MenuChangeType changeType = MenuChangeType.TurnOnMenu;

		[SerializeField] protected RemoveJournalPageMethod removeJournalPageMethod = RemoveJournalPageMethod.RemoveSinglePage;
		public enum RemoveJournalPageMethod { RemoveSinglePage, RemoveAllPages };

		public string menuToChange = "";
		public int menuToChangeParameterID = -1;
		
		public string elementToChange = "";
		public int elementToChangeParameterID = -1;
		
		public string journalText = "";
		public bool onlyAddNewJournal = false;

		public bool doFade = false;
		public int lineID = -1;

		public int journalPageIndex = -1;
		public int journalPageIndexParameterID = -1;

		protected LocalVariables localVariables;
		protected string runtimeMenuToChange, runtimeElementToChange;


		public ActionMenuState ()
		{
			this.isDisplayed = true;
			lineID = -1;
			category = ActionCategory.Menu;
			title = "Change state";
			description = "Provides various options to show and hide both menus and menu elements.";
		}


		override public void AssignParentList (ActionList actionList)
		{
			if (actionList != null)
			{
				localVariables = UnityVersionHandler.GetLocalVariablesOfGameObject (actionList.gameObject);
			}
			if (localVariables == null)
			{
				localVariables = KickStarter.localVariables;
			}

			base.AssignParentList (actionList);
		}
		
		
		override public void AssignValues (List<ActionParameter> parameters)
		{
			menuToChange = AssignString (parameters, menuToChangeParameterID, menuToChange);
			elementToChange = AssignString (parameters, elementToChangeParameterID, elementToChange);
			journalPageIndex = AssignInteger (parameters, journalPageIndexParameterID, journalPageIndex);

			runtimeMenuToChange = AdvGame.ConvertTokens (menuToChange, Options.GetLanguage (), localVariables, parameters);
			runtimeElementToChange = AdvGame.ConvertTokens (elementToChange, Options.GetLanguage (), localVariables, parameters);
		}
				
		
		override public float Run ()
		{
			if (!isRunning)
			{
				AC.Menu _menu = PlayerMenus.GetMenuWithName (runtimeMenuToChange);

				if (_menu == null)
				{
					if (!string.IsNullOrEmpty (runtimeMenuToChange))
					{
						ACDebug.LogWarning ("Could not find menu of name '" + runtimeMenuToChange + "'", this);
					}
					return 0f;
				}

				isRunning = true;

				switch (changeType)
				{
					case MenuChangeType.TurnOnMenu:
						if (_menu.IsManualControlled ())
						{
							if (!_menu.TurnOn (doFade))
							{
								// Menu is already on
								isRunning = false;
								return 0f;
							}
							
							if (doFade && willWait)
							{
								return _menu.fadeSpeed;
							}
						}
						else
						{
							LogWarning ("Can only turn on Menus with an Appear Type of Manual, OnInputKey, OnContainer or OnViewDocument - did you mean 'Unlock Menu'?");
						}
						break;

					case MenuChangeType.TurnOffMenu:
						if (_menu.IsManualControlled () || _menu.appearType == AppearType.OnInteraction)
						{
							if (!_menu.TurnOff (doFade))
							{
								// Menu is already off
								isRunning = false;
								return 0f;
							}
							
							if (doFade && willWait)
							{
								return _menu.fadeSpeed;
							}
						}
						else
						{
							LogWarning ("Can only turn off Menus with an Appear Type of Manual, OnInputKey, OnContainer or OnViewDocument - did you mean 'Lock Menu'?");
						}
						break;

					case MenuChangeType.LockMenu:
						if (doFade)
						{
							_menu.TurnOff (true);
						}
						else
						{
							_menu.ForceOff ();
						}
						_menu.isLocked = true;
						
						if (doFade && willWait)
						{
							return _menu.fadeSpeed;
						}
						break;

					default:
						RunInstant (_menu);
						break;
				}
			}
			else
			{
				isRunning = false;
				return 0f;
			}
			
			return 0f;
		}
		
		
		override public void Skip ()
		{
			AC.Menu _menu = PlayerMenus.GetMenuWithName (runtimeMenuToChange);
			if (_menu == null)
			{
				if (!string.IsNullOrEmpty (runtimeMenuToChange))
				{
					ACDebug.LogWarning ("Could not find menu of name '" + runtimeMenuToChange + "'");
				}
				return;
			}

			switch (changeType)
			{
				case MenuChangeType.TurnOnMenu:
					if (_menu.IsManualControlled ())
					{
						_menu.TurnOn (false);
					}
					break;

				case MenuChangeType.TurnOffMenu:
					if (_menu.IsManualControlled () || _menu.appearType == AppearType.OnInteraction)
					{
						_menu.ForceOff ();
					}
					break;

				case MenuChangeType.LockMenu:
					_menu.isLocked = true;
					_menu.ForceOff ();
					break;

				default:
					RunInstant (_menu);
					break;
			}
		}


		protected void RunInstant (AC.Menu _menu)
		{
			if (changeType == MenuChangeType.HideMenuElement || changeType == MenuChangeType.ShowMenuElement)
			{
				MenuElement _element = PlayerMenus.GetElementWithName (runtimeMenuToChange, runtimeElementToChange);
				if (_element != null)
				{
					if (changeType == MenuChangeType.HideMenuElement)
					{
						_element.IsVisible = false;
						KickStarter.playerMenus.DeselectInputBox (_element);
					}
					else
					{
						_element.IsVisible = true;
					}
					
					_menu.ResetVisibleElements ();
					_menu.Recalculate ();

					KickStarter.playerMenus.FindFirstSelectedElement ();
				}
				else
				{
					LogWarning ("Could not find element of name '" + elementToChange + "' on menu '" + menuToChange + "'");
				}
			}
			else if (changeType == MenuChangeType.UnlockMenu)
			{
				_menu.isLocked = false;
			}
			else if (changeType == MenuChangeType.AddJournalPage)
			{
				MenuElement _element = PlayerMenus.GetElementWithName (runtimeMenuToChange, runtimeElementToChange);
				if (_element != null)
				{
					if (!string.IsNullOrEmpty (journalText))
					{
						if (_element is MenuJournal)
						{
							MenuJournal journal = (MenuJournal) _element;
							JournalPage newPage = new JournalPage (lineID, journalText);
							journal.AddPage (newPage, onlyAddNewJournal, journalPageIndex);

							if (lineID == -1)
							{
								LogWarning ("The new Journal page has no ID number, and will not be included in save game files - this can be corrected by clicking 'Gather text' in the Speech Manager");
							}
						}
						else
						{
							ACDebug.LogWarning (_element.title + " is not a journal!");
						}
					}
					else
					{
						ACDebug.LogWarning ("No journal text to add!");
					}
				}
				else
				{
					LogWarning ("Could not find menu element of name '" + elementToChange + "' inside '" + menuToChange + "'");
				}
				_menu.Recalculate ();
			}
			else if (changeType == MenuChangeType.RemoveJournalPage)
			{
				MenuElement _element = PlayerMenus.GetElementWithName (runtimeMenuToChange, runtimeElementToChange);
				if (_element != null)
				{
					if (_element is MenuJournal)
					{
						MenuJournal journal = (MenuJournal) _element;

						if (removeJournalPageMethod == RemoveJournalPageMethod.RemoveAllPages)
						{
							journal.RemoveAllPages ();
						}
						else if (removeJournalPageMethod == RemoveJournalPageMethod.RemoveSinglePage)
						{
							journal.RemovePage (journalPageIndex);
						}
					}
					else
					{
						LogWarning (_element.title + " is not a journal!");
					}
				}
				else
				{
					LogWarning ("Could not find menu element of name '" + elementToChange + "' inside '" + menuToChange + "'");
				}
				_menu.Recalculate ();
			}
		}

		
		#if UNITY_EDITOR

		public override void ClearIDs ()
		{
			lineID = -1;
		}

		
		override public void ShowGUI (List<ActionParameter> parameters)
		{
			changeType = (MenuChangeType) EditorGUILayout.EnumPopup ("Change type:", changeType);
			
			if (changeType == MenuChangeType.TurnOnMenu)
			{
				menuToChangeParameterID = Action.ChooseParameterGUI ("Menu to turn on:", parameters, menuToChangeParameterID, ParameterType.String);
				if (menuToChangeParameterID < 0)
				{
					menuToChange = EditorGUILayout.TextField ("Menu to turn on:", menuToChange);
				}
				doFade = EditorGUILayout.Toggle ("Fade?", doFade);
			}
			
			else if (changeType == MenuChangeType.TurnOffMenu)
			{
				menuToChangeParameterID = Action.ChooseParameterGUI ("Menu to turn off:", parameters, menuToChangeParameterID, ParameterType.String);
				if (menuToChangeParameterID < 0)
				{
					menuToChange = EditorGUILayout.TextField ("Menu to turn off:", menuToChange);
				}
				doFade = EditorGUILayout.Toggle ("Fade?", doFade);
			}
			
			else if (changeType == MenuChangeType.HideMenuElement)
			{
				menuToChangeParameterID = Action.ChooseParameterGUI ("Menu containing element:", parameters, menuToChangeParameterID, ParameterType.String);
				if (menuToChangeParameterID < 0)
				{
					menuToChange = EditorGUILayout.TextField ("Menu containing element:", menuToChange);
				}
				
				elementToChangeParameterID = Action.ChooseParameterGUI ("Element to hide:", parameters, elementToChangeParameterID, ParameterType.String);
				if (elementToChangeParameterID < 0)
				{
					elementToChange = EditorGUILayout.TextField ("Element to hide:", elementToChange);
				}
			}
			
			else if (changeType == MenuChangeType.ShowMenuElement)
			{
				menuToChangeParameterID = Action.ChooseParameterGUI ("Menu containing element:", parameters, menuToChangeParameterID, ParameterType.String);
				if (menuToChangeParameterID < 0)
				{
					menuToChange = EditorGUILayout.TextField ("Menu containing element:", menuToChange);
				}
				
				elementToChangeParameterID = Action.ChooseParameterGUI ("Element to show:", parameters, elementToChangeParameterID, ParameterType.String);
				if (elementToChangeParameterID < 0)
				{
					elementToChange = EditorGUILayout.TextField ("Element to show:", elementToChange);
				}
			}
			
			else if (changeType == MenuChangeType.LockMenu)
			{
				menuToChangeParameterID = Action.ChooseParameterGUI ("Menu to lock:", parameters, menuToChangeParameterID, ParameterType.String);
				if (menuToChangeParameterID < 0)
				{
					menuToChange = EditorGUILayout.TextField ("Menu to lock:", menuToChange);
				}
				doFade = EditorGUILayout.Toggle ("Fade?", doFade);
			}
			
			else if (changeType == MenuChangeType.UnlockMenu)
			{
				menuToChangeParameterID = Action.ChooseParameterGUI ("Menu to unlock:", parameters, menuToChangeParameterID, ParameterType.String);
				if (menuToChangeParameterID < 0)
				{
					menuToChange = EditorGUILayout.TextField ("Menu to unlock:", menuToChange);
				}
			}
			
			else if (changeType == MenuChangeType.AddJournalPage)
			{
				if (lineID > -1)
				{
					EditorGUILayout.LabelField ("Speech Manager ID:", lineID.ToString ());
				}
				
				menuToChangeParameterID = Action.ChooseParameterGUI ("Menu containing element:", parameters, menuToChangeParameterID, ParameterType.String);
				if (menuToChangeParameterID < 0)
				{
					menuToChange = EditorGUILayout.TextField ("Menu containing element:", menuToChange);
				}
				
				elementToChangeParameterID = Action.ChooseParameterGUI ("Journal element:", parameters, elementToChangeParameterID, ParameterType.String);
				if (elementToChangeParameterID < 0)
				{
					elementToChange = EditorGUILayout.TextField ("Journal element:", elementToChange);
				}
				
				EditorGUILayout.LabelField ("New page text:");
				journalText = EditorGUILayout.TextArea (journalText);
				onlyAddNewJournal = EditorGUILayout.Toggle ("Only add if not already in?", onlyAddNewJournal);
				if (onlyAddNewJournal && lineID == -1)
				{
					EditorGUILayout.HelpBox ("The page text must be added to the Speech Manager by clicking the 'Gather text' button, in order for duplicates to be prevented.", MessageType.Warning);
				}

				journalPageIndexParameterID = Action.ChooseParameterGUI ("Index to insert into:", parameters, journalPageIndexParameterID, ParameterType.Integer);
				if (journalPageIndexParameterID < 0)
				{
					journalPageIndex = EditorGUILayout.IntField ("Index to insert into:", journalPageIndex);
					EditorGUILayout.HelpBox ("An index value of -1 will add the page to the end of the Journal.", MessageType.Info);
				}
			}

			else if (changeType == MenuChangeType.RemoveJournalPage)
			{
				menuToChangeParameterID = Action.ChooseParameterGUI ("Menu containing element:", parameters, menuToChangeParameterID, ParameterType.String);
				if (menuToChangeParameterID < 0)
				{
					menuToChange = EditorGUILayout.TextField ("Menu containing element:", menuToChange);
				}
				
				elementToChangeParameterID = Action.ChooseParameterGUI ("Journal element:", parameters, elementToChangeParameterID, ParameterType.String);
				if (elementToChangeParameterID < 0)
				{
					elementToChange = EditorGUILayout.TextField ("Journal element:", elementToChange);
				}

				removeJournalPageMethod = (RemoveJournalPageMethod) EditorGUILayout.EnumPopup ("Removal method:", removeJournalPageMethod);
				if (removeJournalPageMethod == RemoveJournalPageMethod.RemoveSinglePage)
				{
					journalPageIndexParameterID = Action.ChooseParameterGUI ("Page number to remove:", parameters, journalPageIndexParameterID, ParameterType.Integer);
					if (journalPageIndexParameterID < 0)
					{
						journalPageIndex = EditorGUILayout.IntField ("Page number to remove:", journalPageIndex);
						EditorGUILayout.HelpBox ("An index value of -1 will remove the last page of the Journal.", MessageType.Info);
					}
				}
			}
			
			if (doFade && (changeType == MenuChangeType.TurnOnMenu || changeType == MenuChangeType.TurnOffMenu || changeType == MenuChangeType.LockMenu))
			{
				willWait = EditorGUILayout.Toggle ("Wait until finish?", willWait);
			}
			
			AfterRunningOption ();
		}
		
		
		public override string SetLabel ()
		{
			string labelAdd = changeType.ToString () + " '" + menuToChange;
			if (changeType == MenuChangeType.HideMenuElement || changeType == MenuChangeType.ShowMenuElement)
			{
				labelAdd += " " + elementToChange;
			}
			return labelAdd;
		}


		public override bool ConvertLocalVariableToGlobal (int oldLocalID, int newGlobalID)
		{
			bool wasAmended = base.ConvertLocalVariableToGlobal (oldLocalID, newGlobalID);

			string updatedJournalText = AdvGame.ConvertLocalVariableTokenToGlobal (journalText, oldLocalID, newGlobalID);
			if (journalText != updatedJournalText)
			{
				wasAmended = true;
				journalText = updatedJournalText;
			}
			return wasAmended;
		}


		public override bool ConvertGlobalVariableToLocal (int oldGlobalID, int newLocalID, bool isCorrectScene)
		{
			bool isAffected = base.ConvertGlobalVariableToLocal (oldGlobalID, newLocalID, isCorrectScene);

			string updatedJournalText = AdvGame.ConvertGlobalVariableTokenToLocal (journalText, oldGlobalID, newLocalID);
			if (journalText != updatedJournalText)
			{
				isAffected = true;
				if (isCorrectScene)
				{
					journalText = updatedJournalText;
				}
			}
			return isAffected;
		}


		public override int GetVariableReferences (List<ActionParameter> parameters, VariableLocation location, int varID, Variables _variables)
		{
			int thisCount = 0;
			string tokenText = AdvGame.GetVariableTokenText (location, varID);
			if (!string.IsNullOrEmpty (tokenText) && journalText.Contains (tokenText))
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
			return journalText;
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


		public bool CanTranslate (int index)
		{
			if (changeType == ActionMenuState.MenuChangeType.AddJournalPage && !string.IsNullOrEmpty (journalText))
			{
				return true;
			}
			return false;
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
			return AC_TextType.JournalEntry;
		}
		
		#endif

		#endregion


		/**
		 * <summary>Creates a new instance of the 'Menu: Change state' Action, set to turn a menu on</summary>
		 * <param name = "menuToTurnOn">The name of the menu to turn on</param>
		 * <param name = "unlockMenu">If True, the menu will be unlocked as well</param>
		 * <param name = "doTransition">If True, the menu will transition on - as opposed to turning on instantly</param>
		 * <param name = "waitUntilFinish">If True, the Action will wait until the transition has completed</param>
		 * <returns>The generated Action</returns>
		 */
		public static ActionMenuState CreateNew_TurnOnMenu (string menuToTurnOn, bool unlockMenu = true, bool doTransition = true, bool waitUntilFinish = false)
		{
			ActionMenuState newAction = (ActionMenuState) CreateInstance <ActionMenuState>();
			newAction.changeType = (unlockMenu) ? MenuChangeType.UnlockMenu : MenuChangeType.TurnOnMenu;
			newAction.menuToChange = menuToTurnOn;
			newAction.doFade = doTransition;
			newAction.willWait = waitUntilFinish;
			return newAction;
		}


		/**
		 * <summary>Creates a new instance of the 'Menu: Change state' Action, set to turn a menu off</summary>
		 * <param name = "menuToTurnOff">The name of the menu to turn off</param>
		 * <param name = "lockMenu">If True, the menu will be locked as well</param>
		 * <param name = "doTransition">If True, the menu will transition off - as opposed to turning off instantly</param>
		 * <param name = "waitUntilFinish">If True, the Action will wait until the transition has completed</param>
		 * <returns>The generated Action</returns>
		 */
		public static ActionMenuState CreateNew_TurnOffMenu (string menuToTurnOff, bool lockMenu = false, bool doTransition = true, bool waitUntilFinish = false)
		{
			ActionMenuState newAction = (ActionMenuState) CreateInstance <ActionMenuState>();
			newAction.changeType = (lockMenu) ? MenuChangeType.LockMenu : MenuChangeType.TurnOffMenu;
			newAction.menuToChange = menuToTurnOff;
			newAction.doFade = doTransition;
			newAction.willWait = waitUntilFinish;
			return newAction;
		}


		/**
		 * <summary>Creates a new instance of the 'Menu: Change state' Action, set to alter a menu element's visibility</summary>
		 * <param name = "menuName">The name of the menu with the element</param>
		 * <param name = "elementToAffect">The name of the element to affect</param>
		 * <param name = "makeVisible">If True, the element will be made visible. Otherwise, it will be made invisible</param>
		 * <returns>The generated Action</returns>
		 */
		public static ActionMenuState CreateNew_SetElementVisibility (string menuName, string elementToAffect, bool makeVisible)
		{
			ActionMenuState newAction = (ActionMenuState) CreateInstance <ActionMenuState>();
			newAction.changeType = (makeVisible) ? MenuChangeType.ShowMenuElement : MenuChangeType.HideMenuElement;
			newAction.menuToChange = menuName;
			newAction.elementToChange = elementToAffect;
			return newAction;
		}


		/**
		 * <summary>Creates a new instance of the 'Menu: Change state' Action, set to add a new page to a journal elemnt</summary>
		 * <param name = "menuName">The name of the menu with the journal</param>
		 * <param name = "journalElementName">The name of the journal element to update</param>
		 * <param name = "newPageText">The text of the new page to add</param>
		 * <param name = "newPageranslationID">The new page's translation ID number, as generated by the Speech Manager</param>
		 * <param name = "pageIndexToInsertInto">The index number of the journal's existing pages to insert the new page into. If negative, it will be inserted at the end</param>
		 * <param name = "avoidDuplicated">If True, the page will only be added if not already present in the journal</param>
		 * <returns>The generated Action</returns>
		 */
		public static ActionMenuState CreateNew_AddJournalPage (string menuName, string journalElementName, string newPageText, int newPageTranslationID = -1, int pageIndexToInsertInto = -1, bool avoidDuplicates = true)
		{
			ActionMenuState newAction = (ActionMenuState) CreateInstance <ActionMenuState>();
			newAction.changeType = MenuChangeType.AddJournalPage;
			newAction.menuToChange = menuName;
			newAction.elementToChange = journalElementName;
			newAction.journalText = newPageText;
			newAction.lineID = newPageTranslationID;
			newAction.journalPageIndex = pageIndexToInsertInto;
			newAction.onlyAddNewJournal = avoidDuplicates;
			return newAction;
		}


		public static ActionMenuState CreateNew_RemoveJournalPage (string menuName, string journalElementName, RemoveJournalPageMethod removeJournalPageMethod = RemoveJournalPageMethod.RemoveSinglePage, int pageIndexToRemove = -1)
		{
			ActionMenuState newAction = (ActionMenuState) CreateInstance <ActionMenuState>();
			newAction.changeType = MenuChangeType.RemoveJournalPage;
			newAction.menuToChange = menuName;
			newAction.elementToChange = journalElementName;
			newAction.removeJournalPageMethod = removeJournalPageMethod;
			newAction.journalPageIndex = pageIndexToRemove;
			return newAction;
		}

	}
	
}