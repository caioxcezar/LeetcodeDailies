using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;

namespace LeetcodeDailies;

public class Strings
{
    public int LengthOfLastWord(string s) => s.Trim().Split(" ").Last().Length;

    public int MaxDepth(string s)
    {
        var count = 0;
        var max = 0;
        foreach (var @char in s)
        {
            if (@char == '(') count++;
            if (@char == ')') count--;
            max = Math.Max(max, count);
        }
        return max;
    }

    public bool IsIsomorphic(string s, string t)
    {
        if (s.Length != t.Length) return false;
        var lastCharS = s[0];
        var lastCharT = t[0];
        var charMap = new Dictionary<char, char>();
        var valueMap = new Dictionary<char, char>();
        for (var i = 0; i < t.Length; i++)
        {
            if (!charMap.ContainsKey(s[i]))
            {
                charMap.Add(s[i], t[i]);
                valueMap.TryAdd(t[i], s[i]);
            }
            else if (charMap[s[i]] != t[i]) return false;

            if (valueMap.ContainsKey(t[i]) && valueMap[t[i]] != s[i]) return false;

            if (lastCharS == s[i] && lastCharT != t[i]) return false;
            lastCharS = s[i];
            lastCharT = t[i];
        }
        return true;
    }

    /// <summary>
    /// 1544. Make The String Great
    /// https://leetcode.com/problems/make-the-string-great/
    /// </summary>
    /// <param name="s">String of lower and upper case English letters.</param>
    /// <returns>String after making it good. </returns>
    public string MakeGood(string s)
    {
        if (s.Length <= 1) return s;

        for (int i = 0; i < s.Length - 1; i++)
            if (s[i] == s[i + 1] - 32 || s[i] == s[i + 1] + 32)
                return MakeGood(s.Remove(i, 2));

        return s;
    }

    /// <summary>
    /// 1249. Minimum Remove to Make Valid Parentheses
    /// </summary>
    /// <param name="s">String s of '(' , ')' and lowercase English characters</param>
    /// <returns>String with the minimum number of parentheses to be removed to make the resulting string valid</returns>
    public string MinRemoveToMakeValid(string s)
    {
        var stack = new Stack<int>();
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '(') stack.Push(i);
            else if (s[i] == ')')
            {
                if (stack.Count > 0) stack.Pop();
                else
                {
                    s = s.Remove(i, 1);
                    i--;
                }
            }
        }

        while (stack.Count > 0)
        {
            var index = stack.Pop();
            s = s.Remove(index, 1);
        }

        return s;
    }

    /// <summary>
    /// 678. Valid Parenthesis String
    /// https://leetcode.com/problems/valid-parenthesis-string/
    /// </summary>
    /// <param name="s">String containing only three types of characters: '(', ')' and '*'</param>
    /// <returns>string is valid or not</returns>

    public bool CheckValidString(string s)
    {
        Stack<int> openStack = new Stack<int>();
        Stack<int> starStack = new Stack<int>();

        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '(') openStack.Push(i);
            else if (s[i] == '*') starStack.Push(i);
            else
            {
                if (openStack.Count == 0 && starStack.Count == 0) return false;
                if (openStack.Count > 0) openStack.Pop();
                else starStack.Pop();
            }
        }

        while (openStack.Count > 0 && starStack.Count > 0)
        {
            if (openStack.Peek() > starStack.Peek()) return false;
            openStack.Pop();
            starStack.Pop();
        }

        return openStack.Count == 0;
    }

    /// <summary>
    /// 402. Remove K Digits
    /// https://leetcode.com/problems/remove-k-digits
    /// </summary>
    /// <param name="num"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public string RemoveKdigits(string num, int k)
    {
        if (num.Length <= k) return "0";

        var result = new List<char>();
        foreach (var digit in num)
        {
            while (k > 0 && result.Count > 0 && result.Last() > digit)
            {
                result.Remove(result.Last());
                k--;
            }
            result.Add(digit);
        }

        while (k > 0)
        {
            result.Remove(result.Last());
            k--;
        }

        var str = string.Join("", result).TrimStart('0');

        return str == "" ? "0" : str;
    }

    /// <summary>
    /// 2370. Longest Ideal Subsequence
    /// https://leetcode.com/problems/longest-ideal-subsequence
    /// </summary>
    /// <param name="s"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public int LongestIdealString(string s, int k)
    {
        var dp = new int[26];
        foreach (var c in s)
        {
            var i = c - 'a';
            var first = Math.Max(0, i - k);
            var last = Math.Min(25, i + k);
            var maxReachable = 0;
            for (var j = first; j <= last; ++j)
                maxReachable = Math.Max(maxReachable, dp[j]);
            dp[i] = 1 + maxReachable;
        }
        return dp.Max();
    }

    /// <summary>
    /// 514. Freedom Trail
    /// https://leetcode.com/problems/freedom-trail
    /// </summary>
    /// <param name="ring"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public int FindRotateSteps(string ring, string key) => DFS(ring, key, 0, new()) + key.Length;

    private int DFS(string ring, string key, int index, Dictionary<string, int> mem)
    {
        if (index == key.Length) return 0;

        string hashKey = $"{ring}:{index}";
        if (mem.ContainsKey(hashKey)) return mem[hashKey];

        int result = int.MaxValue;
        for (int i = 0; i < ring.Length; i++)
        {
            if (ring[i] != key[index]) continue;
            var steps = Math.Min(i, ring.Length - i);
            var newRing = ring.Substring(i) + ring.Substring(0, i);
            result = Math.Min(result, steps + DFS(newRing, key, index + 1, mem));
        }

        mem.Add(hashKey, result);
        return result;
    }

    /// <summary>
    /// 2000. Reverse Prefix of Word
    /// https://leetcode.com/problems/reverse-prefix-of-word
    /// </summary>
    /// <param name="word"></param>
    /// <param name="ch"></param>
    /// <returns></returns>
    public string ReversePrefix(string word, char ch)
    {
        var index = word.IndexOf(ch);
        if (index == -1) return word;
        var chars = word.ToCharArray();
        Array.Reverse(chars, 0, index + 1);
        return new string(chars);
    }


    /// <summary>
    /// 165. Compare Version Numbers
    /// https://leetcode.com/problems/compare-version-numbers
    /// </summary>
    /// <param name="version1"></param>
    /// <param name="version2"></param>
    /// <returns></returns>
    public int CompareVersion(string version1, string version2)
    {
        var v1 = version1.Split('.').Select(int.Parse);
        var v2 = version2.Split('.').Select(int.Parse);
        var n = Math.Max(v1.Count(), v2.Count());

        for (int i = 0; i < n; i++)
        {
            var val1 = v1.ElementAtOrDefault(i);
            var val2 = v2.ElementAtOrDefault(i);
            if (val1 > val2) return 1;
            if (val1 < val2) return -1;
        }
        return 0;
    }
}
