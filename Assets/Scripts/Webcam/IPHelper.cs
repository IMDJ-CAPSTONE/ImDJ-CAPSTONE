/*! @file       : 	IPHelper.cs
*   @author     : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   @date       : 	2021-03-01
*   @brief      : 	a class to help with various IP related requirments
*/

using System.Net;

/*! <summary>
*  a class to help with various IP related requirments, Original source can be found here:
*  https://stickler.de/en/information/code-snippets/get-external-ip
*  </summary>
*/
public static class IPHelper
{
    /*! <summary>
     *  this function looks for the public/external ip address of the host
     *  </summary>
     *  <param name="none"></param>
     *  <returns>IPAddres : The IP if it is private, otherwise null</returns>
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

    /*! <summary>
     *  Checks if the given IP is private
     *  </summary>
     *  <param name="myIPAddress">the address you want to check</param>
     *  <returns>bool : True if the passed in IP is private</returns>
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

    /*! <summary>
     *  this function compares two IP addresses and returns a true if the are identical
     *  </summary>
     *  <param name="IPAddress1">First IP to compare</param>
     *  <param name="IPAddress2">Second IP to compare</param>
     *  <returns>bool : True if they match otherwise false</returns>
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
