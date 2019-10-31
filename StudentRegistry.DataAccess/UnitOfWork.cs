using StudentRegistry.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentRegistry.DataAccess
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly StudenDbContext _context;
        public UnitOfWork(StudenDbContext context)
        {
            _context = context;

        }
        private IGenericRepository<Student> studentRepository;
       
        private IGenericRepository<Gender> genderRepository;
       
        public IGenericRepository<Student> StudentRepository
        {
            get
            {

                if (this.studentRepository == null)
                {
                    this.studentRepository = new GenericRepository<Student>(_context);
                }
                return studentRepository;
            }
        }

        public IGenericRepository<Gender> GenderRepository
        {
            get
            {

                if (this.genderRepository == null)
                {
                    this.genderRepository = new GenericRepository<Gender>(_context);
                }
                return genderRepository;
            }
        }
      

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
