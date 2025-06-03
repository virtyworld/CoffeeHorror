using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharController_Motor : MonoBehaviour
{

	public float speed = 10.0f;
	public float sensitivity = 30.0f;
	public float WaterHeight = 15.5f;
	CharacterController character;
	public GameObject cam;
	float moveFB, moveLR;
	float rotX, rotY;
	public bool webGLRightClickRotation = true;
	float gravity = -9.8f;

	private PlayerInput playerInput;
	private InputAction moveAction;
	private InputAction lookAction;

	void Start()
	{
		//LockCursor ();
		character = GetComponent<CharacterController>();
		if (Application.isEditor)
		{
			webGLRightClickRotation = false;
			sensitivity = sensitivity * 1.5f;
		}

		// Setup Input System
		playerInput = GetComponent<PlayerInput>();
		if (playerInput == null)
		{
			playerInput = gameObject.AddComponent<PlayerInput>();
		}

		moveAction = playerInput.actions["Move"];
		lookAction = playerInput.actions["Look"];
	}

	void CheckForWaterHeight()
	{
		if (transform.position.y < WaterHeight)
		{
			gravity = 0f;
		}
		else
		{
			gravity = -9.8f;
		}
	}

	void Update()
	{
		// Get movement input
		Vector2 moveInput = moveAction.ReadValue<Vector2>();
		moveFB = moveInput.x * speed;
		moveLR = moveInput.y * speed;

		// Get look input
		Vector2 lookInput = lookAction.ReadValue<Vector2>();
		rotX = lookInput.x * sensitivity;
		rotY = lookInput.y * sensitivity;

		CheckForWaterHeight();

		Vector3 movement = new Vector3(moveFB, gravity, moveLR);

		if (webGLRightClickRotation)
		{
			if (Mouse.current.leftButton.isPressed)
			{
				CameraRotation(cam, rotX, rotY);
			}
		}
		else if (!webGLRightClickRotation)
		{
			CameraRotation(cam, rotX, rotY);
		}

		movement = transform.rotation * movement;
		character.Move(movement * Time.deltaTime);
	}

	void CameraRotation(GameObject cam, float rotX, float rotY)
	{
		transform.Rotate(0, rotX * Time.deltaTime, 0);
		cam.transform.Rotate(-rotY * Time.deltaTime, 0, 0);
	}
}
