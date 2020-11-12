using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakingSightNS {
	public class WSEnablePickups : MonoBehaviour {
		public WSPickupLevel pickupLevel = WSPickupLevel.None;

		void OnTriggerEnter2D(Collider2D other) {
			PresistentData.pickupLevel = pickupLevel;
			Destroy(this.gameObject);
		}
	}
}