using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineMeasure : MonoBehaviour
{
    // Start is called before the first frame update
    LineRenderer lineRend;
    Vector3 StartPos;
    Vector3 EndPos;
    float distance;
    float distance2;
    [SerializeField]
    Text distanceText;
    [SerializeField]
    Text distanceText2;

    Camera cam;
    bool fidPoint = false;



    void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        lineRend.positionCount = 2;
        cam = Camera.main;
    }

    public void ToggleMode()
    {
        fidPoint = !fidPoint;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, 1000));

            if (Physics.Raycast(ray, out hit))
            {
                StartPos = hit.point;
            }

        }


        if(Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(new Vector3(Input.mousePosition.x, 
                Input.mousePosition.y, 1000));

            if (Physics.Raycast(ray, out hit))
            {
                
                EndPos = hit.point;
                if(fidPoint)
                {
                    lineRend.SetPosition(0, new Vector3(0, 0, 0));
                    lineRend.SetPosition(1, StartPos);
                }
                else
                {
                    lineRend.SetPosition(0, StartPos);
                    lineRend.SetPosition(1, EndPos);
                }

                if(fidPoint)
                {
                    distance2 = (Vector3.zero - StartPos).magnitude * 10 * 10;
                    distanceText2.text = distance2.ToString("F2") + "mm";
                }
                else
                {
                    distance = (EndPos - StartPos).magnitude * 10 * 10;
                    distanceText.text = distance.ToString("F2") + "mm";
                }

                
            }


        }
    }
}
