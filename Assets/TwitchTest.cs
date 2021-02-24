// If type or namespace TwitchLib could not be found. Make sure you add the latest TwitchLib.Unity.dll to your project folder
// Download it here: https://github.com/TwitchLib/TwitchLib.Unity/releases
// Or download the repository at https://github.com/TwitchLib/TwitchLib.Unity, build it, and copy the TwitchLib.Unity.dll from the output directory
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;

public class TwitchTest : MonoBehaviour
{
	[SerializeField] //[SerializeField] Allows the private field to show up in Unity's inspector. Way better than just making it public
	private string _channelToConnectTo = Secrets.USERNAME_FROM_OAUTH_TOKEN;

	private Client _client;
	private Poll poll;

	private void Start()
	{
		// To keep the Unity application active in the background, you can enable "Run In Background" in the player settings:
		// Unity Editor --> Edit --> Project Settings --> Player --> Resolution and Presentation --> Resolution --> Run In Background
		// This option seems to be enabled by default in more recent versions of Unity. An aditional, less recommended option is to set it in code:
		Application.runInBackground = true;

		//Create Credentials instance
		ConnectionCredentials credentials = new ConnectionCredentials(Secrets.USERNAME_FROM_OAUTH_TOKEN, Secrets.OAUTH_TOKEN);

		// Create new instance of Chat Client
		_client = new Client();

		// Initialize the client with the credentials instance, and setting a default channel to connect to.
		_client.Initialize(credentials, _channelToConnectTo);

		// Bind callbacks to events
		_client.OnConnected += OnConnected;
		_client.OnJoinedChannel += OnJoinedChannel;
		_client.OnMessageReceived += OnMessageReceived;
		_client.OnChatCommandReceived += OnChatCommandReceived;		//research this
		_client.OnWhisperReceived += OnWhisperReceived;
		_client.OnNewSubscriber += OnNewSubscriber;
		//_client.OnBeingHosted += OnBeingHosted;
		_client.OnGiftedSubscription += OnGiftedSub;
		_client.OnModeratorJoined += OnModJoin;
		_client.OnReSubscriber += OnReSub;
		_client.OnUserBanned += UserBanned;

		
		// Connect
		_client.Connect();
	}

	

	private void OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
	{
		Debug.Log($"The bot {e.BotUsername} succesfully connected to Twitch.");

		if (!string.IsNullOrWhiteSpace(e.AutoJoinChannel))
			Debug.Log($"The bot will now attempt to automatically join the channel provided when the Initialize method was called: {e.AutoJoinChannel}");
	}

	private void OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
	{
		Debug.Log($"The bot {e.BotUsername} just joined the channel: {e.Channel}");
		_client.SendMessage(e.Channel, "I just joined the channel! PogChamp");
	}

	private void OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
	{
		if (e.ChatMessage.Message.Contains("fuck"))
            {
                //client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromSeconds(5), "Bad word! 5 second timeout!");
                _client.SendMessage(e.ChatMessage.Channel, "hey now...");
            }

            if (e.ChatMessage.Message.StartsWith("!"))
            {
                //COMMAND DETECTED
                if(e.ChatMessage.Message.StartsWith("!newpoll "))
                {
                    if(e.ChatMessage.IsModerator || e.ChatMessage.IsBroadcaster)
                    {
                        //!newpoll what should be the next song? All Star, 22, Smells Like Teen Spirit
                        
                        poll = new Poll(e.ChatMessage.Message);
                        
                    }
                    else
                    {
                        _client.SendMessage(e.ChatMessage.Channel, "only mods can use this command");
                    }
                }


                if(e.ChatMessage.Message.StartsWith("!vote"))
                {
                    if (poll.active)
                    {
                        poll.vote(e.ChatMessage.Message);
                        _client.SendMessage(e.ChatMessage.Channel, poll.display());
                    }
                    else
                    {
                        //no poll to vote so it gets ignored
                    }
                }
            }
            
		
		Debug.Log($"Message received from {e.ChatMessage.Username}: {e.ChatMessage.Message}");
	}

