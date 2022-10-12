using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 참고
 * - 유니티 화면 터치 : https://chameleonstudio.tistory.com/14
 * - 유니티 오브젝트를 마우스 드래그&드롭(터치 이동)으로 이동시키기 : http://hb7083.blogspot.com/2016/02/blog-post_13.html
 * - [Unity] 두 점 사이의 각도 구하기 : https://unity-programmer.tistory.com/30
 */
public class PlayerController : MonoBehaviour
{
    public static bool s_canPressKey = true;
    public FloatingJoystick joy;

    // 이동
    [SerializeField] float moveSpeed = 3; // 이동 속도
    Vector3 dir = new Vector3(); // 움직일 방향
    public Vector3 destPos = new Vector3(); // 목적지
    Vector3 originPos = new Vector3();

    // 회전
    [SerializeField] float spinSpeed = 270; // 회전 속도
    Vector3 rotDir = new Vector3(); // 회전시킬 방향
    Quaternion destRot = new Quaternion(); //  목표 회전값, 가짜 큐브를 먼저 돌려 놓고, 돌아간 만큼의 값을 목표 회전값으로 삼음

    // 반동
    [SerializeField] float recoilPosY = 0.25f; // 반동값
    [SerializeField] float recoilSpeed = 1.5f; // 반동 속도

    bool canMove = true;
    public bool isFalling = false;

    // 기타
    [SerializeField] Transform fakeCube = null;
    [SerializeField] Transform realCube = null;

    TimingManager theTimingManager;
    AudioManager theAudioManager;

    Rigidbody myRigidbody;

    float x;
    float z;
    Vector2 start;
    Vector2 end;
    float dragAngle;
    bool drag;
    public int dragCnt;

    private void Awake()
    {
        myRigidbody = GetComponentInChildren<Rigidbody>(); // GetComponentInChildren : 자식 객체 중에 특정 컴포넌트가 있다면 가져옴
    }

    private void Start()
    {
        theTimingManager = FindObjectOfType<TimingManager>();
        theAudioManager = FindObjectOfType<AudioManager>();
        
        originPos = transform.position;
    }

    // 초기화
    public void Initialized()
    {
        // 위치 값 초기화
        transform.position = Vector3.zero;
        destPos = Vector3.zero;
        transform.Find("PlayerPos").localPosition = Vector3.zero;
        realCube.localPosition = Vector3.zero;

        // 회전 값 초기화
        realCube.localRotation = Quaternion.identity;
        fakeCube.localRotation = Quaternion.identity;

        canMove = true;
        s_canPressKey = true;
        isFalling = false;
        myRigidbody.useGravity = false;
        myRigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (GameManager.instance.isStartGame)
        {
            CheckFalling();

            x = joy.Horizontal;
            z = joy.Vertical;

            if (!drag)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    start = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                }
                if (Input.GetMouseButton(0))
                {
                    end = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                }

                dragAngle = GetAngle(start, end);
                if (dragAngle < 0) dragAngle *= -1;
            }

            if (Input.GetMouseButtonUp(0))
            {
                drag = false;
            }

            if (x != 0 || z != 0)
            {
                if (canMove && s_canPressKey && !isFalling && !drag)
                {
                    if (0 < dragCnt)
                    {
                        dragCnt--;
                    }
                    else
                    {
                        drag = true;
                    }

                    Cal(((45 < dragAngle && dragAngle < 135) ? 0 : x), ((45 < dragAngle && dragAngle < 135) ? z : 0));

                    // 판정 체크
                    if (theTimingManager.CheckTiming())
                    {
                        CalRot();
                        StartAction();
                        start = Vector2.zero;
                        end = Vector2.zero;
                    }
                }
            }
        }
    }

    // 두 점 사이의 각도 구하기
    float GetAngle(Vector2 start, Vector2 end)
    {
        Vector2 v2 = end - start;
        return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
    }

    // 목표 지점 계산
    void Cal(float x, float z)
    {
        // 방향 계산
        dir.Set(x, 0, z);

        // 이동 목표값 계산
        destPos = transform.position + new Vector3(dir.x, 0, dir.z);
    }
    /* 회전 목표값 계산
     * Cal 함수에 회전 목표값을 같이 넣어서 계산할 경우 Miss가 떴을 때 회전을 계산하게 되어 동작할 때 회전이 원하는 대로 나오지 않음
     * Cal 함수 자체를 판정 이후에 넣을 경우 스테이지 발판이 제대로 동작하지 않음
     */
    void CalRot()
    {
        rotDir = new Vector3(-dir.z, 0, dir.x);
        fakeCube.RotateAround(transform.position, rotDir, spinSpeed); // RotateAround : 태양 주변을 공전하는 지구등을 구현할 때 사용
        destRot = fakeCube.rotation;
    }

    void StartAction()
    {
        StartCoroutine("MoveCo");
        StartCoroutine("Spin");
        StartCoroutine("RecoilCo");
    }

    IEnumerator MoveCo()
    {
        canMove = false;
        while (Vector3.SqrMagnitude(transform.position - destPos) >= 0.001f)    // Vector3.Distance : A좌표와 B좌표간의 거리차를 반환
                                                                                // Vector3.SqrMagnitude 제곱근을 반환
        {
            transform.position = Vector3.MoveTowards(transform.position, destPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = destPos;
        canMove = true;
    }

    IEnumerator Spin()
    {
        while (Quaternion.Angle(realCube.rotation, destRot) > 0.5f)
        {
            realCube.rotation = Quaternion.RotateTowards(realCube.rotation, destRot, spinSpeed * Time.deltaTime);
            yield return null;
        }
        realCube.rotation = destRot;
    }

    IEnumerator RecoilCo()
    {
        while (realCube.position.y < recoilPosY)
        {
            realCube.position += new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }
        while (realCube.position.y > 0)
        {
            realCube.position -= new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }
        realCube.localPosition = Vector3.zero;
    }

    void CheckFalling()
    {
        if (!isFalling & canMove)
        {
            if (!Physics.Raycast(transform.position, Vector3.down, 1f))
            {
                StartCoroutine("Falling");
            }
        }
    }

    /*
     * Rigidbody의 중력으로 추락할 경우 위치값을 제대로 잡지 못하는 이슈가 있어서 y값으로 강제로 이동
     * 원인 파악 필요
    void Falling()
    {

        isFalling = true;
        myRigidbody.useGravity = true;
        myRigidbody.isKinematic = false;
    }
    */
    IEnumerator Falling()
    {
        isFalling = true;
        myRigidbody.useGravity = true;
        myRigidbody.isKinematic = false;

        float speedFalling = 10f;

        theAudioManager.PlayeSFX("Falling");

        while (true)
        {
            transform.Find("PlayerPos").Translate(Vector3.down * speedFalling * Time.deltaTime);
            speedFalling += speedFalling * 0.01f; 
            yield return null;
        }
    }

    public void ResetFalling()
    {
        isFalling = false;
        myRigidbody.useGravity = false;
        myRigidbody.isKinematic = true;

        StopCoroutine("Falling");
        transform.Find("PlayerPos").localPosition = Vector3.zero;

        transform.position = originPos;

        realCube.localRotation = Quaternion.identity;
        realCube.localPosition = Vector3.zero;

        fakeCube.localRotation = Quaternion.identity;
    }
}
