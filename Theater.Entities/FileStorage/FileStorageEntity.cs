using System;

namespace Theater.Entities.FileStorage
{
    public sealed class FileStorageEntity
    {
        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Исходное наименование файла
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Наименование файла в хранилище
        /// </summary>
        public string FileStorageName { get; set; }

        /// <summary>
        /// Идентификатор бакета
        /// </summary>
        public BucketType BucketId { get; set; }

        /// <summary>
        /// Тип файла
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Размер файла
        /// </summary>
        public ulong Size { get; set; }

        /// <summary>
        /// Дата и время загрузки файла
        /// </summary>
        public DateTime UploadAt { get; set; }
    }
}
