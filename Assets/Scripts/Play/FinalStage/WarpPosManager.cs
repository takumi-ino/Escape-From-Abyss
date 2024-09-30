using System.Collections.Generic;
using UnityEngine;

public class WarpPosManager : MonoBehaviour
{
    //�@�e���[�v�|�C���g�����X�g�Ɋi�[
    List<Transform> warpPoints = new List<Transform>();

    public void AssignWarpPoint(Transform w)
    {
        warpPoints.Add(w);
    }

    public Transform GetWarpPointByName(string name)
    {

        foreach (Transform warpPoint in warpPoints)
        {
            // ���O����v������A���̃��[�v�ӏ���Ԃ�
            if (warpPoint.name == name)
            {
                return warpPoint;
            }
        }

        return null;
    }
}