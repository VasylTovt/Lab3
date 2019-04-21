
namespace CourseWork.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public override string ToString()
        {
            return string.Format("Email: {0}, Role: {1}", Email, Role);
        }
    }
}
