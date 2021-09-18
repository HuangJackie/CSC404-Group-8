using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriverAI : MonoBehaviour
{
    public Transform path;
    private float maxSteerAngle = 40f;
    public float torque = 200f;

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    private List<Transform> nodes;
    
    private int currentNode = 0;
    // Start is called before the first frame update
    void Start()
    {
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();
        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Drive();
    }

    private void Drive()
    {
        wheelFL.motorTorque = torque;
        wheelFR.motorTorque = torque;
    }
}
