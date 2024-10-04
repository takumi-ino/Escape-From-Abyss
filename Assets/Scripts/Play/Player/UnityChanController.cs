using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


namespace UnityChan
{
    // 必要なコンポーネントの列記
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]

    public class UnityChanController : MonoBehaviour
    {
        public static UnityChanController instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        [SerializeField] private new GameObject camera;            // カメラ
        private GameObject cameraObject;                           // メインカメラへの参照
        private Quaternion cameraRotation;                         // カメラ回転

        // Audio　＆　Clip --------------------------------------------------
        [SerializeField] private AudioSource footStepSource;       // 足音用AudioSource
        [SerializeField] private AudioClip footStepClip;           // 足音用Clip

        // ステータス --------------------------------------------------
        private int maxHp;                                         // 最大HP
        public int playerHp = 35;                                  // HP
        [System.NonSerialized] public int deadCount = 0;
        [SerializeField] private float healPoint;                  // 回復ポイント
        [SerializeField] private float walkSpeed = 2.0f;           // 歩行速度
        [SerializeField] private float runSpeed = 3.0f;            // 走る速度
        [SerializeField] private float rotateSpeed = 8.0f;         // 旋回速度

        public float forwardSpeed = 7.0f;                          // 前進速度
        public float backwardSpeed = 2.0f;                         // 後退速度
        public float jumpPower = 3.0f;                             // ジャンプ力
        public float animatorSpeed = 1.5f;                         // アニメーション再生速度設定
        private float capMaxFallDistance = -30.0f;                 // 落下の深さ（これがないと永遠に落下する）
        private float animationTimer;                              // 体内時計（startRestAnimDurationごとにRestアニメーションを起動）
        private float startRestAnimDuration = 5.0f;                // 5秒ごとにRestアニメーションを起動
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
        private Animator animator = null;

        // アニメーターの各ステートへの参照
        static int idleState = Animator.StringToHash("Base Layer.Idle");
        static int runState = Animator.StringToHash("Base Layer.Run");
        static int jumpState = Animator.StringToHash("Base Layer.Jump");
        static int restState = Animator.StringToHash("Base Layer.Rest");

        // コンポーネント---------------------------------------------------------
        private CapsuleCollider capsuleCollider;
        public Rigidbody rigidBody = null;
        public Slider hpSlider = null;

        // 他クラスオブジェクト --------------------------------------------------
        DamageCanvasEffect damageCanvasEffect;

        TextTimingManager textTimingManager;

        BlackBoxManager blackBoxManager;

        public static Canvas finishGameButtonCanvas { get; set; }

        //　--------------------------------------------------

        void Start()
        {
            // ゲームクリアまたはゲームオーバー時に表示するキャンバス
            finishGameButtonCanvas = GetComponent<Canvas>();
            finishGameButtonCanvas = GameObject.Find("GameFinishedButtonCanvas").GetComponent<Canvas>();
            finishGameButtonCanvas.enabled = false;

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
            animator = GetComponent<Animator>();

            cameraObject = GameObject.FindWithTag("MainCamera");
            damageCanvasEffect = GameObject.Find("DamageManager").GetComponent<DamageCanvasEffect>();
            textTimingManager = GameObject.Find("TextTimingManager").GetComponent<TextTimingManager>();

            startPos = transform.position;
        }


        void Update()
        {
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

            if (isDead) return;

            if (GameState.gameClear)
            {
                CharacterVoiceManager.instance.Play(CharacterVoiceManager.Select.GameClear);

                footStepSource.Stop();

                StopPlayerMovement();

                canMove = false;

                return;
            }

            if (GameState.gameOver)
            {
                // 息と足音の SE無効化
                CharacterVoiceManager.instance.Stop(CharacterVoiceManager.Select.Breath);

                footStepSource.Stop();

                StopPlayerMovement();

                // アニメーション無効化
                animator.enabled = false;

                StartCoroutine(ShowFinishGameButtonCanvas());
                canMove = false;
                return;
            }
            else
            {
                animator.enabled = true;
                canMove = true;
            }

            OnOffMouseCursor();
            CameraRotationLogic();
            transform.localRotation = characterRotation;           // キャラクターにも同様に値を代入
        }


        void FixedUpdate()
        {
            if (SceneManager.GetActiveScene().name == "Result") return;

            if (isDead) return;

            // 落下チェック
            if (CheckIfPlayerGetOutOfStage())
            {
                showCharaControlGui = false;
                hpSlider.gameObject.SetActive(false);

                Vector3 position = transform.position;
                position.y = capMaxFallDistance;
                rigidBody.isKinematic = true;
                transform.position = position;

                StartCoroutine(ShowFinishGameButtonCanvas());

                StopPlayerMovement();
                DiePlayer();

                return;
            }

            // 画面上のGUIスイッチ　バックスペース
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                showCharaControlGui = !showCharaControlGui;
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(h, 0, v);

            AddPlayerMoveForce(out h, out v, out movement);

            JumpCheck();
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


        bool CheckIfPlayerGetOutOfStage()
        {
            // ステージ外に落下したら死亡
            return transform.position.y < capMaxFallDistance;
        }


        void AddPlayerMoveForce(out float h, out float v, out Vector3 movement)
        {
            movement = MakeMoveDirectionForce(out h, out v);

            // キャラクターが動ける状態 ＆ 移動している状態
            if (canMove && movement != Vector3.zero)
            {
                movement = MakeAnimationMove(h, v, movement);

                rigidBody.useGravity = true;

                float speed = (v > 0) ? forwardSpeed : backwardSpeed;

                rigidBody.MovePosition(transform.position + (movement * speed * Time.deltaTime));
            }
            else
            {
                animator.SetFloat("Speed", 0);

                // 時々　Restアニメーションを実行
                if (PlayerState.fullPathHash == idleState)
                {
                    int rate = Random.Range(0, 100);

                    if (rate > 20)
                    {
                        animator.SetBool("Rest", true);
                    }
                }
            }
        }

        private Vector3 MakeAnimationMove(float h, float v, Vector3 movement)
        {
            animator.SetFloat("Direction", h);

            if (v > 0.1)
            {
                animator.SetFloat("Speed", movement.magnitude);
            }

            else if (v < -0.1)
            {
                animator.SetFloat("Speed", v);
            }

            animator.speed = animatorSpeed;

            // ジャンプ処理
            PlayerState = animator.GetCurrentAnimatorStateInfo(0);

            return movement;
        }

        Vector3 MakeMoveDirectionForce(out float h, out float v)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            // カメラの視点から見た入力方向を計算
            Vector3 cameraForward = Vector3.Scale(cameraObject.transform.forward, new Vector3(1, 0, 1)).normalized;
            // カメラの前方向と右方向を基に移動方向を計算
            Vector3 movement = cameraForward * v + cameraObject.transform.right * h;

            // 移動方向を正規化
            if (movement.magnitude > 1)
            {
                movement.Normalize();
            }

            return movement;
        }


        public void TakeHit(int damage)
        {
            if (playerHp <= 0 && !GameState.gameOver)   // HPが０になったら
            {
                CharacterVoiceManager.instance.Play(CharacterVoiceManager.Select.Dead);

                SoundEffectManager.instance.Play(SoundEffectManager.Select.ImpactSeWhenPlayerDead);

                DiePlayer();

                showCharaControlGui = false;
            }
            else
            {
                playerHp -= damage;

                CharacterVoiceManager.instance.Play(CharacterVoiceManager.Select.Damage);
            }

            HpSliderUpdate();
            damageCanvasEffect.IgniteDamagedEffect();
        }


        public void HpSliderUpdate() { hpSlider.value = playerHp; }  // HPの更新関数


        void DiePlayer()
        {
            if (GameState.gameClear) return;

            isDead = true;
            GameState.gameOver = true;
            playerHp = 0;
            hpSlider.value = playerHp;

            instance.deadCount++;
        }


        void JumpAnimation()
        {
            animator.SetBool("Jump", true);

            animator.SetFloat("JumpHeight", 1);       // JumpHeight:JUMP00でのジャンプの高さ（0〜1）
            animator.SetFloat("GravityControl", 0);   // GravityControl:1⇒ジャンプ中（重力無効）、0⇒重力有効
        }


        public void JumpCheck()
        {
            //  アイドル状態の時 または 走ってる状態の時
            if (PlayerState.fullPathHash == idleState ||
                PlayerState.fullPathHash == runState)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    JumpAnimation();

                    rigidBody.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                    rigidBody.useGravity = true;
                }
                else
                    animator.SetBool("Jump", false);
            }
            //　ジャンプしている状態の時
            else if (PlayerState.fullPathHash == jumpState)
            {
                if (!animator.IsInTransition(0))
                {
                    animator.SetFloat("JumpHeight", 0.5f);
                    animator.SetFloat("GravityControl", 0.5f);

                    float jumpHeight = animator.GetFloat("JumpHeight");
                    float gravityControl = animator.GetFloat("GravityControl");

                    if (gravityControl > 0)
                    {
                        rigidBody.useGravity = false;              //　ジャンプ中の重力の影響を切る
                    }

                    animator.SetBool("Jump", false);
                }
            }
            else if (PlayerState.fullPathHash == restState)        //　一定時間アイドル状態の時
            {
                if (!animator.IsInTransition(0))
                    animator.SetBool("Rest", false);
            }
        }


        IEnumerator LoadSecondStage()
        {
            yield return new WaitForSeconds(3);
        }

        // 表示---------------------------------------------------------------------------------------

        // Title, CharacterSelect, Retry の３つのメニュー
        IEnumerator ShowFinishGameButtonCanvas()
        {
            yield return new WaitForSeconds(1.75f);
            finishGameButtonCanvas.enabled = true;
        }


        // 当たり判定----------------------------------------------------------
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Goal")           // ゴールエリアに入ったら
            {
                GameState.gameClear = true;
            }
            else if (other.gameObject.tag == "HealItem")  // ヒールアイテムを取得したら
            {
                if (maxHp > playerHp)                   　// HPが減っている場合のみ回復
                {
                    HealItem healItem = other.GetComponent<HealItem>();

                    healItem.HeaPlayerlHp(ref playerHp, maxHp);

                    SoundEffectManager.instance.Play(SoundEffectManager.Select.HealPlayer);

                    HpSliderUpdate();
                }
            }
            else if (other.gameObject.tag == "BlackBox")  // ブラックボックスを取得したら
            {
                SoundEffectManager.instance.Play(SoundEffectManager.Select.GetBlackBox);

                Destroy(other.gameObject);
                BlackBoxManager.AddHavingCount();

                // 必要数揃ったら
                if (BlackBoxManager.HasAllRequiredBox())
                {
                    GoalPosController.isLightUpDestinationPoint = true;

                    StartCoroutine(textTimingManager.ShowAimForThePlaceText());
                }
            }
        }

        void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }

        void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }

        //　Animationから --------------------------------------------------

        public void PlayFootStepSE()
        {
            //if (!isGrounded) return;

            footStepSource.clip = footStepClip;
            footStepSource.Play();
        }

        public void PlayBreathSE()
        {
            //if (!isGrounded) return;

            CharacterVoiceManager.instance.Play(CharacterVoiceManager.Select.Breath);
        }

        //　画面上のGUI --------------------------------------------------------
        void OnGUI()
        {
            if (showCharaControlGui)
            {
                GUI.Box(new Rect(Screen.width - 260, 10, 250, 90), "Interaction");
                GUI.Label(new Rect(Screen.width - 245, 30, 250, 30), "Up/Down Arrow : Go Forwald/Go Back");
                GUI.Label(new Rect(Screen.width - 245, 50, 250, 30), "Left/Right Arrow : Go Left/Go Right");
                GUI.Label(new Rect(Screen.width - 245, 70, 250, 30), "Hit Space key while Running : Jump");
            }
        }

        // ステージをリトライするときに開始直後の状態に戻す
        public void ResetCurrentStatus()
        {
            transform.position = DestinationPosSceneTransition.transitionPoint_stage1;
            playerHp = maxHp;
            finishGameButtonCanvas.enabled = false;
            TextTimingManager.instance.gameOverText.SetActive(false);
            GameState.gameOver = false;

            isDead = false;
        }
    }
}