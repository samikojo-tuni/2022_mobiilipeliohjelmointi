using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PeliprojektiExamples
{
	[RequireComponent(typeof(InputProcessor))]
	public class CharacterControl : MonoBehaviour
	{
		public enum ControlState
		{
			GamePad,
			Touch
		}

		[SerializeField]
		private float velocity = 1;

		private Animator animator;

		private new SpriteRenderer renderer;

		private Vector2 moveInput;

		private Vector2 touchPosition;

		private Vector2 targetPosition;

		private ControlState controlState = ControlState.GamePad;

		private void Awake()
		{
			animator = GetComponent<Animator>();
			if (animator == null)
			{
				Debug.LogError("Character is missing an animator component!");
				Debug.Break();
			}

			renderer = GetComponent<SpriteRenderer>();
			if (renderer == null)
			{
				Debug.LogError("Character is missing an renderer component!");
				Debug.Break();
			}
		}

		private void Update()
		{
			MoveCharacter();
			UpdateAnimator();
		}

		private void UpdateAnimator()
		{
			renderer.flipX = moveInput.x < 0;

			// Same as
			// if (moveInput.x < 0)
			// {
			// 	renderer.flipX = true;
			// }
			// else
			// {
			// 	renderer.flipX = false;
			// }

			animator.SetFloat("speed", moveInput.magnitude);
			animator.SetFloat("horizontal", moveInput.x);
			animator.SetFloat("vertical", moveInput.y);
		}

		private void MoveCharacter()
		{
			switch (controlState)
			{
				case ControlState.GamePad:
					Vector2 movement = moveInput * Time.deltaTime * velocity;
					// transform property allows us to read and manipulate GameObject's position
					// in the game world.
					transform.Translate(movement);
					break;
				case ControlState.Touch:
					// Koska Vector2:sta ei voi vähentää Vector3:a, pitää suorittaa tyyppimuunnos
					Vector3 travel = (Vector3)targetPosition - transform.position;

					// Normalisointi muuntaa vektorin pituuden yhdeksi
					Vector3 frameMovement = travel.normalized * velocity * Time.deltaTime;

					// magnitude palauttaa vektorin pituuden. Tässä vektorin pituus kuvaa
					// jäljellä olevaa matkaa
					float distance = travel.magnitude;

					if (frameMovement.magnitude < distance)
					{
						// Matkaa on vielä jäljellä, kuljetaan kohti kohdepistettä
						transform.Translate(frameMovement);
					}
					else
					{
						// Päämäärä saavutettu
						transform.position = targetPosition;
						moveInput = Vector2.zero;
					}

					break;
			}
		}

		private void OnMove(InputAction.CallbackContext callbackContext)
		{
			controlState = ControlState.GamePad;
			moveInput = callbackContext.ReadValue<Vector2>();
		}

		private void OnTap(InputAction.CallbackContext context)
		{
			Debug.Log("Tap!");
			Debug.Log(context.phase);
		}

		private void OnTouchPosition(InputAction.CallbackContext context)
		{
			controlState = ControlState.Touch;

			this.touchPosition = context.ReadValue<Vector2>();

			// Muunnetaan 2D koordinaatti 3D-koordinaatistoon
			Vector3 screenCoordinate = new Vector3(touchPosition.x, touchPosition.y, 0);

			// Muunnetaan näytön koordinaatti pelimaailman koordinaatistoon
			Vector3 worldCoordinate = Camera.main.ScreenToWorldPoint(screenCoordinate);

			// Muunnetaan maailman koordinaatti 2D-koordinaatistoon. HUOM! implisiittinen
			// tyyppimuunnos Vector3 -> Vector2
			targetPosition = worldCoordinate;

			// Päivitetään myös moveInput-vektoria, koska animaattorin parametrit asetetaan sen
			// perusteella
			moveInput = (targetPosition - (Vector2)transform.position).normalized;
		}
	}
}
