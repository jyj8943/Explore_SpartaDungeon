using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("좌표 관련")] 
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private float moveSpeed;
    private Vector3 targetPos;

    private void Start()
    {
        startPos = transform.position;
        targetPos = endPos;
    }

    private void FixedUpdate()
    {
        MovePlatForm();
    }

    private void MovePlatForm()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, endPos) <= 0.01f)
        {
            targetPos = startPos;
        }
        else if (Vector3.Distance(transform.position, startPos) <= 0.01f)
        {
            targetPos = endPos;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (other.transform.position.y >= this.transform.position.y)
            {
                other.transform.SetParent(this.transform);
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}
