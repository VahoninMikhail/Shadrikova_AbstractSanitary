using System;

namespace AbstractSanitaryService.Attributies
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class CustomInterfaceAttribute : Attribute
    {
        public CustomInterfaceAttribute(string descript)
        {
            Description = string.Format("Описание интерфейса: ", descript);
        }

        public string Description { get; private set; }
    }
}
