// -----------------------------------------------------------------------
// <copyright file="EntityFrameworkConverter.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System;
    using Kitpymes.Core.Entities;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

    /*
       Clase de extensión EntityFrameworkConverter
       Contiene las extensiones para convertir enumeraciones o value object
    */

    /// <summary>
    /// Clase de extensión <c>EntityFrameworkConverter</c>.
    /// Contiene las extensiones para convertir enumeraciones o value object.
    /// </summary>
    /// <remarks>
    /// <para>En esta clase se pueden agregar todas las extensiones para convertir enumeraciones o value object.</para>
    /// </remarks>
    public abstract class EntityFrameworkConverter
    {
        /// <summary>
        /// Convierte una enumeración.
        /// </summary>
        /// <typeparam name="TEnum">Tipo de enumeración.</typeparam>
        /// <typeparam name="TValue">Tipo de valor del id de la enumeración.</typeparam>
        /// <returns>ValueConverter{TEnum, string}.</returns>
        public static ValueConverter<TEnum, string> ToEnumName<TEnum, TValue>()
            where TEnum : EnumerationBase<TEnum, TValue>
            where TValue : IEquatable<TValue>, IComparable<TValue>
        => new ValueConverter<TEnum, string>(
            v => v.Name,
            v => EnumerationBase<TEnum, TValue>.ToEnum(v));

        /// <summary>
        /// Convierte una enumeración.
        /// </summary>
        /// <typeparam name="TEnum">Tipo de enumeración.</typeparam>
        /// <typeparam name="TValue">Tipo de valor del id de la enumeración.</typeparam>
        /// <returns>ValueConverter{TEnum, TValue}.</returns>
        public static ValueConverter<TEnum, TValue> ToEnumValue<TEnum, TValue>()
            where TEnum : EnumerationBase<TEnum, TValue>
            where TValue : IEquatable<TValue>, IComparable<TValue>
        => new ValueConverter<TEnum, TValue>(
            v => v.Value,
            v => EnumerationBase<TEnum, TValue>.ToEnum(v));

        /// <summary>
        /// Convierte una enumeración.
        /// </summary>
        /// <returns>ValueConverter{StatusEnum, string}.</returns>
        public static ValueConverter<StatusEnum, string> ToStatus()
        => new ValueConverter<StatusEnum, string>(
            v => v.Name,
            v => StatusEnum.ToEnum(v));

        /// <summary>
        /// Convierte un value object.
        /// </summary>
        /// <returns>ValueConverter{Email, string}.</returns>
        public static ValueConverter<Email, string> ToEmail<TEnum>()
        => new ValueConverter<Email, string>(
           v => v.Value ?? string.Empty,
           v => Email.Create(v));

        /// <summary>
        /// Convierte un value object.
        /// </summary>
        /// <returns>ValueConverter{Name, string}.</returns>
        public static ValueConverter<Name, string> ToName<TEnum>()
        => new ValueConverter<Name, string>(
           v => v.Value ?? string.Empty,
           v => Name.Create(v));

        /// <summary>
        /// Convierte un value object.
        /// </summary>
        /// <returns>ValueConverter{Subdomain, string}.</returns>
        public static ValueConverter<Subdomain, string> ToSubdomain<TEnum>()
        => new ValueConverter<Subdomain, string>(
            v => v.Value ?? string.Empty,
            v => Subdomain.Create(v));
    }
}
