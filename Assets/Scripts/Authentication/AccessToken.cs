/*! @file       : 	AccessToken.cs
*   @author     : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   @date       : 	2021-03-01
*   @brief      : 	Contains the class we use to hold authentication data receieved from twitch
*/

using System;

/*! <summary>
*  Contains all the data needed to be authorized using Twitch.tv oauth2 servers
*  </summary>
*/
[Serializable]
public class AccessToken
{
    public string access_token { get; set; }
    public int expires_in { get; set; } = 0;
    public string id_token { get; set; }
    public string refresh_token { get; set; }
    public string[] scope { get; set; }
    public string token_type { get; set; }
}
