using System.Collections.Generic;
using UnityEngine;

public class WarpPosManager : MonoBehaviour
{
    //　各ワープポイントをリストに格納
    List<Transform> warpPoints = new List<Transform>();

    public void AssignWarpPoint(Transform w)
    {
        warpPoints.Add(w);
    }

    public Transform GetWarpPointByName(string name)
    {

        foreach (Transform warpPoint in warpPoints)
        {
            // 名前が一致したら、そのワープ箇所を返す
            if (warpPoint.name == name)
            {
                return warpPoint;
            }
        }

        return null;
    }
}