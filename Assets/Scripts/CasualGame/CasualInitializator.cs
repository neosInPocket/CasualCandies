using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Finger = UnityEngine.InputSystem.EnhancedTouch.Finger;
using System;
using System.Linq;
using UnityEditor;

public class CasualInitializator : MonoBehaviour
{
	[SerializeField] private CandyRotator candyRotator;
	[SerializeField] private EnemyCandyRotator startEnemyCandyRotator;
	[SerializeField] private CameraShake cameraShake;
	[SerializeField] private List<Image> rounds;
	[SerializeField] private TMP_Text roundCounter;
	[SerializeField] private GeneralStudy generalStudy;
	[SerializeField] private PostFactumScreen postFactumScreen;
	[SerializeField] private Sprite player;
	[SerializeField] private Sprite enemy;
	[SerializeField] private TMP_Text preambuleScreen;
	private List<RoundResult> roundsStatus;
	private int currentRound = 1;
	private int reward;

	private void Start()
	{
		roundsStatus = new List<RoundResult> { RoundResult.None, RoundResult.None, RoundResult.None };
		reward = (int)(20f * Mathf.Sqrt((float)Conserver.playerGameProgress));
		RefreshRoundsStatus();

		if (Conserver.casualTrain == 1)
		{
			Conserver.casualTrain = 0;
			Conserver.Save();
			generalStudy.GeneralStudyCompleted += GeneralStudyCompleted;
			generalStudy.StartStudy();
		}
		else
		{
			GeneralStudyCompleted();
		}
	}

	public void GeneralStudyCompleted()
	{
		if (preambuleScreen != null)
		{
			preambuleScreen.gameObject.SetActive(true);
		}

		preambuleScreen.text = $"LEVEL {Conserver.playerGameProgress}";
		Touch.onFingerDown += BattleStarted;
	}

	public void BattleStarted(Finger finger)
	{
		Touch.onFingerDown -= BattleStarted;
		if (preambuleScreen != null)
		{
			preambuleScreen.gameObject.SetActive(false);
		}

		candyRotator.EnableCandy(true);
		startEnemyCandyRotator.Enabled = true;
		candyRotator.Crushed += CandyCrushed;
		startEnemyCandyRotator.Crushed += EnemyCrushed;
	}

	private void CandyCrushed()
	{
		NextRound(false);
	}

	private void EnemyCrushed()
	{
		NextRound(true);
	}

	public void RestartRound()
	{
		candyRotator.SetDefaults();
		startEnemyCandyRotator.SetDefaults();

		preambuleScreen.gameObject.SetActive(true);
		preambuleScreen.text = $"ROUND {currentRound}";
		roundCounter.text = preambuleScreen.text;
		Touch.onFingerDown += BattleStarted;
	}

	public void NextRound(bool playerWins)
	{
		currentRound++;
		if (currentRound > 3)
		{
			LaunchDelay(EndGame);
		}
		else
		{
			LaunchDelay(RestartRound);
		}

		roundsStatus[currentRound - 2] = playerWins ? RoundResult.Purple : RoundResult.Red;
		RefreshRoundsStatus();
	}

	public void LaunchDelay(Action action)
	{
		DisableAllInstances();
		cameraShake.Shake(1.75f);
		StartCoroutine(Delay(action));
	}

	public void EndGame()
	{
		var winsCount = roundsStatus.Count(x => x == RoundResult.Purple);

		if (winsCount >= 2)
		{
			postFactumScreen.PostFactum(reward, true);
			Conserver.coinsAmount += reward;
			Conserver.playerGameProgress++;
			Conserver.Save();
		}
		else
		{
			postFactumScreen.PostFactum(0, false);
		}
	}

	private IEnumerator Delay(Action action)
	{
		yield return new WaitForSeconds(3.5f);
		action();
	}

	public void RefreshRoundsStatus()
	{
		for (int i = 0; i < 3; i++)
		{
			if (roundsStatus[i] == RoundResult.Red)
			{
				rounds[i].color = Color.white;
				rounds[i].sprite = enemy;
			}

			if (roundsStatus[i] == RoundResult.Purple)
			{
				rounds[i].color = Color.white;
				rounds[i].sprite = player;
			}

			if (roundsStatus[i] == RoundResult.None)
			{
				rounds[i].color = new Color(0, 0, 0, 0);
			}
		}
	}

	public void DisableAllInstances()
	{
		startEnemyCandyRotator.Enabled = false;
		candyRotator.EnableCandy(false);
		candyRotator.Crushed -= CandyCrushed;
		startEnemyCandyRotator.Crushed -= EnemyCrushed;
		if (startEnemyCandyRotator.Rigidbody.velocity != null)
		{
			startEnemyCandyRotator.Rigidbody.velocity = Vector2.zero;
		}

		candyRotator.Rigidbody.velocity = Vector2.zero;
	}

	private void OnDestroy()
	{
		DisableAllInstances();
	}
}

public enum RoundResult
{
	Purple,
	Red,
	None
}
