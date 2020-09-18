using System;

namespace Pubsub {
	public class PlayerDeathEvent : EventArgs {

		public string deathMessage { get; set; }

		public PlayerDeathEvent(string message) {

			deathMessage = message;

		}

	}
}