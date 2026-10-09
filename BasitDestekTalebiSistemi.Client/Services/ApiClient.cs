using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using BasitDestekTalebiSistemi.Core.Constants;
using BasitDestekTalebiSistemi.Entities.DTOs;

namespace BasitDestekTalebiSistemi.Client.Services
{

    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiClient(string baseUrl = "http://localhost:5000")
        {

            var formattedBaseUrl = baseUrl.TrimEnd('/') + "/";

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(formattedBaseUrl),
                Timeout = TimeSpan.FromSeconds(10)
            };

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<ApiResponse<List<TicketSummaryResponse>>> GetTicketsAsync()
        {
            try
            {

                var response = await _httpClient.GetAsync("api/tickets");

                if (!response.IsSuccessStatusCode)
                {
                    return await HandleErrorResponseAsync<List<TicketSummaryResponse>>(response);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<TicketSummaryResponse>>>(_jsonOptions);
                return result ?? new ApiResponse<List<TicketSummaryResponse>> { Success = false, Message = Messages.EmptyResponse };
            }
            catch (HttpRequestException)
            {
                return new ApiResponse<List<TicketSummaryResponse>>
                {
                    Success = false,
                    Message = Messages.ServerConnectionError
                };
            }
            catch (TaskCanceledException)
            {
                return new ApiResponse<List<TicketSummaryResponse>>
                {
                    Success = false,
                    Message = Messages.ServerTimeout
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<TicketSummaryResponse>>
                {
                    Success = false,
                    Message = $"Talepler alınırken hata: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<TicketDetailResponse>> GetTicketDetailAsync(int id)
        {
            try
            {

                var response = await _httpClient.GetAsync($"api/tickets/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return await HandleErrorResponseAsync<TicketDetailResponse>(response);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<TicketDetailResponse>>(_jsonOptions);
                return result ?? new ApiResponse<TicketDetailResponse> { Success = false, Message = Messages.EmptyResponse };
            }
            catch (HttpRequestException)
            {
                return new ApiResponse<TicketDetailResponse>
                {
                    Success = false,
                    Message = Messages.ServerConnectionError
                };
            }
            catch (TaskCanceledException)
            {
                return new ApiResponse<TicketDetailResponse>
                {
                    Success = false,
                    Message = Messages.ServerTimeout
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TicketDetailResponse>
                {
                    Success = false,
                    Message = $"Talep detayı alınırken hata: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<TicketDetailResponse>> CreateTicketAsync(CreateTicketRequest request)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync("api/tickets", request);

                if (!response.IsSuccessStatusCode)
                {
                    return await HandleErrorResponseAsync<TicketDetailResponse>(response);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<TicketDetailResponse>>(_jsonOptions);
                return result ?? new ApiResponse<TicketDetailResponse> { Success = false, Message = Messages.EmptyResponse };
            }
            catch (HttpRequestException)
            {
                return new ApiResponse<TicketDetailResponse>
                {
                    Success = false,
                    Message = Messages.ServerConnectionError
                };
            }
            catch (TaskCanceledException)
            {
                return new ApiResponse<TicketDetailResponse>
                {
                    Success = false,
                    Message = Messages.ServerTimeout
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TicketDetailResponse>
                {
                    Success = false,
                    Message = $"Talep oluşturulurken hata: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<List<ReplyResponse>>> GetRepliesAsync(int ticketId)
        {
            try
            {

                var response = await _httpClient.GetAsync($"api/tickets/{ticketId}/replies");

                if (!response.IsSuccessStatusCode)
                {
                    return await HandleErrorResponseAsync<List<ReplyResponse>>(response);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<ReplyResponse>>>(_jsonOptions);
                return result ?? new ApiResponse<List<ReplyResponse>> { Success = false, Message = Messages.EmptyResponse };
            }
            catch (HttpRequestException)
            {
                return new ApiResponse<List<ReplyResponse>>
                {
                    Success = false,
                    Message = Messages.ServerConnectionError
                };
            }
            catch (TaskCanceledException)
            {
                return new ApiResponse<List<ReplyResponse>>
                {
                    Success = false,
                    Message = Messages.ServerTimeout
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ReplyResponse>>
                {
                    Success = false,
                    Message = $"Yanıtlar alınırken hata: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<ReplyResponse>> AddReplyAsync(int ticketId, CreateReplyRequest request)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync($"api/tickets/{ticketId}/replies", request);

                if (!response.IsSuccessStatusCode)
                {
                    return await HandleErrorResponseAsync<ReplyResponse>(response);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ReplyResponse>>(_jsonOptions);
                return result ?? new ApiResponse<ReplyResponse> { Success = false, Message = Messages.EmptyResponse };
            }
            catch (HttpRequestException)
            {
                return new ApiResponse<ReplyResponse>
                {
                    Success = false,
                    Message = Messages.ServerConnectionError
                };
            }
            catch (TaskCanceledException)
            {
                return new ApiResponse<ReplyResponse>
                {
                    Success = false,
                    Message = Messages.ServerTimeout
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ReplyResponse>
                {
                    Success = false,
                    Message = $"Yanıt gönderilirken hata: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<object>> CloseTicketAsync(int ticketId)
        {
            try
            {

                var response = await _httpClient.PutAsync($"api/tickets/{ticketId}/close", null);

                if (!response.IsSuccessStatusCode)
                {
                    return await HandleErrorResponseAsync<object>(response);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>(_jsonOptions);
                return result ?? new ApiResponse<object> { Success = false, Message = Messages.EmptyResponse };
            }
            catch (HttpRequestException)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = Messages.ServerConnectionError
                };
            }
            catch (TaskCanceledException)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = Messages.ServerTimeout
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Talep kapatılırken hata: {ex.Message}"
                };
            }
        }

        private async Task<ApiResponse<T>> HandleErrorResponseAsync<T>(HttpResponseMessage response)
        {
            try
            {

                var errorResult = await response.Content.ReadFromJsonAsync<ApiResponse<T>>(_jsonOptions);
                if (errorResult != null && !string.IsNullOrWhiteSpace(errorResult.Message))
                {
                    return errorResult;
                }
            }
            catch
            {

            }

            var message = response.StatusCode switch
            {
                System.Net.HttpStatusCode.NotFound => Messages.NotFound404,
                System.Net.HttpStatusCode.BadRequest => Messages.BadRequest400,
                System.Net.HttpStatusCode.InternalServerError => Messages.InternalServerError500,
                _ => $"Sunucu hatası: {response.StatusCode} ({(int)response.StatusCode})"
            };

            return new ApiResponse<T> { Success = false, Message = message };
        }
    }

    public class ApiResponse<T>
    {

        public bool Success { get; set; }

        public string? Message { get; set; }

        public T? Data { get; set; }
    }
}
