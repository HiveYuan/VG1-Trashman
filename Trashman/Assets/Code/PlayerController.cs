using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
        CapsuleCollider2D _collider;
        public Transform[] attackZones;
        public GameController gameController;

        float moveSpeed = 4f;
        float healthLoseSpeed = 8f;
        float x_direction = 0f;
        float y_direction = 0f;

        float center_offset_x = 0.5f;
        float center_offset_y = 0.5f;

        // health bar
        public float maxHealth = 40f;
        public float currentHealth = 40f;

        public Health health;
        public Image prompt;
        public InventoryManager inventory;
        public UIManager _uiManager;

        Animator _animator;

        // State Tracking
        public Direction facingDirection;



        // Start is called before the first frame update
        void Start() {
            prompt.enabled = false;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _collider = GetComponent<CapsuleCollider2D>();
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

            float posX = _rigidbody2D.position.x - center_offset_x;
            float posY = _rigidbody2D.position.y - center_offset_y;

            if (currentHealth > 1e-5) {
                if (Math.Abs(h) > float.Epsilon || Math.Abs(v) > float.Epsilon) {
                    x_direction = h;
                    y_direction = v;
                    //in the center
                    if (Math.Abs(posX - Math.Round(posX)) < 0.05f && Math.Abs(posY - Math.Round(posY)) < 0.05f) {
                        //Debug.Log("detected");
                        _collider.enabled = false;
                        RaycastHit2D hit = Physics2D.Linecast(_rigidbody2D.position, _rigidbody2D.position + new Vector2(h, v));
                        _collider.enabled = true;

                        //hit wall
                        if (hit.collider != null && hit.collider.tag == "Wall") {
                            Debug.Log("collided with wall");
                            _rigidbody2D.velocity = new Vector2(0, 0);
                            _animator.ResetTrigger("Move");
                        } else {
                            _rigidbody2D.velocity = new Vector2(h, v) * moveSpeed;
                            _animator.SetTrigger("Move");
                            LoseHealth(Time.deltaTime * healthLoseSpeed);
                        }
                    } else {

                        if (Math.Abs(posX - Math.Round(posX)) > 0.05f && h == 0) {
                            _animator.SetTrigger("Move");
                            _rigidbody2D.velocity = new Vector2(x_direction, 0) * moveSpeed;
                            LoseHealth(Time.deltaTime * healthLoseSpeed);
                        } else if (Math.Abs(posY - Math.Round(posY)) > 0.05f && v == 0) {
                            _animator.SetTrigger("Move");
                            _rigidbody2D.velocity = new Vector2(0, y_direction) * moveSpeed;
                            LoseHealth(Time.deltaTime * healthLoseSpeed);
                        } else {

                            _rigidbody2D.velocity = new Vector2(h, v) * moveSpeed;
                            _animator.SetTrigger("Move");
                            LoseHealth(Time.deltaTime * healthLoseSpeed);
                        }
                    }


                } else {    //no key pressed
                    
                    //not stop in the center
                    if (Math.Abs(posX - Math.Round(posX)) > 0.05f) {
                        _animator.SetTrigger("Move");
                        _rigidbody2D.velocity = new Vector2(x_direction, 0) * moveSpeed;
                        LoseHealth(Time.deltaTime * healthLoseSpeed);
                    } else if (Math.Abs(posY - Math.Round(posY)) > 0.05f) {
                        _animator.SetTrigger("Move");
                        _rigidbody2D.velocity = new Vector2(0, y_direction) * moveSpeed;
                        LoseHealth(Time.deltaTime * healthLoseSpeed);
                    } else {
                        _rigidbody2D.velocity = new Vector2(0, 0);
                        _animator.ResetTrigger("Move");
                    }
                }
            } else {    //no health
                _rigidbody2D.velocity = new Vector2(0, 0);
                _animator.ResetTrigger("Move");
            }

            _animator.SetFloat("Way2GoX", x_direction);
            _animator.SetFloat("Way2GoY", y_direction);



        }


        // Update is called once per frame
        void Update() {

            // Use item in inventory - By Hou
            for (int i = 0; i < 10; i++) {
                ItemClass item;
                if (Input.GetKeyDown("" + i)) {
                    if (i == 0)
                    {
                        item = inventory.Get(10);
                    }
                    else
                    {
                        item = inventory.Get(i);
                    }
                    if (item != null) {
                        if (item.itemType == "food") {
                            item = inventory.Remove(i);
                            GainHealth(((FoodClass)item).GetFood().healthAdded);
                        } else {
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

                                    // if(item.itemType.ToolType == "knife" && barrier.BarrierClass.BarrierType == "wood" || 
                                    //     item.itemType.ToolType == "dollars" && barrier.BarrierClass.BarrierType == "security") {
                                    //     item = inventory.Remove(i);
                                    //     _animator.SetTrigger("Attack");
                                    //     barrier.Break();

                                    //     //trigger "Get Star" tutorial
                                    //     print("trigger last tutorial! " + gameController.tutorialStageChange);
                                    //     if (gameController.isTutorialOn == 1 && gameController.tutorialStageChange == (int)TutorialStages.AttackBarrier)
                                    //     {
                                    //         gameController.tutorialStageChange = (int) TutorialStages.GetStar;
                                    //     }
                                    // }
                                    
                                    item = inventory.Remove(i);
                                    _animator.SetTrigger("Attack");
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
                FoodClass food = inventory.foods[other.gameObject.name];
                if (gameController.isTutorialOn == 0 && food.isFirstTime) {
                    _uiManager.CreateItemBox(other.gameObject.name, food.itemIntro, food.itemIcon);
                    food.isFirstTime = false;
                }
                inventory.Add(food);

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
                ToolClass tool = inventory.tools[other.gameObject.name];
                if (gameController.isTutorialOn == 0 && tool.isFirstTime)
                {
                    _uiManager.CreateItemBox(other.gameObject.name, tool.itemIntro, tool.itemIcon);
                    tool.isFirstTime = false;
                }
                inventory.Add(tool);


                Destroy(other.gameObject);
            }
            if (other.gameObject.CompareTag("Target"))
            {
                Debug.Log("collision with target");
                // Reach the target star
                gameController.didSucceedChange = 1;

                Destroy(other.gameObject);
            }
        }

        // Health - By Hou
        void GainHealth( float hp ) {
            if (currentHealth < maxHealth) {
                currentHealth += hp;
            }
            //added 0215
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            health.SetHealth(currentHealth, maxHealth);
            health.SetPrompt(true);

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

            if (currentHealth < 0f)
            {
                currentHealth = 0f;
            }

            health.SetHealth(currentHealth, maxHealth);
            health.SetPrompt(false);

            if (currentHealth == 0)
            {
                gameController.didSucceedChange = -1;
            }
        }

        // Identify the facing direction
        void LateUpdate() {
            if (String.Equals(_spriteRenderer.sprite.name, "trashman_13")) {
                facingDirection = Direction.Up;
            } else if (String.Equals(_spriteRenderer.sprite.name, "trashman_8")) {
                facingDirection = Direction.Down;
            } else if (String.Equals(_spriteRenderer.sprite.name, "trashman_5")) {
                facingDirection = Direction.Left;
            } else if (String.Equals(_spriteRenderer.sprite.name, "trashman_0")) {
                facingDirection = Direction.Right;
            }
        }

    }
}
