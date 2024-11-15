using UnityEngine;

public class GoalController : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Получаем компонент AudioSource на этом объекте
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            audioSource.PlayOneShot(audioSource.clip); // Воспроизводим текущий клип, назначенный на AudioSource
        }
    }
}
