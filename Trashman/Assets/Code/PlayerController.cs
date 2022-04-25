using System;
using System.Linq;
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
        public List<GameObject> attackZones;
        public GameObject attackZonePrefab;
        public InterfaceManager interfaceManager;
        public GameController gameController;

        public GameObject buffPanel;
        TMP_Text damage;
        TMP_Text range;
        TMP_Text pick;
        TMP_Text lucky;
        int maxDamageBuff = 16;
        int maxRangeBuff = 4;
        int maxPickBuff = 4;
        float maxLuckyBuff = 1;

        public GameObject statePanel;
        TMP_Text coin;
        TMP_Text star;

        AudioSource walkSound;
        public TMP_Text buffPrompt;
        public TMP_Text addSymbol;
        public Image treasurePrompt;

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
        public float luckyBuff = 0;


        // Start is called before the first frame update
        void Start() {
            damage = buffPanel.transform.GetChild(0).transform.GetComponent<TMP_Text>();
            range = buffPanel.transform.GetChild(1).transform.GetComponent<TMP_Text>();
            pick = buffPanel.transform.GetChild(2).transform.GetComponent<TMP_Text>();
            lucky = buffPanel.transform.GetChild(3).transform.GetComponent<TMP_Text>();
            coin = statePanel.transform.GetChild(0).transform.GetChild(1).transform.GetComponent<TMP_Text>();
            star = statePanel.transform.GetChild(1).transform.GetChild(1).transform.GetComponent<TMP_Text>();
            coin.text = "" + PlayerPrefs.GetInt("coin", 0);
            star.text = "" + PlayerPrefs.GetInt("star", 0);

            prompt.enabled = false;
            buffPrompt.text = "";
            addSymbol.text = "";
            treasurePrompt.enabled = false;

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
                                    if (barrier.barrierType == BarrierClass.BarrierType.Trader)
                                    {
                                        _uiManager.CreateHintBox(objName, barrier.itemIntro, barrier.itemIcon, barrier.getTreasureNameList(), barrier.getDropNameList());
                                    }
                                    else
                                    {
                                        _uiManager.CreateHintBox(objName, barrier.itemIntro, barrier.itemIcon, barrier.getToolNameList(), barrier.getDropNameList());
                                    }
                                    PlayerPrefs.SetInt(objName + "_new", 0);
                                    interfaceManager.RefreshStoreUI(objName, "Barrier");
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
                            if (item.name == "WholeApple") {
                                SoundManager.instance.PlaySoundEat1();
                            } else {
                                SoundManager.instance.PlaySoundEat4();
                            }
                            inventory.Remove(i);
                            GainHealth(item.GetFood().healthAdded);
                        }
                        else if (item.GetTool() != null)
                        {
                            if (item.name == "Bomb") {
                                SoundManager.instance.PlaySoundExplosion();
                            } else {
                                SoundManager.instance.PlaySoundSword();
                            }
                            // Convert enrumeration to an index
                            int facingDirectionIndex = (int)facingDirection;

                            // Current attack zones number is not enough
                            if (attackZones.Count < item.GetTool().range * rangeBuff)
                            {
                                int addNumber = item.GetTool().range * rangeBuff - attackZones.Count;
                                for (int j = 0; j < addNumber; j++)
                                {
                                    GameObject addAttackZone = Instantiate(attackZonePrefab, gameObject.transform);
                                    Transform[] transforms = addAttackZone.GetComponentsInChildren<Transform>(); // length=5
                                    Transform[] previousTransforms = attackZones.Last().GetComponentsInChildren<Transform>(); // length=5
                                    transforms[1].position = new Vector3(previousTransforms[1].position.x, previousTransforms[1].position.y + 1);
                                    transforms[2].position = new Vector3(previousTransforms[2].position.x, previousTransforms[2].position.y - 1);
                                    transforms[3].position = new Vector3(previousTransforms[3].position.x - 1, previousTransforms[3].position.y);
                                    transforms[4].position = new Vector3(previousTransforms[4].position.x + 1, previousTransforms[4].position.y);
                                    attackZones.Add(addAttackZone);
                                }
                            }
                            int toBeRemoved = 0;
                            for (int j = 0; j < item.GetTool().range * rangeBuff; j++)
                            {
                                Transform[] attackZonesGroup = attackZones[j].GetComponentsInChildren<Transform>(); // length=5
                                if (item.GetTool().toolType == ToolClass.ToolType.Attack)
                                {
                                    Transform attackZone = attackZonesGroup[facingDirectionIndex + 1]; // +1: skip the transform of itself
                                    toBeRemoved += Attack(attackZone, item, i);
                                }
                                else if (item.GetTool().toolType == ToolClass.ToolType.CircleAttack)
                                {
                                    for (int k = 0; k < 4; k++)
                                    {
                                        Transform attackZone = attackZonesGroup[k + 1]; // +1: skip the transform of itself
                                        toBeRemoved += Attack(attackZone, item, i);
                                    }
                                }
                            }
                            if (toBeRemoved != 0)
                            {
                                inventory.Remove(i);
                            }
                        }
                        else if (item.GetTreasure() != null)
                        {
                            // Convert enrumeration to an index
                            int facingDirectionIndex = (int)facingDirection;

                            Transform[] attackZonesGroup = attackZones[0].GetComponentsInChildren<Transform>(); // length=5
                            Transform attackZone = attackZonesGroup[facingDirectionIndex + 1]; // +1: skip the transform of itself
                            Trade(attackZone, item, i);
                        }
                        else if (item.GetPotion() != null)
                        {
                            SoundManager.instance.PlaySoundDrink();
                            PotionClass potion = item.GetPotion();
                            //buffPrompt.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.7f);
                            switch (potion.potionType)
                            {
                                case PotionClass.PotionType.DamagePower:
                                    if (damageBuff * potion.buff > maxDamageBuff)
                                    {
                                        damageBuff = maxDamageBuff;
                                        buffPrompt.text = "<gradient=GoldWhite>Reach Max Buff</gradient>";
                                    }
                                    else
                                    {
                                        inventory.Remove(i);
                                        damageBuff *= potion.buff;
                                        buffPrompt.text = "<gradient=Red>Damage X" + potion.buff + "</gradient>";
                                    }
                                    damage.text = "Damage X" + damageBuff;
                                    break;
                                case PotionClass.PotionType.RangePower:
                                    if (rangeBuff * potion.buff > maxRangeBuff)
                                    {
                                        rangeBuff = maxRangeBuff;
                                        buffPrompt.text = "<gradient=GoldWhite>Reach Max Buff</gradient>";
                                    }
                                    else
                                    {
                                        inventory.Remove(i);
                                        rangeBuff *= potion.buff;
                                        buffPrompt.text = "<gradient=Blue>Range X" + potion.buff + "</gradient>";
                                    }
                                    range.text = "Range X" + rangeBuff;
                                    break;
                                case PotionClass.PotionType.PickPower:
                                    if (pickBuff * potion.buff > maxPickBuff)
                                    {
                                        pickBuff = maxPickBuff;
                                        buffPrompt.text = "<gradient=GoldWhite>Reach Max Buff</gradient>";
                                    }
                                    else
                                    {
                                        inventory.Remove(i);
                                        pickBuff *= potion.buff;
                                        buffPrompt.text = "<gradient=Green>Pick X" + potion.buff + "</gradient>";
                                    }
                                    pick.text = "Pick X" + pickBuff;
                                    break;
                                case PotionClass.PotionType.LuckyPower:
                                    if (luckyBuff == maxLuckyBuff)
                                    {
                                        buffPrompt.text = "<gradient=GoldWhite>Reach Max Buff</gradient>";
                                    }
                                    else if (luckyBuff + potion.lucky > maxLuckyBuff)
                                    {
                                        buffPrompt.text = "<gradient=GoldWhite>Can't use.\nBuff will exceed.</gradient>";
                                    }
                                    else
                                    {
                                        inventory.Remove(i);
                                        luckyBuff += potion.lucky;
                                        buffPrompt.text = "<gradient=Orange>Lucky +" + potion.lucky * 100 + "%</gradient>";
                                    }
                                    lucky.text = "Lucky +" + luckyBuff * 100 + "%";
                                    break;
                                default:
                                    break;
                            }
                            buffPrompt.canvasRenderer.SetAlpha(1f);
                            buffPrompt.CrossFadeAlpha(0f, 2f, false);
                        }
                        break;
                    }
                    else
                    {
                        //TODO: User try to use non-existing item
                        print("Player try to use non-existing item.");
                    }
                }
            }
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
                    interfaceManager.RefreshStoreUI(objName, "Food");
                }
                inventory.Add(food, true);
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
                    interfaceManager.RefreshStoreUI(objName, "Tool");
                }
                inventory.Add(tool, true);
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

        int Attack(Transform attackZone, ItemClass item, int inventoryIndex)
        {
            int toRemove = 0;
            // What objects are within a circle at that attack zone
            Collider2D[] hits = Physics2D.OverlapCircleAll(attackZone.position, 0.1f);
            if (hits.Length == 0) // facing no obstacles
            {
                print("There is no barrier to be destroyed.");
            }

            // Handle each hit target
            foreach (Collider2D hit in hits)
            {
                string objName = hit.gameObject.name.Split(" ")[0];
                if (hit.gameObject.CompareTag("Barrier") || hit.gameObject.CompareTag("Monster"))
                {
                    BarrierClass barrier = inventory.barriers[objName];
                    if (barrier.barrierType != BarrierClass.BarrierType.Trader)
                    {
                        // Verify the relation between the tool and the barrier
                        if (barrier.availableTools.Contains(item.GetTool()))
                        {
                            //inventory.Remove(inventoryIndex);
                            toRemove = 1;
                            _animator.SetTrigger("Attack");

                            // Destroy barrier
                            BarrierController barrierController = hit.gameObject.GetComponent<BarrierController>();
                            if (barrierController.LoseHP(item.GetTool().damage * damageBuff) <= 0)
                            {
                                AddCoins(barrier.bounty);
                                DropTreasure(barrier);
                                Destroy(hit.gameObject);
                            }

                            //trigger "Get Star" tutorial
                            print("trigger last tutorial! " + gameController.tutorialStageChange);
                            if (gameController.isTutorialOn == 1 && gameController.tutorialStageChange == (int)TutorialStages.AttackBarrier)
                            {
                                gameController.tutorialStageChange = (int)TutorialStages.GetStar;
                            }
                            return toRemove;
                        }
                        else // tool does not match the barrier/monster
                        {
                            
                            print(barrier.name + " can not be destroyed by " + item.name);
                            return toRemove;
                        }
                    }
                    else // trader type barrier
                    {
                        print(barrier.name + " is a trader " + item.name);
                        return toRemove;
                    }
                }
                else // facing some obstacles but not barrier 
                {
                    print("There is no barrier to be destroyed.");
                    return toRemove;
                }
            }
            return toRemove;
        }

        void Trade(Transform attackZone, ItemClass item, int inventoryIndex)
        {
            // What objects are within a circle at that attack zone
            Collider2D[] hits = Physics2D.OverlapCircleAll(attackZone.position, 0.1f);
            if (hits.Length == 0) // facing no obstacles
            {
                print("There is no trader to trade with.");
            }

            // Handle each hit target
            foreach (Collider2D hit in hits)
            {
                string objName = hit.gameObject.name.Split(" ")[0];
                if (hit.gameObject.CompareTag("Barrier"))
                {
                    BarrierClass barrier = inventory.barriers[objName];
                    if (barrier.barrierType == BarrierClass.BarrierType.Trader)
                    {
                        // Verify the relation between the treasure and the trader
                        if (barrier.availableTreasures.Contains(item.GetTreasure()))
                        {
                            inventory.Remove(inventoryIndex);
                            _animator.SetTrigger("Attack");

                            // add coins
                            AddCoins(barrier.bounty);

                            // Destroy trader
                            Destroy(hit.gameObject);
                        }
                        else // treasure does not match the trader
                        {
                            print(barrier.name + " does not like " + item.name);
                        }
                    }
                    else // barrier or monster type barrier
                    {
                        print(barrier.name + " is not a trader " + item.name);
                    }
                }
                else // facing some obstacles but not barrier 
                {
                    print("There is no trader to trade with.");
                }
            }
        }

        // Drop treasure with lucky buff
        void DropTreasure(BarrierClass barrier)
        {
            System.Random rd = new System.Random();
            int prob = rd.Next(0, 10000);
            int lowerBoundary = 0;
            for (int i = 0; i < barrier.dropTreasureProbs.Count; i++)
            {
                if (lowerBoundary <= prob && prob < lowerBoundary + (barrier.dropTreasureProbs[i] * 10000  * ( 1 + luckyBuff)))
                {
                    TreasureClass treasure = barrier.dropTreasures[i];

                    // prompt
                    addSymbol.text = "+";
                    treasurePrompt.enabled = true;
                    treasurePrompt.sprite = treasure.itemIcon;
                    addSymbol.canvasRenderer.SetAlpha(1f);
                    treasurePrompt.canvasRenderer.SetAlpha(1f);
                    addSymbol.CrossFadeAlpha(0f, 2f, false);
                    treasurePrompt.CrossFadeAlpha(0f, 2f, false);

                    // First time meet will show item box
                    if (gameController.isTutorialOn == 0 && PlayerPrefs.GetInt(treasure.name + "_new") == 1)
                    {
                        _uiManager.CreateItemBox(treasure.name, treasure.itemIntro, treasure.itemIcon);
                        PlayerPrefs.SetInt(treasure.name + "_new", 0);
                    }
                    // Add to collection
                    int currentQuantity = PlayerPrefs.GetInt(treasure.name + "_quantity", 0);
                    PlayerPrefs.SetInt(treasure.name + "_quantity", currentQuantity + 1);
                    interfaceManager.RefreshStoreUI(treasure.name, "Treasure");

                    SoundManager.instance.PlaySoundToolPickup();
                    break;
                }
                lowerBoundary += (int)(barrier.dropTreasureProbs[i] * 10000 * (1 + luckyBuff));
            }
        }

        // Add coins
        public void AddCoins(int quantity)
        {
            int currentCoinQuantity = PlayerPrefs.GetInt("coin", 0);
            PlayerPrefs.SetInt("coin", currentCoinQuantity + quantity);
            coin.text = "" + PlayerPrefs.GetInt("coin");
        }

        // Sub coins
        public void SubCoins(int quantity)
        {
            int currentCoinQuantity = PlayerPrefs.GetInt("coin");
            PlayerPrefs.SetInt("coin", currentCoinQuantity - quantity);
            coin.text = "" + PlayerPrefs.GetInt("coin");
        }

        // Add stars
        public void AddStars()
        {
            int currentStarQuantity = PlayerPrefs.GetInt("star", 0);
            PlayerPrefs.SetInt("star", currentStarQuantity + 1);
            star.text = "" + PlayerPrefs.GetInt("star");
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
