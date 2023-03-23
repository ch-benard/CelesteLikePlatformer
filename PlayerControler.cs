using Godot;
using System;
using System.Drawing.Printing;

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
    private bool canClimb = true;
    private int climbSpeed = 100;
    private bool isClimbing = false;
    private float climbTimer = 5f;
    private float climbTimerReset = 5f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (!isDashing && !isWallJumping) {
            ProcessMovement();
        }

        if (IsOnFloor()) {
            if (Input.IsActionJustPressed("jump")) {
                velocity.y = -jumpHeight;
            }
            canClimb = true;
            isDashAvailable = true;
        }

        if (canClimb) {
        ProcessClimb(delta);
        }

        if (!IsOnFloor()) {
            ProcessWallJump(delta);
        }

        if (isDashAvailable) {
            ProcessDash();
        }

        if (isDashing) {
            dashTimer -= delta;
            if (dashTimer <= 0) {
            isDashing = false;
            velocity = new Vector2(0, 0);
            }
        } else if (!isClimbing) {
            velocity.y += gravity * delta;
        } else {
            climbTimer -= delta;
            if (climbTimer <= 0) {
                isClimbing = false;
                canClimb = false;
                climbTimer = climbTimerReset;
            }
        }
        MoveAndSlide(velocity, Vector2.Up);
    }

    private void ProcessClimb(float delta) {
        if (Input.IsActionPressed("climb")) {
            if (canClimb && !isWallJumping) {
                isClimbing = true;
                if (Input.IsActionPressed("ui_up")
                    && (GetNode<RayCast2D>("RayCast2dLeft").IsColliding() || GetNode<RayCast2D>("RayCast2dRight").IsColliding()
                    || GetNode<RayCast2D>("RayCast2dLeftClimb").IsColliding() || GetNode<RayCast2D>("RayCast2dRightClimb").IsColliding())) {
                    velocity.y = -climbSpeed;
                } else if (Input.IsActionPressed("ui_down")) {
                    velocity.y = climbSpeed;
                } else {
                    velocity = new Vector2(0, 0);
                }
            } else {
                isClimbing = false;
            }
        }else {
            isClimbing = false;
        }
    }
    private void ProcessMovement() {
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
    private void ProcessDash() {
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
    private void ProcessWallJump(float delta) {
        if (Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RayCast2dLeft").IsColliding()) {
            velocity.y = -jumpHeight;
            velocity.x = jumpHeight;
            isWallJumping = true;
        } else if(Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RayCast2dRight").IsColliding()) {
            velocity.y = -jumpHeight;
            velocity.x = -jumpHeight;
            isWallJumping = true;
        }

        if (isWallJumping) {
            wallJumpTimer -= delta;
            if (wallJumpTimer <= 0) {
            isWallJumping = false;
            wallJumpTimer = wallJumpTimerReset;
            }
        }
    }
}