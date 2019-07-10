using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 min;
    Vector3 max;

    public Texture2D Map;

    [SerializeField] float xDisplacement = 0.025f;

    void Awake()
    {
        min = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height)) + Vector3.down;
        max = new Vector3(Map.width, min.y) - min;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(xDisplacement, 0.0f);

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, min.x, max.x),
            Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector2 size = new Vector2(Map.width, Map.height);
        Gizmos.DrawWireCube(((Vector3)size - new Vector3(0.0f, size.y, 0.0f)) / 2, size);
    }
}
