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

        public SerialRCSender()
        {
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
    }


}
