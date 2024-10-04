using UnityChan;
using UnityEngine;

public class CyberMonsterController : EnemyBaseController
{
    [SerializeField] AudioClip swordAttackSE;
    [SerializeField] AudioClip gunAttackSE;
    public CyberMonsterController() : base("サイバー", 4, 4f, 7f, 5f) { }

    private void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void PlaySwordAttackSE()
    {
        weaponAudio.clip = swordAttackSE;
        weaponAudio.Play();
    }

    public void PlayGunAttackSE()
    {
        weaponAudio.clip = gunAttackSE;
        weaponAudio.Play();
    }
}

// ctrl + r + m