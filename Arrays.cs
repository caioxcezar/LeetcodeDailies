using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace LeetcodeDailies;

public class Arrays
{
    /// <summary>
    /// 950. Reveal Cards In Increasing Order
    /// https://leetcode.com/problems/reveal-cards-in-increasing-order
    /// </summary>
    /// <param name="deck">Array to be ordered</param>
    /// <returns>Ordered array</returns>
    public int[] DeckRevealedIncreasing(int[] deck)
    {
        Array.Sort(deck);
        var result = new int[deck.Length];
        var index = new Queue<int>();

        for (var i = 0; i < deck.Length; i++)
            index.Enqueue(i);

        foreach (var card in deck)
        {
            result[index.Dequeue()] = card;
            if (index.Count > 0)
                index.Enqueue(index.Dequeue());
        }

        return result;
    }

    /// <summary>
    /// 2073. Time Needed to Buy Tickets
    /// https://leetcode.com/problems/time-needed-to-buy-tickets
    /// </summary>
    /// <param name="tickets"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public int TimeRequiredToBuy(int[] tickets, int k)
    {
        var remaining = tickets[k];
        var time = tickets.Aggregate(0, (acc, ticket) => Math.Min(ticket, remaining) + acc);
        time -= tickets.Skip(k + 1).Count((ticket) => ticket >= remaining);
        return time;
    }

    /// <summary>
    /// 1700. Number of Students Unable to Eat Lunch
    /// https://leetcode.com/problems/number-of-students-unable-to-eat-lunch/
    /// </summary>
    /// <param name="students">List of types of sandwiches that students prefer to eat</param>
    /// <param name="sandwiches">List of types of sandwiches</param>
    /// <returns>Number of students that are unable to eat</returns>
    public int CountStudents(int[] students, int[] sandwiches)
    {
        var i = 0;
        var std = new Queue<int>(students);
        var sdw = new Queue<int>(sandwiches);
        while (std.Count > 0 && sdw.Count > 0)
        {
            if (std.Peek() == sdw.Peek())
            {
                std.Dequeue();
                sdw.Dequeue();
                i = 0;
            }
            else
            {
                std.Enqueue(std.Dequeue());
                i++;
            }
            if (i == std.Count) break;
        }
        return std.Count;
    }

    /// <summary>
    /// 42. Trapping Rain Water
    /// https://leetcode.com/problems/trapping-rain-water
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public int Trap(int[] height)
    {
        int left = 0, right = height.Length - 1;
        int leftMax = 0, rightMax = 0;
        int result = 0;

        while (left < right)
        {
            if (height[left] < height[right])
            {
                leftMax = Math.Max(height[left], leftMax);
                result += leftMax - height[left];
                left++;
            }
            else
            {
                rightMax = Math.Max(height[right], rightMax);
                result += rightMax - height[right];
                right--;
            }
        }

        return result;
    }

    /// <summary>
    /// 752. Open the Lock
    /// https://leetcode.com/problems/open-the-lock
    /// </summary>
    /// <param name="deadends"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public int OpenLock(string[] deadends, string target)
    {
        var deadSet = new HashSet<string>(deadends);
        var visited = new HashSet<string>();
        var queue = new Queue<string>();
        queue.Enqueue("0000");
        visited.Add("0000");

        var count = 0;

        while (queue.Count > 0)
        {
            var size = queue.Count;
            for (var i = 0; i < size; i++)
            {
                var current = queue.Dequeue();
                if (deadSet.Contains(current)) continue;
                if (current == target) return count;

                for (int j = 0; j < 4; j++)
                {
                    for (int k = -1; k <= 1; k += 2)
                    {
                        var value = current.ToCharArray();
                        value[j] = (char)(((value[j] - '0' + k + 10) % 10) + '0');
                        var next = new string(value);

                        if (visited.Contains(next)) continue;
                        visited.Add(next);
                        queue.Enqueue(next);
                    }
                }
            }
            count++;
        }

        return -1;
    }

    /// <summary>
    /// 2441. Largest Positive Integer That Exists With Its Negative
    /// https://leetcode.com/problems/largest-positive-integer-that-exists-with-its-negative
    /// </summary>
    /// <param name="nums"></param>
    /// <returns></returns>
    public int FindMaxK(int[] nums)
    {
        Array.Sort(nums);
        for (var i = nums.Length - 1; i > -1; i--)
        {
            if (nums[i] < 0) break;
            if (nums.Contains(-nums[i])) return nums[i];
        }
        return -1;
    }

    /// <summary>
    /// 881. Boats to Save People
    /// https://leetcode.com/problems/boats-to-save-people
    /// </summary>
    /// <param name="people"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public int NumRescueBoats(int[] people, int limit)
    {
        Array.Sort(people);
        var count = 0;
        int left = 0, right = people.Count() - 1;
        while (left <= right)
        {
            count++;
            if (people[left] + people[right] <= limit) left++;
            right--;
        }

        return count;
    }

