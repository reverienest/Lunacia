using System;
using WakingSightNS;

namespace Pubsub {
	public class WakingSightModeEventArgs : EventArgs {

		public int ActiveMode { get; set; }
		public WSPickupLevel PickupLevel { get; set; }

		public WakingSightModeEventArgs(int activeMode, WSPickupLevel pickupLevel) {
			ActiveMode = activeMode;
			PickupLevel = pickupLevel;
		}

	}
}