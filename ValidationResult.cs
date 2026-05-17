namespace FileManager
{
    public class ValidationResult
    {
        public bool IsValid { get; private set; }
        public bool IsConflict { get; private set; }
        public string Message { get; private set; }

        public static ValidationResult Success() => new ValidationResult { IsValid = true };
        public static ValidationResult Error(string message) => new ValidationResult { IsValid = false, Message = message };
        public static ValidationResult Conflict(string message) => new ValidationResult { IsValid = false, IsConflict = true, Message = message };
    }
}