using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScuffedBottleChanger : MonoBehaviour
{
    // Start is called before the first frame update
    
    public DialogueManager dialogueManager;
    public LiquorBottle liquorBottle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeColorAndName()
    {
        liquorBottle.SetParticleColor(dialogueManager.ObjectColor);
        liquorBottle.SetNameField(dialogueManager.OBjectName);
    }
}
