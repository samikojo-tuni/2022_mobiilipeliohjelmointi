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
		[SerializeField]
		private float velocity = 1;

		private Animator animator;

		private new SpriteRenderer renderer;

		private Vector2 moveInput;

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
			Vector2 movement = moveInput * Time.deltaTime * velocity;
			// transform property allows us to read and manipulate GameObject's position
			// in the game world.
			transform.Translate(movement);
		}

		private void OnMove(InputAction.CallbackContext callbackContext)
		{
			moveInput = callbackContext.ReadValue<Vector2>();
		}
	}
}
