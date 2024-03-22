using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    private Rigidbody rigidbody;

    [SerializeField] private bool isActive = false;
    [SerializeField] private GameObject owner;

    [SerializeField] private TrailRenderer trail;

    [SerializeField] private float speed = 32f;
    [SerializeField] private float raycastDistance = 6f;

    [SerializeField] private MeshRenderer renderer;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(10f);
        Destroy(this);
    }

    private void Update()
    {
        if (!isActive) return;

        rigidbody.AddForce((transform.forward * this.speed) + new Vector3(0, -9.81f, 0) * Time.fixedDeltaTime, ForceMode.Force);

        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, raycastDistance, ~LayerMask.GetMask("Projectile"))) {
            if (hit.collider == null) return;
            if (hit.collider.gameObject.layer == LayerMask.GetMask("Player")) return;

            if (hit.collider.gameObject.tag == "Entity")
            {
                Entity entity = hit.collider.gameObject.GetComponent<Entity>();

                if (entity != null) 
                {
                    entity.Damage(5f);
                }
            }

            Debug.Log($"HitPlane {hit.collider.gameObject.name}");
            this.isActive = false;

            trail.time = 0f;
            renderer.enabled = false;
            rigidbody.isKinematic = true;
        }
    }

    public void Init(GameObject bOwner, float bSpeed, float bDistance)
    {
        this.owner = bOwner;
        this.speed = bSpeed;
        this.raycastDistance = bDistance;

        this.isActive = true;

        StartCoroutine(Despawn());
    }
}
