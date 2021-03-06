﻿using System;

namespace FPVRemote.valueChanger
{

    public interface IValueChanger
    {

        IValueChanger SrcChanger
        {
            get; set;
        }

        /// <summary>
        /// Computes this changer's value, using its source changer recursively.
        /// </summary>
        int ComputeValue(ChangerContext cc);

        /// <summary>
        /// Computes a provided value directly
        /// </summary>
        int ComputeValueDirectly(int val, ChangerContext cc);

        /// <summary>
        /// Chains this changer easily. Assigns "this" as next changer's srcChanger.
        /// </summary>
        /// <example>     
        /// </example>
        /// <param name="nextChanger">A Changer that stands in front of this one.</param>
        /// <returns>nextChanger</returns>
        IValueChanger Chain(IValueChanger nextChanger);

    }
}
