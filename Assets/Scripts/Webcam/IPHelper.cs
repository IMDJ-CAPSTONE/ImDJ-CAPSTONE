/*  FILE          : 	PerformerUserUIController.cs
*   PROJECT       : 	PROG3221 - Capstone
*   PROGRAMMER    : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   FIRST VERSION : 	2021-04-05
*   DESCRIPTION   : 	a class to help with various IP related requirments 
*/

using System.Net;

// https://stickler.de/en/information/code-snippets/get-external-ip

public static class IPHelper
{
	/*  Function	:	GetExternalIPAddress()
    *
    *	Description	:	this function looks for the public/external ip address of the host
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
	public static IPAddress GetExternalIPAddress()
	{
		IPHostEntry myIPHostEntry = Dns.GetHostEntry(Dns.GetHostName());

		foreach (IPAddress myIPAddress in myIPHostEntry.AddressList)
		{
			byte[] ipBytes = myIPAddress.GetAddressBytes();

			if (myIPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
			{
				if (!IsPrivateIP(myIPAddress))
				{
					return myIPAddress;
				}
			}
		}

		return null;
	}

	/*  Function	:	IsPrivateIP()
    *
    *	Description	:	this function get called before anything else happens
    *
    *	Parameters	:	IPAddress myIPAddress :  the address you want to check
    *
    *	Returns		:	bool telling if the passed in IP is private or not
    */
	private static bool IsPrivateIP(IPAddress myIPAddress)
	{
		if (myIPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
		{
			byte[] ipBytes = myIPAddress.GetAddressBytes();

			// 10.0.0.0/24 
			if (ipBytes[0] == 10)
			{
				return true;
			}
			// 172.16.0.0/16
			else if (ipBytes[0] == 172 && ipBytes[1] == 16)
			{
				return true;
			}
			// 192.168.0.0/16
			else if (ipBytes[0] == 192 && ipBytes[1] == 168)
			{
				return true;
			}
			// 169.254.0.0/16
			else if (ipBytes[0] == 169 && ipBytes[1] == 254)
			{
				return true;
			}
		}

		return false;
	}

	/*  Function	:	CompareIpAddress()
    *
    *	Description	:	this function compares two IP addresses and returns a true if the are identical
    *
    *	Parameters	:	IPAddress IPAddress1 : the first address
    *					IPAddress IPAddress2 : the other address
    *
    *	Returns		:	bool : true if they match otherwise false
    */
	private static bool CompareIpAddress(IPAddress IPAddress1, IPAddress IPAddress2)
	{
		byte[] b1 = IPAddress1.GetAddressBytes();
		byte[] b2 = IPAddress2.GetAddressBytes();

		if (b1.Length == b2.Length)
		{
			for (int i = 0; i < b1.Length; ++i)
			{
				if (b1[i] != b2[i])
				{
					return false;
				}
			}
		}
		else
		{
			return false;
		}

		return true;
	}
}
