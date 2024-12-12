using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class PaddleSyncClient : MonoBehaviour
{

GameObject target_Paddle_left;
GameObject target_Paddle_right;

   //----------- variable pour fonction Sender--------------------------------

    public UDPSender Sender;
    public int DestinationPort = 25000;
    public string DestinationIP = "127.0.0.1";

    
    UdpClient udp;
    IPEndPoint localEP;

    //--




    public string ObjectId ;
    UDPService UDP;


    void Awake()
    {
        if (Globals.IsServer)
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



        UDP = FindFirstObjectByType<UDPService>();

        UDP.OnMessageReceived += (string message, IPEndPoint sender) => {

            if (!message.StartsWith("UPDATEPADDLESERVER")) { return; }

                string[] tokens = message.Split('|');

            //if(ObjectId != tokens[1]) {
            /*
            if(ObjectId != "1") {
                Debug.LogWarning("++++ ERRORRR +++ MESSAGE NON RECUE PAR LE CLIENT VENANT DU SERVEUR : " + tokens[1]+" MESSAGE EST == " +message);
                return;  // ignore message if not for this client
            }*/
            
            Debug.LogWarning("++++ OBJET ID == "+ObjectId+"--- +++"); //
                
                Debug.Log("**** target padd left :: "+target_Paddle_left+"  ******** Target padd right ::  "+target_Paddle_right+" ********\n");

           // if(ObjectId == "1"){
                Debug.Log("CONFIRM--- MESSAGE RECUE AU CLIENT ENVOYER PAR PADDLE LEFT (CLIENT)  OBJET ID -->> "+ObjectId+"-->>> message is ::  "+ message);
                string json = tokens[2];
                PaddleState state = JsonUtility.FromJson<PaddleState>(json);
                //transform.position = state.PositionPaddle;
                if(tokens[1]=="1"){
                    target_Paddle_left.transform.position = state.PositionPaddle;    
                }else if(tokens[1]=="2"){
                    target_Paddle_right.transform.position = state.PositionPaddle;
                }
                
       
           // }

            
              


        };
        
    }
    
    // Update is called once per frame
    void Update()
    {

        SendPositionToServer();
        
    }













    //------ METHODES DU SENDER ------

    public void SendUDPMessage(string message)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(message);
        SendUDPBytes(bytes);
    }

    private void SendUDPBytes(byte[] bytes)
    {
        if (udp == null)
        {
            udp = new UdpClient();
            localEP = new IPEndPoint(IPAddress.Any, 0);
            udp.Client.Bind(localEP);
        }

        try
        {
            IPEndPoint destEP = new IPEndPoint(IPAddress.Parse(DestinationIP), DestinationPort);
            udp.Send(bytes, bytes.Length, destEP);

        }
        catch (SocketException e)
        {
            Debug.LogWarning(e.Message);
        }
    }



    
    void SendPositionToServer()
    {
        PaddleState state = new PaddleState
        {
            //PositionPaddle = transform.position
            PositionPaddle = target_Paddle_right.transform.position
        };

        ObjectId = "2";

        string json = JsonUtility.ToJson(state);
        string message = "UPDATEPADDLECLIENT|" + ObjectId + "|" + json;

        // Envoyer le message au serveur
        SendUDPMessage(message); // Pas besoin de fournir un IPEndPoint


        Debug.Log("/++++ CLIENT ENVOIE MESSAGE PADDLE AUTRE ++++ //////////////////////////");
        Debug.Log("MESSAGE ENVOYER AU SERVEUR PAR LE CLIENT ----MESSAGE = " + message + "*******objetID == " + ObjectId + "-->>> is ::  json ==" + json);
        Debug.Log("+++++++++++++++++++++++++++++++++++++++++++++++++++++++");

    }


    //--------------END --------------------




}
