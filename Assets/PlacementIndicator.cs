using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation; //ar foundation library
using UnityEngine.XR.ARSubsystems; //ar foundation library

public class PlacementIndicator : MonoBehaviour
{

    private ARRaycastManager rayManager; // tracking variable
    private GameObject visual; // tracking variable
    public GameObject Go;
    void Start()
    {
        // get the components
        rayManager = FindObjectOfType<ARRaycastManager>();
        visual = transform.GetChild(0).gameObject;

        // hide the placement indicator visual
        visual.SetActive(false);
    }

    void Update()
    {
        UpdatePlacementPose();
        if(Input.GetTouch(0).phase == TouchPhase.Began)//Input.GetKeyDown(KeyCode.A))//Input.GetTouch(0).phase == TouchPhase.Began)
        {
          SpawnObject();
        }
    }

    void UpdatePlacementPose(){

          // shoot a raycast from the center of the screen
          List<ARRaycastHit> hits = new List<ARRaycastHit>();
          rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

          // if we hit an AR plane surface, update the position and rotation
          if (hits.Count > 0)
          {
              transform.position = hits[0].pose.position;
              transform.rotation = hits[0].pose.rotation;

              // enable the visual if it's disabled
              if (!visual.activeInHierarchy)
                  visual.SetActive(true);
          }
    }

    void SpawnObject(){
      GameObject InGo = Instantiate(Go,transform.position,transform.rotation);
      InGo.transform.Rotate(0f,-180f,0f);
    }
}
