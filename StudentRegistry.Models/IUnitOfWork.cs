using System;
using System.Collections.Generic;
using System.Text;

namespace StudentRegistry.Models
{
    public interface IUnitOfWork
    {
        IGenericRepository<Student> StudentRepository { get; }
        IGenericRepository<Gender> GenderRepository { get; }
        void Save();
    }
}
