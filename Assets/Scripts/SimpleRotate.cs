using UnityEngine;
using System.Collections;

public class SimpleRotate : MonoBehaviour {

	void Update()
	{
		float rotate_speed = 5f;
		transform.position = new Vector3(Mathf.Cos(Mathf.Repeat(Time.time*rotate_speed, Mathf.PI*2f)),
										 0.5f,
										 Mathf.Sin(Mathf.Repeat(Time.time*rotate_speed, Mathf.PI*2f))) * 8f;
		transform.LookAt(Vector3.zero);
	}
}
