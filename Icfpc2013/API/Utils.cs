﻿namespace Icfpc2013
{
    using System;
    using System.Text;

    public static class Utils
    {
        #region Public Methods and Operators

        public static string ToString(object obj)
        {
            // http://stackoverflow.com/questions/4023462/how-do-i-automatically-display-all-properties-of-a-class-and-their-values-in-a-s
            var properties = obj.GetType().GetProperties();

            var sb = new StringBuilder();

            foreach (var property in properties)
            {
                sb.AppendLine(property.Name + ": " + property.GetValue(obj, null));
            }

            return sb.ToString();
        }

        #endregion
    }
}