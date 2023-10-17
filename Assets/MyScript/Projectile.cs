using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 500.0f;
    float damage = 1;
    float skinWidth = 0.1f;

    public LayerMask EnemyMask;
    public LayerMask ObstacleMask;
    public LayerMask GroundMask;

    // Start is called before the first frame update
    void Start()
    {
        // Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        transform.Translate(Vector3.up * moveDistance);
        CheckCollision(10);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void CheckCollision(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.up);
        RaycastHit hit;
        Physics.Raycast(ray.direction, ray.origin, out hit);
        Debug.DrawRay(ray.origin, ray.direction * 10000, Color.red);
        
        if(Physics.Raycast(ray, out hit, moveDistance + skinWidth, EnemyMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
        if(Physics.Raycast(ray, out hit, moveDistance, ObstacleMask))
        {
            Destroy(this.gameObject);
        }
        if(Physics.Raycast(ray, out hit, moveDistance, GroundMask))
        {
            Destroy(this.gameObject);
        }
    }
    void OnHitObject(RaycastHit hit)
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();

        if(damageableObject != null)
        {
            damageableObject.TakeHit(damage);
            Destroy(this.gameObject);
        }
    }
}
