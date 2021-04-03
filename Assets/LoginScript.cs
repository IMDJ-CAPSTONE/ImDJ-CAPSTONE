using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Net;
using System.Threading;


public class LoginScript : MonoBehaviour
{
    private HttpListener listener;
    private Thread listenerThread;

    private string clientId = "8yy8vq1az7kz901p7cgjr4lmlosfwn";
    private string sec = "o1117owvzk8gi00smeq8f9irh6bku5";

    private string redirectUrl = "https://localhost:9989/";
    private string claims = "claims={\"id_token\":{\"email\":null,\"email_verified\":null},\"userinfo\":{\"picture\":null,\"preferred_username\": null}}";

    private string scope = "user:read:email";

    

    url = url.Replace("&", "^&");

        Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
    // Start is called before the first frame update
    void Start()
    {
        string url = $"https://id.twitch.tv/oauth2/authorize?client_id={clientId}&redirect_uri={redirectUrl}&response_type=code&scope={scope}+openid&claims={claims}";
        listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:9989/");
        listener.Prefixes.Add("http://127.0.0.1:9989/");
        listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
        listener.Start();

        listenerThread = new Thread(startListener);
        listenerThread.Start();
        Debug.Log("Server Started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void startListener()
    {
        while (true)
        {
            var result = listener.BeginGetContext(ListenerCallback, listener);
            result.AsyncWaitHandle.WaitOne();
        }
    }

    private void ListenerCallback(IAsyncResult result)
    {
        var context = listener.EndGetContext(result);

        Debug.Log("Method: " + context.Request.HttpMethod);
        Debug.Log("LocalUrl: " + context.Request.Url.LocalPath);

        if (context.Request.QueryString.AllKeys.Length > 0)
            foreach (var key in context.Request.QueryString.AllKeys)
            {
                Debug.Log("Key: " + key + ", Value: " + context.Request.QueryString.GetValues(key)[0]);
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
}
