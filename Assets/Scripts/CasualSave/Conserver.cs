using UnityEngine;

public class Conserver : MonoBehaviour
{
	[SerializeField] private bool eraseLastData;
	public static int playerGamePgoress;
	public static int coinsAmount;
	public static int storeUpgrade;
	public static int storeSideUpgrade;
	public static int soundEffects;
	public static int musicEffects;
	public static int casualTrain;

	private void Awake()
	{
		if (eraseLastData)
		{
			Default();
			Save();
		}
		else
		{
			Load();
		}
	}
	public void Default()
	{
		playerGamePgoress = 1;
		coinsAmount = 200909000;
		storeUpgrade = 0;
		storeSideUpgrade = 0;
		soundEffects = 1;
		musicEffects = 1;
		casualTrain = 1;
	}

	public static void Save()
	{
		PlayerPrefs.SetInt("playerGamePgoress", playerGamePgoress);
		PlayerPrefs.SetInt("coinsAmount", coinsAmount);
		PlayerPrefs.SetInt("storeUpgrade", storeUpgrade);
		PlayerPrefs.SetInt("storeSideUpgrade", storeSideUpgrade);
		PlayerPrefs.SetInt("soundEffects", soundEffects);
		PlayerPrefs.SetInt("musicEffects", musicEffects);
		PlayerPrefs.SetInt("casualTrain", casualTrain);
	}

	public void Load()
	{
		playerGamePgoress = PlayerPrefs.GetInt("playerGamePgoress", 1);
		coinsAmount = PlayerPrefs.GetInt("coinsAmount", 0);
		storeUpgrade = PlayerPrefs.GetInt("storeUpgrade", 0);
		storeSideUpgrade = PlayerPrefs.GetInt("storeSideUpgrade", 0);
		soundEffects = PlayerPrefs.GetInt("soundEffects", 1);
		musicEffects = PlayerPrefs.GetInt("musicEffects", 1);
		casualTrain = PlayerPrefs.GetInt("casualTrain", 1);
	}
}
