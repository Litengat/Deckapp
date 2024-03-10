using System;
using System.IO.Ports;  


namespace Deckapp
{
    class SerialCom
    {
        SerialPort serialPort;
        public SerialCom() {
            Thread serialConection = new Thread(SerialConection);
            serialConection.Start();
        }
        public void SerialConection()
        {
            SearchCom();

            string message;
            Console.WriteLine("te");
            while (true)
            {
                if (serialPort.BytesToRead > 0)
                {

                    message = serialPort.ReadLine();
                    string[] subs = message.Split(';');
                    foreach (string s in subs)
                    {
                        if(s.StartsWith("Fader1"))
                        {
                            string[] splited = message.Split(':');
                            Int32.TryParse(splited[1], out int volume);
                            Console.WriteLine(volume);
                            Deckapp.MainWindow.selectedSession.SimpleAudioVolume.MasterVolume = volume / 100.0F;
                        }
                    }
                }
            }
        }






        public bool SearchCom() {
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
                Thread.Sleep(1000);
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
