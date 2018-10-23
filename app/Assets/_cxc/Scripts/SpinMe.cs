using UnityEngine;
using System.Collections;

public class SpinMe:MonoBehaviour
{
	public float speed = 40.0F;

	void Start()
	{
		// ...
	}

	void Update()
	{
		// rotate along y-axis
		transform.Rotate(0.0F, speed * Time.deltaTime, 0.0F);
	}
}