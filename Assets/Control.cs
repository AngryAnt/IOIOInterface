using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Control : MonoBehaviour
{
	const int kMaxLogLines = 10;


	public Font bigFont;


	GUIStyle bigLabel;
	bool interactive = false;
	List<string> log = new List<string> ();


	void Awake ()
	{
		Application.RegisterLogCallback (OnLog);
	}


	void Start ()
	{
		bigLabel = new GUIStyle ()
		{
			name = "Big label",
			font = bigFont
		};
		bigLabel.normal.textColor = Color.white;

		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}


	void OnLog (string message, string callStack, LogType type)
	{
		log.Insert (0, message);
		while (log.Count > kMaxLogLines)
		{
			log.RemoveAt (log.Count - 1);
		}
	}


	void OnGUI ()
	{
		GUI.contentColor = Color.white;

		GUILayout.BeginArea (new Rect (0.0f, 0.0f, Screen.width, Screen.height));
			GUILayout.BeginHorizontal ();
				GUILayout.FlexibleSpace ();
				GUILayout.BeginVertical ();
					GUILayout.FlexibleSpace ();
					GUILayout.Label ("IOIO", bigLabel);
					foreach (string entry in log)
					{
						GUILayout.Label (entry);
					}
					GUILayout.FlexibleSpace ();
				GUILayout.EndVertical ();
				GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
		GUILayout.EndArea ();

		if (interactive)
		{
			if (GUILayout.Button ("Quit", GUILayout.Width (150.0f), GUILayout.Height (150.0f)))
			{
				Application.Quit ();
			}
		}

		if (Event.current.type == EventType.MouseDown)
		{
			interactive = !interactive;
		}
	}
}
