namespace RecipeRating.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; } // ID of the HTTP request that generated the error.

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId); // Helper property to determine whether to show the Request ID in the view.
    }
}