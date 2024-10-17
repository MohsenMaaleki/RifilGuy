using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tutorial : MonoBehaviour
{
    public Camera cameraToLookAt;
    public GameObject leftGesture;
    public GameObject rightGesture;

    public int clickCount = 0;

    void Start()
    {
        

        
        
    }

    // Update is called once per frame
    void Update()
    {

        
        if(clickCount == 0)
        {
            PauseGame();
            
            leftGesture.SetActive(true);
            rightGesture.SetActive(false);
            if(Input.GetMouseButtonDown(0))
            {
                clickCount++;
            }
            
        }
        else if(clickCount == 1)
        {
            PauseGame();
            leftGesture.SetActive(false);
            rightGesture.SetActive(true);
            if(Input.GetMouseButtonDown(0))
            {
                clickCount++;
            }
        
        }
        else if(clickCount == 2)
        {
            leftGesture.SetActive(false);
            rightGesture.SetActive(false);
            ResumeGame();
        }

        transform.LookAt(cameraToLookAt.transform);
    }


    void PauseGame ()
    {
        Time.timeScale = 0;
    }
    void ResumeGame ()
    {
        Time.timeScale = 1;
    }
}
