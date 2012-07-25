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


	void OnInterfaceStart ()
	{
		Debug.Log ("Interface is now running");

		if (connectOnStart)
		{
			Connect ();
		}
	}


	public void Connect ()
	{
		JavaCall ("Connect");
	}


	void OnIOIOConnected ()
	{
		Debug.Log ("IOIO board connected");
	}


	public void Disconnect ()
	{
		JavaCall ("Disconnect");
	}


	void OnIOIODisconnected ()
	{
		Debug.Log ("IOIO board disconnected");
	}


	public void ToggleDigitalOutput (int pin, bool value = false)
	{
		JavaCall ("ToggleDigitalOutput", pin, value);
	}


	public void ClosePort (int pin)
	{
		JavaCall ("ClosePort");
	}
}
