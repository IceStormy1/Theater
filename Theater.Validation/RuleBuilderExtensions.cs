using System;
using FluentValidation;

namespace Theater.Validation
{
    public static class RuleBuilderExtensions
    {
        /// <summary>
        /// Провалидировать Описание
        /// </summary>
        public static IRuleBuilderOptions<T, string> Description<T>(this IRuleBuilderInitial<T, string> ruleBuilder, string overridenName)
        {
            return ruleBuilder
                .Cascade(CascadeMode.Continue)
                .NotEmpty()
                .MaximumLength(512)
                .MinimumLength(5)
                .WithName(overridenName);
        }

        /// <summary>
        /// Провалидировать идентификатор пользователя
        /// </summary>
        public static IRuleBuilderOptions<T, Guid> ValidateGuid<T>(this IRuleBuilderInitial<T, Guid> ruleBuilder, string overridenMessage)
        {
            return ruleBuilder
                .Cascade(CascadeMode.Continue)
                .NotEqual(Guid.Empty)
                .WithMessage(overridenMessage);
        }
    }
}
