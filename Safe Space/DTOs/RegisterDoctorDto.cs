namespace SafeSpace.DTOs
{
    public class RegisterDoctorDto
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";

        public string Specialization { get; set; } = "";
        public string Bio { get; set; } = "";
    }
}
