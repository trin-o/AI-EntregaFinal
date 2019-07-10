using UnityEngine;
using AI;

enum E_STATE { FOLLOW, OUT_RANGE }

public class Enemy : BaseAgent
{
    Transform player;
    Vector3 position;

    E_STATE state;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null)
        {
            state = E_STATE.FOLLOW;
        }

        position = transform.position;
    }

    void Update()
    {
        switch (state)
        {
            case E_STATE.FOLLOW:
                if (transform.position.x > player.position.x)
                {
                    addSeek(player.position);
                    transform.position = new Vector3(position.x, transform.position.y, position.z);
                }
                else
                {
                    position = transform.position;
                    state = E_STATE.OUT_RANGE;
                }
                break;
            case E_STATE.OUT_RANGE:
                if (transform.position.x < player.position.x)
                {
                    transform.position = new Vector3(position.x, transform.position.y, position.z);
                }
                else
                {
                    position = transform.position;
                    state = E_STATE.FOLLOW;
                }
                break;
            default:
                break;
        }
        
    }
}
    