	private void OnChatCommandReceived(object sender, TwitchLib.Client.Events.OnChatCommandReceivedArgs e)
	{
		switch (e.Command.CommandText)
		{
			case "hello":
				_client.SendMessage(e.Command.ChatMessage.Channel, $"Hello {e.Command.ChatMessage.DisplayName}!");
				break;
			case "about":
				_client.SendMessage(e.Command.ChatMessage.Channel, "I am a Twitch bot running on TwitchLib!");
				break;
			default:
				_client.SendMessage(e.Command.ChatMessage.Channel, $"Unknown chat command: {e.Command.CommandIdentifier}{e.Command.CommandText}");
				break;
		}
	}

	private void OnWhisperReceived(object sender, TwitchLib.Client.Events.OnWhisperReceivedArgs e)
	{
		if (e.WhisperMessage.Username == "my_friend")
			_client.SendWhisper(e.WhisperMessage.Username, "Hey! Whispers are so cool!!");
	}

	private void OnNewSubscriber(object sender, TwitchLib.Client.Events.OnNewSubscriberArgs e)
	{
		/*
		if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
		{
			_client.SendMessage(e.Channel, $"{e.Subscriber.DisplayName} joined to the rave thanks to Prime!");
			_client.SendMessage(e.Channel, "Hey there! Do you want to know about Twitch Prime? Oh! You may be asking, " +
											"What's Twitch Prime? Let me tell ya! When you connect your Amazon account to your " +
											"Twitch account, you can get 1 free sub to ANY streamer on Twitch, every month! " +
											"Yup, and along with that, get yourself some Twitch loot! With Twitch loot, you can " +
											"go ahead and get yourself some exclusive Twitch gear and your favorite games! And until April 30th, " +
											"you can get yourself some Fortnite skins, with Twitch loot! So go ahead! Grab your Amazon account," +
											" grab a family or friend's Amazon Prime account, and link it to your Twitch account TODAY!");
		}                
		else
		{
			_client.SendMessage(e.Channel, $"{e.Subscriber.DisplayName} joined to the rave!");
		}

		*/
		_client.SendMessage(e.Channel, e.ToString() + " joined to the rave!");
	}

	private void OnReSub(object sender, TwitchLib.Client.Events.OnReSubscriberArgs e)
	{
		/*
		if (e.ReSubscriber.SubscriptionPlan == SubscriptionPlan.Prime)
		{
			_client.SendMessage(e.Channel, $"{e.ReSubscriber.DisplayName} has been raving for {e.ReSubscriber.Months} months thanks to Prime!!");
		}
		else
		{
			_client.SendMessage(e.Channel, $"{e.ReSubscriber.DisplayName} has been raving for {e.ReSubscriber.Months} months!!");
		}
		*/
		_client.SendMessage(e.Channel, e.ReSubscriber.DisplayName + " has been raving for " + e.ReSubscriber.Months + " months!!");
	}

	private void OnGiftedSub(object sender, TwitchLib.Client.Events.OnGiftedSubscriptionArgs e)
	{
		_client.SendMessage(e.Channel, $"{e.GiftedSubscription.DisplayName} is giving out free bumps!!");
	}

/*
	private void OnBeingHosted(object sender, TwitchLib.Client.Events.OnBeingHostedArgs e)
	{
		_client.SendMessage(e.BeingHostedNotification.Channel, $"Shoutout to {e.BeingHostedNotification.HostedByChannel} for the Host!!");
	}
*/

	private void OnModJoin(object sender, TwitchLib.Client.Events.OnModeratorJoinedArgs e)
	{
		_client.SendMessage(e.Channel, $"Everybody watch out, we got a snitch over here --> {e.Username}");
	}

	private void UserBanned(object sender, TwitchLib.Client.Events.OnUserBannedArgs e)
	{
		_client.SendMessage(e.UserBan.Channel, $"THE BAN HAMMER HAS STRUCK! rip {e.UserBan.Username}, maybe dont get caught doing {e.UserBan.BanReason} next time");
	}


	private void Update()
	{
		// Don't call the client send message on every Update,
		// this is sample on how to call the client,
		// not an example on how to code.
		if (Input.GetKeyDown(KeyCode.Space))
		{
			_client.SendMessage(_channelToConnectTo, "I pressed the space key within Unity.");
		}
	}
}