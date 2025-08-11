using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;
    
    private Condition health
    {
        get { return uiCondition.health; }
    }
    private Condition stamina
    {
        get { return uiCondition.stamina; }
    }

    public event Action onTakeDamge;

    private void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (health.curValue <= 0f)
        {
            PlayerDie();
        }
    }

    public void PlayerDie()
    {
        Debug.Log("플레이어 사망!");
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        stamina.Add(amount);
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        onTakeDamge?.Invoke();
    }

    // 스태미나 소모 메서드
    public void DecreaseStamina(float amount)
    {
        stamina.Subtract(amount);
    }

    // 스태미나가 일정 기준치 이상인지 판별하는 메서드 -> 만족하면 스태미나를 사용할 수 있도록 구현
    public bool CanUseStamina(float standardAmount)
    {
        if (stamina.curValue <= standardAmount)
        {
            return false;
        }

        return true;
    }
}
