using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleInfoControll : MonoBehaviour
{
    private GameObject posterObj;

    [SerializeField] private TextMesh circle_name, circle_pos, chairman_name, chairman_num;
    [SerializeField] private GameObject button_call;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            string callText = "tel://";
            string callNum = ""; 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.Equals(button_call))
            {
                callNum = chairman_num.text;
                callNum = callNum.Replace("-", "");
                callNum = callNum.Replace(" ", "");
                //callNum = callNum.Remove(0, 1);
                callText = callText + callNum;
                Application.OpenURL(callText);
                Debug.Log("Call " + callText);
            }
        }
    }

    public void SetPosterObj(GameObject obj)
    {
        posterObj = obj;
    }

    public void SetCircleName(string text)
    {
        circle_name.text = text;
    }

    public void SetCirclePos(string text)
    {
        circle_pos.text = text;
    }

    public void SetChairmanName(string text)
    {
        chairman_name.text = text;
    }

    public void SetChairmanNum(string text)
    {
        chairman_num.text = text;

        if (text[0] != '0')
        {
            button_call.SetActive(false);
        }
        else
        {
            button_call.SetActive(true);
        }
    }
}
