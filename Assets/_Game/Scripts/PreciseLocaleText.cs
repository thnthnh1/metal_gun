using System;
using UnityEngine;
using UnityEngine.UI;

public class PreciseLocaleText : MonoBehaviour
{
	private void Start()
	{
		base.GetComponent<Text>().text = string.Format("LANGUAGE ID: {0} \nLANGUAGE: {1} \n REGION: {2} \n CURRENCY CODE: {3} \n CURRENCY SYMBOL: {4}", new object[]
		{
			//PreciseLocale.GetLanguageID(),
			//PreciseLocale.GetLanguage(),
			//PreciseLocale.GetRegion(),
			//PreciseLocale.GetCurrencyCode(),
			//PreciseLocale.GetCurrencySymbol()
		});
	}
}
