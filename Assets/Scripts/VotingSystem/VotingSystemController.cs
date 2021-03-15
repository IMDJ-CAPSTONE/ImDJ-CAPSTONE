using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.IO;


public class VotingSystemController : MonoBehaviour
{
    #region Public Members
    public string Question = "";
	public string TwitchChannelName = "";
    #endregion


    #region Private Members
    private Client _client;
	private Dictionary<int, OptionData> options = new Dictionary<int, OptionData>();
	private string Username;
	private string OAuth;
    #endregion

    private void Start() 
	{
		//pull JSON secrets
		using (StreamReader r = new StreamReader("./Secrets.json"))
        {
            string json = r.ReadToEnd();
            JObject jobj = JObject.Parse(json);
			//pull the property from JSONObject then parse the value out
			JProperty jOAuth = jobj.Property("OAUTH_TOKEN");
			OAuth = jOAuth.Value.ToString();
			JProperty jUsername = jobj.Property("USERNAME");
			Username = jUsername.Value.ToString();
        }
		
		Application.runInBackground = true;

		//Create Credentials instance
		ConnectionCredentials credentials = new ConnectionCredentials(Username, OAuth);

		// Create new instance of Chat Client
		_client = new Client();

		// Initialize the client with the credentials instance, and setting a default channel to connect to.
		_client.Initialize(credentials, Username);

		// Bind callbacks to events
		//_client.OnConnected += OnConnected;
		_client.OnJoinedChannel += OnJoinedChannel;
		//_client.OnMessageReceived += OnMessageReceived;
		_client.OnChatCommandReceived += OnChatCommandReceived;     //research this
		//_client.OnWhisperReceived += OnWhisperReceived;
		//_client.OnNewSubscriber += OnNewSubscriber;
		//_client.OnBeingHosted += OnBeingHosted;
		//_client.OnGiftedSubscription += OnGiftedSub;
		//_client.OnModeratorJoined += OnModJoin;
		//_client.OnReSubscriber += OnReSub;
		//_client.OnUserBanned += UserBanned;


		// Connect
		_client.Connect();
	}

	public void AddOption(string optionName) 
	{
		//adding option to options list to keep track of voting count
		Debug.Log(optionName);
		var option = new OptionData(optionName);
		options.Add(options.Count + 1, option);	
	}

	public void NewQuestion(string question) 
	{
		this.Question = question;
    }

	public void Voting(int optionNumber) 
	{
		if(optionNumber != 0 && options != null)
        {
			options[optionNumber].VoteCount += 1;
        }
    }

	public int GetVoteCount(int optionNumber) 
	{ 
		return options[optionNumber].VoteCount;
    }

	public void ClearVoting() {
		options.Clear();
		Question = "";
    }

	private void OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
	{
		//Debug.Log($"The bot {e.BotUsername} just joined the channel: {e.Channel}");
		_client.SendMessage(e.Channel, "I just joined the channel! PogChamp");
	}

	private void OnChatCommandReceived(object sender, TwitchLib.Client.Events.OnChatCommandReceivedArgs e)
	{
		Debug.Log($"Command received from {e.Command.ChatMessage.DisplayName}: {e.Command.ChatMessage.Message}");

		int selectedOption = 0;

		switch (e.Command.CommandText)
        {
			case "vote1":
				selectedOption = 1;
				break;

			case "vote2":
				selectedOption = 2;
				break;

			case "vote3":
				selectedOption = 3;
				break;

			case "vote4":
				selectedOption = 4;
				break;
			case "display":
				
				break;

			case "result":
				SentResultToChat();
				break;
			default:
				selectedOption = 0;
				break;
		};
		

		Voting(selectedOption);
	}


	public void SendPollToChat()
    {
		string message = "New Poll";

		message = $"{message} \n\n--Question: {Question} --\n\n";

		if(options != null)
		{
			foreach (KeyValuePair<int, OptionData> entry in options)
			{
				message += "\n\n";
				message = $"{message} Vote{entry.Key} : {entry.Value.OptionName}";
			}
		}
		Debug.Log(message);
		_client.SendMessage(Username, message);

		InvokeRepeating("SentResultToChat", 10f, 10f);
	}



	public void SentResultToChat()
    {
		string message = "Result";

		message = $"{message} \n\n-- Question: {Question} --\n\n";

		if (options != null)
		{
			foreach (KeyValuePair<int, OptionData> entry in options)
			{
				message += "\n\n";
				message = $"{message} {entry.Value.OptionName} : {entry.Value.VoteCount}";
			}
		}
		Debug.Log(message);
		_client.SendMessage(Username, message);
	}

}
