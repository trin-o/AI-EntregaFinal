using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float RailChangeSpeed = 0.5f;
    [SerializeField] AnimationCurve RailChangeCurve;

    private float h, w;
    private Vector3 prevMouse;
    private Camera m_camera;

    Coroutine changeZ;

    void Start()
    {
        m_camera = Camera.main;

        h = m_camera.fieldOfView / 12.0f; // result => 5.0f
        w = h * (16f / 9f);
    }

    void Update()
    {
        if (GameController.GC.state == GAME_STATE.JUGANDO)
        {
            transform.position = Move();
            ShiftRail();
        }
    }

    Vector3 Move()
    {
        // mouse
        Vector3 mousePos = -Vector3.one;

        // movemente

        if (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            if (prevMouse == Vector3.zero) prevMouse = m_camera.transform.InverseTransformPoint(m_camera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10)));
            mousePos = m_camera.transform.InverseTransformPoint(m_camera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10)));
            transform.position += mousePos - prevMouse;
            prevMouse = mousePos;
        }
        else if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            prevMouse = Vector3.zero;


        // limit to camera

        float min = -w + m_camera.transform.position.x;
        float max = w + m_camera.transform.position.x;

        return new Vector3(Mathf.Clamp(transform.position.x, min, max), Mathf.Clamp(transform.position.y, -h, h), transform.position.z);
    }

    void ShiftRail()
    {
        if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).tapCount == 2) && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (transform.parent.position.z == -10) StartChangeZ(-5);
            else if (transform.parent.position.z == -5) StartChangeZ(-10);
        }
    }

    void StartChangeZ(float newZ)
    {
        if (changeZ == null)
            changeZ = StartCoroutine(ChangeZ(newZ));
    }

    IEnumerator ChangeZ(float newZ)
    {
        float iniZ = transform.parent.position.z;
        float timer = 0;
        float TimeToMove = Mathf.Abs(iniZ - newZ) / RailChangeSpeed;
        if (TimeToMove != 0)
        {
            while (timer <= TimeToMove)
            {
                timer += Time.deltaTime;
                float percent = timer / TimeToMove;
                transform.parent.position = new Vector3(
                    transform.parent.position.x,
                    transform.parent.position.y,
                    Mathf.Lerp(iniZ, newZ, RailChangeCurve.Evaluate(percent)));
                yield return null;
            }
        }
        transform.parent.position = new Vector3(
            transform.parent.position.x,
            transform.parent.position.y,
            newZ);
        changeZ = null;
    }


}
