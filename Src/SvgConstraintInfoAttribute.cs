using System;

namespace SvgPuzzleConstraints
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    sealed class SvgConstraintInfoAttribute(string name) : Attribute
    {
        public string Name { get; private set; } = name;
    }
}
