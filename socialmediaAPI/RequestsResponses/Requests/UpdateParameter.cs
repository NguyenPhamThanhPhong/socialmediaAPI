using System.ComponentModel.DataAnnotations;

namespace socialmediaAPI.RequestsResponses.Requests
{
#pragma warning disable CS8618

    public class UpdateParameter
    {
        [Required]
        public string FieldName { get; set; }
        public object? Value { get; set; }
        public UpdateAction updateAction { get; set; }
    }
    public enum UpdateAction
    {
        set,
        push,
        pull
    }
}
