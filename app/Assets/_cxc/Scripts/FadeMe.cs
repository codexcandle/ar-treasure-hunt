using UnityEngine;
using System.Collections;

public class FadeMe:MonoBehaviour
{
	public float fadeTimeSecs = 1.0F;
	public float saturationFactor = 1.0F;

	public void InitializeWithFadeIn()
	{
		float alphaStart = 0.0F;

		SetAlpha(alphaStart);

		StopAllCoroutines();

		StartCoroutine(FadeTo(alphaStart, saturationFactor, fadeTimeSecs, true));
	}

	internal void Update()
	{
		if(Input.GetKeyUp(KeyCode.F))
		{
			FadeIn();
		}

		if(Input.GetKeyUp(KeyCode.O))
		{
			FadeOut();
		}
	}

	private float GetAlpha()
	{
		return GetComponent<Renderer>().material.GetFloat("_Alpha");
	}

	private void SetAlpha(float val)
	{
		GetComponent<Renderer>().material.SetFloat("_Alpha", val);
	}

	private void FadeIn()
	{
		StopAllCoroutines();

		StartCoroutine(FadeTo(GetAlpha(), saturationFactor, fadeTimeSecs, false));
	}

	private void FadeOut()
	{
		StopAllCoroutines();

		StartCoroutine(FadeTo(GetAlpha(), 0.0F, fadeTimeSecs, false));
	}

	IEnumerator FadeTo(float startVal, float endVal, float aTime, bool doPause)
	{
		if(doPause)
		{
			yield return new WaitForSeconds(0.5F);
		}

		for(float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			float val = Mathf.Lerp(startVal, endVal, t);

			SetAlpha(val);

			yield return null;
		}
	}
}