using System.Collections;
using System.Collections.Generic;
using Trashman;
using UnityEngine;

public class StarController : MonoBehaviour
{
    public GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            print("123");
            gameController.didSucceedChange = 1;
            Destroy(gameObject);
        }
    }
}
