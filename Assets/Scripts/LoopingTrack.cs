using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingTrack : MonoBehaviour
{
	public List<GameObject> Tracks;
	public GameObject Container;
	public float CreateTrackAfterNormalizedDistance = 0.1f;
	public bool RandomizeSpawn = false;

	private Track mCurrentTrack;
	private List<Track> mCreatedTracks;

	void Start()
    {
		CreateTracks();
		mCurrentTrack = mCreatedTracks[0];
		mCurrentTrack.transform.position = Vector3.zero;
	}

    // Update is called once per frame
    void Update()
    {
		if (mCurrentTrack != null && PlayerController.Instance.transform.position.z > mCurrentTrack.transform.position.z + mCurrentTrack.Length * CreateTrackAfterNormalizedDistance)
		{
			ChangeTrack();
		}
    }

	void CreateTracks()
	{
		int tracksToCreate = 0;
		mCreatedTracks = new List<Track>();

		if (Tracks.Count == 1)
			tracksToCreate = 2;
		else if (Tracks.Count > 1)
			tracksToCreate = Tracks.Count;
		else
			Debug.LogError("No Tracks to be created");

		for (int i = 0; i < tracksToCreate; i++)
		{
			int j = 0;
			if (tracksToCreate != 2)
				j = i;

			GameObject newTrack = Instantiate(Tracks[j]);
			newTrack.transform.SetParent(Container.transform);
			Track track = newTrack.GetComponent<Track>();
			track.SetIndex(i);
			mCreatedTracks.Add(track);
		}
	}

	void ChangeTrack()
	{
		int nextTrackIndex = mCurrentTrack.Index;

		if (RandomizeSpawn)
		{
			while (nextTrackIndex == mCurrentTrack.Index)
			{
				nextTrackIndex = Random.Range(0, mCreatedTracks.Count);
			}
		}
		else
		{
			nextTrackIndex = mCurrentTrack.Index + 1;
			if (nextTrackIndex >= mCreatedTracks.Count)
				nextTrackIndex = 0;
		}

		mCreatedTracks[nextTrackIndex].transform.position = new Vector3(0, 0, mCurrentTrack.transform.position.z + mCurrentTrack.Length);
		mCurrentTrack = mCreatedTracks[nextTrackIndex];
	}

}
