namespace Theater.Policy
{
    internal class RoleModel
    {
        public User User { get; set; }
        //public MedicalWorker MedicalWorker { get; set; }
    }

    internal class User
    {
        /// <summary>
        /// Политики МР
        /// </summary>
        public UserPolices Policies { get; set; }
    }

    internal class UserPolices
    {
        /// <summary>
        /// Поиск пользователя
        /// </summary>
        public string[] UserSearch { get; set; }

        /// <summary>
        /// Просмотр сведений об организации
        /// </summary>
        public string[] UserView { get; set; }

        /// <summary>
        /// Редактирование пользователя
        /// </summary>
        public string[] UserEdit { get; set; }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        public string[] UserDelete { get; set; }
    }
}