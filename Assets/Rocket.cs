using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField]
    float rcsThrust = 100f;
    [SerializeField]
    float mainThrust = 20f;
    [SerializeField]
    AudioClip mainEngineClip;
    [SerializeField]
    AudioClip explosionClip;
    [SerializeField]
    AudioClip successClip;

    [SerializeField]
    ParticleSystem mainEngineParticles;
    [SerializeField]
    ParticleSystem explosionParticles;
    [SerializeField]
    ParticleSystem successParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;
    
    enum State
    {
        ALIVE,
        DYING,
        TRANSCENDING
    }

    State state = State.ALIVE;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (state == State.ALIVE)
        {
            RespondeToThrustInput();
            RespondeToRotateInput();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.ALIVE)
        {
            return;
        }
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                {
                    break;
                }
            case "Finish":
                {
                    StartSuccessSequence();
                    break;
                }
            default:
                {
                    StartExplodeSequence();
                    break;
                }
        }
    }

    private void StartExplodeSequence()
    {
        rigidBody.freezeRotation = false;
        rigidBody.drag = 0;
        state = State.DYING;
        audioSource.Stop();
        audioSource.PlayOneShot(explosionClip);
        explosionParticles.Play();
        Invoke("LoadFirstScene", 1f);
    }

    private void StartSuccessSequence()
    {
        rigidBody.freezeRotation = false;
        rigidBody.drag = 0;
        state = State.TRANSCENDING;
        audioSource.Stop();
        audioSource.PlayOneShot(successClip);
        successParticles.Play();
        Invoke("LoadNextScene", 1f);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void RespondeToRotateInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            //rigidBody.freezeRotation = true;
            float rotationThisFrame = rcsThrust * Time.deltaTime;
            transform.Rotate(Vector3.forward * rotationThisFrame);
            //rigidBody.freezeRotation = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //rigidBody.freezeRotation = true;
            float rotationThisFrame = rcsThrust * Time.deltaTime;
            transform.Rotate(Vector3.forward * (-rotationThisFrame));
            //rigidBody.freezeRotation = false;
        }
    }

    private void RespondeToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngineClip);
        }
        mainEngineParticles.Play();
    }
}
