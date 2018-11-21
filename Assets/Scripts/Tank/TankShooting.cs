using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public Rigidbody m_Shell;
    public Transform m_FireTransform;
    public Slider m_AimSlider;
    public AudioSource m_ShootingAudio;
    public AudioClip m_ChargingClip;
    public AudioClip m_FireClip;
    public float m_MinLaunchForce = 15f;
    public float m_MaxLaunchForce = 30f;
    public float m_MaxChargeTime = 0.75f;

    private string m_FireButton;
    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;
    private bool m_Fired;


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
        m_AimSlider.minValue = m_MinLaunchForce;
        m_AimSlider.maxValue = m_MaxLaunchForce;
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }

    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        if (Input.GetButtonDown(m_FireButton))
        {
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
            m_Fired = false;
        }
        if (!m_Fired)
        {
            if (Input.GetButton(m_FireButton))
            {
                if (m_CurrentLaunchForce < m_MaxLaunchForce)
                {
                    m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
                    if (m_CurrentLaunchForce > m_MaxLaunchForce)
                    {
                        m_CurrentLaunchForce = m_MaxLaunchForce;
                    }
                }
            }
            else if (Input.GetButtonUp(m_FireButton))
            {
                Fire();
            }
        }
        m_AimSlider.value = m_CurrentLaunchForce;
    }


    private void Fire()
    {
        // Instantiate and launch the shell.
        m_Fired = true;
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation);
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;
        // m_ShootingAudio.Stop(); // 在clip切换时自动停止播放
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}