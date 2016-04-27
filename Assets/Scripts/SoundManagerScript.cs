using UnityEngine;
using System.Collections;

public class SoundManagerScript : MonoBehaviour {

	public AudioSource audioPlayer;
	public AudioClip sourceClip;
	public int samplesPerBar;
	public int barsToSelect;
	public int barsToSkip;

	void Start(){
				
		//TODO: Add bar offset?

		int samplesPerSegment = (barsToSelect + barsToSkip) * samplesPerBar;
		int segmentsInSource = (int)sourceClip.samples / samplesPerSegment;
		int samplesInSelection = barsToSelect * samplesPerBar;
		int samplesInOutput = samplesInSelection * segmentsInSource;

		Debug.Log (segmentsInSource);

		//Create samples array with correct length to hold clip samples
		float[] outputSamples = new float[samplesInOutput * sourceClip.channels];

		Debug.Log (outputSamples.Length);

		//Load the current clip data into that array
		for (int i = 0; i < segmentsInSource; i++) {

			float[] selectionSamples = new float[samplesInSelection * sourceClip.channels];
			//Load from the source, offsetting by samples per segment
			sourceClip.GetData (selectionSamples, i * samplesPerSegment);
			Debug.Log("Pass " + (i + 1));
			Debug.Log ("Copying " + selectionSamples.Length + " samples to output. " + (outputSamples.Length - ((i + 1) * selectionSamples.Length)) + " spaces remains in output.");
			selectionSamples.CopyTo (outputSamples, i * samplesInSelection);
		}

		audioPlayer.clip = AudioClip.Create ("finalAudio", outputSamples.Length/sourceClip.channels, sourceClip.channels, sourceClip.frequency, false);
		audioPlayer.clip.SetData (outputSamples, 0);
		audioPlayer.loop = true;
		audioPlayer.Play ();
		Debug.Log (audioPlayer.clip.length);
		Debug.Log (audioPlayer.clip.samples);

		SavWav.Save(sourceClip.name + " - " + samplesPerBar + " - " + + barsToSelect + " " + barsToSkip, audioPlayer.clip);

	}
}
