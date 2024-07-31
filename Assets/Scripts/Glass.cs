using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TMPro;

public class Glass : MonoBehaviour
{
    public float maxMilliliters = 200f;  // Maximum capacity of the glass
    public float pourRate = 10f;  // Amount of liquid added per second when pouring
    public Transform liquidTransform;  // Transform of the liquid object
    public Renderer liquidRenderer;  // Renderer of the liquid object
    public Text liquidAmountText;  // UI Text to display the liquid amount (optional)
    public TextMeshProUGUI liquidDetailsText; 
    public Vector3 liquidInitialScale;  // Initial scale of the liquid object
    public Vector3 liquidInitialPosition;  // Initial position of the liquid object

    private Dictionary<string, (float Amount, string Name)> liquidAmounts = new Dictionary<string, (float, string)>();
    private float totalLiquidAmount = 0f;

    private Tuple<float, string> _drinkData = new Tuple<float, string>(0.0f, ""); 

    private void Start()
    {
        // Store the initial scale and position
        liquidInitialScale = liquidTransform.localScale;
        liquidInitialPosition = liquidTransform.localPosition;

        // Update the initial visual state
        UpdateLiquidVisuals();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Liquid"))
        {
            // Get the color of the incoming liquid from the particle system
            ParticleSystem particleSystem = other.GetComponent<ParticleSystem>();
            Color particleColor = particleSystem.main.startColor.color;

            LiquorBottle drink = other.GetComponentInParent<LiquorBottle>();

            // Use the color as a key to track different liquids
            string colorKey = ColorUtility.ToHtmlStringRGBA(particleColor);

            // this color hasn't been poured
            if (!liquidAmounts.ContainsKey(colorKey))
            {
                string ingName = drink.IngredientName;
                liquidAmounts[colorKey] = (0f, ingName);
            }

            // Increase the amount of the corresponding liquid
            liquidAmounts[colorKey] = (liquidAmounts[colorKey].Amount + pourRate * Time.deltaTime, liquidAmounts[colorKey].Name);
            totalLiquidAmount += pourRate * Time.deltaTime;
            totalLiquidAmount = Mathf.Clamp(totalLiquidAmount, 0, maxMilliliters);

            // Update the UI Text if assigned
            if (liquidAmountText != null)
            {
                liquidAmountText.text = "Liquid Amount: " + totalLiquidAmount.ToString("F2");
            }
            
            DisplayLiquidAmounts();
            // Update the visual representation of the liquid
            UpdateLiquidVisuals();
        }
    }

    private void UpdateLiquidVisuals()
    {
        // Calculate the fill amount as a percentage
        float fillAmount = totalLiquidAmount / maxMilliliters;

        // Scale the liquid object based on the fill amount
        liquidTransform.localScale = new Vector3(
            liquidInitialScale.x,
            liquidInitialScale.y * fillAmount,
            liquidInitialScale.z
        );

        // Adjust the position to ensure it scales from the bottom
        liquidTransform.localPosition = new Vector3(
            liquidInitialPosition.x,
            liquidInitialPosition.y + (liquidInitialScale.y * fillAmount) / 2,
            liquidInitialPosition.z
        );

        // Blend the colors of the different liquids
        Color blendedColor = Color.clear;
        foreach (var entry in liquidAmounts)
        {
            Color color;
            ColorUtility.TryParseHtmlString("#" + entry.Key, out color);
            blendedColor += color * (entry.Value.Amount / totalLiquidAmount);
        }

        // Update the color of the liquid
        liquidRenderer.material.color = blendedColor;
    }

    public void SetLiquidColor(Color newColor)
    {
        // This method can be used to set the color of the liquid dynamically if needed
        UpdateLiquidVisuals();
    }

    public string[] GetIngredientsAndAmounts()
    {
        if (liquidAmounts.Count == 0)
        {
            return null;
        }

        string[] strings = new string[liquidAmounts.Count];
        int i = 0;
    
        foreach (var entry in liquidAmounts)
        {
            float roundedAmount = Mathf.Round(entry.Value.Amount);
            strings[i] = entry.Value.Name + ": " + roundedAmount + "ml";
            i++;
        }

        return strings;
    } 
    
    public void ResetGlass()
    {
        // Reset the total liquid amount
        totalLiquidAmount = 0f;

        // Clear the dictionary of liquid amounts
        liquidAmounts.Clear();

        // Reset the UI Text if assigned
        if (liquidAmountText != null)
        {
            liquidAmountText.text = "Liquid Amount: 0.00";
        }
        
        DisplayLiquidAmounts();
        // Reset the visual representation of the liquid
        UpdateLiquidVisuals();
    }
    
    public void DisplayLiquidAmounts()
    {
        if (liquidDetailsText == null)
        {
            return;
        }

        if (liquidAmounts.Count == 0)
        {
            liquidDetailsText.text = "No liquids added.";
            return;
        }

        System.Text.StringBuilder details = new System.Text.StringBuilder();
        details.AppendLine("Drink Details:");

        foreach (var entry in liquidAmounts)
        {
            float roundedAmount = Mathf.Round(entry.Value.Amount);
            details.AppendLine($"{entry.Value.Name}: {roundedAmount} ml");
        }

        liquidDetailsText.text = details.ToString();
    }

}
