using System.Collections.Generic;
using UnityEngine;

public class VisitorManager : MonoBehaviour
{
    public static VisitorManager Instance { get; private set; }
    public List<GameObject> visitorPrefabs;

    public GameObject serveDrinkButton;

    private GameObject currentVisitor;
    private GameObject nextVisitor;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        currentVisitor = SpawnVisitor(true);
        nextVisitor = SpawnVisitor(false);
    }

    public void OnVisitorFinishedTalking()
    {
        StartVisitorConversation(nextVisitor);
        currentVisitor = nextVisitor;
        nextVisitor = SpawnVisitor(false);
    }
    
    public void OnDrinkServed(Glass drink)
    {
        Visitor visitorScript = currentVisitor.GetComponent<Visitor>();
        if (visitorScript != null)
        {
            visitorScript.SpeakAboutDrink(drink);
        }
        else
        {
            Debug.LogWarning("No Visitor script found on current Visitor.");
        }
    }

    private GameObject SpawnVisitor(bool startConversation)
    {
        if (visitorPrefabs.Count > 0)
        {
            int index = Random.Range(0, visitorPrefabs.Count);
            GameObject visitorPrefab = visitorPrefabs[index];
            GameObject visitor = Instantiate(visitorPrefab, visitorPrefab.transform.position, visitorPrefab.transform.rotation);
            if (startConversation)
            {
                StartVisitorConversation(visitor);
            }
            return visitor;
        }
        else
        {
            Debug.LogWarning("No visitor prefabs assigned to VisitorManager.");
            return null;
        }
    }

    private void StartVisitorConversation(GameObject visitor)
    {
        Visitor visitorScript = visitor.GetComponent<Visitor>();
        if (visitorScript != null)
        {
            visitorScript.StartConversation();
        }
        else
        {
            Debug.LogWarning("No Visitor script found on Visitor prefab.");
        }
    }
}
