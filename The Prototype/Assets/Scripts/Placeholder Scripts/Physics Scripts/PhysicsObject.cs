﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
  public float gravityModifier = 1f;
  public float minGroundNormalY = .65f;

  protected bool isGrounded;
  protected Vector2 groundNormal;
  protected Vector2 targetVelocity;
  protected Vector2 velocity;
  protected Rigidbody2D rb2d;
  protected ContactFilter2D contactFilter;
  protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
  protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

  protected const float MinMoveDistance = 0.001f;
  protected const float ShellRadius = 0.01f;

  void OnEnable()
  {
    rb2d = GetComponent<Rigidbody2D>();
  }

  void Start ()
  {
    contactFilter.useTriggers = false;
    contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    contactFilter.useLayerMask = true;
	}

  void Update()
  {
    targetVelocity = Vector2.zero;
    ComputeVelocity();
  }

  protected virtual void ComputeVelocity() {}

  void FixedUpdate()
  {
    velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
    velocity.x = targetVelocity.x;

    isGrounded = false;

    Vector2 deltaPosition = velocity * Time.deltaTime;

    Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

    Vector2 move = moveAlongGround * deltaPosition.x;

    Movement(move, false);

    move = Vector2.up * deltaPosition.y;

    Movement(move, true);
  }

  // Move the object by the value passed in.
  void Movement(Vector2 move, bool yMovement)
  {
    float distance = move.magnitude;

    if (distance > MinMoveDistance)
    {
      int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + ShellRadius);

      hitBufferList.Clear();
      for (int i = 0; i < count; i++)
      {
        hitBufferList.Add(hitBuffer[i]);
      }

      foreach (RaycastHit2D hit in hitBufferList)
      {
        Vector2 currentNormal = hit.normal;
        if (currentNormal.y > minGroundNormalY)
        {
          isGrounded = true;
          if (yMovement)
          {
            groundNormal = currentNormal;
            currentNormal.x = 0;
          }
        }

        float projection = Vector2.Dot(velocity, currentNormal);
        if (projection < 0)
        {
          velocity = velocity - projection * currentNormal;
        }

        float modifiedDistance = hit.distance - ShellRadius;
        distance = modifiedDistance < distance ? modifiedDistance : distance;
      }
    }

    rb2d.position = rb2d.position + move.normalized * distance;
  }
}
