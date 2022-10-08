using DataAccess.MongoDb.Abstract;
using Entities.Concrete;
using ServiceLayer.Abstract;
using ServiceLayer.Utilities.Authorization;
using ServiceLayer.Utilities.ExceptionHandling;
using ServiceLayer.Utilities.JWT;
using ServiceLayer.Utilities.MessageBroker.Kafka;
using ServiceLayer.Utilities.MessageBroker.RabbitMQ;
using ServiceLayer.Utilities.Validation;
using System.Net;

namespace ServiceLayer.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly AuthorizationManager _authorizationManager;
        private readonly UserValidationManager _validationManager;
        private readonly Utilities.Authentication.AuthenticationManager _authenticationManager;
        private readonly JwtHelper _jwtHelper;
        private readonly MqPublisherHelper _mqPublisher;
        private readonly KafkaPublisherHelper _kafkaPublisherHelper;


        public UserService(IUserDal userDal, AuthorizationManager authorizationManager, UserValidationManager validationManager, Utilities.Authentication.AuthenticationManager authenticationManager, JwtHelper jwtHelper, MqPublisherHelper mqPublisher, KafkaPublisherHelper kafkaPublisherHelper)
        {
            _userDal = userDal;
            _authorizationManager = authorizationManager;
            _validationManager = validationManager;
            _authenticationManager = authenticationManager;
            _jwtHelper = jwtHelper;
            _mqPublisher = mqPublisher;
            _kafkaPublisherHelper = kafkaPublisherHelper;
        }

        public void Add(User user)
        {
            _validationManager.Validate(user);
            bool result = this.IsUserAlreadyRegistered(user);
            if (result)
                throw new MyException("Email or Username already taken.", HttpStatusCode.InternalServerError, ErrorCodes.EmailOrUsernameAlreadyTaken);

            user.Id = Guid.NewGuid();
            user.Type = "user";
            _userDal.Add(user);
        }

        public void Authorize(Guid id)
        {
            _authorizationManager.Authorize("admin");

            var userToAuth = this.GetById(id);
            if (userToAuth.Type == "admin")
                throw new MyException("User is already admin.", HttpStatusCode.BadRequest, ErrorCodes.UserIsAlreadyAdmin);

            userToAuth.Type = "admin";
            _userDal.Update(userToAuth.Id, userToAuth);
        }

        public bool CheckIfIsAdmin(Guid id)
        {
            if (this.GetById(id).Type == "admin")
                return true;
            return false;
        }

        public void Delete(Guid id)
        {
            //var user = this.GetById(id);
            for (int i = 0; i < 10; i++)
            {
                _mqPublisher.Publish(i+1, "e1", "r1");
                _mqPublisher.Publish(i+1, "e1", "r2");
                _kafkaPublisherHelper.PublishMessage("deleteduserid", i+1);
            }



            _userDal.Delete(id);
        }

        public IList<User> GetAll()
        {
            //_authorizationManager.Authorize("admin");
            return _userDal.GetList();
        }

        public User GetByEmail(string email)
        {
            var result = _userDal.Get(u => u.Email == email);
            if (result == null)
                throw new MyException("User not found", HttpStatusCode.NotFound, ErrorCodes.UserNotFound);
            return result;
        }

        public User GetById(Guid id)
        {
            var result = _userDal.Get(u => u.Id == id);
            if (result == null)
                throw new MyException("User not found", HttpStatusCode.NotFound, ErrorCodes.UserNotFound);
            return result;
        }

        public User GetByUsername(string username)
        {
            return _userDal.Get(u => u.Username == username);
        }

        public bool IsUserAlreadyRegistered(User user)
        {
            var checkForEmail = this.GetByEmail(user.Email);
            var checkForUsername = this.GetByUsername(user.Username);
            if (checkForEmail != null || checkForUsername != null)
                return true;
            return false;
        }

        public string Login(string email, string password)
        {
            var user = this.GetByEmail(email);
            _authenticationManager.AuthenticateUser(password, user);

            var token = _jwtHelper.CreateToken(user.Id, user.Type);
            return token;
        }
    }
}
