﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
	[Header("Parameters")]
	[SerializeField] Transform player;
	[SerializeField] float speed;

	PlayerController playerController;
	Camera mainCamera;
	Vector3 mousePosition;
	Vector3 offset;
	float speedСontroller;

	// Use this for initialization
	void Start()
	{
		playerController = player.GetComponent<PlayerController>();
		mainCamera = Camera.main;
		offset = transform.position;
		Cursor.lockState = CursorLockMode.Confined;
	}

    // Update is called once per frame
    private void Update()
    {
		if (!playerController.IsActive)
		{
			transform.position = Vector3.Lerp(transform.position, player.position + offset, speed / speedСontroller * Time.deltaTime);
		}
	}
    void FixedUpdate()
	{
		if (Input.GetMouseButton(1) && playerController.IsActive && player)
		{
			mousePosition = Input.mousePosition;
			mousePosition.z = Vector3.Dot(player.position - transform.position, transform.forward);
			mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

			speedСontroller = (mousePosition - transform.position).magnitude;
			transform.position = Vector3.Lerp(transform.position, mousePosition + offset, speed / speedСontroller * Time.deltaTime);
		}
	}
}