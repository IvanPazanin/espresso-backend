﻿// SkipParseConfiguration.cs
//
// © 2021 Espresso News. All rights reserved.

using System.Collections.Generic;
using Espresso.Domain.Infrastructure;

namespace Espresso.Domain.ValueObjects.RssFeedValueObjects
{
    public class SkipParseConfiguration : ValueObject
    {
        public int? NumberOfSkips { get; private set; }

        public int? CurrentSkip { get; private set; }

        /// <summary>
        /// ORM COnstructor.
        /// </summary>
        private SkipParseConfiguration() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkipParseConfiguration"/> class.
        /// </summary>
        /// <param name="numberOfSkips"></param>
        /// <param name="currentSkip"></param>
        public SkipParseConfiguration(int? numberOfSkips, int? currentSkip)
        {
            NumberOfSkips = numberOfSkips;
            CurrentSkip = currentSkip;
        }

        public bool ShouldParse()
        {
            if (NumberOfSkips is null && CurrentSkip is null)
            {
                return true;
            }

            CurrentSkip = CurrentSkip == NumberOfSkips ? 0 : CurrentSkip + 1;

            return CurrentSkip == 1;
        }

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return NumberOfSkips;
            yield return CurrentSkip;
        }
    }
}
