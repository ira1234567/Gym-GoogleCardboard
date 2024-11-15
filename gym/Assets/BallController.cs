using UnityEngine;
using TMPro;

public class BallController : MonoBehaviour
{
    public float throwForce = 50f; //���� ������ ��� �������� ������
    public Camera playerCamera;
    public GameObject goal;
    public TMP_Text scoreText;
    public float maxSpeed = 3f; //����������� ������������ �������� ����
    private Rigidbody rb;
    private int score = 0;
    private bool isGazingAtGoal = false;
    private float gazeTime = 0f;
    private const float requiredGazeTime = 3f; // ����� � �������� ��� ������
    private Vector3 initialPosition; // ��������� ������� ����

    private Ray currentRay; // ������ ������� ���

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 0.5f; 
        scoreText.text = "Score: 0";
        initialPosition = transform.position; // ��������� ��������� �������
    }

    void Update()
    {
        currentRay = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(currentRay, out hit))
        {
            if (hit.collider.gameObject == goal)
            {
                if (!isGazingAtGoal)
                {
                    isGazingAtGoal = true;
                    gazeTime = 0f;
                }

                gazeTime += Time.deltaTime;
                if (gazeTime >= requiredGazeTime)
                {
                    ThrowBall();
                    gazeTime = 0f;
                    isGazingAtGoal = false;
                }
            }
            else
            {
                isGazingAtGoal = false;
                gazeTime = 0f;
            }
        }
        else
        {
            isGazingAtGoal = false;
            gazeTime = 0f;
        }
    }

    void ThrowBall()
    {
        Vector3 targetPoint;

        RaycastHit hit;

        if (Physics.Raycast(currentRay, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = goal.transform.position;
        }

        // ��������� ����� ������� ���� ��� ����� ������� ����
        targetPoint.y += 1.5f;

        // ������������ ����������� � ���� ������
        Vector3 direction = CalculateThrowDirection(targetPoint);
        rb.velocity = Vector3.zero; // ���������� ���������� �������� ����
        rb.AddForce(direction * throwForce); // ������� ��� � ������� ����
        rb.useGravity = true; // �������� ����������, ����� ��� ���������� �����

        // ������������ ������������ �������� ����
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // ��������� ���� ��� ������ ������
        scoreText.text = "Score: " + score;
    }

    Vector3 CalculateThrowDirection(Vector3 targetPoint)
    {
        // ���������� �� ���� �� ����������� (XY ���������)
        Vector3 horizontalDistance = new Vector3(targetPoint.x - transform.position.x, 0, targetPoint.z - transform.position.z);
        float horizontalDistanceMagnitude = horizontalDistance.magnitude;

        // ���������� �� ���� �� ��������� (Y)
        float verticalDistance = targetPoint.y - transform.position.y;

        // ���� ��� ������
        float angle = 60f * Mathf.Deg2Rad; // 60 �������� � ��������

        // ���������� ������� ��� ������� ��������
        float velocity = Mathf.Sqrt((horizontalDistanceMagnitude * Physics.gravity.magnitude) / Mathf.Sin(2 * angle));

        // ������������ ���������� ��������
        float velocityY = velocity * Mathf.Sin(angle);
        float velocityXZ = velocity * Mathf.Cos(angle);

        // �������� ����������� ������
        Vector3 direction = (horizontalDistance.normalized * velocityXZ) + (Vector3.up * velocityY);
        return direction;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            score++;
            scoreText.text = "Score: " + score;
            ResetBallPosition();
        }
    }

    void ResetBallPosition()
    {
        rb.useGravity = false; // ��������� ����������, ����� ��� �� ����� �� ������
        rb.velocity = Vector3.zero; // ������������� ���
        rb.angularVelocity = Vector3.zero; // ���������� �������� ����
        transform.position = initialPosition; // ���������� ��� � ��������� �������
    }
}
