using System.Numerics;
using System.Text;

namespace LeetcodeDailies;

public class ListNode(int val)
{
    public int val = val;
    public ListNode? next;

    public static ListNode Build(int[] array)
    {
        var root = new ListNode(array[0]);
        var current = root;
        for (int i = 1; i < array.Length; i++)
        {
            current.next = new ListNode(array[i]);
            current = current.next;
        }
        return root;
    }

    public ListNode? Get(int val)
    {
        var current = this;
        while (current != null)
        {
            if (current.val == val) return current;
            current = current.next;
        }
        return null;
    }

    public static ListNode BuildAndGet(int[] array, int val) => Build(array).Get(val)!;

    public int[] ToArray() {
        var list = new List<int>();
        var current = this;
        while (current != null)
        {
            list.Add(current.val);
            current = current.next;
        }
        return list.ToArray();
    }
}

public class Pointers
{
    public void DeleteNode(ListNode node)
    {
        if (node == null || node.next == null) return;
        node.val = node.next.val;
        node.next = node.next.next;
    }

    public ListNode RemoveNodes(ListNode head)
    {
        if (head.next == null) return head;
        var pos = RemoveNodes(head.next);
        head.next = pos;
        if (head.val < pos.val) return pos; 
        return head;
    }

    public ListNode DoubleIt(ListNode head)
    {
        var prev = new ListNode(0);
        prev.next = head;
        DoubledValue(head, prev);
        return prev.val > 0 ? prev : head;
    }

    private void DoubledValue(ListNode? head, ListNode prev)
    {
        if (head == null) return;
        var newVal = head.val * 2;
        if (newVal > 9) prev.val++;
        head.val = newVal % 10;
        DoubledValue(head.next, head);
    }
}
