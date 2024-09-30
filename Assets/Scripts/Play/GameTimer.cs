using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    int minutes = 0;
    float seconds = 0.0f;

    public static GameTimer instance;

    public bool stop = false;
    bool isNullText = true;

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


    private void Update()
    {
        // ゲームクリアし、リザルトシーンに飛んだら
        if (instance.stop && isNullText && SceneManager.GetActiveScene().name == "Result")
        {
            // 秒
            Text sec = GameObject.Find("ResultTimeSeconds").GetComponent<Text>();
            int s = (int)seconds;
            sec.text = s.ToString();
            sec.enabled = true;

            // 分
            Text minut = GameObject.Find("ResultTimeMinutes").GetComponent<Text>();

            if (minutes < 10) minut.text = "0" + minutes.ToString();
            else              minut.text = minutes.ToString();

            minut.enabled = true;
            
            isNullText = false;

            return;
        }
        // ゲームがまだ続行中なら時間計測を続ける
        else if (!instance.stop && isNullText)
        {
            seconds += Time.deltaTime;
        }

        if (seconds > 60.0f)
        {
            seconds = 0.0f;
            minutes++;
        }
    }
}