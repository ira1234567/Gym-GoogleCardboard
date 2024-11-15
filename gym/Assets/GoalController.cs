using UnityEngine;

public class GoalController : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // �������� ��������� AudioSource �� ���� �������
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            audioSource.PlayOneShot(audioSource.clip); // ������������� ������� ����, ����������� �� AudioSource
        }
    }
}
