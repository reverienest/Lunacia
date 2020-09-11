using System;

namespace Pubsub {
	public class ExampleEventArgs : EventArgs {

		public string ExampleMessage { get; set; }

		public ExampleEventArgs(string exampleMessage) {

			ExampleMessage = exampleMessage;

		}

	}
}