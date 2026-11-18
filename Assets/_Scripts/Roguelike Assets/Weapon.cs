using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 200; 
    [SerializeField]
    private Vector3 rotationPoint = Vector3.zero;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotationAmount = CalculateRotationAmount(Time.deltaTime); 
        transform.RotateAround(rotationPoint, Vector3.forward, rotationAmount);
        return; 
    }

    private float CalculateRotationAmount(float delta) 
    {
        return rotationSpeed * delta;
    }

    public void AddRotationSpeed(float amount)
    {
        rotationSpeed += amount;
    }
}
