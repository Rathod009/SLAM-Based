using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMove : MonoBehaviour
{

    Touch touch;
    float speedFactor;


    // Start is called before the first frame update
    void Start()
    {
        speedFactor = 0.001f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Moved)
            {
                transform.position = new Vector3(
                    transform.position.x + touch.deltaPosition.x * speedFactor,
                    transform.position.y,
                    transform.position.z + touch.deltaPosition.y * speedFactor);
            }

        }
    }
}
