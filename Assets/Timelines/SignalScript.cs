using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;

public class SignalScript : MonoBehaviour
{
    [SerializeField]
    public PlayableDirector Timeline1;
    [SerializeField]
    GameObject ElevatorStartPanel;
    [SerializeField]
    GameObject ElevatorEndPanel;
    [SerializeField]
    Transform Hero;
    [SerializeField]
    Transform ElevatorDoorL_UpperFloor;
    [SerializeField]
    Transform ElevatorDoorR_UpperFloor;
    [SerializeField]
    Transform ElevatorDoorL_DownFloor;
    [SerializeField]
    Transform ElevatorDoorR_DownFloor;
    [SerializeField]
    GameObject dialogueBallon;

    public void PrepareTimeline()
    {
        MasterManager.Singleton.DisablePlayerMovingManager();
    }

    public void EndTimeline()
    {
        ElevatorEndPanel.SetActive(false);
        MasterManager.Singleton.EnablePlayerMovingManager();
    }

    public void ElevatorStartPanelOn()
    {
        ElevatorStartPanel.SetActive(true);
        ElevatorStartPanel.GetComponent<DOTweenAnimation>().DORestart();
    }

    public void ElevatorEndPanellOn()
    {
        ElevatorStartPanel.SetActive(false);
        ElevatorEndPanel.SetActive(true);
        ElevatorEndPanel.GetComponent<DOTweenAnimation>().DORestart();
    }

    public void DowntoUpEvent()
    {
        Hero.position = new Vector3(-8.7f, 2.7f, 5.4f);
        Hero.rotation = Quaternion.Euler(0, 180, 0);

        Timeline1.stopped += RestoreDownFloorDoor;
    }

    private void RestoreDownFloorDoor(PlayableDirector pd)
    {
        ElevatorDoorL_DownFloor.localPosition = new Vector3(0.65f, -1.25f, 0.5f);
        ElevatorDoorR_DownFloor.localPosition = new Vector3(-0.65f, -1.25f, 0.5f);
    }

    public void UptoDownEvent()
    {
        Hero.position = new Vector3(-8.7f, 5.1f, 5.44f);
        Hero.rotation = Quaternion.Euler(0, 0, 0);

        ElevatorDoorL_UpperFloor.position = new Vector3(-0.65f, 1.1f, -0.6f);
        ElevatorDoorR_UpperFloor.position = new Vector3(0.65f, 1.1f, -0.6f);
    }
}
