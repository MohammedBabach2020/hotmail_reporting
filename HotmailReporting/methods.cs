using IWshRuntimeLibrary;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace HotmailReporting
{
    public class methods
    {
        public static void CreateIceDragonProfileShortcut(string iceDragonPath, string profilePath, string profileName, string profilesFolderPath)
        {
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Path.Combine(profilesFolderPath, profileName + ".lnk"));
            shortcut.TargetPath = $"\"{iceDragonPath}\"";
            shortcut.Arguments = $" -P {profileName} -no-remote";
            shortcut.WorkingDirectory = Path.GetDirectoryName(iceDragonPath);
            shortcut.Description = "IceDragon Profile: " + profileName;
            shortcut.Save();
        }

        public static void UpdateProxySettings(string prefsFilePath, string proxy, int proxyPort)
        {
            string[] lines = { "user_pref(\"browser.shell.checkDefaultBrowser\", false);", "user_pref(\"network.cookie.prefsMigrated\", true);", "user_pref(\"network.predictor.cleane-up\", true);", $"user_pref(\"network.proxy.http\",{proxy});", $"user_pref( \"network.proxy.http_port\", {proxyPort});", "user_pref(\"network.proxy.type\", 1);" };

            System.IO.File.AppendAllLines(prefsFilePath, lines);
        }


      public   static bool TestProxy(string proxyAddress, int proxyPort)
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                // Set a timeout for the connection attempt (in milliseconds)
                int timeout = 5000;

                // Attempt to connect to the proxy
                IAsyncResult result = tcpClient.BeginConnect(proxyAddress, proxyPort, null, null);

                // Wait for the connection attempt to complete or timeout
                bool success = result.AsyncWaitHandle.WaitOne(timeout, true);

                if (success)
                {
                    // Connection successful, close the connection
                    tcpClient.EndConnect(result);
                    return true;
                }
                else
                {
                    // Connection attempt timed out or failed
                    tcpClient.Close();
                    return false;
                }
            }
        
    }
}
}
