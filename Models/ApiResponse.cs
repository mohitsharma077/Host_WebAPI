namespace WebAPI.Models
{
    public class ApiResponse
    {
        public int status_code { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public int total_records { get; set; }
        public object data { get; set; }
        public int inserted_employees { get; set; }
        public int updated_employees { get; set; }
    }
    public class FileUploadDto
    {
        public IFormFile File { get; set; }
    }
}
