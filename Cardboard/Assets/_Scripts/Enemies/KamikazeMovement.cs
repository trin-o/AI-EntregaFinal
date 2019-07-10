using System.Collections.Generic;
using UnityEngine;
using AI;


public class KamikazeMovement : BaseAgent
{
    public enum KAMI_STATE { LIBRE, CALCULANDO, ESQUIVANDO }
    public KAMI_STATE KamiState;

    PathGenerator currentPathG = null;
    List<int> path;
    int TargetNode;
    float minDistance = 1;
    int pathCounter = 0;
    Vector3 currentTarget;
    SpriteRenderer spr;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        if (GameController.GC.state == GAME_STATE.JUGANDO)
        {
            switch (KamiState)
            {
                case KAMI_STATE.LIBRE:
                    Libre();
                    break;
                case KAMI_STATE.ESQUIVANDO:
                    Esquivando();
                    break;
            }

            CheckNeighbors();

            if (velocity.x > 0) spr.flipX = true;
            else spr.flipX = false;
        }
    }

    void Libre()
    {
        addSeek(GameController.GC.Player.position);
    }

    void SetupEsquivando(PathGenerator pathG)
    {
        currentPathG = pathG;
        int myNearestNode = NodeNearestToTransform(transform);
        TargetNode = NodeNearestToTransform(GameController.GC.Player);
        path = PathFinding.Disjkstra(currentPathG.graph, myNearestNode, TargetNode);

        pathCounter = 0;
        currentTarget = pathG.waypoints[path[pathCounter]];

        KamiState = KAMI_STATE.ESQUIVANDO;
    }


    void Esquivando()
    {
        if (Vector3.Distance(transform.position, currentTarget) < minDistance)
        {

            int newTargetNode = NodeNearestToTransform(GameController.GC.Player);
            if (TargetNode != newTargetNode)
            {
                TargetNode = newTargetNode;
                path = PathFinding.Disjkstra(currentPathG.graph, path[pathCounter], TargetNode);
                pathCounter = 0;
            }

            if (pathCounter + 1 >= path.Count)
            {
                addSeek(GameController.GC.Player.position);
                KamiState = KAMI_STATE.LIBRE;
                return;
            }

            pathCounter++;
            currentTarget = currentPathG.waypoints[path[pathCounter]];
        }
        addSeek(currentTarget);
    }

    void OnEnable()
    {
        GameController.VisibleAgents.Add(transform);
    }

    void CheckNeighbors()
    {
        addSeparate(GameController.VisibleAgents, 2);
    }

    void OnDisable()
    {
        GameController.VisibleAgents.Remove(transform);
    }

    bool PlayerOutOfCluster()
    {
        if (currentPathG)
        {
            if (Vector3.Distance(currentPathG.waypoints[TargetNode], GameController.GC.Player.position) > 4) return true;
            else return false;
        }
        return false;
    }

    int NodeNearestToTransform(Transform t)
    {
        float minD = float.MaxValue;
        int nearestNode = 0;
        for (int i = 0; i < currentPathG.waypoints.Count; i++)
        {
            float d = Vector2.Distance(t.position, currentPathG.waypoints[i]);
            if (d < minD)
            {
                minD = d;
                nearestNode = i;
            }
        }
        return nearestNode;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Cluster")
        {
            if (KamiState == KAMI_STATE.LIBRE && PlayerOutOfCluster() == false)
            {
                SetupEsquivando(other.GetComponent<PathGenerator>());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cluster"))
        {
            KamiState = KAMI_STATE.LIBRE;
        }
    }
}
