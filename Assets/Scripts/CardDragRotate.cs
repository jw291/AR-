using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDragRotate : MonoBehaviour
{
    private Vector3 mPrevPose = Vector3.zero;
    private Vector3 mPosDelta = Vector3.zero;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButton(0))
            {
                mPosDelta = Input.mousePosition - mPrevPose;
                if (Vector3.Dot(transform.up, Vector3.up) >= 0)
                {
                    transform.Rotate(transform.up, Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
                }
                else
                {
                    transform.Rotate(transform.up, -Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
                }

                //transform.Rotate(Camera.main.transform.right, Vector3.Dot(mPosDelta, Camera.main.transform.up), Space.World);
            }
            mPrevPose = Input.mousePosition;
        }
    }

}
