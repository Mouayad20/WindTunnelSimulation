using UnityEngine;

public abstract class AbstractObject : MonoBehaviour
{
    protected GameObject obj;
    protected Vector3 velocity;
    protected Vector3 acceleration;
    protected float mass = 20;


    public abstract bool DetectCollision(AbstractObject anotherObject);


    public void EmptyValues()
    {
        this.acceleration = new Vector3(0, 0, 0);
        this.velocity = new Vector3(0, 0, 0);
    }

    public void UpdateValues(Vector3 force, float dt)
    {
        this.acceleration += force / mass;
        this.velocity += this.acceleration * dt;
        this.obj.transform.position += this.velocity * dt;
    }

    public GameObject GetObj() { return this.obj; }
    public void SetObj(GameObject obj) { this.obj = obj; }

    public Vector3 GetVelocity() { return this.velocity; }
    public void SetVelocity(Vector3 newVelocity) { this.velocity = newVelocity; }

    public Vector3 GetAcceleration() { return this.acceleration; }
    public void SetAcceleration(Vector3 acceleration) { this.acceleration = acceleration; }

    public float GetMass() { return this.mass; }
    public void SetMass(float mass) { this.mass = mass; }
}
