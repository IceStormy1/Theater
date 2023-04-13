using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Theater.Abstractions
{
    /// <summary>
    /// Представляет результат запроса с пагинацией.
    /// </summary>
    /// <typeparam name="T">Тип данных.</typeparam>
    public class PagingResult<T>
    {
        /// <summary>
        /// Найденные записи.
        /// </summary>
        [Required]
        public IReadOnlyCollection<T> Items { get; set; }

        /// <summary>
        /// Общее количество записей.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Представляет пустой результат.
        /// </summary>
        public static PagingResult<T> Empty => new() { Total = 0, Items = Array.Empty<T>() };
    }
}
