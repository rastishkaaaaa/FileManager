using System.IO;
using System.Linq;

namespace FileManager
{
    public static class FileNameValidator
    {
        private static readonly char[] InvalidChars = Path.GetInvalidFileNameChars();

        public static ValidationResult Validate(string fileName, string currentDirectory)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return ValidationResult.Error("Имя файла не может быть пустым!");

            if (fileName.IndexOfAny(InvalidChars) >= 0)
                return ValidationResult.Error("Имя файла содержит недопустимые символы!");

            if (currentDirectory != null && File.Exists(Path.Combine(currentDirectory, fileName)))
                return ValidationResult.Conflict($"\"{fileName}\" уже существует. Хотите заменить его?");

            return ValidationResult.Success();
        }

        public static string RemoveInvalidCharacters(string fileName)
        {
            return new string(fileName.Where(c => !InvalidChars.Contains(c)).ToArray());
        }
    }
}