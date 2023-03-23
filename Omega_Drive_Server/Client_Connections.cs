using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    class Client_Connections:Server_Application_Variables
    {
        internal async Task<bool> Secure_Client_Connection(System.Net.Sockets.Socket client)
        {
            try
            {
                client.SendBufferSize = 18000;
                client.ReceiveBufferSize = 18000;

                int connection_speed = await Connection_Speed_Calculator(((IPEndPoint)client.RemoteEndPoint).Address);

                if(connection_speed > 0)
                {
                    int timeout = 18000 / connection_speed + 1000;

                    client.SendTimeout = timeout;
                    client.ReceiveTimeout = timeout;


                    System.Net.Sockets.NetworkStream client_network_stream = new System.Net.Sockets.NetworkStream(client);

                    try
                    {
                        System.Net.Security.SslStream client_secure_socket_layer_stream = new System.Net.Security.SslStream(client_network_stream, false);

                        try
                        {
                            client_secure_socket_layer_stream.AuthenticateAsServer(server_certificate, false, connection_ssl_protocol, false);
                        }
                        catch (Exception E)
                        {
                            if (client_secure_socket_layer_stream != null)
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


        private static Task<int> Connection_Speed_Calculator(System.Net.IPAddress IP_Address)
        {
            int round_trip_time_counter = 0;
            int calculated_average_round_trip_time = 0;
            int bytes_per_second = 0;


            if (IP_Address != null)
            {
                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();


                System.Net.NetworkInformation.PingOptions ping_options = new System.Net.NetworkInformation.PingOptions();
                ping_options.DontFragment = true;



                while (round_trip_time_counter < 10)
                {
                    System.Net.NetworkInformation.PingReply ping_reply = ping.Send(IP_Address, 100, new byte[1500], ping_options);

                    if (ping_reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                    {
                        calculated_average_round_trip_time += (int)ping_reply.RoundtripTime;
                    }
                    else
                    {
                        bytes_per_second = -1;
                        goto Ping_Failed;
                    }

                    round_trip_time_counter++;
                }



                if (calculated_average_round_trip_time > 0)
                {
                    calculated_average_round_trip_time = calculated_average_round_trip_time / 10;
                }
                else
                {
                    calculated_average_round_trip_time = 1;
                }


                bytes_per_second = 24 / calculated_average_round_trip_time * 125000;

                if (bytes_per_second < 1)
                {
                    bytes_per_second = 1;
                }
            }


            Ping_Failed:
            return Task.FromResult(bytes_per_second);

        }
    }
}
