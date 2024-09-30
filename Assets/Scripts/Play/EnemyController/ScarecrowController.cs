using UnityEngine;

public class ScarecrowController : EnemyBaseController
{
    [SerializeField] AudioClip slapAttackSE;

    public ScarecrowController() : base("�X�P�A�N���E", 6, 5f, 8f, 8f, 5f) { }

    private void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void PlaySlapAttackSE()
    {
        weaponAudio.clip = slapAttackSE;
        weaponAudio.Play();
    }
}
