using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: add prompt for barrier losing health (little hp bar)
public class BarrierHpController : MonoBehaviour
{
    public InventoryManager inventory;
    public int hp;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        Debug.Log(gameObject.name.Split(" ")[0]);
        hp = inventory.barriers[gameObject.name.Split(" ")[0]].hp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update hp when being attacked
    public int loseHealth(int damage)
    {
        hp -= damage;
        return hp;
    }
}
