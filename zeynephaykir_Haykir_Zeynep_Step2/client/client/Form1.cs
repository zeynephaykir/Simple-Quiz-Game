
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;



namespace client
{
    

    public partial class Form1 : Form
    {
        bool terminating = false;
        bool connected = false;
        Socket clientSocket;
        string name = "";
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
            AnswerText.Enabled = false;
            SendButton.Enabled = false;
        }
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = IPText.Text;

            int portNum;
            if (Int32.TryParse(PortText.Text, out portNum))
            {
                try
                {
                    //connecting to the server
                    name = NameText.Text;
                    clientSocket.Connect(IP, portNum);
                    ConnectButton.Enabled = false;
                    DisconnectButton.Enabled = true;
                    AnswerText.Enabled = true;
                    SendButton.Enabled = true;
                    connected = true;
                    Logs.AppendText("Connection established...\n");
                    if (name != "" && name.Length <= 512)
                    {
                        Byte[] buffer = Encoding.Default.GetBytes(name);
                        clientSocket.Send(buffer);
                    }

                    //call recieve function to process the questions and send answers
                    Thread receiveThread = new Thread(Receive);
                    receiveThread.Start();
                }
                catch
                {
                    Logs.AppendText("Problem occurred while connecting...\n");
                }
            }
            else
            {
                Logs.AppendText("Check the port\n");
            }
        }

        private void Receive()
        {
            {
                while (connected)
                {
                    try
                    {
                        Byte[] buffer = new Byte[512];
                        clientSocket.Receive(buffer);

                        //Parse the incomming messages
                        string incomingMessage = Encoding.Default.GetString(buffer);
                        incomingMessage = incomingMessage.Trim('\0');
                        string terminatingMessage = incomingMessage.Substring(0, 7);

                        //If the terminatingMessage is "Endgame" terminate the connection to the server

                       if (String.Equals(terminatingMessage, "GOODBYE"))
                        {
                            //connected = false;

                            //clientSocket.Close();
                            //DisconnectButton.Enabled = false;
                            //ConnectButton.Enabled = true;
                            //AnswerText.Enabled = false;
                            //SendButton.Enabled = false;

                            //Logs.AppendText("Disconnected!\n");


                            string answer = "BYE";
                            if (answer != "" && answer.Length <= 512)
                            {
                                Byte[] buffer1 = Encoding.Default.GetBytes(answer);
                                clientSocket.Send(buffer1);
                            }
                            //return;
                        }
                        else { 
                            //get rid of empty spaces and print the message to logs
                            
                            Logs.AppendText("Server: " + incomingMessage + "\n");
                            SendButton.Enabled = true;
                        }
                    }
                    catch
                    {
                        if (!terminating)
                        {
                            Logs.AppendText("The server has disconnected\n");
                            ConnectButton.Enabled = true;
                            DisconnectButton.Enabled = false;

                        }

                        clientSocket.Close();
                        connected = false;
                    }
                }
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            //get rid of empty spaces and send the answer to the server
            string answer = AnswerText.Text;
            answer.Trim('\0');
            Logs.AppendText("Answer: " + answer + "\n");
            if (answer != "" && answer.Length <= 512)
            {
                Byte[] buffer = Encoding.Default.GetBytes(answer);
                clientSocket.Send(buffer);
            }
            AnswerText.Text = "";
            SendButton.Enabled = false;
        }

        private void Disconnect_Click(object sender, EventArgs e)
        {
            //disconnect from the server and print a message accordingly
            //connected = false;
            //terminating = true;

            //string answer = "BYE";
            //if (answer != "" && answer.Length <= 512)
            //{
            //    Byte[] buffer = Encoding.Default.GetBytes(answer);
            //    clientSocket.Send(buffer);
            //}
            //Receive();

            connected = false;

            clientSocket.Close();
            DisconnectButton.Enabled = false;
            ConnectButton.Enabled = true;
            AnswerText.Enabled = false;
            SendButton.Enabled = false;

            Logs.AppendText("Disconnected!\n");
        }
        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            clientSocket.Close();
            Environment.Exit(0);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void AnswerText_TextChanged(object sender, EventArgs e)
        {

        }

        private void Logs_TextChanged(object sender, EventArgs e)
        {

        }

        private void PortText_TextChanged(object sender, EventArgs e)
        {

        }
    }
}