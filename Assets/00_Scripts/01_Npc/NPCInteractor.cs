using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class NPCInteractor : MonoBehaviour
{
    public float interactDistance =1.0f;
    public string npcTag = "NPC";
    public LayerMask layerMask = 6;
    public float maxViewAngle = 30f; // 정면 인식 각도
    public bool isTaking; // 대화중인지 여부

    private void OnEnable()
    {
        UIManager.Instance.OnClose += () => { isTaking = false; };
    }

    private void OnDisable()
    {
        UIManager.Instance.OnClose = null;
    }

    // F키 눌러서 상호작용
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !isTaking) {

            // 가장 가까운 NPC 찾기
            GameObject nearestNpc = FindNearestNpc();

            if (nearestNpc != null)
            {
                Vector3 direction = (nearestNpc.transform.position - transform.position).normalized;
                float distanceToNpc = Vector3.Distance(transform.position, nearestNpc.transform.position);

                Ray ray = new Ray(transform.position, direction);
                RaycastHit hit;

                // 레이 쏘기
                if (Physics.Raycast(ray, out hit, interactDistance, layerMask))
                {
                    // 가장 가까운 NPC가 일정거리 이내에 들어온다면
                    if (hit.collider.gameObject == nearestNpc && distanceToNpc <= interactDistance)
                    {
                        // 시점 고정
                        ViewpointFixation(nearestNpc);

                        // 상호작용 시작
                        NPC npc = nearestNpc.GetComponent<NPC>();
                        npc.Interact();
                    }
                }
            }
        }
    }
    // 시점 고정
    void ViewpointFixation(GameObject nearestNpc)
    {
        isTaking = true;

        // NPC와 상호작용
        Vector3 dir = nearestNpc.transform.position - transform.position;
        dir.y = 0; // X, Z축 회전을 막기 위해 Y축만 고려
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            nearestNpc.transform.rotation = Quaternion.Slerp(nearestNpc.transform.rotation, targetRotation, Time.deltaTime * 5f); // 부드럽게 회전하고 싶을 때
        }

        // 시점 고정
        CharacterManager.Instance.Player.controller.ToggleCursor(true);
    }
    // 가장 가까운 NPC 찾기
    GameObject FindNearestNpc()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag(npcTag);
        GameObject nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject npc in npcs)
        {
            float distance = Vector3.Distance(transform.position, npc.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = npc;
            }
        }
        return nearest;
    }
}
