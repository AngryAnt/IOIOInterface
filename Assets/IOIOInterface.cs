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


	T JavaCall<T> (string method, params object[] arguments)
	{
		return InterfaceClass.CallStatic<T> (method, arguments);
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


	public bool ToggleDigitalOutput (int pin, bool value)
	{
		return JavaCall<bool> ("ToggleDigitalOutput", pin, value);
	}


	public bool SetPWMCycleOutput (int pin, int frequency, float cycle)
	{
		return JavaCall<bool> ("SetPWMCycleOutput", pin, frequency, cycle);
	}


	public bool SetPWMWidthOutput (int pin, int frequency, int width)
	{
		return JavaCall<bool> ("SetPWMWidthOutput", pin, frequency, width);
	}


	public bool SetPWMPreciseWidthOutput (int pin, int frequency, float width)
	{
		return JavaCall<bool> ("SetPWMPreciseWidthOutput", pin, frequency, width);
	}


	public void Close (int pin)
	{
		JavaCall ("Close", pin);
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
