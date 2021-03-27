using System;

namespace SvgPuzzleConstraints
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    sealed class SvgConstraintInfoAttribute : Attribute
    {
        public string Name { get; private set; }
        public SvgConstraintInfoAttribute(string name) { Name = name; }
    }
}
