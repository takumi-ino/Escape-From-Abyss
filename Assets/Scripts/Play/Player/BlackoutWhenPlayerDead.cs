using UnityChan;
using UnityEngine;
using UnityEngine.UI;

public class BlackoutWhenPlayerDead : MonoBehaviour
{
    Image image;
    private void Start()
    {
        image = gameObject.GetComponentInChildren<Image>();
        Color color = image.color;
        color.a = 0f;
        image.color = color;
    }
    void Update()
    {
        Show();
    }

    void Show()
    {
        if (UnityChanController.isDead)
        {
            Color color = image.color;
            color.a = 0.65f;
            image.color = color;
        }
        else
        {
            Color color = image.color;
            color.a = 0f;
            image.color = color;
        }
    }
}