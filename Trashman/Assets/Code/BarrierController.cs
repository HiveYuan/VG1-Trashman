using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarrierController : MonoBehaviour {
    InventoryManager inventory;
    BarrierClass barrier;
    public Canvas worldSpaceCanvas;

    [Header("HP Bar")]
    public int hp;
    public Image fill;

    [Header("RandomMove")]
    CircleCollider2D _collider;
    Rigidbody2D _rigidbody2D;
    Animator _animator;
    public Vector2 targetPos;
    Vector2 lastPos;
    float timeElapsed = 0;
    public float lerpDuration = 2;


    // Start is called before the first frame update
    void Start() {
        inventory = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        barrier = inventory.barriers[gameObject.name.Split(" ")[0]];
        hp = barrier.hp;

        // HP bar
        if (barrier.barrierType != BarrierClass.BarrierType.Trader) {
            Canvas hpCanvas = Instantiate(worldSpaceCanvas, gameObject.transform);
            hpCanvas.worldCamera = Camera.main;
            fill = hpCanvas.GetComponentsInChildren<Image>()[1];

            // Move
            if (barrier.barrierType == BarrierClass.BarrierType.Monster) {
                _rigidbody2D = GetComponent<Rigidbody2D>();
                _collider = GetComponent<CircleCollider2D>();
                _animator = GetComponent<Animator>();

                targetPos = transform.position;
                lastPos = transform.position;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (barrier.barrierType == BarrierClass.BarrierType.Monster) {
            if (timeElapsed < lerpDuration) {
                transform.position = Vector2.Lerp(lastPos, targetPos, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
            } else {
                lastPos = targetPos;
                transform.position = targetPos;
                timeElapsed = 0;

                RandomMove();

                // Set x-axis movement vector
                _animator.SetFloat("movementX", targetPos.x - transform.position.x);
            }
        }
    }

    // Update hp when being attacked
    public int LoseHP( int damage ) {
        fill.fillAmount = ((hp - damage) / (float)hp);
        hp -= damage;
        return hp;
    }

    // monster random move to 4 directions
    public void RandomMove() {
        System.Random rd = new System.Random();
        bool[] blocked = { false, false, false, false };

        for (int i = 0; i < 10; i++) {
            int direction = rd.Next(0, 4);
            if (blocked[direction]) {
                continue;
            }
            Vector2 upV = new Vector2(0, 1);
            Vector2 rightV = new Vector2(1, 0);
            Vector2 downV = new Vector2(0, -1);
            Vector2 leftV = new Vector2(-1, 0);
            Vector2[] direction_vector = { upV, rightV, downV, leftV };

            //Debug.Log("detect");
            _collider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(_rigidbody2D.position, _rigidbody2D.position + direction_vector[direction]);
            _collider.enabled = true;

            //hit wall or barrier
            if (hit.collider != null && (hit.collider.tag == "Wall" || hit.collider.tag == "Barrier" || hit.collider.tag == "Tool" || hit.collider.tag == "Food" || hit.collider.tag == "Monster")) {
                //Debug.Log("collided with " + hit.collider.tag);
                blocked[direction] = true;

            } else {
                _collider.enabled = false;
                RaycastHit2D hit1 = Physics2D.Linecast(_rigidbody2D.position + direction_vector[direction], _rigidbody2D.position + direction_vector[direction] + direction_vector[direction]);
                RaycastHit2D hit2 = Physics2D.Linecast(_rigidbody2D.position + direction_vector[direction], _rigidbody2D.position + direction_vector[direction] + direction_vector[(direction + 1) % 4]);
                RaycastHit2D hit3 = Physics2D.Linecast(_rigidbody2D.position + direction_vector[direction], _rigidbody2D.position + direction_vector[direction] + direction_vector[(direction + 3) % 4]);
                _collider.enabled = true;

                if (hit1.collider != null && hit1.collider.tag == "Monster" && hit2.collider != null && hit2.collider.tag == "Monster" && hit3.collider != null && hit3.collider.tag == "Monster") {
                    //Debug.Log("collided with " + hit.collider.tag);
                    blocked[direction] = true;
                } else {
                    targetPos += direction_vector[direction];
                    break;
                }



            }
        }//end of loop

    }

    /*void OnCollisionEnter2D( Collision2D other ) {
    
        targetPos = lastPos;
        lastPos = transform.position;
        timeElapsed = 0;
    }*/
}