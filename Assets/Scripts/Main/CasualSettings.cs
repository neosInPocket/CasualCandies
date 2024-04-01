using UnityEngine;
using UnityEngine.UI;

public class CasualSettings : MonoBehaviour
{
	[SerializeField] private Image music;
	[SerializeField] private Image sounds;
	[SerializeField] private Sprite offSprite;
	[SerializeField] private Sprite onSprite;
	bool musicEnabled;
	bool soundsEnabled;
	AudioCasualListener audioCasualListener;

	private void Start()
	{
		audioCasualListener = GameObject.FindAnyObjectByType<AudioCasualListener>();

		RefreshSettingsControls(Conserver.musicEffects == 1, Conserver.soundEffects == 1);
	}

	public void RefreshSettingsControls(bool musisValue, bool soundsValue)
	{
		musicEnabled = musisValue;
		soundsEnabled = soundsValue;

		music.sprite = musicEnabled ? onSprite : offSprite;
		sounds.sprite = soundsEnabled ? onSprite : offSprite;


		int result = musisValue ? 1 : 0;
		audioCasualListener.SetListener(result);

		Conserver.soundEffects = soundsValue ? 1 : 0;
		Conserver.Save();
	}

	public void ManipulateWithMusic()
	{
		RefreshSettingsControls(!musicEnabled, soundsEnabled);
	}

	public void ManipulateWithSounds()
	{
		RefreshSettingsControls(musicEnabled, !soundsEnabled);
	}
}
