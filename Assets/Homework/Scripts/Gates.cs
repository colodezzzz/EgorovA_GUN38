using UnityEngine;

public class Gates : MonoBehaviour
{
    private int _score = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            _score++;
            Destroy(other.gameObject);

            Debug.Log($"Game score: {_score}");
        }
    }
}
