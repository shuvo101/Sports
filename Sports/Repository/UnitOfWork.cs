using Sports.API.Data;
using Sports.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.API.Repository
{
    public class UnitOfWork : IDisposable
    {
        public ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context1)
        {
            context = context1;
        }

        public UnitOfWork()
        {
        }

        private TestCustomRepository _testCustomRepository;
        public TestCustomRepository TestCustomRepository
        {
            get
            {
                if (this._testCustomRepository == null)
                {
                    this._testCustomRepository = new TestCustomRepository(context);
                }

                return _testCustomRepository;
            }
        }

        private readonly GenericRepository<TestType> _testTypeRepository;
        public GenericRepository<TestType> TestTypeRepository
        {
            get
            {
                return this._testTypeRepository ?? new GenericRepository<TestType>(context);
            }
        }

        private readonly GenericRepository<CoachAthlete> _coachAthleteRepository;
        public GenericRepository<CoachAthlete> CoachAthleteRepository
        {
            get
            {
                return this._coachAthleteRepository ?? new GenericRepository<CoachAthlete>(context);
            }
        }

        private readonly GenericRepository<Test> _testRepository;
        public GenericRepository<Test> TestRepository
        {
            get
            {
                return this._testRepository ?? new GenericRepository<Test>(context);
            }
        }

        private readonly GenericRepository<TestAthlete> _testAthleteRepository;
        public GenericRepository<TestAthlete> TestAthleteRepository
        {
            get
            {
                return this._testAthleteRepository ?? new GenericRepository<TestAthlete>(context);
            }
        }

        private readonly GenericRepository<FitnessRating> _fitnessRatingRepository;
        public GenericRepository<FitnessRating> FitnessRatingRepository
        {
            get
            {
                return this._fitnessRatingRepository ?? new GenericRepository<FitnessRating>(context);
            }
        }

        public void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
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
