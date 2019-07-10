using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] float velocityToRotation = 25.0f;
    float zAngle = 0;

    void Update()
    {
        zAngle += Time.deltaTime * velocityToRotation;

        if (zAngle > 360.0f)
        {
            zAngle = 0.0f;
        }

        //Rotation
        transform.localRotation = Quaternion.Euler(0, 0, zAngle);
    }

    void OnEnable()
    {
        GameController.VisibleAgents.Add(transform);
    }

    void OnDisable()
    {
        GameController.VisibleAgents.Remove(transform);
    }
}
