package org.AngryAnt.IOIO;


import org.AngryAnt.IOIO.*;
import com.unity3d.player.*;
import ioio.lib.api.*;
import ioio.lib.api.exception.*;


public class IOIOInterface
{
	private static final int kPortCount = 48;


	private static String s_ControllerName;
	private static IOIO s_IOIO;
	private static Closeable[] s_Ports = new Closeable[kPortCount];


	public static boolean ValidPin (int pin)
	{
		return pin > 0 && pin <= kPortCount;
	}


	public static boolean ValidPWMPin (int pin)
	// NOTE: Max 9 simultaneous PWM signals
	// NOTE: PWM ports: [3-7], [9-14], [27-32], ([34-40], [45-48])
	{
		return	(pin > 2 && pin < 8) ||
				(pin > 8 && pin < 15) ||
				(pin > 26 && pin < 33) ||
				(pin > 33 && pin < 41) ||
				(pin > 44 && pin < 49);
	}


	private static void MessageController (String function, String message)
	{
		UnityPlayer.UnitySendMessage (s_ControllerName, function, message);
	}


	public static void Log (String message)
	{
		MessageController ("Log", message);
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


	public static void Disconnect ()
	{
		IOIOWaitThread.Wait (s_IOIO, IOIOWaitThread.WaitTask.Disconnect);
	}


	public static boolean ToggleDigitalOutput (int pin, boolean value)
	{
		if (!ValidPin (pin))
		{
			Log ("Pin out of range: " + pin);

			return false;
		}

		int index = pin - 1;

		if (s_Ports[index] != null)
		// TODO: Only close it if not a digital output. Otherwise just set it. Use Java keyword instanceof.
		{
			Close (pin);
		}

		try
		{
			s_Ports[index] = s_IOIO.openDigitalOutput (pin, value);

			return true;
		}
		catch (ConnectionLostException e)
		{
			Log ("Lost connection during ToggleDigitalOutput");
			OnIOIODisconnected ();

			return false;
		}
	}


	private static boolean SetPWMOutput (int pin, int frequency, PWMOutputSetting setting)
	// NOTE: To change the frequency of a PWM output, you must first close it
	{
		if (!ValidPWMPin (pin))
		{
			Log ("Pin out of PWM range: " + pin);

			return false;
		}

		int index = pin - 1;

		if (s_Ports[index] != null)
		// TODO: Only close it if not a PWM output. Otherwise just set it. Use Java keyword instanceof.
		{
			Close (pin);
		}

		try
		{
			s_Ports[index] = s_IOIO.openPwmOutput (pin, frequency);
				// TODO: Need to get number of active PWM outputs from IOIO board before attempting to open a new one. Max is 9.

			return setting.Set ((PwmOutput)s_Ports[index]);
		}
		catch (ConnectionLostException e)
		{
			Log ("Lost connection during SetPWMOutput");
			OnIOIODisconnected ();

			return false;
		}
	}


	public static boolean SetPWMCycleOutput (int pin, int frequency, float cycle)
	{
		return SetPWMOutput (pin, frequency, PWMOutputSetting.SetDutyCycle (cycle));
	}


	public static boolean SetPWMWidthOutput (int pin, int frequency, int width)
	{
		return SetPWMOutput (pin, frequency, PWMOutputSetting.SetPulseWidth (width));
	}


	public static boolean SetPWMPreciseWidthOutput (int pin, int frequency, float width)
	{
		return SetPWMOutput (pin, frequency, PWMOutputSetting.SetPrecisePulseWidth (width));
	}


	public static void Close (int pin)
	{
		if (!ValidPin (pin))
		{
			Log ("Pin out of range: " + pin);
		}

		s_Ports[pin - 1].close ();
	}


	public static void OnIOIOConnected ()
	{
		MessageController ("OnIOIOConnected", "");
	}


	public static void OnIOIODisconnected ()
	{
		MessageController ("OnIOIODisconnected", "");
	}


	public static void OnIOIOIncompatible ()
	{
		MessageController ("OnIOIOIncompatible", "");
	}
}
