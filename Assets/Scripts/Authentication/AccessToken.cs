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
