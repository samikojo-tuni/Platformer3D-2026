using System;
using System.Collections;
using System.Collections.Generic;

namespace GA.Common.Messaging
{
	/// <summary>
	/// A simple, single-threaded message bus implementation for decoupled communication.
	/// This implementation is not thread-safe and is optimized to reduce garbage collection.
	/// </summary>
	public class MessageBus : IMessageBus
	{
		private readonly IDictionary<Type, IList> _subscriptions =
			new Dictionary<Type, IList>();

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <typeparam name="TMessage"><inheritdoc/></typeparam>
		/// <param name="message"><inheritdoc/></param>
		/// <exception cref="ArgumentNullException">Thrown if the parameter is null.</exception>
		public void Send<TMessage>(TMessage message)
			where TMessage : IMessage
		{
			if (message == null)
			{
				throw new ArgumentNullException(nameof(message));
			}

			Type messageType = typeof(TMessage);
			if (_subscriptions.TryGetValue(messageType, out IList subscriptionList))
			{
				List<ISubscription<TMessage>> subscriptions = subscriptionList as List<ISubscription<TMessage>>;
				if (subscriptions == null)
				{
					// TODO: This is most likely a bug. Perhaps throw an exception.
					return;
				}

				// Iterate backwards to allow unsubscribing during a Send operation
				// without causing a "Collection was modified" exception and without
				// allocating a new list (which would create garbage).
				for (int i = subscriptions.Count - 1; i >= 0; --i)
				{
					subscriptions[i].Action.Invoke(message);
				}
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <typeparam name="TMessage"><inheritdoc/></typeparam>
		/// <param name="action"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="ArgumentNullException">Thrown if the parameter is null.</exception>
		public ISubscription<TMessage> Subscribe<TMessage>(Action<TMessage> action)
			where TMessage : IMessage
		{
			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}

			Type messageType = typeof(TMessage);
			Subscription<TMessage> subscription = new Subscription<TMessage>(action);

			if (_subscriptions.TryGetValue(messageType, out IList subscriptionList))
			{
				// The list containing subscriptions exists. Let's add the new subscription there.
				subscriptionList.Add(subscription);
			}
			else
			{
				// No subscriptions to this message yet. Create a list containing those subscriptions.
				List<ISubscription<TMessage>> subscriptions = new List<ISubscription<TMessage>>();
				subscriptions.Add(subscription);
				_subscriptions.Add(messageType, subscriptions);
			}

			return subscription;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		/// <typeparam name="TMessage"><inheritdoc/></typeparam>
		/// <param name="subscription"><inheritdoc/></param>
		/// <returns><inheritdoc/></returns>
		/// <exception cref="ArgumentNullException">Thrown if the parameter is null.</exception>
		public bool Unsubscribe<TMessage>(ISubscription<TMessage> subscription)
			where TMessage : IMessage
		{
			if (subscription == null)
			{
				throw new ArgumentNullException(nameof(subscription));
			}

			Type messageType = typeof(TMessage);
			if (_subscriptions.TryGetValue(messageType, out IList subscriptionList))
			{
				if (subscriptionList.Contains(subscription))
				{
					subscriptionList.Remove(subscription);
					return true;
				}
			}

			return false;
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			UnsubscribeAll();
		}

		private void UnsubscribeAll()
		{
			foreach (IList subscriptions in _subscriptions.Values)
			{
				subscriptions.Clear();
			}

			_subscriptions.Clear();
		}
	}
}