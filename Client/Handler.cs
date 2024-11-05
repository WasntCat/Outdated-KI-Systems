using System.Text;
using System.Net.Sockets;
using System.Security.Cryptography;

using KIComunication.Core.Utiltiys;
using static KIComunication.Core.LocalClient;

namespace KIComunication {
    public class Handler {
        // Don't, i was lazy 
        public static bool ResonseBoolen = false;
        internal static bool LocalLogged = false;
        public static string SessionToken { get; private set; }

        public static void ILogin(string usr, string pass) {
            SendRequest($"/login|{usr}|{pass}|{GetHwid()}");
            string dat = Incoming();

            if (dat.StartsWith("__D_A_R_L_I_N_G__")) {
                SessionToken = dat.Split('|')[1];
                ResonseBoolen = true;
                LocalLogged = true;
            } else if (dat.StartsWith("__W_A_S_T_E_D__")) {
                // Accounts Expierd
                LocalLogged = false;
                ResonseBoolen = false;
            } else if (dat.StartsWith("__D_I_E__")) {
                // Account Doesnt exist/someone tried to brute
                LocalLogged = false;
                ResonseBoolen = false;
            } else {
                ResonseBoolen = false;
                LocalLogged = false;
            }
        }

        public static void IRegister(string usr, string pass, string inv) {
            SendRequest($"/register|{usr}|{pass}|{GetHwid()}|{inv}");
            string dat = Incoming();

            if (dat.StartsWith("__M_Y__L_O_V_E__")) {
                ResonseBoolen = true;
                if (!Mapper.key) {
                    File.Create($"{Mapper.Rsc}\\KeyData.Darling").Dispose();
                    File.Create($"{Mapper.Rsc}\\DscData.Darling").Dispose();
                }
                File.WriteAllText($"{Mapper.Rsc}\\DscData.Darling", usr);
                File.WriteAllText($"{Mapper.Rsc}\\KeyData.Darling", pass);
            } else if (dat.StartsWith("__F_A_K_E__")) {
                ResonseBoolen = true;
                LocalLogged = false;
            } else {
                ResonseBoolen = false;
                LocalLogged = false;
            }
        }

        public static void IMessage(string msg, string usr) => SendRequest($"/networked|{msg}|{usr}|{GetHwid()}");

	// Changed to AES like the server, no using my method c:
        public static byte[] IRequest(string usr, string proxy) {
            SendRequest($"/request2|{usr}|{GetHwid()}|{proxy}|{SessionToken}");
            byte[] key = new byte[32];
            byte[] iv = new byte[16];
            Reader(_str, key);
            Reader(_str, iv);
            byte[] siz = new byte[4];
            Reader(_str, siz);
            Array.Reverse(siz);
            byte[] encypt = new byte[BitConverter.ToInt32(siz, 0)];
            Reader(_str, encypt);
            byte[] decpt;
            using (Aes aes = Aes.Create()) {
                using (var rev = aes.CreateDecryptor(key, iv))
                using (var ms = new MemoryStream(encypt))
                using (var cs = new CryptoStream(ms, rev, CryptoStreamMode.Read))
                using (var outt = new MemoryStream()) {
                    cs.CopyTo(outt);
                    decpt = outt.ToArray();
                }
            }
            return decpt;
        }

        private static void Reader(NetworkStream str, byte[] buff) {
            int of = 0;
            while (of < buff.Length) {
                int byt = str.Read(buff, of, buff.Length - of);
                if (byt == 0)
                    throw new IOException("__D_A_T_A__E_N_D_E_D__");
                of += byt;
            }
        }
    }
}
