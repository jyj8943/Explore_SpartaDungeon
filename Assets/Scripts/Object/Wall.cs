using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private float height;
    private bool isPlayerClimbing = false;

    private void Start()
    {
        height = transform.localScale.y;
    }

    private void Update()
    {
        if (isPlayerClimbing)
        {
            if (!PlayerPosCheck())
            {
                isPlayerClimbing = false;
                CharacterManager.Instance.Player.controller.ChangeMoving(false);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player") && PlayerPosCheck())
        {
            isPlayerClimbing = true;
            CharacterManager.Instance.Player.controller.ChangeMoving(true);
        }
        else
        {
            isPlayerClimbing = false;
            CharacterManager.Instance.Player.controller.ChangeMoving(false);
        }
    }

    private bool PlayerPosCheck()
    {
        // 위치 값은 계속 비교를 해야할듯
        if (CharacterManager.Instance.Player.transform.position.y < height)
        {
            return true;
        }

        return false;
    }
}
