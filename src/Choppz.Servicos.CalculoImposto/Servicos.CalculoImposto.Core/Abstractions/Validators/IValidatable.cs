namespace Servicos.CalculoImposto.Core.Abstractions.Validators
{
    public interface IValidatable
    {
        /// <summary>
        /// Validates the current state of the object to ensure it meets the required conditions.
        /// </summary>
        /// <remarks>This method checks the object's state and throws an exception if any validation rules
        /// are violated. Call this method before performing operations that depend on the object's validity.</remarks>
        void Validate();
    }
}