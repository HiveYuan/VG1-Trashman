using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trashman
{
    public class PlayerController : MonoBehaviour
    {
        //outlet
        Rigidbody2D _rigidbody2D;
        SpriteRenderer sprite;
        BoxCollider2D _boxCollider;

        public Vector2 targetPos = new Vector2(-2.7f, 1.9f);
        Vector2 lastPos = new Vector2(-2.7f, 1.9f);

        // health bar
        public float maxHealth = 40f;
        public float currentHealth;

        public Health health;


        float timeElapsed = 0;
        float lerpDuration = 1;

        float restTime = 0.9f;
        float restTimer = 0;

        Animator _animator;



        // Start is called before the first frame update
        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();

        }

        void FixedUpdate() {

            if (timeElapsed < lerpDuration) {
                transform.position = Vector2.Lerp(lastPos, targetPos, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
            } else {
                lastPos = targetPos;
                transform.position = targetPos;
                timeElapsed = 0;
            }

            // Not rest for enough time: not moving
            restTimer += Time.deltaTime;
            if (restTimer < restTime)
                return;

            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            if (h != 0) {
                v = 0;
            }

            if (h != 0 || v != 0) {

                _boxCollider.enabled = false;
                RaycastHit2D hit = Physics2D.Linecast(targetPos, targetPos + new Vector2(h, v));
                _boxCollider.enabled = true;
                Debug.Log(hit.collider);
                if (hit.collider == null) {
                    lastPos = targetPos;
                    targetPos += new Vector2(h, v);
                    LoseHealth(2f);
                } else {
                    switch (hit.collider.tag) {
                        case "Wall":
                            break;

                        case "Food":
                            GainHealth(6f);
                            targetPos += new Vector2(h, v);
                            Destroy(hit.transform.gameObject);
                            break;
                    }

                }
                restTimer = 0;
            }
            
        }


        // Update is called once per frame
        void Update()
        {
            if((Math.Abs(transform.position.x - targetPos.x) < 0.001f) && (Math.Abs(transform.position.y - targetPos.y) < 0.001f)) {
                _animator.ResetTrigger("Move");
            } else {
                _animator.SetTrigger("Move");
                _animator.SetFloat("Way2GoX", targetPos.x - transform.position.x);
                _animator.SetFloat("Way2GoY", targetPos.y - transform.position.y);
            }


            /*if (_rigidbody2D.velocity.magnitude > 0) {
                animator.speed = _rigidbody2D.velocity.magnitude / 3f;
            } else {
                animator.speed = 1f;
            }*/

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
        }

        // Health - By Hou
        void LoseHealth( float hp ) {
            if (currentHealth > 0f) {
                currentHealth -= hp;
            }

            health.SetHealth(currentHealth, maxHealth);
        }
    }
}

