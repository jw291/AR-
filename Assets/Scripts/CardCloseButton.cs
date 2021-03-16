using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCloseButton : MonoBehaviour
{
    private GameObject posterObj;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.Equals(this.gameObject))
            {
                posterObj.GetComponent<PosterController>().ClickedCardCloseButton();
            }
        }
    }

    public void SetPosterObj(GameObject obj)
    {
        posterObj = obj;
    }
}
