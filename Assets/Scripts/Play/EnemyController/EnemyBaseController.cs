using UnityChan;
using UnityEngine;
using UnityEngine.AI;

// ���N���X�B�]���r�ɓ\��t����
// �]���r��e�Ƃ��Ċe�G�l�~�[�̍s�����`
public class EnemyBaseController : MonoBehaviour
{
    protected Animator animator;
    protected NavMeshAgent agent;
    protected Transform myTransform;
    protected Transform targetTransform; // �v���C���[�ʒu

    [SerializeField] protected AudioSource weaponAudio;

    // Manager�N���X����Q�Ƃ���\��������̂Ł@public
    public string Name { get; protected set; }
    public int AttackPoint { get; protected set; } // �U����
    public float WalkSpeed { get; protected set; } // ���s���x
    public float RunSpeed { get; protected set; }  // ���s���x
    public float RandomWanderRadius { get; protected set; } // Wander��Ԃ̂Ƃ��̈ړ��͈�
    public float CanSeePlayerRadius { get; protected set; } // Wander��Ԃ̂Ƃ��̈ړ��͈�


    protected void Awake()
    {
        myTransform = transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        weaponAudio = GetComponent<AudioSource>();
    }

    protected EnemyBaseController(
        string name = "�]���r",           // ���O�A
        int at = 5,                //�U���́A
        float walk_s = 4f,          //���s���x
        float run_s = 6f,           //���s���x�A
        float rnd_wanderRad = 5f,   //����\�͈�
        float canSee_playerRad = 3f)//�v���C���[�F�m�\�͈�
    {
        Name = name;
        AttackPoint = at;
        WalkSpeed = walk_s;
        RunSpeed = run_s;
        RandomWanderRadius = rnd_wanderRad;
        CanSeePlayerRadius = canSee_playerRad;
    }

    public enum STATE
    {
        IDLE,
        CHASE,
        WALK,
        ATTACK,
        DEAD
    };

    protected STATE current_enemy_state = STATE.IDLE;


    protected void Attack()
    {
        Vector3 dis = targetTransform.position - transform.position;

        if (dis.magnitude < 1.5f) // �U����������͈�
            targetTransform.GetComponent<UnityChanController>().TakeHit(AttackPoint);
    }

    protected void OnAttackAnimEnd()
    {

        if (animator.HasState(0, Animator.StringToHash("Idle")))
            animator.SetBool("Idle", true);
    }

    protected float DistanceToPlayer()
    {
        if (GameState.gameOver)
        {
            return Mathf.Infinity;
        }

        return Vector3.Distance(targetTransform.position, myTransform.position);
    }


    protected bool CanSeePlayer()
    {
        if (DistanceToPlayer() <= CanSeePlayerRadius)
        {
            return true;
        }

        return false;
    }

    protected bool ReachToPlayer()
    {
        if (DistanceToPlayer() <= agent.stoppingDistance)
        {
            return true;
        }

        return false;
    }

    private void SetAnimationState(STATE state)
    {
        // ���ׂẴA�j���[�V�����̃u�[���l��false�ɐݒ�
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetBool("Attack", false);

        // �V������ԂɊ�Â��ēK�؂ȃA�j���[�V�����̃u�[���l��true�ɐݒ�
        switch (state)
        {
            case STATE.IDLE:
                animator.SetBool("Idle", true);
                break;
            case STATE.WALK:
                animator.SetBool("Walk", true);
                break;
            case STATE.CHASE:
                animator.SetBool("Run", true);
                break;
            case STATE.ATTACK:
                animator.SetBool("Attack", true);
                break;
        }

        // ���݂̃A�j���[�V�����̏�Ԃ��X�V
        current_enemy_state = state;
    }


    protected void Update()
    {
        switch (current_enemy_state)
        {
            // �A�C�h����Ԃ̂Ƃ�
            case STATE.IDLE:

                agent.enabled = false;

                if (!ReachToPlayer())
                {
                    // �v���C���[�𔭌����Ă���Βǂ�������
                    if (CanSeePlayer())
                        current_enemy_state = STATE.CHASE;

                    // �v���C���[�𔭌����Ă��Ȃ���Ίm���ŏ�����n�߂�
                    else 
                        current_enemy_state = STATE.WALK;
                }
                // �v���C���[�ɒǂ������ꍇ
                else
                {
                    current_enemy_state = STATE.ATTACK;
                    break;
                }
                break;

            // �f�r���Ă���Ƃ�
            case STATE.WALK:

                agent.enabled = true;

                // �v���C���[�������Ă��Ȃ����
                if (!agent.hasPath)
                {
                    agent.speed = WalkSpeed;

                    SetAnimationState(STATE.WALK);

                    float x = transform.position.x + Random.Range(-RandomWanderRadius, RandomWanderRadius);    // �������Ƀ����_���ړ�
                    float z = transform.position.z + Random.Range(-RandomWanderRadius, RandomWanderRadius);    // �������Ƀ����_���ړ�
                    Vector3 newPos = new Vector3(x, transform.position.y, z);
                    //agent.SetDestination(newPos);
                }

                if (CanSeePlayer())
                {
                    current_enemy_state = STATE.CHASE;
                    break;
                }
                if (Random.Range(0, 5000) < 5)
                {
                    //agent.ResetPath();
                    current_enemy_state = STATE.IDLE;
                    break;
                }

                break;

            // �v���C���[��ǂ������Ă��鎞
            case STATE.CHASE:

                agent.enabled = true;

                if (!GameState.gameOver)
                {
                    SetAnimationState(STATE.CHASE);

                    agent.SetDestination(targetTransform.position);
                    agent.speed = RunSpeed;

                }
                // �v���C���[������������f�r���n�߂�   �Q�[���I�[�o�[�̏ꍇ�͉i���ɜf�r���n�߂�
                else if (GameState.gameOver || !CanSeePlayer())
                {

                    current_enemy_state = STATE.IDLE;
                    return;
                }

                if (ReachToPlayer())
                    current_enemy_state = STATE.ATTACK;
                break;

            // �v���C���[���U�����Ă��鎞
            case STATE.ATTACK:

                if (!GameState.gameOver)
                {
                    agent.enabled = false;

                    transform.LookAt(new Vector3(targetTransform.position.x, transform.position.y, targetTransform.position.z));

                    SetAnimationState(STATE.ATTACK);

                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 &&
                        animator.IsInTransition(0))
                    {
                        animator.SetBool("Attack", false);
                    }
                }
                else
                {
                    current_enemy_state = STATE.WALK;
                    return;
                }

                if (!ReachToPlayer())
                    current_enemy_state = STATE.CHASE;      // ���ꂽ��܂��ǂ������n�߂�
                break;
        }
    }
}