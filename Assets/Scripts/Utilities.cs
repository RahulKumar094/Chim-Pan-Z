using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
	public static float GetPositiveAngle(float negativeAngle)
	{
		float val = negativeAngle;
		if (val < 0)
		{
			int factor = Mathf.CeilToInt(Mathf.Abs(negativeAngle / 360));
			val = negativeAngle + factor * 360;
		}
		return val;
	}
}
