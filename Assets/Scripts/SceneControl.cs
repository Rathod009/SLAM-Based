using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public GameObject sofa;
    public static bool firstTime = true;
    public static int sceneC = 0;

    public void ChangeScene(string nxtScene)
    {
        SceneManager.LoadScene(nxtScene);
        if (nxtScene == "FirstScreen")
            sceneC = 1;
        else
            sceneC = 0;

        Debug.Log(sceneC);
    }
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Debug.Log("IN");
            if (Input.GetTouch(0).tapCount == 3)
            {
                if (sceneC == 1)
                    activeRotate();
                Debug.Log("IN 3");
            }
            if (Input.GetTouch(0).tapCount == 2)
            {
                if (sceneC == 1)
                    activeScale();
                Debug.Log("IN 2");
            }
        }
        
    }

    public void activeScale()
    {
        Debug.Log("Scale");
        sofa.GetComponent<LeanPinchScale>().enabled = true;
        sofa.GetComponent<LeanTwistRotateAxis>().enabled = false;
    }
    public void activeRotate()
    {
        Debug.Log("Rotate");
        
        sofa.GetComponent<LeanPinchScale>().enabled = false;
        sofa.GetComponent<LeanTwistRotateAxis>().enabled = true;
    }
    
}
