using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosterController : MonoBehaviour
{
    string posterTag = "Poster";
    string posterCode = "";

    private GameObject currentUI, currentInfo, currentCard, currentPoster;
    private PosterData currentData;
    private ARCameraButtonController cameraController;

    [SerializeField] public PosterData[] layer0, layer1, layer2, layer3;
    //[SerializeField] private Vector3 InfoPos = new Vector3(-0.359f, 0, -1.382f);
    [SerializeField] private GameObject uiPrefab, cardPrefab;
    [SerializeField] private Material posterMat;
    [SerializeField] private Texture CardDefaultTex;

    // Start is called before the first frame update
    void Start()
    {
        cameraController = Camera.main.transform.parent.gameObject.GetComponent<ARCameraButtonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!cameraController.handling)
        {
            UserClickedManager();
        }

        if (currentUI != null && Vector3.Distance(Camera.main.transform.position, currentUI.transform.position) >= 5.0f)
        {
            StartCoroutine(CloseButtonEvent());
        }

        if (currentCard != null && Vector3.Distance(Camera.main.transform.position, currentCard.transform.position) >= 5.0f)
        {
            StartCoroutine(CardClose());
        }
        
    }

    private void UserClickedManager()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                //if (hit.collider.gameObject.Equals(testPoster))
                if (hit.collider.gameObject.CompareTag(posterTag))
                {
                    string posterName = hit.collider.gameObject.name;
                    int startIndex = posterName.LastIndexOf('_') + 1;
                    posterCode = posterName.Substring(startIndex);
                    int.TryParse(posterCode[0].ToString(), out int layerNum);
                    int.TryParse(posterCode.Substring(1), out int posterNum);
                    Debug.Log(string.Format("name : {0}, layer : {1}, num : {2}", posterName, layerNum, posterNum));

                    StartCoroutine(SetPosterUI(layerNum, posterNum, hit.collider.gameObject));
                }
            }
        }
    }

    private IEnumerator SetPosterUI(int layerNum, int posterNum, GameObject poster)
    {

        if (currentUI != null)
        {
            if (!currentPoster.Equals(poster))
            {
                if (currentInfo != null)
                {
                    currentInfo.GetComponent<Animation>().Play("InfoCloseAnim");
                    yield return new WaitForSeconds(0.3f);
                    Destroy(currentInfo);

                    yield return new WaitUntil(() => !currentUI.GetComponent<Animation>().isPlaying);

                    currentUI.GetComponent<Animation>().Play("PosterCloseAnim");

                    yield return new WaitForSeconds(1.3f);

                    Destroy(currentUI);
                }
                else if (currentCard == null)
                {
                    yield return new WaitUntil(() => !currentUI.GetComponent<Animation>().isPlaying);

                    currentUI.GetComponent<Animation>().Play("PosterCloseAnim");

                    yield return new WaitForSeconds(1.3f);

                    Destroy(currentUI);
                }
                else if (currentCard != null)
                {
                    //currentcard close anim
                    currentCard.GetComponent<Animation>().Play("CardCloseAnim");
                    yield return new WaitForSeconds(1.5f);

                    Destroy(currentCard);
                    Destroy(currentUI);
                }
            }
        }

        switch (layerNum)
        {
            case 0:
                currentData = layer0[posterNum];
                break;

            case 1:
                currentData = layer1[posterNum];
                break;

            case 2:
                currentData = layer2[posterNum];
                break;

            case 3:
                currentData = layer3[posterNum];
                break;
        }

        if (currentUI == null || (currentPoster != null && !currentPoster.Equals(poster)))
        {
            currentPoster = poster;
            MakePosterUIObj(currentData);
        }
    }

    private void MakePosterUIObj(PosterData data)
    {
        Vector3 destPos = data.destination.transform.position;

        if (Vector3.Distance(Camera.main.transform.position, destPos) < 5.0f)
        {
            currentUI = Instantiate(uiPrefab);//, Vector3.zero, Quaternion.identity, data.destination.transform.parent.transform);
            currentUI.transform.position = destPos;

            currentUI.transform.SetParent(data.destination.transform.parent.transform);
            GameObject posterObj = currentUI.transform.GetChild(1).gameObject;
            Animation posterAnim = currentUI.GetComponent<Animation>();

            Texture posterImage = currentPoster.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.GetTexture("_MainTex");
            posterMat.SetTexture("_MainTex", posterImage);
            posterObj.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = posterMat;
            posterObj.transform.GetChild(0).gameObject.transform.localScale = data.posterSize;

            posterAnim.Play("PosterClickAnim");
            Vector3 lookTarget = new Vector3(posterObj.transform.position.x - Camera.main.transform.position.x, posterObj.transform.position.y, posterObj.transform.position.z - Camera.main.transform.position.z);
            posterObj.transform.rotation = Quaternion.LookRotation(lookTarget);//posterObj.transform.position - Camera.main.transform.position);

            for (int i = 1; i < posterObj.transform.childCount - 1; i++)
            {
                posterObj.transform.GetChild(i).gameObject.GetComponent<PosterUIController>().SetPosterObject(this.gameObject);
                //Debug.LogFormat("Set this Object in {0}", posterObj.transform.GetChild(i).gameObject.name);
            }
            currentInfo = posterObj.transform.GetChild(4).gameObject;
            currentInfo.SetActive(false);
        }
        else
        {
            //Destroy(currentUI);
        }
    }

    public void ClickedInfoButton()
    {
        if (!cameraController.handling)
        {
            StartCoroutine(InfoButtonEvent());
        }
    }

    private IEnumerator InfoButtonEvent()
    {
        //currentInfo = Instantiate(infoPrefab, currentUI.transform.GetChild(1).gameObject.transform);
        //currentInfo.transform.position = InfoPos;
        if (currentInfo.activeInHierarchy)
        {
            currentInfo.GetComponent<Animation>().Play("InfoCloseAnim");
            yield return new WaitForSeconds(0.3f);
            currentInfo.SetActive(false);
        }
        else
        {
            currentInfo.SetActive(true);
            currentInfo.GetComponent<Animation>().Play("InfoOpenAnim");
            CircleInfoControll infoController = currentInfo.GetComponent<CircleInfoControll>();
            infoController.SetCircleName(currentData.circleName);
            infoController.SetCirclePos(currentData.circle_info.circlePos);
            infoController.SetChairmanName(currentData.circle_info.chairmanName);
            infoController.SetChairmanNum(currentData.circle_info.charimanNum);
        }

        //Vector3 lookTarget = new Vector3(Camera.main.transform.position.x, currentInfo.transform.position.y, Camera.main.transform.position.z);
        //currentInfo.transform.LookAt(lookTarget);
        //currentInfo.transform.rotation = Quaternion.LookRotation(currentInfo.transform.position - Camera.main.transform.position);
    }

    public void ClickedCardButton()
    {
        if (!cameraController.handling)
        {
            StartCoroutine(CardButtonEvent());
        }
    }

    private IEnumerator CardButtonEvent()
    {
        if (currentInfo != null)
        {
            currentInfo.GetComponent<Animation>().Play("InfoCloseAnim");
            yield return new WaitForSeconds(0.3f);
            currentInfo.SetActive(false);
            //Destroy(currentInfo);
        }
        currentUI.GetComponent<Animation>().Play("PosterCloseAnim");

        yield return new WaitForSeconds(1.3f);

        Vector3 destPos = currentData.destination.transform.position;

        if (Vector3.Distance(Camera.main.transform.position, destPos) < 5.0f)
        {
            currentCard = Instantiate(cardPrefab, currentData.destination.transform.parent.transform);
            currentCard.transform.position = destPos;
            currentCard.transform.GetChild(2).gameObject.GetComponent<CardCloseButton>().SetPosterObj(this.gameObject);
            currentCard.GetComponent<Animation>().Play("CardOpenAnim");

            GameObject cardSet = currentCard.transform.GetChild(1).gameObject;

            if (currentData.CardSet.Length > 0)
            {
                for (int i = 0; i < cardSet.transform.childCount; i++)
                {
                    GameObject card = cardSet.transform.GetChild(i).gameObject;

                    if (i < currentData.CardSet.Length)
                    {
                        card.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", currentData.CardSet[i]);
                    }
                    else
                    {
                        card.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", CardDefaultTex);
                    }
                }
            }
            Vector3 lookTarget = new Vector3(Camera.main.transform.position.x, currentCard.transform.position.y, Camera.main.transform.position.z);
            //currentCard.transform.rotation = Quaternion.LookRotation(currentCard.transform.position - Camera.main.transform.position);
            currentCard.transform.LookAt(lookTarget);
        }
    }

    public void ClickedCardCloseButton()
    {
        if (!cameraController.handling)
        {
            StartCoroutine(CardClose());
        }
    }

    private IEnumerator CardClose()
    {
        currentCard.GetComponent<Animation>().Play("CardCloseAnim");
        yield return new WaitForSeconds(1.5f);
        Destroy(currentCard);
        if (currentUI != null)
        {
            currentUI.GetComponent<Animation>().Play("PosterClickAnim");
        }
    }

    public void ClickedCloseButton()
    {
        if (!cameraController.handling)
        {
            StartCoroutine(CloseButtonEvent());
        }
    }

    private IEnumerator CloseButtonEvent()
    {
        if (currentInfo != null)
        {
            currentInfo.GetComponent<Animation>().Play("InfoCloseAnim");
            yield return new WaitForSeconds(0.3f);
            Destroy(currentInfo);
        }

        currentUI.GetComponent<Animation>().Play("PosterCloseAnim");

        yield return new WaitForSeconds(1.3f);

        Destroy(currentUI);
    }
}

[System.Serializable]
public class PosterData// : MonoBehaviour
{
    
    public string circleName;
    public CircleInfo circle_info;
    public Texture[] CardSet;
    public GameObject destination;
    public GameObject posterObj;
    public Vector3 posterSize = new Vector3 (0.4f, 0.8f, 1);
}

[System.Serializable]
public class CircleInfo
{
    public enum CircleType { sports, volunteer, academic, employ, creative, perpoming, religion, culcural, etc }; //스포츠, 봉사, 학술, 취업, 문예창작, 공연예술, 종교, 교양, 그 외
    public CircleType circleType;
    public string circleName = "";
    public string circlePos = "---";
    public string chairmanName = "---";
    public string charimanNum = "---";

    public bool EqualCircleType(CircleType type)
    {
        bool result = false;

        if (circleType == type)
        {
            result = true;
        }

        return result;
    }
}