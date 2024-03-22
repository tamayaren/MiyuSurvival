using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Entity : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer renderer;    
    public UnityEvent OnDied = new UnityEvent();
    public UnityEvent OnDamage = new UnityEvent();

    private float lastHealth = 10f;
    
    //
    [SerializeField] private float _health = 10f;
    public float Health
    {
        get { return _health; } 
        private set {
            this._health = Mathf.Clamp(value, 0, this.MaxHealth);
            if ((this.lastHealth - this._health) < 0)
                this.DamageVanity();

            if (this._health <= 0f)
                this.OnDied.Invoke();


            this.lastHealth = value;
        }
    }

    [SerializeField] private float _maxHealth = 10f;
    public float MaxHealth 
    {
        get { return _maxHealth; }
        private set
        {
            this._maxHealth = value;
            this._health = Mathf.Clamp(value, 0, _maxHealth);
        }
    }

    private IEnumerator Unhighlight()
    {
        foreach (Material material in renderer.materials)
        {
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", Color.red);
        }
        yield return new WaitForSeconds(.1f);
        foreach (Material material in renderer.materials)
            material.DisableKeyword("_EMISSION");
    }

    private void DamageVanity()
    {
        StartCoroutine(Unhighlight());
    }

    public void SetHealth(float health)
    {
        this.Health = health;
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.MaxHealth = maxHealth;
    }

    public void Damage(float damage)
    {
        this.OnDamage.Invoke();
        //
        this.Health -= damage;
        this.DamageVanity();
    }
}
