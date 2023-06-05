using FMODUnity;
using ImGuiNET;
using KSerialization;
using System.Collections;
using UnityEngine;

namespace Beached.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class CustomAudioPlayer : KMonoBehaviour, IImguiDebug
	{
		[Serialize] private string audio;
		[Serialize] private float volume;
		[Serialize] private bool isLooping;

		private float lengthSeconds;
		private FMOD.Channel channel;
		private int channelIdx;
		private float targetVolume;
		private float volumeChangeSpeed;

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (isLooping)
			{
				PlayLoop(audio, volume);
			}
		}

		public void SetTargetVolume(float volume, float volumeChangeSpeed)
		{
			targetVolume = volume;
			this.volumeChangeSpeed = volumeChangeSpeed;
			StartCoroutine(UpdateVolume());
		}

		public void Play(string audio, float volume = 1f)
		{
			Stop();
			this.audio = audio;
			channel = AudioUtil.CreateSound(audio);
			var pos = CameraController.Instance.GetVerticallyScaledPosition(transform.position, false).ToFMODVector();
			var vel = new FMOD.VECTOR();
			channel.set3DAttributes(ref pos, ref vel);
			channel.setVolume(volume);
			channel.setPaused(false);
			channel.getIndex(out channelIdx);
		}

		public void SetPaused(bool paused)
		{
			if (channelIdx < 0)
			{
				return;
			}

			channel.setPaused(paused);
		}

		public void SetVolume(float volume)
		{
			if (channelIdx < 0)
			{
				return;
			}

			channel.setVolume(volume);
		}

		public void PlayLoop(string audio, float volume = 1f)
		{
			Stop();

			this.audio = audio;
			this.volume = volume;
			isLooping = true;
			lengthSeconds = AudioUtil.GetLength(audio) / 1000f;

			StartCoroutine(PlaySoundCoroutine());
		}

		private IEnumerator PlaySoundCoroutine()
		{
			while (isLooping)
			{
				Play(audio, volume);
				yield return new WaitForSeconds(lengthSeconds);
			}
		}

		private IEnumerator UpdateVolume()
		{
			while (!Mathf.Approximately(volume, targetVolume))
			{
				var dt = Time.deltaTime * volumeChangeSpeed;
				volume += (targetVolume - volume) * dt;

				yield return new WaitForEndOfFrame();
			}
		}

		public void StopLooping()
		{
			isLooping = false;
			Stop();
		}

		public void Stop()
		{
			if (channelIdx < 0)
			{
				return;
			}

			AudioUtil.StopSound(channelIdx);
			channelIdx = -1;
		}

		public void OnImguiDraw()
		{
			ImGui.Text("index: " + channelIdx);
			ImGui.Text("Volume: " + volume);
			ImGui.Text("Target volume: " + targetVolume);
			ImGui.Text("length: " + lengthSeconds);
			ImGui.Text("islooping: " + isLooping);
		}
	}
}
