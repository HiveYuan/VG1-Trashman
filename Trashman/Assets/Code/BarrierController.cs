using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: add prompt for barrier losing health (little hp bar)
public class BarrierController : MonoBehaviour
{
    InventoryManager inventory;
    BarrierClass barrier;
    public int hp;
    public Image fill;
    public Canvas worldSpaceCanvas;

    [Header("RandomMove")]
    BoxCollider2D _collider;
    Rigidbody2D _rigidbody2D;
    Animator _animator;
    public Vector2 targetPos;
    Vector2 lastPos;
    float timeElapsed = 0;
    public float lerpDuration = 2;
    

    // TODO: fix bug for monster hp bar => unity will crash?
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        barrier = inventory.barriers[gameObject.name.Split(" ")[0]];
        hp = barrier.hp;

        // HP bar
        if (barrier.barrierType != BarrierClass.BarrierType.Trader)
        {
            Canvas hpCanvas = (Canvas)Instantiate(worldSpaceCanvas, gameObject.transform);
            hpCanvas.worldCamera = Camera.main;
            fill = hpCanvas.GetComponentsInChildren<Image>()[1];

            // Move
            if (barrier.barrierType == BarrierClass.BarrierType.Monster)
            {
                _rigidbody2D = GetComponent<Rigidbody2D>();
                _collider = GetComponent<BoxCollider2D>();
                _animator = GetComponent<Animator>();

                targetPos = transform.position;
                lastPos = transform.position;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (barrier.barrierType == BarrierClass.BarrierType.Monster)
        {
            if (timeElapsed < lerpDuration)
            {
                transform.position = Vector2.Lerp(lastPos, targetPos, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
            }
            else
            {
                lastPos = targetPos;
                transform.position = targetPos;
                timeElapsed = 0;

                RandomMove();

                // Set x-axis movement vector
                _animator.SetFloat("movementX", _rigidbody2D.velocity.x);
            }
        }
    }

    // Update hp when being attacked
    public int LoseHP(int damage)
    {
        fill.fillAmount = ((hp-damage) / (float)hp);
        hp -= damage;
        return hp;
    }

    // monster random move to 4 directions
    public void RandomMove()
    {
        System.Random rd = new System.Random();
        bool[] blocked = { false, false, false, false };

        while (true) {
            int direction = rd.Next(0, 4);
            if (blocked[direction]) {
                continue;
            }

            Vector2 direction_vector;

            switch (direction) {
                case 0:
                    direction_vector = new Vector2(0, 1);
                    break;
                case 1:
                    direction_vector = new Vector2(0, -1);
                    break;
                case 2:
                    direction_vector = new Vector2(-1, 0);
                    break;
                case 3:
                    direction_vector = new Vector2(1, 0);
                    break;
                default:
                    direction_vector = new Vector2(0, 0);
                    break;
            }

            //Debug.Log("detect");
            _collider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(_rigidbody2D.position, _rigidbody2D.position + direction_vector);
            _collider.enabled = true;

            //hit wall or barrier
            if (hit.collider != null && (hit.collider.tag == "Wall" || hit.collider.tag == "Barrier" || hit.collider.tag == "Tool" || hit.collider.tag == "Food")) {
                //Debug.Log("collided with " + hit.collider.tag);
                blocked[direction] = true;
                
            } else {
                targetPos += direction_vector;
                break;
            }
        }//end of while
        
    }
}
