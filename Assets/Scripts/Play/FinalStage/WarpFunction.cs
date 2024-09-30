using UnityChan;
using UnityEngine;

public class WarpFunction : MonoBehaviour
{
    WarpPosManager warpManager;

    bool isWarpEnabled = true;
    float warpDisableTime = 5f; // ワープ無効化時間

    public Vector3 startPos_stage1 { get; private set; }
    public Vector3 startPos_stage2 { get; private set; }
    public Vector3 startPos_stage3 { get; private set; }

    private void Start()
    {
        warpManager = FindObjectOfType<WarpPosManager>();

        if (warpManager != null)
        {
            warpManager.AssignWarpPoint(transform);
        }

    }


    void GetNextWarpPos(Collider c, Transform t, string posName)
    {
        t = warpManager.GetWarpPointByName(posName);

        if (t != null)
        {
            c.transform.position = t.position;
            c.transform.rotation = t.rotation;
            isWarpEnabled = false;
            Invoke(nameof(EnableWarp), warpDisableTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 当たった相手がプレイヤーでなければ
        if (!other.gameObject.CompareTag("Player")) return;
        //　warpManagerオブジェクトが生成されてなければ
        if (warpManager == null) return;
        // ワープできなければ
        if (!isWarpEnabled) return;

        switch (gameObject.name)
        {
            case "Warp_red":

                GetNextWarpPos(other, transform, "Warp_red (1)");

                other.gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);

                break;
            //case "Warp_red (1)":

            //    GetNextWarpPos(other, transform, "Warp_red"); // back
            //    break;
            case "Warp_red (2)":

                GetNextWarpPos(other, transform, "Warp_red (3)");

                other.gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);

                break;
            //case "Warp_red (3)":

            //    GetNextWarpPos(other, transform, "Warp_red (2)"); // back
            //    break;
            case "Warp_red (4)":

                GetNextWarpPos(other, transform, "Warp_blue");
                break;
            //case "Warp_blue":

            //    GetNextWarpPos(other, transform, "Warp_red (4)"); // back
            //    break;
            case "Warp_blue (1)":

                GetNextWarpPos(other, transform, "Warp_green");
                break;
            //case "Warp_green":

            //    GetNextWarpPos(other, transform, "Warp_blue (1)"); // back
            //    break;
            case "Warp_green (1)":

                GetNextWarpPos(other, transform, "Warp_green (2)");
                break;
                //case "Warp_green (2)":

                //    GetNextWarpPos(other, transform, "Warp_green (1)"); // back
                //    break;
        }
    }

    void EnableWarp()
    {
        isWarpEnabled = true;
    }
}