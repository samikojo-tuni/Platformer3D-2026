using System;

namespace GA.Common.Messaging
{
	/// <summary>
	/// A generic interface for message subscriptions. The related message has to implement the IMessage
	/// interface.
	/// </summary>
	/// <typeparam name="TMessage">The type of the message.</typeparam>
	public interface ISubscription<TMessage>
	 where TMessage : IMessage
	{
		/// <summary>
		/// A reference to the method which is executed when the message is received.
		/// </summary>
		Action<TMessage> Action { get; }

		// TODO: Consider implemeting Unsubscribe here.
	}
}