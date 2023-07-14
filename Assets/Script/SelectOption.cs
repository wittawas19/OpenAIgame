using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using Unity.VisualScripting;
using UnityEngine.UIElements;


public class SelectOption : MonoBehaviour
{

    public OpenAIController openAi;
    private OpenAIAPI api;
    private List<ChatMessage> messagesAll; 
    


    // Start is called before the first frame update
    void Start()
    {
        api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User));
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public async void GetResponseFromOption(int numOption)
    {


        ChatMessage userOption = new ChatMessage();
        userOption.Role = ChatMessageRole.User;
        userOption.Content = openAi.option[numOption].text;
       
        openAi.messages.Add(userOption);
        messagesAll = openAi.messages;

        Debug.Log("This is Contemt" + messagesAll);
        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.9,
            MaxTokens = 500,
            Messages = messagesAll
        });

        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;

        string result = ExtractResult(responseMessage.Content);
        Debug.Log(string.Format("This is System {0}: {1}", responseMessage.rawRole, result));

        openAi.textField.text = result;

        openAi.canvas.SetActive(false); 

    }

    string ExtractResult(string text)
    {
        int startIndex = text.IndexOf("Result:") +7 ;
        int endIndex = text.IndexOf("/End");

        string result = text.Substring(startIndex, endIndex - startIndex).Trim();

        return result;
    }


    public void SelectOption1Answer() {

        GetResponseFromOption(0); 
        Debug.Log(openAi.option[0].text + "This is option from another class" );  
        
    }

    public void SelectOption2Answer() 
    {
        GetResponseFromOption(1);
        Debug.Log(openAi.option[1].text + "This is option from another class");
    }

    public void SelectOption3Answer()
    {

        GetResponseFromOption(2);
        Debug.Log(openAi.option[2].text + "This is option from another class");
    }


}
