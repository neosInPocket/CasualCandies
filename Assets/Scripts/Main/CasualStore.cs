using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CasualStore : MonoBehaviour
{
	[SerializeField] private List<Component> points;
	[SerializeField] private Button leftArrow;
	[SerializeField] private Button rightArrow;
	[SerializeField] private TMP_Text describer;
	[SerializeField] private TMP_Text upgradeCaption;
	[SerializeField] private Image upgradeImage;
	[SerializeField] private GameObject nocoins;
	[SerializeField] private Button mainButton;
	[SerializeField] private TMP_Text mainButtonText;
	[SerializeField] private TMP_Text cost;
	[SerializeField] private CoinTrack coinTrack1;
	[SerializeField] private CoinTrack coinTrack2;
	[SerializeField] private StoreUpgradeInfo storeUpgradeInfo1;
	[SerializeField] private StoreUpgradeInfo storeUpgradeInfo2;
	bool currentUpgrade;

	private void Start()
	{
		SetUpgradeVisible(storeUpgradeInfo1, true);
	}

	public void SetUpgradeVisible(StoreUpgradeInfo info, bool upgrade1)
	{
		currentUpgrade = upgrade1;
		points.ForEach(x => x.gameObject.SetActive(false));
		var pointsAmount = upgrade1 ? Conserver.storeUpgrade : Conserver.storeSideUpgrade;
		for (int i = 0; i < pointsAmount; i++)
		{
			points[i].gameObject.SetActive(true);
		}

		cost.text = info.coinsAmount.ToString();
		describer.text = info.describer;
		upgradeCaption.text = info.upgradeName;
		upgradeImage.sprite = info.upgradeImage;

		if (pointsAmount < 5)
		{
			if (Conserver.coinsAmount >= info.coinsAmount)
			{
				nocoins.gameObject.SetActive(false);
				mainButton.gameObject.SetActive(true);
				mainButtonText.text = "PURCHASE";
			}
			else
			{
				nocoins.gameObject.SetActive(true);
				mainButton.gameObject.SetActive(false);
			}
		}
		else
		{
			nocoins.gameObject.SetActive(false);
			mainButton.gameObject.SetActive(true);
			mainButtonText.text = "PURCHASED";
			mainButton.interactable = false;
		}

		SetStoreArrows(upgrade1);
	}

	public void ChooseOneUpgrade(bool first)
	{
		if (first)
		{
			SetUpgradeVisible(storeUpgradeInfo1, true);
		}
		else
		{
			SetUpgradeVisible(storeUpgradeInfo2, false);
		}

	}

	public void SetStoreArrows(bool first)
	{
		if (first)
		{
			leftArrow.interactable = false;
			rightArrow.interactable = true;
		}
		else
		{
			leftArrow.interactable = true;
			rightArrow.interactable = false;
		}
	}

	public void PurchaseOneUpgrade()
	{
		bool first = currentUpgrade;

		if (first)
		{
			Conserver.coinsAmount -= storeUpgradeInfo1.coinsAmount;
			Conserver.storeUpgrade++;
			Conserver.Save();
			SetUpgradeVisible(storeUpgradeInfo1, true);
		}
		else
		{
			Conserver.coinsAmount -= storeUpgradeInfo2.coinsAmount;
			Conserver.storeSideUpgrade++;
			Conserver.Save();
			SetUpgradeVisible(storeUpgradeInfo2, false);
		}

		coinTrack1.TrackCoinsStats();
		coinTrack2.TrackCoinsStats();
	}
}
