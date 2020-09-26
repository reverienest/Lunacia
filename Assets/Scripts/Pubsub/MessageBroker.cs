using System;

namespace Pubsub {
	public class MessageBroker {

		private static readonly MessageBroker instance = new MessageBroker();

		// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
		static MessageBroker() {
		}

		private MessageBroker() {
		}

		public static MessageBroker Instance {
			get {
				return instance;
			}
		}

		public event EventHandler<ExampleEventArgs> ExampleTopic = delegate { };

		public void Raise(ExampleEventArgs exampleEventArgs) { ExampleTopic(this, exampleEventArgs); }

		public event EventHandler<PlayerDeathEventArguments> playerDeath = delegate { };

		public void Raise(PlayerDeathEventArguments playerDeathArgs) { playerDeath(this, playerDeathArgs); }
	}
}