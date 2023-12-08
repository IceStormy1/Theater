using Theater.Contracts.FileStorage;

namespace Theater.Contracts.UserAccount;

// TODO: для ЛК добавить новую модель для обновления профиля
public class UserParameters : UserBase
{
    /// <summary>
    /// Никнейм пользователя
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Телефон пользователя
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// СНИЛС
    /// </summary>
    public string Snils { get; set; }

    /// <summary>
    /// Мета-данные фотографии пользователя в ЛК 
    /// </summary>
    public StorageFileListItem Photo { get; set; }
}