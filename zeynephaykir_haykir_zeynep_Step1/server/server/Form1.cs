using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text;

namespace server
{
    public partial class Form1 : Form
    {
        Socket server_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> client_Socket = new List<Socket>();
        List<string> nicknames = new List<string>(); //nickname list of the players
        bool terminating = false;
        public bool listening = false;
        int numOfClient = 2;  //maximum number of client is determined here.
        public bool connected = false;
        public string nameOfClient = "";
        private double sum = 0;  // total sum earning from questions
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        int questionNum;
        bool leftGame = false;
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(server_FormClosing);
            InitializeComponent();
        }

        private void server_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;

            Environment.Exit(0);
        }

        private void path_button_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.ShowDialog();
            path_textbox.Text = openFileDialog1.FileName;

        }

        private void listen_button_Click(object sender, EventArgs e)
        {
            int port_Num;

            if (Int32.TryParse(port_textBox.Text, out port_Num))
            {

                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port_Num);
                server_Socket.Bind(endPoint);
                server_Socket.Listen(numOfClient); // socket listens as the max number of client
                listening = true;
                listen_button.Enabled = false;

                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();

            }
        }
        private void Accept()
        {
            int playerCounter = 0;
            while (listening)
            {
                try
                {

                    Socket newClient = server_Socket.Accept();
                    Byte[] buffer = new Byte[64];
                    newClient.Receive(buffer);
                    nameOfClient = Encoding.Default.GetString(buffer);
                    nameOfClient = nameOfClient.Substring(0, nameOfClient.IndexOf("\0"));

                    if (nicknames.Contains(nameOfClient))  //checking whether there is such a name in the nickname list.
                    {
                        richTextBox1.AppendText(nameOfClient + " is already in the game");
                        newClient.Close();

                    }
                    else
                    {
                        if (playerCounter < 2)
                        {
                            client_Socket.Add(newClient);
                            nicknames.Add(nameOfClient);

                            richTextBox1.AppendText(nameOfClient + " is connected to the server\n");
                            Thread receiveThread = new Thread(() => Receive1(newClient, nameOfClient));
                            receiveThread.Start();
                            playerCounter++;
                            if (playerCounter == 2)
                            {
                                Game();
                            }



                        }
                     

                    }


                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;

                    }
                    else
                    {
                        richTextBox1.AppendText("Socket stopped working \n");
                        listen_button.Enabled = true;
                        listening = false;
                    }
                }
            }
        }

        private int Receive(Socket thisClient) // updated
        {
            bool connected = true;

            while (connected && !terminating)
            {
                try
                {
                    Byte[] buffer = new Byte[256];
                    thisClient.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                    richTextBox1.AppendText("Client: " + incomingMessage + "\n");
                    
                    int result;
                    if(String.Equals("BYE", incomingMessage))
                    {
                        richTextBox1.AppendText("oLDU");
                       
                        return -30;
                    }
    
                    bool success = int.TryParse(incomingMessage, out result);
                    if (success)
                    {
                        return result;
                    }
                    else
                    {
                        richTextBox1.AppendText("Error receiving message.\n");
                     
                    }
           
                   
                }
                catch
                {
                    if (!terminating)
                    {
                        richTextBox1.AppendText("A client has disconnected\n");
                    }
                    thisClient.Close();
                    client_Socket.Remove(thisClient);
                    connected = false;
                   
                    return -1;

                }


            }
            return 0;
        }

        public void Game()
        {
            double pointP1 = 0;
            double pointP2 = 0;
            int numberQuestions = questionNum;
            string[] questions;
        
            OpenFileDialog file = openFileDialog1;
     
            file.Filter = "(*.gc)|*.gc|(*.etf)|*.etf|(*.txt)|*.txt|(*.GC)|*.GC|(*.tap)|*.tap";
        


            int ansP1 = -1;
            int ansP2 = -1;
            questions = File.ReadAllLines(file.FileName);
            int lineNum = questions.Length / 2;

            int qcounter = 1;
        Restart:
            foreach (var line in questions)
            {
                if (numberQuestions > 0)
                {
                    if (qcounter == 1)
                    {
                        
                     // this means we are now sending a question
                        string message = line;
                        if (message != "" && message.Length <= 256)
                        {
                             richTextBox1.AppendText("Question: "+ message+"\n");
                            Byte[] buffer = Encoding.Default.GetBytes(message);
                            foreach (Socket client in client_Socket)
                            {
                                try
                                {
                                    client.Send(buffer);
                                   
                                }
                                catch
                                {
                                    richTextBox1.AppendText("There is a problem! Check the connection...\n");
                                    terminating = true;
                                    richTextBox1.Enabled = false;
                                }
                            }
                        }
  
                        ansP1 = Receive(client_Socket[0]);
                        if (ansP1 == -30)
                        {
                           
                            pointP1 = 0;
                            string message1 = "Endgame: Player1 has left the game!\n" + nicknames[1] + " is the winner!\nPlayer1: " + pointP1.ToString() + " Player2: " + pointP2.ToString();
                            Byte[] buffer = Encoding.Default.GetBytes(message1);

                            try
                            {
                                client_Socket[1].Send(buffer);
                            }
                            catch
                            {
                                richTextBox1.AppendText("There is a problem! Check the connection...\n");
                                terminating = true;
                                richTextBox1.Enabled = false;
                            }
                            leftGame = false;

                            nicknames = new List<string>();
                            client_Socket = new List<Socket>();
                            server_Socket.Close();
                            server_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                            return;

                        }
                        ansP2 = Receive(client_Socket[1]);

                      
                      if (ansP2 == -30)
                        {
                          
                            pointP2 = 0;
                            string message1 = "Endgame: Player1 has left the game!\n" + nicknames[0] + " is the winner!\nPlayer1: " + pointP1.ToString() + " Player2: " + pointP2.ToString();
                            Byte[] buffer = Encoding.Default.GetBytes(message1);

                            try
                            {
                                client_Socket[0].Send(buffer);
                            }
                            catch
                            {
                                richTextBox1.AppendText("There is a problem! Check the connection...\n");
                                terminating = true;
                                richTextBox1.Enabled = false;
                            }
                            leftGame = false;
                           

                            nicknames = new List<string>();
                            client_Socket = new List<Socket>();
                            server_Socket.Close();
                            server_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                            return;
                        }


                        qcounter++;
                    }
                    else if (qcounter == 2)
                    {
                        numberQuestions--;
                        lineNum--;

                        richTextBox1.AppendText("Answer: " + line + "\n");
                        int realAnswer = Convert.ToInt32(line);

                        if (Math.Abs(realAnswer - ansP1) < Math.Abs(realAnswer - ansP2))
                        {
                            pointP1++;
                            richTextBox1.AppendText("P1 Wins\nPlayer1: " + pointP1.ToString() + " Player2: " + pointP2.ToString() + "\n");
                            string message = nicknames[0] + " wins this round!\nPlayer1: " + pointP1.ToString() + " Player2: " + pointP2.ToString();
                            if (message != "" && message.Length <= 256)
                            {
                               
                                Byte[] buffer = Encoding.Default.GetBytes(message);
                                foreach (Socket client in client_Socket)
                                {
                                    try
                                    {
                                        client.Send(buffer);
                                       
                                    }
                                    catch
                                    {
                                        richTextBox1.AppendText("There is a problem! Check the connection...\n");
                                        terminating = true;
                                        richTextBox1.Enabled = false;
                                    }

                                }


                            }

                        }
                        else if (Math.Abs(realAnswer - ansP1) > Math.Abs(realAnswer - ansP2))
                        {
                            pointP2++;
                            richTextBox1.AppendText("P2 Wins\nPlayer1: " + pointP1.ToString() + " Player2: " + pointP2.ToString() + "\n");
                            string message = nicknames[1] + " wins this round!\nPlayer1: " + pointP1.ToString() + " Player2: " + pointP2.ToString() + "\nCorrect Answer: " + realAnswer.ToString();
                            if (message != "" && message.Length <= 256)
                            {
                                Byte[] buffer = Encoding.Default.GetBytes(message);
                                foreach (Socket client in client_Socket)
                                {
                                    try
                                    {
                                        client.Send(buffer);
                                    }
                                    catch
                                    {
                                        richTextBox1.AppendText("There is a problem! Check the connection...\n");
                                        terminating = true;
                                        richTextBox1.Enabled = false;
                                    }



                                }
                            }

                        }
                        else
                        {
                            pointP1 += 0.5;
                            pointP2 += 0.5;
                            richTextBox1.AppendText("Draw\nPlayer1: " + pointP1.ToString() + " Player2: " + pointP2.ToString() + "\n");
                            string message = "Draw!\nPlayer1: " + pointP1.ToString() + " Player2: " + pointP2.ToString() + "\nCorrect Answer: " + realAnswer.ToString();
                            if (message != "" && message.Length <= 256)
                            {
                                Byte[] buffer = Encoding.Default.GetBytes(message);
                                foreach (Socket client in client_Socket)
                                {
                                    try
                                    {
                                        client.Send(buffer);
                                    }
                                    catch
                                    {
                                        richTextBox1.AppendText("There is a problem! Check the connection...\n");
                                        terminating = true;
                                        richTextBox1.Enabled = false;
                                    }



                                }
                            }
                        }
                        qcounter--;
            
                        if (lineNum == 0)
                        {
                            goto Restart;
                        }

                    }
                  

                }

               

                    }

            listen_button.Enabled = true;
            listening = false;

            if (pointP1 > pointP2)
            {
                richTextBox1.AppendText("Endgame: " + nicknames[0] + " is the winner!\nPlayer1: " + pointP1.ToString() + " Player2: " + pointP2.ToString() + "\n");
                string message = "Endgame: " + nicknames[0] + " is the winner!\nPlayer1: " + pointP1.ToString() + " Player2: " + pointP2.ToString();
                if (message != "" && message.Length <= 256)
                {
                    Byte[] buffer = Encoding.Default.GetBytes(message);
                    foreach (Socket client in client_Socket)
                    {
                        try
                        {
                            client.Send(buffer);
                        }
                        catch
                        {
                            richTextBox1.AppendText("There is a problem! Check the connection...\n");
                            terminating = true;
                            richTextBox1.Enabled = false;
                        }
                    }
                }
            }
            else if (pointP1 < pointP2)
            {
                richTextBox1.AppendText("Endgame: " + nicknames[0] + " is the winner!\nPlayer1: " + pointP1.ToString() + " Player2: " + pointP2.ToString()+"\n");
                string message = "Endgame: "+nicknames[1] + " is the winner!\nPlayer1: " + pointP1.ToString() + " Player2: " + pointP2.ToString();
                if (message != "" && message.Length <= 256)
                {
                    Byte[] buffer = Encoding.Default.GetBytes(message);
                    foreach (Socket client in client_Socket)
                    {
                        try
                        {
                            client.Send(buffer);
                        }
                        catch
                        {
                            richTextBox1.AppendText("There is a problem! Check the connection...\n");
                            terminating = true;
                            richTextBox1.Enabled = false;
                        }
                    }
                }
            }
            else 
            {
                richTextBox1.AppendText("Endgame: Draw!\nPlayer1: " + pointP1.ToString() + " Player2: " + pointP2.ToString()+ "\n");
                string message = "Endgame: Draw!\nPlayer1: " + pointP1.ToString() + " Player2: " + pointP2.ToString();
                if (message != "" && message.Length <= 256)
                {
                    Byte[] buffer = Encoding.Default.GetBytes(message);
                    foreach (Socket client in client_Socket)
                    {
                        try
                        {
                            client.Send(buffer);
                        }
                        catch
                        {
                            richTextBox1.AppendText("There is a problem! Check the connection...\n");
                            terminating = true;
                            richTextBox1.Enabled = false;
                        }
                    }
                }
            }

            nicknames = new List<string>();
            client_Socket = new List<Socket>();
            server_Socket.Close();
            server_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);







        }



        private void Receive1(Socket thisClient, string nameOf)
        {
            connected = true;
            while (connected && !terminating)
            {
                try
                {

                }
                catch
                {
                    if (!terminating)
                    {
                        richTextBox1.AppendText("Client: " + nameOf + " has left the game");
                    }
                    thisClient.Close();
                    client_Socket.Remove(thisClient);
                    nicknames.Remove(nameOf);
                    connected = false;
                    sum = 0;
                }
            }
        }

        private void path_label_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void quiz_question_textbox_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(quiz_question_textbox.Text, out questionNum); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void path_textbox_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
