using System;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using Deckapp;

namespace Deckapp
{
    class SerialCom
    {
        static SerialPort serialPort;


        public static void StartSerialCom() {
            SearchCom();
            Timer updateSessionTimer = new Timer();
            updateSessionTimer.Tick += new EventHandler(delegate (object o, EventArgs a)
            {
                SerialConection();
            });
            updateSessionTimer.Interval = 10; // in miliseconds
            updateSessionTimer.Start();
        }
        static void SerialConection()
        {

            string message;
            Char[] lastValues = new Char[12];
            if (serialPort.BytesToRead > 0)
              {

                    message = serialPort.ReadLine();
                    string[] subs = message.Split(';');
                    //Console.WriteLine(string.Join(", ", subs));
                    foreach (string s in subs)
                    {
                        if(s.StartsWith("Fader"))
                        {
                            string[] splited = s.Split(':');
                            Int32.TryParse(splited[1], out int volume);
                            Int32.TryParse(splited[0].Remove(0, 5), out int faderid);

                            Fader.Faders[faderid].setVolume(volume / 100.0F);

                        }
                        if (s.StartsWith("Buttons:"))
                        {
                            Char[] Values = s.Remove(0, 8).ToCharArray();
                            for (int i = 0; i < Values.Length; i++)
                            {
                                if (Values[i] != lastValues[i])
                                {
                                    if (Values[i].ToString() == "1")
                                    {
                                        DeckButton.Buttons[i].press();
                                    }
                                }
                            }
                            lastValues = Values;
                        } 
                    }
                }
       
        }






        public static bool SearchCom() {
            foreach (string COM in SerialPort.GetPortNames())
            {
                Console.WriteLine($"SearchCom: {COM}");
                try {
                    serialPort = new SerialPort(COM, 9600);
                    serialPort.Open();
                    
                }
                catch
                {
                    Console.WriteLine("Com Error");
                }
                serialPort.WriteLine("Hi");
                System.Threading.Thread.Sleep(100);
                if (serialPort.BytesToRead > 0)
                {
                    string message = serialPort.ReadLine();
                    Console.WriteLine($"Message: {message}");
                    if (message == "Hi")
                    {
                        Console.WriteLine("Device conected");
                        return true;
                    }
                }
                //serialPort.Close();
                Console.WriteLine("Device is not a Deck");
                return false;
            }
            return false;
        }
    }
}
