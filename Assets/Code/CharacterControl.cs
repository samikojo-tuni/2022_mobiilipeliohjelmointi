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

		private Vector2 moveInput;

		private void Awake()
		{
			animator = GetComponent<Animator>();
			if (animator == null)
			{
				Debug.LogError("Character is missing an animator component!");
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
			animator.SetFloat("speed", moveInput.magnitude);
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
