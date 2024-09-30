using UnityChan;
using UnityEngine;

public class DestinationPosSceneTransition : MonoBehaviour
{

    public static DestinationPosSceneTransition instance;

    public static Vector3 transitionPoint_stage1 { get; private set; }
    public static Vector3 transitionPoint_stage2 { get; private set; }
    public static Vector3 transitionPoint_stage3 { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        // ステージ１ デバッグ用スポーン位置　Vector3(25,8.36,38)
        // ステージ１ 定位置                  Vector3((24f, 8.36f, 37.75f)
        transitionPoint_stage1 = new Vector3(25, 8.36f, 38);

        // ステージ２ デバッグ用スポーン位置　Vector3(101, 4.5f, 92f)
        // ステージ２ 定位置                  Vector3(83, 7.7f, -34)
        transitionPoint_stage2 = new Vector3(101, 4.5f, 92f);

        // ステージ３ デバッグ用スポーン位置　Vector3(-374, 10f, 11f)
        // ステージ３ 望ましい定位置（梯子で登れないため不可）Vector3(Vector3(70.1800003,2.6099999,26.75)
        // ステージ３ サブスポーン位置(デフォルト）　Vector3(55.0730019, 7.8130002, 9.125)

        transitionPoint_stage3 = new Vector3(-374f, 10f, 11f);
    }

    public void SceneTransition(string nextSceneName)
    {
        if (!BlackBoxManager.HasAllRequiredBox()) return;

        // new Vector3で y軸を低く設定しすぎると、遷移と同時にゲームオーバーになってしまうので注意

        if (nextSceneName == "SecondStage")
        {
            SceneChange.instance.ChangeScene(nextSceneName);
            UnityChanController.instance.transform.position = transitionPoint_stage2;
            BlackBoxManager.boxNum = 0;

            return;

            //player.transform.position = new Vector3(60, 0, -16);
        }
        else if (nextSceneName == "FinalStage")
        {
            SceneChange.instance.ChangeScene(nextSceneName);
            UnityChanController.instance.transform.position = transitionPoint_stage3;
            BlackBoxManager.boxNum = 0;

            //player.transform.position = new Vector3(55f, 7.7f, 7.7f);
            return;
        }
        else if (nextSceneName == "Result")
        {
            GameState.gameClear = true;

            SceneChange.instance.ChangeScene(nextSceneName);
            BlackBoxManager.boxNum = 0;

            return;
        }
    }
}