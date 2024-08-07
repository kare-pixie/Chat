using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//----------
using System; //.net ���̺귯��
using System.Net;
using System.Net.Sockets; //���� ����� �ϱ� ���� ���̺귯��
using System.IO; //�����͸� �а� ���� �ϱ� ���� ���̺귯��
using System.Threading; //��Ƽ �������� ���� ���̺귯��

public class TCP_Manager : MonoBehaviour
{
    public InputField IP_InputField;
    public InputField Port_InputField;

    [SerializeField] private Text Status;

    //��Ŷ -> Stream

    private StreamReader reader;
    private StreamWriter writer;

    public InputField Message_Box;

    private Message_Pooling message;

    private Queue<string> Log = new Queue<string>();
    
    /// <summary>
    /// ����ϱ� ���� �޼���
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
            //TcpListener ��ü ����
            TcpListener tcp = new TcpListener(IPAddress.Parse(IP_InputField.text), int.Parse(Port_InputField.text));
            tcp.Start();
            Log.Enqueue("Start Server..");

            TcpClient client = tcp.AcceptTcpClient();
            //TcpListener�� Client�� ������ �� ������ ��ٷȴٰ� ������ �Ǹ� Client�� �Ҵ�
            Log.Enqueue("Client ���� Ȯ��...");

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
    //������ �����ϴ� ��
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
        //���� ���� �޼����� ���´ٸ�... ���� ���� �޼����� Message Box�� �־�ߵ�
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
