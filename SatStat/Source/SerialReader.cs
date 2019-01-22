using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace SatStat
{
    class SerialReader
    {
        private SerialPort sp = new SerialPort("COM3", 115200, Parity.None, 8, StopBits.One);
        private string input = "";

        private Action<string> dataReceivedCallback;
        private Thread readThread;

        private bool running;

        public SerialReader()
        {
             readThread = new Thread(Read);
        }

        public void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            input = sp.ReadLine();
            dataReceivedCallback.Invoke(input);
        }
        public void Run()
        {
            Console.WriteLine("Starting serial reader");

            //Set the datareceived event handler
            //sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
            //Open the serial port
            sp.Open();
            //Read from the console, to stop it from closing.

            running = true;
            readThread.Start();
        }

        public void Read()
        {
            while(running)
            {
                input = sp.ReadLine();
                dataReceivedCallback.Invoke(input);
            }

        }

        public void Stop()
        {
            running = false;
        }

        public string GetData()
        {
            return input;
        }

        public void OnDataReceived(Action<string> cb)
        {
            dataReceivedCallback = cb;
        }
    }
}
