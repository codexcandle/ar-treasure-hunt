using UnityEngine;
using System;

[Serializable]
public class GameInfo
{
	public bool won;
	public int id;
	public string prompt;
	public string clue;
	public string target;

	public static GameInfo CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<GameInfo>(jsonString);
	}
}