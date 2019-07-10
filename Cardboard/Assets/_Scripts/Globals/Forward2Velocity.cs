using UnityEngine;

public class Forward2Velocity : MonoBehaviour
{
    [SerializeField] float rightPower = 5;
    [SerializeField] float smoothSpeed = 4;
    [SerializeField] bool enemy;
    Vector3 prevPos;
    void Start()
    {
        prevPos = transform.position;
    }

    void Update()
    {
        Vector3 velocity = Vector3.zero;

        if (enemy)
           velocity = prevPos - transform.position;
        else
           velocity = transform.position - prevPos;

        velocity += Vector3.right * rightPower;
        transform.right = Vector3.Lerp(transform.right, velocity, Time.deltaTime * smoothSpeed);
        prevPos = transform.position;
    }
}
