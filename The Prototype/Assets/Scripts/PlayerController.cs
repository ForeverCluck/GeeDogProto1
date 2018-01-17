using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  Animator anim;
  Rigidbody2D rb2d;
  SpriteRenderer sr;

  float speed = 6f;
  float lauchValue = 300f;

  bool jump = false;

	void Start ()
  {
    anim = GetComponent<Animator>();
    rb2d = GetComponent<Rigidbody2D>();
    sr = GetComponent<SpriteRenderer>();
	}

	void Update ()
  {
    if (Input.GetButtonDown("Jump"))
    {
      jump = true;
    }
      
	}

  // Physics update.
  void FixedUpdate()
  {
    PlayerMove();
  }

  void PlayerMove()
  {
    float h = Input.GetAxis("Horizontal");

    Vector2 movement = new Vector2(h * speed, rb2d.velocity.y);

    rb2d.velocity = movement;

    if (jump)
    {
      Debug.Log("Jump");
      rb2d.AddForce(new Vector2(0f, lauchValue));
      jump = false;
    }

    FlipSpriteX(h);

    Animations(movement);
  }

  void Animations(Vector2 movement)
  {
    // Locomotion.
    anim.SetFloat("VelocityX", Mathf.Abs(movement.x));
    anim.SetFloat("VelocityY", Mathf.Abs(movement.y));

    // Attacking.

    // Dying.

    // Jumping.
  }

  void FlipSpriteX(float horizontal)
  {
    bool flipX = (sr.flipX);

    if (horizontal > 0)
      flipX = false;
    else if (horizontal < 0)
      flipX = true;

    sr.flipX = flipX;
  }
}
