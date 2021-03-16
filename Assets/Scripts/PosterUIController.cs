using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosterUIController : MonoBehaviour
{
    private enum buttonType { Info, Close, Card };

    [SerializeField]
    private buttonType buttonT;

    private GameObject posterObj;
    private bool count = false;

    void Start()
    {
        this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.Equals(this.gameObject))
            {
                if (count)
                {
                    this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    count = false;
                }
                else
                {
                    this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    count = true;
                }

                switch (buttonT)
                {
                    case buttonType.Info:
                        posterObj.GetComponent<PosterController>().ClickedInfoButton();
                        break;

                    case buttonType.Card:
                        posterObj.GetComponent<PosterController>().ClickedCardButton();
                        break;

                    case buttonType.Close:
                        posterObj.GetComponent<PosterController>().ClickedCloseButton();
                        break;
                }
            }
        }
    }

    public void SetPosterObject(GameObject obj)
    {
        posterObj = obj;
    }
}
