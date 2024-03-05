using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace RazorShop.Client;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _http;

    public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient http)
    {
        _localStorage = localStorage; // Инициализация сервиса локального хранилища для сохранения токена аутентификации.
        _http = http; // Инициализация HTTP-клиента для выполнения HTTP-запросов.
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authToken = await _localStorage.GetItemAsStringAsync("authToken"); // Получение токена аутентификации из локального хранилища.
        var identity = new ClaimsIdentity(); // Создание новой идентичности пользователя.

        _http.DefaultRequestHeaders.Authorization = null; // Очистка заголовка авторизации HTTP-клиента.

        if (!string.IsNullOrEmpty(authToken)) // Проверка наличия токена аутентификации.
        {
            try
            {
                identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt"); // Парсинг токена аутентификации и создание идентичности пользователя.
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Replace("\"", "")); // Установка заголовка авторизации HTTP-клиента с использованием токена аутентификации.
            }
            catch (Exception e)
            {
                await _localStorage.RemoveItemAsync("authToken"); // Удаление токена аутентификации из локального хранилища в случае ошибки при парсинге.
                identity = new ClaimsIdentity(); // Создание пустой идентичности пользователя.
            }
        }

        var user = new ClaimsPrincipal(identity); // Создание объекта, представляющего аутентифицированного пользователя.
        var state = new AuthenticationState(user); // Создание объекта, представляющего состояние аутентификации.

        NotifyAuthenticationStateChanged(Task.FromResult(state)); // Уведомление о изменении состояния аутентификации.

        return state; // Возврат состояния аутентификации.
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4) // Вычисление количества отсутствующих символов заполнения в строке base64.
        {
            case 2 :
                base64 += "=="; break; // Добавление двух символов заполнения, если количество символов кратно 2.
            case 3: 
                base64 += "="; break; // Добавление одного символа заполнения, если количество символов кратно 3.
        }

        return Convert.FromBase64String(base64); // Декодирование строки base64 без символов заполнения.
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1]; // Извлечение части токена JWT, содержащей payload.
        var jsonBytes = ParseBase64WithoutPadding(payload); // Декодирование payload из формата Base64.
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, Object>>(jsonBytes); // Десериализация payload в словарь пар ключ-значение.

        var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())); // Создание списка утверждений на основе пар ключ-значение.
        return claims; // Возврат списка утверждений.
    }
}