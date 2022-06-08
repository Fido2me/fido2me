﻿using System;
using System.Text.Json.Serialization;

namespace Fido2NetLib
{
    [JsonConverter(typeof(ToStringJsonConverter<TypedString>))]
    public class TypedString : IEquatable<TypedString>
    {
        [JsonConstructor]
        protected TypedString(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static implicit operator string(TypedString op) { return op.Value; }

        public override string ToString()
        {
            return Value;
        }

        public bool Equals(TypedString? other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other is null)
                return false;

            //if your below implementation will involve objects of derived classes, then do a 
            //GetType == other.GetType comparison
            if (GetType() != other.GetType())
                return false;

            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as TypedString);
        }

        public static bool operator ==(TypedString e1, TypedString e2)
        {
            if (e1 is null)
                return e2 is null;

            return e1.Equals(e2);
        }

        public static bool operator !=(TypedString e1, TypedString e2)
        {
            return !(e1 == e2);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
