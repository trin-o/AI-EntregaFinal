using UnityEngine;
using AI;

public class Misile : Bullet
{
    MisileAgent agent;
    public bool InPosition = false;
    public Misile(int id, GameObject prefab, Transform parent = null) : base(id, prefab, parent)
    {
        agent = obj.GetComponent<MisileAgent>();
        agent.velocity = Vector3.right * 10;
    }

    public override void Spawn(Transform origin, float speed)
    {
        base.Spawn(origin, speed);
        InPosition = false;
    }

    public void GetInPosition(Vector3 pos)
    {
        if (agent)
        {
            agent.addSeek(pos);
            if (!InPosition)
            {
                if (Vector3.Distance(obj.transform.position, pos) < 0.1f)
                    InPosition = true;
                obj.transform.right = -agent.desiredVector;
            }
        }
    }

    public void Aim(Vector3 pos)
    {
        pos.z = obj.transform.position.z;
        obj.transform.right =
        Vector3.Lerp(
            obj.transform.right,
            obj.transform.position - pos,
            Time.deltaTime);
    }

}

public class MisileAgent : BaseAgent
{


}

