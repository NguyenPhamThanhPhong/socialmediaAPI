﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace socialmediaAPI.RequestsResponses.Requests
{
#pragma warning disable CS8618

    public class UpdateParameter
    {
        [Required]
        public string FieldName { get; set; }
        public object? Value { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UpdateAction updateAction { get; set; }

        public UpdateParameter()
        {

        }
        public UpdateParameter(string fieldName,object value, UpdateAction action)
        {
            FieldName = fieldName;
            Value = value;
            updateAction = action;
        }
    }
    public enum UpdateAction
    {
        [Description("set")]
        set,
        [Description("push")]
        push,
        [Description("pull")]
        pull
    }
}
