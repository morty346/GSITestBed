using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineMeasure : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    LineRenderer lineRend;
    [SerializeField]
    LineRenderer lineRendFiducial;
    [SerializeField]
    LineRenderer lineRendSegment;

    Vector3 StartPos;
    Vector3 EndPos;
    float distance;
    float FiducialDistance;
    float SegmentDistance;
    
    [SerializeField]
    Text distanceText;
    [SerializeField]
    Text FiducialPoint;
    [SerializeField]
    Text SegmentTotal;
    Camera cam;



    enum LineType
    {
        FiducialPoint = 0,
        SegmentPoint = 1,
        LinePoint = 2
    }

    private LineType currentType = LineType.FiducialPoint;
    private int maxPoint = 5;
    void Start()
    {
        curPoint = 0;
        
        lineRend.positionCount = 2;
        lineRendFiducial.positionCount = 2;
      
        cam = Camera.main;
    }

    public void setModeSegment()
    {
        currentType = LineType.SegmentPoint;
    }
    public void setModeFid()
    {
        currentType = LineType.FiducialPoint;
    }
    public void segModeLine()
    {
        currentType = LineType.LinePoint;
    }

    private int curPoint = 0;
    public void ResetSeg()
    {
        curPoint = 0;
    }
    public string text;
    private Vector3[] positions;

    private void OnGUI()
    {
        
        if (positions != null)
        {
            if (positions.Length > 0)
            {
                for (int i = 0; i < positions.Length-1; i++)
                {
                    var pos = Camera.main.WorldToScreenPoint(positions[i]);
                   
                    text = positions[i].ToString();
                    var textSize = GUI.skin.label.CalcSize(new GUIContent(text));
                    GUI.contentColor = Color.red;
                    GUI.Label(new Rect(pos.x, Screen.height - pos.y, textSize.x, textSize.y), text);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            ResetSeg();
        
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, 1000));

            if (Physics.Raycast(ray, out hit))
            {
                StartPos = hit.point;
            }

            if (currentType == LineType.SegmentPoint)
            {
                if (curPoint >= maxPoint)
                    curPoint = 0;
                lineRendSegment.positionCount = curPoint+1;
                lineRendSegment.SetPosition(curPoint, StartPos);
                Debug.Log(curPoint);
                curPoint++;
                float alldistance = 0;
                positions = new Vector3[lineRendSegment.positionCount];
                lineRendSegment.GetPositions(positions);

                Vector3 lastPoint = positions[0];
                for (int i = 1; i < positions.Length; ++i)
                {
                    alldistance += (positions[i] - lastPoint).magnitude * 10 * 10;
                    lastPoint = positions[i];
                }
               
                
                
                SegmentDistance = alldistance;
                SegmentTotal.text = SegmentDistance.ToString("F2") + "mm";;
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
                switch (currentType)
                {
                    case LineType.FiducialPoint:
                        lineRendFiducial.SetPosition(0, new Vector3(0, 0, 0));
                        lineRendFiducial.SetPosition(1, StartPos);
                        FiducialDistance = (Vector3.zero - StartPos).magnitude * 10 * 10;
                        FiducialPoint.text = FiducialDistance.ToString("F2") + "mm";
                        break;
                   
                    case LineType.LinePoint:
                        lineRend.SetPosition(0, StartPos);
                        lineRend.SetPosition(1, EndPos);
                        distance = (EndPos - StartPos).magnitude * 10 * 10;
                        distanceText.text = distance.ToString("F2") + "mm";
                        break;
                }
            }
        }
    }
}
