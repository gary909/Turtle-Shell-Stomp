using UnityEngine;

// This script will spawn the points prefab when triggered by an event, such as an enemy hit or coin pickup

public class PointsPopup : MonoBehaviour
{
    public GameObject pointsPrefab; // Assign PointsPopup prefab here
    public Vector3 popupOffset = new Vector3(0, 1, 0); // Position offset
    public float destroyTime = 1.5f; // Time to destroy popup

    public void ShowPoints(int points, Vector3 position)
    {
        // Instantiate the points prefab at the position of the hit enemy or coin
        GameObject popup = Instantiate(pointsPrefab, position + popupOffset, Quaternion.identity);
        popup.GetComponent<TextMesh>().text = "+" + points.ToString();

        // Destroy popup after a set time
        Destroy(popup, destroyTime);
    }
}
