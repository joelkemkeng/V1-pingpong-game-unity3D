using System.Net;
using UnityEngine;

public class PaddleLeftSyncClient : MonoBehaviour
{

    UDPService UDP;

    void Awake()
    {
        if (Globals.IsServer)
        {
            enabled = false;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UDP = FindFirstObjectByType<UDPService>();

        UDP.OnMessageReceived += (string message, IPEndPoint sender) => {

            if (!message.StartsWith("UPDATEPADLEFT")) { return; }

            Debug.Log("COMMANDE RECU +++++++++++  PADLE LEFT VALUE ");

            string[] tokens = message.Split('|');
            string json = tokens[1];

            Debug.Log("NEWVALUE PADLE LEFT -->>> is ::  "+ tokens[1]);

            PaddleLeftState state = JsonUtility.FromJson<PaddleLeftState>(json);

            
            transform.position = state.PositionPaddleLeft;

        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
