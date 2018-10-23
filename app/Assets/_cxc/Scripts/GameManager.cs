using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager:MonoBehaviour
{
	public GameObject loginPanel;
	public GameObject promptPanel;
	public Text promptPanelPrompt1;
	public Text promptPanelPrompt2;
	public GameObject cluePanel;
	public Text cluePanelPrompt;
	public GameObject successPanel;
	public Text successPanelText;
	public Dropdown dd;
	public Button submit;
	public Text level;
	public bool searching;

	private bool teamSelected;
	private string targetID;
	private GameInfo levelInfo;

	public void SetDropdownIndex(int index)
	{
		dd.value = index;
	}

	public void HandleLoginButtonClick()
	{
		if(teamSelected)
		{
			StartCoroutine(HandleSubmit());
		}
	}

	public void HandleClueButtonClick()
	{
		promptPanel.SetActive(false);

		// show "clue" panel
		cluePanel.SetActive(true);

		searching = true;
	}

	public void HandleTargetFound(string vuforiaTargetID)
	{
		if(searching == true)
		{
			if (vuforiaTargetID == targetID)
			{
				searching = false;

				cluePanel.SetActive (false);

				successPanel.SetActive (true);

				successPanelText.text = "Success! You found " + vuforiaTargetID + "!";
			
				StartCoroutine (NotifyLevelTargetFound (vuforiaTargetID));
			}
			else
			{
				Debug.Log ("*** Unfortunately, found target: " + vuforiaTargetID + ", but needed target: " + targetID);
			}
		}
	}

	public void HandleSuccessButtonClick()
	{
		successPanel.SetActive(false);

		promptPanel.SetActive (true);
	}

	void Start()
	{
		StartCoroutine(GetTeamList());

		dd.onValueChanged.AddListener(delegate
		{
			OnDDValueChange(dd);
		});

		// clear "level" text
		UpdateLevelText();

		// hide "prompt" panel
		promptPanel.SetActive(false);

		// hide "clue" panel
		cluePanel.SetActive(false);

		// hide "success" panel
		successPanel.SetActive(false);
	}

	void Destroy()
	{
		dd.onValueChanged.RemoveAllListeners();
	}

	private void EnableSubmitButton(bool value)
	{
		teamSelected = true;

		// TODO - figure out why color theme wasn't showing if disabled (hence hack boolean used above!)?
		// submit.enabled = value;
	}

	private void UpdateLevelText(int levelNum = -1)
	{
		if(levelNum >= 1)
		{
			level.text = "Level: " + levelNum;
		}
		else
		{
			level.text = "";
		}
	}

	private void OnDDValueChange(Dropdown target)
	{
		EnableSubmitButton(true);
	}

	private void LoadPromptPanel(bool enable, GameInfo info)
	{
		// show "clue" panel
		promptPanel.SetActive(enable);

		// set "prompt panel" text (parsing w/ newlines!)
		promptPanelPrompt1.text = System.Text.RegularExpressions.Regex.Unescape(info.prompt);
		promptPanelPrompt2.text = cluePanelPrompt.text = info.clue;

		// set "level target" id
		targetID = info.target;

		// store level info
		levelInfo = info;
	}

	private IEnumerator GetTeamList()
	{
		string post_url = "http://viscira3.com/visciraar/fetchTeams.php";
		// string post_url =phpPath+ "?username=" + WWW.EscapeURL(profileName) + "&password=" + WWW.EscapeURL(password);
		// string post_url = "http://viscira3.com/visciraar/test.php";	// ?f=treasureFunction";

		// Post the URL to the site and create a download object to get the result
		WWW hs_post = new WWW(post_url);

		// wait until the download is done
		yield return hs_post; 

		if(hs_post.error != null)
		{
			Debug.Log("error" + hs_post.error);
		}
		else
		{
			dd.ClearOptions();

			// populate "team names" dropdown
			string[] objects = JsonHelper.getJsonArray<string> (hs_post.text);
			foreach(string option in objects)
			{
				dd.options.Add(new Dropdown.OptionData(option));
			}

			dd.captionText.text = "Choose your team.";
		}
	}

	private IEnumerator HandleSubmit()
	{
		string post_url = "http://viscira3.com/visciraar/login.php?f=login";	

		var form = new WWWForm();
		form.AddField("teamIndex", dd.value);

		WWW hs_post = new WWW (post_url, form);

		// wait until the download is done
		yield return hs_post; 

		if(hs_post.error != null)
		{
			Debug.Log("error" + hs_post.error);
		}
		else
		{
			if(hs_post.text != null)
			{
				// success! - parse "level-specific" data
				GameInfo[] objects = JsonHelper.getJsonArray<GameInfo> (hs_post.text);

				// assume we only get 1 response back anyhow from above...
				GameInfo obj = objects[0];

				UpdateLevelText(obj.id);

				// hide "login" panel
				loginPanel.SetActive(false);

				// show "prompt" panel
				LoadPromptPanel(true, obj);
			}
			else
			{
				Debug.Log ("Fail @ HandleSubmitResponse.");
			}
		}
	}

	private IEnumerator NotifyLevelTargetFound(string targetID)
	{
		string post_url = "http://viscira3.com/visciraar/handleLevelTargetFound.php?f=handleLevelTargetFound";	

		var form = new WWWForm();

		form.AddField("teamIndex", dd.value);
		form.AddField("targetID", targetID);

		WWW hs_post = new WWW (post_url, form);

		// wait until the download is done
		yield return hs_post; 

		// if (hs_post.error != null) {
		// success! - parse "level-specific" data
		GameInfo[] objects = JsonHelper.getJsonArray<GameInfo> (hs_post.text);

		// assume we only get 1 response back anyhow from above...
		GameInfo obj = objects [0];

		if(obj.won == true)
		{
			EndGame();
		}
		else
		{
			UpdateLevelText (obj.id);

			LoadPromptPanel (false, obj);
			// } else {
			//	Debug.Log ("Fail @ NotifyLevelTargetFound.");
			// }
		}
	}

	private void EndGame()
	{
		successPanelText.text = "Congratulations, You Win!";
	}
}