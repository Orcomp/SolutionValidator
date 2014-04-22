// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResolver.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SolutionValidator.Infrastructure.DependencyInjection
{
    /// <summary>
    ///     Very simple Inversion of Control interface for global DI resolving.
    ///     NOTE: This interface is DI container independent.
    /// </summary>
    public interface IResolver
    {
        #region Methods
        /// <summary>
        ///     Use this for all service / interface resolving
        /// </summary>
        /// <typeparam name="T">Interface to resolve</typeparam>
        /// <returns>Resolved concrete instance (service)</returns>
        T Resolve<T>();
        #endregion
    }
}