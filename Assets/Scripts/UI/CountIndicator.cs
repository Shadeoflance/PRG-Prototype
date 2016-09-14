using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

class CounterNode
{
    public int value;
    Image image;
    public List<CounterNode> children = new List<CounterNode>(5);
    public CounterNode next;
    Color c;

    public CounterNode(int value, Image image, Color color)
    {
        this.value = value;
        this.image = image;
        c = color;
    }

    public void Activate()
    {
        image.color = new Color(c.r, c.g, c.b, 1);
    }

    public void Deactivate()
    {
        image.color = new Color(c.r, c.g, c.b, 0);
    }
}
enum Resource
{
    hp, orbs, pixels
}
class CountIndicator : MonoBehaviour
{
    public Color color;
    List<CounterNode> nodes = new List<CounterNode>(5);
    int count = 0;
    public Resource resource;
    void Start()
    {
        foreach(Transform t20 in transform)
        {
            CounterNode n20 = new CounterNode(20, t20.GetComponent<Image>(), color);
            if(nodes.Count > 0)
                nodes[nodes.Count - 1].next = n20;
            nodes.Add(n20);
            foreach(Transform t5 in t20)
            {
                CounterNode n5 = new CounterNode(5, t5.GetComponent<Image>(), color);
                if(n20.children.Count > 0)
                    n20.children[n20.children.Count - 1].next = n5;
                n20.children.Add(n5);
                foreach(Transform t1 in t5)
                {
                    CounterNode n1 = new CounterNode(1, t1.GetComponent<Image>(), color);
                    if(n5.children.Count > 0)
                        n5.children[n5.children.Count - 1].next = n1;
                    n5.children.Add(n1);
                }
            }
        }
        Clear();
    }

    void Update()
    {
        if (resource == Resource.orbs)
        {
            if (Player.instance.orbs != count)
                ChangeCount(Player.instance.orbs);
        }
        else if(resource == Resource.pixels)
        {
            if (Player.instance.pixels != count)
                ChangeCount(Player.instance.pixels);
        }
        else if(resource == Resource.hp)
        {
            if ((int)Player.instance.stats.hp != count)
                ChangeCount((int)Player.instance.stats.hp);
        }
    }

    void Clear()
    {
        foreach(var n20 in nodes)
        {
            n20.Deactivate();
            foreach(var n5 in n20.children)
            {
                n5.Deactivate();
                foreach (var n1 in n5.children)
                    n1.Deactivate();
            }
        }
    }

    void ChangeCount(int count)
    {
        this.count = count;
        Clear();
        int curCount = 0;
        CounterNode node = nodes[0];
        while(curCount < count && node != null)
        {
            if (curCount + node.value <= count)
            {
                curCount += node.value;
                node.Activate();
                node = node.next;
            } else
            {
                node = node.children[0];
            }
        }
    }
}