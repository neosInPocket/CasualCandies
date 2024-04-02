using UnityEngine;

public class UIEffects : MonoBehaviour
{
	[SerializeField] private AudioSource EffectsSource;

	private void Start()
	{
		EffectsSource.enabled = Conserver.soundEffects == 1;
	}
}
