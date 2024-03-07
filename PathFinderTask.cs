using Avalonia.Markup.Xaml.Templates;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using static RoutePlanning.PointExtensions;

namespace RoutePlanning;

public static class PathFinderTask
{
	public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
	{
        if(checkpoints.Length > 12)
        {
            List<int> visited = new()
            {
                0
            };
            List<int> unvisited = new();
            for(int i = 1; i < checkpoints.Length; i++)
            {
                unvisited.Add(i);
            }

            GreedyRouteFinder(ref checkpoints, ref visited, unvisited);
            return visited.ToArray();
        }
        else
        {
            int[] tempRoute = new int[checkpoints.Length];
            int[] route = MakeTrivialPermutation(checkpoints.Length);
            double bestRouteLength = PointExtensions.GetPathLength(checkpoints, route);
            MakePermutations(tempRoute, 1, ref checkpoints, ref bestRouteLength, ref route);
            return route;
        }
    }

    private static int[] MakeTrivialPermutation(int size)
    {
        var bestOrder = new int[size];
        for (var i = 0; i < bestOrder.Length; i++)
            bestOrder[i] = i;
        return bestOrder;
    }

    static void MakePermutations(int[] permutation, int position, ref Point[] checkpoints, ref double bestRouteLength, ref int[] route)
    {
        if (position == permutation.Length)
        {
            route = (int[])permutation.Clone();
            bestRouteLength = PointExtensions.GetPathLength(checkpoints, permutation);
            return;
        }

        double currentDistance = PointExtensions.GetPathLength(checkpoints, permutation[0..position]);

        for (int i = 1; i < permutation.Length; i++)
        {
            var index = Array.IndexOf(permutation, i, 1, position - 1);
            if (index != -1)
                continue;
            if (currentDistance + checkpoints[permutation[position - 1]].DistanceTo(checkpoints[i]) < bestRouteLength)
            {
                permutation[position] = i;
                MakePermutations(permutation, position + 1, ref checkpoints, ref bestRouteLength, ref route);
            }
            else
            {
                return;
            }
        }
    }

    static void GreedyRouteFinder(ref Point[] checkpoints, ref List<int> visited, List<int> unvisited)
    {
        if (unvisited.Count == 1)
        {
            visited.Add(unvisited[0]);
            return;
        }

        int indexOfNextPoint = 0;
        for (int i = 1; i < unvisited.Count; i++)
        {
            if (checkpoints[visited[^1]].DistanceTo(checkpoints[unvisited[indexOfNextPoint]]) > checkpoints[visited[^1]].DistanceTo(checkpoints[unvisited[i]]))
            {
                indexOfNextPoint = i;
            }
        }

        visited.Add(unvisited[indexOfNextPoint]);
        unvisited.Remove(unvisited[indexOfNextPoint]);

        GreedyRouteFinder(ref checkpoints, ref visited, unvisited);
    }
}