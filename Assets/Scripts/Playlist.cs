using UnityEngine;

namespace Jake.Audio
{
	public class Playlist : MonoBehaviour
	{
		public AudioSource source;
		public float fade;
		public bool repeat;
		public AudioClip[] clips;
		
		private int currentClipIndex;
		private float sourceVolume;
		
		void Start()
		{
			sourceVolume = source.volume;

			if (source.playOnAwake)
			{
				Play();
			}
		}
		
		public void Play()
		{
			if (currentClipIndex == clips.Length)
			{
				if (repeat)
				{
					currentClipIndex = 0;
				}
				else
				{
					return;
				}
			}

			// play clip
			var clip = clips[currentClipIndex++];
			source.PlayOneShot(clip);
				
			// ready next clip
			Runner.RunOnce(Play, clip.length);

			// fade
			if (fade > Mathf.Epsilon)
			{
				// fade in
				Lerper.LerpOverTime((v) =>
				{
					source.volume = v;
				},
				0, sourceVolume, fade);

				// ready fade out
				Runner.RunOnce(() =>
				{
				// save source volume
				sourceVolume = source.volume;

				// fade out
				Lerper.LerpOverTime((v) =>
					{
						source.volume = v;
					},
					sourceVolume, 0, fade);
				},
				clip.length - fade);
			}
		}
	}
}
