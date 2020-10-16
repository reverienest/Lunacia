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
		public event EventHandler<PlayerDeathEventArguments> PlayerDeathTopic = delegate { };
		public event EventHandler<WakingSightModeEventArgs> WakingSightModeTopic = delegate { };
		public event EventHandler<SceneTransitionEventArgs> SceneTransitionTopic = delegate { };

		public void Raise(ExampleEventArgs exampleEventArgs) { ExampleTopic(this, exampleEventArgs); }
		public void Raise(PlayerDeathEventArguments playerDeathArgs) { PlayerDeathTopic(this, playerDeathArgs); }
		public void Raise(WakingSightModeEventArgs wakingSightModeEventArgs) { WakingSightModeTopic(this, wakingSightModeEventArgs); }
		public void Raise(SceneTransitionEventArgs sceneTransitionEventArgs) { SceneTransitionTopic(this, sceneTransitionEventArgs); }
	}
}