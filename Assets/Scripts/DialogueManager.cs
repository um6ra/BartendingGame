using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLMUnity;

public class DialogueManager : MonoBehaviour 
{
    public LLM llm;
    public TMPro.TMP_InputField input;
    public TMPro.TextMeshProUGUI replyBox;

    void HandleReply(string reply)
    {
        replyBox.text = reply;
    }

    public void OnMessageWritten()
    {
        string msg = input.text;
        _ = llm.Chat(msg, HandleReply);
  }
}

