using System.Text;
using System.Net.Sockets;

using KIComunication.Core.Utiltiys;

namespace KIComunication.Core {
    internal class LocalClient {
        internal static TcpClient _cli;
        internal static NetworkStream _str;

        public static void _Instance() {
            if (_cli == null) {
                _cli = new TcpClient();
                _cli.Connect("Nu uh", 12345);
                _str = _cli.GetStream();
            }
        }

        public static void SendRequest(string dat) {
            _Instance();
            if (_cli == null || !_cli.Connected || _str == null) {
                ILogger.WrittenLog("[CLIENT] WARNING DATA STREAMS ARE NULL DATA CANT BE SENT OR RETURNED");
                return;
            }
            byte[] _dat = Encoding.ASCII.GetBytes(dat);
            _str.Write(_dat, 0, _dat.Length);
        }

        public static string Incoming() {
            int _RetyLimit = 5;
            int _Trys = 0;
            while (_Trys < _RetyLimit) {
                 if (_str.DataAvailable)
                  {
                     byte[] byts = new byte[1024];
                     int bytsR = _str.Read(byts, 0, byts.Length);
                     if (bytsR == 0)
                     {
                        End();
                        return string.Empty;
                     }
                     string _dat = Encoding.ASCII.GetString(byts, 0, bytsR);
                     return _dat;
                    }
                    else
                    {
                        _Trys++;
                        Thread.Sleep(100);
                    }
            }
            return "Couldn't Find data in the stream, Network Error try again later </3";
        }

        public static void End() {
          _str?.Close();
          _cli?.Close();
          _str = null;
          _cli = null;
        }

        public static string GetHwid() => System.Security.Principal.WindowsIdentity.GetCurrent().User.Value; // Not the best way, Im just lazy
    }
}
