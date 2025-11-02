using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SecuritySystemClient.Services
{
    public class ServerConnection
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private string _serverIP = "127.0.0.1";
        private int _serverPort = 5000;

        public bool IsConnected => _client?.Connected ?? false;

        public async Task<bool> ConnectAsync()
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(_serverIP, _serverPort);
                _stream = _client.GetStream();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка підключення: {ex.Message}");
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                _stream?.Close();
                _client?.Close();
            }
            catch { }
        }

        private async Task<string> SendRequestAsync(string command, object data)
        {
            try
            {
                var message = new { Command = command, Data = data };
                string json = JsonConvert.SerializeObject(message);
                byte[] requestData = Encoding.UTF8.GetBytes(json);

                await _stream.WriteAsync(requestData, 0, requestData.Length);
                await _stream.FlushAsync();

                byte[] buffer = new byte[4096];
                int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка відправки запиту: {ex.Message}");
            }
        }

        public async Task<ServerResponse> LoginAsync(string username, string password)
        {
            var data = new { Username = username, Password = password };
            string response = await SendRequestAsync("LOGIN", data);
            return JsonConvert.DeserializeObject<ServerResponse>(response);
        }

        public async Task<ServerResponse> RegisterAsync(string username, string password, string role)
        {
            var data = new { Username = username, Password = password, Role = role };
            string response = await SendRequestAsync("REGISTER", data);
            return JsonConvert.DeserializeObject<ServerResponse>(response);
        }

        public async Task<CamerasResponse> GetCamerasAsync()
        {
            string response = await SendRequestAsync("GET_CAMERAS", null);
            return JsonConvert.DeserializeObject<CamerasResponse>(response);
        }

        public async Task<NotificationsResponse> GetNotificationsAsync()
        {
            string response = await SendRequestAsync("GET_NOTIFICATIONS", null);
            return JsonConvert.DeserializeObject<NotificationsResponse>(response);
        }

        public async Task<ServerResponse> AddCameraAsync(string name, string location, bool isActive)
        {
            var data = new { Name = name, Location = location, IsActive = isActive };
            string response = await SendRequestAsync("ADD_CAMERA", data);
            return JsonConvert.DeserializeObject<ServerResponse>(response);
        }

        public async Task<ServerResponse> UpdateCameraAsync(int id, string name, string location, bool isActive)
        {
            var data = new { Id = id, Name = name, Location = location, IsActive = isActive };
            string response = await SendRequestAsync("UPDATE_CAMERA", data);
            return JsonConvert.DeserializeObject<ServerResponse>(response);
        }

        public async Task<ServerResponse> DeleteCameraAsync(int id)
        {
            var data = new { Id = id };
            string response = await SendRequestAsync("DELETE_CAMERA", data);
            return JsonConvert.DeserializeObject<ServerResponse>(response);
        }
    }

    // Класи відповідей
    public class ServerResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }

    public class CamerasResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public System.Collections.Generic.List<CameraDTO> Data { get; set; }
    }

    public class CameraDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
    }

    public class NotificationsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public System.Collections.Generic.List<NotificationDTO> Data { get; set; }
    }

    public class NotificationDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public System.DateTime Timestamp { get; set; }
        public int CameraId { get; set; }
        public string Severity { get; set; }
    }
}