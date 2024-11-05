using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

using KINetwork.Global.Utilitys;
using KINetwork.KIServer.Core;
using KINetwork.KIServer.CatEngine;

namespace KINetwork {
    class Server
    {
        private static TcpListener _lis;
        public Server(int _p) => _lis = new TcpListener(IPAddress.Any, _p);
        public async Task Start()
        {
            ILogger.ILog("KI Network Is booting up", 0, 0);
            Mapper._Blacklist();
            _lis.Start();
            ILogger.ILog("KI Network has finished booting up and is waiting for Request", 0, 0);

            while (true)  {
                var cli = await Task.Run(() => _lis.AcceptTcpClient());
                _ = Task.Run(() => server(cli));
            }
        }

        private static void server(TcpClient cli) {
            var (str, ip) = ((Func<TcpClient, (NetworkStream, string)>)(_cli => (_cli.GetStream(), ((IPEndPoint)_cli.Client.RemoteEndPoint).Address.ToString())))(cli);
            try
            {
                if (NetChecks.BlacklistCheck(ip, null, null))  {
                    ILogger.ILog($"Blacklisted IP Attempted to Connect to server\n     [ Data-Block | Ip : {ip} ]", 2, 0);
                    cli.Close();
                    return;
                }

                while (true)  { // c:
                    var (buf, byt) = ((Func<NetworkStream, (byte[], int)>)(ste => { byte[] buff = new byte[4096]; int bytr = ste.Read(buff, 0, buff.Length); return (buff, bytr); }))(str);

                    if (byt == 0)
                        break;

                    if (NetChecks.RatelimitCheck(ip)) {
                        ILogger.ILog($"API Ratelimit Hit\n     [ Data-Block | Ip : {ip} ]", 2, 0);
                        Functions.Outgoing("__E_R_M__N_O__L_I_F_E__", str);
                        continue;
                    }

                    string[] dat = Encoding.ASCII.GetString(buf, 0, byt).Split('|');
                    switch (dat[0])  {
                        case "/login":
                            Functions.HandleLogin(dat, ip, str);
                            break;
                        case "/register":
                            Functions.HandleRegister(dat, ip, str);
                            break;
                        case "/request2":
                            Functions.HandleRequest1(dat, ip, str);
                            break;
                        case "/networked":
                            Functions.HandleWebhook(dat);
                            break;

                        /// NON AUTHETNTICATION CALLS

                        case "/AssetLogAvatar":
                            Functions.HandleInternalAvatar(dat);
                            break;
                        case "/NetworkedNameplateTag":
                            Functions.HandleInternalTag(dat, str);
                            break;
                        case "/NetworkedSearchBot":
                            Functions.HandleInternalSearch(dat);
                            break;
                        case "/console":
                            Functions.HandleNetworkConsole(dat);
                            break;

                        default:
                            ILogger.ILog($"Invaild Command Give to server, Blacklisting ip from futher connection\n     [ Data-Block | Ip : {ip}, Data : {dat[0]} ]", 1, 0);
                            NetChecks.Blacklist(ip, null, null);
                            cli.Close();
                            return;
                    }
                }
            }
            catch (Exception e)  {
                ILogger.ILog($"Server had a Hard Fail-Point with a clients network handle\n     [ Data-Block | Ip : {ip}, Error : {e} ]", 3, 0);
            }
            finally {
                cli.Close();
            }
        }
    }
}
