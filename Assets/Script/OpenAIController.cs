using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class OpenAIController : MonoBehaviour
{
    public TMP_Text textField;
    public TMP_InputField inputField;
    public Button okButton;
    public TextMeshProUGUI[] option;
    public GameObject canvas;
    public GameObject canvasText;
    public Button[] button; 

    private OpenAIAPI api;
    public List<ChatMessage> messages;
    public bool isWork = false;


    // Start is called before the first frame update
    void Start()
    {
        
        // This line gets your API key (and could be slightly different on Mac/Linux)
        api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User));
        StartConversation();
        okButton.onClick.AddListener(() => GetResponse());


    }

    private void Update()
    {

        okButton.onClick.AddListener(() => ActiveOption(isWork));
       
    }

    public void SelectOption(){ 
    
    }

    void ActiveOption(bool isWork) 
    {
        if (isWork == true) {
            canvas.SetActive(true);
            textField.text = "";
        }

        
    }



    string ExtractStory(string sentence)
    {
        int storyIndex = sentence.IndexOf("Story:");
        string story = sentence.Substring(storyIndex + 6).Trim();

        return story;
    }


    string[] ExtractOptions(string sentence)
    {
        string[] separator = { "Option " };
        string[] options = sentence.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < options.Length; i++)
        {
            int colonIndex = options[i].IndexOf("=");
            options[i] = options[i].Substring(colonIndex + 1).Trim();
        }

        return options;
    }


    private void StartConversation()
    {
        messages = new List<ChatMessage> {
            new ChatMessage(ChatMessageRole.System, "You are present in Kingdom you come to meet your lord and bring your problem in your village" + 
            "i need you to give me story in you village with how problem appear then give title of problem and random short 3 options one is good to use it and two are bad options and you should shuffle option to me and last you must use this format in option you must give only option no anything else " +
             "(Story - {content})" +  
            "(Title: {content})" + 

"Option 1 ({good or bad}) = {content})"
+
"Option 2 ({good or bad}) = {content})"
+
"Option 3 ({good or bad}) = {content})"
+
"after I pick Option you will sent Result of Option that I pick up Then You will only give Answer by this format Result: {content} and End with /End "

            )
        };

        inputField.text = "p";
        string startString = "You met the peasant who came to the palace seeking help from you.";
        textField.text = startString;
        Debug.Log(startString);
    }

    public async void GetResponse()
    {
        if (inputField.text.Length < 1)
        {
            return;
        }

        

        // Disable the OK button
        okButton.enabled = false;

        inputField.text = "What is you problem"; 

        // Fill the user message from the input field
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.Content = inputField.text;
        if (userMessage.Content.Length > 100)
        {
            // Limit messages to 100 characters
            userMessage.Content = userMessage.Content.Substring(0, 1000);
        }
        Debug.Log(string.Format("{0}: {1}", userMessage.rawRole, userMessage.Content));

        // Add the message to the list
        messages.Add(userMessage);

        // Update the text field with the user message
        textField.text = string.Format("You: {0}", userMessage.Content);

        // Clear the input field
        inputField.text = "";

        // Send the entire chat to OpenAI to get the next message
        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.9,
            MaxTokens = 500,
            Messages = messages  
        });

        // Get the response message
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
        Debug.Log(string.Format("This is System {0}: {1}", responseMessage.rawRole, responseMessage.Content));
        
        string[] options = ExtractOptions(responseMessage.Content);
        
        for (int i = 0; i < 3; i++)
        {
            option[i].text = options[i+1];

            Debug.Log(option[i]+ "This cut option" );  
            Debug.Log("Option" + (i + 1) + ": " + options[i]);
        }

        // Add the response to the list of messages
        messages.Add(responseMessage);

        string story = ExtractStory(responseMessage.Content);
        Debug.Log("This is Story" + story);


        // Update the text field with the response
        textField.text = string.Format("You: {0}\n\nGuard: {1}", userMessage.Content, story);

        if (textField.text != null) 
        {
            isWork = true;
        } 

        // Re-enable the OK button
        okButton.enabled = true;
    }
}
