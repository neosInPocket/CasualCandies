using System;
using TMPro;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Finger = UnityEngine.InputSystem.EnhancedTouch.Finger;

public class GeneralStudy : MonoBehaviour
{
	[SerializeField] private TMP_Text study;
	[SerializeField] private Animator hand;
	public Action GeneralStudyCompleted;

	public void StartStudy()
	{
		gameObject.SetActive(true);
		Touch.onFingerDown += YourCandy;
		study.text = "welcome to Sweet Candies Rainbow! i'll be your guide for now";
	}

	private void YourCandy(Finger finger)
	{
		Touch.onFingerDown -= YourCandy;
		Touch.onFingerDown += EnemyCandy;
		study.text = "meet your candy warrior! it is very easy to control: tap the screen and it will fly in the direction the green arrow on it is pointing in";
		hand.SetTrigger("next");
	}

	private void EnemyCandy(Finger finger)
	{
		Touch.onFingerDown -= EnemyCandy;
		Touch.onFingerDown += LevelStarts;
		study.text = "same thing with your opponent: you can predict his next action by his green arrow";
		hand.SetTrigger("next");
	}

	private void LevelStarts(Finger finger)
	{
		Touch.onFingerDown -= LevelStarts;
		Touch.onFingerDown += Rounds;
		study.text = "once the level starts, your task will be to knock your opponent off the screen";
		hand.SetTrigger("next");
	}

	private void Rounds(Finger finger)
	{
		Touch.onFingerDown -= Rounds;
		Touch.onFingerDown += CompleteLevels;
		study.text = "Each level consists of 3 rounds: win at least two to continue the game!";
		hand.SetTrigger("next");
	}

	private void CompleteLevels(Finger finger)
	{
		Touch.onFingerDown -= CompleteLevels;
		Touch.onFingerDown += StudyEnd;
		study.text = "complete levels, get rewards and take care of your head! Good luck!";
		hand.SetTrigger("next");
	}

	private void StudyEnd(Finger finger)
	{
		Touch.onFingerDown -= StudyEnd;
		GeneralStudyCompleted?.Invoke();
		gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		Touch.onFingerDown -= YourCandy;
		Touch.onFingerDown -= EnemyCandy;
		Touch.onFingerDown -= LevelStarts;
		Touch.onFingerDown -= Rounds;
		Touch.onFingerDown -= CompleteLevels;
		Touch.onFingerDown -= StudyEnd;
	}
}
