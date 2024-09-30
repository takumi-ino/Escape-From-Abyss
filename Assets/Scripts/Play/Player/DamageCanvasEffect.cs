using UnityEngine;
using UnityEngine.UI;

public class DamageCanvasEffect : MonoBehaviour
{
    [SerializeField] Image DamageImg;

    private float effectRate = 0.2f;
    void Start()
    {
        DamageImg.color = Color.clear;
    }


    void Update()
    {
        DamageImg.color = Color.Lerp(DamageImg.color, Color.clear, Time.deltaTime);
    }

    public void IgniteDamagedEffect()
    {
        DamageImg.color = new Color(effectRate, 0, 0, effectRate);

        if (effectRate < 0.6f)
            AddEffectRate();
    }

    void AddEffectRate()
    {
        effectRate += 0.1f;
    }
}