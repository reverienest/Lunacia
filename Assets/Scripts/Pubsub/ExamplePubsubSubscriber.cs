using UnityEngine;

namespace Pubsub {

	public class ExamplePubsubSubscriber : MonoBehaviour {
		void Awake() {
			MessageBroker.Instance.ExampleTopic += consumeExampleMessage;
		}

		private void consumeExampleMessage(object sender, ExampleEventArgs example) {
			print(example.ExampleMessage);
		}
	}
}