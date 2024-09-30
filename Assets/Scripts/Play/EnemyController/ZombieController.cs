using UnityEngine;

public class ZombieController : EnemyBaseController
{
    [SerializeField] AudioClip scratchAttackSE;


    // 名前、攻撃力、歩行速度、走行速度、巡回範囲、プレイヤー視認可能範囲
    public ZombieController() : base("ゾンビ", 5, 3.5f, 5.5f, 8f, 3f) { }

    private void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void PlayScratchAttackSE()
    {
        weaponAudio.clip = scratchAttackSE;
        weaponAudio.Play();
    }
}
