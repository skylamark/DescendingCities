﻿/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2019
 *	
 *	"ActionCameraCrossfade.cs"
 * 
 *	This action crossfades the MainCamera from one
 *	GameCamera to another.
 * 
 */

using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionCameraCrossfade : Action
	{

		public int parameterID = -1;
		public int constantID = 0;
		public _Camera linkedCamera;
		protected _Camera runtimeLinkedCamera;

		public float transitionTime;
		public int transitionTimeParameterID = -1;

		public bool returnToLast;

		
		public ActionCameraCrossfade ()
		{
			this.isDisplayed = true;
			category = ActionCategory.Camera;
			title = "Crossfade";
			description = "Crossfades the camera from its current GameCamera to a new one, over a specified time.";
		}


		override public void AssignValues (List<ActionParameter> parameters)
		{
			runtimeLinkedCamera = AssignFile <_Camera> (parameters, parameterID, constantID, linkedCamera);
			transitionTime = AssignFloat (parameters, transitionTimeParameterID, transitionTime);

			if (returnToLast)
			{
				runtimeLinkedCamera = KickStarter.mainCamera.GetLastGameplayCamera ();
			}
		}

		
		override public float Run ()
		{
			if (!isRunning)
			{
				isRunning = true;
				MainCamera mainCam = KickStarter.mainCamera;

				if (runtimeLinkedCamera != null && mainCam.attachedCamera != runtimeLinkedCamera)
				{
					if (runtimeLinkedCamera is GameCameraThirdPerson)
					{
						GameCameraThirdPerson tpCam = (GameCameraThirdPerson) runtimeLinkedCamera;
						tpCam.ResetRotation ();
					}
					else if (runtimeLinkedCamera is GameCameraAnimated)
					{
						GameCameraAnimated animCam = (GameCameraAnimated) runtimeLinkedCamera;
						animCam.PlayClip ();
					}
					
					runtimeLinkedCamera.MoveCameraInstant ();
					mainCam.Crossfade (transitionTime, runtimeLinkedCamera);
						
					if (transitionTime > 0f && willWait)
					{
						return (transitionTime);
					}
				}
			}
			else
			{
				isRunning = false;
			}
			
			return 0f;
		}
		
		
		override public void Skip ()
		{
			MainCamera mainCam = KickStarter.mainCamera;

			if (runtimeLinkedCamera != null && mainCam.attachedCamera != runtimeLinkedCamera)
			{
				if (runtimeLinkedCamera is GameCameraThirdPerson)
				{
					GameCameraThirdPerson tpCam = (GameCameraThirdPerson) runtimeLinkedCamera;
					tpCam.ResetRotation ();
				}
				else if (runtimeLinkedCamera is GameCameraAnimated)
				{
					GameCameraAnimated animCam = (GameCameraAnimated) runtimeLinkedCamera;
					animCam.PlayClip ();
				}
				
				runtimeLinkedCamera.MoveCameraInstant ();
				mainCam.SetGameCamera (runtimeLinkedCamera);
			}
		}


		#if UNITY_EDITOR
		
		override public void ShowGUI (List<ActionParameter> parameters)
		{
			returnToLast = EditorGUILayout.Toggle ("Return to last gameplay?", returnToLast);
			if (!returnToLast)
			{
				parameterID = Action.ChooseParameterGUI ("New camera:", parameters, parameterID, ParameterType.GameObject);
				if (parameterID >= 0)
				{
					constantID = 0;
					linkedCamera = null;
				}
				else
				{
					linkedCamera = (_Camera) EditorGUILayout.ObjectField ("New camera:", linkedCamera, typeof(_Camera), true);
					
					constantID = FieldToID <_Camera> (linkedCamera, constantID);
					linkedCamera = IDToField <_Camera> (linkedCamera, constantID, true);
				}
			}

			transitionTimeParameterID = Action.ChooseParameterGUI ("Transition time (s):", parameters, transitionTimeParameterID, ParameterType.Float);
			if (transitionTimeParameterID < 0)
			{
				transitionTime = EditorGUILayout.FloatField ("Transition time (s):", transitionTime);
			}
			willWait = EditorGUILayout.Toggle ("Wait until finish?", willWait);

			AfterRunningOption ();
		}


		override public void AssignConstantIDs (bool saveScriptsToo, bool fromAssetFile)
		{
			if (saveScriptsToo)
			{
				AddSaveScript <ConstantID> (linkedCamera);
			}
			AssignConstantID <_Camera> (linkedCamera, constantID, parameterID);
		}

		
		override public string SetLabel ()
		{
			if (linkedCamera != null)
			{
				return linkedCamera.name;
			}
			return string.Empty;
		}
		
		#endif


		/**
		 * <summary>Creates a new instance of the 'Camera: Crossfade' Action with key variables already set.</summary>
		 * <param name = "newCamera">The camera to crossfade to</param>
		 * <param name = "transitionTime">The time, in seconds, to take while transitioning to the new camera</param>
		 * <param name = "waitUntilFinish">If True, then the Action will wait until the transition is complete<param>
		 * <returns>The generated Action</returns>
		 */
		public static ActionCameraCrossfade CreateNew (_Camera newCamera, float transitionTime = 1f, bool waitUntilFinish = false)
		{
			ActionCameraCrossfade newAction = (ActionCameraCrossfade) CreateInstance <ActionCameraCrossfade>();
			newAction.linkedCamera = newCamera;
			newAction.transitionTime = transitionTime;
			newAction.willWait = waitUntilFinish;
			return newAction;
		}
		
	}

}