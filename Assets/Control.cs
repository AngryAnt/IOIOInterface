using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Debug = UnityEngine.Debug;


[RequireComponent (typeof (IOIOInterface))]
[RequireComponent (typeof (NetworkView))]
public class Control : NetworkLogPairer
{
	const int kMaxLogLines = 10;


	public Font bigFont;


	GUIStyle bigLabel;
	bool interactive = false;
	List<string> log = new List<string> (), logcatBuffer = new List<string> ();
	IOIOInterface ioioInterface;
	Process logcat;
	int firstLogFrame = -1;


	void Awake ()
	{
		Application.RegisterLogCallback (OnLog);
		ioioInterface = GetComponent<IOIOInterface> ();
		if (Application.platform == RuntimePlatform.Android)
		{
			server = true;
		}
		else
		{
			server = false;
			ioioInterface.enabled = false;
		}
	}


	protected override void Start ()
	{
		base.Start ();

		bigLabel = new GUIStyle ()
		{
			name = "Big label",
			font = bigFont
		};
		bigLabel.normal.textColor = Color.white;

		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		if (Application.platform != RuntimePlatform.Android)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo ("/android-sdk-mac_86/platform-tools/adb", "logcat")
			{
				UseShellExecute = false,
				RedirectStandardOutput = true,
			};

			logcat = Process.Start (startInfo);

			logcat.OutputDataReceived += (object sender, DataReceivedEventArgs args) => {
				int index = args.Data.IndexOf ("): ");
				if (index > 0)
				{
					logcatBuffer.Add (args.Data.Substring (index + 3));
				}
			};

			logcat.BeginOutputReadLine ();
		}
	}


	void OnApplicationQuit ()
	{
		if (logcat != null && !logcat.HasExited)
		{
			logcat.Kill ();
		}
	}


	protected override string[] ReadLog ()
	{
		if (logcat == null || logcatBuffer == null)
		{
			return new string[0];
		}

		if (firstLogFrame == -1)
		{
			firstLogFrame = Time.frameCount;

			logcatBuffer.Clear ();

			return new string[0];
		}
		else if (firstLogFrame == Time.frameCount)
		{
			logcatBuffer.Clear ();

			return new string[0];
		}

		string[] result = logcatBuffer.ToArray ();

		logcatBuffer.Clear ();

		return result;
	}


	void OnConnectedToServer ()
	{
		Debug.Log ("Connected to device");
		logcat.Kill ();
		logcatBuffer.Clear ();
	}


	void OnPlayerConnected ()
	{
		Debug.Log ("Remote controller connected");
	}


	void OnLog (string message, string callStack, LogType type)
	{
		if (message.IndexOf (identifier) == 0)
		{
			return;
		}

		log.Insert (0, message);
		while (log.Count > kMaxLogLines)
		{
			log.RemoveAt (log.Count - 1);
		}
	}


	[RPC]
	void Port48On ()
	{
		ioioInterface.ToggleDigitalOutput (48, true);
	}


	[RPC]
	void Port48Off ()
	{
		ioioInterface.ToggleDigitalOutput (48, false);
	}


	const int
		kPinAIN1 = 41,
		kPinAIN2 = 40,
		kPinPWMA = 39,
		kPinPWMB = 45,
		kPinBIN2 = 44,
		kPinBIN1 = 43,
		kPinSTDBY = 42,
		kPWMFreq = 100;


	[RPC]
	void MotorAForward ()
	{
		ioioInterface.SetPWMCycleOutput (kPinPWMA, kPWMFreq, 1.0f);
		ioioInterface.ToggleDigitalOutput (kPinAIN1, true);
		ioioInterface.ToggleDigitalOutput (kPinAIN2, false);
		ioioInterface.ToggleDigitalOutput (kPinSTDBY, true);
	}


	[RPC]
	void MotorABack ()
	{
		ioioInterface.SetPWMCycleOutput (kPinPWMA, kPWMFreq, 1.0f);
		ioioInterface.ToggleDigitalOutput (kPinAIN1, false);
		ioioInterface.ToggleDigitalOutput (kPinAIN2, true);
		ioioInterface.ToggleDigitalOutput (kPinSTDBY, true);
	}


	[RPC]
	void MotorAStop ()
	{
		ioioInterface.ToggleDigitalOutput (kPinSTDBY, false);
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

			if (GUILayout.Button ("Forward"))
			{
				if (Network.isClient)
				{
					networkView.RPC ("MotorAForward", RPCMode.Server);
				}
				else
				{
					MotorAForward ();
				}
			}

			if (GUILayout.Button ("Back"))
			{
				if (Network.isClient)
				{
					networkView.RPC ("MotorABack", RPCMode.Server);
				}
				else
				{
					MotorABack ();
				}
			}

			if (GUILayout.Button ("Stop"))
			{
				if (Network.isClient)
				{
					networkView.RPC ("MotorAStop", RPCMode.Server);
				}
				else
				{
					MotorAStop ();
				}
			}

			if (GUILayout.Button ("Port 48: on", GUILayout.Width (150.0f), GUILayout.Height (150.0f)))
			{
				if (Network.isClient)
				{
					networkView.RPC ("Port48On", RPCMode.Server);
				}
				else
				{
					Port48On ();
				}
			}

			if (GUILayout.Button ("Port 48: off", GUILayout.Width (150.0f), GUILayout.Height (150.0f)))
			{
				if (Network.isClient)
				{
					networkView.RPC ("Port48Off", RPCMode.Server);
				}
				else
				{
					Port48Off ();
				}
			}
		}

		if (Application.platform != RuntimePlatform.Android)
		{
			if (Event.current.type == EventType.Repaint)
			{
				interactive = Network.isClient;
			}
		}
		else if (Event.current.type == EventType.MouseDown)
		{
			interactive = !interactive;
		}
	}
}
