using UnityEngine;
using UnityEngine.Playables;

public class CloseUpperFloorDoorScript : MonoBehaviour
{
    [SerializeField]
    PlayableDirector CloseUpperFloorDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloseUpperFloorDoor.Play();
            CloseUpperFloorDoor.stopped += DeactiveCloseUpperFloorDoor;
        }
    }

    private void DeactiveCloseUpperFloorDoor(PlayableDirector pd)
    {
        this.gameObject.SetActive(false);
    }
}
