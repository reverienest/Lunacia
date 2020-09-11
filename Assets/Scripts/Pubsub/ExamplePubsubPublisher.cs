using UnityEngine;

namespace Pubsub {

	public class ExamplePubsubPublisher : MonoBehaviour {

		public string Message;

		void Start() {
			MessageBroker.Instance.Raise(new ExampleEventArgs(Message));
		}
	}
}