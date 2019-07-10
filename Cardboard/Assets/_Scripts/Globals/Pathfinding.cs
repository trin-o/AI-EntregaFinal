using UnityEngine;
using System.Collections.Generic;

public class PathFinding
{
    static public List<int> Disjkstra(Matrix graph, int start, int end)
    {
        List<int> path = new List<int>();
        if (start == end)
        {
            path.Add(start);
            return path;
        }

        // Inicializa valores

        int N = graph.rows;

        int[] distances = new int[N];
        int[] procedences = new int[N];
        bool[] blacklist = new bool[N];

        for (int i = 0; i < N; i++)
        {
            distances[i] = int.MaxValue;
            procedences[i] = -1;
            blacklist[i] = false;
        }

        distances[start] = 0;

        // Se repite esto N veces

        for (int count = 0; count < N - 1; count++)
        {
            int minIndex = MinDistance(distances, blacklist);
            blacklist[minIndex] = true;

            for (int neighborIndex = 0; neighborIndex < N; neighborIndex++)
            {
                if (!blacklist[neighborIndex] &&
                    graph.GetCell(minIndex, neighborIndex) != 0 &&
                    distances[minIndex] + graph.GetCell(minIndex, neighborIndex) < distances[neighborIndex])
                {
                    distances[neighborIndex] = distances[minIndex] + graph.GetCell(minIndex, neighborIndex);
                    procedences[neighborIndex] = minIndex;
                }
            }
        }

        int pre = -1;
        int current = end;
        path.Add(current);

        while (pre != start)
        {
            pre = procedences[current];
            path.Add(pre);
            current = pre;
        }

        path.Reverse();

        // PrintSolution(path);
        return path;
    }

    static private int MinDistance(int[] distances, bool[] blacklist)
    {
        int min = int.MaxValue;
        int minIndex = 0;

        for (int v = 0; v < blacklist.Length; v++)
        {
            if (blacklist[v] == false && distances[v] <= min)
            {
                min = distances[v];
                minIndex = v;
            }
        }

        return minIndex;
    }

    static private void PrintSolution(List<int> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            Debug.Log(path[i]);
        }
    }
}
