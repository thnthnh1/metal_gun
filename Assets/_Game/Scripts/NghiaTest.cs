using System;
using UnityEngine;

public class NghiaTest : MonoBehaviour
{
	private void Start()
	{
		this.Calculate();
	}

	private void Calculate()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		for (int i = 0; i < 100000; i++)
		{
			int num6 = UnityEngine.Random.Range(1, 1001);
			if (num6 <= 500)
			{
				num++;
			}
			else if (num6 <= 700)
			{
				num2++;
			}
			else if (num6 <= 850)
			{
				num3++;
			}
			else if (num6 <= 950)
			{
				num4++;
			}
			else
			{
				num5++;
			}
		}
	}
}
