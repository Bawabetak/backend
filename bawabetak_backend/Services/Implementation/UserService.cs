namespace bawabetak_backend.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ISendVerificationFactory _sendVerificationFactory;
        private readonly ICheckVerificationFactory _checkVerificationFactory;
        private readonly ILoginFactory _loginFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICacheHelper _cacheHelper;
        private readonly IRefreshTokenFactory _refreshTokenFactory;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ICurrentUserHelper _currentUserHelper;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ISendVerificationFactory sendVerificationFactory,
            ICheckVerificationFactory checkVerificationFactory,
            ILoginFactory loginFactory,
            IHttpContextAccessor httpContextAccessor,
            ICacheHelper cacheHelper,
            IRefreshTokenFactory refreshTokenFactory,
            IFileService fileService,
            IMapper mapper,
            IMediator mediator,
            ICurrentUserHelper currentUserHelper
            )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _sendVerificationFactory = sendVerificationFactory;
            _checkVerificationFactory = checkVerificationFactory;
            _loginFactory = loginFactory;
            _httpContextAccessor = httpContextAccessor;
            _cacheHelper = cacheHelper;
            _refreshTokenFactory = refreshTokenFactory;
            _fileService = fileService;
            _mapper = mapper;
            _mediator = mediator;
            _currentUserHelper = currentUserHelper;
        }

        public async Task SendVerificationCodeAsync(SendVerificationDto dto)
        {
            var strategy = _sendVerificationFactory.GetStrategy(dto.Type);
            await strategy.SendVerficationCode(dto.Email);
        }

        public async Task VerifyCodeAsync(VerifyCodeDto dto)
        {
            var strategy = _checkVerificationFactory.GetStrategy(dto.Type);
            await strategy.VerifyCodeAsync(dto.Email, dto.Code);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            var user= await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user != null&&user.IsEmailVerified)
            {
                throw new BadRequestCustomException(ResponseKeys.EmailAlreadyExists);
            }
            if(user!=null&&!user.IsEmailVerified)
            {
                await _userRepository.DeleteUserAsync(user);
                
            }
            user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email,
            };
            var result = await _userRepository.CreateUserAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return result;
            }
            var roleResult = await _roleRepository.AddUserToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
            {
                throw new BadRequestCustomException(ResponseKeys.RoleAssignmentFailed);
            }

            if (result.Succeeded)
            {
                   await _mediator.Publish(new UserRegisteredEvent(user.Email));
            }

            
            return result;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            string clientType = "Web"; 

            if (httpContext != null && httpContext.Request.Headers.TryGetValue("X-Client-Type", out var headerValue))
            {
                clientType = headerValue.ToString();
            }

            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null) throw new NotFoundCustomException(ResponseKeys.InvalidEmailOrPassword);

            var isPasswordValid = await _userRepository.IsSamePasswordAsync(user, dto.Password);
            if (!isPasswordValid) throw new NotFoundCustomException(ResponseKeys.InvalidEmailOrPassword);

            var roles = await _roleRepository.GetRolesByUserAsync(user);

            var strategy = _loginFactory.GetStrategy(clientType);
            var result= await strategy.Login(user, roles);
            if(!user.IsCompleteRegistration||!user.IsEmailVerified||!user.IsApproved)
            {
                result.AccessToken=string.Empty;
                result.RefreshToken = string.Empty;
            }
            return result;
        }

        public async Task ForgetPasswordAsync(ForgetPasswordDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
            {
                throw new NotFoundCustomException(ResponseKeys.UserNotFound);
            }
            var approvalKey = $"ResetApproved_{dto.Email}";
            var IsApproved = await _cacheHelper.GetAsync<bool>(approvalKey);
            if (!IsApproved)
            {
                throw new BadRequestCustomException(ResponseKeys.ResetNotApproved);
            }
            await _userRepository.ResetPasswordAsync(user, dto.Password);
            await _cacheHelper.RemoveAsync(approvalKey);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ChangePasswordDto dto)
        {
            var IsSamePassword = dto.NewPassword == dto.OldPassword;
            if (IsSamePassword)
            {
                throw new BadRequestCustomException(ResponseKeys.NewPasswordCannotBeSameAsOld);
            }

            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null) throw new NotFoundCustomException(ResponseKeys.UserNotFound);

            var isOldPasswordValid = await _userRepository.IsSamePasswordAsync(user, dto.OldPassword);
            if (!isOldPasswordValid)
            {
                throw new BadRequestCustomException(ResponseKeys.InvalidOldPassword);
            }
          

            var result = await _userRepository.ChangePassword(user, dto.OldPassword, dto.NewPassword);
            return result;
        }
        public async Task<AuthTokenResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            string clientType = "Web";

            if (httpContext != null && httpContext.Request.Headers.TryGetValue("X-Client-Type", out var headerValue))
            {
                clientType = headerValue.ToString();
            }

            var strategy = _refreshTokenFactory.GetStrategy(clientType);
            return await strategy.RefreshAsync(dto?.RefreshToken);
        }

        

        public async Task CompleteRegister(CompleteRegisterDto dto)
        {
            var isApproved = await _cacheHelper.GetAsync<bool>($"RegisterApproved_{dto.Email}");

            if (!isApproved)
            {
                throw new BadRequestCustomException(ResponseKeys.EmailNotVerified);
            }

            var user = await _userRepository.GetUserByEmailAsync(dto.Email);

            if (user == null)
            {
                throw new NotFoundCustomException(ResponseKeys.UserNotFound);
            }

            if (user.IsCompleteRegistration)
            {
                throw new BadRequestCustomException(ResponseKeys.RegistrationAlreadyCompleted);
            }

            var photoFileName = await _fileService.SaveFileAsync(
                dto.Photo,
                FileCategory.UserPhoto);

            var identityFileName = await _fileService.SaveFileAsync(
                dto.IdentityDocument,
                FileCategory.UserIdentityDocument);

            _mapper.Map(dto, user);

            user.Photo = photoFileName;
            user.IdentityDocument = identityFileName;

            user.IsCompleteRegistration = true;

            await _userRepository.UpdateUserAsync(user);

         

            await _cacheHelper.RemoveAsync($"RegisterApproved_{dto.Email}");
        }

        public async Task<IdentityResult> DeleteMe(string email)
        {
           
            var user =await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundCustomException(ResponseKeys.UserNotFound);
            }
            var result = await _userRepository.DeleteUserAsync(user);
            return result;
        }
    }
}