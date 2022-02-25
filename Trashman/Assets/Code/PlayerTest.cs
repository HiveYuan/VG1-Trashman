using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trashman
{
    public class PlayerTest : MonoBehaviour
    {
        //outlet
        Rigidbody2D _rigidbody2D;
        SpriteRenderer sprite;
        BoxCollider2D _boxCollider;

        float moveSpeed = 5f;
        float healthLoseSpeed = 10f;
        float x_direction = 0f;
        float y_direction = 0f;
        // health bar
        public float maxHealth = 40f;
        public float currentHealth = 40f;

        public Health health;



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

            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            if (Math.Abs(h) > float.Epsilon) {
                v = 0;
            }
            if (Math.Abs(v) > float.Epsilon) {
                h = 0;
            }
            Debug.Log(currentHealth);
            if (currentHealth > 1e-5) {
                _rigidbody2D.velocity = new Vector2(h, v) * moveSpeed;
                if (Math.Abs(h) > float.Epsilon || Math.Abs(v) > float.Epsilon) {
                    x_direction = h;
                    y_direction = v;
                    _animator.SetTrigger("Move");
                    LoseHealth(Time.deltaTime * healthLoseSpeed);

                } else {
                    _animator.ResetTrigger("Move");
                }
            } else {
                _rigidbody2D.velocity = new Vector2(0, 0);
                _animator.ResetTrigger("Move");
            }

            _animator.SetFloat("Way2GoX", x_direction);
            _animator.SetFloat("Way2GoY", y_direction);
        }


        // Update is called once per frame
        void Update()
        {


            /*if (_rigidbody2D.velocity.magnitude > 0) {
                animator.speed = _rigidbody2D.velocity.magnitude / 3f;
            } else {
                animator.speed = 1f;
            }*/

        }

        void OnCollisionEnter2D( Collision2D other ) {
            Debug.Log("collision in player");
            if (other.gameObject.CompareTag("Food")) {
                Debug.Log("collision with food");
                GainHealth(2f);
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

