using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Glass : MonoBehaviour
{
    public float maxCapacity = 100f;  // Maximum capacity of the glass
    public float pourRate = 10f;  // Amount of liquid added per second when pouring
    public Transform liquidTransform;  // Transform of the liquid object
    public Renderer liquidRenderer;  // Renderer of the liquid object
    public Text liquidAmountText;  // UI Text to display the liquid amount (optional)
    public Vector3 liquidInitialScale;  // Initial scale of the liquid object
    public Vector3 liquidInitialPosition;  // Initial position of the liquid object

    private Dictionary<string, float> liquidAmounts = new Dictionary<string, float>();
    private float totalLiquidAmount = 0f;

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

            // Use the color as a key to track different liquids
            string colorKey = ColorUtility.ToHtmlStringRGBA(particleColor);
            if (!liquidAmounts.ContainsKey(colorKey))
            {
                liquidAmounts[colorKey] = 0f;
            }

            // Increase the amount of the corresponding liquid
            liquidAmounts[colorKey] += pourRate * Time.deltaTime;
            totalLiquidAmount += pourRate * Time.deltaTime;
            totalLiquidAmount = Mathf.Clamp(totalLiquidAmount, 0, maxCapacity);

            // Update the UI Text if assigned
            if (liquidAmountText != null)
            {
                liquidAmountText.text = "Liquid Amount: " + totalLiquidAmount.ToString("F2");
            }

            // Update the visual representation of the liquid
            UpdateLiquidVisuals();
        }
    }

    private void UpdateLiquidVisuals()
    {
        // Calculate the fill amount as a percentage
        float fillAmount = totalLiquidAmount / maxCapacity;

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
            blendedColor += color * (entry.Value / totalLiquidAmount);
        }

        // Update the color of the liquid
        liquidRenderer.material.color = blendedColor;
    }

    public void SetLiquidColor(Color newColor)
    {
        // This method can be used to set the color of the liquid dynamically if needed
        UpdateLiquidVisuals();
    }
}
