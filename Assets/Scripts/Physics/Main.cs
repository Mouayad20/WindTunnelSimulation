using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Main : MonoBehaviour
{
    public List<GameObject> worldObjects;
    public float gravity = 9.8f;

    public float dt = 0.01f;
    public Vector3 velocity;
    private Physics py;
    bool coll = false;
    void Start()
    {
        py = new Physics(worldObjects)
        {
            dt = dt
        };

    }

    // Update is called once per frame
    void Update()
    {
        ///////////////// example of applying force ///////////////// 

        // py.ApplyForce(new Vector3(1, 0, 0), py.worldObjects[0]);
        // py.ApplyForce(new Vector3(0, -0.5f, 0), py.worldObjects[1]);

        ///////////////// example of applying velocity ///////////////// 

        // py.ApplyForce(new Vector3(0, -0.5f, 0), py.worldObjects[1]);

        ///////////////// example of applying gravity /////////////////

        // py.ApplyGravity(gravity);

        ///////////////// detecte sphare collision /////////////////
        if (!coll)
        {
            py.ApplyVelocity(new Vector3(1, 0, 0), py.worldObjects[0], inverse: false);
        }
        else
        {

            py.ApplyVelocity(new Vector3(1, 0, 0), py.worldObjects[0], inverse: true);
        }
        Debug.Log(py.worldObjects[0].HandleCollision(py.worldObjects[1]));
        if (py.worldObjects[0].HandleCollision(py.worldObjects[1]))
        {
            coll = true;
            // py.worldObjects[0].EmptyValues();
        }

    }
}
