
using System.Net;
using UnityEngine;




[System.Serializable]
public class PaddleState
{
    public Vector3 PositionPaddle;
}



public class PaddleSyncServer : MonoBehaviour
{



    GameObject target_Paddle_left;
    GameObject target_Paddle_right;


    
    public string ObjectId;

    //----- variables ecouter ----
        UDPService UDP;

    //----- end -------




    

    
    ServerManager ServerMan;
    float NextUpdateTimeout = -1;


    void Awake()
    {
        if (!Globals.IsServer)
        {
            enabled = false;
        }

        target_Paddle_left = GameObject.Find("PaddleLeft");
        target_Paddle_right = GameObject.Find("PaddleRight");

    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        target_Paddle_left = GameObject.Find("PaddleLeft");
        target_Paddle_right = GameObject.Find("PaddleRight");




        ServerMan = FindFirstObjectByType<ServerManager>();



        //------- ECOUTE DE MESSAGES --------------------------------
         UDP = FindFirstObjectByType<UDPService>();

            UDP.OnMessageReceived += (string message, IPEndPoint sender) => {

                Debug.Log("---EPHEMERE--- MESSAGE RECUE AU SERVEUR VENANT DU CLIENT is >> "+ message);

                if (!message.StartsWith("UPDATEPADDLECLIENT")) { return; }

                    string[] tokens = message.Split('|');

                /*if(ObjectId != "2"  ) {
                    Debug.LogWarning("++++ ERRORRR +++MESSAGE NON RECUE PAR SERVEUR ENVOYER PAR client: " + tokens[1] + "MESSAGE RECUE" + message);
                    return;  // ignore message if not for this client
                }*/

                Debug.Log("CONFIRM--- MESSAGE RECUE AU SERVEUR ENVOYER PAR CLIENT PADDLE (CLIENT) RIGTH OBJET ID -->> "+ObjectId+"-->>> message is ::  "+ message);
  
                string json = tokens[2];

                PaddleState state = JsonUtility.FromJson<PaddleState>(json);
                /*
                 if(tokens[1]=="1"){
                    target_Paddle_left.transform.position = state.PositionPaddle;    
                }*/
                 if(tokens[1]=="2"){
                    target_Paddle_right.transform.position = state.PositionPaddle;
                }
                //transform.position = state.PositionPaddle;

            };

        //------------------------ENDS--------
    }

    // Update is called once per frame
    void Update()
    {

         if (Time.time > NextUpdateTimeout)
        {
            PaddleState state = new PaddleState
            {
                PositionPaddle = transform.position
            };

            string json = JsonUtility.ToJson(state);

            ObjectId ="1";

        

            ServerMan.BroadcastUDPMessage("UPDATEPADDLESERVER|"+ObjectId+"|"+ json);

            Debug.Log("//////////////////////////++++ SERVEUR ++++ //////////////////////////");
            Debug.Log("ENVOIE AU CLIENT DE NEWVALUE PADLE SERVER Numero "+ObjectId+"-->>> is ::  "+ json);
            Debug.Log("//////////////////////////////////////////////////////////////////");

            NextUpdateTimeout = Time.time + 0.03f;
        }
        
    }
}
