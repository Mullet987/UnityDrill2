using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PlayerMovingManager : ManagerBase
{
    #region ManagerBase ���
    public override void EnableManager()
    {
        //�Ŵ��� Ȱ��ȭ
        this.gameObject.SetActive(true);
    }

    public override void DisableManager()
    {
        //�̵� ���õ� ������ 0���� �ʱ�ȭ
        _animator.SetFloat("Run", 0f);
        _rigidbody.linearVelocity = Vector3.zero;
        movingVector = Vector2.zero;

        //�Ŵ��� ��Ȱ��ȭ
        this.gameObject.SetActive(false);
    }
    #endregion

    #region ����
    private AudioSource PlayerMovingManagerAudioSource;
    private GameObject _playerObject;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private float _currentVelocity;

    public float gravityScale;
    public float speed;
    public Vector2 movingVector;
    public Transform cameraTransform;
    public float distanceToGround; //Ȯ�ο�
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
        //�����̴� ���� ����
        movingVector = value.Get<Vector2>();
    }

    //ĳ���͸� �����̰� �ϴ� �޼���
    private void CharaMove()
    {
        if (movingVector != Vector2.zero)
        {
            if (!PlayerMovingManagerAudioSource.isPlaying)
            {
                //�ӵ��� 1 �̻��� �Ǹ� �߼Ҹ��� ���
                PlayerMovingManagerAudioSource.Play();
            }

            // �Է� ���� ���
            Vector3 inputDirection = new Vector3(movingVector.x, 0.0f, movingVector.y).normalized;

            // ī�޶��� ���� ��ǥ�踦 �������� �̵� ���� ��ȯ
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            // ī�޶��� Y���� ���� (��� �̵�)
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            // ���� �̵� ���� ���
            Vector3 moveDirection = (inputDirection.x * cameraRight) + (inputDirection.z * cameraForward);

            // �̵��ϴ� �������� ĳ���Ͱ� �ٶ󺸰� ����
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle
                (_playerObject.transform.eulerAngles.y, //���� ����
                targetAngle, //��ǥ ����
                ref _currentVelocity, // ���� �ӵ��� ������
                0.1f //�ε巴�� �����ϴµ� �ɸ��� �ð�
                );
            _playerObject.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            // Rigidbody �ӵ� ����
            _rigidbody.linearVelocity = moveDirection * speed;
        }
        else
        {
            if (PlayerMovingManagerAudioSource.isPlaying)
            {
                //�ӵ��� 0�� �Ǹ� �߼Ҹ��� ����
                PlayerMovingManagerAudioSource.Stop();
            }

            // �Է��� ���� ��� �ӵ� 0
            _rigidbody.linearVelocity = Vector3.zero;
        }

        //�߷�����
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

        // �ִϸ������� Speed �Ķ���� ����
        _animator.SetFloat("Run", _rigidbody.linearVelocity.magnitude);
    }
}
