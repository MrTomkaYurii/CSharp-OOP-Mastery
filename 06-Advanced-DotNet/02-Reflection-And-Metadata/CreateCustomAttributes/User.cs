namespace CreateCustomAttributes
{
    /// <summary>
    /// Модель користувача з декларативними валідаційними атрибутами.
    /// </summary>
    public class User
    {
        [Required(ErrorMessage = "Ім'я користувача є обов'язковим")]
        [StringLength(2, 30)]
        public string Name { get; set; } = string.Empty;

        [MinValue(18, ErrorMessage = "Вік повинен бути не менше 18 років")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Email є обов'язковим")]
        public string Email { get; set; } = string.Empty;
    }
}
