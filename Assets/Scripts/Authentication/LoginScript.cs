/*  FILE          : 	LoginScript.cs
*   PROJECT       : 	PROG3221 - Capstone
*   PROGRAMMER    : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   FIRST VERSION : 	2021-04-05
*   DESCRIPTION   : 	This file handles the Login verification with Twitch oauth2
*/

using Proyecto26;
using System;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

public class LoginScript : MonoBehaviour
{
    public GameObject appMenu;
    private static AccessToken accessToken = null;
    private HttpListener listener;
    private Thread listenerThread;
    private static string clientId = "8yy8vq1az7kz901p7cgjr4lmlosfwn";
    private static string clientSecret = "cceidhgj2wovfk4fru33ki9gwyodut";
    private static string redirectUri = "http://localhost:9989/";
    private static string claims = "claims={\"id_token\":{\"email\":null,\"email_verified\":null},\"userinfo\":{\"picture\":null,\"preferred_username\": null}}";
    private static string scope = "user:read:email";
    private static string authCode = null;
    private static string tokenUrl = null;
    private static bool runServer = true;
    private static bool calledAPI = true;
    private static bool authenticated = false;
    
    /*  Function	:	Start()
    *
    *	Description	:	This function get called before anything else happens
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
    void Start()
    {
        listener = new HttpListener();
        listener.Prefixes.Add(redirectUri);

        listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
        listener.Start();

        listenerThread = new Thread(startListener);
        Debug.Log("Server Starting");
        listenerThread.Start();
        Debug.Log("Server Started");
    }

    /*  Function	:	Update()
    *
    *	Description	:	This function get called once every frame
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
    void Update()
    {
        if (!runServer && calledAPI)
        {
            listener.Stop();
            Debug.Log("stoped Server");
            Debug.Log("making Post CAll");
            GetToken();
            calledAPI = false;
        }

        if (authenticated)
        {
            appMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    /*  Function	:	GetToken()
    *
    *	Description	:	This function sends an HTTP Post to Twitch oauth2 server with client data and if it receives
    *	                an HTTP 200 as response then the user has been succesfully authorized 
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
    void GetToken()
    {
        RestClient.Post(tokenUrl, "").Then(response => {
            
            if(response.StatusCode == 200)
            {
                //EditorUtility.DisplayDialog("Authetication", "You have succsfully Logedin Using Twitch", "Ok");
                Debug.Log("You have succsfully Logedin Using Twitch");
                authenticated = true;
            }
            else
            {
                //EditorUtility.DisplayDialog("Authetication", "There is an Error Please try again", "Error");
                Debug.Log("There is an Error Please try again");
            }
            accessToken = StringSerializationAPI.Deserialize(typeof(AccessToken),response.Text) as AccessToken;
            Debug.Log(accessToken.id_token);
        });
    }

    /*  Function	:	startListener()
    *
    *	Description	:	This function get runs in its own thread, 
    *	                it wait for a HTTP response and then passes the data to the ListenerCallback() function
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
    private void startListener()
    {
        while (runServer)
        {
            var result = listener.BeginGetContext(ListenerCallback, listener);
            result.AsyncWaitHandle.WaitOne();
        }     
    }

    /*  Function	:	ListenerCallback()
    *
    *	Description	:	This function takes the results of the HTTP and uses the data to build a URL to Twitch oauth2 server
    *
    *	Parameters	:	IAsyncResult result : data recieved from startListener() recieving data
    *
    *	Returns		:	Void
    */
    private void ListenerCallback(IAsyncResult result)
    {
        var context = listener.EndGetContext(result);
        Debug.Log("Method: " + context.Request.HttpMethod);
        Debug.Log("LocalUrl: " + context.Request.Url.LocalPath);

        if (context.Request.QueryString.AllKeys.Length > 0)
        {
            foreach (var key in context.Request.QueryString.AllKeys)
            {
                Debug.Log("Key: " + key + ", Value: " + context.Request.QueryString.GetValues(key)[0]);
                if (key == "code")
                {
                    authCode = context.Request.QueryString.GetValues(key)[0];
                    tokenUrl = $"https://id.twitch.tv/oauth2/token?client_id={clientId}&client_secret={clientSecret}&grant_type=authorization_code&redirect_uri={redirectUri}&code={authCode}";
                    Debug.Log(tokenUrl);
                    runServer = false;
                }
            }
        }

        if (context.Request.HttpMethod == "POST")
        {
            Thread.Sleep(1000);
            var data_text = new StreamReader(context.Request.InputStream,
                                context.Request.ContentEncoding).ReadToEnd();
            Debug.Log(data_text);
        }
        context.Response.Close();
    }

    /*  Function	:	SignInWithTwitch()
    *
    *	Description	:	This function builds a url using client data and opens it, it pings Twitch's oauth2 server
    *
    *	Parameters	:	None
    *
    *	Returns		:	Void
    */
    public void SignInWithTwitch()
    {
        string openUrl = $"https://id.twitch.tv/oauth2/authorize?client_id={clientId}&redirect_uri={redirectUri}&response_type=code&scope={scope}+openid&claims={claims}";
        Application.OpenURL(openUrl);
    }
}


