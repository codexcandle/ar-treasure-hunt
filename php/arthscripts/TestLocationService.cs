using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TestLocationService : MonoBehaviour
{
	public Text debugText;

	public void SetDebugText(string value)
	{
		debugText.text = value;
	}

	IEnumerator Start()
	{
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser)
		{
			SetDebugText("The location service is disabled.");

			yield break;
		}

		// Start service before querying location
		Input.location.Start();

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(1);

			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			SetDebugText("Timed out");

			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			SetDebugText("Unable to determine device location");
			
			yield break;
		}
		else
		{
			// Access granted and location value could be retrieved
			LocationInfo locationInfo = Input.location.lastData;

			string latitude = "1. Latitude: " + locationInfo.latitude.ToString() + "\n";
			string longitude = "2. Longitude: " + (locationInfo.longitude * -1).ToString() + "\n";	// << - TODO - remove hack -1.
			string altitude = "3. Altitude: " + locationInfo.altitude.ToString() + "\n";
			string horizontalAccuracy = "4. Hor-Acc: " + locationInfo.horizontalAccuracy.ToString() + "\n";
			string timeStamp = "5. TimeStamp: " + UnixTimeStampToDateTime(locationInfo.timestamp).ToString() + "\n";

			string str = latitude + longitude + altitude + horizontalAccuracy + timeStamp;
			SetDebugText(str);

			// Stop service if there is no need to query location updates continuously
			// Input.location.Stop();
		}
	}

	public static System.DateTime UnixTimeStampToDateTime( double unixTimeStamp )
	{
		// Unix timestamp is seconds past epoch
		System.DateTime dtDateTime = new System.DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
		dtDateTime = dtDateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
		return dtDateTime;
	}
}