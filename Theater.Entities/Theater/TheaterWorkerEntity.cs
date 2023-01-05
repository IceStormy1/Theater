using System;
using System.Collections.Generic;
using Theater.Entities.Authorization;

namespace Theater.Entities.Theater
{
    public class TheaterWorkerEntity
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Имя 
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия 
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public GenderType Gender { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Описание работника
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Идентификатор должности работника театра
        /// </summary>
        public ushort PositionId { get; set; }

        /// <summary>
        /// Ссылка на должность 
        /// </summary>
        public WorkersPositionEntity Position { get; set; }

        public List<RepertoryWorkerEntity> RepertoriesWorkers { get; set; }
    }
}
