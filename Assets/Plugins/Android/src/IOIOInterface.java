package org.AngryAnt.IOIO;

import com.unity3d.player.UnityPlayer;


public class IOIOInterface
{
	private static String m_ControllerName;


	public static void Start (String sender)
	{
		m_ControllerName = sender;

		System.out.println ("IOIOInterface started from controller: " + m_ControllerName);

		MessageController ("OnInterfaceStart", "");
	}


	private static void MessageController (String function, String message)
	{
		UnityPlayer.UnitySendMessage (m_ControllerName, function, message);
	}
}