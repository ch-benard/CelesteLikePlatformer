using Godot;
using System;

namespace Namespace
{
    public class PlayerControler : KinematicBody2D
    {
        private readonly int speed = 100;
        private readonly int gravity = 200;
        private readonly float friction = .1f;
        private readonly float acceleration = .5f;
        private readonly int jumpHeight = 100;
        private readonly int dashSpeed = 500;
        private bool isDashing = false;
        private float dashTimer = .2f;
        private readonly float dashTimerReset = .2f;
        private bool isDashAvailable = true;
        private bool isWallJumping = false;
        private float wallJumpTimer = .45f;
        private readonly float wallJumpTimerReset = .45f;
         Vector2 velocity = new Vector2();

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            if (!isDashing && !isWallJumping) {
               int direction = 0;

               if (Input.IsActionPressed("ui_left"))
               {
                  direction --;
               }

               if (Input.IsActionPressed("ui_right"))
               {
                  direction ++;
               }

               if (direction != 0) {
                  velocity.x = Mathf.Lerp(velocity.x, direction * speed, acceleration);
               } else {
                  velocity.x = Mathf.Lerp(velocity.x, 0, friction);
               }
            }

            if (IsOnFloor()) {
               if (Input.IsActionJustPressed("jump")) {
                    velocity.y = -jumpHeight;
                }
                isDashAvailable = true;
            }

            if (Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RayCast2dLeft").IsColliding()) {
               velocity.y = -jumpHeight;
               velocity.x = jumpHeight;
               isWallJumping = true;
            } else if(Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RayCast2dRight").IsColliding()) {
               velocity.y = -jumpHeight;
               velocity.x = -jumpHeight;
               isWallJumping = true;
            }

            if (isDashAvailable) {
                if(Input.IsActionJustPressed("dash")) {
                  if (Input.IsActionPressed("ui_left"))
                  {
                     velocity.x = -dashSpeed;
                     isDashing = true;
                  }

                  if (Input.IsActionPressed("ui_right"))
                  {
                     velocity.x = dashSpeed;
                     isDashing = true;
                  }
                  if (Input.IsActionPressed("ui_up"))
                  {
                     velocity.y = -dashSpeed;
                     isDashing = true;
                  }
                  if (Input.IsActionPressed("ui_right") && Input.IsActionPressed("ui_up")) {
                     velocity.x = dashSpeed;
                     velocity.y = -dashSpeed;
                     isDashing = true;
                  }
                  if (Input.IsActionPressed("ui_leftt") && Input.IsActionPressed("ui_up")) {
                     velocity.x = -dashSpeed;
                     velocity.y = -dashSpeed;
                     isDashing = true;
                  }
                  dashTimer = dashTimerReset;
                }
            }

            if (isWallJumping) {
               wallJumpTimer -= delta;
               if (wallJumpTimer <= 0) {
                  isWallJumping = false;
                  wallJumpTimer = wallJumpTimerReset;
               }
            }

            if (isDashing) {
               dashTimer -= delta;
               if (dashTimer <= 0) {
                  isDashing = false;
                  velocity = new Vector2(0, 0);
               }
            } else {
                  velocity.y += gravity * delta;
            }
            MoveAndSlide(velocity, Vector2.Up);
        }
    }
}