package org.AngryAnt.IOIO;


import org.AngryAnt.IOIO.*;
import com.unity3d.player.*;
import ioio.lib.api.*;
import ioio.lib.api.exception.*;


public class IOIOInterface
{
	private static String s_ControllerName;
	private static IOIO s_IOIO;
	private static Closeable[] s_Ports = new Closeable[48];


	private static void MessageController (String function, String message)
	{
		UnityPlayer.UnitySendMessage (s_ControllerName, function, message);
	}


	public static void Start (String sender)
	{
		s_ControllerName = sender;

		s_IOIO = IOIOFactory.create ();

		MessageController ("OnInterfaceStart", "");
	}


	public static void Connect ()
	{
		IOIOWaitThread.Wait (s_IOIO, IOIOWaitThread.WaitTask.Connect);
	}


	public static void OnIOIOConnected ()
	{
		MessageController ("OnIOIOConnected", "");
	}


	public static void Disconnect ()
	{
		IOIOWaitThread.Wait (s_IOIO, IOIOWaitThread.WaitTask.Disconnect);
	}


	public static void OnIOIODisconnected ()
	{
		MessageController ("OnIOIODisconnected", "");
	}


	public static void ToggleDigitalOutput (int pin)
	{
		ToggleDigitalOutput (pin, false);
	}


	public static void ToggleDigitalOutput (int pin, boolean value)
	{
		int index = pin - 1;

		if (s_Ports[index] != null)
		{
			s_Ports[index].close ();
		}

		try
		{
			s_Ports[index] = s_IOIO.openDigitalOutput (pin, value);
		}
		catch (ConnectionLostException e)
		{
			// TODO: Exception handling
		}
	}


	public static void ClosePort (int pin)
	{
		s_Ports[pin].close ();
	}
}
