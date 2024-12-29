using System.Collections;
using System.Linq;
using UnityEngine;
// influenced by Unity
[RequireComponent(typeof(CharacterController))]
public class DemoCharacter : MonoBehaviour
{
    [SerializeField]
    public Camera cam;
    public GameObject g_RayCamera;
    [SerializeField]
    float gravityModifier = 2f;
    [SerializeField]
    public float walkSpeed = 5f;
    public float walkMaxSpeed = 5f;
    [SerializeField]
    public float runSpeed = 10f;
    public float runMaxSpeed = 10f;
    [SerializeField]
    public float jumpSpeed = 10f;
    [SerializeField]
    float landingForce = 10f;

    [SerializeField]
    float mouseXSensitivity = 2f;
    [SerializeField]
    float mouseYSensitivity = 2f;
    public LayerMask cameraCollision;   // 카메라 충돌 체크 레이어
    public float cameraPosZ;
    // 스무스 이동에 필요한 변수들
    private float currentZ = -4f;  // 현재 Z 위치
    private float velocityZ = 0f;  // Z 위치 변경 속도
    public float smoothTime = 0.1f;  // 스무스 이동 시간
    public float minDistanceFromWall = 1f;  // 벽과의 최소 거리
    public Transform natural_Movement_After_Attack;//자연스러운 공격이동
    float charControlenabelTime = 0;

    public GameObject cameraClime;
    CharacterController charControl;
    AttackAndCombo andCombo;
    Skill skill;

    Quaternion characterTargetRot;
    Quaternion cameraTargetRot;

    bool isWalking = true;
    Vector2 moveInput = Vector2.zero;
    Vector3 move = Vector3.zero;
    bool jumpPressed = false;

    CollisionFlags collisionFlags;
    public QuestManager questManager;
    Animator animator;
    public static bool noneSpeed;
    public GameObject attackSize;
    int attackCount;
    int kickAttackCount;
    public float speed;
    [Header("오디오")]
    public AudioSource warkeSound;
    public AudioClip[] warkeClip;
    public AudioClip[] dunjeon_warkeClip;
    public AudioClip[] swimClip;
    public AudioClip inWaterClip;
    public enum FloorArer { Temple, Dungeon, BoseRoom };
    public FloorArer floorArer;


    int warkTypeAudioCount; //warkTypeAudioCount 가 증가 하면 warkeClip배열 안에있는 클립을 차례대로 수집
    int warkDunjeongTypeAudioCount; //warkTypeAudioCount 가 증가 하면 warkeClip배열 안에있는 클립을 차례대로 수집
    int swmmingCount;

    public AudioSource attackSound;
    public AudioClip[] attackClip;


    int attackAudioCount;
    Stamina stamina;
    bool exhaustion;//탈진 상태(스태미너가 0이 될경우)
    public PlayerUI playerUI;
    public float slow = 0;

    //cc
    public bool slowBool;
    public GameObject[] abnormalstatus;//상태이상(0:저주 = 달리기 안됌 1:슬로우 = 느려짐)


    [SerializeField] private Transform waterSurface; // 물 표면의 Transform
    private float halfSubmergeHeight = -1.2f; // 캐릭터의 잠기는 깊이 (조정 가능)
    public bool isInWater = false; // 물 안에 있는 상태를 확인하기 위한 변수
    public bool stayClimeBool = false;
    [SerializeField]
    private LayerMask loadCheckLayerMask;
    [SerializeField]
    private LayerMask floorCheckLayerMask;
    SkillItemFusion skillItemFusion;
    public SkillTree skillTree;
    public float walkSpeeds = 0;
    public float runSpeeds = 0;
    public GameObject startMenu;
    void Start()
    {
        /*        if (cam == null)
                {
                    cam = Camera.main;
                }*/
        skill = GetComponent<Skill>();
        skillItemFusion = GetComponent<SkillItemFusion>();
        charControl = GetComponent<CharacterController>();
        andCombo = GetComponent<AttackAndCombo>();
        animator = GetComponent<Animator>();
        stamina = GetComponent<Stamina>();
        characterTargetRot = transform.localRotation;
        cameraTargetRot = cam.transform.localRotation;
    }

