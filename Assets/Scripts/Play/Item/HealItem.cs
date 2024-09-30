using UnityEngine;

public class HealItem : MonoBehaviour
{

    int healPoint = 5;

    public void HeaPlayerlHp(ref int hp, int maxHp)
    {

        hp += Mathf.Max(maxHp - hp, healPoint);

        ItemSoundManager.instance.Play(ItemSoundManager.Select.HealPlayer);
        Destroy(gameObject);
    }
}