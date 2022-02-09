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
        [SerializeField]
        Vector2 targetPos = new Vector2(-2.7f, 1.9f);
        Vector2 lastPos = new Vector2(-2.7f, 1.9f);

        float timeElapsed = 0;
        float lerpDuration = 1;

        float restTime = 0.9f;
        float restTimer = 0;

        Vector2 way2Go;
        Animator animator;


        // Start is called before the first frame update
        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            _boxCollider = GetComponent<BoxCollider2D>();
            animator = GetComponent<Animator>();
        }

        void FixedUpdate() {

            animator.SetFloat("Way2GoX", targetPos.x - transform.position.x);
            animator.SetFloat("Way2GoY", targetPos.y - transform.position.y);

            if (timeElapsed < lerpDuration) {
                _rigidbody2D.MovePosition(Vector2.Lerp(lastPos, targetPos, timeElapsed / lerpDuration));
                timeElapsed += Time.deltaTime;
            } else {
                lastPos = targetPos;
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

                if (hit.transform == null) {
                    lastPos = targetPos;
                    targetPos += new Vector2(h, v);

                }
                restTimer = 0;
            }

            /*if (_rigidbody2D.velocity.magnitude > 0) {
                animator.speed = _rigidbody2D.velocity.magnitude / 3f;
            } else {
                animator.speed = 1f;
            }*/
        }

        // Update is called once per frame
        void Update()
        {
            

        }

    }
}

