using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PlayerMovingManager : ManagerBase
{
    #region ManagerBase 상속
    public override void EnableManager()
    {
        //매니저 활성화
        this.gameObject.SetActive(true);
    }

    public override void DisableManager()
    {
        //이동 관련된 변수들 0으로 초기화
        _animator.SetFloat("Run", 0f);
        _rigidbody.linearVelocity = Vector3.zero;
        movingVector = Vector2.zero;

        //매니저 비활성화
        this.gameObject.SetActive(false);
    }
    #endregion

    #region 변수
    private AudioSource PlayerMovingManagerAudioSource;
    private GameObject _playerObject;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private float _currentVelocity;

    public float gravityScale;
    public float speed;
    public Vector2 movingVector;
    public Transform cameraTransform;
    public float distanceToGround; //확인용
    #endregion

    private void Start()
    {
        PlayerMovingManagerAudioSource = GetComponent<AudioSource>();
        _playerObject = GameObject.Find("HERO");

        if (_playerObject != null)
        {
            _rigidbody = _playerObject.GetComponent<Rigidbody>();
            _animator = _playerObject.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        CharaMove();
    }

    public void OnMove(InputValue value)
    {
        //움직이는 방향 감지
        movingVector = value.Get<Vector2>();
    }

    //캐릭터를 움직이게 하는 메서드
    private void CharaMove()
    {
        if (movingVector != Vector2.zero)
        {
            if (!PlayerMovingManagerAudioSource.isPlaying)
            {
                //속도가 1 이상이 되면 발소리를 재생
                PlayerMovingManagerAudioSource.Play();
            }

            // 입력 방향 계산
            Vector3 inputDirection = new Vector3(movingVector.x, 0.0f, movingVector.y).normalized;

            // 카메라의 로컬 좌표계를 기준으로 이동 방향 변환
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            // 카메라의 Y축은 제거 (평면 이동)
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            // 최종 이동 방향 계산
            Vector3 moveDirection = (inputDirection.x * cameraRight) + (inputDirection.z * cameraForward);

            // 이동하는 방향으로 캐릭터가 바라보게 설정
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle
                (_playerObject.transform.eulerAngles.y, //현재 각도
                targetAngle, //목표 각도
                ref _currentVelocity, // 현재 속도의 참조값
                0.1f //부드럽게 감속하는데 걸리는 시간
                );
            _playerObject.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            // Rigidbody 속도 설정
            _rigidbody.linearVelocity = moveDirection * speed;
        }
        else
        {
            if (PlayerMovingManagerAudioSource.isPlaying)
            {
                //속도가 0이 되면 발소리를 중지
                PlayerMovingManagerAudioSource.Stop();
            }

            // 입력이 없을 경우 속도 0
            _rigidbody.linearVelocity = Vector3.zero;
        }

        //중력적용
        Ray ray = new Ray(_playerObject.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            distanceToGround = hit.distance;

            if (distanceToGround > 0.12f)
            {
                Vector3 customGravity = Physics.gravity * gravityScale;
                _rigidbody.AddForce(customGravity - Physics.gravity, ForceMode.Acceleration);
            }
        }

        // 애니메이터의 Speed 파라미터 설정
        _animator.SetFloat("Run", _rigidbody.linearVelocity.magnitude);
    }
}
