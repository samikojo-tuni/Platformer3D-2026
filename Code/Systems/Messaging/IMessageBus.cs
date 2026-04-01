using System;

namespace GA.Common.Messaging
{
	public interface IMessageBus : IDisposable
	{
		/// <summary>
		/// Sends the message to all recipients.
		/// </summary>
		/// <param name="message">The message that will be sent.</param>
		/// <typeparam name="TMessage">The type of the message.</typeparam>
		void Send<TMessage>(TMessage message) where TMessage : IMessage;

		/// <summary>
		/// Registers the message listener.
		/// </summary>
		/// <param name="action">The action receiver should execute when a message arrives.</param>
		/// <typeparam name="TMessage">The type of the message.</typeparam>
		/// <returns>The subscription</returns>
		ISubscription<TMessage> Subscribe<TMessage>(Action<TMessage> action)
			where TMessage : IMessage;

		/// <summary>
		/// Unregisters from listening to the messages of type TMessage.
		/// </summary>
		/// <param name="subscription">The subscription object Subscribe method has returned.</param>
		/// <typeparam name="TMessage">The type of the message.</typeparam>
		/// <returns><c>True</c>, if unsubscribed successfully, <c>false</c> otherwise.</returns>
		bool Unsubscribe<TMessage>(ISubscription<TMessage> subscription)
			where TMessage : IMessage;
	}
}