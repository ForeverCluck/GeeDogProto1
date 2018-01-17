using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlaformerController : PhysicsObject
{
  public float jumpTakeOffSpeed = 14f;
  public float MaxSpeed = 7f;

  private SpriteRenderer spriteRenderer;
  private Animator animator;

	void Awake ()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    animator = GetComponent<Animator>();
	}

  protected override void ComputeVelocity()
  {
    Vector2 move = Vector2.zero;

    move.x = Input.GetAxis("Horizontal");

    if (Input.GetButtonDown("Jump") && isGrounded)
    {
      velocity.y = jumpTakeOffSpeed;
    }
    else if (Input.GetButtonUp("Jump"))
    {
      if (velocity.y > 0)
        velocity.y = velocity.y * .5f;
    }

    bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));

    if (flipSprite)
      spriteRenderer.flipX = !spriteRenderer.flipX;

    animator.SetBool("grounded", isGrounded);
    animator.SetFloat("VelocityX", Mathf.Abs(velocity.x) / MaxSpeed);

    targetVelocity = move * MaxSpeed;

    base.ComputeVelocity();
  }
}
