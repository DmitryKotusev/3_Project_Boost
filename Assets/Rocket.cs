using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField]
    float rcsThrust = 100f;
    [SerializeField]
    float mainThrust = 20f;
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

    // Update is called once per frame
    void Update()
    {
        if (state == State.ALIVE)
        {
            Thrust();
            Rotate();
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
                    Debug.Log("Ok");
                    break;
                }
            case "Finish":
                {
                    Debug.Log("Finish");
                    rigidBody.freezeRotation = false;
                    rigidBody.drag = 0;
                    state = State.TRANSCENDING;
                    Invoke("LoadNextScene", 1f);
                    break;
                }
            default:
                {
                    rigidBody.freezeRotation = false;
                    rigidBody.drag = 0;
                    state = State.DYING;
                    if (audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }
                    Invoke("LoadFirstScene", 1f);
                    break;
                }
        }
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void Rotate()
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

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
