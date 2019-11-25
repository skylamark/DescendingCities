﻿/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2019
 *	
 *	"ActionCharDirection.cs"
 * 
 *	This action is used to make characters turn to face fixed directions relative to the camera.
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
	public class ActionCharFaceDirection : Action
	{
		
		public int charToMoveParameterID = -1;

		public int charToMoveID = 0;

		public bool isInstant;
		public CharDirection direction;

		public Char charToMove;
		protected Char runtimeCharToMove;

		public bool isPlayer;
		[SerializeField] protected RelativeTo relativeTo = RelativeTo.Camera;
		public enum RelativeTo { Camera, Character };

		
		public ActionCharFaceDirection ()
		{
			this.isDisplayed = true;
			category = ActionCategory.Character;
			title = "Face direction";
			description = "Makes a Character turn, either instantly or over time, to face a direction relative to the camera – i.e. up, down, left or right.";
		}
		
		
		public override void AssignValues (List<ActionParameter> parameters)
		{
			runtimeCharToMove = AssignFile <Char> (parameters, charToMoveParameterID, charToMoveID, charToMove);

			if (isPlayer)
			{
				runtimeCharToMove = KickStarter.player;
			}
		}


		override public float Run ()
		{
			if (!isRunning)
			{
				isRunning = true;
				
				if (runtimeCharToMove != null)
				{
					if (!isInstant && runtimeCharToMove.IsMovingAlongPath ())
					{
						runtimeCharToMove.EndPath ();
					}

					runtimeCharToMove.SetLookDirection (GetLookVector (), isInstant);

					if (!isInstant)
					{
						if (willWait)
						{
							return (defaultPauseTime);
						}
					}
				}
				
				return 0f;
			}
			else
			{
				if (runtimeCharToMove.IsTurning ())
				{
					return defaultPauseTime;
				}
				else
				{
					isRunning = false;
					return 0f;
				}
			}
		}
		
		
		override public void Skip ()
		{
			if (runtimeCharToMove != null)
			{
				runtimeCharToMove.SetLookDirection (GetLookVector (), true);
			}
		}


		protected Vector3 GetLookVector ()
		{
			Vector3 camForward = KickStarter.CameraMain.transform.forward;
			camForward = new Vector3 (camForward.x, 0f, camForward.z).normalized;

			if (SceneSettings.IsTopDown ())
			{
				camForward = -KickStarter.CameraMain.transform.forward;
			}
			else if (SceneSettings.CameraPerspective == CameraPerspective.TwoD)
			{
				camForward = KickStarter.CameraMain.transform.up;
			}

			Vector3 camRight = new Vector3 (KickStarter.CameraMain.transform.right.x, 0f, KickStarter.CameraMain.transform.right.z);

			// Angle slightly so that left->right rotations face camera
			if (KickStarter.settingsManager.IsInFirstPerson ())
			{
				// No angle tweaking in first-person
			}
			else if (SceneSettings.CameraPerspective == CameraPerspective.TwoD)
			{
				camRight -= new Vector3 (0f, 0f, 0.01f);
			}
			else
			{
				camRight -= camForward * 0.01f;
			}

			if (relativeTo == RelativeTo.Character)
			{
				camForward = runtimeCharToMove.TransformForward;
				camRight = runtimeCharToMove.TransformRight;
			}

			Vector3 lookVector = Vector3.zero;
			switch (direction)
			{
				case CharDirection.Down:
					lookVector = -camForward;
					break;

				case CharDirection.Left:
					lookVector = -camRight;
					break;

				case CharDirection.Right:
					lookVector = camRight;
					break;

				case CharDirection.Up:
					lookVector = camForward;
					break;

				case CharDirection.DownLeft:
					lookVector = (-camForward - camRight).normalized;
					break;

				case CharDirection.DownRight:
					lookVector = (-camForward + camRight).normalized;
					break;

				case CharDirection.UpLeft:
					lookVector = (camForward - camRight).normalized;
					break;

				case CharDirection.UpRight:
					lookVector = (camForward + camRight).normalized;
					break;
			}

			if (SceneSettings.IsTopDown ())
			{
				return lookVector;
			}
			if (SceneSettings.CameraPerspective == CameraPerspective.TwoD && relativeTo == RelativeTo.Camera)
			{
				return new Vector3 (lookVector.x, 0f, lookVector.y).normalized;
			}
			return lookVector;
		}
		
		
		#if UNITY_EDITOR
		
		override public void ShowGUI (List<ActionParameter> parameters)
		{
			isPlayer = EditorGUILayout.Toggle ("Affect Player?", isPlayer);
			if (!isPlayer)
			{
				charToMoveParameterID = Action.ChooseParameterGUI ("Character to turn:", parameters, charToMoveParameterID, ParameterType.GameObject);
				if (charToMoveParameterID >= 0)
				{
					charToMoveID = 0;
					charToMove = null;
				}
				else
				{
					charToMove = (Char) EditorGUILayout.ObjectField ("Character to turn:", charToMove, typeof(Char), true);
					
					charToMoveID = FieldToID <Char> (charToMove, charToMoveID);
					charToMove = IDToField <Char> (charToMove, charToMoveID, false);
				}
			}

			direction = (CharDirection) EditorGUILayout.EnumPopup ("Direction to face:", direction);
			relativeTo = (RelativeTo) EditorGUILayout.EnumPopup ("Direction is relative to:", relativeTo);
			isInstant = EditorGUILayout.Toggle ("Is instant?", isInstant);
			if (!isInstant)
			{
				willWait = EditorGUILayout.Toggle ("Wait until finish?", willWait);
			}
			
			AfterRunningOption ();
		}


		override public void AssignConstantIDs (bool saveScriptsToo, bool fromAssetFile)
		{
			if (!isPlayer)
			{
				if (saveScriptsToo && charToMove != null && charToMove.GetComponent <NPC>())
				{
					AddSaveScript <RememberNPC> (charToMove);
				}

				AssignConstantID <Char> (charToMove, charToMoveID, charToMoveParameterID);
			}
		}

		
		override public string SetLabel ()
		{
			if (charToMove != null)
			{
				return charToMove.name + " - " + direction;
			}
			return string.Empty;
		}
		
		#endif


		/**
		 * <summary>Creates a new instance of the 'Character: Face direction' Action</summary>
		 * <param name = "characterToTurn">The character to affect</param>
		 * <param name = "directionToFace">The direction to face</param>
		 * <param name = "relativeTo">What the supplied direction is relative to</param>
		 * <param name = "isInstant">If True, the character will stop turning their head instantly</param>
		 * <param name = "waitUntilFinish">If True, then the Action will wait until the transition is complete</param>
		 * <returns>The generated Action</returns>
		 */
		public static ActionCharFaceDirection CreateNew (AC.Char characterToTurn, CharDirection directionToFace, RelativeTo relativeTo = RelativeTo.Camera, bool isInstant = false, bool waitUntilFinish = false)
		{
			ActionCharFaceDirection newAction = (ActionCharFaceDirection) CreateInstance <ActionCharFaceDirection>();
			newAction.charToMove = characterToTurn;
			newAction.direction = directionToFace;
			newAction.relativeTo = relativeTo;
			newAction.isInstant = isInstant;
			newAction.willWait = waitUntilFinish;
			return newAction;
		}
		
	}

}