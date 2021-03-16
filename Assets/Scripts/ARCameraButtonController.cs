using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ARCameraButtonController : MonoBehaviour
{
    [SerializeField]
    private GameObject uiRoot;

    private GameObject CameraHandle, HandleController, move_front, move_left, move_right, move_back;
    private bool isActive = false;
    private GraphicRaycaster uiRaycaster;

    public bool handling = false;

    void Start()
    {
        CameraHandle = uiRoot.transform.GetChild(1).gameObject;
        HandleController = uiRoot.transform.GetChild(0).gameObject;

        move_front = CameraHandle.transform.GetChild(0).gameObject;
        move_left = CameraHandle.transform.GetChild(1).gameObject;
        move_right = CameraHandle.transform.GetChild(2).gameObject;
        move_back = CameraHandle.transform.GetChild(3).gameObject;

        uiRaycaster = uiRoot.GetComponent<GraphicRaycaster>();
        CameraHandle.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            PointerEventData ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            uiRaycaster.Raycast(ped, results);

            if (results.Count > 0)
            {
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.transform.IsChildOf(CameraHandle.transform))
                    {
                        handling = true;

                        if (result.gameObject.Equals(move_front))
                        {
                            this.transform.Translate(Camera.main.transform.forward * Time.deltaTime, Space.Self);
                        }
                        else if (result.gameObject.Equals(move_left))
                        {
                            this.transform.Translate(-Camera.main.transform.right * Time.deltaTime, Space.Self);
                        }
                        else if (result.gameObject.Equals(move_right))
                        {
                            this.transform.Translate(Camera.main.transform.right * Time.deltaTime, Space.Self);
                        }
                        else if (result.gameObject.Equals(move_back))
                        {
                            this.transform.Translate(-Camera.main.transform.forward * Time.deltaTime, Space.Self);
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        handling = false;
                    }
                }
            }
            else
            {
                handling = false;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            handling = false;
        }
    }

    public void HandleActive()
    {
        if (isActive)
        {
            CameraHandle.SetActive(false);
            isActive = false;
            HandleController.transform.GetChild(0).gameObject.GetComponent<Text>().text = "+ Handle";
        }
        else
        {
            CameraHandle.SetActive(true);
            isActive = true;
            HandleController.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Just walk";
        }
    }
}
