using System.Net;
using UnityEngine;

public class PaddleSyncClient : MonoBehaviour
{

    public string ObjectId ;
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

            if (!message.StartsWith("UPDATEPADDLE")) { return; }

                string[] tokens = message.Split('|');

            if(ObjectId != tokens[1]) {
                Debug.LogWarning("++++ ERRORRR +++Received update for object not associated with this client: " + tokens[1]);
                return;  // ignore message if not for this client
            }
            string json = tokens[2];
            PaddleState state = JsonUtility.FromJson<PaddleState>(json);
            transform.position = state.PositionPaddle;

       
              


        };
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
