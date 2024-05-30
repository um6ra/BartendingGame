using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLMUnity;
using System.Text;

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

    /// <summary>
    /// Retrieves the ingredients from the current glass and feeds them as a prompt to the visitor LLM
    /// </summary>
    public void SpeakAboutDrink(Glass drink)
    {
        string[] ingredients = drink.GetIngredientsAndAmounts();

        if (ingredients == null) return;

        // Constructing the ingredients string
        StringBuilder ingredientsString = new StringBuilder();
        for (int i = 0; i < ingredients.Length; i++)
        {
            if (i > 0)
            {
                ingredientsString.Append(", ");
            }
            ingredientsString.Append(ingredients[i]);
        }

        string finalString = "(You will now rate the following drink I am serving you. Be brutally honest and include a rating from 1 to 10. YOU HAVE TO INCLUDE THE RATING. Write it in the following format X/10.) Here is your drink!:" +
                 ingredientsString.ToString();

       _ = llm.Chat(finalString, DisplayReply, SpawnContinueButton);
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
    