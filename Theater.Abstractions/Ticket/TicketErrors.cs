using Theater.Common;

namespace Theater.Abstractions.Ticket
{
    public static class TicketErrors
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
            WriteResult.FromError(ErrorModel.Default("ticket/already-booked", "Билет уже забронирован"));

        /// <summary>
        /// Произошла ошибка при покупке билета
        /// </summary>
        public static WriteResult BuyTicketConflict =>
            WriteResult.FromError(ErrorModel.Default("ticket/buy-conflict", "Произошла ошибка при покупке билета"));

        /// <summary>
        /// Произошла ошибка при покупке билета
        /// </summary>
        public static WriteResult BookTicketConflict =>
            WriteResult.FromError(ErrorModel.Default("ticket/book-conflict", "Произошла ошибка при покупке билета"));
    }
}