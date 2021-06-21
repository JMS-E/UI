/********************************************************
******* 2018 alexander.de.bruijn@politie.nl   ***********
*********************************************************
a virtual tapemeasure(in unity units)
draw a line between two points, measure the magnitude and the deltas in x y and z and print these values on screen
*/

using UnityEngine;
using System;
using System.Text;
using HTC.UnityPlugin.Vive;
using Sirenix.OdinInspector;


public class Yardstick : MonoBehaviour
{
    public float lineWidth = 0.15f;
    public Color linecolor = Color.red;
    public GameObject measurementPoint;
    public GameObject TextParrentObject;

    private Vector3 pos1;
    private Vector3 pos2;
    //private float distance;
    private float deltaX;
    private float deltaY;
    private float deltaZ;
    private float angleToGround;

    private GameObject textpivot;
    private TextMesh textpivottextmesh;
    private Vector3 midpoint;
    private Vector3[] PointsArray = new Vector3[2];
    private LineRenderer TapeMeasure;
    private Material m_linecolor;
    private StringBuilder sb = new StringBuilder();
    private Shader unlit;

    void Start()
    {
        //create and set the measurement object
        TapeMeasure = gameObject.AddComponent<LineRenderer>() as LineRenderer;

        m_linecolor = TapeMeasure.GetComponent<Renderer>().material;
        unlit = Shader.Find("Universal Render Pipeline/Unlit");
        m_linecolor.shader = unlit;
        m_linecolor.SetColor("_BaseColor", linecolor);

        TapeMeasure.positionCount = 2;
        TapeMeasure.widthMultiplier = lineWidth;

        //create and set the distances text object
        textpivot = new GameObject("txt");
        textpivot.transform.SetParent(TextParrentObject.transform);
        textpivot.AddComponent<TextMesh>();
        textpivottextmesh = textpivot.GetComponent<TextMesh>();
        textpivottextmesh.fontSize = 100;
        textpivottextmesh.color = Color.yellow;
        textpivot.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
    }//eo start

    void FixedUpdate()
    {   if(ViveInput.GetPress(HandRole.RightHand, ControllerButton.DPadDown))
        //if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.DPadDown))
        {
            PointsArray[0] = measurementPoint.transform.position;
        }

        if (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.DPadDown))
        {
            PointsArray[1] = measurementPoint.transform.position;
        }

        // dynamically create line 
        TapeMeasure.SetPositions(PointsArray);

        // position the text object and set its text
        midpoint = (PointsArray[0] + PointsArray[1]) * 0.5f;
        textpivot.transform.position = midpoint;
        textpivottextmesh.text = MeasureDistance().ToString();

        //    //billboard the distances-text towards the camera
        Vector3 v = Camera.main.transform.position - textpivot.transform.position;
        v.x = v.z = 0.0f;
        textpivot.transform.LookAt(Camera.main.transform.position - v);
        textpivot.transform.Rotate(0, 180, 0);
    }//eo update

    string MeasureDistance()
    {
        //calculate the distances
        pos1 = PointsArray[0];
        pos2 = PointsArray[1];
        deltaX = Math.Abs(pos1.x - pos2.x);
        deltaY = Math.Abs(pos1.y - pos2.y);
        deltaZ = Math.Abs(pos1.z - pos2.z);
        Vector3 vectorBetweenP1P2 = pos2 - pos1;
        Vector3 vectorP1P2OnGroundplane = new Vector3(deltaX, 0.0f, deltaZ);
        angleToGround = (Vector3.Angle(vectorBetweenP1P2, vectorP1P2OnGroundplane) - 180) * -1; 

        //build the output string
        sb.Remove(0, sb.Length);
        sb.Append("Distance = ");
        sb.Append(Vector3.Distance(pos1, pos2).ToString("N2")); //round to 2 decimal places
        sb.Append("\n         ΔX = ");
        sb.Append(deltaX.ToString("N2"));
        sb.Append("\n         ΔY = ");
        sb.Append(deltaY.ToString("N2"));
        sb.Append("\n         ΔZ = ");
        sb.Append(deltaZ.ToString("N2"));
        sb.Append("\n         ∠Y = ");
        sb.Append((angleToGround).ToString("N1"));
        sb.Append("° / ");
        sb.Append((180 - angleToGround).ToString("N1"));
        sb.Append("°");

        return sb.ToString();
    }//eo measure

}//eo class