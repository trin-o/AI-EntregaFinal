using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{

    [SerializeField] bool preview = true;
    [HideInInspector] public Matrix graph = null;
    [SerializeField] float RockRadius = 1.5f;
    public List<Vector3> waypoints;
    public void GenerateGraph()
    {
        waypoints = new List<Vector3>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeInHierarchy)
            {
                Vector3 origin = transform.GetChild(i).position;
                AddNoRepeat(waypoints, origin + Vector3.up * RockRadius * 1.5f);
                AddNoRepeat(waypoints, origin + Vector3.up * RockRadius * -1.5f);
                AddNoRepeat(waypoints, origin + Vector3.right * RockRadius * 1.5f);
                AddNoRepeat(waypoints, origin + Vector3.right * RockRadius * -1.5f);
            }
        }

        graph = new Matrix(waypoints.Count);


        for (int i = 0; i < waypoints.Count; i++)
        {
            Vector3 origin = waypoints[i];
            for (int j = 0; j < waypoints.Count; j++)
            {
                Vector3 target = waypoints[j];
                if (origin != target)
                {
                    bool obstructed = false;
                    for (int k = 0; k < transform.childCount; k++)
                    {
                        Vector2 obstacle = transform.GetChild(k).position;
                        if (Geometry.LineCircleIntersection(origin, target, obstacle, RockRadius))
                        {
                            obstructed = true;
                            break;
                        }
                    }
                    if (!obstructed)
                    {
                        graph.SetCell(i, j, Mathf.RoundToInt(Vector3.Distance(waypoints[i], waypoints[j])));
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!preview) return;
        if (graph != null && graph.data != null && graph.data.Length > 0)
        {
            for (int i = 0; i < graph.rows; i++)
            {
                for (int j = 0; j < graph.rows; j++)
                {
                    if (graph.GetCell(i, j) > 0)
                    {
                        Gizmos.DrawLine(waypoints[i], waypoints[j]);
                    }
                }
            }
        }
    }

    void AddNoRepeat(List<Vector3> list, Vector3 newItem)
    {
        for (int l = 0; l < list.Count; l++)
        {
            if (Vector2.Distance(newItem, list[l]) < 1f)
            {
                return;
            }
        }
        list.Add(newItem);
    }
}
[System.Serializable]
public class Matrix
{
    public int[] data;
    public int rows { get { return (int)Mathf.Sqrt(data.Length); } private set { } }
    public Matrix(int length)
    {
        data = new int[length * length];
    }

    public int GetCell(int x, int y)
    {
        return data[x + y * rows];
    }
    public void SetCell(int x, int y, int value)
    {
        data[x + y * rows] = value;
    }
}
