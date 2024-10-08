using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class AcquireChanController : MonoBehaviour
{
    public static AcquireChanController instance;

    private void Awake()
    {
        if (Player.UseUnityChan)
        {
            gameObject.SetActive(false);
            return;
        }

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        m_RigidBody = this.GetComponentInChildren<Rigidbody>();
        m_Animator = this.GetComponentInChildren<Animator>();
        m_MoveSpeed = walkSpeed;
    }

    [SerializeField] private new GameObject camera;            // カメラ
    private GameObject cameraObject;                           // メインカメラへの参照
    private Quaternion cameraRotation;                         // カメラ回転

    // Inspector
    private int maxHp;                                         // 最大HP
    public int playerHp = 35;                                  // HP
    [System.NonSerialized] public int deadCount = 0;
    [SerializeField] private float healPoint;                  // 回復ポイント

    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 3.5f;
    [SerializeField] private float rotateSpeed = 8.0f;
    [SerializeField] private float jumpPower = 300.0f;
    [SerializeField] private float runningStart = 1.0f;

    public float forwardSpeed = 7.0f;                          // 前進速度
    public float backwardSpeed = 2.0f;                         // 後退速度
    public float animatorSpeed = 1.5f;                         // アニメーション再生速度設定
    private float capMaxFallDistance = -30.0f;                 // 落下の深さ（これがないと永遠に落下する）
    private float animationTimer;                              // 体内時計（startRestAnimDurationごとにRestアニメーションを起動）

    // member
    private Rigidbody m_RigidBody = null;
    private Animator m_Animator = null;
    private float m_MoveTime = 0;
    private float m_MoveSpeed = 0.0f;
    private bool m_IsGround = true;

    private float mouseAngle_minX = -90f;                      // マウス角度制限 最小
    private float mouseAngle_maxX = 90f;                       // マウス角度制限 最大
    private float x_sensitivity = 3f;                          // マウスでの視点移動感度＿X軸
    private float y_sensitivity = 3f;                          // マウスでの視点移動感度＿Y軸

    private bool cursorLock = false;                           // カーソル表示
    private bool showCharaControlGui = true;                   // 右上の操作説明GUI表示
    private bool canMove;
    private bool isGrounded = true;

    public static bool isDead { get; private set; }            // 死亡フラグ

    Quaternion characterRotation;                              // キャラクター回転

    public static Vector3 startPos { get; private set; }

    // アニメーション--------------------------------------------------
    private AnimatorStateInfo PlayerState;                     // base layerで使われる、アニメーターの現在の状態の参照

    // コンポーネント---------------------------------------------------------
    private CapsuleCollider capsuleCollider;
    public Rigidbody rigidBody = null;
    public Slider hpSlider = null;

    // 他クラスオブジェクト --------------------------------------------------
    DamageCanvasEffect damageCanvasEffect;

    TextTimingManager textTimingManager;

    BlackBoxManager blackBoxManager;

    private void Start()
    {
        GameState.gameOver = false;
        GameState.gameClear = false;

        isDead = false;
        canMove = true;

        // カメラの回転、キャラクターの回転取得
        cameraRotation = camera.transform.localRotation;
        characterRotation = transform.localRotation;

        hpSlider = GameObject.Find("HpSlider").GetComponent<Slider>();
        hpSlider.value = playerHp;

        maxHp = playerHp;

        capsuleCollider = GetComponent<CapsuleCollider>();
        rigidBody = GetComponent<Rigidbody>();

        cameraObject = GameObject.FindWithTag("MainCamera");
        damageCanvasEffect = GameObject.Find("DamageManager").GetComponent<DamageCanvasEffect>();
        textTimingManager = GameObject.Find("TextTimingManager").GetComponent<TextTimingManager>();

        startPos = transform.position;
    }

    private void Update()
    {
        // Unityちゃんを使用していればfalse
        if (Player.UseUnityChan)
        {
            gameObject.SetActive(false);
            return;
        }

        if (SceneManager.GetActiveScene().name == "Result")
        {
            showCharaControlGui = false;
            hpSlider.enabled = false;

            return;
        }
        // ステージをプレイ中で
        else
        {
            //　且つ死んでいなければ
            if (!isDead)
            {
                showCharaControlGui = true;
                hpSlider.enabled = true;
            }
        }

        if (SceneManager.GetActiveScene().name == "Result") return;

        if (isDead) return;
        if (null == m_RigidBody) return;
        if (null == m_Animator) return;

        // 落下チェック
        if (CheckIfPlayerGetOutOfStage())
        {
            showCharaControlGui = false;
            hpSlider.gameObject.SetActive(false);

            Vector3 position = transform.position;
            position.y = capMaxFallDistance;
            rigidBody.isKinematic = true;
            transform.position = position;

            StartCoroutine(PauseMenu.ShowPauseMenuCanvas());

            StopPlayerMovement();
            DiePlayer();

            return;
        }

        if (GameState.gameClear)
        {
            CharacterVoiceManager.instance.Play(CharacterVoiceManager.Select.GameClear);

            SoundEffectManager.instance.StopFootStep();

            StopPlayerMovement();

            canMove = false;

            return;
        }

        if (GameState.gameOver)
        {
            // 息と足音の SE無効化
            CharacterVoiceManager.instance.Stop(CharacterVoiceManager.Select.Breath);

            SoundEffectManager.instance.StopFootStep();

            StopPlayerMovement();

            StartCoroutine(PauseMenu.ShowPauseMenuCanvas());
            canMove = false;
            return;
        }
        else
        {
            canMove = true;
        }

        OnOffMouseCursor();
        CameraRotationLogic();
        transform.localRotation = characterRotation;           // キャラクターにも同様に値を代入

        // input
        Vector3 vel = m_RigidBody.velocity;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool isMove = ((0 != h) || (0 != v));

        m_MoveTime = isMove ? (m_MoveTime + Time.deltaTime) : 0;
        bool isRun = (runningStart <= m_MoveTime);

        // move speed (walk / run)
        float moveSpeed = isRun ? runSpeed : walkSpeed;
        m_MoveSpeed = isMove ? Mathf.Lerp(m_MoveSpeed, moveSpeed, (8.0f * Time.deltaTime)) : walkSpeed;
        //		m_MoveSpeed = moveSpeed;

        Vector3 inputDir = new Vector3(h, 0, v);
        if (1.0f < inputDir.magnitude) inputDir.Normalize();

        if (0 != h) vel.x = (inputDir.x * m_MoveSpeed);
        if (0 != v) vel.z = (inputDir.z * m_MoveSpeed);

        m_RigidBody.velocity = vel;

        if (isMove)
        {
            // rotation
            float t = (rotateSpeed * Time.deltaTime);
            Vector3 forward = Vector3.Slerp(this.transform.forward, inputDir, t);
            this.transform.rotation = Quaternion.LookRotation(forward);
        }

        m_Animator.SetBool("isMove", isMove);
        m_Animator.SetBool("isRun", isRun);

        // jump
        if (Input.GetButtonDown("Jump") && m_IsGround)
        {
            m_Animator.Play("jump");
            m_RigidBody.AddForce(Vector3.up * jumpPower);
        }

        // quit
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        PlayLandingAnim();
    }

    void DiePlayer()
    {
        if (GameState.gameClear) return;

        isDead = true;
        GameState.gameOver = true;
        playerHp = 0;
        hpSlider.value = playerHp;

        instance.deadCount++;
    }

    bool CheckIfPlayerGetOutOfStage()
    {
        // ステージ外に落下したら死亡
        return transform.position.y < capMaxFallDistance;
    }

    private void PlayLandingAnim()
    {
        // check ground
        float rayDistance = 0.3f;
        Vector3 rayOrigin = (this.transform.position + (Vector3.up * rayDistance * 0.5f));
        bool ground = Physics.Raycast(rayOrigin, Vector3.down, rayDistance, LayerMask.GetMask("Default"));
        if (ground != m_IsGround)
        {
            m_IsGround = ground;

            // landing
            if (m_IsGround)
            {
                m_Animator.Play("landing");
            }
        }
    }

    private void StopPlayerMovement()
    {
        // ※　transform.positionを使ってオブジェクトの位置を変更すると、物理エンジンとの整合性が崩れることがある
        rigidBody.MovePosition(new Vector3
            (
                transform.position.x,
                capMaxFallDistance,
                transform.position.z
            ));

        // プレイヤーの回転も同時に制御
        rigidBody.MoveRotation(Quaternion.identity);
    }

    void OnOffMouseCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            cursorLock = false;                         // エスケープでカーソル表示                
        else if (Input.GetMouseButtonDown(0))
            cursorLock = true;                          // 左クリックでカーソルロック

        if (cursorLock)
            Cursor.lockState = CursorLockMode.Locked;   // Lock
        else if (!cursorLock)
            Cursor.lockState = CursorLockMode.None;     // None
    }

    void CameraRotationLogic()
    {
        float xRot = Input.GetAxis("Mouse X") * y_sensitivity;  // マウスX軸に Y方向の速度を掛ける
        float yRot = Input.GetAxis("Mouse Y") * x_sensitivity;  // マウスY軸に X方向の速度を掛ける

        cameraRotation *= Quaternion.Euler(-yRot, 0, 0);        // カメラ左右の動き
        characterRotation *= Quaternion.Euler(0, xRot, 0);      // カメラ上下の動き

        cameraRotation = ClampRotation(cameraRotation);         // カメラの動く範囲を制限した値を
        camera.transform.localRotation = cameraRotation;        // 実際にカメラの回転軸に代入
    }

    Quaternion ClampRotation(Quaternion q)
    {
        q.w = 1;

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, mouseAngle_minX, mouseAngle_maxX);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);
        return q;
    }
}