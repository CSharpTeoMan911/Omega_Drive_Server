using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    class Client_Connections:Server_Application_Variables
    {
        private byte[] server_response = Encoding.UTF8.GetBytes("OK");


        private Server_Function_Selector server_function_selector = new Server_Function_Selector();


        internal async Task<bool> Secure_Client_Connection(System.Net.Sockets.Socket client)
        {
            try
            {
                client.SendBufferSize = 18000;
                client.ReceiveBufferSize = 18000;
                client.SendTimeout = 1000;
                client.ReceiveTimeout = 1000;


                int buffer_length = 0;

                System.Net.Sockets.NetworkStream client_network_stream = new System.Net.Sockets.NetworkStream(client);

                try
                {
                    System.Net.Security.SslStream client_secure_socket_layer_stream = new System.Net.Security.SslStream(client_network_stream, false, null, null, System.Net.Security.EncryptionPolicy.RequireEncryption);

                    try
                    {
                        client_secure_socket_layer_stream.AuthenticateAsServer(server_certificate, false, connection_ssl_protocol, false);

                        int bytes_per_second = await Connection_Speed_Calculator(((IPEndPoint)client.RemoteEndPoint).Address, client_secure_socket_layer_stream);


                        if(bytes_per_second > 0)
                        {
                            byte[] client_payload_size_buffer = new byte[1024];
                            buffer_length = client_payload_size_buffer.Length;

                            await Calculate_Timeout(client, ref buffer_length, ref bytes_per_second);

                            await client_secure_socket_layer_stream.ReadAsync(client_payload_size_buffer, 0, buffer_length);

                            await client_secure_socket_layer_stream.FlushAsync();




                            buffer_length = server_response.Length;

                            await Calculate_Timeout(client, ref buffer_length, ref bytes_per_second);

                            await client_secure_socket_layer_stream.WriteAsync(server_response, 0, buffer_length);

                            await client_secure_socket_layer_stream.FlushAsync();





                            byte[] client_payload_buffer = new byte[BitConverter.ToInt32(client_payload_size_buffer, 0)];

                            buffer_length = client_payload_buffer.Length;

                            await Calculate_Timeout(client, ref buffer_length, ref bytes_per_second);

                            int total_bytes_read = 0;

                            while (total_bytes_read < client_payload_buffer.Length)
                            {
                                total_bytes_read += await client_secure_socket_layer_stream.ReadAsync(client_payload_buffer, total_bytes_read, buffer_length - total_bytes_read);
                            }








                            Payload_Serialization payload_Serialization = new Payload_Serialization();
                            Client_WSDL_Payload client_WSDL_Payload = await payload_Serialization.Deserialize_Payload(client_payload_buffer);


                            byte[] server_payload = await server_function_selector.Server_Function_Selection(client_WSDL_Payload);









                            byte[] server_payload_length = BitConverter.GetBytes(server_payload.Length);

                            buffer_length = server_payload_length.Length;

                            await Calculate_Timeout(client, ref buffer_length, ref bytes_per_second);

                            await client_secure_socket_layer_stream.WriteAsync(server_payload_length, 0, buffer_length);

                            await client_secure_socket_layer_stream.FlushAsync();







                            byte[] client_response = new byte[Encoding.UTF8.GetBytes("OK").Length];

                            buffer_length = client_response.Length;

                            await Calculate_Timeout(client, ref buffer_length, ref bytes_per_second);

                            await client_secure_socket_layer_stream.ReadAsync(client_response, 0, client_response.Length);

                            await client_secure_socket_layer_stream.FlushAsync();







                            buffer_length = server_payload.Length;

                            await Calculate_Timeout(client, ref buffer_length, ref bytes_per_second);

                            await client_secure_socket_layer_stream.WriteAsync(server_payload, 0, buffer_length);

                            await client_secure_socket_layer_stream.FlushAsync();
                        }

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
            catch (Exception E)
            {
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


        private async Task<int> Connection_Speed_Calculator(System.Net.IPAddress IP_Address, System.Net.Security.SslStream client_secure_socket_layer_stream)
        {
            int round_trip_time_counter = 0;
            int calculated_average_round_trip_time = 0;
            int bytes_per_second = 0;


            byte[] packet = new byte[1500];

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();


            if (IP_Address != null)
            {

                try
                {

                Ping_Test:

                    stopwatch.Start();

                    await client_secure_socket_layer_stream.ReadAsync(packet, 0, packet.Length);

                    await client_secure_socket_layer_stream.WriteAsync(packet, 0, packet.Length);

                    stopwatch.Stop();



                    if (round_trip_time_counter < 10)
                    {
                        calculated_average_round_trip_time += (int)stopwatch.ElapsedMilliseconds;
                        stopwatch.Reset();
                        round_trip_time_counter++;

                        goto Ping_Test;
                    }





                    if (calculated_average_round_trip_time > 0)
                    {
                        calculated_average_round_trip_time = calculated_average_round_trip_time / 10;

                        if (calculated_average_round_trip_time == 0)
                        {
                            calculated_average_round_trip_time = 1;
                        }
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
                catch
                {
                    bytes_per_second = -1;
                }
            }


            return bytes_per_second;
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Task<bool> Calculate_Timeout(System.Net.Sockets.Socket client, ref int payload_size, ref int bytes_per_second)
        {
            client.SendBufferSize = payload_size / bytes_per_second + 1000;
            client.ReceiveBufferSize = payload_size / bytes_per_second + 1000;

            return Task.FromResult(true);
        }
    }
}
