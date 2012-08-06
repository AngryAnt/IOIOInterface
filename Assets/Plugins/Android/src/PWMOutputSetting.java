package org.AngryAnt.IOIO;


import org.AngryAnt.IOIO.*;
import ioio.lib.api.*;
import ioio.lib.api.exception.*;


public class PWMOutputSetting
{
	private enum PWMType
	{
		DutyCycle,
		PulseWidth,
		PrecisePulseWidth
	};


	private PWMType m_Type;
	private int m_IntegerValue;
	private float m_FloatValue;


	public static PWMOutputSetting SetDutyCycle (float cycle)
	{
		return new PWMOutputSetting (PWMType.DutyCycle, 0, Math.min (1.0f, Math.max (0.0f, cycle)));
	}


	public static PWMOutputSetting SetPulseWidth (int microSeconds)
	{
		return new PWMOutputSetting (PWMType.PulseWidth, microSeconds, 0.0f);
	}


	public static PWMOutputSetting SetPrecisePulseWidth (float microSeconds)
	{
		return new PWMOutputSetting (PWMType.PrecisePulseWidth, 0, microSeconds);
	}


	private PWMOutputSetting (PWMType type, int integerValue, float floatValue)
	{
		m_Type = type;
		m_IntegerValue = integerValue;
		m_FloatValue = floatValue;
	}


	public boolean Set (PwmOutput output)
	{
		try
		{
			switch (m_Type)
			{
				case DutyCycle:
					output.setDutyCycle (m_FloatValue);
				break;
				case PulseWidth:
					output.setPulseWidth (m_IntegerValue);
				break;
				case PrecisePulseWidth:
					output.setPulseWidth (m_FloatValue);
				break;
			}

			return true;
		}
		catch (ConnectionLostException e)
		{
			IOIOInterface.Log ("Lost connection during PWMOutputSetting.Set (" + m_Type + ")");
			IOIOInterface.OnIOIODisconnected ();

			return false;
		}
	}
}
