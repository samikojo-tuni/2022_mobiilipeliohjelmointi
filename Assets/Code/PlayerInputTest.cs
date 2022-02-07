using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PeliprojektiExamples
{
	public class PlayerInputTest : MonoBehaviour
	{
		// The value of a constant can't be changed.
		// Describes the name of the horizontal input axis.
		public const string HorizontalAxisName = "Horizontal";

		// Describes the name of the vertical input axis.
		public const string VerticalAxisName = "Vertical";

		[SerializeField]
		private float velocity = 1;

		// A member variable which is used to store the information about move input
		private Vector2 moveInput;

		// isJumping == true when the user keeps pressing the jump button
		private bool isJumping;

		// Update is called once per frame
		private void Update()
		{
			// Move character here if we don't require physics calculation.
			// Let's form a movement vector.

			// By multiplying the input vector with Time.deltaTime we make the calculation
			// framerate independent. Without this the unit of speed would be meters / frame
			// and not meters / second.
			Vector2 movement = moveInput * Time.deltaTime * velocity;

			// transform property allows us to read and manipulate GameObject's position
			// in the game world.
			transform.Translate(movement);

			// Same without using Translate method.
			// Vector3 position = transform.position;
			// position += (Vector3)movement;
			// transform.position = position;


			if (isJumping)
			{
				Debug.Log("Jumping");
			}
		}

		// Reads the input using the old input manager.
		// To use this, you must enable the input manager backend from the player settings.
		private static void ReadInputOld()
		{
			// Read the input from the horizontal axis
			float horizotalInput = Input.GetAxisRaw(HorizontalAxisName);

			// Read the input from the vertical axis
			float verticalInput = Input.GetAxisRaw(VerticalAxisName);

			Debug.Log("Current input: " + horizotalInput + ", " + verticalInput);
		}

		private void OnMove(InputAction.CallbackContext callbackContext)
		{
			// Read the move input from callbackContext. ReadValue requires the type parameter
			// inside <> parenthesis as shown below.
			// Store the result to the member variable moveInput.
			moveInput = callbackContext.ReadValue<Vector2>();
			Debug.Log("Current input: " + moveInput);
		}

		private void OnJump(InputAction.CallbackContext callbackContext)
		{
			InputActionPhase inputPhase = callbackContext.phase;
			Debug.Log("Jump input phase: " + inputPhase);

			isJumping = inputPhase == InputActionPhase.Performed;
		}
	}
}
