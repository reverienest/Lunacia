using System;

namespace Pubsub {
	public class SceneTransitionEventArgs : EventArgs {

		public string sceneTransitionMessage { get; set; }

		public SceneTransitionEventArgs(string message) {

			sceneTransitionMessage = message;

		}

	}
}
