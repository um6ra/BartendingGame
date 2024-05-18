using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLMUnity;
using System;

public class DialogueManager : MonoBehaviour 
{
    public LLM llm;
    public TMPro.TMP_InputField input;
    public TMPro.TextMeshProUGUI replyBox;

    private string _reply;

    public string OBjectName;
    public Color ObjectColor;

    void HandleReply(string reply)
    {
        replyBox.text = reply;
        _reply = reply;
    }

    void CompleteReply()
    {
        // Split the input string into lines
        string[] lines = _reply.Split('\n');

        // Loop through each line
        foreach (string line in lines)
        {
            // Split the line into object name and RGB values
            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Ensure the line contains object name and RGB values
            if (parts.Length >= 4)
            {
                string objectName = parts[0];

                // Retrieve RGB values from string parts
                int r, g, b;
                if (int.TryParse(RemoveNonDigits(parts[1]), out r) &&
                    int.TryParse(RemoveNonDigits(parts[2]), out g) &&
                    int.TryParse(RemoveNonDigits(parts[3]), out b))
                {
                    // Create Color object from RGB values
                    Color color = new Color(r / 255f, g / 255f, b / 255f);
                
                    OBjectName = objectName;
                    ObjectColor = color;
                
                }
                else
                {
                    Debug.LogWarning("Invalid RGB values for object: " + objectName);
                }
            }
            else
            {
                Debug.LogWarning("Invalid input format: " + line);
            }
        }
        }

    public void OnMessageWritten()
    {
        string msg = input.text;
        _ = llm.Chat(msg, HandleReply, CompleteReply);
  }

    private string RemoveNonDigits(string input)
    {
        return new string(Array.FindAll(input.ToCharArray(), char.IsDigit));
    }
}