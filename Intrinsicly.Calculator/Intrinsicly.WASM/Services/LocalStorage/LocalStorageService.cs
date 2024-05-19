﻿using Microsoft.JSInterop;
using System.Text.Json;
using System.Threading.Tasks;

namespace Intrinsicly.WASM.Services.LocalStorage
{
    public class LocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetItemAsync<T>(string key, T item)
        {
            var json = JsonSerializer.Serialize(item);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
        }

        public async Task<T> GetItemAsync<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            return json == null ? default : JsonSerializer.Deserialize<T>(json);
        }

        public async Task RemoveItemAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}