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
        public class player
        {
            public double answer { get; set; }
            public double point { get; set; }
            public string name { get; set; }
            public bool dc { get; set; }
            public Socket socket { get; set; }
            public double abs { get; set; }
            public bool correct { get; set; }
            public bool winner { get; set; }

        };

        Socket server_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> client_Socket = new List<Socket>();
        List<string> nicknames = new List<string>(); //nickname list of the players
        bool terminating = false;
        public bool listening = false;
        int numOfClient = 99999;  //maximum number of client is determined here.
        Thread gameThread;
        public string nameOfClient = "";

        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        int questionNum;

        List<player> players = new List<player>();
        bool beforegameListen = false;
        bool deact = true;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(server_FormClosing);
            InitializeComponent();
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void server_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;

            Environment.Exit(0);
        }

        //Path Button
        private void path_button_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.ShowDialog();
            path_textbox.Text = openFileDialog1.FileName;

        }

        //Activate Button
        private void listen_button_Click(object sender, EventArgs e)
        {
            deact = false;
            beforegameListen = true;
            button2.Enabled = true;
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
                            richTextBox1.AppendText(nameOfClient + " is already in the game\n");
                            newClient.Close();

                        }
                        else
                        {
                            if (!button1.Enabled && (players.Count < 2) && beforegameListen )
                            {
                                client_Socket.Add(newClient);
                                nicknames.Add(nameOfClient);
                                richTextBox1.AppendText(nameOfClient + " is connected to the server\n");
                                Thread receiveThread = new Thread(() => Receive1(newClient, nameOfClient));
                                receiveThread.Start();

                                player newplayer = new player();
                                newplayer.point = 0;
                                newplayer.name = nameOfClient;

                                newplayer.socket = newClient;
                                newplayer.correct = false;
                                players.Add(newplayer);

                            }

                            if (button1.Enabled && beforegameListen)
                        {
                                client_Socket.Add(newClient);
                                nicknames.Add(nameOfClient);

                                richTextBox1.AppendText(nameOfClient + " is connected to the server\n");
                                Thread receiveThread = new Thread(() => Receive1(newClient, nameOfClient));
                                receiveThread.Start();

                                player newplayer = new player();
                                newplayer.point = 0;
                                newplayer.name = nameOfClient;

                                newplayer.socket = newClient;
                                newplayer.correct = false;
                                players.Add(newplayer);
                            }


                            if (players.Count == 2 && beforegameListen)
                        {
                              
                                button1.Enabled = true;
                            }
                            if (!beforegameListen)
                        {
                            newClient.Close();

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

        private int Receive2(Socket thisClient, int index) // updated
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
                    if (!String.Equals("BYE", incomingMessage))
                    {
                        richTextBox1.AppendText("Client " + players[index].name + ": " + incomingMessage + "\n");
                    }
                 

                    int result;
                    //if (String.Equals("BYE", incomingMessage))
                    //{
                    //    players[index].dc = true;
                    //    return -30;
                    //}

                    bool success = int.TryParse(incomingMessage, out result);
                    if (success)
                    {
                        players[index].answer = result;
                        return result;
                    }
                    else
                    {
                        if (!String.Equals("BYE", incomingMessage))
                        {
                            richTextBox1.AppendText("Error receiving message.\n");
                        }
                        

                    }


                }
                catch
                {
                    if (!deact)
                    {
                        richTextBox1.AppendText("Client: " + players[index].name + " has disconnected.\n");
                        thisClient.Close();
                        client_Socket.Remove(thisClient);
                        nicknames.Remove(players[index].name);
                        players[index].dc = true;
                        connected = false;


                    }


                    return -1;

                }


            }
            return 0;
        }


        // The game, played by the clients
        public void Game()
        {
            // Get the question database file
            int numberQuestions = questionNum;
            string[] questions;

            OpenFileDialog file = openFileDialog1;

            file.Filter = "(*.gc)|*.gc|(*.etf)|*.etf|(*.txt)|*.txt|(*.GC)|*.GC|(*.tap)|*.tap";


            questions = File.ReadAllLines(file.FileName);

            int lineNum = questions.Length / 2;

            int qcounter = 1;

            // Broadcast the questions one by one to each client connected before start game
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
                            richTextBox1.AppendText("Question: " + message + "\n");
                            Byte[] buffer4 = Encoding.Default.GetBytes(message);


                            for (int i = 0; i < players.Count; i++)
                            {
                                try
                                {
                                    players[i].socket.Send(buffer4);
                                }
                                catch
                                {
                                    richTextBox1.AppendText("There is a problem! Check the connection...\n");
                                    listen_button.Enabled = true;
                                    listening = false;
                                }
                            }



                            //foreach (Socket client in client_Socket)
                            //{
                            //    try
                            //    {
                            //        client.Send(buffer);

                            //    }
                            //    catch
                            //    {
                            //        richTextBox1.AppendText("There is a problem! Check the connection...\n");
                            //        listen_button.Enabled = true;
                            //        listening = false;
                            //    }
                            //}
                        }
                        List<Thread> threads = new List<Thread>();
                        //   richTextBox1.AppendText(players.Count.ToString());
                        //multithread receive for each of the players
                        for (int i = 0; i < players.Count; i++)
                        {
                            int curr = i;
                            Thread receiveThread = new Thread(() => Receive2(players[curr].socket, curr));
                            receiveThread.Start();
                            threads.Add(receiveThread);
                        }
                        foreach (Thread thread in threads)
                        {
                            thread.Join();
                        }


                        //disconnection check loop
                        for (int i = 0; i < players.Count; i++)
                        {
                            if (players[i].dc)
                            {
                                //string message2 = "GOODBYE";
                                //Byte[] buffer2 = Encoding.Default.GetBytes(message2);

                                try
                                {
                                    //players[i].socket.Send(buffer2);
                                 
                                    players.RemoveAt(i);
                                    // richTextBox1.AppendText(i.ToString());
                                    threads[i].Abort();

                                }
                                catch
                                {
                                    richTextBox1.AppendText("There is a problem! Check the connection...\n");
                                    listen_button.Enabled = true;
                                    listening = false;
                                }
                                i--;

                            }


                        }

                        // If there is a single player left in the game, s/he is the winner
                        if (players.Count == 1)
                        {
                            string message1 = "Endgame: All other players have left the game!\n" + players[0].name + " is the winner!\nPlayer " + players[0].name + " : " + players[0].point.ToString();
                            Byte[] buffer3 = Encoding.Default.GetBytes(message1);
                            richTextBox1.AppendText(message1 + "\n");
                            try
                            {
                                players[0].socket.Send(buffer3);

                                //nicknames = new List<string>();
                                //client_Socket = new List<Socket>();
                                //server_Socket.Close();
                                //server_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                button1.Enabled = false;

                                beforegameListen = true;
                                listening = true;
                                listen_button.Enabled = false;

                                Thread acceptThread = new Thread(Accept);
                                acceptThread.Start();

                                Thread receiveThread = new Thread(() => Receive1(players[0].socket, players[0].name));
                                receiveThread.Start();


                                return;
                            }

                            catch
                            {
                                richTextBox1.AppendText("There is a problem! Check the connection...\n");
                                listen_button.Enabled = true;
                                listening = false;
                            }
                        }
                        // If all players are left the game, server goes back to the initial state
                        else if(players.Count == 0)
                        {
                            button1.Enabled = false;

                            beforegameListen = true;
                            listening = true;
                            listen_button.Enabled = false;

                            Thread acceptThread = new Thread(Accept);
                            acceptThread.Start();

                            return;
                        }




                        qcounter++;
                    }

                    // Get the answers from clients and calculate the points of each client and send relevant messages
                    else if (qcounter == 2)
                    {
                        numberQuestions--;
                        lineNum--;

                        richTextBox1.AppendText("Answer: " + line + "\n");
                        int realAnswer = Convert.ToInt32(line);


                        for (int i = 0; i < players.Count; i++)
                        {
                            players[i].abs = Math.Abs(realAnswer - players[i].answer);
                        }


                        double closest = players.Min(m => m.abs);
                        double closestCount = 0;
                        for (int i = 0; i < players.Count; i++)
                        {
                            if (players[i].abs == closest)
                            {
                                players[i].correct = true;
                                closestCount++;
                            }
                            else
                            {
                                players[i].correct = false;
                            }
                        }


                        string message = "Winners of this round:\n";
                        for (int i = 0; i < players.Count; i++)
                        {
                            if (players[i].correct)
                            {
                                players[i].point += 1 / closestCount;
                                message += players[i].name + "\n";
                            }
                        }
                        message += "\n";
                        message += "**************************\n";
                        message += "Score Table:\n";
                        for (int i = 0; i < players.Count; i++)
                        {
                            message += players[i].name + ": " + players[i].point + "\n";
                        }

                        message += "**************************\n";
                        Byte[] buffer2 = Encoding.Default.GetBytes(message);
                        for (int i = 0; i < players.Count; i++)
                        {
                            try
                            {
                                players[i].socket.Send(buffer2);

                            }
                            catch
                            {
                                richTextBox1.AppendText("There is a problem! Check the connection...\n");
                                listen_button.Enabled = true;
                                listening = false;
                            }

                        }
                        richTextBox1.AppendText(message + "\n");
                        qcounter--;

                        if (lineNum == 0)
                        {
                            goto Restart;
                        }

                    }


                }
            }






            // Display the clients with the highest points as winners at the end-game, while keeping the socket open for the next game
            double highest = players.Max(m => m.point);

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].point == highest)
                {
                    players[i].winner = true;
                }
                else
                {
                    players[i].winner = false;
                }
            }


            string message3 = "Endgame: Winners!\n";
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].winner)
                {
                    message3 += players[i].name + "\n";
                }
            }
            message3 += "**************************\n";
            message3 += "Score Table:\n";
            for (int i = 0; i < players.Count; i++)
            {
                message3 += players[i].name + ": " + players[i].point + "\n";
            }


            Byte[] buffer = Encoding.Default.GetBytes(message3);
            for (int i = 0; i < players.Count; i++)
            {
                try
                {
                    players[i].socket.Send(buffer);

                }
                catch
                {
                    richTextBox1.AppendText("There is a problem! Check the connection...\n");
                    listen_button.Enabled = true;
                    listening = false;
                }

            }
            richTextBox1.AppendText(message3 + "\n");
            button1.Enabled = true;


            //nicknames = new List<string>();
            //client_Socket = new List<Socket>();


            numberQuestions = questionNum;
            lineNum = questions.Length / 2;

            beforegameListen = true;
            //listening = true;
            //listen_button.Enabled = false;

            //Thread acceptThread2 = new Thread(Accept);
            //acceptThread2.Start();





        }


        // Handles the connection/disconnection of players to the server, before the game starts
        private void Receive1(Socket thisClient, string nameOf)
        {
            bool connected = true;
            while (connected && !terminating && beforegameListen)
            {
                try
                {
                    //richTextBox1.AppendText( "R1: "+ nameOf + "\n");
                    Byte[] buffer = new Byte[256];
                    thisClient.Receive(buffer);



                }
                catch
                {

                    richTextBox1.AppendText("Client: " + nameOf + " has disconnected.\n");

                    thisClient.Close();
                    client_Socket.Remove(thisClient);
                    players.Remove(players.Find(x => x.name == nameOf));
                    nicknames.Remove(nameOf);
                    connected = false;

                }
            }
        }

        private void path_label_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //Number of Quiz Questions textbox
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

        // Handles the cases when server is disabled by closing the form window
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string message = "GOODBYE";
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
                        listen_button.Enabled = true;
                        listening = false;
                    }



                }
            }
        }

        // Start Game button: starts the game with all players conected to the server
        private void button1_Click(object sender, EventArgs e)
        {


            button1.Enabled = false;
            beforegameListen = false;

            string message2 = "GOODBYE";


            Byte[] buffer2 = Encoding.Default.GetBytes(message2);
            for (int i = 0; i < players.Count; i++)
            {
                try
                {
                    players[i].point = 0;
                    players[i].socket.Send(buffer2);

                }
                catch
                {
                    richTextBox1.AppendText("There is a problem! Check the connection...\n");
                }

            }

            gameThread = new Thread(Game);
            gameThread.Start();

        }

        //Deactivate button: Close the current socket and create a new one
        private void button2_Click(object sender, EventArgs e)
        {
            deact = true;
            gameThread.Abort();
            for (int i = 0; i < players.Count; i++)
            {
                //players[i].socket.Shutdown(SocketShutdown.Receive);
                players[i].socket.Close();

            }
            
            server_Socket.Close();
            server_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            button1.Enabled = false;
            listen_button.Enabled = true;
            listening = false;
           

            nicknames = new List<string>();
            client_Socket = new List<Socket>();
            button2.Enabled = false;
            players = new List<player>();


        }

        private void port_textBox_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
