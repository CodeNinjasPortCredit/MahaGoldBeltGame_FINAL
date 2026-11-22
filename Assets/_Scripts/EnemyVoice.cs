using UnityEngine;

// Makes an enemy say something while it is chasing you.
//
// It asks EnemyAI whether it can see the player, so EnemyAI itself does not
// have to change. The sound is 3D, so it gets louder as the enemy closes in
// and you can hear which direction it is coming from.
[RequireComponent(typeof(EnemyAI))]
public class EnemyVoice : MonoBehaviour
{
    [Header("Voice")]
    public AudioClip chaseClip;
    public float volume = 0.9f;

    [Header("Timing")]
    public float repeatEvery = 9f;      // seconds between lines while chasing
    public float startDelay = 0f;       // so a pack does not speak in unison

    [Header("Hearing")]
    public float nearDistance = 3f;     // full volume closer than this
    public float farDistance = 30f;     // silent past this

    private EnemyAI ai;
    private AudioSource source;
    private float timer;

    private void Start()
    {
        ai = GetComponent<EnemyAI>();

        source = GetComponent<AudioSource>();
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
        }

        source.clip = chaseClip;
        source.loop = false;
        source.playOnAwake = false;
        source.volume = volume;
        source.spatialBlend = 1f;                       // 3D, not flat
        source.rolloffMode = AudioRolloffMode.Linear;
        source.minDistance = nearDistance;
        source.maxDistance = farDistance;

        timer = startDelay;
    }

    private void Update()
    {
        if (ai == null || source == null || chaseClip == null)
        {
            return;
        }

        // Not chasing: stay quiet, and be ready to speak the instant it spots you.
        if (!ai.CanSeePlayer())
        {
            timer = 0f;
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f && !source.isPlaying)
        {
            source.Play();
            timer = repeatEvery;
        }
    }
}
