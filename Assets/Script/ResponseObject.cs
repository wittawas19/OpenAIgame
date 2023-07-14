using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResponseObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myText;
    private int choiceValue;
    // Start is called before the first frame update


    public void Setup(string newDialog, int myChoice)
    {
        myText.text = newDialog; 
        choiceValue = myChoice;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
