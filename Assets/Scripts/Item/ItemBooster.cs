using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBooster : ItemObject
{
    private Coroutine coroutine;
    [SerializeField] private Collider collider;
    [SerializeField] private MeshRenderer meshRenderer;

    private void Start()
    {
        collider = GetComponent<Collider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    // 기존의 ItemObject 스크립트를 상속받아 부스터 기능을 작동
    public override void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        
        Debug.Log("부스터 효과를 얻습니다!");

        if (data.type == ItemType.Booster)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(BoostTimer());
        }
    }

    private IEnumerator BoostTimer()
    {
        CharacterManager.Instance.Player.controller.PlayerBoost(data.boosters[0].value);
        collider.enabled = false;
        meshRenderer.enabled = false;
        
        float curTime = 0f;
        while (curTime < data.boosters[1].value)
        {
            Debug.Log(curTime + "초 동안 행동 완료!");
            curTime += Time.deltaTime;

            if (curTime >= data.boosters[1].value)
            {
                CharacterManager.Instance.Player.controller.PlayerBoostEnd(data.boosters[0].value);
                Debug.Log(curTime + "초 동안 행동 완료 후 부스터 종료");
                Destroy(gameObject);
            }
            
            yield return null;
        }
    }
}
