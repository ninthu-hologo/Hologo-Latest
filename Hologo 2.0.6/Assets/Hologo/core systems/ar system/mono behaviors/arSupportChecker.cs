using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class arSupportChecker : MonoBehaviour
{
	[SerializeField]
	ARSession m_Session;

	public ARSession session
	{
		get { return m_Session; }
		set { m_Session = value; }
	}

	private bool isSupported;


	public IEnumerator CheckARSupport()
	{

		yield return ARSession.CheckAvailability();

		switch (ARSession.state)
		{
			case ARSessionState.Ready:
				isSupported = true;
					break;
			case ARSessionState.Unsupported:
				//"Your device does not support AR.");
				isSupported = false;
				break;

		}
	}


    public bool getSupportStatus()
	{
		return isSupported;
	}
}
