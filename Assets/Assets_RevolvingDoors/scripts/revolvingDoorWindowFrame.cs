using UnityEngine;
using System.Collections;

public class revolvingDoorWindowFrame : MonoBehaviour
{
    public GameObject[] doors1;
    public GameObject[] doors2;

    public Vector3[]    doors1_OpenForward;
    public Vector3[]    doors1_OpenReverse;

    public Vector3[]    doors2_OpenForward;
    public Vector3[]    doors2_OpenReverse;

    public int  doors1_OpenState;
    public int  doors2_OpenState;
    // 0 = closed
    // 1 = open forward
    // -1 = open reverse

    public float doorSpeed = 10;

	void Start ()
    {
	    doors1_OpenState = doors2_OpenState = 0;
	}
	
	void Update ()
    {
        doors1_OpenState = Mathf.Clamp( doors1_OpenState, -1 , 1 );
        doors2_OpenState = Mathf.Clamp( doors2_OpenState, -1 , 1 );

        if (doors1_OpenState == 0)
        {
            foreach (GameObject door in doors1)
            {
                float yawAngle = Mathf.LerpAngle ( door.transform.localEulerAngles.y, 0.0f, Time.deltaTime * doorSpeed ); 
                door.transform.localEulerAngles = new Vector3( door.transform.localEulerAngles.x, yawAngle, door.transform.localEulerAngles.z );
            }
        }
        else if (doors1_OpenState >= 1)
        {
            for (int i = 0; i < doors1.Length; i++)
            {
                GameObject door = doors1[i];

                float yawAngle = Mathf.LerpAngle ( door.transform.localEulerAngles.y, doors1_OpenForward[i].y, Time.deltaTime * doorSpeed ); 
                door.transform.localEulerAngles = new Vector3( door.transform.localEulerAngles.x, yawAngle, door.transform.localEulerAngles.z );
            }
        }
        else // if (doors1_OpenState <= -1)
        {
            for (int i = 0; i < doors1.Length; i++)
            {
                GameObject door = doors1[i];

                float yawAngle = Mathf.LerpAngle ( door.transform.localEulerAngles.y, doors1_OpenReverse[i].y, Time.deltaTime * doorSpeed ); 
                door.transform.localEulerAngles = new Vector3( door.transform.localEulerAngles.x, yawAngle, door.transform.localEulerAngles.z );
            }
        }

        // Doors on the opposite side of the revolving door
        if (doors2_OpenState == 0)
        {
            foreach (GameObject door in doors2)
            {
                float yawAngle = Mathf.LerpAngle ( door.transform.localEulerAngles.y, 0.0f, Time.deltaTime * doorSpeed ); 
                door.transform.localEulerAngles = new Vector3( door.transform.localEulerAngles.x, yawAngle, door.transform.localEulerAngles.z );
            }
        }
        else if (doors2_OpenState >= 1)
        {
            for (int i = 0; i < doors2.Length; i++)
            {
                GameObject door = doors2[i];

                float yawAngle = Mathf.LerpAngle ( door.transform.localEulerAngles.y, doors2_OpenForward[i].y, Time.deltaTime * doorSpeed ); 
                door.transform.localEulerAngles = new Vector3( door.transform.localEulerAngles.x, yawAngle, door.transform.localEulerAngles.z );
            }
        }
        else // if (doors2_OpenState <= -1)
        {
            for (int i = 0; i < doors2.Length; i++)
            {
                GameObject door = doors2[i];

                float yawAngle = Mathf.LerpAngle ( door.transform.localEulerAngles.y, doors2_OpenReverse[i].y, Time.deltaTime * doorSpeed ); 
                door.transform.localEulerAngles = new Vector3( door.transform.localEulerAngles.x, yawAngle, door.transform.localEulerAngles.z );
            }
        }

	}
}
