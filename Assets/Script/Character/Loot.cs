using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot
{
    public ItemStack LootItemStack { get; protected set; }

    private float probability;

    public Loot(ItemStack stack, float chanceToLoot)
    {
        probability = Mathf.Clamp01(chanceToLoot);
    }

    public bool RollLoot()
    {
        float result = Random.Range(0f, 1f);

        return result <= probability;
    }
}
