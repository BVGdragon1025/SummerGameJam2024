using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //This section might get shortened if needed
    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;
    [Range(0, 1)]
    public float sfxVolume = 1;
    [Range(0, 1)]
    public float ambienceVolume = 1;

    private Bus _masterBus;
    private Bus _musicBus;
    private Bus _sfxBus;
    private Bus _ambienceBus;

    private List<EventInstance> _eventInstances;
    private List<StudioEventEmitter> _eventEmitters;

    private EventInstance _ambienceEventInstance;
    private EventInstance _musicEventInstance;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"More than one instance of {Instance.name} detected! GameObject: {name}. Instance has been deleted.");
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }

        SetPublicVariable("Forest_State", 0.0f);
        _eventInstances = new List<EventInstance>();
        _eventEmitters = new List<StudioEventEmitter>();

        /*
        _masterBus = RuntimeManager.GetBus("bus:/");
        _musicBus = RuntimeManager.GetBus("bus:/Music");
        _ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        _sfxBus = RuntimeManager.GetBus("bus:/SFX");
        */
    }

    private void Start()
    {

    }

    private void Update()
    {
        _masterBus.setVolume(masterVolume);
        _musicBus.setVolume(musicVolume);
        _ambienceBus.setVolume(ambienceVolume);
        _sfxBus.setVolume(sfxVolume);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPosition)
    {
        RuntimeManager.PlayOneShot(sound, worldPosition);
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.AddComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        _eventEmitters.Add(emitter);
        return emitter;
    }

    public void SetPublicVariable(string name, float value)
    {
        RuntimeManager.StudioSystem.setParameterByName(name, value);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        _eventInstances.Add(eventInstance);

        return eventInstance;
    }

    public void InitializeEvent(EventReference eventReference)
    {
        _ambienceEventInstance = CreateEventInstance(eventReference);
        _ambienceEventInstance.start();
    }

    public void StopEvent(EventReference eventReference)
    {
        _ambienceEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void SetAmbienceParameter(string parameterName, float parameterValue)
    {
        _ambienceEventInstance.setParameterByName(parameterName, parameterValue);
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        _musicEventInstance = CreateEventInstance(musicEventReference);
        _musicEventInstance.start();
    }

    public void SetMusicArea(MusicArea area)
    {
        _musicEventInstance.setParameterByName("area", (float) area);
    }

    private void CleanUp()
    {
        foreach (EventInstance eventInstance in _eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        foreach(StudioEventEmitter emitter in _eventEmitters)
        {
            emitter.Stop();
        }

    }

    private void OnDestroy()
    {
        CleanUp();
    }

}
