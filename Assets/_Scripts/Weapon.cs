using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 200; //Teniendo el private hace que otros scripts no lo manipulen, pero el Serialized permite que este en el Inspector
    [SerializeField]
    private Vector3 rotationPoint = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotationAmount = CalculateRotationAmount(Time.deltaTime); // Seria lo mismo que poner rotationSpeed * Time.deltaTime pero estamos probando Metodos
        transform.RotateAround(rotationPoint, Vector3.forward, rotationAmount);
        return; // El Vector representa ciertos puntos del espacio
                //Planteamos el Movimiento mediante el Transform, y con los Vectores permitimos el movimiento
    }

    private float CalculateRotationAmount(float delta) // Probando nuevos Metodos, usamos los () para explicar el valor que quiero tomar del codigo para usar
    {
        return rotationSpeed * delta;
    }

    public void AddRotationSpeed(float amount)
    {
        rotationSpeed += amount;
    }
}
