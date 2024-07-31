using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLMUnity;
using System.Text;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;

public class Visitor : MonoBehaviour
{
    public LLMClient llm;
    public TMPro.TextMeshProUGUI dialogueBox;
    public TMPro.TMP_InputField inputField;
    
    [SerializeField] Canvas _canvas;
    [SerializeField] GameObject _continueButton;
 
    
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

        string finalString = "(You will now rate the following drink I am serving you. Be brutally honest and include a rating from 1 to 10. YOU HAVE TO INCLUDE THE RATING. Write it in the following format X/10. Here is your drink!:" +
                 ingredientsString.ToString();
        
       _ = llm.Chat(finalString, DisplayReply, SpawnContinueButton);
    }

    
    private int ExtractRating(string response)
    {
        Regex regex = new Regex(@"\b(\d+)/10\b");
        Match match = regex.Match(response);
        if (match.Success)
        {
            return int.Parse(match.Groups[1].Value);
        }
        else
        {
            return -1; // Return -1 if extraction fails
        }
    }
    
    public void AddRatingToScore(string ratingResponse)
    {
        int rating = ExtractRating(ratingResponse);
        if (rating >= 0) // Ensure the rating is valid
        {
            ScoreSystem.Instance.score += rating;
            
        }
        else
        {
            Debug.Log("Failed to extract rating from response.");
        }
    }
    
    public void SpawnContinueButton()
    {
        _continueButton.gameObject.SetActive(true);
        string reply = dialogueBox.text;
        AddRatingToScore(reply);    
    }
    public void FinishTalking()
    {
        VisitorManager.Instance.OnVisitorFinishedTalking();
        Destroy(gameObject);
    }
}
    