﻿/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2019
 *	
 *	"AnimEngine_SpritesUnity.cs"
 * 
 *	This script uses Unity's built-in 2D
 *	sprite engine for animation.
 * 
 */

using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{
	
	public class AnimEngine_SpritesUnity : AnimEngine
	{

		#if UNITY_EDITOR
		private bool listExpectedAnimations = false;
		#endif

		protected string hideHeadClip = "HideHead";
		protected string headDirection;


		public override void Declare (AC.Char _character)
		{
			character = _character;
			turningStyle = TurningStyle.Linear;
			isSpriteBased = true;
			updateHeadAlways = true;
		}
		

		#if UNITY_EDITOR

		private string ShowExpected (AC.Char character, string animName, string result, int layerIndex)
		{
			if (character == null || animName == "")
			{
				return result;
			}

			string indexString = "   (" + layerIndex + ")";

			result += character.spriteDirectionData.GetExpectedList (character.frameFlipping, animName, indexString);

			return result;
		}

		#endif

		
		public override void CharSettingsGUI ()
		{
			#if UNITY_EDITOR
			
			EditorGUILayout.BeginVertical ("Button");
			EditorGUILayout.LabelField ("Standard 2D animations:", EditorStyles.boldLabel);
			
			character.talkingAnimation = (TalkingAnimation) CustomGUILayout.EnumPopup ("Talk animation style:", character.talkingAnimation, "", "How talking animations are handled");
			character.spriteChild = (Transform) CustomGUILayout.ObjectField <Transform> ("Sprite child:", character.spriteChild, true, "", "The sprite Transform, which should be a child GameObject");

			if (character.spriteChild != null && character.spriteChild.GetComponent <Animator>() == null)
			{
				character.customAnimator = (Animator) CustomGUILayout.ObjectField <Animator> ("Animator (if not on s.c.):", character.customAnimator, true, "", "The Animator component, which will be assigned automatically if not set manually.");
			}

			character.idleAnimSprite = CustomGUILayout.TextField ("Idle name:", character.idleAnimSprite, "", "The name of the 'Idle' animation(s), without suffix");
			character.walkAnimSprite = CustomGUILayout.TextField ("Walk name:", character.walkAnimSprite, "", "The name of the 'Walk' animation(s), without suffix");
			character.runAnimSprite = CustomGUILayout.TextField ("Run name:", character.runAnimSprite, "", "The name of the 'Run' animation(s), without suffix");
			if (character.talkingAnimation == TalkingAnimation.Standard)
			{
				character.talkAnimSprite = CustomGUILayout.TextField ("Talk name:", character.talkAnimSprite, "", "The name of the 'Talk' animation(s), without suffix");
				character.separateTalkingLayer = CustomGUILayout.Toggle ("Head on separate layer?", character.separateTalkingLayer, "", "If True, the head animation will be handled on a non-root layer when talking");
				if (character.separateTalkingLayer)
				{
					character.headLayer = CustomGUILayout.IntField ("Head layer:", character.headLayer, "", "The Animator layer used to play head animations while talking");
					if (character.headLayer < 1)
					{
						EditorGUILayout.HelpBox ("The head layer index must be 1 or greater.", MessageType.Warning);
					}
				}
			}

			character.spriteDirectionData.ShowGUI ();
			character.angleSnapping = AngleSnapping.None;

			if (character.spriteDirectionData.HasDirections ())
			{
				character.frameFlipping = (AC_2DFrameFlipping) CustomGUILayout.EnumPopup ("Frame flipping:", character.frameFlipping, "", "The type of frame-flipping to use");
				if (character.frameFlipping != AC_2DFrameFlipping.None)
				{
					character.flipCustomAnims = CustomGUILayout.Toggle ("Flip custom animations?", character.flipCustomAnims, "", "If True, then custom animations will also be flipped");
				}
			}
			
			character.crossfadeAnims = CustomGUILayout.Toggle ("Crossfade animation?", character.crossfadeAnims, "", "If True, characters will crossfade between standard animations");
			
			Animator charAnimator = character.GetAnimator ();
			if (charAnimator == null || !charAnimator.applyRootMotion)
			{
				character.antiGlideMode = CustomGUILayout.ToggleLeft ("Only move when sprite changes?", character.antiGlideMode, "", "If True, then sprite-based characters will only move when their sprite frame changes");

				if (character.antiGlideMode)
				{
					if (character.GetComponent <Rigidbody2D>())
					{
						EditorGUILayout.HelpBox ("This feature will disable use of the Rigidbody2D component.", MessageType.Warning);
					}
					if (character is Player && AdvGame.GetReferences () != null && AdvGame.GetReferences ().settingsManager != null)
					{
						if (AdvGame.GetReferences ().settingsManager.movementMethod != MovementMethod.PointAndClick && AdvGame.GetReferences ().settingsManager.movementMethod != MovementMethod.None)
						{
							EditorGUILayout.HelpBox ("This feature will not work with collision - it is not recommended for " + AdvGame.GetReferences ().settingsManager.movementMethod.ToString () + " movement.", MessageType.Warning);
						}
					}
				}
			}
			
			if (SceneSettings.CameraPerspective != CameraPerspective.TwoD)
			{
				character.rotateSprite3D = (RotateSprite3D) CustomGUILayout.EnumPopup ("Rotate sprite to:", character.rotateSprite3D, "", "The method by which the character should face the camera");
			}

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField (" ", GUILayout.Width (9));
			listExpectedAnimations = EditorGUILayout.Foldout (listExpectedAnimations, "List expected animations?");
			EditorGUILayout.EndHorizontal ();
			if (listExpectedAnimations)
			{
				string result = "\n";
				result = ShowExpected (character, character.idleAnimSprite, result, 0);
				result = ShowExpected (character, character.walkAnimSprite, result, 0);
				result = ShowExpected (character, character.runAnimSprite, result, 0);
				if (character.talkingAnimation == TalkingAnimation.Standard)
				{
					if (character.separateTalkingLayer)
					{
						result = ShowExpected (character, character.idleAnimSprite, result, character.headLayer);
						result = ShowExpected (character, character.talkAnimSprite, result, character.headLayer);
						result += "\n- " + hideHeadClip + "  (" + character.headLayer + ")";
					}
					else
					{
						result = ShowExpected (character, character.talkAnimSprite, result, 0);
					}
				}
				
				EditorGUILayout.HelpBox ("The following animations are required, based on the settings above (numbers are the Animator layer indices):" + result, MessageType.Info);
			}

			EditorGUILayout.EndVertical ();
			
			if (GUI.changed)
			{
				EditorUtility.SetDirty (character);
			}
			
			#endif
		}
		
		
		public override void ActionCharAnimGUI (ActionCharAnim action, List<ActionParameter> parameters = null)
		{
			#if UNITY_EDITOR
			
			action.method = (ActionCharAnim.AnimMethodChar) EditorGUILayout.EnumPopup ("Method:", action.method);
			
			if (action.method == ActionCharAnim.AnimMethodChar.PlayCustom)
			{
				action.clip2DParameterID = Action.ChooseParameterGUI ("Clip:", parameters, action.clip2DParameterID, ParameterType.String);
				if (action.clip2DParameterID < 0)
				{
					action.clip2D = EditorGUILayout.TextField ("Clip:", action.clip2D);
				}

				action.includeDirection = EditorGUILayout.Toggle ("Add directional suffix?", action.includeDirection);

				if (action.animChar != null && action.animChar.talkingAnimation == TalkingAnimation.Standard && action.animChar.separateTalkingLayer)
				{
					action.hideHead = EditorGUILayout.Toggle ("Hide head?", action.hideHead);
					if (action.hideHead)
					{
						EditorGUILayout.HelpBox ("The head layer will play '" + hideHeadClip + "' for the duration.", MessageType.Info);
					}
				}
								
				action.layerInt = EditorGUILayout.IntField ("Mecanim layer:", action.layerInt);
				action.fadeTime = EditorGUILayout.Slider ("Transition time:", action.fadeTime, 0f, 1f);
				action.willWait = EditorGUILayout.Toggle ("Wait until finish?", action.willWait);
				if (action.willWait)
				{
					action.idleAfter = EditorGUILayout.Toggle ("Return to idle after?", action.idleAfter);
				}
			}
			else if (action.method == ActionCharAnim.AnimMethodChar.StopCustom)
			{
				EditorGUILayout.HelpBox ("This Action does not work for Sprite-based characters.", MessageType.Info);
			}
			else if (action.method == ActionCharAnim.AnimMethodChar.SetStandard)
			{
				action.standard = (AnimStandard) EditorGUILayout.EnumPopup ("Change:", action.standard);

				action.clip2DParameterID = Action.ChooseParameterGUI ("Clip:", parameters, action.clip2DParameterID, ParameterType.String);
				if (action.clip2DParameterID < 0)
				{
					action.clip2D = EditorGUILayout.TextField ("Clip:", action.clip2D);
				}

				if (action.standard == AnimStandard.Walk || action.standard == AnimStandard.Run)
				{
					action.changeSound = EditorGUILayout.Toggle ("Change sound?", action.changeSound);
					if (action.changeSound)
					{
						action.newSoundParameterID = Action.ChooseParameterGUI ("New sound:", parameters, action.newSoundParameterID, ParameterType.UnityObject);
						if (action.newSoundParameterID < 0)
						{
							action.newSound = (AudioClip) EditorGUILayout.ObjectField ("New sound:", action.newSound, typeof (AudioClip), false);
						}
					}
					action.changeSpeed = EditorGUILayout.Toggle ("Change speed?", action.changeSpeed);
					if (action.changeSpeed)
					{
						action.newSpeedParameterID = Action.ChooseParameterGUI ("New speed:", parameters, action.newSpeedParameterID, ParameterType.Float);
						if (action.newSpeedParameterID < 0)
						{
							action.newSpeed = EditorGUILayout.FloatField ("New speed:", action.newSpeed);
						}
					}
				}
			}
			else if (action.method == ActionCharAnim.AnimMethodChar.ResetToIdle)
			{
				action.idleAfterCustom = EditorGUILayout.Toggle ("Wait for animation to finish?", action.idleAfterCustom);
			}
			
			if (GUI.changed)
			{
				EditorUtility.SetDirty (action);
			}
			
			#endif
		}
		
		
		public override float ActionCharAnimRun (ActionCharAnim action)
		{
			string clip2DNew = action.clip2D;
			if (action.includeDirection)
			{
				clip2DNew += character.GetSpriteDirection ();
			}
			
			if (!action.isRunning)
			{
				action.isRunning = true;
				
				if (action.method == ActionCharAnim.AnimMethodChar.PlayCustom && !string.IsNullOrEmpty (action.clip2D))
				{
					if (character.GetAnimator ())
					{
						#if UNITY_EDITOR && (UNITY_5 || UNITY_2017_1_OR_NEWER)
						int hash = Animator.StringToHash (clip2DNew);
						if (!character.GetAnimator ().HasState (action.layerInt, hash))
						{
							action.ReportWarning ("Cannot play clip " + clip2DNew + " on " + character.name);
						}
						#endif

						character.charState = CharState.Custom;
						character.GetAnimator ().CrossFade (clip2DNew, action.fadeTime, action.layerInt);

						if (action.hideHead && character.talkingAnimation == TalkingAnimation.Standard && character.separateTalkingLayer)
						{
							PlayHeadAnim (hideHeadClip, false);
						}
					}
				}
				
				else if (action.method == ActionCharAnim.AnimMethodChar.ResetToIdle)
				{
					if (action.idleAfterCustom)
					{
						action.layerInt = 0;
						return (action.defaultPauseTime);
					}
					else
					{
						character.ResetBaseClips ();
						character.charState = CharState.Idle;
					}
				}
				
				else if (action.method == ActionCharAnim.AnimMethodChar.SetStandard)
				{
					if (!string.IsNullOrEmpty (action.clip2D))
					{
						if (action.standard == AnimStandard.Idle)
						{
							character.idleAnimSprite = action.clip2D;
						}
						else if (action.standard == AnimStandard.Walk)
						{
							character.walkAnimSprite = action.clip2D;
						}
						else if (action.standard == AnimStandard.Talk)
						{
							character.talkAnimSprite = action.clip2D;
						}
						else if (action.standard == AnimStandard.Run)
						{
							character.runAnimSprite = action.clip2D;
						}
					}
					
					if (action.changeSpeed)
					{
						if (action.standard == AnimStandard.Walk)
						{
							character.walkSpeedScale = action.newSpeed;
						}
						else if (action.standard == AnimStandard.Run)
						{
							character.runSpeedScale = action.newSpeed;
						}
					}
					
					if (action.changeSound)
					{
						if (action.standard == AnimStandard.Walk)
						{
							if (action.newSound != null)
							{
								character.walkSound = action.newSound;
							}
							else
							{
								character.walkSound = null;
							}
						}
						else if (action.standard == AnimStandard.Run)
						{
							if (action.newSound != null)
							{
								character.runSound = action.newSound;
							}
							else
							{
								character.runSound = null;
							}
						}
					}
				}
				
				if (action.willWait && !string.IsNullOrEmpty (action.clip2D))
				{
					if (action.method == ActionCharAnim.AnimMethodChar.PlayCustom)
					{
						// In 2019, sometimes more than 1 frame is necessary for the transition to kick in
						#if UNITY_2019_1_OR_NEWER
						return Time.fixedDeltaTime * 2f;
						#else
						return action.defaultPauseTime;
						#endif
					}
				}
			}	
			
			else
			{
				if (character.GetAnimator ())
				{
					// Calc how much longer left to wait
					float totalLength = character.GetAnimator ().GetCurrentAnimatorStateInfo (action.layerInt).length;
					float timeLeft = (1f - character.GetAnimator ().GetCurrentAnimatorStateInfo (action.layerInt).normalizedTime) * totalLength;
					
					// Subtract a small amount of time to prevent overshooting
					timeLeft -= 0.1f;
					
					if (timeLeft > 0f)
					{
						return (timeLeft);
					}
					else
					{
						if (action.method == ActionCharAnim.AnimMethodChar.ResetToIdle)
						{
							character.ResetBaseClips ();
							character.charState = CharState.Idle;
						}
						else if (action.idleAfter)
						{
							character.charState = CharState.Idle;
						}
						
						action.isRunning = false;
						return 0f;
					}
				}
				else
				{
					action.isRunning = false;
					character.charState = CharState.Idle;
					return 0f;
				}
			}
			
			return 0f;
		}
		
		
		public override void ActionCharAnimSkip (ActionCharAnim action)
		{
			if (action.method == ActionCharAnim.AnimMethodChar.SetStandard)
			{
				ActionCharAnimRun (action);
				return;
			}
			else if (action.method == ActionCharAnim.AnimMethodChar.ResetToIdle)
			{
				character.ResetBaseClips ();
				character.charState = CharState.Idle;
				return;
			}
			
			string clip2DNew = action.clip2D;
			if (action.includeDirection)
			{
				clip2DNew += character.GetSpriteDirection ();
			}
			
			if (action.method == ActionCharAnim.AnimMethodChar.PlayCustom)
			{
				if (action.willWait && action.idleAfter)
				{
					character.charState = CharState.Idle;
				}
				else if (character.GetAnimator ())
				{
					character.charState = CharState.Custom;
					character.GetAnimator ().Play (clip2DNew, action.layerInt, 0.8f);
				}
			}
		}
		
		
		public override void ActionSpeechGUI (ActionSpeech action, Char speaker)
		{
			#if UNITY_EDITOR
			
			if (speaker != null && speaker.talkingAnimation == TalkingAnimation.CustomFace)
			{
				action.play2DHeadAnim = EditorGUILayout.BeginToggleGroup ("Custom head animation?", action.play2DHeadAnim);
				action.headClip2D = EditorGUILayout.TextField ("Head animation:", action.headClip2D);
				action.headLayer = EditorGUILayout.IntField ("Mecanim layer:", action.headLayer);
				EditorGUILayout.EndToggleGroup ();
				
				action.play2DMouthAnim = EditorGUILayout.BeginToggleGroup ("Custom mouth animation?", action.play2DMouthAnim);
				action.mouthClip2D = EditorGUILayout.TextField ("Mouth animation:", action.mouthClip2D);
				action.mouthLayer = EditorGUILayout.IntField ("Mecanim layer:", action.mouthLayer);
				EditorGUILayout.EndToggleGroup ();
			}
			
			if (GUI.changed)
			{
				EditorUtility.SetDirty (action);
			}
			
			#endif
		}
		
		
		public override void ActionSpeechRun (ActionSpeech action)
		{
			if (action.Speaker.talkingAnimation == TalkingAnimation.CustomFace && action.Speaker.GetAnimator ())
			{
				if (action.play2DHeadAnim && !string.IsNullOrEmpty (action.headClip2D))
				{
					try
					{
						action.Speaker.GetAnimator ().Play (action.headClip2D, action.headLayer);
					}
					catch {}
				}
				
				if (action.play2DMouthAnim && !string.IsNullOrEmpty (action.mouthClip2D))
				{
					try
					{
						action.Speaker.GetAnimator ().Play (action.mouthClip2D, action.mouthLayer);
					}
					catch {}
				}
			}
		}
		
		
		public override void ActionAnimGUI (ActionAnim action, List<ActionParameter> parameters)
		{
			#if UNITY_EDITOR
			
			action.method = (AnimMethod) EditorGUILayout.EnumPopup ("Method:", action.method);
			
			if (action.method == AnimMethod.PlayCustom)
			{
				action.parameterID = AC.Action.ChooseParameterGUI ("Animator:", parameters, action.parameterID, ParameterType.GameObject);
				if (action.parameterID >= 0)
				{
					action.constantID = 0;
					action.animator = null;
				}
				else
				{
					action.animator = (Animator) EditorGUILayout.ObjectField ("Animator:", action.animator, typeof (Animator), true);
					
					action.constantID = action.FieldToID <Animator> (action.animator, action.constantID);
					action.animator = action.IDToField <Animator> (action.animator, action.constantID, false);
				}

				action.clip2DParameterID = Action.ChooseParameterGUI ("Clip:", parameters, action.clip2DParameterID, ParameterType.String);
				if (action.clip2DParameterID < 0)
				{
					action.clip2D = EditorGUILayout.TextField ("Clip:", action.clip2D);
				}

				action.layerInt = EditorGUILayout.IntField ("Mecanim layer:", action.layerInt);
				action.fadeTime = EditorGUILayout.Slider ("Transition time:", action.fadeTime, 0f, 2f);
				action.willWait = EditorGUILayout.Toggle ("Wait until finish?", action.willWait);
			}
			else if (action.method == AnimMethod.StopCustom)
			{
				EditorGUILayout.HelpBox ("'Stop Custom' is not available for Unity-based 2D animation.", MessageType.Info);
			}
			else if (action.method == AnimMethod.BlendShape)
			{
				EditorGUILayout.HelpBox ("BlendShapes are not available in 2D animation.", MessageType.Info);
			}
			
			if (GUI.changed)
			{
				EditorUtility.SetDirty (action);
			}
			
			#endif
		}
		
		
		public override string ActionAnimLabel (ActionAnim action)
		{
			string label = "";
			
			if (action.animator)
			{
				label = action.animator.name;
				
				if (action.method == AnimMethod.PlayCustom && action.clip2D != "")
				{
					label += " - " + action.clip2D;
				}
			}
			
			return label;
		}
		
		
		public override void ActionAnimAssignValues (ActionAnim action, List<ActionParameter> parameters)
		{
			action.runtimeAnimator = action.AssignFile <Animator> (parameters, action.parameterID, action.constantID, action.animator);
		}
		
		
		public override float ActionAnimRun (ActionAnim action)
		{
			if (!action.isRunning)
			{
				action.isRunning = true;
				
				if (action.runtimeAnimator && !string.IsNullOrEmpty (action.clip2D))
				{
					if (action.method == AnimMethod.PlayCustom)
					{
						#if UNITY_EDITOR && (UNITY_5 || UNITY_2017_1_OR_NEWER)
						int hash = Animator.StringToHash (action.clip2D);
						if (!action.runtimeAnimator.HasState (action.layerInt, hash))
						{
							action.ReportWarning ("Cannot play clip " + action.clip2D + " on " + action.runtimeAnimator.name, action.runtimeAnimator.gameObject);
						}
						#endif

						action.runtimeAnimator.CrossFade (action.clip2D, action.fadeTime, action.layerInt);
						
						if (action.willWait)
						{
							// In 2019, sometimes more than 1 frame is necessary for the transition to kick in
							#if UNITY_2019_1_OR_NEWER
							return Time.fixedDeltaTime * 2f;
							#else
							return action.defaultPauseTime;
							#endif
						}
					}
					else if (action.method == AnimMethod.BlendShape)
					{
						action.ReportWarning ("BlendShapes not available for 2D animation.");
						return 0f;
					}
				}
			}
			else
			{
				if (action.runtimeAnimator && !string.IsNullOrEmpty (action.clip2D))
				{
					if (action.runtimeAnimator.GetCurrentAnimatorStateInfo (action.layerInt).normalizedTime < 1f)
					{
						return (action.defaultPauseTime / 6f);
					}
					else
					{
						action.isRunning = false;
						return 0f;
					}
				}
			}
			
			return 0f;
		}
		
		
		public override void ActionAnimSkip (ActionAnim action)
		{
			if (action.runtimeAnimator && !string.IsNullOrEmpty (action.clip2D))
			{
				if (action.method == AnimMethod.PlayCustom)
				{
					action.runtimeAnimator.Play (action.clip2D, action.layerInt, 0.8f);
				}
			}
		}
		
		
		public override void ActionCharRenderGUI (ActionCharRender action, List<ActionParameter> parameters)
		{
			#if UNITY_EDITOR
			
			EditorGUILayout.Space ();
			action.renderLock_scale = (RenderLock) EditorGUILayout.EnumPopup ("Sprite scale:", action.renderLock_scale);
			if (action.renderLock_scale == RenderLock.Set)
			{
				action.scale = EditorGUILayout.IntField ("New scale (%):", action.scale);
			}
			
			EditorGUILayout.Space ();
			action.renderLock_direction = (RenderLock) EditorGUILayout.EnumPopup ("Sprite direction:", action.renderLock_direction);
			if (action.renderLock_direction == RenderLock.Set)
			{
				action.direction = (CharDirection) EditorGUILayout.EnumPopup ("New direction:", action.direction);
			}
			
			EditorGUILayout.Space ();
			action.renderLock_sortingMap = (RenderLock) EditorGUILayout.EnumPopup ("Sorting Map:", action.renderLock_sortingMap);
			if (action.renderLock_sortingMap == RenderLock.Set)
			{
				action.sortingMapParameterID = Action.ChooseParameterGUI ("New Sorting Map:", parameters, action.sortingMapParameterID, ParameterType.GameObject);
				if (action.sortingMapParameterID >= 0)
				{
					action.sortingMapConstantID = 0;
					action.sortingMap = null;
				}
				else
				{
					action.sortingMap = (SortingMap) EditorGUILayout.ObjectField ("New Sorting Map:", action.sortingMap, typeof (SortingMap), true);
					
					action.sortingMapConstantID = action.FieldToID <SortingMap> (action.sortingMap, action.sortingMapConstantID);
					action.sortingMap = action.IDToField <SortingMap> (action.sortingMap, action.sortingMapConstantID, false);
				}
			}
			
			if (GUI.changed)
			{
				EditorUtility.SetDirty (action);
			}
			
			#endif
		}
		
		
		public override float ActionCharRenderRun (ActionCharRender action)
		{
			if (action.renderLock_scale == RenderLock.Set)
			{
				character.lockScale = true;
				character.spriteScale = (float) action.scale / 100f;
			}
			else if (action.renderLock_scale == RenderLock.Release)
			{
				character.lockScale = false;
			}
			
			if (action.renderLock_direction == RenderLock.Set)
			{
				character.SetSpriteDirection (action.direction);
			}
			else if (action.renderLock_direction == RenderLock.Release)
			{
				character.lockDirection = false;
			}
			
			if (action.renderLock_sortingMap != RenderLock.NoChange && character.GetComponentInChildren <FollowSortingMap>())
			{
				FollowSortingMap[] followSortingMaps = character.GetComponentsInChildren <FollowSortingMap>();
				SortingMap sortingMap = (action.renderLock_sortingMap == RenderLock.Set) ? action.RuntimeSortingMap : KickStarter.sceneSettings.sortingMap;
				
				foreach (FollowSortingMap followSortingMap in followSortingMaps)
				{
					followSortingMap.SetSortingMap (sortingMap);
				}
			}
			
			return 0f;
		}
		
		
		public override void PlayIdle ()
		{
			PlayStandardAnim (character.idleAnimSprite, character.spriteDirectionData.HasDirections ());
			PlaySeparateHead ();
		}
		
		
		public override void PlayWalk ()
		{
			PlayStandardAnim (character.walkAnimSprite, character.spriteDirectionData.HasDirections ());
			PlaySeparateHead ();
		}
		
		
		public override void PlayRun ()
		{
			if (!string.IsNullOrEmpty (character.runAnimSprite))
			{
				PlayStandardAnim (character.runAnimSprite, character.spriteDirectionData.HasDirections ());
			}
			else
			{
				PlayWalk ();
			}
			PlaySeparateHead ();
		}
		
		
		public override void PlayTalk ()
		{
			if (string.IsNullOrEmpty (character.talkAnimSprite))
			{
				PlayIdle ();
			}
			else if (character.talkingAnimation == TalkingAnimation.Standard && character.separateTalkingLayer)
			{
				PlayIdle ();
			}
			else if (character.LipSyncGameObject () && character.GetAnimator ())
			{
				PlayLipSync (false);
			}
			else
			{
				PlayStandardAnim (character.talkAnimSprite, character.spriteDirectionData.HasDirections ());
			}
		}


		protected void PlaySeparateHead ()
		{
			if (character.talkingAnimation == TalkingAnimation.Standard && character.separateTalkingLayer)
			{
				if (character.isTalking)
				{
					if (character.LipSyncGameObject () && character.GetAnimator ())
					{
						PlayLipSync (true);
					}
					else
					{
						PlayHeadAnim (character.talkAnimSprite, character.spriteDirectionData.HasDirections ());
					}
				}
				else
				{
					PlayHeadAnim (character.idleAnimSprite, character.spriteDirectionData.HasDirections ());
				}
			}
		}


		protected void PlayLipSync (bool onlyHead)
		{
			string clip = character.talkAnimSprite;
			int layer = (onlyHead) ? character.headLayer : 0;
			if (character.spriteDirectionData.HasDirections ())
			{
				if (layer > 0)
				{
					clip += headDirection;
				}
				else
				{
					clip += character.GetSpriteDirection ();
				}
			}
			character.GetAnimator ().speed = 0f;
			
			#if UNITY_EDITOR && (UNITY_5 || UNITY_2017_1_OR_NEWER)
			
			int hash = Animator.StringToHash (clip);
			if (character.GetAnimator ().HasState (layer, hash))
			{
				character.GetAnimator ().Play (hash, layer, character.GetLipSyncNormalised ());
			}
			else
			{
				ACDebug.LogWarning ("Cannot play clip " + clip + " (layer " + layer + ") on " + character.name, character);
			}
			
			#else
			
			try
			{
				character.GetAnimator ().Play (clip, layer, character.GetLipSyncNormalised ());
			}
			catch
			{}
			
			#endif
			
			character.GetAnimator ().speed = 1f;
		}


		public override void TurnHead (Vector2 angles)
		{
			if (character.lockDirection)
			{
				headDirection = character.GetSpriteDirection ();
			}
			else if (character.talkingAnimation == TalkingAnimation.Standard && character.separateTalkingLayer)
			{
				float spinAngleOffset = angles.x * Mathf.Rad2Deg;
				float headAngle = character.GetSpriteAngle () + spinAngleOffset;

				headDirection = "_" + character.spriteDirectionData.GetDirectionalSuffix (headAngle);
			}
		}


		protected void PlayStandardAnim (string clip, bool includeDirection)
		{
			if (character && character.GetAnimator () && !string.IsNullOrEmpty (clip))
			{
				if (includeDirection)
				{
					clip += character.GetSpriteDirection ();
				}
				
				PlayCharAnim (clip, 0);
			}
		}


		protected void PlayHeadAnim (string clip, bool includeDirection)
		{
			if (character && character.GetAnimator () && !string.IsNullOrEmpty (clip))
			{
				if (includeDirection)
				{
					clip += headDirection;
				}

				PlayCharAnim (clip, character.headLayer);
			}
		}


		protected void PlayCharAnim (string clip, int layer)
		{
			#if UNITY_EDITOR && (UNITY_5 || UNITY_2017_1_OR_NEWER)

			int hash = Animator.StringToHash (clip);
			if (character.GetAnimator ().HasState (layer, hash))
			{
				if (character.crossfadeAnims)
				{
					// Already playing?
					if (character.GetAnimator ().GetNextAnimatorStateInfo (layer).IsName (clip))
					{
						return;
					}
					if (character.GetAnimator ().GetCurrentAnimatorStateInfo (layer).IsName (clip))
					{
						return;
					}

					character.GetAnimator ().CrossFade (hash, character.animCrossfadeSpeed, layer);
				}
				else
				{
					character.GetAnimator ().Play (hash, layer);
				}
			}
			else
			{
				ACDebug.LogWarning ("Cannot play animation clip " + clip + " (layer " + layer + ") on " + character.name, character);
			}
			
			#else
			
			if (character.crossfadeAnims)
			{
				try
				{
					// Already playing?
					if (character.GetAnimator ().GetNextAnimatorStateInfo (layer).IsName (clip))
					{
						return;
					}
					if (character.GetAnimator ().GetCurrentAnimatorStateInfo (layer).IsName (clip))
					{
						return;
					}

					character.GetAnimator ().CrossFade (clip, character.animCrossfadeSpeed, layer);
				}
				catch
				{}
			}
			else
			{
				try
				{
					character.GetAnimator ().Play (clip, layer);
				}
				catch
				{}
			}
			
			#endif
		}
		
	}

}