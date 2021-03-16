using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class revolvingDoor : MonoBehaviour
{
    public List<GameObject>    objectsInTriggerZone;

    public GameObject   door;
    public float        doorSpeed = 10;

    public AudioSource  doorSpinSound;
    public bool         autoRotate = false;
    int     minObjectsToTrigger = 1;

	void Start ()
    {
	
	}
	
	void Update ()
    {
        if ( autoRotate == true )
            minObjectsToTrigger = 0;
        else
             minObjectsToTrigger = 1;

        if (    (Mathf.Abs(doorSpeed) > 0.1f) && (objectsInTriggerZone.Count > minObjectsToTrigger )   )
        {
            if (doorSpinSound.isPlaying == false)
                doorSpinSound.Play();
        }
        else
        {
            if (doorSpinSound.isPlaying )
                doorSpinSound.Stop();
        }

        if (objectsInTriggerZone.Count > minObjectsToTrigger)
    	    door.transform.localEulerAngles = door.transform.localEulerAngles + new Vector3( 0, doorSpeed * Time.deltaTime ,0 );
	}



    private void OnTriggerEnter(Collider other)
    {
        if (objectsInTriggerZone.Contains(other.gameObject) == false)
            objectsInTriggerZone.Add(other.gameObject);
    }


    private void OnTriggerExit(Collider other)
    {
        if ( objectsInTriggerZone.Contains( other.gameObject ) == true )
            objectsInTriggerZone.Remove( other.gameObject );
    }
}
