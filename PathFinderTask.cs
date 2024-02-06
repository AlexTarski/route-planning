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
        int[] tempRoute = new int[checkpoints.Length];
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

    static void MakePermutations(int[] permutation, int position, ref Point[] checkpoints, ref double bestRouteLength, ref int[] route)
    {
        if (position == permutation.Length)
        {
            route = (int[])permutation.Clone();
            bestRouteLength = PointExtensions.GetPathLength(checkpoints, permutation);
            return;
        }

        for (int i = 1; i < permutation.Length; i++)
        {
            var index = Array.IndexOf(permutation, i, 1, position - 1);
            if (index != -1)
                continue;
            if (PointExtensions.GetPathLength(checkpoints, permutation[0 .. position]) + checkpoints[permutation[position - 1]].DistanceTo(checkpoints[i]) < bestRouteLength)
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
}