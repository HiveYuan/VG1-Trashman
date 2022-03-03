using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Direction {
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3
}

namespace Trashman {
    public class PlayerController : MonoBehaviour {
        //outlet
        Rigidbody2D _rigidbody2D;
        SpriteRenderer _spriteRenderer;
        public Transform[] attackZones;
        public GameController gameController;

        float moveSpeed = 5f;
        float healthLoseSpeed = 10f;
        float x_direction = 0f;
        float y_direction = 0f;

        float center_offset_x = 0.7f;
        float center_offset_y = 0.9f;

        // health bar
        public float maxHealth = 40f;
        public float currentHealth = 40f;

        public Health health;
        public InventoryManager inventory;


        Animator _animator;

        // State Tracking
        public Direction facingDirection;



        // Start is called before the first frame update
        void Start() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            gameController = GameObject.Find("GameManager").GetComponent<GameController>();

        }

        void FixedUpdate() {

            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            if (Math.Abs(h) > float.Epsilon) {
                v = 0;
            }
            if (Math.Abs(v) > float.Epsilon) {
                h = 0;
            }
            // Debug.Log(currentHealth);
            if (currentHealth > 1e-5) {
                _rigidbody2D.velocity = new Vector2(h, v) * moveSpeed;
                if (Math.Abs(h) > float.Epsilon || Math.Abs(v) > float.Epsilon) {
                    _animator.SetTrigger("Move");
                    x_direction = h;
                    y_direction = v;
                    LoseHealth(Time.deltaTime * healthLoseSpeed);

                } else {
                    float posX = _rigidbody2D.position.x;
                    float posY = _rigidbody2D.position.y;
                    if (Math.Abs(Math.Abs(posX - (int)posX) - center_offset_x) > 0.05f) {
                        _animator.SetTrigger("Move");
                        _rigidbody2D.velocity = new Vector2(x_direction, 0) * moveSpeed;
                    } else if (Math.Abs(Math.Abs(posY - (int)posY) - center_offset_y) > 0.05f) {
                        _animator.SetTrigger("Move");
                        _rigidbody2D.velocity = new Vector2(0, y_direction) * moveSpeed;
                    } else {
                        _rigidbody2D.velocity = new Vector2(0, 0);
                        _animator.ResetTrigger("Move");
                    }
                }
            } else {
                _rigidbody2D.velocity = new Vector2(0, 0);
                _animator.ResetTrigger("Move");
            }

            _animator.SetFloat("Way2GoX", x_direction);
            _animator.SetFloat("Way2GoY", y_direction);



        }


        // Update is called once per frame
        void Update() {

            // Use item in inventory - By Hou
            for (int i = 1; i < 10; i++) {
                if (Input.GetKeyDown("" + i)) {
                    ItemClass item = inventory.Get(i);
                    if (item != null) {
                        if (item.itemType == "food") {
                            item = inventory.Remove(i);
                            GainHealth(((FoodClass)item).GetFood().healthAdded);
                        } else {
                            //_animator.SetTrigger("attack");

                            // Convert enrumeration to an index
                            int facingDirectionIndex = (int)facingDirection;

                            // Get attack zone from index
                            Transform attackZone = attackZones[facingDirectionIndex];

                            // What objects are within a circle at that attack zone
                            Collider2D[] hits = Physics2D.OverlapCircleAll(attackZone.position, 0.1f);

                            // Handle each hit target
                            foreach (Collider2D hit in hits) {
                                //bool knifeFlag = true;
                                BarrierClass barrier = hit.GetComponent<BarrierClass>();
                                if (barrier != null) {
                                    item = inventory.Remove(i);
                                    barrier.Break();
                                    
                                    //trigger "Get Star" tutorial
                                    print("trigger last tutorial! " + gameController.tutorialStageChange);
                                    if (gameController.isTutorialOn == 1 && gameController.tutorialStageChange == (int)TutorialStages.AttackBarrier)
                                    {
                                        gameController.tutorialStageChange = (int) TutorialStages.GetStar;
                                    }
                                    
                                } else {
                                    print("There is no barrier to be destroyed.");
                                }
                            }
                        }
                        break;
                    } else {
                        //TODO: User try to use non-existing item
                        //TODO: Add detection for whether this item can be used
                        print("Player try to use non-existing item.");
                    }
                }
            }


            /*if (_rigidbody2D.velocity.magnitude > 0) {
                animator.speed = _rigidbody2D.velocity.magnitude / 3f;
            } else {
                animator.speed = 1f;
            }*/

        }

        void OnCollisionEnter2D( Collision2D other ) {
            if (other.gameObject.CompareTag("Food")) {
                Debug.Log("collision with food");
                // Pickup food - By Hou
                inventory.Add(inventory.foods[other.gameObject.name]);
                
                //trigger "item use" tutorial
                if (gameController.isTutorialOn == 1 && gameController.tutorialStageChange == (int)TutorialStages.HealthLost)
                {
                    gameController.tutorialStageChange = (int) TutorialStages.ItemsUse;
                }

                Destroy(other.gameObject);
            }
            if (other.gameObject.CompareTag("Tool")) {
                Debug.Log("collision with tool");
                // Pickup tool - By Hou
                inventory.Add(inventory.tools[other.gameObject.name]);


                Destroy(other.gameObject);
            }
        }

        // Health - By Hou
        void GainHealth( float hp ) {
            if (currentHealth < maxHealth) {
                currentHealth += hp;
            }
            //added 0215
            if (currentHealth > maxHealth) {
                currentHealth = maxHealth;
            }

            health.SetHealth(currentHealth, maxHealth);
            
            //trigger "Barrier" tutorial 
            if (gameController.isTutorialOn == 1 && gameController.tutorialStageChange == (int)TutorialStages.ItemsUse)
            {
                gameController.tutorialStageChange = (int)TutorialStages.AttackBarrier;
            }
        }

        // Health - By Hou
        void LoseHealth( float hp ) {
            if (currentHealth > 0f) {
                currentHealth -= hp;
            }

            health.SetHealth(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                gameController.didSucceedChange = -1;
            }
        }

        // Identify the facing direction
        void LateUpdate() {
            if (String.Equals(_spriteRenderer.sprite.name, "mario_24")) {
                facingDirection = Direction.Up;
            } else if (String.Equals(_spriteRenderer.sprite.name, "mario_1")) {
                facingDirection = Direction.Down;
            } else if (String.Equals(_spriteRenderer.sprite.name, "mario_10")) {
                facingDirection = Direction.Left;
            } else if (String.Equals(_spriteRenderer.sprite.name, "mario_13")) {
                facingDirection = Direction.Right;
            }
        }

    }
}
