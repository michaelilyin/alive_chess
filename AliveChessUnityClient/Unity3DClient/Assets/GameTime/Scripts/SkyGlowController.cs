using UnityEngine;
using System.Collections;


[AddComponentMenu("Day-Night Cycle/SkyGlowController")]
public class SkyGlowController : MonoBehaviour
{
	public float degreeAboveHorizon = 30;

	void Update()
	{
		var angle = 90 - Vector3.Angle(transform.position, Vector3.up);

		transform.LookAt(transform.parent, Vector3.up);
		transform.Rotate(new Vector3(90, 0, 0));

		float alpha = Mathf.InverseLerp(degreeAboveHorizon, 0, angle);
		renderer.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, alpha / 4));
	}
}
