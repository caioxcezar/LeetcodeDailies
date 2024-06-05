namespace LeetcodeDailies;

public class TreeNode(int val = 0, TreeNode? left = null, TreeNode? right = null)
{
    public int val = val;
    public TreeNode? left = left;
    public TreeNode? right = right;

    public static TreeNode BuildTree(int?[] array)
    {
        var queue = new Queue<TreeNode?>();
        var root = new TreeNode(array[0]!.Value);
        queue.Enqueue(root);

        for (int i = 1; i < array.Length; i += 2)
        {
            var current = queue.Dequeue();
            if (array[i] != null)
            {
                current!.left = new TreeNode(array[i].Value);
                queue.Enqueue(current.left);
            }
            if (i + 1 < array.Length && array[i + 1] != null)
            {
                current!.right = new TreeNode(array[i + 1].Value);
                queue.Enqueue(current.right);
            }
        }

        return root;
    }

    public override string ToString()
    {
        var list = new List<string>();
        ConvertToArray(this, list);
        return String.Join(',', list);
    }

    public void ConvertToArray(TreeNode? curr, List<string> list)
    {
        if (curr == null) return;
        list.Add(curr.val.ToString());
        ConvertToArray(curr.left, list);
        ConvertToArray(curr.right, list);
    }
}

public class Trees
{
    /// <summary>
    /// 404. Sum of Left Leaves
    /// https://leetcode.com/problems/sum-of-left-leaves
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    public int SumOfLeftLeaves(TreeNode root) => SumOfLeftLeaves(root, false, 0);

    private int SumOfLeftLeaves(TreeNode? root, bool isLeft, int sum)
    {
        if (root == null) return isLeft ? sum : 0;
        if (root.left == null && root.right == null) return isLeft ? root.val + sum : sum;
        return SumOfLeftLeaves(root.left, true, sum) + SumOfLeftLeaves(root.right, false, sum);
    }

    /// <summary>
    /// 129. Sum Root to Leaf Numbers
    /// https://leetcode.com/problems/sum-root-to-leaf-numbers
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    public int SumNumbers(TreeNode root) => SumNumbers(root, "");

    private int SumNumbers(TreeNode? root, string val)
    {
        if (root == null) return 0;
        var newVal = $"{val}{root.val}";
        if (root.left != null || root.right != null)
            return SumNumbers(root.left, newVal) + SumNumbers(root.right, newVal);
        return int.Parse(newVal);
    }

    /// <summary>
    /// 623. Add One Row to Tree
    /// https://leetcode.com/problems/add-one-row-to-tree
    /// </summary>
    /// <param name="root"></param>
    /// <param name="val"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    public TreeNode AddOneRow(TreeNode root, int val, int depth)
    {
        if (depth == 1) return new TreeNode(val, root);

        AddOneRow(root, val, depth, 1);
        return root;
    }

    private void AddOneRow(TreeNode? node, int val, int depth, int currentDepth)
    {
        if (node == null) return;

        if (currentDepth == depth - 1)
        {
            node.left = new TreeNode(val, node.left);
            node.right = new TreeNode(val, null, node.right);
            return;
        }

        AddOneRow(node.left, val, depth, currentDepth + 1);
        AddOneRow(node.right, val, depth, currentDepth + 1);
    }

    /// <summary>
    /// 988. Smallest String Starting From Leaf
    /// https://leetcode.com/problems/smallest-string-starting-from-leaf
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    public string SmallestFromLeaf(TreeNode root)
    {
        var record = new List<string>();
        SmallestFromLeaf(root, "", record);
        return record.Min();
    }

    private void SmallestFromLeaf(TreeNode? root, string current, List<string> record)
    {
        if (root == null)
        {
            record.Add(current);
            return;
        }
        var curr = $"{(char)(root.val + 97)}{current}";
        SmallestFromLeaf(root.left, curr, record);
        SmallestFromLeaf(root.right, curr, record);
    }

    /// <summary>
    /// 2331. Evaluate Boolean Binary Tree
    /// https://leetcode.com/problems/evaluate-boolean-binary-tree
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    public bool EvaluateTree(TreeNode root) => root.val switch
    {
        2 => EvaluateTree(root.left) || EvaluateTree(root.right),
        3 => EvaluateTree(root.left) && EvaluateTree(root.right),
        _ => root.val == 1
    };

    /// <summary>
    /// 1325. Delete Leaves With a Given Value
    /// https://leetcode.com/problems/delete-leaves-with-a-given-value
    /// </summary>
    /// <param name="root"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public TreeNode RemoveLeafNodes(TreeNode root, int target)
    {
        if (root.left != null) root.left = RemoveLeafNodes(root.left, target);
        if (root.right != null) root.right = RemoveLeafNodes(root.right, target);
        if (root.left == null && root.right == null && root.val == target) return null;
        return root;
    }

    /// <summary>
    /// 979. Distribute Coins in Binary Tree
    /// https://leetcode.com/problems/distribute-coins-in-binary-tree
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    public int DistributeCoins(TreeNode root) => DistributeCoins(root, null);

    private int DistributeCoins(TreeNode? root, TreeNode? prev)
    {
        if (root == null) return 0;
        int moves = DistributeCoins(root.left, root) + DistributeCoins(root.right, root);
        int val = root.val - 1;
        if (prev != null) prev.val += val;
        moves += Math.Abs(val);
        return moves;
    }
}
