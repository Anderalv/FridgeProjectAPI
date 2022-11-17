using System.Threading.Tasks;
using Contracts;
using Entities.Models;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager {
        private RepositoryContext _repositoryContext;
        private IFridgeRepository _fridgeRepository; 
        private IModelRepository _modelRepository;
        private IProductRepository _productRepository;
        private IFridgeProductRepository _fridgeProductRepository;
        
        
        public RepositoryManager(RepositoryContext repositoryContext) {
            _repositoryContext = repositoryContext; }
        
        
        public IFridgeRepository Fridge {
            get
            {
                if(_fridgeRepository == null)
                    _fridgeRepository = new FridgeRepository(_repositoryContext); 
                return _fridgeRepository;
            } }
        
        
        public IModelRepository Model {
            get
            {
                if(_modelRepository == null)
                    _modelRepository = new ModelRepository(_repositoryContext); 
                return _modelRepository;
            } } 
        
        
        public IProductRepository Product {
            get
            {
                if(_productRepository == null)
                    _productRepository = new ProductRepository(_repositoryContext); 
                return _productRepository;
            } }
        

        public IFridgeProductRepository FridgeProduct {
            get
            {
                if(_fridgeProductRepository == null)
                    _fridgeProductRepository = new FridgeProductRepository(_repositoryContext); 
                return _fridgeProductRepository;
            } }
        
        
        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
    }
}