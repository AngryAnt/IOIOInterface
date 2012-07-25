using UnityEngine;
using System.Collections;
using System.Linq;
using System.Net.Sockets;
using System.Net;


public abstract class NetworkLogPairer : MonoBehaviour
{
	public bool server;
	public int port = 557721;
	public string identifier = "NetworkPair";
	public float broadcastInterval = 3.0f;


	static IPAddress ip;


	protected virtual void Start ()
	{
		if (server)
		{
			Network.InitializeServer (1, port, false);
		}
		else
		{
			StartCoroutine (ScanForService ());
		}
	}


	protected abstract string[] ReadLog ();


	public static IPAddress IP
	{
		get
		{
			if (ip == null)
			{
				ip = (
					from entry in Dns.GetHostEntry (Dns.GetHostName ()).AddressList
						where entry.AddressFamily == AddressFamily.InterNetwork
							select entry
				).FirstOrDefault ();
			}

			return ip;
		}
	}


	IEnumerator OnServerInitialized ()
	{
		while (Application.isPlaying && Network.connections.Length == 0)
		{
			Debug.Log (identifier + IP);
			yield return new WaitForSeconds (broadcastInterval);
		}
	}


	IEnumerator ScanForService ()
	{
		while (Application.isPlaying)
		{
			foreach (string line in ReadLog ())
			{
				if (line.IndexOf (identifier) == 0)
				{
					Network.Connect (line.Substring (identifier.Length), port);
					yield break;
				}
			}
			yield return new WaitForSeconds (broadcastInterval);
		}
	}
}
