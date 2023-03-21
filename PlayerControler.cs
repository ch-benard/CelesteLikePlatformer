using Godot;
using System;

namespace Namespace
{
    public class PlayerControler : KinematicBody2D
    {
        private readonly int speed = 200;
        private readonly int gravity = 400;
        private readonly float friction = .1f;
        private readonly float acceleration = .5f;
        private readonly int jumpHeight = 400;
        private readonly int dashSpeed = 500;
        private bool isDashing = false;
        private float dashTimer = .2f;
        private readonly float dashTimerReset = .2f;
        private bool isDashAvailable = true;
         Vector2 velocity = new Vector2();

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            if (!isDashing) {
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
                    velocity.y -= jumpHeight;
                }
                isDashAvailable = true;
            }

            if (isDashAvailable && Input.IsActionJustPressed("dash"))
            {
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