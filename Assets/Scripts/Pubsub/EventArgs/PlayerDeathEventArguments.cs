using System;

namespace Pubsub {
	public class PlayerDeathEventArguments : EventArgs {

		public string deathMessage { get; set; }

		public PlayerDeathEventArguments(string message) {

			deathMessage = message;

		}

	}
}