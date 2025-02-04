﻿/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2019
 *	
 *	"MenuManager.cs"
 * 
 *	This script handles the "Menu" tab of the main wizard.
 *	It is used to define the menus that make up the game's GUI.
 * 
 */

using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	/**
	 * Handles the "Menu" tab of the Game Editor window.
	 * All Menus are defined here.
	 */
	[System.Serializable]
	public class MenuManager : ScriptableObject
	{

		/** The EventSystem to instantiate when Unity UI-based Menus are used */
		public UnityEngine.EventSystems.EventSystem eventSystem;
		/** The game's full list of menus */
		public List<Menu> menus = new List<Menu>();
		/** The depth at which to draw OnGUI-based (Adventure Creator) Menus */
		public int globalDepth;
		/** A texture to apply full-screen when a 'Pause' Menu is enabled */
		public Texture2D pauseTexture = null;
		/** If True, then the size of text effects (shadows, outlines) will be based on the size of the text, rather than fixed */
		public bool scaleTextEffects = false;
		/** If True, then Menus will be navigated directly, not with the cursor, when the game is paused (if inputMethod = InputMethod.KeyboardAndController in SettingsManager) */
		public bool keyboardControlWhenPaused = true;
		/** If True, then Menus will be navigated directly, not with the cursor, when Conversation dialogue options are shown (if inputMethod = InputMethod.KeyboardAndController in SettingsManager) */
		public bool keyboardControlWhenDialogOptions = true;
		/** If True, then the simulated cursor will auto-select valid Unity UI elements */
		public bool autoSelectValidRaycasts = false;

		#if UNITY_EDITOR

		public bool doWindowsPreviewFix = true;

		public bool drawOutlines = true;
		public bool drawInEditor = false;

		public static Menu copiedMenu = null;
		public static MenuElement copiedElement = null;

		private Menu selectedMenu = null;
		private MenuElement selectedMenuElement = null;
		private int sideMenu = -1;
		private int sideElement = -1;

		private bool showSettings = true;
		private bool showMenuList = true;
		private bool showMenuProperties = true;
		private bool showElementList = true;
		private bool showElementProperties = true;

		private Vector2 scrollPos;
		private Vector2 elementScrollPos;
		private string nameFilter = "";
		private bool oldVisibility;
		private int typeNumber = 0;
		private string[] elementTypes = { "Button", "Crafting", "Cycle", "DialogList", "Drag", "Graphic", "Input", "Interaction", "InventoryBox", "Journal", "Label", "ProfilesList", "SavesList", "Slider", "Timer", "Toggle" };


		private void OnEnable ()
		{
			if (menus == null)
			{
				menus = new List<Menu>();
			} 
		}


		/**
		 * Shows the GUI.
		 */
		public void ShowGUI ()
		{
			EditorGUILayout.BeginVertical (CustomStyles.thinBox);

			showSettings = CustomGUILayout.ToggleHeader (showSettings, "Global menu settings");
			if (showSettings)
			{
				drawInEditor = CustomGUILayout.Toggle ("Preview in Game window?", drawInEditor, "", "If True, Adventure Creator-sourced menus will be displayed in the Game window in Edit mode so long as a GameEngine prefab is present in the scene.");
				if (drawInEditor)
				{
					drawOutlines = CustomGUILayout.Toggle ("Draw outlines?", drawOutlines, "", "If True, yellow outlines will be drawn around Adventure Creator-sourced menus and elements when previewing");
		            if (drawOutlines && Application.platform == RuntimePlatform.WindowsEditor)
					{
						doWindowsPreviewFix = CustomGUILayout.Toggle ("Apply outline offset fix?", doWindowsPreviewFix, "", "In some versions of Windows, preview outlines can appear offset. Checking this box should fix this.");
					}
				}
				scaleTextEffects = CustomGUILayout.Toggle ("Scale text effects?", scaleTextEffects, "AC.KickStarter.menuManager.scaleTextEffects", "If True, then the size of text effects (shadows, outlines) will be based on the size of the text, rather than fixed");
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField (new GUIContent ("Pause background texture:", "A texture to apply full-screen when a 'Pause' Menu is enabled"), GUILayout.Width (255f));
				pauseTexture = (Texture2D) CustomGUILayout.ObjectField <Texture2D> (pauseTexture, false, GUILayout.Width (70f), GUILayout.Height (30f), "AC.Kickstarter.menuManager.pauseTexture");
				EditorGUILayout.EndHorizontal ();
				globalDepth = CustomGUILayout.IntField ("GUI depth:", globalDepth, "AC.KickStarter.menuManager.globalDepth", "The depth at which to draw OnGUI-based (Adventure Creator) menus");
				eventSystem = (UnityEngine.EventSystems.EventSystem) CustomGUILayout.ObjectField <UnityEngine.EventSystems.EventSystem> ("Event system prefab:", eventSystem, false, "AC.KickStarter.menuManager.eventSystem", "The EventSystem to instantiate when Unity UI-based Menus are used. If none is set, a default one will be used");

				if (AdvGame.GetReferences ().settingsManager != null && AdvGame.GetReferences ().settingsManager.inputMethod != InputMethod.TouchScreen)
				{
					EditorGUILayout.Space ();
					keyboardControlWhenPaused = CustomGUILayout.ToggleLeft ("Directly-navigate Menus when paused?", keyboardControlWhenPaused, "AC.KickStarter.menuManager.keyboardControlWhenPaused", "If True, then Menus will be navigated directly, not with the cursor, when the game is paused");
					keyboardControlWhenDialogOptions = CustomGUILayout.ToggleLeft ("Directly-navigate Menus during Conversations?", keyboardControlWhenDialogOptions, "AC.KickStarter.menuManager.keyboardControlWhenDialogOptions", "If True, then Menus will be navigated directly, not with the cursor, when Conversation dialogue options are shown");

					if (AdvGame.GetReferences ().settingsManager != null && AdvGame.GetReferences ().settingsManager.inputMethod == InputMethod.KeyboardOrController)
					{
						autoSelectValidRaycasts = CustomGUILayout.ToggleLeft ("Auto-select valid UI raycasts?", autoSelectValidRaycasts, "AC.KickStarter.menuManager.autoSelectValidRaycasts", "If True, then the simulated cursor will auto-select valid Unity UI elements");
					}
				}


				if (drawInEditor && KickStarter.menuPreview == null)
				{	
					EditorGUILayout.HelpBox ("A GameEngine prefab is required to display menus while editing - please click Organise Room Objects within the Scene Manager.", MessageType.Warning);
				}
				else if (Application.isPlaying)
				{
					EditorGUILayout.HelpBox ("Changes made to the menus will not be registed by the game until the game is restarted.", MessageType.Info);
				}
			}
			EditorGUILayout.EndVertical ();
			
			EditorGUILayout.Space ();

			CreateMenusGUI ();

			if (selectedMenu != null && menus.Contains (selectedMenu))
			{
				EditorGUILayout.Space ();
				
				string menuTitle = selectedMenu.title;
				if (menuTitle == "")
				{
					menuTitle = "(Untitled)";
				}
				
				EditorGUILayout.BeginVertical (CustomStyles.thinBox);

				showMenuProperties = CustomGUILayout.ToggleHeader (showMenuProperties, "Menu " + selectedMenu.id + ": '" + menuTitle + "' properties");
				if (showMenuProperties)
				{
					selectedMenu.ShowGUI ();
				}
				EditorGUILayout.EndVertical ();
				
				EditorGUILayout.Space ();
				
				EditorGUILayout.BeginVertical (CustomStyles.thinBox);
				showElementList = CustomGUILayout.ToggleHeader (showElementList, "Menu " + selectedMenu.id + ": '" + menuTitle + "' elements");
				if (showElementList)
				{
					CreateElementsGUI (selectedMenu);
				}
				EditorGUILayout.EndVertical ();
				
				if (selectedMenuElement != null && selectedMenu.elements.Contains (selectedMenuElement))
				{
					EditorGUILayout.Space ();
					
					string elementName = selectedMenuElement.title;
					if (elementName == "")
					{
						elementName = "(Untitled)";
					}
					
					string elementType = "";
					foreach (string _elementType in elementTypes)
					{
						if (selectedMenuElement.GetType ().ToString ().Contains (_elementType))
						{
							elementType = _elementType;
							break;
						}
					}

					EditorGUILayout.BeginVertical (CustomStyles.thinBox);
					showElementProperties = CustomGUILayout.ToggleHeader (showElementProperties, elementType + " " + selectedMenuElement.ID + ": '" + elementName + "' properties");
					if (showElementProperties)
					{
						oldVisibility = selectedMenuElement.IsVisible;
						selectedMenuElement.ShowGUIStart (selectedMenu);
					}
					else
					{
						EditorGUILayout.EndVertical ();
					}
					if (selectedMenuElement.IsVisible != oldVisibility)
					{
						if (!Application.isPlaying)
						{
							selectedMenu.Recalculate ();
						}
					}
				}
			}
			
			if (GUI.changed)
			{
				if (!Application.isPlaying)
				{
					SaveAllMenus ();
				}
				EditorUtility.SetDirty (this);
			}
		}
		
		
		private void SaveAllMenus ()
		{
			#if !UNITY_5_4_OR_NEWER
			foreach (AC.Menu menu in menus)
			{
				if (!Application.isPlaying)
				{
					menu.Recalculate ();
				}
			}
			#endif
		}
		
		
		private void CreateMenusGUI ()
		{
			EditorGUILayout.BeginVertical (CustomStyles.thinBox);
			showMenuList = CustomGUILayout.ToggleHeader (showMenuList, "Menus");
			if (showMenuList)
			{
				if (menus != null && menus.Count > 1)
				{
					nameFilter = EditorGUILayout.TextField ("Filter by name:", nameFilter);
					EditorGUILayout.Space ();
				}

				int numInFilter = 0;
				foreach (AC.Menu _menu in menus)
				{
					if (_menu == null)
					{
						menus.Remove (_menu);
						CleanUpAsset ();
						EditorGUILayout.EndVertical ();
						return;
					}

					_menu.showInFilter = false;
					if (nameFilter == "" || _menu.title.ToLower ().Contains (nameFilter.ToLower ()))
					{
						_menu.showInFilter = true;
						numInFilter ++;
					}
				}

				if (numInFilter > 0)
				{
					scrollPos = EditorGUILayout.BeginScrollView (scrollPos, GUILayout.Height (Mathf.Min (numInFilter * 21, 295f)+5));

					foreach (AC.Menu _menu in menus)
					{
						if (_menu.showInFilter)
						{
							EditorGUILayout.BeginHorizontal ();
						
							string buttonLabel = _menu.title;
							if (buttonLabel == "")
							{
								buttonLabel = "(Untitled)";	
							}
							if (GUILayout.Toggle (selectedMenu == _menu, buttonLabel, "Button"))
							{
								if (selectedMenu != _menu)
								{
									DeactivateAllMenus ();
									ActivateMenu (_menu);
								}
							}

							if (GUILayout.Button (string.Empty, CustomStyles.IconCog))
							{
								SideMenu (_menu);
							}
					
							EditorGUILayout.EndHorizontal ();
						}
					}

					EditorGUILayout.EndScrollView ();
					EditorGUILayout.HelpBox ("Filtering " + numInFilter + " out of " + menus.Count + " menus.", MessageType.Info);
				}
				else if (menus.Count > 0)
				{
					EditorGUILayout.HelpBox ("No Menus that match the above filters have been created.", MessageType.Info);
				}

				EditorGUILayout.Space ();

				EditorGUILayout.BeginHorizontal ();
				if (GUILayout.Button ("Create new menu"))
				{
					Undo.RecordObject (this, "Add menu");
					
					Menu newMenu = (Menu) CreateInstance <Menu>();
					newMenu.Declare (GetIDArray ());
					menus.Add (newMenu);
					
					DeactivateAllMenus ();
					ActivateMenu (newMenu);
					
					newMenu.hideFlags = HideFlags.HideInHierarchy;
					AssetDatabase.AddObjectToAsset (newMenu, this);
					AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (newMenu));
					AssetDatabase.SaveAssets ();
					CleanUpAsset ();
				}
				if (MenuManager.copiedMenu == null)
				{
					GUI.enabled = false;
				}
				if (GUILayout.Button ("Paste menu"))
				{
					PasteMenu ();
				}
				GUI.enabled = true;
				EditorGUILayout.EndHorizontal ();
			}
			EditorGUILayout.EndVertical ();
		}


		private void CleanUpAsset ()
		{
			string assetPath = AssetDatabase.GetAssetPath (this);
			Object[] objects = AssetDatabase.LoadAllAssetsAtPath (assetPath);

			foreach (Object _object in objects)
			{
				if (_object is Menu)
				{
					AC.Menu _menu = (Menu) _object;

					bool found = false;
					foreach (AC.Menu menu in menus)
					{
						if (menu == _menu)
						{
							_object.hideFlags = HideFlags.HideInHierarchy;
							found = true;
							break;
						}
					}

					if (!found)
					{
						ACDebug.Log ("Deleted unset menu: " + _menu.title);
						DestroyImmediate (_object, true);
					}

					for (int i=0; i<_menu.elements.Count; i++)
					{
						if (_menu.elements[i] == null)
						{
							_menu.elements.RemoveAt (i);
							i=0;
						}
					}
				}
			}

			foreach (Object _object in objects)
			{
				if (_object is MenuElement)
				{
					MenuElement _element = (MenuElement) _object;

					bool found = false;
					foreach (AC.Menu menu in menus)
					{
						foreach (MenuElement element in menu.elements)
						{
							if (element == _element)
							{
								_object.hideFlags = HideFlags.HideInHierarchy;
								found = true;
								break;
							}
						}
					}

					if (!found)
					{
						ACDebug.Log ("Deleted unset element: " + _element.title);
						DestroyImmediate (_object, true);
					}
				}
			}

			AssetDatabase.SaveAssets ();
		}


		/**
		 * <summary>Selects a MenuElement within a Menu and display its properties.</summary>
		 * <param name = "_menu">The Menu that the MenuElement is a part of</param>
		 * <param name = "_element">The MenuElement to select</param>
		 */
		public void SelectElementFromPreview (AC.Menu _menu, MenuElement _element)
		{
			if (_menu.elements.Contains (_element))
			{
				if (selectedMenuElement != _element)
				{
					DeactivateAllElements (_menu);
					ActivateElement (_element);
				}
			}
		}
		
		
		private void CreateElementsGUI (AC.Menu _menu)
		{	
			if (_menu.elements != null && _menu.elements.Count > 0)
			{
				elementScrollPos = EditorGUILayout.BeginScrollView (elementScrollPos, GUILayout.Height (Mathf.Min (_menu.elements.Count * 21, 295f)+5));

				foreach (MenuElement _element in _menu.elements)
				{
					if (_element != null)
					{
						string elementName = _element.title;
						
						if (elementName == "")
						{
							elementName = "(Untitled)";
						}
						
						EditorGUILayout.BeginHorizontal ();
						
						if (GUILayout.Toggle (selectedMenuElement == _element, elementName, "Button"))
						{
							if (selectedMenuElement != _element)
							{
								DeactivateAllElements (_menu);
								ActivateElement (_element);
							}
						}

						if (GUILayout.Button ("", CustomStyles.IconCog))
						{
							SideMenu (_menu, _element);
						}
					
						EditorGUILayout.EndHorizontal ();
					}
				}

				EditorGUILayout.EndScrollView ();
			}

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Element type:", GUILayout.Width (80f));
			typeNumber = EditorGUILayout.Popup (typeNumber, elementTypes);
			
			if (GUILayout.Button ("Add new"))
			{
				AddElement (elementTypes[typeNumber], _menu);
			}

			if (copiedElement != null)
			{
				if (GUILayout.Button ("Paste"))
				{
					PasteElement (menus.IndexOf (_menu), _menu.elements.Count -1);
				}
			}
			EditorGUILayout.EndHorizontal ();
		}
		
		
		private void ActivateMenu (AC.Menu menu)
		{
			selectedMenu = menu;
		}
		
		
		private void DeactivateAllMenus ()
		{
			selectedMenu = null;
			selectedMenuElement = null;
			EditorGUIUtility.editingTextField = false;
		}
		
		
		private void ActivateElement (MenuElement menuElement)
		{
			selectedMenuElement = menuElement;
		}
		
		
		private void DeleteAllElements (AC.Menu menu)
		{
			foreach (MenuElement menuElement in menu.elements)
			{
				UnityEngine.Object.DestroyImmediate (menuElement, true);
				AssetDatabase.SaveAssets();
			}
			CleanUpAsset ();
		}
		
		
		private void DeactivateAllElements (AC.Menu menu)
		{
			foreach (MenuElement menuElement in menu.elements)
			{
				if (menuElement != null)
				EditorGUIUtility.editingTextField = false;
			}
		}


		private int[] GetElementIDArray (int i)
		{
			// Returns a list of id's in the list
			List<int> idArray = new List<int>();
			
			foreach (MenuElement _element in menus[i].elements)
			{
				if (_element != null)
				{
					idArray.Add (_element.ID);
				}
			}
			
			idArray.Sort ();
			return idArray.ToArray ();
		}
					
		
		private int[] GetIDArray ()
		{
			// Returns a list of id's in the list
			List<int> idArray = new List<int>();
			
			foreach (AC.Menu menu in menus)
			{
				if (menu != null)
				{
					idArray.Add (menu.id);
				}
			}
			
			idArray.Sort ();
			return idArray.ToArray ();
		}
		
		
		private void AddElement (string className, AC.Menu _menu)
		{
			Undo.RecordObject (_menu, "Add element");

			List<int> idArray = new List<int>();
			
			foreach (MenuElement _element in _menu.elements)
			{
				if (_element != null)
				{
					idArray.Add (_element.ID);
				}
			}
			idArray.Sort ();
			
			className = "Menu" + className;
			MenuElement newElement = (MenuElement) CreateInstance (className);
			newElement.Declare ();
			newElement.title = className.Substring (4);
			
			// Update id based on array
			foreach (int _id in idArray.ToArray())
			{
				if (newElement.ID == _id)
				{
					newElement.ID ++;
				}
			}
			
			_menu.elements.Add (newElement);
			if (!Application.isPlaying)
			{
				_menu.Recalculate ();
			}
			DeactivateAllElements (_menu);
			selectedMenuElement = newElement;

			newElement.hideFlags = HideFlags.HideInHierarchy;
			AssetDatabase.AddObjectToAsset (newElement, this);
			AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (newElement));
			AssetDatabase.SaveAssets ();

			CleanUpAsset ();
		}


		private void PasteMenu ()
		{
			PasteMenu (menus.Count-1);
		}


		private void PasteMenu (int i)
		{
			if (MenuManager.copiedMenu != null)
			{
				Undo.RecordObject (this, "Paste menu");
				
				Menu newMenu = (Menu) CreateInstance <Menu>();
				newMenu.Declare (GetIDArray ());
				int newMenuID = newMenu.id;
				newMenu.Copy (MenuManager.copiedMenu, true);
				newMenu.Recalculate ();
				newMenu.id = newMenuID;

				foreach (Menu menu in menus)
				{
					if (menu.title == newMenu.title)
					{
						newMenu.title += " (Copy)";
						break;
					}
				}

				menus.Insert (i+1, newMenu);
				
				DeactivateAllMenus ();
				ActivateMenu (newMenu);

				newMenu.hideFlags = HideFlags.HideInHierarchy;
				AssetDatabase.AddObjectToAsset (newMenu, this);
				AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (newMenu));

				foreach (MenuElement newElement in newMenu.elements)
				{
					newElement.hideFlags = HideFlags.HideInHierarchy;
					AssetDatabase.AddObjectToAsset (newElement, this);
					AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (newElement));
				}

				AssetDatabase.SaveAssets ();
				CleanUpAsset ();
			}
		}


		private void PasteElement (int menuIndex, int elementIndex)
		{
			if (MenuManager.copiedElement != null)
			{
				Undo.RegisterCompleteObjectUndo (menus[menuIndex], "Paste menu element");
             	
				int[] idArray = GetElementIDArray (menuIndex);

				MenuElement newElement = MenuManager.copiedElement.DuplicateSelf (true, false);
				newElement.linkedUiID = 0;

				foreach (MenuElement menuElement in menus[menuIndex].elements)
				{
					if (menuElement.title == newElement.title)
					{
						newElement.title += " (Copy)";
						break;
					}
				}

				newElement.UpdateID (idArray);
				newElement.lineID = -1;
				newElement.hideFlags = HideFlags.HideInHierarchy;
				menus[menuIndex].elements.Insert (elementIndex+1, newElement);

				AssetDatabase.AddObjectToAsset (newElement, this);
				AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (newElement));
				AssetDatabase.SaveAssets ();

				CleanUpAsset ();

				EditorUtility.SetDirty (this);
			}
		}


		private void SideMenu (AC.Menu _menu)
		{
			GenericMenu menu = new GenericMenu ();
			sideMenu = menus.IndexOf (_menu);

			menu.AddItem (new GUIContent ("Insert after"), false, MenuCallback, "Insert after");
			if (menus.Count > 0)
			{
				menu.AddItem (new GUIContent ("Delete"), false, MenuCallback, "Delete");
			}

			menu.AddSeparator (string.Empty);
			menu.AddItem (new GUIContent ("Copy"), false, MenuCallback, "Copy");
			if (MenuManager.copiedMenu != null)
			{
				menu.AddItem (new GUIContent ("Paste after"), false, MenuCallback, "Paste after");
			}

			if (sideMenu > 0 || sideMenu < menus.Count-1)
			{
				menu.AddSeparator (string.Empty);
				if (sideMenu > 0)
				{
					menu.AddItem (new GUIContent ("Re-arrange/Move to top"), false, MenuCallback, "Move to top");
					menu.AddItem (new GUIContent ("Re-arrange/Move up"), false, MenuCallback, "Move up");
				}
				if (sideMenu < menus.Count-1)
				{
					menu.AddItem (new GUIContent ("Re-arrange/Move down"), false, MenuCallback, "Move down");
					menu.AddItem (new GUIContent ("Re-arrange/Move to bottom"), false, MenuCallback, "Move to bottom");
				}
			}
			
			menu.ShowAsContext ();
		}


		private void MenuCallback (object obj)
		{
			if (sideMenu >= 0)
			{
				switch (obj.ToString ())
				{
				case "Copy":
					MenuManager.copiedMenu = (Menu) CreateInstance <Menu>();
					MenuManager.copiedMenu.Copy (menus[sideMenu], true);
					break;

				case "Paste after":
					PasteMenu (sideMenu);
					break;

				case "Insert after":
					Undo.RecordObject (this, "Insert menu");
					Menu newMenu = (Menu) CreateInstance <Menu>();
					newMenu.Declare (GetIDArray ());
					menus.Insert (sideMenu+1, newMenu);
					
					DeactivateAllMenus ();
					ActivateMenu (newMenu);

					newMenu.hideFlags = HideFlags.HideInHierarchy;
					AssetDatabase.AddObjectToAsset (newMenu, this);
					AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (newMenu));
					break;
					
				case "Delete":
					Undo.RegisterCompleteObjectUndo (this, "Delete menu");
					Undo.RegisterCompleteObjectUndo (menus[sideMenu], "Delete menu");
					for (int i=0; i<menus[sideMenu].elements.Count; i++)
					{
						if (menus[sideMenu].elements[i] != null)
						{
							Undo.RegisterCompleteObjectUndo (menus[sideMenu].elements[i], "Delete menu");
						}
					}
             		
					if (menus[sideMenu] == selectedMenu)
					{
						DeactivateAllElements (menus[sideMenu]);
						DeleteAllElements (menus[sideMenu]);
						selectedMenuElement = null;
					}
					DeactivateAllMenus ();
					Menu tempMenu = menus[sideMenu];
					foreach (MenuElement element in tempMenu.elements)
					{
						if (element != null)
						{
							Undo.DestroyObjectImmediate (element);
						}
					}
					Undo.SetCurrentGroupName ("Delete menu '" + tempMenu.title + "'");

					menus.RemoveAt (sideMenu);
					Undo.DestroyObjectImmediate (tempMenu);
					AssetDatabase.SaveAssets ();
					CleanUpAsset ();
					break;
					
				case "Move up":
					Undo.RecordObject (this, "Move menu up");
					menus = SwapMenus (menus, sideMenu, sideMenu-1);
					menus[sideMenu].ResetVisibleElements ();
					AssetDatabase.SaveAssets ();
					break;
					
				case "Move down":
					Undo.RecordObject (this, "Move menu down");
					menus = SwapMenus (menus, sideMenu, sideMenu+1);
					menus[sideMenu].ResetVisibleElements ();
					AssetDatabase.SaveAssets ();
					break;

				case "Move to top":
					Undo.RecordObject (this, "Move menu to top");
					menus = MoveMenuToTop (menus, sideMenu);
					menus[sideMenu].ResetVisibleElements ();
					AssetDatabase.SaveAssets ();
					break;
				
				case "Move to bottom":
					Undo.RecordObject (this, "Move menu to bottom");
					menus = MoveMenuToBottom (menus, sideMenu);
					menus[sideMenu].ResetVisibleElements ();
					AssetDatabase.SaveAssets ();
					break;
				}
			}
			
			sideMenu = -1;
			sideElement = -1;
			SaveAllMenus ();

			EditorUtility.SetDirty (this);
		}


		private void SideMenu (AC.Menu _menu, MenuElement _element)
		{
			GenericMenu menu = new GenericMenu ();
			sideElement = _menu.elements.IndexOf (_element);
			sideMenu = menus.IndexOf (_menu);
			
			if (_menu.elements.Count > 0)
			{
				menu.AddItem (new GUIContent ("Delete"), false, ElementCallback, "Delete");
			}

			menu.AddSeparator ("");
			menu.AddItem (new GUIContent ("Copy"), false, ElementCallback, "Copy");
			if (MenuManager.copiedElement != null)
			{
				menu.AddItem (new GUIContent ("Paste after"), false, ElementCallback, "Paste after");
			}
			if (sideElement > 0 || sideElement < _menu.elements.Count-1)
			{
				menu.AddSeparator ("");
			}

			if (sideElement > 0)
			{
				menu.AddItem (new GUIContent ("Re-arrange/Move to top"), false, ElementCallback, "Move to top");
				menu.AddItem (new GUIContent ("Re-arrange/Move up"), false, ElementCallback, "Move up");
			}
			if (sideElement < _menu.elements.Count-1)
			{
				menu.AddItem (new GUIContent ("Re-arrange/Move down"), false, ElementCallback, "Move down");
				menu.AddItem (new GUIContent ("Re-arrange/Move to bottom"), false, ElementCallback, "Move to bottom");
			}
			
			menu.ShowAsContext ();
		}
		
		
		private void ElementCallback (object obj)
		{
			if (sideElement >= 0 && sideMenu >= 0)
			{
				switch (obj.ToString ())
				{
				case "Copy":
					MenuManager.copiedElement = menus[sideMenu].elements[sideElement].DuplicateSelf (true, false);
					break;
					
				case "Paste after":
					PasteElement (sideMenu, sideElement);
					break;

				case "Delete":
					Undo.RegisterCompleteObjectUndo (menus[sideMenu], "Delete menu element");
                 	DeactivateAllElements (menus[sideMenu]);
					selectedMenuElement = null;
					MenuElement tempElement = menus[sideMenu].elements[sideElement];
					menus[sideMenu].elements.RemoveAt (sideElement);
					Undo.SetCurrentGroupName ("Delete menu element '" + tempElement.title + "'");
					Undo.DestroyObjectImmediate (tempElement);
					AssetDatabase.SaveAssets ();
					CleanUpAsset ();
					break;
					
				case "Move up":
					Undo.RegisterCompleteObjectUndo (menus[sideMenu], "Move menu element up");
					menus[sideMenu].elements = SwapElements (menus[sideMenu].elements, sideElement, sideElement-1);
					menus[sideMenu].ResetVisibleElements ();
					AssetDatabase.SaveAssets ();

					break;
					
				case "Move down":
					Undo.RegisterCompleteObjectUndo (menus[sideMenu], "Move menu element down");
					menus[sideMenu].elements = SwapElements (menus[sideMenu].elements, sideElement, sideElement+1);
					menus[sideMenu].ResetVisibleElements ();
					AssetDatabase.SaveAssets ();
					break;

				case "Move to top":
					Undo.RegisterCompleteObjectUndo (menus[sideMenu], "Move menu element to top");
					menus[sideMenu].elements = MoveElementToTop (menus[sideMenu].elements, sideElement);
					menus[sideMenu].ResetVisibleElements ();
					AssetDatabase.SaveAssets ();
					break;
					
				case "Move to bottom":
					Undo.RegisterCompleteObjectUndo (menus[sideMenu], "Move menu element to bottom");
					menus[sideMenu].elements = MoveElementToBottom (menus[sideMenu].elements, sideElement);
					menus[sideMenu].ResetVisibleElements ();
					AssetDatabase.SaveAssets ();
					break;
				}
			}

			EditorUtility.SetDirty (this);

			sideMenu = -1;
			sideElement = -1;
			SaveAllMenus ();
		}


		private List<Menu> MoveMenuToTop (List<Menu> list, int a1)
		{
			Menu tempMenu = list[a1];
			list.Insert (0, tempMenu);
			list.RemoveAt (a1+1);
			return (list);
		}


		private List<Menu> MoveMenuToBottom (List<Menu> list, int a1)
		{
			Menu tempMenu = list[a1];
			list.Add (tempMenu);
			list.RemoveAt (a1);
			return (list);
		}
		

		private List<Menu> SwapMenus (List<Menu> list, int a1, int a2)
		{
			Menu tempMenu = list[a1];
			list[a1] = list[a2];
			list[a2] = tempMenu;
			return (list);
		}


		private List<MenuElement> MoveElementToTop (List<MenuElement> list, int a1)
		{
			MenuElement tempElement = list[a1];
			list.Insert (0, tempElement);
			list.RemoveAt (a1+1);
			return (list);
		}
		
		
		private List<MenuElement> MoveElementToBottom (List<MenuElement> list, int a1)
		{
			MenuElement tempElement = list[a1];
			list.Add (tempElement);
			list.RemoveAt (a1);
			return (list);
		}

		
		private List<MenuElement> SwapElements (List<MenuElement> list, int a1, int a2)
		{
			MenuElement tempElement = list[a1];
			list[a1] = list[a2];
			list[a2] = tempElement;
			return (list);
		}
		

		/**
		 * <sumamry>Gets the currently-selected Menu.</summary>
		 * <returns>The currently-selected Menu</returns>
		 */
		public Menu GetSelectedMenu ()
		{
			return selectedMenu;
		}
		

		/**
		 * <sumamry>Gets the currently-selected MenuElement.</summary>
		 * <returns>The currently-selected MenuElement</returns>
		 */
		public MenuElement GetSelectedElement ()
		{
			return selectedMenuElement;
		}


		/**
		 * <summary>Gets a MenuElement by name.</summary>
		 * <param name = "menuName">The title of the Menu that the MenuElement is a part of</param>
		 * <param name = "menuElementName">The title of the MenuElement to return</param>
		 * <returns>The MenuElement</returns>
		 */
		public static MenuElement GetElementWithName (string menuName, string menuElementName)
		{
			if (AdvGame.GetReferences () && AdvGame.GetReferences ().menuManager)
			{
				foreach (AC.Menu menu in AdvGame.GetReferences ().menuManager.menus)
				{
					if (menu.title == menuName)
					{
						foreach (MenuElement menuElement in menu.elements)
						{
							if (menuElement.title == menuElementName)
							{
								return menuElement;
							}
						}
					}
				}
			}
			
			return null;
		}


		/**
		 * <summary>Converts the Menu Managers's references from a given global variable to a given local variable</summary>
		 * <param name = "oldGlobalID">The ID number of the old global variable</param>
		 * <param name = "newLocalID">The ID number of the new local variable</param>
		 */
		public void CheckConvertGlobalVariableToLocal (int oldGlobalID, int newLocalID)
		{
			if (menus != null)
			{
				foreach (Menu menu in menus)
				{
					if (menu != null && menu.elements != null)
					{
						foreach (MenuElement element in menu.elements)
						{
							if (element != null)
							{
								bool isAffected = element.CheckConvertGlobalVariableToLocal (oldGlobalID, newLocalID);
								if (isAffected)
								{
									ACDebug.LogWarning ("Menu Element '" + element.title + "' in Menu '" + menu.title + "' refers to removed the Global Variable, but cannot refer to Local Variables.");
								}
							}
						}
					}
				}
			}
		}


		/**
		 * <summary>Gets a Menu by ID number</summary>
		 * <param name = "id">The ID number of the Menu to get.  This is the number to the left of the Menu's name in the Menu Manager.</param>
		 * <returns>The Menu with the ID supplied</returns>
		 */
		public Menu GetMenuWithID (int id)
		{
			foreach (Menu menu in menus)
			{
				if (menu != null && menu.id == id)
				{
					return menu;
				}
			}
			return null;
		}


		/**
		 * <summary>Creates a Menu to be used as a preview for subtitles in Timeline</summary>
		 * <param name = "previewMenuName">The name of the Menu to use</param>
		 * <returns>The Menu to preview subtitles with.  If the Menu relies on Unity UI, this copy will be converted to use Adventure Creator</returns>
		 */
		public Menu CreatePreviewMenu (string previewMenuName)
		{
			if (KickStarter.speechManager != null && !string.IsNullOrEmpty (previewMenuName))
			{
				foreach (Menu menu in menus)
				{
					if (menu != null && menu.title == previewMenuName)
					{
						Menu newMenu = ScriptableObject.CreateInstance <Menu>();
						newMenu.Copy (menu, false);
						if (newMenu.menuSource != MenuSource.AdventureCreator)
						{
							newMenu.menuSource = MenuSource.AdventureCreator;
							ACDebug.LogWarning ("Cannot preview subtitles menu ;" + newMenu.title + "' in Unity UI - switching its Source to Adventure Creator!");
						}
						return newMenu;
					}
				}
			}
			return null;
		}

		#endif
		
	}

}