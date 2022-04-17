using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: add prompt for barrier losing health (little hp bar)
public class BarrierController : MonoBehaviour
{
    public InventoryManager inventory;
    public BarrierClass barrier;
    public int hp;

    [Header("RandomMove")]
    public Rigidbody2D _rigidbody2D;
    public Transform pos_left, pos_right;   // Left and right boundaries
    public float speed;
    public bool faceLeft;   // Face to the left
    public float leftx, rightx;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        Debug.Log(gameObject.name.Split(" ")[0]);
        barrier = inventory.barriers[gameObject.name.Split(" ")[0]];
        hp = barrier.hp;

        // Move
        _rigidbody2D = GetComponent<Rigidbody2D>();
        leftx = pos_left.position.x;
        rightx = pos_right.position.x;

        Destroy(pos_left.gameObject);
        Destroy(pos_right.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (barrier.barrierType == BarrierClass.BarrierType.Monster)
        {
            RandomMove();
        }
    }

    // Update hp when being attacked
    public int LoseHP(int damage)
    {
        hp -= damage;
        return hp;
    }

    // monster random move
    // TODO: random move to 4 directions
    // TODO: stop in the middle of the grid
    public void RandomMove()
    {
        if (faceLeft)
        {
            _rigidbody2D.velocity = new Vector2(-speed, _rigidbody2D.velocity.y);

            if (transform.position.x < leftx)
            {
                faceLeft = false;
            }
        }
        else
        {
            _rigidbody2D.velocity = new Vector2(speed, _rigidbody2D.velocity.y);

            if (transform.position.x > rightx)
            {
                faceLeft = true;
            }
        }
    }
}
