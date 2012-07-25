using UnityEngine;
using System.Collections;


public class IOIOInterface : MonoBehaviour
{
	public bool connectOnStart = true;


	AndroidJavaClass interfaceClass;


	AndroidJavaClass InterfaceClass
	{
		get
		{
			if (interfaceClass == null)
			{
				interfaceClass = new AndroidJavaClass ("org.AngryAnt.IOIO.IOIOInterface");
			}

			return interfaceClass;
		}
	}


	void Start ()
	{
		StartInterface ();
	}


	void JavaCall (string method, params object[] arguments)
	{
		InterfaceClass.CallStatic (method, arguments);
	}


	public void StartInterface ()
	{
		JavaCall ("Start", gameObject.name);
	}


	public void Log (string message)
	{
		Debug.Log ("IOIO: " + message);
	}


	public void Connect ()
	{
		JavaCall ("Connect");
	}


	public void Disconnect ()
	{
		JavaCall ("Disconnect");
	}


	public void ToggleDigitalOutput (int pin, bool value = false)
	{
		JavaCall ("ToggleDigitalOutput", pin, value);
	}


	public void ClosePort (int pin)
	{
		JavaCall ("ClosePort");
	}


	void OnInterfaceStart ()
	{
		Debug.Log ("Interface is now running");

		if (connectOnStart)
		{
			Connect ();
		}
	}


	void OnIOIOConnected ()
	{
		Debug.Log ("IOIO board connected");
	}


	void OnIOIODisconnected ()
	{
		Debug.Log ("IOIO board disconnected");
	}


	void OnIOIOIncompatible ()
	{
		Debug.Log ("IOIO board incompatibility detection");
	}
}
