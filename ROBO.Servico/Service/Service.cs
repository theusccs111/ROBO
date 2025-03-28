using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ROBO.Dominio.Entidades.Base;
using ROBO.Dominio.Resource.Base;
using ROBO.Servico.Interfaces;
using ROBO.Servico.Validations.Base;

namespace ROBO.Servico.Service
{
    public abstract class Service<T, R> where T : EntidadeBase where R : ResourceBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected IHttpContextAccessor HttpContextAccessor { get { return _httpContextAccessor; } }
        private readonly IUnityOfWork _uow;
        protected IUnityOfWork Uow { get { return _uow; } }
        private readonly IMapper _mapper;
        protected IMapper Mapper { get { return _mapper; } }
        private readonly IConfiguration _config;
        protected IConfiguration Config { get { return _config; } }

        private readonly IValidator<T> _validator;
        protected IValidator<T> Validator { get { return _validator; } }
        public Service(IHttpContextAccessor httpContextAccessor, IMapper mapper, IUnityOfWork uow, IConfiguration config, IValidator<T> validator)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _uow = uow;
            _config = config;
            _validator = validator;
        }

        public virtual Resultado<IEnumerable<R>> Get()
        {
            var entities = _uow.Repository<T>().GetAll();
            return new Resultado<IEnumerable<R>>(Mapper.Map<IEnumerable<R>>(entities));
        }

        public virtual Resultado<R> GetById(int Id)
        {
            var entity = _uow.Repository<T>().GetFirst(e => e.Id == Id);
            return new Resultado<R>(Mapper.Map<R>(entity));
        }

        public virtual Resultado<R> Add(R request)
        {
            var entity = Mapper.Map<T>(request);

            var validReturn = Validator.Validate(entity);

            if (!validReturn.IsValid)
                throw new Dominio.Exceptions.ValidationException(validReturn.Errors.ToList());

            _uow.Repository<T>().Create(entity);

            return new Resultado<R>(Mapper.Map<R>(entity));
        }

        public virtual Resultado<R[]> AddMany(R[] request)
        {
            var resultList = new List<R>();

            foreach (var item in request)
            {
                var result = Add(item).Data;
                resultList.Add(result);
            }

            return new Resultado<R[]>(resultList.ToArray());
        }

        public virtual Resultado<R> Update(R request)
        {
            var entity = Mapper.Map<T>(request);

            var validReturn = Validator.Validate(entity);

            if (!validReturn.IsValid)
                throw new Dominio.Exceptions.ValidationException(validReturn.Errors.ToList());

            _uow.Repository<T>().Update(entity);

            return new Resultado<R>(Mapper.Map<R>(entity));

        }

        public virtual Resultado<R[]> UpdateMany(R[] request)
        {
            var resultList = new List<R>();

            foreach (var item in request)
            {
                var result = Update(item).Data;
                resultList.Add(result);
            }

            return new Resultado<R[]>(resultList.ToArray());
        }

        public virtual Resultado<R> Delete(R request)
        {
            var entity = Mapper.Map<T>(request);

            var deleteValidator = new DeleteValidator<T>(_uow.Repository<T>());
            var deleteValidationResult = deleteValidator.Validate(entity);

            if (!deleteValidationResult.IsValid)
                throw new Dominio.Exceptions.ValidationException(deleteValidationResult.Errors.ToList());

            var entityValidator = Validator.Validate(entity);

            if (!entityValidator.IsValid)
                throw new Dominio.Exceptions.ValidationException(entityValidator.Errors.ToList());

            _uow.Repository<T>().Delete(entity);

            return new Resultado<R>(Mapper.Map<R>(entity));
        }

        public virtual Resultado<R[]> DeleteMany(R[] request)
        {
            var resultList = new List<R>();

            foreach (var item in request)
            {
                var result = Delete(item).Data;
                resultList.Add(result);
            }

            return new Resultado<R[]>(resultList.ToArray());
        }

        public virtual void Complete()
        {
            Uow.Complete();
        }
    }
}
