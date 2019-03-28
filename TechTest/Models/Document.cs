namespace TechTest.Models
{
    /// <summary>
    /// Класс документов
    /// </summary>
    public class Document : Base
    {
        /// <summary>
        /// Цифровая подпись документа
        /// </summary>
        public string Uuid { get; set; }
    }
}