using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActive : MonoBehaviour
{

    [SerializeField]
    private GameObject dialogManager;
    [SerializeField]
    private GameObject canvas; 

    private bool mouseOver; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseOver && Input.GetMouseButtonDown(0)) {
            dialogManager.SetActive(true);  
            canvas.SetActive(true);
        } 
        
    }

    void OnMouseEnter() {
        mouseOver = true; 
    }
    void OnMouseExit()
    {
        mouseOver = false; 
    }
}
