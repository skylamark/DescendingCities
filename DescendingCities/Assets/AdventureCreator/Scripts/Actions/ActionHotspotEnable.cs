﻿/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2019
 *	
 *	"ActionHotspotEnable.cs"
 * 
 *	This Action can enable and disable a Hotspot.
 * 
 */

using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionHotspotEnable : Action
	{

		public int parameterID = -1;
		public int constantID = 0;
		public Hotspot hotspot;
		protected Hotspot runtimeHotspot;
		public bool affectChildren = false;

		public ChangeType changeType = ChangeType.Enable;

		
		public ActionHotspotEnable ()
		{
			this.isDisplayed = true;
			category = ActionCategory.Hotspot;
			title = "Enable or disable";
			description = "Turns a Hotspot on or off. To record the state of a Hotspot in save games, be sure to add the RememberHotspot script to the Hotspot in question.";
		}


		override public void AssignValues (List<ActionParameter> parameters)
		{
			runtimeHotspot = AssignFile <Hotspot> (parameters, parameterID, constantID, hotspot);
		}

		
		override public float Run ()
		{
			if (runtimeHotspot == null)
			{
				return 0f;
			}

			DoChange (runtimeHotspot);

			if (affectChildren)
			{
				Hotspot[] hotspots = runtimeHotspot.GetComponentsInChildren <Hotspot>();
				foreach (Hotspot _hotspot in hotspots)
				{
					if (_hotspot != runtimeHotspot)
					{
						DoChange (_hotspot);
					}
				}
			}

			return 0f;
		}


		protected void DoChange (Hotspot _hotspot)
		{
			if (changeType == ChangeType.Enable)
			{
				_hotspot.TurnOn ();
			}
			else
			{
				_hotspot.TurnOff ();
			}
		}

		
		#if UNITY_EDITOR
		
		override public void ShowGUI (List<ActionParameter> parameters)
		{
			parameterID = Action.ChooseParameterGUI ("Hotspot to affect:", parameters, parameterID, ParameterType.GameObject);
			if (parameterID >= 0)
			{
				constantID = 0;
				hotspot = null;
			}
			else
			{
				hotspot = (Hotspot) EditorGUILayout.ObjectField ("Hotspot to affect:", hotspot, typeof (Hotspot), true);
				
				constantID = FieldToID <Hotspot> (hotspot, constantID);
				hotspot = IDToField <Hotspot> (hotspot, constantID, false);
			}

			changeType = (ChangeType) EditorGUILayout.EnumPopup ("Change to make:", changeType);
			affectChildren = EditorGUILayout.Toggle ("Also affect children?", affectChildren);

			AfterRunningOption ();
		}


		override public void AssignConstantIDs (bool saveScriptsToo, bool fromAssetFile)
		{
			if (saveScriptsToo)
			{
				AddSaveScript <RememberHotspot> (hotspot);
			}
			AssignConstantID <Hotspot> (hotspot, constantID, parameterID);
		}
		
		
		public override string SetLabel ()
		{
			if (hotspot != null)
			{
				return hotspot.name + " - " + changeType;
			}
			return string.Empty;
		}
		
		#endif


		/**
		 * <summary>Creates a new instance of the 'Hotspot: Enable or disable' Action</summary>
		 * <param name = "hotspotToAffect">The Hotspot to affect</param>
		 * <param name = "changeToMake">The type of change to make</param>
		 * <returns>The generated Action</returns>
		 */
		public static ActionHotspotEnable CreateNew (Hotspot hotspotToAffect, ChangeType changeToMake)
		{
			ActionHotspotEnable newAction = (ActionHotspotEnable) CreateInstance <ActionHotspotEnable>();
			newAction.hotspot = hotspotToAffect;
			newAction.changeType = changeToMake;
			return newAction;
		}
		
	}

}