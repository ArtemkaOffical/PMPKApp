using PMPK.Models;
using System;

namespace PMPK.DAL
{
    public class UnitOfWork : IDisposable
    {
        private PMPKContext _context = new PMPKContext();
        private GenericRepository<Organization> _organizationRepository;
        private GenericRepository<Children> _childrensRepository;
        private GenericRepository<Specialists> _specialistsRepository;
        private GenericRepository<Classes> _classesRepository;
        private GenericRepository<Parent> _parentsRepository;
        private GenericRepository<AdaptiveProgram> _aProgramsRepository;
        private GenericRepository<Passport> _passportsRepository;
        private GenericRepository<Document> _documentsRepository;
        private GenericRepository<Models.PMPK> _pmpksRepository;
        private GenericRepository<FormStudies> _formStudiesRepository;
        private GenericRepository<EdOrg> _edOrgRepository;
        private GenericRepository<PlaceStudy> _placeStudiesRepository;
        private GenericRepository<PlaceOfPMPK> _placeOfPMPKsRepository;

        public GenericRepository<PlaceOfPMPK> PlaceOfPMPKsRepository
        {
            get
            {
                if (this._placeOfPMPKsRepository == null)
                {
                    this._placeOfPMPKsRepository = new GenericRepository<PlaceOfPMPK>(_context);
                }
                return _placeOfPMPKsRepository;
            }
        }
        public GenericRepository<PlaceStudy> PlaceStydiesRepository
        {
            get
            {
                if (this._placeStudiesRepository == null)
                {
                    this._placeStudiesRepository = new GenericRepository<PlaceStudy>(_context);
                }
                return _placeStudiesRepository;
            }
        }
        public GenericRepository<EdOrg> EdOrgsRepository
        {
            get
            {
                if (this._edOrgRepository == null)
                {
                    this._edOrgRepository = new GenericRepository<EdOrg>(_context);
                }
                return _edOrgRepository;
            }
        }
        public GenericRepository<FormStudies> FormStudiesRepository
        {
            get
            {
                if (this._formStudiesRepository == null)
                {
                    this._formStudiesRepository = new GenericRepository<FormStudies>(_context);
                }
                return _formStudiesRepository;
            }
        }
        public GenericRepository<Models.PMPK> PMPKsRepository
        {
            get
            {
                if (this._pmpksRepository == null)
                {
                    this._pmpksRepository = new GenericRepository<Models.PMPK>(_context);
                }
                return _pmpksRepository;
            }
        }
        public GenericRepository<Document> DocumentsRepository
        {
            get
            {
                if (this._documentsRepository == null)
                {
                    this._documentsRepository = new GenericRepository<Document>(_context);
                }
                return _documentsRepository;
            }
        }
        public GenericRepository<Passport> PassportRepository
        {
            get
            {
                if (this._passportsRepository == null)
                {
                    this._passportsRepository = new GenericRepository<Passport>(_context);
                }
                return _passportsRepository;
            }
        }
        public GenericRepository<Parent> ParentsRepository
        {
            get
            {
                if (this._parentsRepository == null)
                {
                    this._parentsRepository = new GenericRepository<Parent>(_context);
                }
                return _parentsRepository;
            }
        }
        public GenericRepository<AdaptiveProgram> AProgramsRepository
        {
            get
            {
                if (this._aProgramsRepository == null)
                {
                    this._aProgramsRepository = new GenericRepository<AdaptiveProgram>(_context);
                }
                return _aProgramsRepository;
            }
        }
        public GenericRepository<Organization> OrganizationRepository
        {
            get
            {
                if (this._organizationRepository == null)
                {
                    this._organizationRepository = new GenericRepository<Organization>(_context);
                }
                return _organizationRepository;
            }
        }
        public GenericRepository<Children> ChildrenRepository
        {
            get
            {
                if (this._childrensRepository == null)
                {
                    this._childrensRepository = new GenericRepository<Children>(_context);
                }
                return _childrensRepository;
            }
        }
        public GenericRepository<Specialists> SpecialistsRepository
        {
            get
            {
                if (this._specialistsRepository == null)
                {
                    this._specialistsRepository = new GenericRepository<Specialists>(_context);
                }
                return _specialistsRepository;
            }
        }
        public GenericRepository<Classes> ClassesRepository
        {
            get
            {
                if (this._classesRepository == null)
                {
                    this._classesRepository = new GenericRepository<Classes>(_context);
                }
                return _classesRepository;
            }
        }

        //Сохранить изменения в контексте
        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        //Переобпределение поведения базового метода
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

        //закрыть соединение с контекстом
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
