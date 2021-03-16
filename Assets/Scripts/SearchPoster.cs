using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchPoster : MonoBehaviour
{
    private PosterController posterController;
    private GameObject targetPoint, arrow, posterEffect, targetPoster, uiRoot;
    private ARCameraButtonController cameraController;

    [SerializeField] private GameObject[] tagButtonSet;

    [SerializeField] private GameObject tagPanel, circlePanel, makeParent, buttonPrefab, arrowPrefab, posterEffectPrefab;

    void Start()
    {
        cameraController = Camera.main.transform.parent.gameObject.GetComponent<ARCameraButtonController>();
        posterController = this.GetComponent<PosterController>();
        uiRoot = tagPanel.transform.parent.gameObject;
        SetTagButton();

        uiRoot.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !cameraController.handling)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("NPC"))
            {
                GameObject target = hit.collider.gameObject.transform.parent.gameObject;
                StartCoroutine(NPCInteraction(target));
            }
        }

        if (arrow != null && targetPoint != null)
        {
            StartCoroutine(ArrowController());
        }
    }

    private IEnumerator ArrowController()
    {
        if (Vector3.Distance(Camera.main.transform.position, targetPoint.transform.position) < 5)
        {
            arrow.transform.position = Vector3.Lerp(arrow.transform.position, targetPoster.transform.position, 3 * Time.deltaTime);
            yield return new WaitUntil(() => Vector3.Distance(arrow.transform.position, targetPoster.transform.position) < 0.1f);
            Destroy(arrow);
            Destroy(posterEffect);
        }
        else
        {
            arrow.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2;
            arrow.transform.rotation = Quaternion.LookRotation(arrow.transform.position - targetPoint.transform.position);
        }
    }

    private IEnumerator NPCInteraction(GameObject target)
    {
        Quaternion defaultRotate = target.transform.rotation;
        Vector3 lookTarget = new Vector3(Camera.main.transform.position.x, target.transform.position.y, Camera.main.transform.position.z);
        target.transform.LookAt(lookTarget);

        uiRoot.SetActive(true);

        if (!tagPanel.activeInHierarchy)
        {
            if (arrow != null)
            {
                Destroy(arrow);
                Destroy(posterEffect);
            }
            uiRoot.GetComponent<Animation>().Play("TagPanelOpentAnim");
        }

        yield return new WaitForSeconds(3.0f);

        target.transform.rotation = defaultRotate;
    }

    private void SetTagButton()
    {
        for (int i = 0; i < tagButtonSet.Length; i++)
        {
            int temp = i;
            CircleInfo.CircleType type = (CircleInfo.CircleType)i;
            tagButtonSet[i].GetComponent<Button>().onClick.AddListener(() => ClickedTagButton(type));
        }
    }

    public void ClickedTagButton(CircleInfo.CircleType type)
    {
        List<PosterData> resultList;
        resultList = SearchCircle(type, posterController.layer0);
        AddResult(resultList, SearchCircle(type, posterController.layer1));
        AddResult(resultList, SearchCircle(type, posterController.layer2));
        AddResult(resultList, SearchCircle(type, posterController.layer3));

        if (makeParent.transform.childCount > 0)
        {
            for (int i = 0; i < makeParent.transform.childCount; i++)
            {
                Destroy(makeParent.transform.GetChild(i).gameObject);
            }
        }

        foreach (PosterData data in resultList)
        {
            GameObject instance = Instantiate(buttonPrefab, makeParent.transform);
            instance.GetComponent<Button>().onClick.AddListener(() => ClickedCircleButton(data));
            instance.transform.GetChild(0).gameObject.GetComponent<Text>().text = data.circleName;
        }

        uiRoot.GetComponent<Animation>().Play("CirclePanelOpenAnim");
    }

    public void ClickedCircleButton(PosterData data)
    {
        targetPoint = data.destination;
        targetPoster = data.posterObj;
        Debug.Log(targetPoint.name);

        StartCoroutine(CircleButtonEvent());
    }

    private IEnumerator CircleButtonEvent()
    {
        uiRoot.GetComponent<Animation>().Play("SearchStartAnim");

        yield return new WaitUntil(() => !uiRoot.GetComponent<Animation>().isPlaying);

        uiRoot.SetActive(false);
        arrow = Instantiate(arrowPrefab, this.gameObject.transform);
        posterEffect = Instantiate(posterEffectPrefab, targetPoster.transform);
    }

    public void ClickedBackButton()
    {
        uiRoot.GetComponent<Animation>().Play("BackButtonAnim");
    }

    private void AddResult(List<PosterData> result, List<PosterData> target)
    {
        foreach (PosterData data in target)
        {
            bool isTrue = false;

            foreach (PosterData _data in result)
            {
                if (_data.circleName == data.circleName)
                {
                    isTrue = true;
                }
            }

            if (!isTrue)
            {
                result.Add(data);
            }
        }
    }

    private List<PosterData> SearchCircle(CircleInfo.CircleType type, PosterData[] layer)
    {
        List<PosterData> dataList = new List<PosterData>();

        foreach (PosterData data in layer)
        {
            if (data.circle_info.EqualCircleType(type))
            {
                dataList.Add(data);
            }
        }

        return dataList;
    }
}
