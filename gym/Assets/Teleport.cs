using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public LayerMask teleportLayer; // Слой объектов для телепортации
    public float gazeDuration = 2f; // Время удержания взгляда для активации

    private float gazeTimer = 0f; // Таймер взгляда
    private GameObject currentTarget = null;

    void Update()
    {
        // Проверяем, куда смотрит игрок
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 100f, teleportLayer))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Если мы смотрим на новый объект
            if (hitObject != currentTarget)
            {
                gazeTimer = 0f; // Сбрасываем таймер
                currentTarget = hitObject;
            }

            // Увеличиваем таймер взгляда
            gazeTimer += Time.deltaTime;

            // Если время удержания взгляда превышено, телепортируем игрока
            if (gazeTimer >= gazeDuration)
            {
                TeleportTo(hit.point);
                gazeTimer = 0f; // Сбрасываем таймер после телепортации
            }
        }
        else
        {
            // Сбрасываем, если пользователь перестал смотреть на объект
            gazeTimer = 0f;
            currentTarget = null;
        }
    }

    void TeleportTo(Vector3 targetPosition)
    {
        transform.position = targetPosition; // Перемещаем текущий объект (Player) к точке телепорта
    }
}
