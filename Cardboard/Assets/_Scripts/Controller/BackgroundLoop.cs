using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    [SerializeField] GameObject[] level;

    Camera m_cam;
    Vector2 m_bounds;

    public float choke;
        
    void Start()
    {
        m_cam = gameObject.GetComponent<Camera>();
        m_bounds = m_cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, m_cam.transform.position.z));

        foreach (GameObject obj in level)
        {
            LoadChildObj(obj);
        }
    }

    void LoadChildObj(GameObject obj)
    {
        float objWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x - choke;
        int childsNeeded = (int)Mathf.Ceil(m_bounds.x * 2 / objWidth);

        Debug.Log(childsNeeded);

        GameObject temp = Instantiate(obj) as GameObject;

        for (int i = 0; i <= childsNeeded; i++)
        {
            GameObject t = Instantiate(temp) as GameObject;
            t.transform.SetParent(obj.transform);
            t.transform.position = new Vector3(objWidth * i, obj.transform.position.y, obj.transform.position.z);
            t.name = obj.name + i;
        }

        Destroy(temp);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }

    void RepositionChildObj(GameObject obj)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();

        if (children.Length > 1)
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;

            float halfObjWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x - choke;

            if (transform.position.x + m_bounds.x + 10> lastChild.transform.position.x + halfObjWidth)
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfObjWidth * 2, lastChild.transform.position.y, lastChild.transform.position.z);
            }
            else if (transform.position.x - m_bounds.x - 10 < firstChild.transform.position.x - halfObjWidth)
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfObjWidth * 2, firstChild.transform.position.y, firstChild.transform.position.z);
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach (GameObject obj in level)
        {
            RepositionChildObj(obj);
        }
    }
}
