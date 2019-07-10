using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class EnemyMovement : BaseAgent
{
    Vector3 target;

    int index = -1;

    Transform player;

    [SerializeField] bool yoyo = true;
    [SerializeField] Transform paths;
    List<Transform> path = new List<Transform>();
    Vector3 fixedPosition;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        fixedPosition = paths.position;

        for (int i = 0; i < paths.childCount; i++)
        {
            path.Add(paths.GetChild(i));
        }

        NextPath();
    }

    // Update is called once per frame
    void Update()
    {
        addSeek(target);

        if (yoyo)
        {
            paths.position = fixedPosition;


            if (Vector3.Distance(transform.position, target) < 1.0f)
            {
                NextPath();
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, player.position) < 20.0f)
            {
                target = new Vector3(transform.position.x, player.position.y, transform.position.z);
            }
        }
    }

    void NextPath()
    {
        index++;

        if (index == path.Count)
        {
            index = 0;
        }

        target = path[index].position;
    }
}
