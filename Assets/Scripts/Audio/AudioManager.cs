using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambienceSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource footstepSource;

    [Header("Music")]
    public AudioClip platformerMusic;
    public AudioClip rpgMusic;

    [Header("Loops")]
    public AudioClip hubPortalHum;
    public AudioClip footstepLoop;

    [Header("Player SFX")]
    public AudioClip jump;
    public AudioClip attack;
    public AudioClip takeDamage;

    [Header("Enemy SFX")]
    public AudioClip enemyDeath;
    public AudioClip enemyTakeDamage;
    public AudioClip tomatoSpit;

    [Header("Item / World SFX")]
    public AudioClip itemPickup;
    public AudioClip enterPortal;

    [Header("Cooking SFX")]
    public AudioClip foodCreation;
    public AudioClip dropFoodIntoPot;
    public AudioClip giveFoodToCustomer;
    public AudioClip customerYuck;

    [Header("Volumes")]
    [Range(0f, 1f)] public float musicVolume = 0.6f;
    [Range(0f, 1f)] public float ambienceVolume = 0.45f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float footstepVolume = 0.35f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetupSource(musicSource, true, musicVolume);
        SetupSource(ambienceSource, true, ambienceVolume);
        SetupSource(sfxSource, false, sfxVolume);
        SetupSource(footstepSource, true, footstepVolume);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        ApplySceneAudio(SceneManager.GetActiveScene().name);
    }

    private void SetupSource(AudioSource source, bool shouldLoop, float volume)
    {
        if (source == null) return;

        source.playOnAwake = false;
        source.loop = shouldLoop;
        source.volume = volume;
        source.spatialBlend = 0f;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySceneAudio(scene.name);
    }

    private void ApplySceneAudio(string sceneName)
    {
        StopFootsteps();

        if (sceneName == "RPG Phase")
        {
            PlayMusic(rpgMusic);
            StopAmbience();
            return;
        }

        if (sceneName == "Hub Room")
        {
            StopMusic();
            PlayAmbience(hubPortalHum);
            return;
        }

        if (sceneName == "Platformer Phase" ||
            sceneName == "Corn gunner" ||
            sceneName == "DoubleLayered" ||
            sceneName == "Lettuce 1" ||
            sceneName == "Tomato 1")
        {
            PlayMusic(platformerMusic);
            StopAmbience();
            return;
        }

        // if (start scene)
        // {
        //     PlayMusic(rpgMusic);
        //     StopAmbience();
        // }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource == null) return;
        musicSource.Stop();
        musicSource.clip = null;
    }

    public void PlayAmbience(AudioClip clip)
    {
        if (clip == null || ambienceSource == null) return;
        if (ambienceSource.clip == clip && ambienceSource.isPlaying) return;

        ambienceSource.clip = clip;
        ambienceSource.volume = ambienceVolume;
        ambienceSource.loop = true;
        ambienceSource.Play();
    }

    public void StopAmbience()
    {
        if (ambienceSource == null) return;

        ambienceSource.Stop();
        ambienceSource.clip = null;
    }

    //an audio mulitplier might be nice for intensifying game? wont hurt to add

    public void PlaySFX(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.PlayOneShot(clip, sfxVolume * volumeMultiplier);
    }

    public void StartFootsteps()
    {
        if (footstepLoop == null || footstepSource == null) return;
        if (footstepSource.isPlaying) return;

        footstepSource.clip = footstepLoop;
        footstepSource.volume = footstepVolume;
        footstepSource.loop = true;
        footstepSource.Play();
    }

    public void StopFootsteps()
    {
        if (footstepSource == null) return;

        footstepSource.Stop();
        footstepSource.clip = null;
    }

    public void PlayJump() => PlaySFX(jump);
    public void PlayAttack() => PlaySFX(attack);
    public void PlayTakeDamage() => PlaySFX(takeDamage);
    public void PlayEnemyDeath() => PlaySFX(enemyDeath);
    public void PlayEnemyTakeDamage() => PlaySFX(enemyTakeDamage);
    public void PlayTomatoSpit() => PlaySFX(tomatoSpit);
    public void PlayItemPickup() => PlaySFX(itemPickup);
    public void PlayEnterPortal() => PlaySFX(enterPortal);
    public void PlayFoodCreation() => PlaySFX(foodCreation);
    public void PlayDropFoodIntoPot() => PlaySFX(dropFoodIntoPot);
    public void PlayGiveFoodToCustomer() => PlaySFX(giveFoodToCustomer);
    public void PlayCustomerYuck() => PlaySFX(customerYuck);
}
