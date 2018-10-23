using UnityEngine;

namespace codebycandle.ar_hunt
{
	// a custom handler that implements the ITrackableEventHandler interface.
	public class AppTrackableEventHandler:MonoBehaviour, ITrackableEventHandler
	{
		#region VARS (public)
		public GameManager gameManager;
		// public FadeMe fadeMeScript;
		#endregion

		#region VARS (private)
		private TrackableBehaviour mTrackableBehaviour;
		#endregion

		#region METHODS (internal)
		void Start()
		{
			mTrackableBehaviour = GetComponent<TrackableBehaviour>();
			if(mTrackableBehaviour)
			{
				mTrackableBehaviour.RegisterTrackableEventHandler(this);
			}
		}
		#endregion

		#region METHODS (public)
		/*
		 * implementation of the ITrackableEventHandler function called 
		 * when the tracking state changes.
		 */
		public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus,
											TrackableBehaviour.Status newStatus)
		{
			if(newStatus == TrackableBehaviour.Status.DETECTED ||
				newStatus == TrackableBehaviour.Status.TRACKED ||
				newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
			{
				OnTrackingFound();

				// fadeMeScript.InitializeWithFadeIn();
				gameManager.HandleTargetFound(mTrackableBehaviour.TrackableName);
			}
			else
			{
				OnTrackingLost();
			}
		}
		#endregion

		#region METHODS (private)
		private void OnTrackingFound()
		{
			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

			// Enable rendering:
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = true;
			}

			// Enable colliders:
			foreach (Collider component in colliderComponents)
			{
				component.enabled = true;
			}

			Debug.Log("AppTrackable Trackable " + mTrackableBehaviour.TrackableName + " found");
		}

		private void OnTrackingLost()
		{
			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

			// Disable rendering:
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = false;
			}

			// Disable colliders:
			foreach (Collider component in colliderComponents)
			{
				component.enabled = false;
			}

			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
		}
		#endregion
	}
}