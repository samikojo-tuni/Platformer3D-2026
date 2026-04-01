using GA.Common.Messaging;

namespace GA.Platformer3D.Messages
{
	public class HealthChangedMessage : IMessage
	{
		public IHealth Health { get; }

		public HealthChangedMessage(IHealth health)
		{
			Health = health;
		}
	}
}