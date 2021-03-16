using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalManager : MonoBehaviour
{
  private GameObject MainCamera;

  public GameObject Sponza;

  private Renderer[] ObjectChildren;

  private Material PortalPlaneMaterial;

  private List<Material> Objectmaterial;


    // Start is called before the first frame update
    void Start()
    {
      Objectmaterial = new List<Material>();
      MainCamera = GameObject.FindGameObjectWithTag("ARCamera");
      ObjectChildren = Sponza.GetComponentsInChildren<Renderer>();
      PortalPlaneMaterial = transform.GetComponent<Renderer>().material;

      ObjectMaterialAdd();
    }

    // Update is called once per frame
    void OnTriggerStay(Collider collider)
    {
      Debug.Log("ddd");
      Vector3 camPositionInPortalSpace = transform.InverseTransformPoint(MainCamera.transform.position);

      if (camPositionInPortalSpace.y <= 0.0f)
      {
        StencilNotEqualPortal();
      }
      else if(camPositionInPortalSpace.y < 0.5f)
      {
        StencilAlwaysPortal();
      }
      else
      {
        StencilEqualPortal();
      }

    }

    public void ObjectMaterialAdd(){
      foreach(Renderer rd in ObjectChildren){
        for(int i = 0; i < rd.sharedMaterials.Length; ++i){
          Objectmaterial.Add(rd.sharedMaterials[i]);
        }
      }
    }

    public void StencilNotEqualPortal(){
      for(int i = 0; i < Objectmaterial.Count; i++)
      {
        Objectmaterial[i].SetInt("_StencilComp",(int)CompareFunction.NotEqual);
      }
      Debug.Log("NotEqual");
      PortalPlaneMaterial.SetInt("_CullMode", (int)CullMode.Front);
    }

    //inside
    public void StencilAlwaysPortal(){
      for(int i = 0; i < Objectmaterial.Count; i++)
      {
        Objectmaterial[i].SetInt("_StencilComp",(int)CompareFunction.Always);
      }
      Debug.Log("Always");
      PortalPlaneMaterial.SetInt("_CullMode", (int)CullMode.Off);
    }

    //outside
    public void StencilEqualPortal(){
      for(int i = 0; i < Objectmaterial.Count; i++)
      {
        Objectmaterial[i].SetInt("_StencilComp",(int)CompareFunction.Equal);
      }
      Debug.Log("Equal");
      PortalPlaneMaterial.SetInt("_CullMode", (int)CullMode.Back);
    }


}
