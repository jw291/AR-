using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour
{
    public GlobalFlock myFlock;
    public GameObject fishPrefab;

    static int numFish= 7;
    public GameObject[] allFish = new GameObject[numFish];
    public Vector3 goalPos;

    public Vector3 swimLimits = new Vector3(17.77f,0.96f,17.43f);

    public void FishSpeed(float speedMult)
    {
      Debug.Log(speedMult);
      for(int i = 0; i < numFish; i++)
      {
        allFish[i].GetComponent<Flock>().speedMult = speedMult;
      }
    }

    void OnDrawGizmosSelected()
    {
      Gizmos.color = new Color(1,0,0,0.5F);
      Gizmos.matrix = this.transform.localToWorldMatrix;
      Gizmos.DrawCube(Vector3.zero, new Vector3(swimLimits.x*2, swimLimits.y*2, swimLimits.z*2));
      Gizmos.color = new Color(0,1,0,1);
      Gizmos.DrawSphere(goalPos, 0.1f);
    }

    void Awake()
    {
      myFlock = this;
      goalPos = this.transform.position;
      for(int i = 0; i < numFish; i++)
      {
        Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x,swimLimits.x),
                                                            Random.Range(-swimLimits.y,swimLimits.y),
                                                            Random.Range(-swimLimits.z,swimLimits.z));
        allFish[i] = (GameObject) Instantiate(fishPrefab, pos, Quaternion.identity);
        allFish[i].transform.parent = GameObject.Find("Drone_Manager").transform;
        allFish[i].GetComponent<Flock>().myManager = this;

      }

      FishSpeed(0.5f);
    }

    void Update()
    {
      if(Random.Range(0,10000) < 50 )
      {
        goalPos = this.transform.position + new Vector3(Random.Range(-swimLimits.x,swimLimits.x),
                                                        Random.Range(-swimLimits.y,swimLimits.y),
                                                        Random.Range(-swimLimits.z,swimLimits.z));
      }
    }
}
