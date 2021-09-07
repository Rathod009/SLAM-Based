using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARViewScene : MonoBehaviour
{

    public ARPlaneManager aRPlaneManager;
    public ARSessionOrigin arOrigin;
    public GameObject sofa;
    private bool firstPlace;
    public Text infoTxt;
    public Image img;
    public Button btnReset;

    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        sofa.SetActive(false);
        btnReset.gameObject.SetActive(false);
        firstPlace = false;

        if(!SceneControl.firstTime)
        {
            img.gameObject.SetActive(false);
            infoTxt.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(SceneControl.firstTime && aRPlaneManager.trackables.count > 0)
        {
            string cntnt = "Plane Detected!\nClick on Place to Augment Sofa";
            infoTxt.text = cntnt;
        }


        if(firstPlace)
        {
            foreach (var plane in aRPlaneManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
        }

        if (!firstPlace && Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            
            if(arOrigin.GetComponent<ARRaycastManager>().Raycast(ray,hits))
            {
                Pose p = hits[0].pose;
                sofa.SetActive(true);
                sofa.transform.position = p.position;
                var cameraViewSide = Camera.main.transform.forward.normalized;
                var userFaceSide = new Vector3(cameraViewSide.x * -1, 0f, cameraViewSide.z * -1).normalized;
                sofa.transform.rotation = Quaternion.LookRotation(userFaceSide);
            }
            firstPlace = true;

            if(SceneControl.firstTime)
            {
                SceneControl.firstTime = false;
                img.gameObject.SetActive(false);
                infoTxt.gameObject.SetActive(false);
            }

            btnReset.gameObject.SetActive(true);
        }

        if (firstPlace && Input.touchCount > 0)
        {
            Debug.Log("IN");
            if (Input.GetTouch(0).tapCount == 3)
            {
                activeARRotate();
            }
            else if (Input.GetTouch(0).tapCount == 2)
            {
                activeARScale();
            }
        }
    }


    public void ResetAll()
    {
        sofa.SetActive(false);
        firstPlace = false;
        foreach (var plane in aRPlaneManager.trackables)
        {
            plane.gameObject.SetActive(true);
        }

    }


    public void activeARDragMove()
    {
        sofa.GetComponent<DragMove>().enabled = true;
        sofa.GetComponent<LeanPinchScale>().enabled = false;
        sofa.GetComponent<LeanTwistRotateAxis>().enabled = false;
    }
    public void activeARScale()
    {
        sofa.GetComponent<LeanPinchScale>().enabled = true;
        sofa.GetComponent<LeanTwistRotateAxis>().enabled = false;
        sofa.GetComponent<DragMove>().enabled = false;
    }
    public void activeARRotate()
    {
        sofa.GetComponent<LeanPinchScale>().enabled = false;
        sofa.GetComponent<LeanTwistRotateAxis>().enabled = true;
        sofa.GetComponent<DragMove>().enabled = false;
    }

}
