using UnityEngine;


[System.Serializable]
public class PaddleLeftState
{
    public Vector3 PositionPaddleLeft;
}


public class PaddleLeftSyncServer : MonoBehaviour
{

    ServerManager ServerMan;
    float NextUpdateTimeout = -1;


    void Awake()
    {
        if (!Globals.IsServer)
        {
            enabled = false;
        }

    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        ServerMan = FindFirstObjectByType<ServerManager>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time > NextUpdateTimeout)
        {
            PaddleLeftState state = new PaddleLeftState
            {
                PositionPaddleLeft = transform.position
            };

            string json = JsonUtility.ToJson(state);

            ServerMan.BroadcastUDPMessage("UPDATEPADLEFT|" + json);
            NextUpdateTimeout = Time.time + 0.03f;
        }

    }
}
