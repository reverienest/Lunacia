using System;

namespace Pubsub {
	public class WakingSightModeEventArgs : EventArgs {

		public int ActiveMode { get; set; }

		public WakingSightModeEventArgs(int activeMode) {

			ActiveMode = activeMode;

		}

	}
}