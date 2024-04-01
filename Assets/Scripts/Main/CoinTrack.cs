using TMPro;
using UnityEngine;

public class CoinTrack : MonoBehaviour
{
	[SerializeField] private TMP_Text coinTrack;

	public void TrackCoinsStats()
	{
		coinTrack.text = Conserver.coinsAmount.ToString();
	}

	private void Start()
	{
		TrackCoinsStats();
	}
}
