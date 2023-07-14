using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BranchDialog : MonoBehaviour
{

    [SerializeField]
    private string[] sentence1;

    [SerializeField]
    private string[] sentence2;

    public GameObject[] answer;

    public TextMeshProUGUI dialogText;

    private int index;

    private bool canContinue;

    private int option; 
    // Start is called before the first frame update
    void Start()
    {
       
        for (int i = 0; i < answer.Length; i++) 
        {

            Debug.Log(answer[i]); 
            answer[i].SetActive(true);
        }
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (canContinue && Input.GetMouseButtonDown(0)) 
        {
            index++;
            if (option == 1) { 
                dialogText.text = sentence1[index]; 
            }
        }
    }

    public void DialogOption1() {
        
        option = 1;
        for (int i = 0; i < answer.Length; i++)
        {
            answer[i].SetActive(true);
        }
        canContinue = true;
        dialogText.gameObject.SetActive(true);
        dialogText.text = (sentence1[index]);
        index++;

    }

    public void DialogOption2()
    {
        for (int i = 0; i < answer.Length; i++)
        {
            answer[i].SetActive(true);
        }
        canContinue = true; 
        dialogText.gameObject.SetActive(true);
        dialogText.text = (sentence2[index]);
        index++;
    }

}
