using UnityEngine;

public class CharacterInCar : MonoBehaviour
{
    public Transform carTransform; // assign the car's transform in the inspector
    public Transform characterTransform; // assign the character's transform in the inspector
    public float distanceThreshold = 0.1f; // adjust this value to control the distance between the character and the car

    private Vector3 characterOffset; // store the initial offset between the character and the car

    void Start()
    {
        // calculate the initial offset between the character and the car
        characterOffset = characterTransform.position - carTransform.position;
    }

    void Update()
    {
        // maintain the character's position inside the car
        characterTransform.position = carTransform.position + characterOffset;

        // check if the character is outside the car
        float distance = Vector3.Distance(characterTransform.position, carTransform.position);
        if (distance > distanceThreshold)
        {
            // snap the character back to the car
            characterTransform.position = carTransform.position + characterOffset;
        }
    }
}