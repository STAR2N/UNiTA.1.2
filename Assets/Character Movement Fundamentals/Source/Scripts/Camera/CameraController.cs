using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject playerObject;
	public Vector3 offset;
	private float speed = 10.0f;

	private void Start()
	{

	}
	void Uodate()
	{
		moveObjectFunc();
	}

	private float speed_move = 3.0f;
	private float speed_rota = 2.0f;
	void moveObjectFunc()
	{
		float keyH = Input.GetAxis("Horizontal");
		float keyV = Input.GetAxis("Vertical");
		keyH = keyH * speed_move * Time.deltaTime;
		keyV = keyV * speed_move * Time.deltaTime;
		transform.Translate(Vector3.right * keyH);
		transform.Translate(Vector3.forward * keyV);
	}
}

