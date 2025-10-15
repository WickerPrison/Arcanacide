using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColorChange : MonoBehaviour
{
    public bool colorChange = false;
    [SerializeField] ColorData colorData;
    [SerializeField] LevelColor newLevelColor;
    [SerializeField] float alpha;
    [SerializeField] List<ParticleSystem> particles;
    Color newColor;

    // Start is called before the first frame update
    void Start()
    {
        if (!colorChange) return;

        newColor = colorData.GetColor(newLevelColor);
        foreach (ParticleSystem particle in particles)
        {
            ParticleSystem.MainModule main = particle.main;
            main.startColor = new Color(newColor.r, newColor.g, newColor.b, alpha);
            if (main.playOnAwake)
            {
                particle.Clear();
                particle.Play();
            }
        }
    }
}
