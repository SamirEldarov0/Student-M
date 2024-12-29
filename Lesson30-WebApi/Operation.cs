using System;

namespace Lesson30_WebApi
{
    public interface ISingletonOperation
    {
        public Guid Id { get; set; }

    }
    public interface ITransientOperation
    {
        public Guid Id { get; set; }

    }
    public interface IScopedOperation
    {
        public Guid Id { get; set; }

    }
    public class Operation:ISingletonOperation,ITransientOperation,IScopedOperation
    {
        public Operation()
        {
            Id= Guid.NewGuid();
        }
        public Guid Id { get; set; }
    }
}
