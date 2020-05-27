﻿using System;
using Example.Mobile.Infrastructure.Serialization;
using Utf8Json;

namespace Example.Mobile.Serialization.UTF8Json.Query
{
    internal sealed class QueryDeserializer : IQueryDeserializer
    {
        public object Deserialize(string data, Type type)
        {
            var method = typeof(JsonSerializer).GetMethod(nameof(JsonSerializer.Deserialize));
            var generic = method.MakeGenericMethod(type);
            return  generic.Invoke(data, null);
        }

        public T Deserialize<T>(string data)
        {
            return JsonSerializer.Deserialize<T>(data);
        }
    }
}
