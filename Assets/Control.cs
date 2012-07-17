using UnityEngine;
using System.Collections;


public class Control : MonoBehaviour
{
	void Start ()
	{
		AndroidJNIHelper.debug = true;
			// TODO: Is this needed or just example fluff?

		using (AndroidJavaClass javaClass = new AndroidJavaClass ("org.AngryAnt.IOIO.IOIOInterface"))
		{
			javaClass.CallStatic ("Start", gameObject.name);
		}
	}


	void OnInterfaceStart ()
	{
		Debug.Log ("Interface is now running");
	}
}
