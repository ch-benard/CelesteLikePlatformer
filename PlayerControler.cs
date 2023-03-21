using Godot;
using System;

namespace Namespace
{
    public class PlayerControler : KinematicBody2D
    {
        private readonly int speed = 100;
        private readonly int gravity = 5000;
        private readonly float friction = .1f;
        private readonly float acceleration = 1.05f;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            Vector2 velocity = new Vector2();
            int direction = 0;

            if (Input.IsActionPressed("ui_left"))
            {
               direction -= 1;
            }

            if (Input.IsActionPressed("ui_right"))
            {
               direction += 1;
            }

            if (direction != 0) {
                velocity.x = Mathf.Lerp(velocity.x, direction * speed, acceleration);
            } else {
               velocity.x = Mathf.Lerp(velocity.x, 0, friction);
            }

            if (Input.IsActionJustPressed("jump"))
            {
                if (IsOnFloor())
                {
                    velocity.y -= 4000;
                }
            }

            velocity.y += gravity * delta;
            MoveAndSlide(velocity, Vector2.Up);
        }
    }
}