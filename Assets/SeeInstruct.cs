using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeInstruct : MonoBehaviour
{
    private bool hasInstruct = false;
    public GameObject instructText;

    void Awake()
    {
        // Deactivate pauseScreen initially
        instructText.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void ToggleInstruct()
    {
        hasInstruct = !hasInstruct;

        if (hasInstruct)
            instructText.SetActive(true);
        else
            instructText.SetActive(false);
    }
}
