using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//----------
using System; //.net 라이브러리
using System.Net;
using System.Net.Sockets; //소켓 통신을 하기 위한 라이브러리
using System.IO; //데이터를 읽고 쓰고 하기 위한 라이브러리
using System.Threading; //멀티 스레딩을 위한 라이브러리

public class TCP_Manager : MonoBehaviour
{
    public InputField IP_InputField;
    public InputField Port_InputField;

    [SerializeField] private Text Status;

    //패킷 -> Stream

    private StreamReader reader;
    private StreamWriter writer;

    public InputField Message_Box;

    private Message_Pooling message;

    private Queue<string> Log = new Queue<string>();
    
    /// <summary>
    /// 출력하기 위한 메서드
    /// </summary>
    private void Status_Message()
    {
        if (Log.Count > 0)
        {
            Status.text = Log.Dequeue();
        }
    }
    private void Update()
    {
        Status_Message();
    }
    #region Server
    public void Server_Oepn()
    {
        message = FindObjectOfType<Message_Pooling>();
        Thread thread = new Thread(Server_Connect);
        thread.IsBackground = true;
        thread.Start();
    }
    private void Server_Connect()
    {
        try
        {
            //TcpListener 객체 생성
            TcpListener tcp = new TcpListener(IPAddress.Parse(IP_InputField.text), int.Parse(Port_InputField.text));
            tcp.Start();
            Log.Enqueue("Start Server..");

            TcpClient client = tcp.AcceptTcpClient();
            //TcpListener가 Client가 연결이 될 때까지 기다렸다가 연결이 되면 Client에 할당
            Log.Enqueue("Client 연결 확인...");

            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream());
            writer.AutoFlush = true;

            while(client.Connected)
            {
                string readData = reader.ReadLine();
                message.Message(readData);
            }
        }
        catch(Exception e)
        {
            Log.Enqueue(e.Message);
        }
    }
    #endregion

    #region Client
    public void Client_Connecting()
    {
        message = FindObjectOfType<Message_Pooling>();
        Log.Enqueue("Client_Connecting...");
        Thread thread = new Thread(Client_Connect);
        thread.IsBackground = true;
        thread.Start();
    }
    //서버에 접근하는 쪽
    private void Client_Connect()
    {
        try
        {
            TcpClient client = new TcpClient();
            //IPStartPoint = Server
            //IPEndPoint = Client
            IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse(IP_InputField.text), int.Parse(Port_InputField.text));
            client.Connect(iPEnd);
            Log.Enqueue("Client Sever Connect Complate!");

            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream());
            writer.AutoFlush = true;

            while (client.Connected)
            {
                string readData = reader.ReadLine();
                message.Message(readData);
            }
        }
        catch(Exception e)
        {
            Log.Enqueue(e.Message);
        }
    }
    #endregion
    public void Sending_btn()
    {
        //만약 내가 메세지를 보냈다면... 내가 보낸 메세지도 Message Box에 넣어야됨
        if(Sending_Message(Message_Box.text))
        {
            message.Message(Message_Box.text);
            Message_Box.text = string.Empty;
        }
    }
    private bool Sending_Message(string me)
    {
        if(writer != null)
        {
            writer.WriteLine(me);
            return true;
        }
        Log.Enqueue("Writer null...");
        return false;
    }
}
