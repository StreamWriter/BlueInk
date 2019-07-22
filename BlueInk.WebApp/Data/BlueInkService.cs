using BlueInk.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlueInk.WebApp.Data
{
    public class BlueInkService
    {
        private static HttpClient _client;

        public BlueInkService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:44308");
        }

        public async Task<string> Register(UserCredentials credentials)
        {
            var result = await _client.PostAsync("/auth/register", new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json"));
            var content = await result.Content.ReadAsStringAsync();

            return content;
        }

        public async Task<OwnerData> GetOwnerDataAsync()
        {
            var result = await _client.GetAsync("/api/ownerData");
            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<OwnerData>(content);
        }

        public async Task<Project> GetProjectAsync(int projectId)
        {
            var result = await _client.GetAsync($"/api/project/{projectId}");
            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Project>(content);
        }

        public async Task<ICollection<ProjectType>> GetProjectTypesAsync()
        {
            var result = await _client.GetAsync("/api/projectTypes");
            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ICollection<ProjectType>>(content);
        }

        public async Task<ProjectType> GetProjectTypeAsync(int projectTypeId)
        {
            var result = await _client.GetAsync($"/api/projectTypes/{projectTypeId}");
            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ProjectType>(content);
        }

        public async Task<ICollection<Project>> GetProjectTypeProjectsAsync(int projectTypeId)
        {
            var result = await _client.GetAsync($"/api/projectTypes/{projectTypeId}/projects");
            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ICollection<Project>>(content);
        }

        public async Task<ICollection<Project>> GetProjectsAsync()
        {
            var result = await _client.GetAsync("/api/projects");
            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ICollection<Project>>(content);
        }



    }
}
