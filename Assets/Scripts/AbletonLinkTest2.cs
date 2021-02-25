using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbletonLinkTest2 : MonoBehaviour
{
	long lastbeatnum;
	long beatnum;

	Vector3 size2;
	Vector3 size1;

	private AbletonLink link;

	// Use this for initialization
	void Start()
	{
		size1 = new Vector3(1, 1, 1);
		size2 = new Vector3(2, 2, 2);

		lastbeatnum = 0;
		beatnum = 0;
	}

	// Update is called once per frame
	void Update()
	{
		lastbeatnum = beatnum;
		double beat, phase, tempo, time;
		int numPeers;
		AbletonLink.Instance.update(out beat, out phase, out tempo, out time, out numPeers);
		beatnum = (long)beat;

		// We can obtain the latest beat and phase like this.
		Debug.Log("beat: " + beatnum + " phase:" + phase + " numpeers:" + numPeers + " tempo:" + tempo);

		if ((beatnum - lastbeatnum) == 1)
		{
			if (gameObject.transform.localScale == size1)
			{
				gameObject.transform.localScale = size2;
			}
			else if (gameObject.transform.localScale == size2)
			{
				gameObject.transform.localScale = size1;
			}
		}
	}
}
