using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Main : MonoBehaviour
{
    public  List<GameObject> worldObjects;
    public  float gravity = -9.8f;
    public  Vector3 velocity;
    public  bool coll = false;
    private Physics py;

    void Start()
    {
        py = new Physics(worldObjects);
        // py.worldObjects[0].veclocity = new Vector3(0, 1f, 0);
        velocity = new Vector3(0.01f, 0.001f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // py.worldObjects[0].velocity = velocity;
        if (!coll)
        {
            py.worldObjects[0].UpdateValues2(velocity);
        }

        if (py.worldObjects[0].DetectCollision(py.worldObjects[1]) && !coll)
        {
            coll = true;
        }
        if (coll)
        {
            print(Parameters.FirstName);
            py.AfterCollision(py.worldObjects[0], py.worldObjects[1]);
        }

        // if (py.worldObjects[0].DetectCollision(py.worldObjects[1]))
        // {
        //     // py.worldObjects[0].veclocity = new Vector3(0, 0, 0);
        //     // force = Vector3(+0.1f, 0, 0);
        //     py.ApplyObjectForce(new Vector3(-0.1f, 0, 0), py.worldObjects[0]);
        // }
        // else
        // {
        //     py.ApplyObjectForce(new Vector3(0.1f, 0, 0), py.worldObjects[0]);
        // }
        // Debug.Log(py.worldObjects[0].veclocity);
    }
}
