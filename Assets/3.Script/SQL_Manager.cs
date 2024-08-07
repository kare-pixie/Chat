using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using MySql;
using MySql.Data.MySqlClient;
using System;
using System.IO;

//Table 데이터 자료형
public class User_info
{
    public string User_Name { get; private set; }
    public string User_Password { get; private set; }
    public string User_PhoneNum { get; private set; }

    public User_info(string name, string password, string phone)
    {
        User_Name = name;
        User_Password = password;
        User_PhoneNum = phone;
    }
}
public class SQL_Manager : MonoBehaviour
{
    public User_info info;
    public MySqlConnection conn; //연결
    public MySqlDataReader reader; //데이터를 직접적으로 읽어옴

    [SerializeField] private string DB_Path = string.Empty;

    public static SQL_Manager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DB_Path = Application.dataPath + "/Database";
        string serverInfo = Server_set(DB_Path);

        try
        {
            if(serverInfo.Equals(string.Empty))
            {
                Debug.Log("Server info null");
                return;
            }
            conn = new MySqlConnection(serverInfo);
            conn.Open();
            Debug.Log("DB Open Connection Complete");
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private string Server_set(string path)
    {
        //파일이 있다면 Json을 풀어서 전달
        if(!File.Exists(path))
        {
            Directory.CreateDirectory(path); //경로 파일
        }
        string JsonString = File.ReadAllText(path + "/config.json");
        JsonData itemData = JsonMapper.ToObject(JsonString);
        string serverInfo =
            $"Server={itemData[0]["IP"]};" +
            $"Database={itemData[0]["TableName"]};" +
            $"Uid={itemData[0]["ID"]};" +
            $"Pwd={itemData[0]["PW"]};" +
            $"Port={itemData[0]["PORT"]};" +
            "Charset=utf8;";

        return serverInfo;
    }
    private bool Connection_Check(MySqlConnection con)
    {
        if(con.State != System.Data.ConnectionState.Open)
        {
            con.Open();
            if (con.State != System.Data.ConnectionState.Open)
                return false;
        }
        return true;
    }
    public bool Login(string id, string pw)
    {
        //직접적으로 DB에서 데이터를 가지고 오는 메서드
        try
        {
            if(!Connection_Check(conn))
            {
                return false;
            }
            string SQL_Command =
                string.Format(@$"SELECT User_Name, User_Password, User_PhoneNum From user_info WHERE User_Name='{id}' AND User_Password='{pw}';");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, conn);
            reader = cmd.ExecuteReader();
            if(reader.HasRows)
            {
                //reader가 읽은 데이터가 1개이상 존재함.
                //읽은 데이터를 하나씩 나열해야한다.
                while(reader.Read())
                {
                    string name = (reader.IsDBNull(0)) ? string.Empty : (string)reader["User_Name"];
                    string pass = (reader.IsDBNull(1)) ? string.Empty : (string)reader["User_Password"];
                    string phone = (reader.IsDBNull(2)) ? string.Empty : (string)reader["User_PhoneNum"];

                    if(!name.Equals(string.Empty) || !pass.Equals(string.Empty))
                    {
                        info = new User_info(name, pass, phone);

                        if (!reader.IsClosed)
                            reader.Close();

                        return true;
                    }
                    else
                    {
                        //로그인 실패
                        break;
                    }
                }
            }
            if (reader.IsClosed)
                reader.Close();

            return false;
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);

            if (reader.IsClosed)
                reader.Close();

            return false;
        }
    }
    public bool SignUp(string id, string pw, string phone)
    {
        //직접적으로 DB에서 데이터를 가지고 오는 메서드
        try
        {
            if (!Connection_Check(conn))
            {
                return false;
            }
            string SQL_Command =
                string.Format($"INSERT INTO User_info VALUES ('{id}', '{pw}', '{phone}');");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, conn);
            reader = cmd.ExecuteReader();

            if (reader.IsClosed)
                reader.Close();

            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);

            if (reader.IsClosed)
                reader.Close();

            return false;
        }
    }
}
