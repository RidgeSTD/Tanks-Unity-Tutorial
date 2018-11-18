using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber;
    public float m_Acceleration = 15;
    public float m_TurnSpeed = 3;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;
    public float m_MaxMoveSpeed = 15;

    private string m_MovementAxisName;
    private string m_TurnAxisName;
    private Rigidbody m_Rigidbody;
    private float m_MovementInputValue;
    private float m_TurnInputValue;
    private float m_OriginalPitch;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_MaxMoveSpeed = Mathf.Abs(m_MaxMoveSpeed);
    }


    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }
    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                switchEnginAudio(m_EngineIdling);
            }
        }
        else
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                switchEnginAudio(m_EngineDriving);
            }
        }
    }

    private void switchEnginAudio(AudioClip _clip)
    {
        m_MovementAudio.clip = _clip;
        m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
        m_MovementAudio.Play();
    }


    private void FixedUpdate()
    {
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

        Move();
        Turn();
        EngineAudio();
    }


    private void Move()
    {
        // // move directly to position
        // Vector3 mov = transform.forward * m_MovementInputValue * m_Acceleration * Time.deltaTime;
        // m_Rigidbody.MovePosition(m_Rigidbody.position + mov);

        // // move using phisic model with force
        m_Rigidbody.velocity = m_Rigidbody.velocity + transform.forward * m_MovementInputValue * m_Acceleration * Time.deltaTime;
        m_Rigidbody.velocity = clamp3D(m_Rigidbody.velocity, m_MaxMoveSpeed);
    }

    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.

        Vector3 rotat = new Vector3(0, m_TurnInputValue * m_TurnSpeed, 0);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * Quaternion.Euler(rotat));
    }

    private Vector3 clamp3D(Vector3 vec, float maxAbs)
    {
        maxAbs = Mathf.Abs(maxAbs);
        return new Vector3(
            Mathf.Clamp(vec.x, -maxAbs, maxAbs),
            Mathf.Clamp(vec.y, -maxAbs, maxAbs),
            Mathf.Clamp(vec.z, -maxAbs, maxAbs)
        );
    }
}