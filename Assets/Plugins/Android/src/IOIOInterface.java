package org.AngryAnt.IOIO;


import org.AngryAnt.IOIO.*;
import com.unity3d.player.*;
import ioio.lib.api.*;


public class IOIOInterface
{
	private static String s_ControllerName;
	private static IOIO s_IOIO;


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
}
