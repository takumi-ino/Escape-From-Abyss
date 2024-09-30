using UnityChan;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalPosController : MonoBehaviour
{
    DestinationPosSceneTransition goalPosSceneTransition;

    public static bool isLightUpDestinationPoint = false;

    Light lightSource; // 点滅させるLightコンポーネント
    float time;
    float speed = 2f;

    bool fade = false;

    string currentStage;

    private void Start()
    {
        lightSource = GetComponent<Light>();

        goalPosSceneTransition = FindObjectOfType<DestinationPosSceneTransition>();

        currentStage = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        Flush();

        if (currentStage != SceneManager.GetActiveScene().name)
        {
            string newScene = SceneManager.GetActiveScene().name;
            currentStage = newScene;
        }
    }

    void Flush()
    {
        if (!isLightUpDestinationPoint) return;

        time += speed * Time.deltaTime;
        lightSource.intensity = Mathf.Sin(time) * 15f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLightUpDestinationPoint) return;

        // プレイヤーと接触したら、セカンドステージへ
        if (other.gameObject.CompareTag("Player"))
        {
            if (currentStage == "FirstStage")
            {
                isLightUpDestinationPoint = false;
                goalPosSceneTransition.SceneTransition("SecondStage");
            }
            else if (currentStage == "SecondStage")
            {
                isLightUpDestinationPoint = false;
                goalPosSceneTransition.SceneTransition("FinalStage");
            }
            else
            {
                isLightUpDestinationPoint = false;
                GameTimer.instance.stop = true;

                // 死亡回数情報を渡す
                new SceneResult(UnityChanController.instance.deadCount);

                goalPosSceneTransition.SceneTransition("Result");
            }
        }
    }
}