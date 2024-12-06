using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableInitializer : MonoBehaviour
{
	[SerializeField] private List<DataHolder> _dataHolders;
	public static bool supersonicWisdomReady;

	private void Awake()
	{
		foreach (DataHolder data in _dataHolders)
			data.Init();
		// SupersonicWisdom.Api.AddOnReadyListener(OnSupersonicWisdomReady);
		// SupersonicWisdom.Api.Initialize();

	}

	void OnSupersonicWisdomReady()
	{
		supersonicWisdomReady = true;
	}
}
