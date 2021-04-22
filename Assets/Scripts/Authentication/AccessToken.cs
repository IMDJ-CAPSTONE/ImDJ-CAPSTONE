/*  FILE          : 	AccessToken.cs
*   PROJECT       : 	PROG3221 - Capstone
*   PROGRAMMER    : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   FIRST VERSION : 	2021-04-05
*   DESCRIPTION   : 	Contains the class we use to hold authentication data receieved from twitch
*/
using System;
using UnityEngine;

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
