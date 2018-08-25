using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathManager : MonoBehaviour
{
    public static PathManager instance;
    public GameObject nodPre;
    public PathNode[,] nodes;

    public PathNode strNode;
    public PathNode endNode;

    public List<PathNode> RecoderNodes;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        nodes = new PathNode[100, 100];
        strNode = nodes[0, 0];
        CreatMap();
    }

    /// <summary>
    /// 根据二维数组创建地图
    /// </summary>
    void CreatMap()
    {
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                GameObject node = Instantiate(nodPre) as GameObject;
                node.transform.position = new Vector3(i, 0, j);
                node.transform.SetParent(transform);
                nodes[i, j] = node.GetComponent<PathNode>();
                nodes[i, j].x = i;
                nodes[i, j].y = j;
            }
        }

    }

    public void CheckBlock()
    {
        foreach (var item in nodes)
        {
            item.CheckBlock();
        }
    }

    public void SetEndPoint(PathNode endNode)
    {
        strNode = nodes[0, 0];
        strNode.material.color = Color.green;
        this.endNode = endNode;
        this.endNode.material.color = Color.red;

        foreach (var item in nodes)
        {
            item.toEndPointDistan = Vector3.Distance(item.transform.position, endNode.transform.position);
            item.toStartPointDistan = Vector3.Distance(item.transform.position, strNode.transform.position);
        }

        RecoderNodes.Clear();

        foreach (var item in nodes)
        {
            item.Indit();
        }

        RecoderCheck(strNode);
    }

    /// <summary>
    /// 记录周围的点
    /// </summary>
    /// <param name="node"></param>
    public void RecoderCheck(PathNode node)
    {
        if (CheckOutRange(node.x, node.y + 1))
            Recoder(nodes[node.x, node.y + 1], node);
        if (CheckOutRange(node.x, node.y - 1))
            Recoder(nodes[node.x, node.y - 1], node);
        if (CheckOutRange(node.x + 1, node.y))
            Recoder(nodes[node.x + 1, node.y], node);
        if (CheckOutRange(node.x - 1, node.y))
            Recoder(nodes[node.x - 1, node.y], node);

        if (CheckOutRange(node.x+1, node.y + 1))
            Recoder(nodes[node.x+1, node.y + 1], node);
        if (CheckOutRange(node.x-1, node.y - 1))
            Recoder(nodes[node.x-1, node.y - 1], node);
        if (CheckOutRange(node.x + 1, node.y+1))
            Recoder(nodes[node.x + 1, node.y+1], node);
        if (CheckOutRange(node.x - 1, node.y-1))
            Recoder(nodes[node.x - 1, node.y-1], node);

        if (RecoderNodes.Count > 0)
        {
            PathNode select = SelectNode();   //从记录的点当中选择最近的点
            RecoderNodes.Remove(select);      
            if (select != endNode)            //如果没有到终点 继续寻找
                RecoderCheck(select);
            else
            {
                select.ShowPath();         
            }
        }
    }

    public PathNode SelectNode()
    {
        PathNode target = RecoderNodes[0];
        foreach (var item in RecoderNodes)
        {
            if ((item.currentCostDistan + item.toEndPointDistan) < (target.currentCostDistan + target.toEndPointDistan))
                target = item;
        }
        return target;
    }

    public void Recoder(PathNode node, PathNode proNode)
    {
        if (node.isRecoder) return;
        if(node.isBlock)return;
        node.isRecoder = true;
        RecoderNodes.Add(node);  //记录点
        node.proNode = proNode;
        node.currentCostDistan = node.proNode.currentCostDistan +
                                 Vector3.Distance(node.transform.position, node.proNode.transform.position);
        node.material.color = Color.cyan;
    }

    /// <summary>
    /// 检查数组越界问题
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool CheckOutRange(int x, int y)
    {
        if (x < 0 || y < 0 || x > nodes.GetLength(0) - 1 || y > nodes.GetLength(1) - 1)
        {
            return false;
        }
        return true;
    }
}
