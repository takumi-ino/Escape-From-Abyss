using UnityEngine;

public class ZombieController : EnemyBaseController
{
    [SerializeField] AudioClip scratchAttackSE;


    // ���O�A�U���́A���s���x�A���s���x�A����͈́A�v���C���[���F�\�͈�
    public ZombieController() : base("�]���r", 5, 3.5f, 5.5f, 8f, 3f) { }

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
