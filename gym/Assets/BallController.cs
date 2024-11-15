using UnityEngine;
using TMPro;

public class BallController : MonoBehaviour
{
    public float throwForce = 50f; //сила броска дл€ плавного полета
    public Camera playerCamera;
    public GameObject goal;
    public TMP_Text scoreText;
    public float maxSpeed = 3f; //ограничение максимальной скорости м€ча
    private Rigidbody rb;
    private int score = 0;
    private bool isGazingAtGoal = false;
    private float gazeTime = 0f;
    private const float requiredGazeTime = 3f; // ¬рем€ в секундах дл€ броска
    private Vector3 initialPosition; // Ќачальна€ позици€ м€ча

    private Ray currentRay; // ’ранит текущий луч

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 0.5f; 
        scoreText.text = "Score: 0";
        initialPosition = transform.position; // —охран€ем начальную позицию
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

        // ѕоднимаем точку немного выше дл€ более плавной дуги
        targetPoint.y += 1.5f;

        // –ассчитываем направление и силу броска
        Vector3 direction = CalculateThrowDirection(targetPoint);
        rb.velocity = Vector3.zero; // —брасываем предыдущую скорость м€ча
        rb.AddForce(direction * throwForce); // Ѕросаем м€ч в сторону цели
        rb.useGravity = true; // ¬ключаем гравитацию, чтобы м€ч постепенно падал

        // ќграничиваем максимальную скорость м€ча
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // ќбновл€ем очки при каждом броске
        scoreText.text = "Score: " + score;
    }

    Vector3 CalculateThrowDirection(Vector3 targetPoint)
    {
        // –ассто€ние до цели по горизонтали (XY плоскость)
        Vector3 horizontalDistance = new Vector3(targetPoint.x - transform.position.x, 0, targetPoint.z - transform.position.z);
        float horizontalDistanceMagnitude = horizontalDistance.magnitude;

        // –ассто€ние до цели по вертикали (Y)
        float verticalDistance = targetPoint.y - transform.position.y;

        // ”гол дл€ броска
        float angle = 60f * Mathf.Deg2Rad; // 60 градусов в радианах

        // »спользуем формулу дл€ расчета скорости
        float velocity = Mathf.Sqrt((horizontalDistanceMagnitude * Physics.gravity.magnitude) / Mathf.Sin(2 * angle));

        // –ассчитываем компоненты скорости
        float velocityY = velocity * Mathf.Sin(angle);
        float velocityXZ = velocity * Mathf.Cos(angle);

        //  онечное направление броска
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
        rb.useGravity = false; // ќтключаем гравитацию, чтобы м€ч не падал до броска
        rb.velocity = Vector3.zero; // ќстанавливаем м€ч
        rb.angularVelocity = Vector3.zero; // —брасываем вращение м€ча
        transform.position = initialPosition; // ѕеремещаем м€ч в начальную позицию
    }
}
