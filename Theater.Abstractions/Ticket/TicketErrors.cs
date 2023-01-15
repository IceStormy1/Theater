using Theater.Common;

namespace Theater.Abstractions.Ticket
{
    public class TicketErrors
    {
        /// <summary>
        /// Билет не найден в системе
        /// </summary>
        public static WriteResult NotFound =>
            WriteResult.FromError(ErrorModel.NotFound("ticket/not-found", "Билет не найден в системе"));

        /// <summary>
        /// Билет уже забронирован другим человеком
        /// </summary>
        public static WriteResult AlreadyBooked =>
            WriteResult.FromError(ErrorModel.Default("ticket/already-booked", "Билет уже забронирован другим человеком"));

        /// <summary>
        /// Билет уже забронирован другим человеком
        /// </summary>
        public static WriteResult UpdateConflict =>
            WriteResult.FromError(ErrorModel.Default("ticket/update-conflict", "Произошла ошибка при покупке билета"));
    }
}