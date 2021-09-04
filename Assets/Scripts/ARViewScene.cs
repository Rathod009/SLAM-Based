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
    public GameObject indicator;
    public GameObject sofa;
    private Pose _pose;
    private bool _poseValid = false;
    private bool firstPlace = false;
    private bool firstIndicate = false;

    public Text infoTxt;
    public Image img;
    public Button btnPlace;

    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        sofa.SetActive(false);
        btnPlace.gameObject.SetActive(false);
    }

    void Update()
    {
        updatePose();
        updateIndicator();
    }

    public void placeObject()
    {
        if(_poseValid)
        {
            if (firstPlace)
            {
                infoTxt.gameObject.SetActive(false);
                img.gameObject.SetActive(false);
                firstPlace = false;
            }
            sofa.SetActive(true);
            sofa.transform.position = _pose.position;
            sofa.transform.rotation = _pose.rotation;
        }
        
    }

    private void updateIndicator()
    {
        if(_poseValid)
        {
            if(firstIndicate)
            {
                firstIndicate = false;
                string cntnt = "Plane Detected!\nClick on Place to Augment Sofa";
                infoTxt.text = cntnt;
                firstPlace = true;
            }
            indicator.SetActive(true);
            indicator.transform.SetPositionAndRotation(_pose.position, _pose.rotation);
            btnPlace.gameObject.SetActive(true);
        }
        else
        {
            indicator.SetActive(false);
            btnPlace.gameObject.SetActive(false);
        }
    }
    private void updatePose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        arOrigin.GetComponent<ARRaycastManager>().Raycast(screenCenter, hits, TrackableType.PlaneEstimated);

        _poseValid = hits.Count > 0 ? true : false;

        if(_poseValid)
        {
            TrackableId planeId = hits[0].trackableId;
            ARPlane plane = aRPlaneManager.GetPlane(planeId);
            _pose = hits[0].pose;
            var cameraViewSide = Camera.main.transform.forward.normalized;
            var userFaceSide = new Vector3(cameraViewSide.x * -1, 0f, cameraViewSide.z * -1).normalized;
            _pose.rotation = Quaternion.LookRotation(userFaceSide);
            firstIndicate = true;
        }

    }
}
