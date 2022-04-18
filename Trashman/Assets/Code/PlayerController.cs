using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Trashman {
    public enum Direction {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3
    }
    public class PlayerController : MonoBehaviour {
        //outlet
        Rigidbody2D _rigidbody2D;
        SpriteRenderer _spriteRenderer;
        CapsuleCollider2D _collider;
        public GameObject[] attackZones;
        public GameController gameController;

        AudioSource walkSound;
        public TMP_Text buffPrompt;

        float moveSpeed = 4f;
        float healthLoseSpeed = 8f;
        float x_direction = 0f;
        float y_direction = 0f;

        float center_offset_x = 0.5f;
        float center_offset_y = 0.5f;

        bool move = false;

        // health bar
        public float maxHealth = 80f;
        public float currentHealth = 80f;

        public Health health;
        public Image prompt;
        public InventoryManager inventory;
        public UIManager _uiManager;
        Animator _animator;

        // State Tracking
        public Direction facingDirection;
        public int pickBuff = 1;
        public int damageBuff = 1;
        public int rangeBuff = 1;


        // Start is called before the first frame update
        void Start() {
            prompt.enabled = false;
            buffPrompt.text = "";
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _collider = GetComponent<CapsuleCollider2D>();
            gameController = GameObject.Find("GameManager").GetComponent<GameController>();
            walkSound = GetComponent<AudioSource>();
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

                        //hit wall or barrier
                        if (hit.collider != null && (hit.collider.tag == "Wall" || hit.collider.tag == "Barrier")) {
                            if (hit.collider.tag == "Barrier")
                            {
                                string objName = hit.collider.name.Split(" ")[0];
                                BarrierClass barrier = inventory.barriers[objName];
                                if (gameController.isTutorialOn == 0 && PlayerPrefs.GetInt(objName+"_new") == 1)
                                {
                                    _uiManager.CreateHintBox(objName, barrier.itemIntro, barrier.itemIcon, barrier.getToolNameList());
                                    PlayerPrefs.SetInt(objName + "_new", 0);
                                }
                            }
                            //Debug.Log("collided with " + hit.collider.tag);
                            _rigidbody2D.velocity = new Vector2(0, 0);
                            move = true;
                        } else {
                            _rigidbody2D.velocity = new Vector2(h, v) * moveSpeed;
                            move = true;
                            LoseHealth(Time.deltaTime * healthLoseSpeed);
                        }
                    } else {

                        if (Math.Abs(posX - Math.Round(posX)) > 0.05f && h == 0) {
                            move = true;
                            _rigidbody2D.velocity = new Vector2(x_direction, 0) * moveSpeed;
                            LoseHealth(Time.deltaTime * healthLoseSpeed);
                        } else if (Math.Abs(posY - Math.Round(posY)) > 0.05f && v == 0) {
                            move = true;
                            _rigidbody2D.velocity = new Vector2(0, y_direction) * moveSpeed;
                            LoseHealth(Time.deltaTime * healthLoseSpeed);
                        } else {

                            _rigidbody2D.velocity = new Vector2(h, v) * moveSpeed;
                            move = true;
                            LoseHealth(Time.deltaTime * healthLoseSpeed);
                        }
                    }


                } else {    //no key pressed
                    
                    //not stop in the center
                    if (Math.Abs(posX - Math.Round(posX)) > 0.05f) {
                        move = true;
                        _rigidbody2D.velocity = new Vector2(x_direction, 0) * moveSpeed;
                        LoseHealth(Time.deltaTime * healthLoseSpeed);
                    } else if (Math.Abs(posY - Math.Round(posY)) > 0.05f) {
                        move = true;
                        _rigidbody2D.velocity = new Vector2(0, y_direction) * moveSpeed;
                        LoseHealth(Time.deltaTime * healthLoseSpeed);
                    } else {
                        _rigidbody2D.velocity = new Vector2(0, 0);
                        move = false;
                    }
                }
            } else {    //no health
                _rigidbody2D.velocity = new Vector2(0, 0);
                move = false;
            }

            _animator.SetFloat("Way2GoX", x_direction);
            _animator.SetFloat("Way2GoY", y_direction);
            _animator.SetBool("Move", move);


        }


        // Update is called once per frame
        void Update() {
            //play walking sound - Jiang
            if (move) {
                if (!walkSound.isPlaying) {
                    walkSound.Play();
                }
            } else {
                walkSound.Stop();
            }

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
                        if (item.GetFood() != null) {
                            inventory.Remove(i);
                            GainHealth(item.GetFood().healthAdded);
                        }
                        else if (item.GetTool() != null)
                        {
                            // Convert enrumeration to an index
                            int facingDirectionIndex = (int)facingDirection;

                            if (item.GetTool().toolType == ToolClass.ToolType.Trade)
                            {
                                Transform[] attackZonesGroup = attackZones[0].GetComponentsInChildren<Transform>(); // length=5
                                Transform attackZone = attackZonesGroup[facingDirectionIndex + 1]; // +1: skip the transform of itself
                                Attack(attackZone, item, i);
                            }
                            for (int j = 0; j < item.GetTool().range * rangeBuff; j++)
                            {
                                Transform[] attackZonesGroup = attackZones[j].GetComponentsInChildren<Transform>(); // length=5
                                if (item.GetTool().toolType == ToolClass.ToolType.Attack)
                                {
                                    Transform attackZone = attackZonesGroup[facingDirectionIndex + 1]; // +1: skip the transform of itself
                                    Attack(attackZone, item, i);
                                }
                                else if (item.GetTool().toolType == ToolClass.ToolType.CircleAttack)
                                {
                                    for (int k = 0; k < 4; k++)
                                    {
                                        Transform attackZone = attackZonesGroup[k + 1]; // +1: skip the transform of itself
                                        Attack(attackZone, item, i);
                                    }
                                }
                            }
                        }
                        else if (item.GetPotion() != null)
                        {
                            inventory.Remove(i);
                            PotionClass potion = item.GetPotion();
                            //buffPrompt.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.7f);
                            switch (potion.potionType)
                            {
                                case PotionClass.PotionType.DamagePower:
                                    damageBuff *= potion.buff;
                                    buffPrompt.text = "Damage X" + potion.buff;
                                    break;
                                case PotionClass.PotionType.RangePower:
                                    rangeBuff *= potion.buff;
                                    buffPrompt.text = "Range X" + potion.buff;
                                    break;
                                case PotionClass.PotionType.PickPower:
                                    pickBuff *= potion.buff;
                                    buffPrompt.text = "Pick X" + potion.buff;
                                    break;
                                case PotionClass.PotionType.LuckyPower: // TODO
                                    break;
                                default:
                                    break;
                            }
                            buffPrompt.canvasRenderer.SetAlpha(1f);
                            buffPrompt.CrossFadeAlpha(0f, 1.5f, false);
                        }
                        break;
                    }
                    else
                    {
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
            string objName = other.gameObject.name.Split(" ")[0];
            if (other.gameObject.CompareTag("Food")) {
                Debug.Log("collision with food");

                // Pickup food - By Hou
                FoodClass food = inventory.foods[objName];
                if (gameController.isTutorialOn == 0 && PlayerPrefs.GetInt(objName + "_new") == 1)
                {
                    _uiManager.CreateItemBox(objName, food.itemIntro, food.itemIcon);
                    PlayerPrefs.SetInt(objName + "_new", 0);
                }
                inventory.Add(food);
                SoundManager.instance.PlaySoundFoodPickup();
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
                ToolClass tool = inventory.tools[objName];
                if (gameController.isTutorialOn == 0 && PlayerPrefs.GetInt(objName + "_new") == 1)
                {
                    _uiManager.CreateItemBox(objName, tool.itemIntro, tool.itemIcon);
                    PlayerPrefs.SetInt(objName + "_new", 0);
                }
                inventory.Add(tool);
                SoundManager.instance.PlaySoundToolPickup();

                Destroy(other.gameObject);
            }
            if (other.gameObject.CompareTag("Target"))
            {
                Debug.Log("collision with target");
                // Reach the target star
                gameController.didSucceedChange = 1;

                Destroy(other.gameObject);
            }
            if (other.gameObject.CompareTag("Monster")) {
                Debug.Log("collision with monster");

                currentHealth -= 10f;
            }
        }

        void Attack(Transform attackZone, ItemClass item, int inventoryIndex)
        {
            // What objects are within a circle at that attack zone
            Collider2D[] hits = Physics2D.OverlapCircleAll(attackZone.position, 0.1f);
            if (hits.Length == 0) // facing no obstacles
            {
                if (item.GetTool().toolType == ToolClass.ToolType.Trade)
                {
                    print("There is no trader to trade with.");
                }
                else
                {
                    print("There is no barrier to be destroyed.");
                }
            }

            // Handle each hit target
            foreach (Collider2D hit in hits)
            {
                string objName = hit.gameObject.name.Split(" ")[0];
                if (hit.gameObject.CompareTag("Barrier"))
                {
                    BarrierClass barrier = inventory.barriers[objName];
                    // Verify the relation between the tool and the barrier
                    if (barrier.availableTools.Contains(item.GetTool()))
                    {
                        inventory.Remove(inventoryIndex);
                        _animator.SetTrigger("Attack");

                        // Destroy barrier
                        if (hit.gameObject.GetComponent<BarrierController>().LoseHP(item.GetTool().damage * damageBuff) <= 0)
                        {
                            Destroy(hit.gameObject);
                        }

                        //trigger "Get Star" tutorial
                        print("trigger last tutorial! " + gameController.tutorialStageChange);
                        if (gameController.isTutorialOn == 1 && gameController.tutorialStageChange == (int)TutorialStages.AttackBarrier)
                        {
                            gameController.tutorialStageChange = (int)TutorialStages.GetStar;
                        }
                    }
                    else
                    {
                        print(barrier.name + " can not be destroyed by " + item.name);
                    }
                }
                else // facing some obstacles but not barrier 
                {
                    print("There is no barrier to be destroyed.");
                }
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