    /// <summary>
    /// 506. Relative Ranks
    /// https://leetcode.com/problems/relative-ranks
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public string[] FindRelativeRanks(int[] score)
    {
        var sorted = score.OrderByDescending((val) => val).ToArray();
        var result = new string[score.Length];
        for (var i = 0; i < result.Length; i++)
        {
            var pos = Array.IndexOf(score, sorted[i]);
            result[pos] = i switch
            {
                0 => "Gold Medal",
                1 => "Silver Medal",
                2 => "Bronze Medal",
                _ => (i + 1).ToString()
            };
        }
        return result;
    }

    /// <summary>
    /// 3075. Maximize Happiness of Selected Children
    /// https://leetcode.com/problems/maximize-happiness-of-selected-children
    /// </summary>
    /// <param name="happiness"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public long MaximumHappinessSum(int[] happiness, int k)
    {
        Array.Sort(happiness);
        return happiness.Skip(happiness.Length - k).Select<int, long>((val) => Math.Max(val - --k, 0)).Sum();
    }

    /// <summary>
    /// 786. K-th Smallest Prime Fraction
    /// https://leetcode.com/problems/k-th-smallest-prime-fraction
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public int[] KthSmallestPrimeFraction(int[] arr, int k)
    {
        var n = arr.Length;
        double left = 0D, right = 1D;

        while (left < right)
        {
            var middle = (left + right) / 2D;
            var smallTMid = 0;
            var p = 0;
            var q = 1;

            for (int i = 0, j = 1; i < n; ++i)
            {
                while (j < n && arr[i] > middle * arr[j]) j++;
                if (j == n) break;
                smallTMid += n - j;
                if (p * arr[j] < q * arr[i])
                {
                    p = arr[i];
                    q = arr[j];
                }
            }

            if (smallTMid == k) return [p, q];
            if (smallTMid > k) right = middle;
            else left = middle;
        }

        return arr;
    }

    /// <summary>
    /// 857. Minimum Cost to Hire K Workers
    /// https://leetcode.com/problems/minimum-cost-to-hire-k-workers
    /// </summary>
    /// <param name="quality"></param>
    /// <param name="wage"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public double MincostToHireWorkers(int[] quality, int[] wage, int k)
    {
        var workers = quality.Select((_, i) => (Quality: quality[i], Rate: (double)wage[i] / quality[i])).OrderBy((x) => x.Rate);

        var totalQuality = 0D;
        var pickedQualities = new PriorityQueue<int, int>(k + 1);
        var minPrice = double.PositiveInfinity;

        foreach (var worker in workers)
        {
            totalQuality += worker.Quality;
            pickedQualities.Enqueue(worker.Quality, -worker.Quality);
            if (pickedQualities.Count > k)
                totalQuality -= pickedQualities.Dequeue();

            minPrice = pickedQualities.Count == k ? Math.Min(minPrice, worker.Rate * totalQuality) : minPrice;
        }
        return minPrice;
    }

    /// <summary>
    /// 3068. Find the Maximum Sum of Node Values
    /// https://leetcode.com/problems/find-the-maximum-sum-of-node-values
    /// </summary>
    /// <param name="nums"></param>
    /// <param name="k"></param>
    /// <param name="edges"></param>
    /// <returns></returns>
    public long MaximumValueSum(int[] nums, int k, int[][] edges)
    {
        var total = nums.Sum((val) => (long)val);
        var totalDiff = 0L;
        var positiveCount = 0;
        var minAbsDiff = long.MaxValue;

        foreach (var num in nums)
        {
            var diff = (num ^ k) - num;

            if (diff > 0)
            {
                totalDiff += diff;
                positiveCount++;
            }
            minAbsDiff = Math.Min(minAbsDiff, Math.Abs(diff));
        }
        if (positiveCount % 2 == 1) totalDiff -= minAbsDiff;
        return total + totalDiff;
    }

    /// <summary>
    /// 1863. Sum of All Subset XOR Totals
    /// https://leetcode.com/problems/sum-of-all-subset-xor-totals
    /// </summary>
    /// <param name="nums"></param>
    /// <returns></returns>
    public int SubsetXORSum(int[] nums)
    {
        if (nums.Length == 0) return 0;
        var sum = 0;

        for (int i = 1; i < (1 << nums.Length); i++)
        {
            var subset = 0;
            for (int j = 0; j < nums.Length; j++)
            {
                if ((i & (1 << j)) != 0) subset ^= nums[j];
            }
            sum += subset;
        }
        return sum;
    }

    /// <summary>
    /// 78. Subsets
    /// https://leetcode.com/problems/subsets
    /// </summary>
    /// <param name="nums"></param>
    /// <returns></returns>
    public IList<IList<int>> Subsets(int[] nums)
    {
        var result = new List<IList<int>>();
        var n = nums.Length - 1;
        for (var i = 0; i <= nums.Length; i++)
        {
            var subset = Subsets(nums, i, n);
        }

        return result;
    }

    private IList<int> Subsets(int[] nums, int index, int n)
    {
        var subset = new List<int>();
        for (var i = index; i <= n; i++)
        {
            subset.Add(nums[i]);
        }
        return subset;
    }
}
