using Godot;
using System;

public class PlayerControler : KinematicBody2D
{
   private int speed = 100;
   private int gravity = 5000;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
public override void _Process(float delta)
{
   Vector2 velocity = new Vector2();
   if (Input.IsActionPressed("ui_left")) {
      velocity.x -= speed;
   }
   if (Input.IsActionPressed("ui_right")) {
      velocity.x += speed;
   }
   if (Input.IsActionJustPressed("jump")) {
      if (IsOnFloor()) {
         velocity.y -= 4000;
      }
   }

   velocity.y += gravity * delta;
   MoveAndSlide(velocity, Vector2.Up);
}
}
