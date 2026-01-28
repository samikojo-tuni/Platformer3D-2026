using Godot;
using System;

namespace GA.Platformer3D
{
	public partial class PlayerCharacter : Character
	{

		public override void _Process(double delta)
		{
			bool isOnFloor = IsOnFloor();
			if (isOnFloor && Input.IsActionJustPressed(InputConfig.STRIKE_NAME))
			{
				IsStriking = true;
			}
			else if (IsStriking && Input.IsActionJustReleased(InputConfig.STRIKE_NAME))
			{
				IsStriking = false;
			}
		}

		/// <summary>
		/// Physics calculations are performed in this method.
		/// </summary>
		/// <param name="delta">The time passed since the previous frame.</param>
		public override void _PhysicsProcess(double delta)
		{
			Vector3 velocity = Velocity;

			bool isOnFloor = IsOnFloor();

			// Add the gravity.
			if (!isOnFloor)
			{
				velocity += GetGravity() * (float)delta;
			}

			// Handle Jump.
			if (Input.IsActionJustPressed(InputConfig.JUMP_NAME) && isOnFloor && !IsStriking)
			{
				velocity.Y = JumpVelocity;
				IsJumping = true;
			}
			else if (IsJumping && isOnFloor)
			{
				// Character landed.
				IsJumping = false;
			}

			// Get the input direction and handle the movement/deceleration.
			// As good practice, you should replace UI actions with custom gameplay actions.
			Vector2 inputDir = Input.GetVector(InputConfig.MOVE_LEFT_NAME, InputConfig.MOVE_RIGHT_NAME,
				InputConfig.MOVE_UP_NAME, InputConfig.MOVE_DOWN_NAME, InputConfig.DEAD_ZONE);

			Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
			if (direction != Vector3.Zero)
			{
				velocity.X = direction.X * MaxSpeed;
				velocity.Z = direction.Z * MaxSpeed;

				// There's input. Rotate character.
				if (CharacterRig != null)
				{
					Vector3 targetDirection = new Vector3(direction.X, 0, direction.Z);
					float targetAngle = Mathf.Atan2(-targetDirection.X, -targetDirection.Z);
					Quaternion targetRotation = new Quaternion(Vector3.Up, targetAngle);
					CharacterRig.Quaternion = CharacterRig.Quaternion.Slerp(targetRotation, RotationSpeed * (float)delta);
				}
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, MaxSpeed);
				velocity.Z = Mathf.MoveToward(Velocity.Z, 0, MaxSpeed);
			}

			Velocity = velocity;
			MoveAndSlide();
		}
	}
}