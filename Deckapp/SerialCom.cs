using System;
using System.IO.Ports;
using Deckapp;

namespace Deckapp
{
    class SerialCom
    {
        static SerialPort serialPort;
        public static void StartSerialCom() {
            Thread serialConection = new Thread(SerialConection);
            serialConection.Start();
        }
        static void SerialConection()
        {
            SearchCom();

            string message;
            while (true)
            {
                if (serialPort.BytesToRead > 0)
                {

                    message = serialPort.ReadLine();
                    string[] subs = message.Split(';');
                    Console.WriteLine(string.Join(", ", subs));
                    foreach (string s in subs)
                    {
                        if(s.StartsWith("Fader"))
                        {
                            string[] splited = s.Split(':');
                            Int32.TryParse(splited[1], out int volume);
                            Int32.TryParse(splited[0].Remove(0, 5), out int faderid);
                            
                            Fader.Faders[faderid].selectedSession.SimpleAudioVolume.MasterVolume = volume /100.0F;

                        }
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
                Thread.Sleep(100);
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
