package org.AngryAnt.IOIO;


import org.AngryAnt.IOIO.*;
import ioio.lib.api.*;
import ioio.lib.api.exception.*;


public class IOIOWaitThread extends Thread
{
	public enum WaitTask
	{
		Connect,
		Disconnect
	};


	private IOIO m_IOIO;
	private WaitTask m_Task;
	private static IOIOWaitThread s_Instance;


	public static void Wait (IOIO ioio, WaitTask task)
	{
		if (s_Instance != null)
		{
			s_Instance.interrupt ();
		}

		s_Instance = new IOIOWaitThread (ioio, task);
		s_Instance.start ();
	}


	private IOIOWaitThread (IOIO ioio, WaitTask task)
	{
		m_IOIO = ioio;
		m_Task = task;
	}


	public void run ()
	{
		try
		{
			switch (m_Task)
			{
				case Connect:
					m_IOIO.waitForConnect ();

					IOIOInterface.OnIOIOConnected ();
				break;
				case Disconnect:
					m_IOIO.waitForDisconnect ();

					IOIOInterface.OnIOIODisconnected ();
				break;
			}
		}
		catch (ConnectionLostException e)
		{
			IOIOInterface.Log ("Lost connection during IOIOWaitThread (" + m_Task + ")");
			IOIOInterface.OnIOIODisconnected ();
		}
		catch (IncompatibilityException e)
		{
			IOIOInterface.Log ("Incompatibility detected during IOIOWaitThread (" + m_Task + ")");
			IOIOInterface.OnIOIOIncompatible ();
		}
		catch (InterruptedException e)
		// This one is most likely our own doing
		{}
	}
}
