using UnityEngine;
using System.Collections;

[AddComponentMenu("Day-Night Cycle/SizeController")]
public class SizeController : MonoBehaviour
{

	public float minSize = 10f;
	public float maxSize = 20f;

	void Update()
	{
		var v1 = transform.position;
		var angle = Vector3.Angle(v1, Vector3.up);

		float scale;
		if (angle > 90)
		{
			scale = maxSize;
		}
		else if (angle < 45)
		{
			scale = minSize;
		}
		else
		{
			scale = minSize + (angle - 45) / 45 * (maxSize - minSize);
		}

		transform.localScale = new Vector3(scale, scale, scale);
	}
}
