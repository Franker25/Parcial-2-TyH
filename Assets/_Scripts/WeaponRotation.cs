using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private float speed = 200;
    [SerializeField]
    private GameObject secondAxe;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2d.MoveRotation(rb2d.rotation + speed * Time.fixedDeltaTime);
    }

    public void AddRotationSpeed(float amount)
    {
        speed += amount;
    }

    public void ActivateSecondAxe()
    {
        secondAxe.SetActive(true);
    }

    public void GrowWeapon(float amount)
    {
        transform.localScale += new Vector3(amount, amount, 0f);
    }
}
