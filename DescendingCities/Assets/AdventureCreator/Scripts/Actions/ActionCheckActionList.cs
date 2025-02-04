﻿/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2019
 *	
 *	"ActionCheckActionList.cs"
 * 
 *	This Action will return "TRUE" if the supplied ActionList
 *	is running, and "FALSE" if it is not.
 * 
 */

using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{
	
	[System.Serializable]
	public class ActionCheckActionList : ActionCheck
	{
		
		public enum ListSource { InScene, AssetFile };
		public ListSource listSource = ListSource.InScene;

		public bool checkSelfSkipping = false;
		public ActionList actionList;
		protected ActionList runtimeActionList;
		public ActionListAsset actionListAsset;
		public int constantID = 0;
		public int parameterID = -1;

		protected bool isSkipping = false;


		public ActionCheckActionList ()
		{
			this.isDisplayed = true;
			category = ActionCategory.ActionList;
			title = "Check running";
			description = "Queries whether or not a supplied ActionList is currently running. By looping the If condition is not met field back onto itself, this will effectively “wait” until the supplied ActionList has completed before continuing.";
		}


		override public float Run ()
		{
			isSkipping = false;
			return 0f;
		}


		override public void Skip ()
		{
			isSkipping = true;
		}


		override public void AssignValues (List<ActionParameter> parameters)
		{
			if (listSource == ListSource.InScene)
			{
				runtimeActionList = AssignFile <ActionList> (parameters, parameterID, constantID, actionList);
			}
		}


		override public bool CheckCondition ()
		{
			if (checkSelfSkipping)
			{
				return isSkipping;
			}

			if (isSkipping && IsTargetSkippable ())
			{
				return false;
			}

			if (listSource == ListSource.InScene && runtimeActionList != null)
			{
				return KickStarter.actionListManager.IsListRunning (runtimeActionList);
			}
			else if (listSource == ListSource.AssetFile && actionListAsset != null)
			{
				return KickStarter.actionListAssetManager.IsListRunning (actionListAsset);
			}
			
			return false;
		}


		#if UNITY_EDITOR
		
		override public void ShowGUI (List<ActionParameter> parameters)
		{
			checkSelfSkipping = EditorGUILayout.Toggle ("Check self is skipping?", checkSelfSkipping);
			if (checkSelfSkipping)
			{
				return;
			}

			listSource = (ListSource) EditorGUILayout.EnumPopup ("Source:", listSource);
			if (listSource == ListSource.InScene)
			{
				parameterID = Action.ChooseParameterGUI ("ActionList:", parameters, parameterID, ParameterType.GameObject);
				if (parameterID >= 0)
				{
					constantID = 0;
					actionList = null;
				}
				else
				{
					actionList = (ActionList) EditorGUILayout.ObjectField ("ActionList:", actionList, typeof (ActionList), true);
					
					constantID = FieldToID <ActionList> (actionList, constantID);
					actionList = IDToField <ActionList> (actionList, constantID, true);
				}
			}
			else if (listSource == ListSource.AssetFile)
			{
				actionListAsset = (ActionListAsset) EditorGUILayout.ObjectField ("ActionList asset:", actionListAsset, typeof (ActionListAsset), true);
			}
		}


		override public void AssignConstantIDs (bool saveScriptsToo, bool fromAssetFile)
		{
			if (listSource == ListSource.InScene)
			{
				AssignConstantID <ActionList> (actionList, constantID, parameterID);
			}
		}


		public override string SetLabel ()
		{
			if (listSource == ListSource.InScene && actionList != null)
			{
				return actionList.name;
			}
			else if (listSource == ListSource.AssetFile && actionListAsset != null)
			{
				return actionListAsset.name;
			}
			return string.Empty;
		}

		#endif


		public override void SetLastResult (ActionEnd _actionEnd)
		{
			if (!IsTargetSkippable () && !checkSelfSkipping)
			{
				// When skipping, don't want to rely on last result if target can be skipped as well
				base.SetLastResult (_actionEnd);
				return;
			}

			lastResult = new ActionEnd (-10);
		}


		protected bool IsTargetSkippable ()
		{
			if (listSource == ListSource.InScene && actionList != null)
			{
				return actionList.IsSkippable ();
			}
			else if (listSource == ListSource.AssetFile && actionListAsset != null)
			{
				return actionListAsset.IsSkippable ();
			}
			return false;
		}


		/**
		 * <summary>Creates a new instance of the 'ActionList: Check running' Action, set query if its own ActionList is being skipped</summary>
		 * <returns>The generated Action</returns>
		 */
		public static ActionCheckActionList CreateNew_CheckSelfIsSkipping ()
		{
			ActionCheckActionList newAction = (ActionCheckActionList) CreateInstance <ActionCheckActionList>();
			newAction.checkSelfSkipping = true;
			return newAction;
		}


		/**
		 * <summary>Creates a new instance of the 'ActionList: Check running' Action, set query if a ActionList is running</summary>
		 * <param name = "actionList">The ActionList to check</param>
		 * <returns>The generated Action</returns>
		 */
		public static ActionCheckActionList CreateNew_CheckOther (ActionList actionList)
		{
			ActionCheckActionList newAction = (ActionCheckActionList) CreateInstance <ActionCheckActionList>();
			newAction.listSource = ListSource.InScene;
			newAction.actionList = actionList;
			return newAction;
		}


		/**
		 * <summary>Creates a new instance of the 'ActionList: Check running' Action, set query if a ActionList is running</summary>
		 * <param name = "actionListAsset">The ActionList asset to check</param>
		 * <returns>The generated Action</returns>
		 */
		public static ActionCheckActionList CreateNew_CheckOther (ActionListAsset actionListAsset)
		{
			ActionCheckActionList newAction = (ActionCheckActionList) CreateInstance <ActionCheckActionList>();
			newAction.listSource = ListSource.AssetFile;
			newAction.actionListAsset = actionListAsset;
			return newAction;
		}

	}

}
