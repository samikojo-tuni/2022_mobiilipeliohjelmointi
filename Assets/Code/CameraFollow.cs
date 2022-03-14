using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeliprojektiExamples
{
	public class CameraFollow : MonoBehaviour
	{
		[SerializeField]
		private Transform target;

		[SerializeField]
		private float zOffset;

		[SerializeField]
		private float smoothTime = 0.2f;

		private Vector3 velocity;


		// Update is called once per frame
		void Update()
		{
			// Define a target position above and behind the target transform
			Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, zOffset));

			// Smoothly move the camera towards that target position
			transform.position = Vector3.SmoothDamp(transform.position, targetPosition,
				ref velocity, smoothTime);
		}
	}
}
