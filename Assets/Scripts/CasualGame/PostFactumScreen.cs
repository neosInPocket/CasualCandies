using TMPro;
using Unity.Properties;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PostFactumScreen : MonoBehaviour
{
	[SerializeField] private TMP_Text casualBattleText;
	[SerializeField] private TMP_Text rewardAmountText;
	[SerializeField] private Image winnerImage;
	[SerializeField] private Sprite redSprite;
	[SerializeField] private Sprite purpleSprite;

	public void PostFactum(int rewardedCount, bool playerWins)
	{
		if (playerWins)
		{
			winnerImage.sprite = purpleSprite;
		}
		else
		{
			winnerImage.sprite = redSprite;
		}

		if (rewardedCount != 0)
		{
			casualBattleText.text = "PURPLE WINS!";
			rewardAmountText.text = rewardedCount.ToString();
		}
		else
		{
			casualBattleText.text = "RED WINS!";
			rewardAmountText.text = 0.ToString();
		}

		gameObject.SetActive(true);
	}

	public void MainHead()
	{
		SceneManager.LoadScene("CasualMenu");
	}

	public void GameHead()
	{
		SceneManager.LoadScene("CasualGame");
	}
}
