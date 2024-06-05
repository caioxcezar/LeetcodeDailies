namespace LeetcodeDailies;

public class Graphs
{
    /// <summary>
    /// 1971. Find if Path Exists in Graph
    /// https://leetcode.com/problems/find-if-path-exists-in-graph
    /// </summary>
    /// <param name="n">vertices</param>
    /// <param name="edges"></param>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public bool ValidPath(int n, int[][] edges, int source, int destination)
    {
        if (n == 1 || source == destination) return true;

        var graph = new Dictionary<int, ICollection<int>>();
        foreach (var edge in edges)
        {
            AddToGraph(graph, edge[0], edge[1]);
            AddToGraph(graph, edge[1], edge[0]);
        }

        return DFS(source, destination, graph, new());
    }

    private void AddToGraph(IDictionary<int, ICollection<int>> graph, int key, int value)
    {
        if (!graph.TryAdd(key, new List<int>() { value })) graph[key].Add(value);
    }

    private bool DFS(int source, int destination, IDictionary<int, ICollection<int>> graph, HashSet<int> visited)
    {
        if (visited.Contains(source)) return false;
        if (graph[source].Contains(destination)) return true;

        visited.Add(source);

        foreach (int neighbor in graph[source])
            if (DFS(neighbor, destination, graph, visited)) return true;

        return false;
    }

    /// <summary>
    /// 310. Minimum Height Trees
    /// https://leetcode.com/problems/minimum-height-trees
    /// </summary>
    /// <param name="n"></param>
    /// <param name="edges"></param>
    /// <returns></returns>
    public IList<int> FindMinHeightTrees(int n, int[][] edges)
    {
        if (n == 1) return [0];

        var graph = new Dictionary<int, HashSet<int>>();
        foreach (var edge in edges)
        {
            AddToGraph(graph, edge[0], edge[1]);
            AddToGraph(graph, edge[1], edge[0]);
        }

        var leaves = new Queue<int>();
        foreach (var node in graph.Keys)
        {
            if (graph[node].Count == 1)
            {
                leaves.Enqueue(node);
            }
        }

        while (n > 2)
        {
            var leavesCount = leaves.Count;
            n -= leavesCount;

            for (var i = 0; i < leavesCount; i++)
            {
                var leaf = leaves.Dequeue();
                var neighbor = graph[leaf].First();
                graph[neighbor].Remove(leaf);
                if (graph[neighbor].Count == 1)
                    leaves.Enqueue(neighbor);
            }
        }

        return leaves.ToList();
    }

    private void AddToGraph(IDictionary<int, HashSet<int>> graph, int key, int value)
    {
        if (!graph.TryAdd(key, new() { value })) graph[key].Add(value);
    }

    /// <summary>
    /// 834. Sum of Distances in Tree
    /// https://leetcode.com/problems/sum-of-distances-in-tree
    /// </summary>
    /// <param name="n"></param>
    /// <param name="edges"></param>
    /// <returns></returns>
    public int[] SumOfDistancesInTree(int n, int[][] edges)
    {
        if (n == 1) return [0];
        var graph = new Dictionary<int, HashSet<int>>();
        foreach (var edge in edges)
        {
            AddToGraph(graph, edge[0], edge[1]);
            AddToGraph(graph, edge[1], edge[0]);
        }

        int[] count = new int[n];
        int[] answer = new int[n];

        DFS(0, -1, graph, count, answer);
        DFS2(0, -1, graph, count, answer);

        return answer;
    }

    private void DFS(int node, int parent, Dictionary<int, HashSet<int>> graph, int[] count, int[] answer)
    {
        count[node] = 1;
        foreach (int child in graph[node])
        {
            if (child == parent) continue;
            DFS(child, node, graph, count, answer);
            count[node] += count[child];
            answer[node] += answer[child] + count[child];
        }
    }

    private void DFS2(int node, int parent, Dictionary<int, HashSet<int>> graph, int[] count, int[] answer)
    {
        foreach (int child in graph[node])
        {
            if (child == parent) continue;
            answer[child] = answer[node] - count[child] + count.Length - count[child];
            DFS2(child, node, graph, count, answer);
        }
    }
}
