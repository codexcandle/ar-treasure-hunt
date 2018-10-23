using UnityEngine;
using System.Collections;

public class TestWWW : MonoBehaviour
{
	void Start()
	{
		StartCoroutine(authorize());
	}
		
	private  IEnumerator authorize()
	{
		// replace php path  with  the address to the php file
		// string post_url =phpPath+ "?username=" + WWW.EscapeURL(profileName) + "&password=" + WWW.EscapeURL(password);
		string post_url = "http://fancy-site/test.php?f=treasureFunction";

		// Post the URL to the site and create a download object to get the result.
		WWW hs_post = new WWW(post_url);
		yield return hs_post; // Wait until the download is done

		if (hs_post.error != null)
		{
			Debug.Log("error" + hs_post.error);
		}
		else
		{
			Debug.Log ("result: ----- " + hs_post.text);

			if(hs_post.text=="true")
			{
				Debug.Log ("logged in!!!" + hs_post.text);
			}
			else
			{
				Debug.Log ("NOT logged in!!!!");
			}
		}
	}
}