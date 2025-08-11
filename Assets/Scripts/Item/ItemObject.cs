using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public virtual void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;

        // haracterManager.Instance.Player.addItem?.Invoke();
        if (this.data.type == ItemType.Consumable)
        {
            for (int i = 0; i < data.consumables.Length; i++)
            {
                switch (data.consumables[i].type)
                {
                    case ConsumableType.Health:
                        CharacterManager.Instance.Player.condition.Heal(data.consumables[i].value);
                        break;
                    case ConsumableType.Stamina:
                        CharacterManager.Instance.Player.condition.Eat(data.consumables[i].value);
                        break;
                }
            }

            Destroy(gameObject);
        }
    }
}
