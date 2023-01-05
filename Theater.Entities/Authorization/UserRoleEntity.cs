using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Theater.Entities.Authorization
{
    public class UserRoleEntity
    {
        /// <summary>
        /// Идентификатор роли
        /// </summary>
        public ushort Id { get; set; }

        /// <summary>
        /// Наименование роли
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Ссылка на пользователей
        /// </summary>
        [JsonIgnore]
        public List<UserEntity> Users { get; set; }
    }
}
