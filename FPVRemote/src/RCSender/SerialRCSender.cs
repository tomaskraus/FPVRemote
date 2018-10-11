using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;

using IniParser.Model;


namespace FPVRemote.RCSender
{
    public class SerialRCSender
    {
        SerialPort port;
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
            port = new SerialPort(configData[key]["port"], 9600, Parity.None, 8, StopBits.One);
            port.Open();
            return this;
        }

        public void Send(string value)
        {
            port.Write(value);
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
