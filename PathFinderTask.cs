using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace RoutePlanning;

public static class PathFinderTask
{
	public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
	{
        int[] tempRoute = new int[checkpoints.Length];
        double currentDistance = 0;
        int[] route = MakeTrivialPermutation(checkpoints.Length);
        double bestRouteLength = PointExtensions.GetPathLength(checkpoints, route);
        MakePermutations(tempRoute, 1, ref checkpoints, ref bestRouteLength, ref route);
        return route;
	}

    private static int[] MakeTrivialPermutation(int size)
    {
        var bestOrder = new int[size];
        for (var i = 0; i < bestOrder.Length; i++)
            bestOrder[i] = i;
        return bestOrder;
    }

    static void Evaluate(int[] permutation, ref Point[] checkpoints, ref double bestRouteLength, ref int[] route)
    {
        double distance = PointExtensions.GetPathLength(checkpoints, permutation);
        if (distance < bestRouteLength)
        {
            route = (int[])permutation.Clone();
            bestRouteLength = distance;
        }
    }

    static void MakePermutations(int[] permutation, int position, ref Point[] checkpoints, ref double bestRouteLength, ref int[] route)
    {
        if (position == permutation.Length)
        {
            Evaluate(permutation, ref checkpoints, ref bestRouteLength, ref route);
            return;
        }

        for (int i = 1; i < permutation.Length; i++)
        {
            var index = Array.IndexOf(permutation, i, 1, position - 1);
            if (index != -1)
                continue;
            permutation[position] = i;
            MakePermutations(permutation, position + 1, ref checkpoints, ref bestRouteLength, ref route);
        }
    }
}