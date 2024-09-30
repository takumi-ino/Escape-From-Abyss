using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonClass : MonoBehaviour
{
    
    public static SingletonClass instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "TitleScene" ||
            SceneManager.GetActiveScene().name == "CharaSelectScene" ||
            SceneManager.GetActiveScene().name == "Result")
        {
            if(this) Destroy(gameObject);
        }
    }
}