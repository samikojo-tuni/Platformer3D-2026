using System;

namespace GA.Common.Messaging
{
	/// <summary>
	/// Represents a subscription to a specific message type.
	/// This class holds a reference to the action that should be executed when a message of type <typeparamref name="TMessage"/> is published.
	/// </summary>
	/// <typeparam name="TMessage"><inheritdoc/></typeparam>
	public class Subscription<TMessage> : ISubscription<TMessage>
		where TMessage : IMessage
	{
		/// <inheritdoc/>
		public Action<TMessage> Action { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Subscription{TMessage}"/> class.
		/// </summary>
		/// <param name="action">The action to execute when a message is received. This cannot be null.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is null.</exception>
		public Subscription(Action<TMessage> action)
		{
			Action = action ?? throw new ArgumentNullException(nameof(action));
		}
	}
}