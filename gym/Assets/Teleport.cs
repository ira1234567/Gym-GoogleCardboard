using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public LayerMask teleportLayer; // ���� �������� ��� ������������
    public float gazeDuration = 2f; // ����� ��������� ������� ��� ���������

    private float gazeTimer = 0f; // ������ �������
    private GameObject currentTarget = null;

    void Update()
    {
        // ���������, ���� ������� �����
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 100f, teleportLayer))
        {
            GameObject hitObject = hit.collider.gameObject;

            // ���� �� ������� �� ����� ������
            if (hitObject != currentTarget)
            {
                gazeTimer = 0f; // ���������� ������
                currentTarget = hitObject;
            }

            // ����������� ������ �������
            gazeTimer += Time.deltaTime;

            // ���� ����� ��������� ������� ���������, ������������� ������
            if (gazeTimer >= gazeDuration)
            {
                TeleportTo(hit.point);
                gazeTimer = 0f; // ���������� ������ ����� ������������
            }
        }
        else
        {
            // ����������, ���� ������������ �������� �������� �� ������
            gazeTimer = 0f;
            currentTarget = null;
        }
    }

    void TeleportTo(Vector3 targetPosition)
    {
        transform.position = targetPosition; // ���������� ������� ������ (Player) � ����� ���������
    }
}
