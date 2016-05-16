using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleAttractor : MonoBehaviour {

    ParticleSystem m_System;
    ParticleSystem.Particle[] m_Particles;
    public float m_Drift = 0.01f;

    public Transform attractor;
    public Color startColor;
    public Color endColor;

    private void LateUpdate()
    {
        InitializeIfNeeded();

        // GetParticles is allocation free because we reuse the m_Particles buffer between updates
        int numParticlesAlive = m_System.GetParticles(m_Particles);

        // Change only the particles that are alive
        for (int i = 0; i < numParticlesAlive; i++)
        {
            Vector3 direction = attractor.position - m_Particles[i].position;
            m_Particles[i].velocity = direction * m_Drift / m_System.duration;
            m_Particles[i].color = Color.Lerp(startColor, endColor, 1 - m_Particles[i].lifetime / m_Particles[i].startLifetime);
        }

        // Apply the particle changes to the particle system
        m_System.SetParticles(m_Particles, numParticlesAlive);
    }

    void InitializeIfNeeded()
    {
        if (m_System == null)
            m_System = GetComponent<ParticleSystem>();

        if (m_Particles == null || m_Particles.Length < m_System.maxParticles)
            m_Particles = new ParticleSystem.Particle[m_System.maxParticles];
    }
}
