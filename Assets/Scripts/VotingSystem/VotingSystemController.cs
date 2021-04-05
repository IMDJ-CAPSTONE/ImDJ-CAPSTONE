using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.IO;
using System;
using TwitchLib.Client.Extensions;

public class VotingSystemController : MonoBehaviour
{
    #region Public Members
    public string Question = "";
	public string TwitchChannelName = "";
	public int totalOptions = 0;
	public Dictionary<int, OptionData> options = new Dictionary<int, OptionData>();
	#endregion


	#region Private Members
	private Client _client;
	private string Username;
	private string OAuth;
	private List<string> chatHistory;
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
		Debug.Log("Parsed JSON secrets successfully");
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
		_client.OnChatCommandReceived += OnChatCommandReceived;
		//_client.OnWhisperReceived += OnWhisperReceived;
		//_client.OnNewSubscriber += OnNewSubscriber;
		//_client.OnBeingHosted += OnBeingHosted;
		//_client.OnGiftedSubscription += OnGiftedSub;
		//_client.OnModeratorJoined += OnModJoin;
		//_client.OnReSubscriber += OnReSub;
		//_client.OnUserBanned += UserBanned;

		Debug.Log("Attempting to connect");
		// Connect
		_client.Connect();
	}

	public void AddOption(string optionName) 
	{
		//adding option to options list to keep track of voting count
		Debug.Log("Adding Option \""+optionName + "\" ");
		var option = new OptionData(optionName);
		options.Add(options.Count + 1, option);
		totalOptions = options.Count;
	}

	public void NewQuestion(string question) 
	{
		this.Question = question;
    }

	public void Voting(int optionNumber) 
	{
		if(optionNumber > 0 && optionNumber <= 4 && options != null)
        {
			options[optionNumber].VoteCount += 1;
        }
    }

	public int GetVoteCount(int optionNumber) 
	{ 
		return options[optionNumber].VoteCount;
    }

	public void ClearVoting() {
		Debug.Log("Resetting the Poll");
		options.Clear();
		Question = "";
    }

	private void OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
	{
		Debug.Log($"The bot {e.BotUsername} just joined the channel: {e.Channel}");
		_client.SendMessage(e.Channel, "I just joined the channel! PogChamp");
	}

	//tbh idk if this even works but imma leave it here just incase it does
	private void Client_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
	{
		if (e.ChatMessage.Message.Contains("fuck"))
		{
			_client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromSeconds(10), "Bad word! 10 minutes timeout!");
		}


		//the line in the IF will search through the List chatHistory and find all items that match the string "Kappa"
		List<string> emoteCount;
		double percent;
		//parse out the emote
		//if (e.Command.ChatMessage.Message.Contains("LUL"))
		//{
		//	emoteCount = chatHistory.FindAll(delegate (string s) { return s == "LUL"; });
		//	percent = (double)emoteCount.Count / (double)chatHistory.Count;
		//	_client.SendMessage(e.Command.ChatMessage.Channel, $"LUL % = {percent:F2}");
		//}
		//else if (e.Command.ChatMessage.Message.Contains("<3"))
		//{
		//	emoteCount = chatHistory.FindAll(delegate (string s) { return s == "<3"; });
		//	percent = (double)emoteCount.Count / (double)chatHistory.Count;
		//	_client.SendMessage(e.Command.ChatMessage.Channel, $"<3 % = {percent:F2}");
		//}
		//else if (e.Command.ChatMessage.Message.Contains("Kappa"))
		//{
		//	emoteCount = chatHistory.FindAll(delegate (string s) { return s == "Kappa"; });
		//	percent = (double)emoteCount.Count / (double)chatHistory.Count;
		//	_client.SendMessage(e.Command.ChatMessage.Channel, $"Kappa % = {percent:F2}");
		//}
		//else if (e.Command.ChatMessage.Message.Contains("PogChamp"))
		//{
		//	emoteCount = chatHistory.FindAll(delegate (string s) { return s == "PogChamp"; });
		//	percent = (double)emoteCount.Count / (double)chatHistory.Count;
		//	_client.SendMessage(e.Command.ChatMessage.Channel, $"PogChamp % = {percent:F2}");
		//}
		//else






	}

	private void OnChatCommandReceived(object sender, TwitchLib.Client.Events.OnChatCommandReceivedArgs e)
	{
		Debug.Log($"Command received from {e.Command.ChatMessage.DisplayName}: {e.Command.ChatMessage.Message}");
		int selectedOption = 0;

		switch (e.Command.CommandText)
        {
			case "help":
				_client.SendMessage(Username, "I was created to help immerse twitch viewer into the show! " +
					"If you need help just type in the chat, one of the members of IMDJ will respond to you");
				break;
			case "Vote1":
				selectedOption = 1;
				break;

			case "Vote2":
				selectedOption = 2;
				break;

			case "Vote3":
				selectedOption = 3;
				break;

			case "Vote4":
				selectedOption = 4;
				break;
			case "Display":
				
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
		if(Question != "")	//prevents blank questions
        {
			string message = "New Poll";

			message = $"{message} \n\n--Question: {Question} --\n\n";

			if (options != null)
			{
				foreach (KeyValuePair<int, OptionData> entry in options)
				{
					message += "\n\n";
					message = $"{message} Vote{entry.Key} : {entry.Value.OptionName}";
				}
			}
			_client.SendMessage(Username, message);

			//cancel old invoke to prevent multiples
			CancelInvoke();
			InvokeRepeating("SentResultToChat", 10f, 10f);
			Debug.Log(message);
		}
        else
        {
			Debug.Log("Blank question, poll discarded");
        }
		
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