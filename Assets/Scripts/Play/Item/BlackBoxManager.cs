using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class BlackBoxManager : MonoBehaviour
{
    public GameObject blackBoxPrefab;

    public static int boxNum;

    public static int BoxNum { get { return boxNum; } }

    public static Dictionary<string, int> BoxNumReference { get { return requiredBoxNumList; } }

    public static Dictionary<string, int> requiredBoxNumList = new Dictionary<string, int>()
{
    {"FirstStage", 2},
    {"SecondStage", 3},
    {"FinalStage", 4}
};

    float spawnRadius;
    int blackBoxSum;
    bool spawnOnStart = true;

    private void Start()
    {
        boxNum = 0;

        if (spawnOnStart)
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "FirstStage":

                    blackBoxSum = 2;
                    break;
                case "SecondStage":

                    blackBoxSum = 3;
                    break;
                case "FinalStage":

                    blackBoxSum = 4;
                    break;
            }

            SpawnAll();
            spawnOnStart = false;
        }
    }

    public void SpawnAll()
    {

        for (int i = 0; i < blackBoxSum; i++)
        {
            Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRadius;

            GameObject obj = Instantiate(blackBoxPrefab, randomPos, Quaternion.identity);
        }
    }


    public void BottomUp(GameObject obj, float y, int dis)
    {
        Ray ray = new Ray(obj.transform.position, Vector3.up);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, dis))
        {
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y + y, obj.transform.position.z);
            BottomUp(obj, y, dis);
        }
    }

    public void ToGround(GameObject obj, float y)
    {
        Ray ray = new Ray(obj.transform.position, Vector3.down);
        int dis = 100;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, dis))
            BottomUp(obj, y, dis);
    }


    // blackBoxŠŽ”‘‰Á
    public static void AddHavingCount()
    {
        boxNum++;
    }

    public static bool HasAllRequiredBox()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        int requiredBoxNum = BoxNum;

        if (requiredBoxNum == BlackBoxManager.requiredBoxNumList[currentScene])
        {
            return true;
        }

        return false;
    }
}