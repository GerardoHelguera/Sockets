using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SServidor
{
    class Program
    {

        private TcpListener servidor;
        private TcpClient User = new TcpClient();
        private IPEndPoint punto = new IPEndPoint(IPAddress.Any, 8000);
        private List<Connection> conec = new List<Connection>();

        Connection start;


        private struct Connection
        {
            public NetworkStream corr;
            public StreamWriter esc;
            public StreamReader lec;
            public string name;
        }

        public Program()
        {
            Inicializacion();
        }

        public void Inicializacion()
        {

            Console.WriteLine("Servidor encendido");
            servidor = new TcpListener(punto);
            servidor.Start();

            while (true)
            {
                User = servidor.AcceptTcpClient();

                start = new Connection();
                start.corr = User.GetStream();
                start.lec = new StreamReader(start.corr);
                start.esc = new StreamWriter(start.corr);

                start.name = start.lec.ReadLine();

                conec.Add(start);
                Console.WriteLine(start.name + "Conectado!");



                Thread hilo = new Thread(conexion_Serv);

                hilo.Start();
            }


        }

        void conexion_Serv()
        {
            Connection hcon = start;

            do
            {
                try
                {
                    string tmp = hcon.lec.ReadLine();
                    Console.WriteLine(hcon.name + "  " + tmp);
                    foreach (Connection en in conec)
                    {
                        try
                        {
                            en.esc.WriteLine(hcon.name + "  " + tmp);
                            en.esc.Flush();
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                    conec.Remove(hcon);
                    Console.WriteLine(start.name + " se a desconectado del servidor!.");
                    break;
                }
            } while (true);
        }
    }
}
