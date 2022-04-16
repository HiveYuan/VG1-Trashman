using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trashman {
    public class Monster : MonoBehaviour {
        public Rigidbody2D _rigidbody2D;
        public Transform pos_left, pos_right;   // Left and right boundaries
        public float speed;
        public bool faceLeft;   // Face to the left
        public float leftx, rightx;

        void Start() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            leftx = pos_left.position.x;
            rightx = pos_right.position.x;

            Destroy(pos_left.gameObject);
            Destroy(pos_right.gameObject);
        }

        void Update() {
            move();
        }

        // Methods
        void move() {
            if(faceLeft) {
                _rigidbody2D.velocity = new Vector2(-speed, _rigidbody2D.velocity.y);

                if(transform.position.x < leftx) {
                    faceLeft = false;
                }
            }
            else {
                _rigidbody2D.velocity = new Vector2(speed, _rigidbody2D.velocity.y);

                if(transform.position.x > rightx) {
                    faceLeft = true;
                }
            }
        }
    }
}






// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEngine;
// using UnityEngine.UI;

// namespace Trashman {
//     public class Monster : MonoBehaviour
//     {
//         public float speed = 10f;   // Move speed
//         Animator = animator;
//         Rigidbody2D = _rigidbody2D;
//         public Vector2 movePlay;    // Move vector
//         public float moveTimer;
//         public float moveTime = 3;  // Move time

//         // Start is called before the first frame update
//         void Start()
//         {
//             _rigidbody2D = GetComponent<Rigidbody2D>();
//             animator = GetComponent<Animator>();
//             moveAuto();
//         }

//         // Update is called once per frame
//         void Update()
//         {
//             if(moveTimer >= 0) {
//                 Vector3 thisScalse = this.transform.localScale;
//                 Vector2 position = _rigidbody2D.position;
//                 position += movePlay * speed * Time.deltaTime;
//                 _rigidbody2D.MovePosition(position);
//                 moveTimer -= Time.deltaTime;
//             }
//             else {
//                 moveAuto();
//                 moveTimer = moveTime;
//             }
//         }

//         // Methods
//         void moveAuto() {
//             System.Random rd = new System.Random();
//             float x = rd.Next(-1, 2);
//             float y = rd.Next(-1, 2);

//             movePlay = new Vector2(x, y);
//             Vector3 thisScalse = this.transform.localScale;
//             if(x != 0) {
//                 thisScalse.x = Math.Abs(thisScalse) * x;
//                 transform.localScale = thisScalse;
//             }

//             animator.SetFloat("speed", movePlay.magnitude);
//         }
//     }
// }
