using UnityEngine;
using System.Collections;

public class PathNode : MonoBehaviour
{
    public int x, y;

    public float toEndPointDistan;
    public float toStartPointDistan;
    public float currentCostDistan;

    public bool isBlock;
    public bool isRecoder;

    public PathNode proNode; //上一个点

    public Material material { get { return gameObject.GetComponent<Renderer>().material; } }


    public void Indit()
    {
        isRecoder = false;
    }

    void OnMouseDown()
    {
        PathManager.instance.CheckBlock();
        PathManager.instance.SetEndPoint(this);
    }

    public void CheckBlock()
    {
        isBlock = Physics.Raycast(transform.position, Vector3.up, 1);
        if (isBlock)
            material.color = Color.black;
        else
        {
            material.color = Color.white;
        }
    }

    public void ShowPath()
    {
        if (this != PathManager.instance.strNode)
        {
            material.color = Color.blue;
            proNode.ShowPath();
        }
        else
        {
            material.color = Color.green;
        }
            

    }

}
