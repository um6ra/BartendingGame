using UnityEngine;

public class LiquorBottle : MonoBehaviour
{
    public TMPro.TextMeshProUGUI input;
    public ParticleSystem liquidParticleSystem;
    public Transform pourPoint;  // Point from where the liquid pours
    public Vector3 foregroundPosition;
    public Vector3 backgroundPosition;
    public float rotationSpeed = 100f;
    public float pourAngleThreshold = 30f;  // Angle threshold to start pouring
    public float maxRotationAngle = 90f;  // Maximum rotation angle to the right
    [SerializeField] Color startColor;
    [SerializeField] string startName;

    // Drink Ingredient Name
    private string _ingredientName = "pineapple Juice";
    public string IngredientName { get => _ingredientName; }


    private bool isInForeground = false;
    private bool isPouring = false;
    private Quaternion initialRotation;
    private float currentRotationAngle = 0f;
    private Camera mainCamera;

    private void Start()
    {
        // Store the initial rotation
        initialRotation = transform.rotation;
        // Move the bottle to the background position initially
        transform.position  = new Vector3(transform.position.x, transform.position.y, backgroundPosition.z); ;
        // Get the main camera
        mainCamera = Camera.main;

        SetParticleColor(startColor);
        SetNameField(startName);
    }

    private void Update()
    {
        HandlePouring();
        HandleDragging();
    }

    private void OnMouseDown()
    {
        // Move to the foreground position
        transform.position = new Vector3(transform.position.x, transform.position.y, foregroundPosition.z);
        isInForeground = true;
    }

    private void OnMouseUp()
    {
        Vector3 point = CameraToMousePointToBackground.Instance.GetMouseHitPoint();
        transform.position = new Vector3(point.x, point.y, backgroundPosition.z);
        transform.rotation = initialRotation;  // Reset rotation
        isInForeground = false;
        isPouring = false;
        liquidParticleSystem.Stop();
        currentRotationAngle = 0f;  // Reset current rotation angle
    }

    private void HandlePouring()
    {
        if (isInForeground && Input.GetMouseButton(1))
        {
            // Rotate the bottle to the right up to the maximum rotation angle
            float rotationAmount = rotationSpeed * Time.deltaTime;
            if (currentRotationAngle + rotationAmount > maxRotationAngle)
            {
                rotationAmount = maxRotationAngle - currentRotationAngle;
            }
            transform.Rotate(Vector3.back, rotationAmount);
            currentRotationAngle += rotationAmount;

            // Start pouring if the angle exceeds the threshold
            if (currentRotationAngle > pourAngleThreshold)
            {
                isPouring = true;
                if (!liquidParticleSystem.isPlaying)
                {
                    liquidParticleSystem.Play();
                }
            }
        }
        else
        {
            isPouring = false;
            if (liquidParticleSystem.isPlaying)
            {
                liquidParticleSystem.Stop();
            }
        }
    }

    private void HandleDragging()
    {
        if (isInForeground && Input.GetMouseButton(0))
        {
            // Move the bottle with the mouse
            transform.position = GetMouseWorldPos();
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        // Convert mouse position to world position
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }
    
    public void SetParticleColor(Color newColor)
    {
        var mainModule = liquidParticleSystem.main;
        mainModule.startColor = newColor;
        startColor = newColor;
    }
    
    public void SetNameField(string newName)
    {
        _ingredientName = newName;
        input.text = _ingredientName;
        startName = newName;
    }
}
