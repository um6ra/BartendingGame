using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLMUnity;

public class Visitor : MonoBehaviour
{
    public LLMClient llm;
    public TMPro.TextMeshProUGUI dialogueBox;
    public TMPro.TMP_InputField inputField;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            llm.Chat("Hi Welcome To my Bar", DisplayReply);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            llm.Chat("(You will now rate the following drink I am serving you. Be brutally honest and include a rating from 1 to 10. YOU HAVE TO INCLUDE THE RATING. Write it in the following format X/10.) Here is your drink!:" +
                     "[50% Mercury, 20% Melon, 30% Liquid Tar]", DisplayReply);
        }
        
    }
    
    void DisplayReply(string reply)
    {
        dialogueBox.text = reply;
        Debug.Log(reply);
    }
    
}
    