using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IDXWeb.Repositories
{
    public class UnitOfWork : IUnitOfWork {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private IFeedbackRepository _feedbackRepository;
        private INewsRepository _newsRepository;
        private IPressReleaseRepository _presssReleaseRepository;
        private ISppaProfileRepository _sppaProfileRepository;
        private ISppaPengumumanRepository _sppaPengumumanRepository;
        private ISuspensiRepository _suspensiRepository;
        private IUnsuspensiRepository _unsuspensiRepository;
        private IAnnouncementRepository _announcementRepository;
        private IPDFRepository _pdfRepository;
        private bool _disposed;

        public UnitOfWork(string connectionString = "idxwebdata") {
            connectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;

            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public INewsRepository NewsRepository {
            get { return _newsRepository ?? (_newsRepository = new NewsRepository(_transaction)); }
        }

        public IPressReleaseRepository PressReleaseRepository {
            get { return _presssReleaseRepository ?? (_presssReleaseRepository = new PressReleaseRepository(_transaction)); }
        }

        public IFeedbackRepository FeedbackRepository {
            get { return _feedbackRepository ?? (_feedbackRepository = new FeedbackRepository(_transaction)); }
        }

        public ISppaProfileRepository SppaProfileRepository {
            get {
                return _sppaProfileRepository ?? (_sppaProfileRepository = new SppaProfileRepository(_transaction));
            }
        }

        public ISppaPengumumanRepository SppaPengumumanRepository {
            get {
                return _sppaPengumumanRepository ?? (_sppaPengumumanRepository = new SppaPengumumanRepository(_transaction));
            }
        }

        public ISuspensiRepository SuspensiRepository
        {
            get { return _suspensiRepository ?? (_suspensiRepository = new SuspensiRepository(_transaction)); }
        }

        public IUnsuspensiRepository UnsuspensiRepository
        {
            get { return _unsuspensiRepository ?? (_unsuspensiRepository = new UnsuspensiRepository(_transaction)); }
        }

        public IAnnouncementRepository AnnouncementRepository
        {
            get { return _announcementRepository ?? (_announcementRepository = new AnnouncementRepository(_transaction)); }
        }

        public IPDFRepository PDFRepository
        {
            get { return _pdfRepository ?? (_pdfRepository = new PDFRepository(_transaction)); }
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                resetRepositories();
            }
        }

        private void resetRepositories()
        {
            _newsRepository = null;
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}