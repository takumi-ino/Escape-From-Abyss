using UnityEngine;

public class NecromanserController : EnemyBaseController
{
    [SerializeField] AudioClip scytheAttackSE;

    public NecromanserController() : base("ネクロマンサー", 9, 5f, 8f, 7f) { }

    private void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void PlayScytheAttackSE()
    {
        weaponAudio.clip = scytheAttackSE;
        weaponAudio.Play();
    }
}
