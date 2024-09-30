using UnityEngine;
using UnityEngine.UI;

public class SceneResult : MonoBehaviour
{
    int resultDeadCount = 0;
    Text deadCountText;

    public SceneResult(int deadCount)
    {
       resultDeadCount = deadCount;
    }

    void Start()
    {
        deadCountText = GameObject.Find("ResultDeathCount").GetComponent<Text>();
        deadCountText.text = resultDeadCount.ToString();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            SceneChange.instance.LoadTitleSceneAfterGame();
        }
    }
}
