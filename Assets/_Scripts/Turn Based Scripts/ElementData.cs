using UnityEngine;

[CreateAssetMenu(fileName = "NewElement", menuName = "Game/Element")]
public class ElementData : ScriptableObject
{
    public string elementName;
    public ElementType type;
    public int normalDamage = 2;
    public int weaknessDamage = 15;
    public bool isWerewolfAttack;
    public AudioClip hitSound;
    public AudioClip criticalSound;
    public AudioClip attackSound;

    public int GetDamage(bool hitWeakness)
    {
        if (hitWeakness)
        {
            return weaknessDamage;
        }

        return normalDamage;
    }

    public void PlayHit(AudioSource source, bool hitWeakness)
    {
        if (source == null)
        {
            return;
        }

        AudioClip clip = hitSound;

        if (hitWeakness)
        {
            clip = criticalSound;
        }

        if (clip != null)
        {
            source.PlayOneShot(clip);
        }
    }

    public void PlayAttack(AudioSource source)
    {
        if (source == null || attackSound == null)
        {
            return;
        }

        source.PlayOneShot(attackSound);
    }
}
