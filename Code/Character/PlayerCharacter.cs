using Godot;
using System;

namespace GA.Platformer3D
{
	public partial class PlayerCharacter : Character
	{

		// TODO: Refactor this. Move common code (between PlayerCharacter and EnemyCharacter) to the 
		// base class.

		/// <summary>
		/// Physics calculations are performed in this method.
		/// </summary>
		/// <param name="delta">The time passed since the previous frame.</param>
		public override void _PhysicsProcess(double delta)
		{
			Vector3 velocity = Velocity;

			// Add the gravity.
			if (!IsOnFloor())
			{
				velocity += GetGravity() * (float)delta;
			}

			// Handle Jump.
			if (Input.IsActionJustPressed("jump") && IsOnFloor())
			{
				velocity.Y = JumpVelocity;
			}

			// Get the input direction and handle the movement/deceleration.
			// As good practice, you should replace UI actions with custom gameplay actions.
			Vector2 inputDir = Input.GetVector("left", "right", "up", "down");			
			Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
			if (direction != Vector3.Zero)
			{
				velocity.X = direction.X * MaxSpeed;
				velocity.Z = direction.Z * MaxSpeed;

				// There's input. Rotate character.
				if (CharacterRig != null)
				{
					Vector3 targetDirection =  new Vector3(direction.X, 0, direction.Z);
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