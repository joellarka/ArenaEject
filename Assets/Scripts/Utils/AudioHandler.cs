using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioHandler : MonoBehaviour
{
    private static AudioHandler instance;
    public static AudioHandler Instance { get => GetInstance(); private set => instance = value; }
    public static bool deleteOtherSources = true;

    [Header("Music")]
    [SerializeField] AudioClip music;
    AudioSource musicPlayer;

    [Header("Effects")]
    [SerializeField] int initialEffectSourceCount = 10;
    List<AudioSource> effectSources = new();

    private void Awake()
    {
        #region Singleton
        if (instance == null || instance == this)
            instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }
        #endregion

        if (deleteOtherSources)
            DestroyAllOtherSources();

        SetUpMusicPlayer();
        ExpandSorceCount(initialEffectSourceCount);


        if (gameObject != null)
            SceneManager.sceneLoaded += delegate { PickMusic(); };
    
        DontDestroyOnLoad(gameObject);
    }

    public static void PlaySoundEffect(AudioClip clipToPlay)
    {
        if (clipToPlay == null) return;
        if (!Application.isPlaying) return;

        // Loop through all effect sources in Instance until a free one plays
        // If no free source is available, make more sources

        // Potential optimization would be to just play from one of the new sources instantly, rather than looping through again.
        // The reason this isn't done, is because Unity has previously created/destroyed objects at end of frame while continuing the stack

        bool soundStartedPlaying = false;
        while (!soundStartedPlaying)
        {
            foreach (AudioSource s in Instance.effectSources)
            {
                if (s.isPlaying)
                    continue;

                s.volume = Settings.EFFECT_VOULME;

                s.PlayOneShot(clipToPlay);
                soundStartedPlaying = true;
                break;
            }

            if (!soundStartedPlaying)
                Instance.ExpandSorceCount(10);
        }

    }

    public static void PlayRandomEffectFromList(List<AudioClip> possibleClips)
    {
        if (possibleClips == null) return;
        if (possibleClips.Count == 0) return;

        int rand = Random.Range(0, possibleClips.Count);
        AudioClip clipToPlay = possibleClips[rand];

        PlaySoundEffect(clipToPlay);
    }

    private void SetUpMusicPlayer()
    {
        PickMusic();

        if (musicPlayer == null)
            musicPlayer = this.gameObject.AddComponent<AudioSource>();

        musicPlayer.loop = true;
        musicPlayer.volume = Settings.MUSIC_VOULME;

        if (!musicPlayer.isPlaying)
        {
            musicPlayer.clip = music;
            musicPlayer.Play();
        }
    }

    private void PickMusic()
    {
        AudioClip prevClip = null;
        if(musicPlayer!= null) { prevClip = musicPlayer.clip; }

        if (SceneManager.GetActiveScene().name == Paths.START_SCENE_NAME)
            music = Resources.Load<AudioClip>(Paths.START_MENU_MUSIC);
        else
            music = Resources.Load<AudioClip>(Paths.DEFAULT_GAMEPLAY_MUSIC);

        if(music != prevClip)
        {
            if (musicPlayer == null) return;

            musicPlayer.Stop();
            musicPlayer.clip = music;
            musicPlayer.loop = true;
            musicPlayer.Play();
        }
    }

    private void ExpandSorceCount(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newSourceObject = new GameObject("Effect Source");
            newSourceObject.transform.parent = transform;
            AudioSource newSource = newSourceObject.AddComponent<AudioSource>();

            newSource.volume = Settings.EFFECT_VOULME;

            effectSources.Add(newSource);
        }
    }

    private void DestroyAllOtherSources()
    {
        AudioSource[] oldSources = Object.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource o in oldSources)
        {
            AudioHandler thisCheck = o.GetComponent<AudioHandler>();
            if (thisCheck != null)
                if (thisCheck == this)
                    return;

            Destroy(o.transform.gameObject);
        }
    }

    public void UpdateMusicVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0f, 1f);

        musicPlayer.volume = volume;
    }

    public void UpdateEffectVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0f, 1f);
        foreach (AudioSource s in effectSources)
        {
            s.volume = volume;
        }
    }

    public void TuneOutMusic()
    {
        StartCoroutine(TurnOffMusic());
    }

    private IEnumerator TurnOffMusic()
    {
        float startVol = musicPlayer.volume;
        float timePassed = 0;
        float timeToQuiet = 1f;

        while (timeToQuiet > timePassed)
        {
            musicPlayer.volume = Mathf.Lerp(startVol, 0, (timePassed / timeToQuiet));
            timePassed += Time.deltaTime;

            yield return null;
        }
        musicPlayer.volume = 0;
        yield return null;
    }


    private static AudioHandler GetInstance()
    {
        if (instance != null)
            return instance;

        if (!Application.isPlaying)
            return null;

        GameObject newManager = new GameObject("AudioManager");
        instance = newManager.AddComponent<AudioHandler>();
        return instance;
    }
}
