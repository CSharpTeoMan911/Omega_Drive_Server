using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    class Client_Connections:Server_Application_Variables
    {
        protected static async Task<bool> Secure_Client_Connection(System.Net.Sockets.Socket client)
        {
            try
            {
                System.Net.Sockets.NetworkStream client_network_stream = new System.Net.Sockets.NetworkStream(client);

                try
                {
                    System.Net.Security.SslStream client_secure_socket_layer_stream = new System.Net.Security.SslStream(client_network_stream, false);

                    try
                    {
                        client_secure_socket_layer_stream.AuthenticateAsServer(server_certificate, false, System.Security.Authentication.SslProtocols.Tls11, false);


                        byte[] test_connection_send = Encoding.UTF8.GetBytes("OK");
                        byte[] test_connection_receive = new byte[Encoding.UTF8.GetBytes("OK").Length];

                        await client_secure_socket_layer_stream.WriteAsync(test_connection_send, 0, test_connection_send.Length);

                        await client_secure_socket_layer_stream.ReadAsync(test_connection_receive, 0, test_connection_receive.Length);
                    }
                    catch(Exception E)
                    {
                        System.Diagnostics.Debug.WriteLine(E.Message);
                        if(client_secure_socket_layer_stream != null)
                        {
                            client_secure_socket_layer_stream.Close();
                        }
                    }
                    finally
                    {
                        if (client_secure_socket_layer_stream != null)
                        {
                            client_secure_socket_layer_stream.Close();
                            await client_secure_socket_layer_stream.DisposeAsync();
                        }
                    }
                }
                catch (Exception E)
                {
                    System.Diagnostics.Debug.WriteLine(E.Message);
                    if (client_network_stream != null)
                    {
                        client_network_stream.Close();
                    }
                }
                finally
                {
                    if (client_network_stream != null)
                    {
                        client_network_stream.Close();
                        await client_network_stream.DisposeAsync();
                    }
                }
            }
            catch (Exception E)
            {
                System.Diagnostics.Debug.WriteLine(E.Message);
                if (client != null)
                {
                    client.Close();
                }
            }
            finally
            {
                if (client != null)
                {
                    client.Close();
                    client.Dispose();
                }
            }


            return true;
        }
    }
}
