using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LeetcodeDailies;

public class Matrix
{
    public int MaximalRectangle(char[][] matrix)
    {
        var max = 0;
        var history = new int[matrix[0].Length];
        Array.Fill(history, 0);
        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[0].Length; j++)
                history[j] = matrix[i][j] == '1' ? history[j] + 1 : 0;
            max = Math.Max(max, LargestRectangleArea(history));
        }
        return max;
    }

    private int LargestRectangleArea(int[] history)
    {
        var stack = new Stack<int>();
        var maxArea = 0;
        for (int i = 0; i <= history.Length; i++)
        {
            var currentHeight = i == history.Length ? 0 : history[i];
            while (stack.Count > 0 && currentHeight < history[stack.Peek()])
            {
                var height = history[stack.Pop()];
                var width = stack.Count == 0 ? i : i - stack.Peek() - 1;
                maxArea = Math.Max(maxArea, height * width);
            }
            stack.Push(i);
        }
        return maxArea;
    }

    /// <summary>
    /// 463. Island Perimeter
    /// https://leetcode.com/problems/island-perimeter
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    public int IslandPerimeter(int[][] grid)
    {
        var count = 0;
        for (var i = 0; i < grid.Length; i++)
            for (var j = 0; j < grid[0].Length; j++)
                if (grid[i][j] == 1) count += CalculatePerimeter(i, j, grid);

        return count;
    }

    private int CalculatePerimeter(int i, int j, int[][] grid)
    {
        var count = 4;
        if (i - 1 >= 0 && grid[i - 1][j] == 1) count--;
        if (i + 1 < grid.Length && grid[i + 1][j] == 1) count--;
        if (j - 1 >= 0 && grid[i][j - 1] == 1) count--;
        if (j + 1 < grid[0].Length && grid[i][j + 1] == 1) count--;
        return count;
    }

    /// <summary>
    /// 200. Number of Islands
    /// https://leetcode.com/problems/number-of-islands
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    public int NumIslands(char[][] grid)
    {
        var count = 0;
        for (var i = 0; i < grid.Length; i++)
            for (var j = 0; j < grid[0].Length; j++)
                if (grid[i][j] == '1')
                {
                    MapIsland(i, j, grid);
                    count++;
                }

        return count;
    }

    private void MapIsland(int i, int j, char[][] grid)
    {
        grid[i][j] = '2';
        if (i - 1 >= 0 && grid[i - 1][j] == '1') MapIsland(i - 1, j, grid);
        if (i + 1 < grid.Length && grid[i + 1][j] == '1') MapIsland(i + 1, j, grid);
        if (j - 1 >= 0 && grid[i][j - 1] == '1') MapIsland(i, j - 1, grid);
        if (j + 1 < grid[0].Length && grid[i][j + 1] == '1') MapIsland(i, j + 1, grid);
    }

    /// <summary>
    /// 1992. Find All Groups of Farmland
    /// https://leetcode.com/problems/find-all-groups-of-farmland
    /// </summary>
    /// <param name="land"></param>
    /// <returns></returns>
    public int[][] FindFarmland(int[][] land)
    {
        var result = new List<int[]>();
        for (var i = 0; i < land.Length; i++)
            for (var j = 0; j < land[i].Length; j++)
                if (land[i][j] == 1) result.Add(GridSize(i, j, land));
        return result.ToArray();
    }

    private int[] GridSize(int i, int j, int[][] land)
    {
        int endI = i, endJ = j;
        int currI = i, currJ = j;
        while (currI < land.Length && land[currI][currJ] == 1)
        {
            endI = currI;
            while (currJ < land[endI].Length && land[currI][currJ] == 1)
            {
                endJ = currJ;
                land[currI][currJ] = 2;
                currJ++;
            }
            currI++;
            currJ = j;
        }

        return [i, j, endI, endJ];
    }

    /// <summary>
    /// 2373. Largest Local Values in a Matrix
    /// https://leetcode.com/problems/largest-local-values-in-a-matrix
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    public int[][] LargestLocal(int[][] grid)
    {
        var n = grid.Length - 2;
        var maxLocal = new int[n][];
        for (var i = 0; i < n; i++)
        {
            maxLocal[i] = new int[n];
            for (var j = 0; j < n; j++)
            {
                int col = i + 1, row = j + 1;
                maxLocal[i][j] = CalcLargestLocal(row, col, grid);
            }
        }
        return maxLocal;
    }

    private int CalcLargestLocal(int row, int colomn, int[][] grid)
    {
        var max = 0;
        var n = grid.Length - 2;
        for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
                max = Math.Max(max, grid[colomn + i - 1][row + j - 1]);
        return max;
    }


    /// <summary>
    /// 861. Score After Flipping Matrix
    /// https://leetcode.com/problems/score-after-flipping-matrix
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    public int MatrixScore(int[][] grid)
    {
        var n = grid.Length;
        var m = grid[0].Length;
        var res = (1 << (m - 1)) * n;

        for (int j = 1; j < m; j++)
        {
            var val = 1 << (m - 1 - j);
            var set = 0;

            for (int i = 0; i < n; i++)
                if (grid[i][j] == grid[i][0]) set++;

            res += Math.Max(set, n - set) * val;
        }

        return res;
    }

    /// <summary>
    /// 1219. Path with Maximum Gold
    /// https://leetcode.com/problems/path-with-maximum-gold
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    public int GetMaximumGold(int[][] grid)
    {
        var max = 0;
        var n = grid.Length;
        var m = grid[0].Length;
        for (var i = 0; i < n; i++)
            for (var j = 0; j < m; j++)
                if (grid[i][j] != 0) max = Math.Max(max, GetMaximumGold(i, j, grid, n, m));
        return max;
    }

    private int GetMaximumGold(int i, int j, int[][] grid, int n, int m)
    {
        if (i < 0 || i >= n || j < 0 || j >= m || grid[i][j] == 0) return 0;

        var aux = grid[i][j];
        var sum = aux;
        grid[i][j] = 0;

        sum = Math.Max(sum, aux + GetMaximumGold(i - 1, j, grid, n, m));
        sum = Math.Max(sum, aux + GetMaximumGold(i, j - 1, grid, n, m));
        sum = Math.Max(sum, aux + GetMaximumGold(i + 1, j, grid, n, m));
        sum = Math.Max(sum, aux + GetMaximumGold(i, j + 1, grid, n, m));

        grid[i][j] = aux;
        return sum;
    }

    private int[] row = { 0, 0, -1, 1 };
    private int[] col = { -1, 1, 0, 0 };
    /// <summary>
    /// 2812. Find the Safest Path in a Grid
    /// https://leetcode.com/problems/find-the-safest-path-in-a-grid
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    public int MaximumSafenessFactor(IList<IList<int>> grid)
    {
        var n = grid.Count - 1;
        var m = grid[0].Count - 1;

        if (grid[0][0] == 1 || grid[n][m] == 1) return 0;

        var queue = new Queue<int[]>();
        for (var i = 0; i <= n; i++)
            for (var j = 0; j <= m; j++)
                if (grid[i][j] == 1)
                {
                    grid[i][j] = 0;
                    queue.Enqueue([i, j]);
                }
                else grid[i][j] = int.MaxValue;

        while (queue.Count > 0)
        {
            var t = queue.Dequeue();
            int x = t[0], y = t[1];
            var s = grid[x][y];

            for (var i = 0; i < 4; i++)
            {
                int newX = x + row[i];
                int newY = y + col[i];

                if (newX < 0 || newX > n || newY < 0 || newY > m || grid[newX][newY] <= 1 + s)
                    continue;
                grid[newX][newY] = 1 + s;
                queue.Enqueue([newX, newY]);
            }
        }

        var vis = new HashSet<string>();
        var pq = new PriorityQueue<int[], int>(Comparer<int>.Create((a, b)=> b - a));
        pq.Enqueue([0, 0], grid[0][0]);

        while (pq.Count > 0)
        {
            pq.TryDequeue(out int[] temp, out int safe);
            int i = temp[0], j = temp[1];

            if (i == n && j == n) return safe;
            vis.Add($"{i},{j}");

            for (int k = 0; k < 4; k++)
            {
                int newX = i + row[k];
                int newY = j + col[k];

                if (newX < 0 || newX > n || newY < 0 || newY > n || vis.Contains($"{newX},{newY}"))
                    continue;
                pq.Enqueue([newX, newY], Math.Min(safe, grid[newX][newY]));
                vis.Add($"{newX},{newY}");
            }
        }

        return -1;
    }
}
