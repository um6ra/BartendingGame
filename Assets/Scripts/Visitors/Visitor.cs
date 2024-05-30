using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLMUnity;

public class Visitor : MonoBehaviour
{
    public LLMClient llm;
    public TMPro.TextMeshProUGUI dialogueBox;
    public TMPro.TMP_InputField inputField;
    
    [SerializeField] Canvas _canvas;
    
    // Start is called before the first frame update
    void Start()
    {
        llm.Warmup();
    }

    // Update is called once per frame
    void Update()
    {
        
     
        
    }
    
    void DisplayReply(string reply)
    {
        dialogueBox.text = reply;
        Debug.Log(reply);
    }
    
    public void StartConversation()
    {
       // _canvas = GetComponentInChildren<Canvas>();
        _canvas.gameObject.SetActive(true);
        llm.Chat("Hi Welcome To my Bar", DisplayReply);
    }

    public void SpeakAboutDrink()
    {
        llm.Chat("(You will now rate the following drink I am serving you. Be brutally honest and include a rating from 1 to 10. YOU HAVE TO INCLUDE THE RATING. Write it in the following format X/10.) Here is your drink!:" +
                 "[50% Mercury, 20% Melon, 30% Liquid Tar]", DisplayReply, SpawnContinueButton);
    }

    public void SpawnContinueButton()
    {
        
    }
    public void FinishTalking()
    {
        VisitorManager.Instance.OnVisitorFinishedTalking();
        Destroy(gameObject);
    }
}
    