    void GetMoveInput(out float speed)
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveInput = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (moveInput.sqrMagnitude > 1)
        {
            moveInput.Normalize();
        }
        if (!slowBool)
        {
            isWalking = !Input.GetKey(KeyCode.LeftShift);
            abnormalstatus[0].SetActive(false);
        }
        else
        {
            abnormalstatus[0].SetActive(true);
        }
        // 캐릭터가 실제로 움직이고 있는지 체크
        if (moveInput.magnitude > 0.01f) // 움직임이 있는 경우
        {
            // 걷고 있는가? 그렇다면 걷기 속도(walkSpeed), 그렇지 않으면 달리기 속도(runSpeed)
            speed = isWalking ? walkSpeed : runSpeed;
        }
        else
        {
            // 움직임이 없을 때 속도를 0으로 설정
            speed = 0f;
        }

        // 애니메이션 업데이트
        if (speed > 0f)
        {

            if (isWalking)
            {
                animator.SetFloat("Walk flaot", moveInput.magnitude * walkSpeed); // 걷는 애니메이션
                animator.SetFloat("Run float", 0f); // 달리기 애니메이션 초기화
            }
            else
            {
                if (stamina.stamina >= 0)
                {
                    stamina.stamina -= Time.deltaTime * 50;
                }
                animator.SetFloat("Run float", moveInput.magnitude * runSpeed); // 달리기 애니메이션
                animator.SetFloat("Walk flaot", 0f); // 걷기 애니메이션 초기화
            }
        }
        else
        {
            // 속도가 0일 때 애니메이션 파라미터 초기화
            animator.SetFloat("Walk flaot", 0f);
            animator.SetFloat("Run float", 0f);
        }

    }

    void CameraLook()
    {
        RaycastHit hit;
        float rayDistance = 4f;

        // 4방향에 레이 쏘기 (전방, 후방, 좌측, 우측)
        Vector3[] directions = new Vector3[]
        {
        g_RayCamera.transform.forward,    // 전방
        -g_RayCamera.transform.forward,   // 후방
        g_RayCamera.transform.right,      // 우측
        -g_RayCamera.transform.right      // 좌측
        };

        float closestHitDistance = rayDistance;

        // 각 방향에 대해 레이를 쏘고 가장 가까운 벽까지의 거리를 계산
        foreach (Vector3 direction in directions)
        {
            if (Physics.Raycast(g_RayCamera.transform.position, direction, out hit, rayDistance, cameraCollision))
            {
                // 충돌 지점까지의 거리 계산
                closestHitDistance = Mathf.Min(closestHitDistance, hit.distance);
            }
        }

        // 벽과의 최소 거리 확보
        float targetZ = Mathf.Clamp(-closestHitDistance + minDistanceFromWall, -3f, -minDistanceFromWall);

        // 현재 위치에서 목표 위치로 부드럽게 이동
        currentZ = Mathf.SmoothDamp(currentZ, targetZ, ref velocityZ, smoothTime);

        // 카메라 위치 업데이트
        g_RayCamera.transform.localPosition = new Vector3(0, 3.5f, currentZ);

        // 마우스 입력 처리
        float mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseYSensitivity;

        // 카메라 회전 계산
        characterTargetRot *= Quaternion.Euler(0f, mouseX, 0f);
        cameraTargetRot *= Quaternion.Euler(-mouseY, 0f, 0f);
        cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot);

        // 회전 적용
        transform.localRotation = characterTargetRot;
        cam.transform.localRotation = cameraTargetRot;
    }

    void CharControlEnabelControl()
    {
        if (!charControl.enabled)
        {
            charControlenabelTime += Time.deltaTime;
        }
        else
        {
            charControlenabelTime = 0;
        }
        if (charControlenabelTime >= 0.5f)
        {
            charControl.enabled = true;
        }
    }

    void Update()
    {
        if (!startMenu.gameObject.activeSelf)
        {
            CharControlEnabelControl();
            Stamnina();
            SizeSpeed();
            if (!questManager.conversationStatus)
            {
                if (!playerUI.uiOpen && !noneSpeed) CameraLook();
                jumpPressed = Input.GetKeyDown(KeyCode.Space);
            }
        }

    }
    public float speedItem(float speed, string speedtype)
    {
        if (speedtype == "Walk")
        {
            walkSpeeds = speed + (skillItemFusion.speedHap.Take(6).Sum() + skillTree.walkSpeedStack);
            return walkSpeeds;
        }
        else if (speedtype == "Run")
        {
            runSpeeds = speed + (skillItemFusion.speedHap.Take(6).Sum() + skillTree.runspeedStack);
            return runSpeeds;
        }
        else
        {
            return 0;
        }
    }
    public void SizeSpeed()
    {
        if (moveInput.magnitude >= 0.1f)
        {
            if (transform.localScale.x == 1f)
            {
                if (skill.b_SkillAttackCorutin[0])
                {
                    walkSpeed = speedItem(5 * 1.5f, "Walk");
                    runSpeed = speedItem(10 * 1.5f, "Run");
                }
                else
                {
                    walkSpeed = speedItem(5, "Walk");
                    runSpeed = speedItem(10, "Run");
                }

            }
            else if (transform.localScale.x == 2f)
            {

                if (!isInWater)
                {
                    if (skill.b_SkillAttackCorutin[0])
                    {
                        walkSpeed = speedItem(10 * 1.5f, "Walk");
                        runSpeed = speedItem(20 * 1.5f, "Run");
                    }
                    else
                    {
                        walkSpeed = speedItem(10, "Walk");
                        runSpeed = speedItem(20, "Run");
                    }
                }
                else
                {
                    if (skill.b_SkillAttackCorutin[0])
                    {
                        walkSpeed = speedItem(7 * 1.5f, "Walk");
                        runSpeed = speedItem(14 * 1.5f, "Run");
                    }
                    else
                    {
                        walkSpeed = speedItem(7, "Walk");
                        runSpeed = speedItem(14, "Run");
                    }
                }
            }
        }
    }
    void Stamnina()
    {
        if (stamina.stamina < stamina.staminaMax)
        {
            stamina.stamina += Time.deltaTime * 10;
        }
        if (andCombo.hitFlyBackBool)
        {
            speed = 0f;
        }
        else
        {
            if (moveInput.magnitude >= 0.1f)
            {
                speed = 4f;
            }

        }
        if (stamina.stamina < 0)
        {

            exhaustion = true;
        }
        else
        {
            exhaustion = false;
        }

    }
    //벽올라가기,움직임 제어
    private void FixedUpdate()
    {
        if (noneSpeed)
        {
            animator.SetFloat("Run float", 0);

            animator.SetFloat("Walk flaot", 0);
        }
        else
        {
            animator.SetFloat("Run float", speed);

            animator.SetFloat("Walk flaot", speed);
        }
        Move();
        if (b_clime)
        {
            transform.position = new Vector3(transform.position.x, 26f, transform.position.z);
            Invoke("InvokeClimeFalse", 1f);
        }
    }
    //달리기,수영
    bool isClimY;
    void Move()
    {
        if (!andCombo.hitFlyBackBool)
        {
            if (!noneSpeed && !questManager.conversationStatus)
            {
                if (!playerUI.uiOpen)
                {
                    GetMoveInput(out speed);
                    if (exhaustion)//탈진 상태
                    {
                        speed = 4;
                    }
                    if (isInWater)
                    {
                        // 물 표면의 높이
                        float waterHeight = waterSurface.position.y;
                        // 캐릭터가 하반신까지만 잠기는 목표 높이
                        float targetHeight = waterHeight - halfSubmergeHeight;

                        // 현재 캐릭터의 위치
                        float currentHeight = transform.position.y;

                        // 캐릭터가 물에 떠 있는 느낌을 주기 위해 Y 위치를 부드럽게 조정
                        if (currentHeight < targetHeight - 0.1f)
                        {
                            // 캐릭터가 목표 높이보다 낮다면 상승
                            move.y = Mathf.Lerp(move.y, 0.5f, Time.fixedDeltaTime * 10f); // 부드러운 상승 효과
                        }
                        else if (currentHeight > targetHeight + 0.1f)
                        {
                            // 캐릭터가 목표 높이보다 높다면 하락
                            move.y = Mathf.Lerp(move.y, -0.5f, Time.fixedDeltaTime * 2f); // 부드러운 하락 효과
                        }
                        else
                        {
                            isClimY = true;
                            // 목표 높이에 가까우면 가라앉지 않도록 유지
                            move.y = Mathf.Lerp(move.y, 0f, Time.fixedDeltaTime * 2f);
                        }
                    }
                    Vector3 desiredMove = transform.forward * moveInput.y + transform.right * moveInput.x;
                    RaycastHit hitInfo;
                    Physics.SphereCast(transform.position, charControl.radius, Vector3.down, out hitInfo,
                                       charControl.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
                    desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
                    move.x = desiredMove.x * speed;
                    move.z = desiredMove.z * speed;


                    if (charControl.isGrounded)
                    {
                        move.y = -landingForce;

                        if (jumpPressed)
                        {
                            move.y = jumpSpeed;
                            jumpPressed = false;
                        }
                    }
                    else
                    {
                        move += Physics.gravity * gravityModifier * Time.fixedDeltaTime;
                    }
                    collisionFlags = charControl.Move(move * Time.fixedDeltaTime);
                }
                else
                {
                    animator.SetFloat("Run float", 0);

                    animator.SetFloat("Walk flaot", 0);
                }
            }
        }
    }
    public void AttackAniTrue()
    {
        noneSpeed = true;
    }

    public void AttackAniFasle()
    {
        noneSpeed = false;
        AttackAndCombo.comboAttakccount += 1;
    }
    public void NoneSpeedTrue()
    {
        noneSpeed = true;
    }
    public void NoneSpeedFalse()
    {
        noneSpeed = false;
        DeAttackCollider();
    }
    public void KickAttackAniFasle()
    {
        noneSpeed = false;
        AttackAndCombo.comboKickCount += 1;
    }
    public void InitializeComboStack()
    {
        noneSpeed = false;
        AttackAndCombo.comboAttakccount = 0;

        /* transform.position = transform.position + new Vector3(natural_Movement_After_Attack.transform.position.x, 0
                                                             , natural_Movement_After_Attack.transform.position.z);*/
        transform.position = transform.position;
        charControl.enabled = false;
    }
    public void InitializeKickComboStack()
    {
        noneSpeed = false;
        AttackAndCombo.comboKickCount = 0;
    }
    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, -90f, 90f);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
    public void AttackCollider()
    {
        attackSize.SetActive(true);
    }
    public void DeAttackCollider()
    {
        attackSize.SetActive(false);
    }
    public void WarkeAudio()
    {
        //걷는 효과음         
        if (floorArer == FloorArer.Dungeon || floorArer == FloorArer.BoseRoom && !isInWater)
        {
            warkDunjeongTypeAudioCount += 1;
            if (warkDunjeongTypeAudioCount == 10)
            {
                warkDunjeongTypeAudioCount = 0;
            }
            warkeSound.PlayOneShot(dunjeon_warkeClip[warkDunjeongTypeAudioCount], playerUI.playerSlider.value + 0.2f);
        }
        else if (floorArer == FloorArer.Temple && !isInWater)
        {
            warkTypeAudioCount += 1;
            if (warkTypeAudioCount == 20)
            {
                warkTypeAudioCount = 0;
            }
            warkeSound.PlayOneShot(warkeClip[warkTypeAudioCount], playerUI.playerSlider.value + 0.2f);
        }


    }
    public void Swim()
    {
        if (isInWater)
        {
            swmmingCount += 1;
            if (swmmingCount == 2)
            {
                swmmingCount = 0;
            }
            warkeSound.PlayOneShot(swimClip[swmmingCount], playerUI.playerSlider.value + 0.2f);
        }
    }
    public void AttackAudio(int attackAudioCount)
    {
        //예외적으로 구르기 효과음은 여기서 처리
        //애니메이터 이벤트에서 원하는 효과음 배열 번호를 넣으면 됨
        attackSound.PlayOneShot(attackClip[attackAudioCount], playerUI.playerSlider.value);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // 캐릭터가 오브젝트 위에 있을 때 리턴
        if (collisionFlags == CollisionFlags.Below)
        {
            return;
        }

        // Rigidbody가 없거나 kinematic인 경우 리턴
        if (body == null || body.isKinematic)
        {
            return;
        }

        // 충돌한 오브젝트에 힘을 가함
        body.AddForceAtPosition(charControl.velocity * 0.1f, hit.point, ForceMode.Impulse);

    }


    // 물에 들어갈 때
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;

            SoundManager.PlaySound(inWaterClip);
            // 물 표면의 Transform 자동 할당
            if (waterSurface == null)
            {
                waterSurface = other.transform;
            }

            EnterWater();
        }

    }
    bool FrontRay()//앞이지 확인 하는 레이는 플레이어 앞으로 발사
    {
        RaycastHit hit;
        float rayDistance = 1.5f;
        bool hitCheck = false;
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance, loadCheckLayerMask))
        {
            Debug.Log("앞에 벽");
            hitCheck = true;
        }
        else
        {
            Debug.Log("앞에 벽이 없음");
            hitCheck = false;
        }
        return hitCheck;
    }
    bool FloorRay()//바닥 확인 하는 레이는 플레이어 아래로 발사
    {
        RaycastHit hit;
        float rayDistance = 2.3f;
        bool hitCheck = false;
        Debug.DrawRay(transform.position, -transform.up * rayDistance, Color.red);
        if (Physics.Raycast(transform.position, -transform.up, out hit, rayDistance, floorCheckLayerMask))
        {
            Debug.Log("바닥에 레이가 닿음");
            hitCheck = true;
        }
        else
        {
            Debug.Log("바닥에 레이가 닿지 않음");
            hitCheck = false;
        }
        return hitCheck;
    }
    IEnumerator ClimY()
    {
        float climTime = 0;
        while (climTime <= 2.5f)
        {
            climTime += Time.deltaTime;
            Debug.Log(climTime);
            charControl.skinWidth = 7f;
            yield return null;
        }
        charControl.skinWidth = 0.08f;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Load") && isInWater && FrontRay() && !FloorRay())
        {
            if (Input.GetKeyUp(KeyCode.Space) && !stayClimeBool && gameObject.transform.position.y >= 19.5f)
            {
                Debug.Log("123");
                charControl.enabled = false;
                StartCoroutine(ClimY());
                animator.SetBool("ClimeBool", true);
                noneSpeed = true;
                Invoke("InvokeNoneSpeed", 2.5f);
                stayClimeBool = true;
            }
        }
    }
    public bool b_clime;
    public void ClimePosY()
    {
        b_clime = true;
        noneSpeed = false;
    }
    public void InvokeClimeFalse()
    {
        b_clime = false;
        stayClimeBool = false;
        charControl.enabled = true;
    }
    public void InvokeNoneSpeed()
    {
        animator.SetBool("ClimeBool", false);
        noneSpeed = false;
        b_clime = false;
    }
    // 물에서 나올 때
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            ExitWater();
        }
    }
    // 물에 들어갔을 때 호출되는 메서드
    private void EnterWater()
    {
        Debug.Log("물에 들어갔습니다!");
        animator.SetLayerWeight(1, 1f);
        charControl.stepOffset = 0.1f; // 물에서 점프를 낮 설정
        gravityModifier = 0f; // 중력 감소

    }

    // 물에서 나왔을 때 호출되는 메서드
    private void ExitWater()
    {
        Debug.Log("물에서 나왔습니다!");
        isInWater = false;
        animator.SetLayerWeight(1, 0f);
        charControl.stepOffset = 0.3f; // 원래 점프로 복원
        gravityModifier = 2f; // 원래 중력으로 복원
    }
}

