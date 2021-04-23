/*! @file		: 	VotingSystemController.cs
*   @author		: 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   @date		: 	2021-04-05
*   @brief		: 	This file contains the logic for the Twitch Bot
*/

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;

/*! <summary>
*  This class contains all the logic for the Twitch bot and Polling mechanic.
*  </summary>
*/
public class VotingSystemController : MonoBehaviour
{
    #region Public Members

    public string Question = "";
    public string TwitchChannelName = "";
    public int totalOptions = 0;
    public Dictionary<int, OptionData> options = new Dictionary<int, OptionData>();
    public double hype;

    #endregion Public Members

    #region Private Members

    private Client _client;
    private string Username;
    private string OAuth;
    private List<DateTime> chatHistory;
    private bool active;

    #endregion Private Members

    /*! <summary>
	 *  This function get executed before anything else in this file
	 *  </summary>
	 *  <param name="none"></param>
	 *  <returns>void</returns>
	 */
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
        _client.OnMessageReceived += OnMessageReceived;
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

        chatHistory = new List<DateTime>();

        //updates hype rating every 10 sec
        InvokeRepeating("updateHype", 10f, 10f);
    }

    /*! <summary>
	 *  event callback function that gets called when the bot joins a channel
	 *  </summary>
	 *  <param name="sender">metadata of the event</param>
	 *  <param name="e">event arguments</param>
	 *  <returns>void</returns>
	 */
    private void OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
    {
        Debug.Log($"The bot {e.BotUsername} just joined the channel: {e.Channel}");
        _client.SendMessage(e.Channel, "I just joined the channel! PogChamp");
    }

    /*! <summary>
	 *  event callback function that gets called every time any message gets entered in Twitch chat,
	 *  not including messages sent by the bot
	 *  </summary>
	 *  <param name="sender">metadata of the event</param>
	 *  <param name="e">event arguments</param>
	 */
    private void OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
    {
        //tbh idk if this even works but imma leave it here just incase it does
        if (e.ChatMessage.Message.Contains("fuck"))
        {
            _client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromMinutes(10), "Bad word! 10 minutes timeout!");
        }

        //check if message has desired emote if so add timestamp to list
        if (e.ChatMessage.Message.Contains("PogChamp") ||
            e.ChatMessage.Message.Contains("Mau5") ||
            e.ChatMessage.Message.Contains("Hype") ||
            e.ChatMessage.Message.Contains("Kreygasm") ||
            e.ChatMessage.Message.Contains("Kappa") ||
            e.ChatMessage.Message.Contains("CurseLit"))
        {
            chatHistory.Add(DateTime.Now);
        }
        hype = chatHistory.Count;
    }

    /*! <summary>
	 *  event callback function that gets called when a command is entered into Twitch chat,
	 *  commands are messages where the first character is an "!"
	 *  </summary>
	 *  <param name="sender">metadata of the event</param>
	 *  <param name="e">event arguments</param>
	 */
    private void OnChatCommandReceived(object sender, TwitchLib.Client.Events.OnChatCommandReceivedArgs e)
    {
        Debug.Log($"Command received from {e.Command.ChatMessage.DisplayName}: {e.Command.ChatMessage.Message}");

        switch (e.Command.CommandText)
        {
            case "help":
                _client.SendMessage(Username, "!about will tell you a little bit about myself.		" +
                    "!Vote# will cast a vote if there is a poll running (change # to the number of the option you wish to vote for.		" +
                    "!Results will show you the current poll and how many votes each option has.		" +
                    "!HYPE will show you the current hype rating.		" +
                    "If you have any other questions just type them into chat and one of the members of IMDJ will respond to you");
                break;

            case "about":
                _client.SendMessage(Username, "I am a bot created with TwitchLib! I am here to help immerse twitch " +
                    "viewer into the show! If you need help just type in the chat, one of the members of IMDJ will respond to you");
                break;

            case "Vote1":
                Voting(1);
                break;

            case "Vote2":
                Voting(2);
                break;

            case "Vote3":
                Voting(3);
                break;

            case "Vote4":
                Voting(4);
                break;

            case "Results":
                SentResultToChat();
                break;

            case "HYPE":
                _client.SendMessage(Username, "CURRENT HYPE " + hype.ToString() + "!!  CurseLit PogChamp CurseLit");
                break;

            default:
                _client.SendMessage(Username, "Sorry, I dont know that command, try !help for a list of commands");
                break;
        };
    }

    /*! <summary>
	 *  returns the number of votes counter for the specified option number
	 *  </summary>
	 *  <param name="optionNumber">The index of the option asked for</param>
	 *  <returns>int - the number of votes cast for the specified option</returns>
	 */
    public int GetVoteCount(int optionNumber)
    {
        try
        {
            return options[optionNumber].VoteCount;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

    /*! <summary>
	 *  returns a string of the option for the specified option number
	 *  </summary>
	 *  <param name="optionNumber">the index of the option asked for</param>
	 *  <returns>string - holding the text for the specified option</returns>
	 */
    public string GetOptionText(int optionNumber)
    {
        try
        {
            return options[optionNumber].OptionName;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /*! <summary>
	 *  this function casts the vote for the poll this gets called from this file
	 *  as well as UIManagement.cs when a desktop user casts a vote
	 *  </summary>
	 *  <param name="optionNumber">an int that hold the option that was voted</param>
	 *  <returns>void</returns>
	 */
    public void Voting(int optionNumber)
    {
        if (active)
        {
            if (optionNumber > 0 && optionNumber <= 4 && options != null)
            {
                options[optionNumber].VoteCount += 1;
            }
        }
    }

    /*! <summary>
	 *  this function add a voting option to a dictionary it gets called from PerformerUserUIController.cs
	 *  </summary>
	 *  <param name="optionName">A string containing the text of the option to vote for</param>
	 *  <returns>void</returns>
	 */
    public void AddOption(string optionName)
    {
        //adding option to options list to keep track of voting count
        Debug.Log("Adding Option \"" + optionName + "\" ");
        OptionData option = new OptionData(optionName);
        options.Add(options.Count + 1, option);
        totalOptions = options.Count;
    }

    /*! <summary>
	 *  this function updates the question for the poll it gets called from PerformerUserUIController.cs
	 *  </summary>
	 *  <param name="question">a string containng the text for the new poll question</param>
	 *  <returns>void</returns>
	 */
    public void NewQuestion(string question)
    {
        this.Question = question;
    }

    /*! <summary>
	 *  resets the poll question and clears the option data gets called from PerformerUserUIController.cs
	 *  </summary>
	 *  <param name="none"></param>
	 *  <returns>void</returns>
	 */
    public void ClearVoting()
    {
        Debug.Log("Resetting the Poll");
        options.Clear();
        Question = "";
    }

    /*! <summary>
	 *  this function converts the poll into a string and sends it to Twitch chat when the poll is first created
	 *  it gets called from PerformerUserUIController.cs
	 *  </summary>
	 *  <param name="none"></param>
	 *  <returns>void</returns>
	 */
    public void SendPollToChat()
    {
        if (Question != "")	//prevents blank questions
        {
            string message = "New Poll";

            message = $"{message} \n\n--Question: {Question} --\n\n";

            if (options != null)
            {
                foreach (KeyValuePair<int, OptionData> entry in options)
                {
                    message += "\n\n";
                    message = $"{message} Vote{entry.Key} : {entry.Value.OptionName} \n\n";
                }
            }
            _client.SendMessage(Username, message);

            //cancel old invoke to prevent multiples
            CancelInvoke("SentResultToChat");
            InvokeRepeating("SentResultToChat", 10f, 10f);
            Debug.Log(message);
        }
        else
        {
            Debug.Log("Blank question, poll discarded");
        }
    }

    /*! <summary>
	 *  this function converts the poll question and options as well as how many votes each has to Twitch chat
	 *  this gets called when the command is entered into Twitch chat as well as being called recursivly
	 *  every 10 seconds after a poll is created
	 *  </summary>
	 *  <param name="none"></param>
	 *  <returns>void</returns>
	 */
    public void SentResultToChat()
    {
        if (Question != "") //prevents printing results when empty
        {
            string message = "Result --> ";

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

    /*! <summary>
	 *  this function checks each item in the list chatHistory, if the item is 60 seconds old it get removed,
	 *  it then updates the hype value
	 *  </summary>
	 *  <param name="none"></param>
	 *  <returns>void</returns>
	 */
    public void updateHype()
    {
        DateTime rn = DateTime.Now;
        chatHistory.RemoveAll(DT => (rn - DT).TotalSeconds > 60);
        hype = chatHistory.Count;
    }

    /*! <summary>
	 *  This function is used to set wether or not votes coming from twitch are counted
	 *  </summary>
	 *  <param name="input">a boolean indecating if we want to start or stop vote counting</param>
	 *  <returns>void</returns>
	 */
    public void setActive(bool input)
    {
        active = input;
    }

    /*! <summary>
	 *  This function allows us to stop the repeating function displaying voting results in chat from other C# files
	 *  </summary>
	 *  <param name="none"></param>
	 *  <returns>void</returns>
	 */
    public void cancelInvoke()
    {
        CancelInvoke("SentResultToChat");
    }

    /*! <summary>
	 *  this function sends a message to twtich chat that the poll has ended
	 *  </summary>
	 *  <param name="none"></param>
	 *  <returns>void</returns>
	 */
    public void endPoll()
    {
        string message = "The Poll has ended!";
        _client.SendMessage(Username, message);
    }
}
