using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;

using IniParser.Model;
using System.Diagnostics;

namespace FPVRemote.RCSender
{
    public class SerialRCSender
    {
        SerialPort port;
        bool enabled;
        int numOfChannels;

        public int NumOfChannels
        {
            get { return numOfChannels;  }
        }


        public SerialRCSender(int numOfChannels)
        {
            this.numOfChannels = numOfChannels;
        }

        public SerialRCSender InitFromConfig(IniData configData, string key)
        {
            enabled = bool.Parse(configData[key]["enabled"]);
            if (enabled)
            {
                string portName = ResolvePortName(configData[key]["port"]);
                if (portName == null)
                {
                    throw new Exception("RC sender device is not attached. \n\n(Reason: No COM port detected.)");
                };
                port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
                port.Open();
            }
            return this;
        }

        private string ResolvePortName(string portName)
        {
            string resultPortName = portName;
            if ("auto".Equals(portName))
            {
                Debug.WriteLine("Auto port detect start: ");
                resultPortName = null;
                if (SerialPort.GetPortNames().Length > 0)
                {
                    resultPortName = SerialPort.GetPortNames()[0];
                }
                Debug.WriteLine("Auto port detect resolved name: " + resultPortName);
            }

            return resultPortName;
        }

        public void Send(string value)
        {
            if (enabled)
            {
                port.Write(value);
            }
        }

        public void sendValues(short[] values)
        {
            StringBuilder res = new StringBuilder();
            res.Append("*");
            for (int i = 0; i < numOfChannels - 1; i++)
            {
                res.Append(values[i].ToString()).Append(" ");
            }
            //last one without a trailing space
            res.Append(values[numOfChannels - 1].ToString())
                .Append("\n");

            Send(res.ToString());
        }
    }


}
